$(document).ready(function () {
    activate();
});

var model = null;
var postModel = null;
var formId = null;

function activate() {

    postModel = {
        mood: null,
        tags: [],
        answers: []
    };

    getIdFromUrl();
    getForm();
}

function getForm() {

    var promise = $.get('/api/forms/' + formId + '/metadata');

    promise.then(function (response) {
        loadForm(response);
        showForm();
    }, function (response) {
        showError();
    });
}

function getIdFromUrl() {

    var url = location.href.split('#')[1];

    formId = url.replace('/', '');
}

function loadForm(data) {

    model = data;

    injectCompany(model.form);
    injectMoods(model.moods);
    injectTags(model.tags);
    injectQuestions(model.questions);
    disableTagsPanel();
    disableQuestionsPanel();
    disableSubmitPanel();
}

function showError() {
    $('.loading-panel').css('display', 'none');
    $('.error-panel').css('display', 'block');
    $('.main-container').css('display', 'none');
}

function showForm() {
    $('.loading-panel').css('display', 'none');
    $('.error-panel').css('display', 'none');
    $('.main-container').css('display', 'block');
}

function injectCompany(form) {

    var html = '';
    var container = $('#company-injector');
    var company = form.company;

    if (company.logo) {
        html += '<img class="company-logo" src="' + company.logo + '"></img>';
    }
    else {
        html += '<h3>' + company.name + '</h3>';
    }

    html += form.title;

    container.html(html);
}

function injectMoods(moods) {

    var html = '';
    var container = $('#moods-injector');

    moods.forEach(function (mood) {
        html += '<img id="mood-' + mood.value + '" src="' + mood.image + '" class="mood-image" onclick="selectMood(\'' + mood.value + '\')" />';
    });

    container.html(html);
}

function injectTags(tags) {

    var html = '';
    var container = $('#tags-injector');

    tags.forEach(function (tag) {
        html += '<div id="tag-' + tag.id + '" class="tag" onclick="selectTag(\'' + tag.id + '\')">' + tag.title + '</div>';
    });

    container.html(html);
}

function injectQuestions(questions) {

    var html = '';
    var container = $('#questions-injector');

    questions.forEach(function (question) {

        html += '<div class="panel questions-panel">';
        html += '<div class="panel-header questions-panel-header">';
        html += '<span>' + question.title + '</span>';
        html += '</div>';

        if (question.type == 'text') {
            html += '<div class="padded-holder">';
            html += '<textarea id="question-' + question.id + '" class="free-text"></textarea>';
            html += '</div>';
        }

        html += '</div>';

    });

    container.html(html);
}

function disableTagsPanel() {
    $('.tags-panel').addClass('disabled');
}

function disableQuestionsPanel() {
    $('.questions-panel').addClass('disabled');
}

function disableSubmitPanel() {
    $('.submit-panel').addClass('disabled');
}

function enableTagsPanel() {
    $('.tags-panel').removeClass('disabled');
}

function enableQuestionsPanel() {
    $('.questions-panel').removeClass('disabled');
}

function enableSubmitPanel() {
    $('.submit-panel').removeClass('disabled');
}

function selectMood(mood) {

    if (postModel.mood) {
        $('#mood-' + postModel.mood).removeClass('mood-image-selected');
    }

    postModel.mood = mood;
    $('#mood-' + postModel.mood).addClass('mood-image-selected');
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

function readAnswers() {

    var answers = [];

    model.questions.forEach(function (question) {

        var answer = {
            value: null,
            question: question.id
        };

        if (question.type == 'text') {
            answer.value = $('#question-' + question.id).val();
        }

        answers.push(answer);
    });

    return answers;
}

function submit() {

    postModel.answers = readAnswers();

    startSubmitLoading();

    var promise = $.ajax({
        type: 'POST',
        url: '/api/forms/' + formId + '/reviews',
        data: JSON.stringify(postModel),
        dataType: 'text',
        contentType: "application/json",
        success: function (response) {
            //TODO: Save cache!
            endSubmitLoading();
            window.location.href = 'success';
        },
        error: function (response) {
            //TODO: Treat errors!
            endSubmitLoading();
            alert('Desculpe, não foi possível enviar a sua avaliação.');
        }
    });
}

function startSubmitLoading() {
    $('#submit-button').addClass('disabled');
    $('#submit-button').html('Enviando');
}

function endSubmitLoading() {
    $('#submit-button').removeClass('disabled');
    $('#submit-button').html('Enviar');
}