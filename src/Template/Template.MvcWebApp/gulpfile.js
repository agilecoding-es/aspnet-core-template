const { series, parallel, src, dest, watch } = require("gulp");

const
    gulp = require("gulp"),
    autoprefixer = require("gulp-autoprefixer"),
    concat = require("gulp-concat"),
    env = require('gulp-env'),
    filter = require('gulp-filter'),
    gulpif = require('gulp-if'),
    sass = require("gulp-sass")(require("sass")),
    cssmin = require('gulp-cssmin'),
    sourcemaps = require('gulp-sourcemaps'),
    beautify = require('gulp-beautify'),
    uglify = require('gulp-uglify'),
    rename = require('gulp-rename'),
    clean = require('gulp-clean'),
    debug = require('gulp-debug'),
    //browserSync = require("browser-sync").create(),
    browserify = require("browserify"),
    es = require('event-stream'),
    merge = require("merge-stream"),
    path = require('path'),
    source = require('vinyl-source-stream'),
    tsify = require("tsify"),
    xml = require('gulp-xml'),
    argv = require('yargs').argv,
    vendors = require("./bundleconfig-vendors.json"),
    bundleconfig = require("./bundleconfig.json"),
    bundleconfigTs = require("./bundleconfig-ts.json")

let isDevelopment = true;

const Bundles = function () {
    const
        regex = {
            css: /\.css$/,
            js: /\.js$/
        },
        _getBundles = (pattern) => bundleconfig.filter((bundle) => pattern.test(bundle.outputFile)),
        _cssBundles = _getBundles(regex.css),
        _jsBundles = _getBundles(regex.js);

    return {
        css: _cssBundles,
        js: _jsBundles,
    }
},
    bundles = new Bundles();

function transpile(done) {
    // body omitted
    done();
}

function test(done) {
    // body omitted
    done();
}

function set_development(done) {
    argv.env = 'development';
    done();
}

function set_environment(done) {
    if ((argv.env === undefined && process.env.NODE_ENV === undefined)) {
        isDevelopment = false;
    } if (process.env.NODE_ENV == 'production') {
        isDevelopment = false;
    }
    if (argv.env == 'development') {
        isDevelopment = true;
    }
    else {
        isDevelopment = false;
    }
    done()
}

function clean_up() {
    return src(['wwwroot/**/*', "!wwwroot/favicon.ico"], { read: false })
        .pipe(clean());
}


//gulp.task('vendors', ['vendors:css', 'vendors:images', 'vendors:js', 'vendors:fonts']);

//gulp.task('vendors:css', function () {
//    return gulp.src(vendors.css)
//        .pipe(gulp.dest('wwwroot/vendors/css/'));
//});

//gulp.task('vendors:js', function () {
//    return gulp.src(vendors.js)
//        .pipe(gulp.dest('wwwroot/vendors/js/'));
//});

//gulp.task('vendors:images', function () {
//    return gulp.src(vendors.images)
//        .pipe(gulp.dest('wwwroot/vendors/images/'));
//});

//gulp.task('vendors:fonts', function () {
//    return gulp.src(vendors.fonts)
//        .pipe(gulp.dest('wwwroot/vendors/fonts/'));
//});


function compile_sass(done) {
    var tasks = bundles.css.map(function (bundleConfig) {
        return src(bundleConfig.inputFiles, { base: "." })
            .pipe(gulpif(isDevelopment, sourcemaps.init()))
            .pipe(sass())
            .pipe(gulpif(isDevelopment, sourcemaps.write({ includeContent: false })))
            .pipe(gulpif(isDevelopment, sourcemaps.init({ loadMaps: true })))
            .pipe(autoprefixer())
            .pipe(concat(bundleConfig.outputFile))
            .pipe(gulpif(isDevelopment, sourcemaps.write(".")))
            .pipe(dest("."))
            .pipe(gulpif(isDevelopment, beautify.css({ indent_size: 2 })))
            .pipe(gulpif(!isDevelopment, cssmin()))
            .pipe(gulpif(!isDevelopment, rename({ suffix: '.min' })))
            .pipe(dest("."));
        //.pipe(browserSync.stream());
    });

    if (tasks.length == 0) return done();

    merge(tasks);

    done();
}

function fonts() {
    return src('Content/fonts/*.*')
        .pipe(dest('wwwroot/fonts'));
}

function images() {
    return src('Content/images/**/*.*')
        .pipe(dest('wwwroot/images'));
}

