(function () {
    'use strict';

    var model = null;

    $.get('/api/forms/57976abd266b3c042d6217f6/metadata')
        .then(function (response) {
            load(response);
        }, function (response) {
            alert(response);
        });

    function load(data) {
        model = data;
        injectMoods(model.moods);
    }

    function injectMoods(moods) {

        var html = '';
        var container = $('#moods-container');

        html += '<div class="text-center">';

        moods.forEach(function (mood) {
            html += '<img src="' + mood.image + '" class="mood-image" />';
        }, this);

        html += '</div>';

        container.html(html);
    }

})();
