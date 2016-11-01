(function () {
    'use strict';
    var calendar = angular.module('app.calendar', []);

    calendar.config(function ($stateProvider) {

        $stateProvider.state({
            name: 'calendar',
            url: '/calendar',
            templateUrl: 'app/calendar/calendar.view.html',
            controller: 'CalendarController'
        });
    });
})();