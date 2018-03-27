(function () {
    'use strict';
    var tags = angular.module('app.tags');

    tags.factory('TagsService', tagsService);

    /* @ngInject */
    function tagsService($http, APP_CONFIG) {

        return {
            all: all,
            post: post,
            enable: enable,
            disable: disable,
            normalizeType: normalizeType
        };

        function all(onlyActives) {

            if (onlyActives) {
                return $http.get(APP_CONFIG.API_BASE_URL + '/tags?onlyActives=true');
            }

            return $http.get(APP_CONFIG.API_BASE_URL + '/tags');
        }

        function post(dto) {
            return $http.post(APP_CONFIG.API_BASE_URL + '/tags', dto);
        }

        function enable(id) {
            return $http.put(APP_CONFIG.API_BASE_URL + '/tags/' + id + '/enable');
        }

        function disable(id) {
            return $http.put(APP_CONFIG.API_BASE_URL + '/tags/' + id + '/disable');
        }

        function normalizeType(type) {
            switch (type) {
                case 'realization':
                    return 'Realização';
                case 'esteem':
                    return 'Autoestima';
                case 'social':
                    return 'Social';
                case 'safety':
                    return 'Segurança';
                case 'physiological':
                    return 'Fisiológico';
                default:
                    return 'Indefinido';
            }
        }
    }
})();