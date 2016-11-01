(function () {
    'use strict';
    var calendar = angular.module('app.calendar');

    calendar.controller('CalendarController', calendarController);

    calendarController.$inject = ['CalendarService'];

    /* @ngInject */
    function calendarController(CalendarService) {

        CalendarService.get()
            .then(function (response) {
                console.log('Get ok!');
            }, function (response) {
                console.log('Get error!');
            });
    }
})();