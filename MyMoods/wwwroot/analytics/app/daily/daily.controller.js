(function () {
    'use strict';
    var daily = angular.module('app.daily');

    daily.controller('DailyController', dailyController);

    dailyController.$inject = [];

    /* @ngInject */
    function dailyController() {

        var vm = this;

        vm.activeDayCallback = function (date) {
            vm.activeDay = date;
        };
    }
})();