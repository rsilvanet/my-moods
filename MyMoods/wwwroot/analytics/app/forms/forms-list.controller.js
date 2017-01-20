(function () {
    'use strict';
    var forms = angular.module('app.forms');

    forms.controller('FormsListController', formsListController);

    formsListController.$inject = ['$state', 'FormsService', 'ErrorHandlerService'];

    /* @ngInject */
    function formsListController($state, FormsService, ErrorHandlerService) {

        var vm = this;

        activate();

        function activate() {

            vm.forms = [];""
            vm.isLoading = true;

            vm.filters = {
                onlyActives: true
            };

            FormsService.all()
                .then(function (response) {
                    vm.isLoading = false;
                    vm.forms = response.data;
                }, function (response) {
                    vm.isLoading = false;
                    ErrorHandlerService.normalizeAndShow(response);
                });;
        }

        vm.new = function () {
            $state.go('forms-new');
        };

        vm.normalizeType = function (type) {
            return FormsService.normalizeType(type);
        };

        vm.filterList = function (tag) {

            var result = true;

            if (vm.filters) {
                if (vm.filters.onlyActives && !tag.active) {
                    result = false;
                }
            }

            return result;
        };

        vm.changeStatus = function (form) {

            if (form.active) {
                FormsService.enable(form.id)
                    .then(function () {
                        form.active = true;
                    }, function (response) {
                        form.active = false;
                        ErrorHandlerService.normalizeAndShow(response);
                    })
            }
            else {
                FormsService.disable(form.id)
                    .then(function () {
                        form.active = false;
                    }, function (response) {
                        form.active = true;
                        ErrorHandlerService.normalizeAndShow(response);
                    })
            }
        };
    }
})();