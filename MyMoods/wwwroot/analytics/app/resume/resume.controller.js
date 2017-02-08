(function () {
    'use strict';
    var resume = angular.module('app.resume');

    resume.controller('ResumeController', resumeController);

    resumeController.$inject = [];

    /* @ngInject */
    function resumeController() {

        var vm = this;

        vm.activeDayCallback = function (date) {
            vm.activeDay = date;
        };
    }
})();