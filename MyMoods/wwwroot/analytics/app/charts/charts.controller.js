(function () {
    'use strict';
    var charts = angular.module('app.charts');

    charts.controller('ChartsController', chartsController);

    chartsController.$inject = [];

    /* @ngInject */
    function chartsController() {

        var vm = this;

        vm.formId = '57976abd266b3c042d6217f6';

        vm.activeDayCallback = function (date) {
            vm.activeDay = date;
        };
    }
})();