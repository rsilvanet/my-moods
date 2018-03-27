(function () {
    'use strict';
    var tags = angular.module('app.tags');

    tags.controller('TagsNewController', tagsNewController);

    tagsNewController.$inject = ['$state', 'TagsService', 'ErrorHandlerService'];

    /* @ngInject */
    function tagsNewController($state, TagsService, ErrorHandlerService) {

        var vm = this;

        activate();

        function activate() {

            vm.model = {
                title: '',
                type: 'undefined'
            };
        }

        vm.save = function () {

            TagsService.post(vm.model)
                .then(function (response) {
                    $state.go('tags-list');
                }, function (response) {
                    ErrorHandlerService.normalizeAndShow(response);
                });;
        };

        vm.normalizeType = function (type) {
            return TagsService.normalizeType(type);
        };
    }
})();