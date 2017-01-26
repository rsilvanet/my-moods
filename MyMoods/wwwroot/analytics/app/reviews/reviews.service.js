(function () {
    'use strict';
    var reviews = angular.module('app.reviews');

    reviews.factory('ReviewsService', reviewsService);

    /* @ngInject */
    function reviewsService($http, APP_CONFIG) {

        return {
            get: get,
            getResume: getResume,
            getDaily: getDaily,
            enable: enable,
            disable: disable
        };

        function get(formId, date) {
            return $http.get(APP_CONFIG.API_BASE_URL + '/forms/' + formId + '/reviews?date=' + date);
        }

        function getResume(formId) {
            return $http.get(APP_CONFIG.API_BASE_URL + '/forms/' + formId + '/reviews/resume');
        }

        function getDaily(formId, date) {
            return $http.get(APP_CONFIG.API_BASE_URL + '/forms/' + formId + '/reviews/daily?date=' + date);
        }

        function enable(formId, id) {
            return $http.put(APP_CONFIG.API_BASE_URL + '/forms/' + formId + '/reviews/' + id + '/enable');
        }

        function disable(formId, id) {
            return $http.put(APP_CONFIG.API_BASE_URL + '/forms/' + formId + '/reviews/' + id + '/disable');
        }
    }
})();