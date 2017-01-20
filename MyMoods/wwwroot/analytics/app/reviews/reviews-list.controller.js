(function () {
    'use strict';
    var reviews = angular.module('app.reviews');

    reviews.controller('ReviewsListController', reviewsListController);

    reviewsListController.$inject = ['ReviewsService', 'ErrorHandlerService'];

    /* @ngInject */
    function reviewsListController(ReviewsService, ErrorHandlerService) {

        var vm = this;

        activate();

        function activate() {

            vm.filters = {
                formId: null,
                date: moment().format('DD/MM/YYYY')
            };
        }

        vm.formSelectCallback = function (id) {
            vm.filters.formId = id;
        };
    }
})();