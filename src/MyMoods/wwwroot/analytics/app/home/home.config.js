(function () {
    'use strict';
    var home = angular.module('app.home', []);

    home.config(function ($stateProvider) {

        $stateProvider.state({
            name: 'home',
            url: '/',
            templateUrl: 'app/home/home.view.html',
            controller: 'HomeController',
            controllerAs: 'vm'
        });
    });
})();