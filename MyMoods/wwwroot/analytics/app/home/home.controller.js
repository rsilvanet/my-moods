(function () {
    'use strict';
    var home = angular.module('app.home');

    home.controller('HomeController', homeController);

    homeController.$inject = ['FormsService'];

    /* @ngInject */
    function homeController(FormsService) {

        var vm = this;

        vm.newModel = {
            title: ''
        };

        FormsService.all().then(function (response) {
            vm.forms = response.data;
        });

        vm.createForm = function () {
            FormsService.post(vm.newModel).then(function (response) {
                vm.newModel.id = response.data;
                vm.forms.push(vm.newModel);
                vm.newModel = null;
            });
        };
    }
})();