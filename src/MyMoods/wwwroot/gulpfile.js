let gulp = require('gulp');
let usemin = require('gulp-usemin');

gulp.task('analytics', function() {
    return gulp.src('analytics/index.html')
      .pipe(usemin({
        html: [],
        jsAttributes : {
          async : true,
          lorem : 'ipsum',
          seq   : [1, 2, 1]
        },
        node_modules: [ ],
        app:[ ]
      }))
      .pipe(gulp.dest('analytics/dist'));
  });