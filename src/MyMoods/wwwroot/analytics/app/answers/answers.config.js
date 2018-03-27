(function () {
    'use strict';
    var answers = angular.module('app.answers', []);

    answers.config(function ($stateProvider) {

        $stateProvider.state({
            name: 'answers',
            url: '/answers',
            templateUrl: 'app/answers/answers.view.html',
            controller: 'AnswersController',
            controllerAs: 'vm'
        });
    });
})();