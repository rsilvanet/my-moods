(function () {
    'use strict';
    var components = angular.module('app.components');

    /* @ngInject */
    components.directive('slyCalendar', function ($window, $timeout, $log, ReviewsService) {
        return {
            restrict: 'E',
            templateUrl: 'app/components/sly-calendar/sly-calendar.html',
            scope: {
                formId: '=',
                activeDayCallback: '&'
            },
            link: function (scope) {

                scope.isLoaded = false;

                scope.$watch('formId', function () {
                    if (scope.formId) {
                        load();
                    }
                });

                function load() {

                    scope.isLoaded = false;

                    var window = $($window);
                    var frame = $('#theframe');
                    var wrap = frame.parent();

                    var options = {
                        itemNav: 'forceCentered',
                        smart: 1,
                        activateOn: 'click',
                        mouseDragging: 1,
                        touchDragging: 1,
                        releaseSwing: 1,
                        startAt: 0,
                        scrollBy: 1,
                        activatePageOn: 'click',
                        speed: 1,
                        elasticBounds: 1,
                        dragHandle: 1,
                        dynamicHandle: 1,
                        clickBar: 1,
                        horizontal: 1,
                        scrollBar: wrap.find('.scrollbar') || null,
                        pagesBar: wrap.find('.pages') || null,
                        forward: wrap.find('.forward') || null,
                        backward: wrap.find('.backward') || null,
                        prev: wrap.find('.prev') || null,
                        next: wrap.find('.next') || null,
                        prevPage: wrap.find('.prevPage') || null,
                        nextPage: wrap.find('.nextPage') || null
                    };

                    var events = {
                        change: function (event) {
                            $timeout(function () {
                                scope.$apply();
                            }, 100);
                        },
                        active: function (event, index) {
                            $timeout(function () {
                                scope.activeDay = scope.days[index].date;
                                scope.activeDayCallback({ date: scope.activeDay });
                            }, 100);
                        }
                    };

                    if (scope.sly) {
                        if (scope.sly.items) {
                            var index = scope.sly.items.length - 1;

                            while (index >= 0) {
                                scope.sly.remove(index);
                                index--;
                            }
                        }

                        scope.sly.destroy();
                    }

                    scope.sly = new Sly(frame, options, events).init();

                    ReviewsService.getResume(scope.formId)
                        .then(function (response) {

                            var items = response.data;

                            if (items && items.length) {

                                items = _.orderBy(items, function (item) {
                                    return moment(item.date);
                                }, 'desc');

                                scope.days = [];

                                items.forEach(function (item) {

                                    var date = moment(item.date);

                                    scope.days.unshift({
                                        date: date.toDate(),
                                        formatted: date.format('DD/MM/YYYY')
                                    });

                                    addSlyItem(item);
                                });

                                scope.sly.toEnd();
                                scope.sly.activate(scope.sly.rel.lastItem);

                                window.on('resize', function () {
                                    frame.sly('reload');
                                });
                            }

                            scope.isLoaded = true;
                        });

                    function addSlyItem(item) {

                        var html = '';
                        var day = moment(item.date);

                        html += '<li>';
                        html += '<span>' + day.format('DD/MM/YYYY') + '</span>';
                        html += '<br><br><br>';
                        html += '<img src="/assets/emojis/' + item.avg.mood + '.png" style="height: 100px;">';
                        html += '<br><br>';
                        
                        if(item.count == 1) {
                            html += '<span>' + item.count + ' avaliação</span>';
                        }
                        else if(item.count > 1) {
                            html += '<span>' + item.count + ' avaliações</span>';
                        }

                        html += '<br><br>';
                        html += '<span>' + item.avg.points.toFixed(2) + ' pontos</span>';
                        html += '</li>';

                        scope.sly.add(html, 0);
                    }
                }
            }
        };
    });
})();