(function () {
    'use strict';
    var reviews = angular.module('app.reviews');

    reviews.factory('ReviewsService', reviewsService);

    /* @ngInject */
    function reviewsService($http, APP_CONFIG) {

        return {
            getResume: getResume,
            getDaily: getDaily
        };

        function getResume(formId) {
            return $http.get(APP_CONFIG.API_BASE_URL + '/forms/' + formId + '/reviews/resume');
        }

        function getDaily(formId, date) {
            return $http.get(APP_CONFIG.API_BASE_URL + '/forms/' + formId + '/reviews/daily?date=' + date);
        }
    }
})();