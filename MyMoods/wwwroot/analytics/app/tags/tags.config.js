(function () {
    'use strict';
    var tags = angular.module('app.tags', []);

    tags.config(function ($stateProvider) {

        $stateProvider.state({
            name: 'tags-list',
            url: '/tags',
            templateUrl: 'app/tags/tags-list.view.html',
            controller: 'TagsListController',
            controllerAs: 'vm'
        });

        $stateProvider.state({
            name: 'tags-new',
            url: '/tags/new',
            templateUrl: 'app/tags/tags-new.view.html',
            controller: 'TagsNewController',
            controllerAs: 'vm'
        });
    });
})();