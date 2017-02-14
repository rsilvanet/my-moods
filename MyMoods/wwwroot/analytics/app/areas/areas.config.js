(function () {
    'use strict';
    var areas = angular.module('app.areas', []);

    areas.config(function ($stateProvider) {

        $stateProvider.state({
            name: 'areas',
            url: '/areas',
            templateUrl: 'app/areas/areas.view.html',
            controller: 'AreasController',
            controllerAs: 'vm'
        });
    });
})();