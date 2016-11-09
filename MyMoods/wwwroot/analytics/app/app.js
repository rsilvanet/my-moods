(function () {
    'use strict';

    var app = angular.module('app', [

        'ui.router',
        
        'app.charts',
        'app.daily',
        'app.forms',
        'app.home',
        'app.login',
        'app.logout',
        'app.reviews',

        'app.components.average-chart',
        'app.components.daily-chart',
        'app.components.daily-panel',
        'app.components.sly-calendar'
    ]);

    app.constant('APP_CONFIG', {
        API_BASE_URL: '/api/analytics'
    });

    app.config(function ($httpProvider, $stateProvider, $urlRouterProvider) {
        $urlRouterProvider.otherwise('/');
        $httpProvider.interceptors.push('HttpInterceptorService');
    });

    app.run(['LoginService', function (loginService) {
        loginService.loadLoggedUserOnApp();
    }]);

})();