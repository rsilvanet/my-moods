(function () {
    'use strict';
    var components = angular.module('app.components');

    /* @ngInject */
    components.directive('maslowRadar', function ($timeout, ReviewsService, ErrorHandlerService) {
        return {
            restrict: 'E',
            templateUrl: 'app/components/maslow-radar/maslow-radar.html',
            scope: {
                formId: '=',
                startDate: '=',
                endDate: '='
            },
            link: function (scope) {

                scope.isLoaded = true;

                scope.search = function () {
                    if (scope.formId && scope.startDate && scope.endDate) {

                        scope.labels = null;
                        scope.values = null;
                        scope.isLoaded = false;

                        var start = moment(scope.startDate, 'DD/MM/YYYY').format('YYYY-MM-DD');
                        var end = moment(scope.endDate, 'DD/MM/YYYY').format('YYYY-MM-DD');

                        ReviewsService.getMaslowCounter(scope.formId, start, end)
                            .then(function (response) {

                                scope.labels = [];
                                scope.values = [];

                                if (response.data && response.data.length) {

                                    response.data.forEach(function (item) {
                                        scope.labels.push(getDescription(item.area));
                                        scope.values.push(item.points.avg.toFixed(2));
                                    });
                                }

                                draw();

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

                    scope.chart = new Chart($('#myChart'), {
                        type: 'radar',
                        data: {
                            labels: scope.labels,
                            datasets: [
                                {
                                    label: "Média",
                                    backgroundColor: "rgba(179,181,198,0.2)",
                                    borderColor: "#FF6384",
                                    pointBackgroundColor: "#FF6384",
                                    pointBorderColor: "#fff",
                                    pointHoverBackgroundColor: "#fff",
                                    pointHoverBorderColor: "#FF6384",
                                    data: scope.values
                                }
                            ]
                        },
                        options: {
                            responsive: true,
                            scale: {
                                display: true,
                                ticks: {
                                    beginAtZero: true,
                                    max: 10
                                }
                            },
                            legend: {
                                display: false,
                                labels: {
                                    display: false
                                }
                            }
                        }
                    });
                }

                function getDescription(area) {
                    switch (area) {
                        case 'realization':
                            return 'Realização';
                        case 'esteem':
                            return 'Autoestima';
                        case 'social':
                            return 'Social';
                        case 'safety':
                            return 'Segurança';
                        case 'physiological':
                            return 'Fisiológico';
                        default:
                            return 'Indefinido';
                    }
                }
            }
        };
    });
})();