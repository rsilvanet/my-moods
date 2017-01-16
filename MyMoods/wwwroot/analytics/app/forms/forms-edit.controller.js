(function () {
    'use strict';
    var forms = angular.module('app.forms');

    forms.controller('FormsEditController', formsEditController);

    formsEditController.$inject = ['$q', '$state', '$stateParams', 'FormsService', 'TagsService', 'ErrorHandlerService'];

    /* @ngInject */
    function formsEditController($q, $state, $stateParams, FormsService, TagsService, ErrorHandlerService) {

        var vm = this;

        activate();

        function activate() {

            var tagsPromise = TagsService.all(true)
                .then(function (response) {
                    vm.tags = response.data;
                });

            var formPromise = FormsService.getById($stateParams.id)
                .then(function (response) {
                    vm.model = response.data;
                }, function (response) {
                    ErrorHandlerService.normalizeAndShow(response);
                });

            $q.all([tagsPromise, formPromise]).then(selectTags);
        }

        function selectTags() {

            if (vm.model && vm.tags) {
                vm.tags.forEach(function (tag) {
                    tag.selected = _.some(vm.model.customTags, function (customTag) {
                        return tag.id == customTag.id;
                    });
                });
            }
        }

        vm.save = function () {

            vm.model.customTags = _.map(_.filter(vm.tags, 'selected'), 'id');

            FormsService.put($stateParams.id, vm.model)
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