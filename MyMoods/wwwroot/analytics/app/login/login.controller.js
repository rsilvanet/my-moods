(function () {
    'use strict';
    var login = angular.module('app.login');

    login.controller('LoginController', loginController);

    loginController.$inject = ['LoginService'];

    /* @ngInject */
    function loginController(LoginService) {

        var vm = this;

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