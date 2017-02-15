(function () {
    'use strict';

    var app = angular.module('app', [
        'ui.router',
        'app.components',
        'app.charts',
        'app.forms',
        'app.home',
        'app.login',
        'app.logout',
        'app.password',
        'app.radar',
        'app.resume',
        'app.reviews',
        'app.tags'
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