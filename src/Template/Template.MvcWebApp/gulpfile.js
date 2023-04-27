const { task } = require("gulp");

const
    gulp = require("gulp"),
    argv = require('yargs').argv,
    path = require('path'),
    tsify = require("tsify"),
    xml = require('gulp-xml'),
    es = require('event-stream'),
    autoprefixer = require("gulp-autoprefixer"),
    concat = require("gulp-concat"),
    env = require('gulp-env'),
    filter = require('gulp-filter'),
    gulpif = require('gulp-if'),
    //browserSync = require("browser-sync").create(),
    sass = require("gulp-sass")(require("sass")),
    cssmin = require('gulp-cssmin'),
    sourcemaps = require('gulp-sourcemaps'),
    beautify = require('gulp-beautify'),
    uglify = require('gulp-uglify'),
    rename = require('gulp-rename'),
    clean = require('gulp-clean'),
    debug = require('gulp-debug'),
    browserify = require("browserify"),
    source = require('vinyl-source-stream'),
    merge = require("merge-stream"),
    bundleconfig = require("./bundleconfig.json"),
    bundleconfigTs = require("./bundleconfig-ts.json");

let isDevelopment = true;

const regex = {
    css: /\.css$/,
    js: /\.js$/
};


const getBundles = function (pattern) {
    return bundleconfig.filter(function (bundle) {
        return pattern.test(bundle.outputFileName);
    });
}

gulp.task('clean', function () {
    return gulp.src(['wwwroot/**/*.*', "!wwwroot", "!wwwroot/fonts/**", "!wwwroot/images/**", "!wwwroot/favicon.ico", "!wwwroot/robots.txt"], { read: false })
        .pipe(clean());
});

gulp.task('vendor', function () {
    return gulp.src('Content/lib/**/*.*')
        .pipe(gulp.dest('wwwroot/lib'));
    //.pipe(browserSync.stream());
});

gulp.task("sass", function (done) {
    var tasks = getBundles(regex.css).map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(gulpif(isDevelopment, sourcemaps.init()))
            .pipe(sass())
            .pipe(gulpif(isDevelopment, sourcemaps.write({ includeContent: false })))
            .pipe(gulpif(isDevelopment, sourcemaps.init({ loadMaps: true })))
            .pipe(autoprefixer())
            .pipe(concat(bundle.outputFileName))
            .pipe(gulpif(isDevelopment, sourcemaps.write(".")))
            .pipe(gulp.dest("."))
            .pipe(gulpif(isDevelopment, beautify.css({ indent_size: 2 })))
            .pipe(gulpif(!isDevelopment, cssmin()))
            .pipe(gulpif(!isDevelopment, rename({ suffix: '.min' })))
            .pipe(gulp.dest("."));
        //.pipe(browserSync.stream());
    });

    if (tasks.length == 0) return done();

    merge(tasks);

    done();
});

gulp.task('fonts', function () {
    return gulp.src('Content/fonts/*.*')
        .pipe(gulp.dest('wwwroot/fonts'));
    //.pipe(browserSync.stream());
});

gulp.task('images', function () {
    return gulp.src('Content/images/**/*.*')
        .pipe(gulp.dest('wwwroot/images'));
    //.pipe(browserSync.stream());
});

gulp.task('theme:scss', function () {
    return gulp.src(['Content/theme/**/scss/style.scss'])
        .pipe(gulpif(isDevelopment, sourcemaps.init()))
        .pipe(sass())
        .pipe(gulpif(isDevelopment, sourcemaps.write({ includeContent: false })))
        .pipe(gulpif(isDevelopment, sourcemaps.init({ loadMaps: true })))
        .pipe(autoprefixer())
        .pipe(gulpif(isDevelopment, beautify.css({ indent_size: 2 })))
        .pipe(gulpif(!isDevelopment, cssmin()))
        .pipe(gulpif(!isDevelopment, rename({ suffix: '.min' })))
        .pipe(gulp.dest('wwwroot/theme/**/css'));
    //.pipe(browserSync.stream());
});

