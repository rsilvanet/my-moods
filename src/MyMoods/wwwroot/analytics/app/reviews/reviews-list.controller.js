(function () {
    'use strict';
    var reviews = angular.module('app.reviews');

    reviews.controller('ReviewsListController', reviewsListController);

    reviewsListController.$inject = [];

    /* @ngInject */
    function reviewsListController() {

        var vm = this;

        activate();

        function activate() {

            vm.filters = {
                formId: null,
                startDate: moment().format('DD/MM/YYYY'),
                endDate: moment().format('DD/MM/YYYY')
            };
        }
    }
})();