(function () {
    'use strict';
    var answers = angular.module('app.answers');

    answers.controller('AnswersController', answersController);

    answersController.$inject = ['ReviewsService', 'ErrorHandlerService'];

    /* @ngInject */
    function answersController(ReviewsService, ErrorHandlerService) {

        var vm = this;

        activate();

        function activate() {

            vm.filters = {
                formId: null,
                startDate: moment().format('DD/MM/YYYY'),
                endDate: moment().format('DD/MM/YYYY')
            };

            vm.isLoaded = true;
        }

        vm.activeDayCallback = function (date) {
            vm.activeDay = date;
        };

        vm.search = function () {

            var start = moment(vm.filters.startDate, 'DD/MM/YYYY').format('YYYY-MM-DD');
            var end = moment(vm.filters.endDate, 'DD/MM/YYYY').format('YYYY-MM-DD');

            vm.isLoaded = false;

            ReviewsService.getAnswersByMood(vm.filters.formId, start, end)
                .then(function (response) {
                    vm.moods = response.data;
                    vm.isLoaded = true;
                }, function (response) {
                    vm.isLoaded = true;
                    ErrorHandlerService.normalizeAndShow(response);
                });
        };
    }
})();