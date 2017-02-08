(function () {
    'use strict';
    var components = angular.module('app.components');

    /* @ngInject */
    components.directive('dailyChart', function ($timeout, ReviewsService, ErrorHandlerService) {
        return {
            restrict: 'E',
            templateUrl: 'app/components/daily-chart/daily-chart.html',
            scope: {
                formId: '=',
                date: '='
            },
            link: function (scope) {

                scope.isLoaded = true;

                scope.search = function () {
                    if (scope.formId && scope.date) {

                        scope.moods = null;
                        scope.counters = null;
                        scope.colors = null;
                        scope.isLoaded = false;

                        ReviewsService.getDaily(scope.formId, moment(scope.date, 'DD/MM/YYYY').format('YYYY-MM-DD'))
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

                                scope.isLoaded = true;

                            }, function (response) {
                                scope.isLoaded = true;
                                ErrorHandlerService.normalizeAndShow(response);
                            });
                    }
                    else if (!scope.formId) {
                        toastr.info('Selecione um formulário.');
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
                            return '#FF5733';
                        case 'unsatisfied':
                            return '#5DADE2';
                        case 'normal':
                            return '#16A085';
                        case 'happy':
                            return '#8E44AD';
                        case 'loving':
                            return '#85929E';
                        default:
                            return 'black';
                    }
                }
            }
        };
    });
})();