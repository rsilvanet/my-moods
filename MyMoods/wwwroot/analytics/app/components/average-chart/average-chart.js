(function () {
    'use strict';
    var chart = angular.module('app.components.average-chart', []);

    /* @ngInject */
    chart.directive('averageChart', function ($q, $timeout, ReviewsService) {
        return {
            restrict: 'E',
            templateUrl: 'app/components/average-chart/average-chart.html',
            scope: {
                formId: '=',
                activeDayCallback: '&'
            },
            link: function (scope) {

                scope.isLoaded = false;

                scope.$watch('formId', function () {
                    if (scope.formId) {
                        load();
                    }
                });

                function load() {

                    scope.isLoaded = false;

                    ReviewsService.getResume(scope.formId)
                        .then(function (response) {

                            scope.points = [];
                            scope.averages = [];
                            scope.dates = [];
                            scope.formattedDates = [];

                            if (response.data && response.data.length) {

                                response.data.forEach(function (item) {

                                    var img = new Image();
                                    img.src = item.avg.image;

                                    scope.points.push(img);
                                    scope.averages.push(item.avg.points.toFixed(2));
                                    scope.dates.push(moment(item.date).date());
                                    scope.formattedDates.push(moment(item.date).format('DD/MM'));
                                });

                                draw();
                            }

                            scope.isLoaded = true;
                        });

                    function draw() {

                        scope.chart = new Chart($('#myChart'), {
                            type: 'line',
                            data: {
                                labels: scope.formattedDates,
                                datasets: [{
                                    label: 'Pontos',
                                    data: scope.averages,
                                    borderWidth: 1
                                }]
                            },
                            options: {
                                scales: {
                                    yAxes: [{
                                        ticks: {
                                            min: 0,
                                            max: 6,
                                            stepSize: 0.25
                                        },
                                        display: false
                                    }]
                                },
                                legend: {
                                    display: false
                                },
                                responsive: false,
                                scaleShowVerticalLines: false
                            }
                        });

                        $('#myChart').click(function (e) {
                            var activePoints = scope.chart.getElementsAtEvent(e);
                            var firstPoint = activePoints[0];

                            if (firstPoint !== undefined) {
                                $timeout(function () {
                                    scope.activeDay = scope.dates[firstPoint._index];
                                    scope.activeDayCallback({ date: scope.activeDay });
                                }, 100);
                            }
                        });

                        $('#myChart').mousemove(function (e) {

                            var activePoints = scope.chart.getElementsAtEvent(e);
                            var firstPoint = activePoints[0];

                            if (firstPoint !== undefined) {
                                $('#myChart').css('cursor', 'pointer');
                            }
                            else {
                                $('#myChart').css('cursor', 'default');
                            }
                        });

                        $timeout(function () {
                            scope.activeDayCallback({ date: scope.dates[scope.dates.length - 1] });
                        }, 100);
                    }
                }
            }
        };
    });
})();