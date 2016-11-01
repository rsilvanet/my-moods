(function () {
    'use strict';
    var app = angular.module('app');

    app.factory('HttpInterceptorService', httpInterceptorService);

    /* @ngInject */
    function httpInterceptorService($rootScope, $location, $q, LogoutService) {

        return {
            request: request,
            responseError: responseError
        };

        function request(config) {

            config.headers = config.headers || {};

            if ($rootScope.user && $rootScope.user.companyId) {
                config.headers['X-Company-Id'] = $rootScope.user.companyId;
            }

            return config;
        }

        function responseError(response) {

            if (response.status == 401) {
                LogoutService.logout();
            }

            return $q.reject(response);
        }
    }
})();