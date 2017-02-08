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
                startDate: '=',
                endDate: '='
            },
            link: function (scope) {

                scope.reviews = null;
                scope.isLoaded = true;

                scope.filters = {
                    onlyActives: true
                };

                scope.search = function () {
                    if (scope.formId && scope.startDate && scope.endDate) {
                        
                        scope.isLoaded = false;

                        var start = moment(scope.startDate, 'DD/MM/YYYY').format('YYYY-MM-DD');
                        var end = moment(scope.endDate, 'DD/MM/YYYY').format('YYYY-MM-DD');

                        ReviewsService.get(scope.formId, start, end)
                            .then(function (response) {
                                scope.reviews = response.data;
                                scope.isLoaded = true;
                            }, function (response) {
                                scope.isLoaded = true;
                                ErrorHandlerService.normalizeAndShow(response);
                            });
                    }
                    else if (!scope.formId) {
                        toastr.info('Selecione um formulário.');
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