(function () {
    'use strict';
    var daily = angular.module('app.daily', []);

    daily.config(function ($stateProvider) {

        $stateProvider.state({
            name: 'daily',
            url: '/daily',
            templateUrl: 'app/daily/daily.view.html',
            controller: 'DailyController',
            controllerAs: 'vm'
        });
    });
})();