const { series, parallel, src, dest, watch } = require("gulp");

const
    //GULP DEPENDENCIES
    autoprefixer = require("gulp-autoprefixer"),
    concat = require("gulp-concat"),
    filter = require('gulp-filter'),
    gulpmode = require('gulp-mode'),
    sass = require("gulp-sass")(require("sass")),
    cssmin = require('gulp-clean-css'),
    sourcemaps = require('gulp-sourcemaps'),
    beautify = require('gulp-beautify'),
    uglify = require('gulp-uglify'),
    rename = require('gulp-rename'),
    imagemin = require('gulp-imagemin'),
    gulpif = require('gulp-if'),
    rm = require('gulp-rm'),
    //NODE DEPENDENCIES
    //browserSync = require("browser-sync").create(),
    babelify = require("babelify"),
    browserify = require("browserify"),
    merge = require("merge-stream"),
    path = require('path'),
    source = require('vinyl-source-stream'),
    buffer = require('vinyl-buffer'),
    tsify = require("tsify"),
    xml = require('gulp-xml'),
    argv = require('yargs').argv,
    vendorsconfig = require("./bundleconfig-vendors.json"),
    glob = require('glob'),
    es = require('event-stream'),
    nodeResolve = require('resolve'),
    mode = gulpmode();

let registerVendors = [];

function clean_up() {
    return src(['wwwroot/**/*', "!wwwroot/images/**/*", "!wwwroot/favicon.ico"], { read: false })
        .pipe(rm());
}

const theme = series(theme_scss);

function theme_scss() {
    return src('./Content/theme/site.scss')
        .pipe(mode.development(sourcemaps.init()))
        .pipe(sass().on('error', sass.logError))
        .pipe(mode.development(sourcemaps.write({ includeContent: false })))
        .pipe(mode.development(sourcemaps.init({ loadMaps: true })))
        .pipe(autoprefixer())
        .pipe(mode.production(cssmin()))
        .pipe(mode.development(sourcemaps.write(".")))
        .pipe(mode.production(rename({ suffix: '.min' })))
        .pipe(dest('./wwwroot/content/theme/'));
    //.pipe(browserSync.stream());
}

//IMAGES 
const images = series(images_clean, images_copy);

function images_clean() {
    return src(["wwwroot/images/**/*"], { read: false })
        .pipe(rm());
}

function images_copy() {
    return src('Content/images/**/*.*')
        .pipe(imagemin([
            imagemin.jpegtran({ progressive: true }),
            imagemin.optipng({ optimizationLevel: 5 }),
            imagemin.svgo({ plugins: [{ removeViewBox: true }] }),
        ], {
            verbose: true
        }))
        .pipe(dest('wwwroot/content/images'))
    //.pipe(browserSync.stream());
}

const vendors = series(vendors_js, vendors_modules_js);

function vendors_js() {
    return src('./Content/vendors/**/*')
        .pipe(dest('./wwwroot/content/vendors/'));
}

function vendors_modules_js(done) {
    var tasks = vendorsconfig.map(function (vendor) {
        const b = browserify({ insertGlobals: true, debug: mode.development() });

        registerVendors.push(vendor.expose);
        const filename = path.basename(vendor.file);
        const minified = filename.endsWith('.min.js');
        return src(vendor.file)
            .pipe(gulpif(!minified, mode.production(uglify())))
            .pipe(gulpif(!minified, mode.production(rename({ extname: '.min.js' }))))
            .pipe(dest('./wwwroot/content/vendors/'));
    });

    if (tasks.length == 0) return done();

    merge(tasks);
    done();
}

const js = series(shared_js, areas_js);

function shared_js(done) {
    return glob('./Content/js/**/*.js', function (err, files) {
        if (err) done(err);

        var tasks = files.map(function (entry) {
            const b = browserify({
                entries: [entry],
                debug: mode.development()
            });

            registerVendors.forEach(function (vendor) {
                b.external(vendor);
            });

            return b
                .transform(babelify)
                .bundle()
                .on('error', function (e) {
                    console.log(e.message);
                    this.emit('end');
                })
                .pipe(source(entry))
                .pipe(buffer())
                .pipe(mode.production(uglify()))
                .pipe(rename({ extname: ".bundle.js" }))
                .pipe(mode.production(rename({ suffix: '.min' })))
                .pipe(dest('./wwwroot'));
        });
        es.merge(tasks).on('end', done);
    });
}

