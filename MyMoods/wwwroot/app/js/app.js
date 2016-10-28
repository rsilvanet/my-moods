$(document).ready(function () {
    activate();
});

var model = null;
var postModel = null;

function activate() {

    postModel = {
        selectedMood: null,
        tags: [],
        questions: []
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
    injectCompanyPanel(container, model.form.company);
    injectMoodsPanel(container, model.moods);
    injectTagsPanel(container, model.tags);
    injectQuestionsPanel(container, model.questions);
    injectSubmitPanel(container);
    disableTagsPanel();
    disableQuestionsPanel();
    disableSubmitPanel();
}

function injectCompanyPanel(container, company) {

    var html = '';

    html += '<div class="company-panel">'
    //html += '<img class="company-logo" src="' + company.logo + '"></img>';
    html += '<img class="company-logo" src="data:image/png;base64,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"></img>';
    html += '</div>'

    container.append(html);
}

function injectMoodsPanel(container, moods) {

    var html = '';

    html += '<div class="panel moods-panel">';
    html += '<div class="panel-header moods-panel-header">';
    html += '<span>Como você está se sentindo?</span>';
    html += '</div>';

    moods.forEach(function (mood) {
        html += '<img id="mood-' + mood.value + '" src="' + mood.image + '" class="mood-image" onclick="selectMood(\'' + mood.value + '\')"></img>';
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
    html += '<span id="tag-title">Selecione um mood</span>';
    html += '</div>';

    tags.forEach(function (tag) {
        html += '<div id="tag-' + tag.id + '" class="tag" onclick="selectTag(\'' + tag.id + '\')">' + tag.title + '</div>';
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

function injectSubmitPanel(container) {

    var html = '';

    html += '<div class="submit-panel">'
    html += '<button class="submit-button" type="submit">Enviar</button>';
    html += '</div>'

    container.append(html);
}

function disableTagsPanel() {
    $('.tags-panel').addClass('panel-disabled');
}

function disableQuestionsPanel() {
    $('.questions-panel').addClass('panel-disabled');
}

function disableSubmitPanel() {
    $('.submit-panel').addClass('panel-disabled');
}

function enableTagsPanel() {
    $('.tags-panel').removeClass('panel-disabled');
}

function enableQuestionsPanel() {
    $('.questions-panel').removeClass('panel-disabled');
}

function enableSubmitPanel() {
    $('.submit-panel').removeClass('panel-disabled');
}

function selectMood(mood) {

    if (postModel.selectedMood) {
        $('#mood-' + postModel.selectedMood).removeClass('mood-image-selected');
    }

    postModel.selectedMood = mood;
    $('#mood-' + postModel.selectedMood).addClass('mood-image-selected');
    $('#selected-mood-label').html(getMoodTitle(mood));
    $('#tag-title').html(getTagsHelpText(mood));

    enableTagsPanel();
}

function selectTag(id) {

    var tag = _.first(model.tags, function (item) {
        return item.id == id;
    });

    var isSelected = _.some(postModel.tags, function (item) {
        return item == id;
    });

    if (isSelected) {
        postModel.tags = _.reject(postModel.tags, function (item) { return item == id; });
        $('#tag-' + id).removeClass('tag-selected');
    }
    else {
        postModel.tags.push(id);
        $('#tag-' + id).addClass('tag-selected');
    }

    enableQuestionsPanel();
    enableSubmitPanel();

    if (!_.some(postModel.tags)) {
        disableQuestionsPanel();
        disableSubmitPanel();
    }
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

function getTagsHelpText(mood) {

    var text = '';

    if (model) {
        model.moods.forEach(function (item) {
            if (item.value == mood) {
                text = item.tagsHelpText;
                return;
            }
        });
    }

    return text;
}

function readQuestions() {

}