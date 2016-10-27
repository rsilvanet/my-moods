$(document).ready(function () {
    activate();
});

var model = null;
var postModel = null;

function activate() {

    postModel = {
        selectedMood: null
    };

    getForm('57976abd266b3c042d6217f6');
}

function getForm(formId) {

    var promise = $.get('/api/forms/' + formId + '/metadata');

    promise.then(function (response) {
        loadForm(response);
    }, function (response) {
        alert(response);
    });
}

function loadForm(data) {

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
    html += '<span>Como você está se sentindo?</span>';
    html += '</div>';

    moods.forEach(function (mood) {
        html += '<img id="' + mood.value + '" src="' + mood.image + '" class="mood-image" onclick="selectMood(\'' + mood.value + '\')"></img>';
    }, this);

    html += '<br>';
    html += '<span id="selected-mood-label"></span>';
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

function selectMood(mood) {

    if (postModel.selectedMood) {
        $('#' + postModel.selectedMood).removeClass('mood-image-selected');
    }

    postModel.selectedMood = mood;
    $('#' + postModel.selectedMood).addClass('mood-image-selected');
    $('#selected-mood-label').html(getMoodTitle(mood));
}

function getMoodTitle(mood) {

    var title = '';

    if (model) {
        model.moods.forEach(function (item) {
            if (item.value == mood) {
                title = item.title;
                return;
            }
        });
    }

    return title;
}