function areas_js(done) {
    return glob('./Areas/**/*.js', function (err, files) {
        if (err) done(err);

        var tasks = files.map(function (entry) {
            const b = browserify({
                entries: [entry],
                debug: mode.development()
            });

            registerVendors.forEach(function (vendor) {
                b.external(vendor);
            });

            return b
                .transform(babelify)
                .bundle()
                .on('error', function (e) {
                    console.log(e.message);
                    this.emit('end');
                })
                .pipe(source(entry))
                .pipe(buffer())
                .pipe(mode.production(uglify()))
                .pipe(rename(
                    function (file) {
                        const parts = file.dirname.split(path.sep);
                        const newParts = parts.filter(part => part !== 'Content' && part !== 'js');
                        const newDirname = newParts.join(path.sep);

                        file.dirname = newDirname;
                        file.extname = ".bundle.js";
                    }))
                .pipe(mode.production(rename({ suffix: '.min' })))
                .pipe(dest('./wwwroot/content/js'));
        });
        es.merge(tasks).on('end', done);
    });
}

const ts = series(shared_ts, areas_ts);

function shared_ts(done) {
    return glob('./Content/ts/**/*.ts', function (err, files) {
        if (err) done(err);

        var tasks = files.map(function (entry) {
            const b = browserify({
                entries: [entry],
                debug: mode.development()
            });

            registerVendors.forEach(function (vendor) {
                b.external(vendor);
            });

            return b
                .plugin(tsify, { noImplicitAny: true })
                .transform(babelify)
                .bundle()
                .on('error', function (e) {
                    console.log(e.message);
                    this.emit('end');
                })
                .pipe(source(entry))
                .pipe(buffer())
                .pipe(mode.production(uglify()))
                .pipe(rename(
                    function (file) {
                        file.dirname = file.dirname.replace(/ts/, 'js');
                        file.extname = ".bundle.js";
                    }))
                .pipe(mode.production(rename({ suffix: '.min' })))
                .pipe(dest('./wwwroot'));
        });
        es.merge(tasks).on('end', done);
    })
}

function areas_ts(done) {
    return glob('./Areas/**/*.ts', function (err, files) {
        if (err) done(err);

        var tasks = files.map(function (entry) {
            const b = browserify({
                entries: [entry],
                debug: mode.development()
            });

            registerVendors.forEach(function (vendor) {
                b.external(vendor);
            });

            return b
                .plugin(tsify, { noImplicitAny: true })
                .transform(babelify)
                .bundle()
                .on('error', function (e) {
                    console.log(e.message);
                    this.emit('end');
                })
                .pipe(source(entry))
                .pipe(buffer())
                .pipe(mode.production(uglify()))
                .pipe(rename(
                    function (file) {
                        const parts = file.dirname.split(path.sep);
                        const newParts = parts.filter(part => part !== 'Content' && part !== 'ts');
                        const newDirname = newParts.join(path.sep);

                        file.dirname = newDirname;
                        file.extname = ".bundle.js";
                    }))
                .pipe(mode.production(rename({ suffix: '.min' })))
                .pipe(dest('./wwwroot/content/js'));
        });
        es.merge(tasks).on('end', done);
    })
}

function settings() {
    return src(`deployment/${mode.development() ? 'development' : 'production'}/*.*`)
        .pipe(dest("."));
}

const keep_watching = series(function (done) {
    if (mode.development()) {
        //browserSync.init({
        //    server: "./",
        //    port: 6060,
        //    open: true,
        //    https: true
        //});

        watch("./deployment/**/*.json", series(settings));
        watch("./Content/theme/**/*.*", series(theme));
        watch(["./Content/js/**/*.js", './Areas/**/*.js'], series(js));
        watch(["./Content/ts/**/*.ts", './Areas/**/*.ts'], series(ts));
        watch(["./Content/vendors/js/**/*", './bundleconfig-vendors.json'], series(vendors));

        //watch("Content/css/*.css", series("css"));
        //watch("**/*.html").on("change", browserSync.reload);
        //watch("**/*.asp").on("change", browserSync.reload);
    }
    done();
});

const build = series(
    clean_up,
    vendors,
    parallel(
        series(theme, ts, js),
        settings,
        images_copy
    )
)

const dev = series(build, keep_watching);

exports.dev = dev;
exports.default = build;
exports.images = images;
exports.prueba = series(clean_up, shared_ts);
