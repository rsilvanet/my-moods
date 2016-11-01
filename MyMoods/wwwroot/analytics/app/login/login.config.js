(function () {
    'use strict';
    var login = angular.module('app.login', []);

    login.config(function ($stateProvider) {

        $stateProvider.state({
            name: 'login',
            url: '/login',
            templateUrl: 'app/login/login.view.html',
            controller: 'LoginController',
            controllerAs: 'vm'
        });
    });
})();