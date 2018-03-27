(function () {
    'use strict';
    var forms = angular.module('app.forms', []);

    forms.config(function ($stateProvider) {

        $stateProvider.state({
            name: 'forms-list',
            url: '/forms',
            templateUrl: 'app/forms/forms-list.view.html',
            controller: 'FormsListController',
            controllerAs: 'vm'
        });

        $stateProvider.state({
            name: 'forms-new',
            url: '/forms/new',
            templateUrl: 'app/forms/forms-new.view.html',
            controller: 'FormsNewController',
            controllerAs: 'vm'
        });

        $stateProvider.state({
            name: 'forms-edit',
            url: '/forms/:id',
            templateUrl: 'app/forms/forms-edit.view.html',
            controller: 'FormsEditController',
            controllerAs: 'vm'
        });
    });
})();