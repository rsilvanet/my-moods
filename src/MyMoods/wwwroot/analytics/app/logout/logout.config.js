(function () {
    'use strict';
    var logout = angular.module('app.logout', []);

    logout.config(function ($stateProvider) {

        $stateProvider.state({
            name: 'logout',
            url: '/logout',
            controller: 'LogoutController'
        });
    });
})();