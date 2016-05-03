/// <binding BeforeBuild='bundle' />
var gulp = require("gulp");

var paths = {
    scripts: "./Scripts/",
    app: "./Scripts/App/",
    styles: "./Styles/"
};

var concat = require("gulp-concat"),
    rename = require("gulp-rename"),
    uglify = require("gulp-uglify"),
    sass = require("gulp-sass");

gulp.task("scriptBundling", function () {
    return gulp.src([
            paths.app + "bootstrap.js",
            paths.app + "controllers/*.js"])
        .pipe(concat("shortenMe.js"))
        .pipe(gulp.dest(paths.scripts))
        .pipe(rename("shortenMe.min.js"))
        .pipe(uglify())
        .pipe(gulp.dest(paths.scripts));
});

gulp.task("styleBundling", function () {
    return gulp.src(
        paths.styles + 'SASS/**/*.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(concat("shortenMe.css"))
        .pipe(gulp.dest(paths.styles));
});

gulp.task("default", ["scriptBundling", "styleBundling"]);