(function () {
    'use strict';
    var components = angular.module('app.components');

    /* @ngInject */
    components.directive('reviewsViewer', function ($timeout, ReviewsService, ErrorHandlerService) {
        return {
            restrict: 'E',
            templateUrl: 'app/components/reviews-viewer/reviews-viewer.html',
            scope: {
                formId: '=',
                date: '='
            },
            link: function (scope) {

                scope.reviews = null;
                scope.isLoaded = true;

                scope.search = function () {

                    if (scope.formId && scope.date) {

                        scope.isLoaded = false;

                        $timeout(function () {
                            ReviewsService.get(scope.formId, moment(scope.date, 'DD/MM/YYYY').format('YYYY-MM-DD'))
                                .then(function (response) {
                                    scope.reviews = response.data;
                                    scope.isLoaded = true;
                                }, function (response) {
                                    scope.isLoaded = true;
                                    ErrorHandlerService.normalizeAndShow(response);
                                });
                        }, 10);
                    }
                    else if (!scope.formId) {
                        toastr.info('Selecione um formulário.');
                    }
                    else if (!scope.date) {
                        toastr.info('Informe uma data válida.');
                    }
                };
            }
        };
    });
})();