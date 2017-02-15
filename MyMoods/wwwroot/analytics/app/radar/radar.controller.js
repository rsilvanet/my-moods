(function () {
    'use strict';
    var radar = angular.module('app.radar');

    radar.controller('RadarController', radarController);

    radar.$inject = [];

    /* @ngInject */
    function radarController() {

        var vm = this;

        activate();

        function activate() {

            vm.filters = {
                formId: null,
                startDate: moment().format('DD/MM/YYYY'),
                endDate: moment().format('DD/MM/YYYY')
            };
        }
    }
})();