(function () {
    'use strict';
    var logout = angular.module('app.logout');

    logout.controller('LogoutController', logoutController);

    logoutController.$inject = ['LogoutService'];

    /* @ngInject */
    function logoutController(LogoutService) {
        LogoutService.logout();
    }
})();