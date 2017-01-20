(function () {
    'use strict';
    var forms = angular.module('app.forms');

    forms.factory('FormsService', formsService);

    /* @ngInject */
    function formsService($http, APP_CONFIG) {

        return {
            all: all,
            getById: getById,
            post: post,
            put: put,
            enable: enable,
            disable: disable,
            normalizeType: normalizeType
        };

        function all(onlyActives) {

            if (onlyActives) {
                return $http.get(APP_CONFIG.API_BASE_URL + '/forms?onlyActives=true');
            }

            return $http.get(APP_CONFIG.API_BASE_URL + '/forms');
        }

        function getById(id) {
            return $http.get(APP_CONFIG.API_BASE_URL + '/forms/' + id);
        }

        function post(dto) {
            return $http.post(APP_CONFIG.API_BASE_URL + '/forms', dto);
        }

        function put(id, dto) {
            return $http.put(APP_CONFIG.API_BASE_URL + '/forms/' + id, dto);
        }

        function enable(id) {
            return $http.put(APP_CONFIG.API_BASE_URL + '/forms/' + id + '/enable');
        }

        function disable(id) {
            return $http.put(APP_CONFIG.API_BASE_URL + '/forms/' + id + '/disable');
        }

        function normalizeType(type) {
            switch (type) {
                case 'simple':
                    return 'Simples';
                case 'general':
                    return 'Com tags padrão';
                case 'generalWithCustomTags':
                    return 'Com tags padrão e customizadas';
                case 'generalOnlyCustomTags':
                    return 'Apenas com tags customizadas';
                default:
                    return 'Indefinido';
            }
        }
    }
})();