function theme_scss() {
    return src(['Content/theme/**/scss/style.scss'])
        .pipe(gulpif(isDevelopment, sourcemaps.init()))
        .pipe(sass())
        .pipe(gulpif(isDevelopment, sourcemaps.write({ includeContent: false })))
        .pipe(gulpif(isDevelopment, sourcemaps.init({ loadMaps: true })))
        .pipe(autoprefixer())
        .pipe(gulpif(isDevelopment, beautify.css({ indent_size: 2 })))
        .pipe(gulpif(!isDevelopment, cssmin()))
        .pipe(gulpif(!isDevelopment, rename({ suffix: '.min' })))
        .pipe(dest('wwwroot/theme/**/css'));
    //.pipe(browserSync.stream());
}

function theme_css() {
    return src('Content/theme/**/css/*.css')
        .pipe(gulpif(isDevelopment, beautify.css({ indent_size: 2 })))
        .pipe(gulpif(!isDevelopment, cssmin()))
        .pipe(gulpif(!isDevelopment, rename({ suffix: '.min' })))
        .pipe(dest('wwwroot/theme/**/css'));
    //.pipe(browserSync.stream());
}

function theme_js() {
    return src(['Content/theme/**/js/*.js'])
        .pipe(dest('wwwroot/theme/**/js'))
        .pipe(uglify())
        .pipe(rename({ suffix: '.min' }))
        .pipe(dest('wwwroot/theme/**/js'));
    //.pipe(browserSync.stream());
}

function theme_images() {
    return src('Content/theme/**/images/**/*')
        .pipe(dest('wwwroot/theme/**/images/'));
    //.pipe(browserSync.stream());
}

const theme = series(theme_scss, theme_css, theme_js, theme_images);

function process_vendors() {
    var dependencies = Object.keys(vendors && vendors.dependencies || {});
    dependencies.forEach(d => {
        return browserify()
            .require(d.group)
            .bundle()
            .pipe(source('vendor.bundle.js'))
            .pipe(gulp.dest(__dirname + '/public/scripts'));
    })
}

function ts(done) {
    const buildTypescript = function (file, development) {

        // map them to our stream function
        var tasks = file.entries.map(function (entry) {
            console.log("File: " + file.baseDir + entry + " output: " + file.outputDir);
            try {
                process.chdir("./");
            }
            catch (ex) {
                console.log(ex);
            }

            console.log(`BaseDir: ${file.baseDir} - Entry: ${entry}`);
            return browserify(
                {
                    basedir: file.baseDir,
                    entries: [entry],
                    debug: development,
                    cache: {},
                    packageCache: {}
                })
                .plugin(tsify, { noImplicitAny: true })
                .bundle()
                .pipe(source(entry))
                .pipe(rename({ extname: '.bundle.js' }))
                .pipe(dest(file.outputDir.toLowerCase()))
                //.pipe(browserSync.stream())
                .on('end', () => console.log(`[${new Date().toLocaleTimeString()}] ${entry}`));
        });

        // create a merged stream
        tasks = [].concat(...tasks);
        return es.merge.apply(null, tasks);
    }

    if (isDevelopment) {
        console.log('[Typescript build - Development]');
    } else {
        console.log('[Typescript build - Production]');
    }
    var files = bundleconfigTs;

    files.forEach(f => {
        buildTypescript(f, isDevelopment);
    });
    done();
}

function js(done) {
    var tasks = bundles.js.map(function (bundleConfig) {
        return src(bundleConfig.inputFiles, { base: "." })
            .pipe(gulpif(isDevelopment, sourcemaps.init()))
            .pipe(concat(bundleConfig.outputFile))
            .pipe(dest("."))
            .pipe(gulpif(isDevelopment, beautify.js({ indent_size: 2 })))
            .pipe(gulpif(!isDevelopment, uglify()))
            .pipe(gulpif(!isDevelopment, rename({ suffix: '.min' })))
            .pipe(dest("."));
    });

    if (tasks.length == 0) return done();

    merge(tasks);

    done();
}

function settings(done) {
    if (isDevelopment) {
        console.log('[Development Settings]');
        return src('deployment/development/*.*')
            .pipe(dest("."));
    } else {
        console.log('[Production Settings]');
        return src('deployment/production/*.*')
            .pipe(dest("."));
    }
    done()
}

const keep_watching = series(function (done) {
    //browserSync.init({
    //    server: "./",
    //    port: 6060,
    //    open: true,
    //    https: true
    //});

    if (isDevelopment) {
        watch("Content/theme/**/*.*", series(theme));
        watch(["Content/scss/**/*.scss", 'Areas/**/*.scss'], series(compile_sass));
        watch(["Content/js/*.js", 'Areas/**/*.js'], series(js));
        //watch("Content/css/*.css", series("css"));
        //watch("**/*.html").on("change", browserSync.reload);
        //watch("**/*.asp").on("change", browserSync.reload);
    }
    done();
});

const build = series(set_environment, clean_up,
    parallel(
        series(compile_sass, theme, ts, js),
        settings
    )
)

const dev = series(set_development, build, keep_watching);

exports.dev = dev;
exports.default = build;