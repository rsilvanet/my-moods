(function () {
    'use strict';
    var daily = angular.module('app.daily');

    daily.controller('DailyController', dailyController);

    dailyController.$inject = [];

    /* @ngInject */
    function dailyController() {

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