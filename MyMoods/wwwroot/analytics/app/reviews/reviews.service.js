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
            getMoodsCounter: getMoodsCounter,
            enable: enable,
            disable: disable
        };

        function get(formId, startDate, endDate) {
            return $http.get(APP_CONFIG.API_BASE_URL + '/forms/' + formId + '/reviews?startdate=' + startDate + '&endDate=' + endDate);
        }

        function getResume(formId) {
            return $http.get(APP_CONFIG.API_BASE_URL + '/forms/' + formId + '/reviews/resume');
        }

        function getDaily(formId, date) {
            return $http.get(APP_CONFIG.API_BASE_URL + '/forms/' + formId + '/reviews/daily?date=' + date);
        }

        function getMoodsCounter(formId, startDate, endDate) {
            return $http.get(APP_CONFIG.API_BASE_URL + '/forms/' + formId + '/reviews/counters/moods?startdate=' + startDate + '&endDate=' + endDate);
        }

        function enable(formId, id) {
            return $http.put(APP_CONFIG.API_BASE_URL + '/forms/' + formId + '/reviews/' + id + '/enable');
        }

        function disable(formId, id) {
            return $http.put(APP_CONFIG.API_BASE_URL + '/forms/' + formId + '/reviews/' + id + '/disable');
        }
    }
})();