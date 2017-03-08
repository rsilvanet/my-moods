(function () {
    'use strict';
    var resumeByDay = angular.module('app.resumeByDay', []);

    resumeByDay.config(function ($stateProvider) {

        $stateProvider.state({
            name: 'resumeByDay',
            url: '/resume/daily',
            templateUrl: 'app/resumeByDay/resumeByDay.view.html',
            controller: 'ResumeByDayController',
            controllerAs: 'vm'
        });
    });
})();