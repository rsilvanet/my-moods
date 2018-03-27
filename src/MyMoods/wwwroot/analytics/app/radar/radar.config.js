(function () {
    'use strict';
    var radar = angular.module('app.radar', []);

    radar.config(function ($stateProvider) {

        $stateProvider.state({
            name: 'radar',
            url: '/radar',
            templateUrl: 'app/radar/radar.view.html',
            controller: 'RadarController',
            controllerAs: 'vm'
        });
    });
})();