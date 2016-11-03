(function () {
    'use strict';
    
    var app = angular.module('app', [
        
        'ui.router',

        'app.charts',
        'app.daily',
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

        $stateProvider.state({
            name: 'home',
            url: '/',
            template: '<h3 style="margin: 10px;">Bem-vindo!</h3>',
            controller: function ($http) {
                $http.get('/api/analytics/ping').then();
            }
        });

        $urlRouterProvider.otherwise('/');

        $httpProvider.interceptors.push('HttpInterceptorService');
    });

    app.run(['LoginService', function (loginService) {
        loginService.loadLoggedUserOnApp();
    }]);

})();