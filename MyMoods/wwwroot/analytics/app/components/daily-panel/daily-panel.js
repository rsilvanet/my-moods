(function () {
    'use strict';
    var daily = angular.module('app.components.daily-panel', []);

    /* @ngInject */
    daily.directive('dailyPanel', function ($timeout, ReviewsService) {
        return {
            restrict: 'E',
            templateUrl: 'app/components/daily-panel/daily-panel.html',
            scope: {
                day: '=',
                formId: '='
            },
            link: function (scope) {

                scope.isLoaded = false;

                scope.activateItem = function (item) {
                    scope.activeItem = item;
                };

                scope.$watch('day', function (day) {

                    scope.day = null;
                    scope.formattedDay = null;
                    scope.items = null;
                    scope.activeItem = null;
                    scope.isLoaded = false;

                    if (day) {

                        scope.day = day;
                        scope.formattedDay = moment(day).format('DD/MM/YYYY');

                        $timeout(function () {
                            ReviewsService.getDaily(scope.formId, moment(day).format('YYYY-MM-DD'))
                                .then(function (response) {
                                    scope.items = response.data;
                                    scope.activeItem = scope.items[0];
                                    scope.isLoaded = true;
                                });
                        }, 100);
                    }
                });
            }
        };
    });
})();