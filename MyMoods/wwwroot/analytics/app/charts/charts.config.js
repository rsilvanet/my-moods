(function () {
    'use strict';
    var charts = angular.module('app.charts', []);

    charts.config(function ($stateProvider) {

        $stateProvider.state({
            name: 'charts',
            url: '/charts',
            templateUrl: 'app/charts/charts.view.html',
            controller: 'ChartsController',
            controllerAs: 'vm'
        });
    });
})();