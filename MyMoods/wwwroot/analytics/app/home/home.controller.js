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
                
                vm.forms.push({
                    id: response.data,
                    title: vm.newModel.title
                });

                vm.newModel.title = '';
            });
        };
    }
})();