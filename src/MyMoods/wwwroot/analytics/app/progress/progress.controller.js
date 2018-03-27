(function () {
    'use strict';
    var progress = angular.module('app.progress');

    progress.controller('ProgressController', progressController);

    progressController.$inject = ['$http', 'APP_CONFIG'];

    /* @ngInject */
    function progressController($http, APP_CONFIG) {

        var vm = this;

        $http.get(APP_CONFIG.API_BASE_URL + '/ping')
            .then(function(){
                //Just for the 401
            }, function(){
                //Just for the 401
            });
    }
})();