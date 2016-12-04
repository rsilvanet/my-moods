(function () {
    'use strict';
    var password = angular.module('app.password');

    password.controller('PasswordController', passwordController);

    passwordController.$inject = ['PasswordService'];

    /* @ngInject */
    function passwordController(PasswordService) {

        var vm = this;

        vm.changePassword = function () {

            var x = vm.model;

            PasswordService.change(x.old, x.new, x.confirmation)
                .then(function () {
                    alert('Ok');
                }, function (response) {
                    alert(response.data.old);
                    console.log(response.data);
                });
        };
    }
})();