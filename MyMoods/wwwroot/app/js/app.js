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

        var container = $('#main-container');
        injectMoodsPanel(container, model.moods);
        injectTagsPanel(container, model.tags);
        injectQuestionsPanel(container, model.questions);
    }

    function injectMoodsPanel(container, moods) {

        var html = '';

        html += '<div class="panel moods-panel">';
        html += '<div class="panel-header moods-panel-header">';
        html += 'Como você está se sentindo?';
        html += '</div>';

        moods.forEach(function (mood) {
            html += '<img src="' + mood.image + '" class="mood-image"></img>';
        }, this);

        html += '</div>';

        container.append(html);
    }

    function injectTagsPanel(container, tags) {

        var html = '';

        html += '<div class="panel tags-panel">';
        html += '<div class="panel-header tags-panel-header">';
        html += 'Tags';
        html += '</div>';

        tags.forEach(function (tag) {
            html += '<div class="tag">' + tag.title + '</div>';
        }, this);

        html += '</div>';

        container.append(html);
    }

    function injectQuestionsPanel(container, questions) {

        var html = '';

        questions.forEach(function (question) {

            html += '<div class="panel questions-panel">';
            html += '<div class="panel-header questions-panel-header">';
            html += '<span>' + question.title + '</span>';
            html += '</div>';

            if (question.type == 'text') {
                html += '<div class="padded-holder">';
                html += '<textarea class="free-text"></textarea>';
                html += '</div>';
            }

            html += '</div>';

        }, this);

        container.append(html);
    }

})();
