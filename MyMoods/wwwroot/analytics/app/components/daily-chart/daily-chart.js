(function () {
    'use strict';
    var daily = angular.module('app.components.daily-chart', []);

    /* @ngInject */
    daily.directive('dailyChart', function ($timeout, ReviewsService) {
        return {
            restrict: 'E',
            templateUrl: 'app/components/daily-chart/daily-chart.html',
            scope: {
                day: '=',
                formId: '='
            },
            link: function (scope) {

                scope.isLoaded = false;

                scope.$watch('day', function (day) {
                    if (day) {
                        scope.day = day;
                        scope.formattedDay = moment(day).format('DD/MM/YYYY');
                        scope.isLoaded = false;

                        $timeout(function () {
                            ReviewsService.getDaily(scope.formId, moment(day).format('YYYY-MM-DD'))
                                .then(function (response) {

                                    scope.moods = [];
                                    scope.counters = [];
                                    scope.colors = [];

                                    if (response.data && response.data.length) {

                                        response.data.forEach(function (item) {

                                            var mood = '';

                                            if (item.mood == 'angry') {
                                                mood = 'Irritado';
                                            }
                                            else if (item.mood == 'unsatisfied') {
                                                mood = 'Insatisfeito';
                                            }
                                            else if (item.mood == 'normal') {
                                                mood = 'Normal';
                                            }
                                            else if (item.mood == 'happy') {
                                                mood = 'Feliz';
                                            }
                                            else if (item.mood == 'loving') {
                                                mood = 'Apaixonado';
                                            }

                                            scope.moods.push(mood);
                                            scope.counters.push(item.count);
                                            scope.colors.push(getColor(scope.colors.length));
                                        });

                                        draw();
                                    }

                                    scope.isLoaded = true;
                                });
                        }, 100);
                    }
                });

                function draw() {

                    if (scope.chart) {
                        scope.chart.destroy();
                    }

                    scope.chart = new Chart($('#myChart2'), {
                        type: 'doughnut',
                        data: {
                            labels: scope.moods,
                            datasets: [{
                                data: scope.counters,
                                backgroundColor: scope.colors,
                                borderWidth: 1
                            }]
                        },
                        options: {
                            responsive: false,
                            legend: {
                                position: 'bottom'
                            }
                        }
                    });
                }

                function getColor(index) {
                    switch (index) {
                        case 0:
                            return 'purple';
                        case 1:
                            return 'red';
                        case 2:
                            return 'green';
                        case 3:
                            return 'yellow';
                        case 3:
                            return 'blue';
                        default:
                            return 'black';
                    }
                }
            }
        };
    });
})();