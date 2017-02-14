(function () {
    'use strict';
    var areas = angular.module('app.areas');

    areas.controller('AreasController', areasController);

    areasController.$inject = ['ReviewsService'];

    /* @ngInject */
    function areasController(ReviewsService) {

        var vm = this;

        activate();

        function activate() {

            vm.filters = {
                formId: null,
                startDate: moment().format('DD/MM/YYYY'),
                endDate: moment().format('DD/MM/YYYY')
            };
        }

        vm.go = function () {
            var start = moment(vm.filters.startDate, 'DD/MM/YYYY').format('YYYY-MM-DD');
            var end = moment(vm.filters.endDate, 'DD/MM/YYYY').format('YYYY-MM-DD');

            ReviewsService.getMaslowCounter(vm.filters.formId, start, end)
                .then(function (response) {
                    console.log(response);

                    var labels = [];
                    var values = [];

                    response.data.forEach(function (item) {
                        labels.push(item.area);
                        values.push(item.points.avg);
                    });

                    new Chart($('#myChart'), {
                        type: 'radar',
                        data: {
                            labels: labels,
                            datasets: [
                                {
                                    label: "Label",
                                    backgroundColor: "rgba(179,181,198,0.2)",
                                    borderColor: "#FF6384",
                                    pointBackgroundColor: "#FF6384",
                                    pointBorderColor: "#fff",
                                    pointHoverBackgroundColor: "#fff",
                                    pointHoverBorderColor: "#FF6384",
                                    data: values
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
                });
        };
    }
})();