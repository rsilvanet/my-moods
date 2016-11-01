(function () {
    'use strict';
    var reviews = angular.module('app.reviews');

    reviews.factory('ReviewsService', reviewsService);

    /* @ngInject */
    function reviewsService($http, APP_CONFIG) {

        return {
            getTopList: getTopList,
            getAverageList: getAverageList,
            getDaily: getDaily
        };

        function getTopList(formId) {
            return $http.get(APP_CONFIG.API_BASE_URL + '/forms/' + formId + '/reviews/top');
        }

        function getAverageList(formId) {
            return $http.get(APP_CONFIG.API_BASE_URL + '/forms/' + formId + '/reviews/average');
        }

        function getDaily(formId, date) {
            return $http.get(APP_CONFIG.API_BASE_URL + '/forms/' + formId + '/reviews/daily?date=' + date);
        }
    }
})();