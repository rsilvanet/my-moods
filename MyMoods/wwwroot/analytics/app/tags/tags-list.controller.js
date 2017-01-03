(function () {
    'use strict';
    var tags = angular.module('app.tags');

    tags.controller('TagsListController', tagsListController);

    tagsListController.$inject = ['$state', 'TagsService', 'ErrorHandlerService'];

    /* @ngInject */
    function tagsListController($state, TagsService, ErrorHandlerService) {

        var vm = this;

        activate();

        function activate() {

            vm.tags = [];
            vm.isLoading = true;

            TagsService.all()
                .then(function (response) {
                    vm.isLoading = false;
                    vm.tags = response.data;
                }, function (response) {
                    vm.isLoading = false;
                    ErrorHandlerService.normalizeAndShow(response);
                });;
        }

        vm.new = function () {
            $state.go('tags-new');
        };

        vm.normalizeType = function (type) {
            return TagsService.normalizeType(type);
        };

        vm.changeStatus = function (tag) {

            if (tag.active) {
                TagsService.enable(tag.id)
                    .then(function () {
                        tag.active = true;
                    }, function (response) {
                        tag.active = false;
                        ErrorHandlerService.normalizeAndShow(response);
                    })
            }
            else {
                TagsService.disable(tag.id)
                    .then(function () {
                        tag.active = false;
                    }, function (response) {
                        tag.active = true;
                        ErrorHandlerService.normalizeAndShow(response);
                    })
            }
        };
    }
})();