(function () {
    'use strict';
    var forms = angular.module('app.forms');

    forms.controller('FormsNewController', formsNewController);

    formsNewController.$inject = ['$state', 'FormsService', 'TagsService', 'ErrorHandlerService'];

    /* @ngInject */
    function formsNewController($state, FormsService, TagsService, ErrorHandlerService) {

        var vm = this;

        activate();

        function activate() {

            vm.model = {
                title: '',
                type: 'simple',
                mainQuestion: '',
                customTags: [],
                freeText: {
                    allow: false,
                    require: false,
                    title: ''
                }
            };

            TagsService.all(true)
                .then(function (response) {
                    vm.tags = response.data;
                });
        }

        vm.save = function () {

            vm.model.customTags = _.map(_.filter(vm.tags, 'selected'), 'id');

            FormsService.post(vm.model)
                .then(function (response) {
                    $state.go('forms-list');
                }, function (response) {
                    ErrorHandlerService.normalizeAndShow(response);
                });
        };

        vm.normalizeType = function (type) {
            return FormsService.normalizeType(type);
        };
    }
})();