(function () {
    'use strict';
    var app = angular.module('app');

    app.factory('ErrorHandlerService', errorHandlerService);

    /* @ngInject */
    function errorHandlerService() {

        return {
            normalizeAndShow: normalizeAndShow
        };

        function normalizeAndShow(response) {

            toastr.remove();

            if (response.status == 400) {
                for (var key in response.data) {
                    toastr.error(response.data[key]);
                }
            }
            else {
                toastr.error(response.data);
            }
        }
    }
})();