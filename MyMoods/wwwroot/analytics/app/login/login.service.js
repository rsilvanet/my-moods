(function () {
    'use strict';
    var login = angular.module('app.login');

    login.factory('LoginService', loginService);

    /* @ngInject */
    function loginService($rootScope, $location, $http, APP_CONFIG) {

        return {
            login: login,
            loadUserOnStorage: loadUserOnStorage,
            loadLoggedUserOnApp: loadLoggedUserOnApp
        };

        function login(email, password) {

            var model = {
                email: email,
                password: password
            };

            return $http.post(APP_CONFIG.API_BASE_URL + '/login', model);
        }

        function loadUserOnStorage(user) {

            localStorage.setItem('moodz_analytics_user_id', user.id);
            localStorage.setItem('moodz_analytics_user_email', user.email);
            localStorage.setItem('moodz_analytics_user_name', user.name);
            localStorage.setItem('moodz_analytics_company_id', user.companies[0]);

            loadLoggedUserOnApp();
        }

        function loadLoggedUserOnApp() {

            $rootScope.user = null;

            if (localStorage.getItem('moodz_analytics_user_id')) {
                $rootScope.user = {
                    id: localStorage.getItem('moodz_analytics_user_id'),
                    email: localStorage.getItem('moodz_analytics_user_email'),
                    name: localStorage.getItem('moodz_analytics_user_name'),
                    companyId: localStorage.getItem('moodz_analytics_company_id')
                };

                $location.path('/home');
            }
        }
    }
})();