(function () {
    'use strict';
    var components = angular.module('app.components');

    /* @ngInject */
    components.directive('formSelector', function (FormsService) {
        return {
            restrict: 'E',
            templateUrl: 'app/components/form-selector/form-selector.html',
            scope: {
                formId: '=',
                showWithTags: '=?',
                showWithoutTags: '=?'
            },
            link: function (scope) {

                scope.isLoaded = false;

                if (scope.showWithTags == undefined) {
                    scope.showWithTags = true;
                }

                if (scope.showWithoutTags == undefined) {
                    scope.showWithoutTags = true;
                }

                FormsService.all(true)
                    .then(function (response) {

                        scope.forms = response.data;

                        if (!scope.showWithTags) {
                            scope.forms = _.filter(scope.forms, function (item) {
                                return item.useTags == false;
                            });
                        }
                        
                        if (!scope.showWithoutTags) {
                            scope.forms = _.filter(scope.forms, function (item) {
                                return item.useTags == true;
                            });
                        }

                        scope.isLoaded = true;
                    });
            }
        };
    });
})();