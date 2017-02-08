(function () {
    'use strict';
    var resume = angular.module('app.resume', []);

    resume.config(function ($stateProvider) {

        $stateProvider.state({
            name: 'resume',
            url: '/resume',
            templateUrl: 'app/resume/resume.view.html',
            controller: 'ResumeController',
            controllerAs: 'vm'
        });
    });
})();