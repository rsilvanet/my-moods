(function () {
    'use strict';
    var resumeByDay = angular.module('app.resumeByDay');

    resumeByDay.controller('ResumeByDayController', resumeByDayController);

    resumeByDayController.$inject = [];

    /* @ngInject */
    function resumeByDayController() {

        var vm = this;

        vm.activeDayCallback = function (date) {
            vm.activeDay = date;
        };
    }
})();