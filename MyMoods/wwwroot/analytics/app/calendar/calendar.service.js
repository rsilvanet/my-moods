(function () {
    'use strict';
    var calendar = angular.module('app.calendar');

    calendar.factory('CalendarService', calendarService);

    /* @ngInject */
    function calendarService($http) {

        return {
            get: get
        };

        function get() {
            return $http.get('https://www.google.com.br');
        }
    }
})();