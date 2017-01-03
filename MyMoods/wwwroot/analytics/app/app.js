(function () {
    'use strict';

    var app = angular.module('app', [
        'ui.router',
        'app.components',
        'app.charts',
        'app.daily',
        'app.forms',
        'app.home',
        'app.login',
        'app.logout',
        'app.password',
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