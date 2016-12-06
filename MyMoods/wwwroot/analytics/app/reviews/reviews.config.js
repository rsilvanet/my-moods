(function () {
    'use strict';
    var reviews = angular.module('app.reviews', []);

    reviews.config(function ($stateProvider) {

        $stateProvider.state({
            name: 'reviews-list',
            url: '/reviews',
            templateUrl: 'app/reviews/reviews-list.view.html',
            controller: 'ReviewsListController',
            controllerAs: 'vm'
        });
    });
})();