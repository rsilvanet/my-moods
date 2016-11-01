(function () {
    'use strict';
    var logout = angular.module('app.logout');

    logout.factory('LogoutService', logoutService);

    /* @ngInject */
    function logoutService($rootScope, $location) {

        return {
            logout: logout
        };

        function logout() {

            $rootScope.user = null;

            localStorage.removeItem('moodz_analytics_user_id');
            localStorage.removeItem('moodz_analytics_user_email');
            localStorage.removeItem('moodz_analytics_user_name');
            localStorage.removeItem('moodz_analytics_company_id');

            $location.path('/login');
        }
    }
})();