gulp.task('theme:css', function () {
    return gulp.src('Content/theme/**/css/*.css')
        .pipe(gulpif(isDevelopment, beautify.css({ indent_size: 2 })))
        .pipe(gulpif(!isDevelopment, cssmin()))
        .pipe(gulpif(!isDevelopment, rename({ suffix: '.min' })))
        .pipe(gulp.dest('wwwroot/theme/**/css'));
    //.pipe(browserSync.stream());
});

gulp.task('theme:js', function () {
    return gulp.src(['Content/theme/**/js/*.js'])
        .pipe(gulp.dest('wwwroot/theme/**/js'))
        .pipe(uglify())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest('wwwroot/theme/**/js'));
    //.pipe(browserSync.stream());
});

gulp.task('theme:images', function () {
    return gulp.src('Content/theme/**/images/**/*')
        .pipe(gulp.dest('wwwroot/theme/**/images/'));
    //.pipe(browserSync.stream());
});

gulp.task('theme', gulp.series('theme:scss', 'theme:js', 'theme:images'));

gulp.task("ts", function (done) {
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
                .pipe(rename({extname: '.bundle.js'}))
                .pipe(gulp.dest(file.outputDir.toLowerCase()))
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
});

gulp.task("js", function (done) {
    var tasks = getBundles(regex.js).map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(concat(bundle.outputFileName))
            .pipe(gulp.dest("."))
            .pipe(gulpif(isDevelopment, beautify.js({ indent_size: 2 })))
            .pipe(gulpif(!isDevelopment, uglify()))
            .pipe(gulpif(!isDevelopment, rename({ suffix: '.min' })))
            .pipe(gulp.dest("."));
    });

    if (tasks.length == 0) return done();

    merge(tasks);

    done();
});

gulp.task("js-debug", function (done) {
    var tasks = getBundles(regex.js).map(function (bundle) {
        return gulp.src(bundle.inputFiles, { base: "." })
            .pipe(concat(bundle.outputFileName))
            .pipe(gulp.dest("."))
            .pipe(debug({ title: 'dest' }))
            .pipe(uglify())
            .pipe(debug({ title: 'uglify' }))
            .pipe(rename({ suffix: '.min' }))
            .pipe(gulp.dest("."));
    });

    if (tasks.length == 0) return done();

    merge(tasks);

    done();
});

gulp.task('settings', function () {
    if (isDevelopment) {
        console.log('[Development Settings]');
        return gulp.src('deployment/development/*.*')
            .pipe(gulp.dest("."));
    } else {
        console.log('[Production Settings]');
        return gulp.src('deployment/production/*.*')
            .pipe(gulp.dest("."));
    }
});

gulp.task('build', gulp.series(function (done) {
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
}, 'clean', 'vendor', 'sass', 'theme', 'ts', 'js', 'settings'));

gulp.task("default", gulp.series('build', function (done) {
            //browserSync.init({
            //    server: "./",
            //    port: 6060,
            //    open: true,
            //    https: true
            //});

            if (isDevelopment) {
                gulp.watch("Content/lib/**/*.*", gulp.series("vendor"));
                gulp.watch("Content/theme/**/*.*", gulp.series("theme"));
                gulp.watch(["Content/scss/**/*.scss", 'Areas/**/*.scss'], gulp.series("sass"));
                //gulp.watch("Content/css/*.css", gulp.series("css"));
                gulp.watch(["Content/js/*.js", 'Areas/**/*.js'], gulp.series("js"));
                //gulp.watch("**/*.html").on("change", browserSync.reload);
                //gulp.watch("**/*.asp").on("change", browserSync.reload);
            }
            done();
        }));

gulp.task( "dev", gulp.series(function (done) {
        argv.env = 'development';
        done();
    }, 'default'));
