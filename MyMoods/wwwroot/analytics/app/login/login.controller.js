(function () {
    'use strict';
    var login = angular.module('app.login');

    login.controller('LoginController', loginController);

    loginController.$inject = ['LoginService', 'LogoutService'];

    /* @ngInject */
    function loginController(LoginService, LogoutService) {

        var vm = this;

        LogoutService.logout();

        vm.login = function () {
            LoginService.login(vm.email, vm.password)
                .then(function (response) {
                    LoginService.loadUserOnStorage(response.data);
                }, function (response) {
                    alert('Login inválido!');
                });
        };

    }
})();