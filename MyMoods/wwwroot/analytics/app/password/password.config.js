(function () {
    'use strict';
    var password = angular.module('app.password', []);

    password.config(function ($stateProvider) {

        $stateProvider.state({
            name: 'password',
            url: '/password',
            templateUrl: 'app/password/password.view.html',
            controller: 'PasswordController',
            controllerAs: 'vm'
        });
    });
})();