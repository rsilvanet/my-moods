(function () {
    'use strict';
    var password = angular.module('app.password');

    password.controller('PasswordController', passwordController);

    passwordController.$inject = ['$state', 'PasswordService', 'ErrorHandlerService'];

    /* @ngInject */
    function passwordController($state, PasswordService, ErrorHandlerService) {

        var vm = this;

        vm.changePassword = function () {

            var x = vm.model;

            PasswordService.change(x.old, x.new, x.confirmation)
                .then(function () {
                    toastr.success('Senha alterada.');
                    $state.go('home');
                }, function (response) {
                    ErrorHandlerService.normalizeAndShow(response);
                });;
        };
    }
})();