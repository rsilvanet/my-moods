(function () {
    'use strict';
    var progress = angular.module('app.progress', []);

    progress.config(function ($stateProvider) {

        $stateProvider.state({
            name: 'progress',
            url: '/progress',
            templateUrl: 'app/progress/progress.view.html',
            controller: 'ProgressController',
            controllerAs: 'vm'
        });
    });
})();