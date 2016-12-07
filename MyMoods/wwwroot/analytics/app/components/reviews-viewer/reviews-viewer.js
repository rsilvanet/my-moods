(function () {
    'use strict';
    var components = angular.module('app.components');

    /* @ngInject */
    components.directive('reviewsViewer', function ($timeout, ReviewsService) {
        return {
            restrict: 'E',
            templateUrl: 'app/components/reviews-viewer/reviews-viewer.html',
            scope: {
                formId: '='
            },
            link: function (scope) {

                scope.dates = [];
                scope.formId = null;
                scope.isLoaded = false;

                scope.$watch('formId', function (formId) {
                    scope.dates = [];
                    scope.formId = null;
                    scope.isLoaded = false;

                    if (formId) {
                        scope.formId = formId;

                        ReviewsService.getResume(scope.formId)
                            .then(function (response) {
                                if (response.data) {
                                    response.data.forEach(function (item) {
                                        scope.dates.push({
                                            value: item.date,
                                            formatted: moment(item.date).format('DD/MM/YYYY')
                                        });
                                    });

                                    scope.dates = _.orderBy(scope.dates, 'value', 'desc');

                                    if (scope.dates && scope.dates.length) {
                                        scope.selectedDate = scope.dates[0].value;
                                        scope.loadList();
                                    }
                                }

                                scope.isLoaded = true;
                            });
                    }
                });

                scope.loadList = function () {

                    scope.listIsLoaded = false;

                    if (scope.selectedDate) {
                        $timeout(function () {
                            ReviewsService.get(scope.formId, moment(scope.selectedDate).format('YYYY-MM-DD'))
                                .then(function (response) {
                                    scope.reviews = response.data;
                                    scope.listIsLoaded = true;
                                });
                        }, 100);
                    }
                };
            }
        };
    });
})();