(function () {
    'use strict';
    var selector = angular.module('app.components.form-selector', []);

    /* @ngInject */
    selector.directive('formSelector', function (FormsService) {
        return {
            restrict: 'E',
            templateUrl: 'app/components/form-selector/form-selector.html',
            scope: {
                selectCallback: '&'
            },
            link: function (scope) {

                scope.isLoaded = false;

                FormsService.all().then(function (response) {
                    scope.forms = response.data;
                    scope.isLoaded = true;
                });

                scope.doCallback = function () {
                    if (scope.selected) {
                        scope.selectCallback({ id: scope.selected });
                    }
                };
            }
        };
    });
})();