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
        'app.password',
        'app.reviews',
        'app.components'
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