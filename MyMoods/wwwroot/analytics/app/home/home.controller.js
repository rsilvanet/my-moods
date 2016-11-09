(function () {
    'use strict';
    var home = angular.module('app.home');

    home.controller('HomeController', homeController);

    homeController.$inject = ['FormsService'];

    /* @ngInject */
    function homeController(FormsService) {

        var vm = this;

        FormsService.all().then(function (response) {
            vm.forms = response.data;
        });

    }
})();