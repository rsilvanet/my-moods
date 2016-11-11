(function () {
    'use strict';
    var charts = angular.module('app.charts');

    charts.controller('ChartsController', chartsController);

    chartsController.$inject = [];

    /* @ngInject */
    function chartsController() {

        var vm = this;

        vm.formSelectCallback = function (id) {
            vm.formId = id;
            vm.activeDay = null;
        };

        vm.activeDayCallback = function (date) {
            vm.activeDay = date;
        };
    }
})();