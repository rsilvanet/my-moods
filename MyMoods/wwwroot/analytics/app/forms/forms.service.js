(function () {
    'use strict';
    var forms = angular.module('app.forms');

    forms.factory('FormsService', formsService);

    /* @ngInject */
    function formsService($http, APP_CONFIG) {

        return {
            all: all,
            post: post,
            put: put
        };

        function all() {
            return $http.get(APP_CONFIG.API_BASE_URL + '/forms');
        }

        function post(dto) {
            return $http.post(APP_CONFIG.API_BASE_URL + '/forms', dto);
        }

        function put(id, dto) {
            return $http.put(APP_CONFIG.API_BASE_URL + '/forms/' + id, dto);
        }
    }
})();