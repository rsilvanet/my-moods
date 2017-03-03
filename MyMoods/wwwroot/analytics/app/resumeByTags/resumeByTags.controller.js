(function () {
    'use strict';
    var resumeByTags = angular.module('app.resumeByTags');

    resumeByTags.controller('ResumeByTagsController', resumeByTagsController);

    resumeByTagsController.$inject = ['ReviewsService', 'ErrorHandlerService'];

    /* @ngInject */
    function resumeByTagsController(ReviewsService, ErrorHandlerService) {

        var vm = this;

        activate();

        function activate() {

            vm.filters = {
                formId: null,
                startDate: moment().format('DD/MM/YYYY'),
                endDate: moment().format('DD/MM/YYYY')
            };
        }

        vm.activeDayCallback = function (date) {
            vm.activeDay = date;
        };

        vm.search = function () {

            var start = moment(vm.filters.startDate, 'DD/MM/YYYY').format('YYYY-MM-DD');
            var end = moment(vm.filters.endDate, 'DD/MM/YYYY').format('YYYY-MM-DD');

            ReviewsService.getTagsCounter(vm.filters.formId, start, end)
                .then(function (response) {
                    vm.tags = response.data;
                    vm.isLoaded = true;
                }, function (response) {
                    vm.isLoaded = true;
                    ErrorHandlerService.normalizeAndShow(response);
                });
        };
    }
})();