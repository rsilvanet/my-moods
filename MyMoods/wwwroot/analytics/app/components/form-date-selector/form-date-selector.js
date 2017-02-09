(function () {
    'use strict';
    var components = angular.module('app.components');

    /* @ngInject */
    components.directive('formDateSelector', function (FormsService) {
        return {
            restrict: 'E',
            templateUrl: 'app/components/form-date-selector/form-date-selector.html',
            scope: {
                formId: '=',
                startDate: '=',
                endDate: '='
            },
            link: function (scope) {

            }
        };
    });
})();