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
    return src(['wwwroot/**/*', "!wwwroot/favicon.ico"], { read: false })
        .pipe(rm());
}

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
        .pipe(dest('./wwwroot/theme/'));
    //.pipe(browserSync.stream());
}

const theme = series(theme_scss);

function vendors_js(done) {
    return src('./Content/vendors/js/**/*')
        .pipe(rename(
            function (file) {
                //const parts = file.dirname.split(path.sep);
                //const newParts = parts.filter(part => part !== 'Content' && part !== 'js');
                //const newDirname = newParts.join(path.sep);

                //file.dirname = newDirname;
                //file.extname = ".bundle.js";
                console.log(file);
            }))
        .pipe(dest('./wwwroot/vendors/js'));
}


function vendors_modules_js(done) {
    var tasks = vendorsconfig.js.map(function (config) {
        const b = browserify({ debug: mode.development() });

        config.vendors.forEach(function (vendor) {
            b.require(nodeResolve.sync(vendor), { expose: vendor });
            registerVendors.push(vendor);
        });

        return b.bundle()
            .pipe(source(config.outputFileName))
            .pipe(buffer())
            .pipe(mode.production(uglify()))
            .pipe(mode.production(rename({ extname: '.min.js' })))
            .pipe(dest('./wwwroot/vendors/js'));
    });

    if (tasks.length == 0) return done();

    merge(tasks);
    done();
}

const vendors = series(vendors_js, vendors_modules_js);

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
                .pipe(dest('./wwwroot'));
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
                .pipe(dest('./wwwroot'));
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

        watch("./Content/theme/**/*.*", series(theme));
        watch(["./Content/js/**/*.js", './Areas/**/*.js'], series(js));
        //watch("Content/css/*.css", series("css"));
        //watch("**/*.html").on("change", browserSync.reload);
        //watch("**/*.asp").on("change", browserSync.reload);
    }
    done();
});

const build = series(clean_up,
    parallel(
        vendors,
        series(theme, ts, js),
        settings
    )
)

const dev = series(build, keep_watching);

exports.dev = dev;
exports.default = build;
exports.prueba = series(clean_up, ts);
