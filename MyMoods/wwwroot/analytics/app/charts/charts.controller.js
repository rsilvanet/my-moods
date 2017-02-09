(function () {
    'use strict';
    var charts = angular.module('app.charts');

    charts.controller('ChartsController', chartsController);

    chartsController.$inject = [];

    /* @ngInject */
    function chartsController() {

        var vm = this;

        activate();

        function activate() {

            vm.filters = {
                formId: null,
                startDate: moment().format('DD/MM/YYYY'),
                endDate: moment().format('DD/MM/YYYY')
            };
        }
    }
})();