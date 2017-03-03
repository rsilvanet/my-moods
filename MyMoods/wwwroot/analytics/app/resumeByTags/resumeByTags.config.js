(function () {
    'use strict';
    var resumeByTags = angular.module('app.resumeByTags', []);

    resumeByTags.config(function ($stateProvider) {

        $stateProvider.state({
            name: 'resumeByTags',
            url: '/resume/tags',
            templateUrl: 'app/resumeByTags/resumeByTags.view.html',
            controller: 'ResumeByTagsController',
            controllerAs: 'vm'
        });
    });
})();