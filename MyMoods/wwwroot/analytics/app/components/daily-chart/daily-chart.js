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
                startDate: '=',
                endDate: '='
            },
            link: function (scope) {

                scope.isLoaded = true;

                scope.search = function () {
                    if (scope.formId && scope.startDate && scope.endDate) {

                        scope.moods = null;
                        scope.counters = null;
                        scope.colors = null;
                        scope.isLoaded = false;

                        var start = moment(scope.startDate, 'DD/MM/YYYY').format('YYYY-MM-DD');
                        var end = moment(scope.endDate, 'DD/MM/YYYY').format('YYYY-MM-DD');

                        ReviewsService.getMoodsCounter(scope.formId, start, end)
                            .then(function (response) {
                                scope.moods = [];
                                scope.counters = [];
                                scope.colors = [];

                                if (response.data && response.data.length) {

                                    response.data.forEach(function (item) {
                                        scope.moods.push(getDescription(item.mood));
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
                            return '#E63434';
                        case 'unsatisfied':
                            return '#FF5733';
                        case 'normal':
                            return '#8E44AD';
                        case 'happy':
                            return '#3470E6';
                        case 'loving':
                            return '#16A085';
                        default:
                            return 'black';
                    }
                }

                function getDescription(mood) {
                    switch (mood) {
                        case 'angry':
                            return 'Irritado';
                        case 'unsatisfied':
                            return 'Insatisfeito';
                        case 'normal':
                            return 'Normal';
                        case 'happy':
                            return 'Feliz';
                        case 'loving':
                            return 'Apaixonado';
                        default:
                            return 'Nenhum';
                    }
                }
            }
        };
    });
})();