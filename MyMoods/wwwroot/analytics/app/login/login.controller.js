(function () {
    'use strict';
    var login = angular.module('app.login');

    login.controller('LoginController', loginController);

    loginController.$inject = ['LoginService', 'LogoutService', '$state'];

    /* @ngInject */
    function loginController(LoginService, LogoutService, $state) {

        var vm = this;

        LogoutService.logout();

        vm.login = function () {
            LoginService.login(vm.email, vm.password)
                .then(function (response) {
                    LoginService.loadUserOnStorage(response.data);
                    $state.go('home');
                }, function (response) {
                    alert('Login inválido!');
                });
        };

    }
})();