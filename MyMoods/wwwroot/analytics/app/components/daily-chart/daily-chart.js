(function () {
    'use strict';
    var daily = angular.module('app.components.daily-chart', []);

    /* @ngInject */
    daily.directive('dailyChart', function ($timeout, ReviewsService) {
        return {
            restrict: 'E',
            templateUrl: 'app/components/daily-chart/daily-chart.html',
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
                                        scope.loadChart();
                                    }
                                }

                                scope.isLoaded = true;
                            });
                    }
                });

                scope.loadChart = function () {

                    scope.chartIsLoaded = false;

                    if (scope.selectedDate) {
                        $timeout(function () {
                            ReviewsService.getDaily(scope.formId, moment(scope.selectedDate).format('YYYY-MM-DD'))
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
                                            scope.colors.push(getColor(item.mood));
                                        });

                                        draw();
                                    }

                                    scope.chartIsLoaded = true;
                                });
                        }, 100);
                    }
                };

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

                function getColor(mood) {
                    switch (mood) {
                        case 'angry':
                            return 'darkblue';
                        case 'unsatisfied':
                            return 'gray';
                        case 'normal':
                            return 'black';
                        case 'happy':
                            return 'purple';
                        case 'loving':
                            return 'darkred';
                        default:
                            return 'black';
                    }
                }
            }
        };
    });
})();