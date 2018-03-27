(function () {
    'use strict';
    var password = angular.module('app.password');

    password.factory('PasswordService', passwordService);

    /* @ngInject */
    function passwordService($http, APP_CONFIG) {

        return {
            change: change
        };

        function change(oldPass, newPass, confirmationPass) {

            var model = {
                old: oldPass,
                new: newPass,
                confirmation: confirmationPass
            };

            return $http.put(APP_CONFIG.API_BASE_URL + '/password', model);
        }
    }
})();