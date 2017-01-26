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

                scope.filters = {
                    onlyActives: true
                };

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

                scope.filterList = function (review) {

                    var result = true;

                    if (scope.filters) {
                        if (scope.filters.onlyActives && !review.active) {
                            result = false;
                        }
                    }

                    return result;
                };

                scope.changeStatus = function (review) {

                    if (review.active) {
                        ReviewsService.enable(scope.formId, review.id)
                            .then(function () {
                                review.active = true;
                            }, function (response) {
                                review.active = false;
                                ErrorHandlerService.normalizeAndShow(response);
                            });
                    }
                    else {
                        ReviewsService.disable(scope.formId, review.id)
                            .then(function () {
                                review.active = false;
                            }, function (response) {
                                review.active = true;
                                ErrorHandlerService.normalizeAndShow(response);
                            });
                    }
                };
            }
        };
    });
})();