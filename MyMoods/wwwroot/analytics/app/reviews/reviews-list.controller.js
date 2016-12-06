(function () {
    'use strict';
    var reviews = angular.module('app.reviews');

    reviews.controller('ReviewsListController', reviewsListController);

    reviewsListController.$inject = ['ReviewsService', 'ErrorHandlerService'];

    /* @ngInject */
    function reviewsListController(ReviewsService, ErrorHandlerService) {

        var vm = this;

        vm.formSelectCallback = function (id) {
            vm.formId = id;
        };
    }
})();