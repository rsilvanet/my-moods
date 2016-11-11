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

            localStorage.setItem('my_moods_analytics_user_id', user.id);
            localStorage.setItem('my_moods_analytics_user_email', user.email);
            localStorage.setItem('my_moods_analytics_user_name', user.name);
            localStorage.setItem('my_moods_analytics_company_id', user.companies[0]);

            loadLoggedUserOnApp();
        }

        function loadLoggedUserOnApp() {

            $rootScope.user = null;

            if (localStorage.getItem('my_moods_analytics_user_id')) {
                $rootScope.user = {
                    id: localStorage.getItem('my_moods_analytics_user_id'),
                    email: localStorage.getItem('my_moods_analytics_user_email'),
                    name: localStorage.getItem('my_moods_analytics_user_name'),
                    companyId: localStorage.getItem('my_moods_analytics_company_id')
                };

                //$location.path('/home');
            }
        }
    }
})();