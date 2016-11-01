(function () {
    'use strict';
    var daily = angular.module('app.daily');

    daily.controller('DailyController', dailyController);

    dailyController.$inject = [];

    /* @ngInject */
    function dailyController() {
        
        var vm = this;

        vm.formId = '57976abd266b3c042d6217f6';
        
        vm.activeDayCallback = function (date) {
            vm.activeDay = date;
        };
    }
})();