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
    clean = require('gulp-clean'),
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
    bundleconfig = require("./bundleconfig.json"),
    bundleconfigTs = require("./bundleconfig-ts.json"),
    glob = require('glob'),
    es = require('event-stream'),
    nodeResolve = require('resolve'),
    mode = gulpmode();

const Bundles = function (configFile) {
    const
        regex = {
            css: /\.css$/,
            js: /\.js$/
        },
        _getBundles = (pattern) => configFile.filter((bundle) => pattern.test(bundle.outputFileName)),
        _cssBundles = _getBundles(regex.css),
        _jsBundles = _getBundles(regex.js);

    return {
        css: _cssBundles,
        js: _jsBundles,
    }
};

let registerVendors = [];

function clean_up() {
    return src(['wwwroot/**/*', "!wwwroot/favicon.ico"], { read: false })
        .pipe(clean());
}

function theme_scss() {
    return src('./Content/theme/**/styles.scss')
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
    var tasks = vendorsconfig.js.map(function (config) {
        const b = browserify({ debug: false });

        config.vendors.forEach(function (vendor) {
            b.require(nodeResolve.sync(vendor), { expose: vendor });
            registerVendors.push(vendor);
        });

        return b.bundle()
            .pipe(source(config.outputFileName))
            .pipe(dest('./wwwroot/vendors/js'))
            .pipe(buffer())
            .pipe(mode.production(uglify()))
            .pipe(mode.production(rename({ extname: '.min.js' })))
            .pipe(dest('./wwwroot/vendors/js'));
    });

    if (tasks.length == 0) return done();

    merge(tasks);
    done();
}

const vendors = series(vendors_js);

function js(done) {
    glob('./Content/js/**/*.js', function (err, files) {
        if (err) done(err);

        var tasks = files.map(function (entry) {
            const b = browserify({
                entries: [entry],
                debug: false
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
                .pipe(rename({ extname: '.bundle.js' }))
                .pipe(mode.production(rename({ suffix: '.min' })))
                .pipe(dest('./wwwroot/bundles/js'));
        });
        es.merge(tasks).on('end', done);
    })
}

function ts(done) {
    const buildTypescript = function (file) {

        // map them to our stream function
        var tasks = file.entries.map(function (entry) {
            try {
                process.chdir("./");
            }
            catch (ex) {
                console.log(ex);
            }

            return browserify(
                {
                    basedir: file.baseDir,
                    entries: [entry],
                    debug: mode.development(),
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

    console.log(`[Typescript build - ${mode.development() ? 'Development' : 'Production'}]`);

    var files = bundleconfigTs;

    files.forEach(f => {
        buildTypescript(f);
    });
    done();
}

function settings() {
    console.log(`[${mode.development() ? 'Development' : 'Production'} Settings]`);

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

        watch("Content/theme/**/*.*", series(theme));
        watch(["Content/js/*.js", 'Areas/**/*.js'], series(js));
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
exports.prueba = series(clean_up, theme_scss);
