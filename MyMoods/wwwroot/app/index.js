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

    injectCompany(model);
    injectMoods(model);

    if (model.form.type == 'simple') {
        hideTagsPanel();
    }
    else {
        injectTags(model);
    }

    injectQuestions(model);
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

function hideTagsPanel() {
    $('#tags-master-holder').css('display', 'none');
}

function injectCompany(model) {

    var html = '';

    if (model.form.company.logo) {
        html += '<img class="company-logo" src="' + model.form.company.logo + '"></img>';
    }
    else {
        html += '<h3>' + model.form.company.name + '</h3>';
    }

    html += model.form.title;

    $('#company-injector').html(html);
}

function injectMoods(model) {

    var html = '';

    $('#moods-main-question').html(model.form.mainQuestion);

    model.moods.forEach(function (mood) {
        html += '<img id="mood-' + mood.value + '" src="' + mood.image + '" class="mood-image" onclick="selectMood(\'' + mood.value + '\')" />';
    });

    $('#moods-injector').html(html);
}

function injectTags(model) {

    var html = '';

    model.tags.forEach(function (tag) {
        html += '<div id="tag-' + tag.id + '" class="tag" onclick="selectTag(\'' + tag.id + '\')">' + tag.title + '</div>';
    });

    $('#tags-injector').html(html);
}

function injectQuestions(model) {

    var html = '';

    model.questions.forEach(function (question) {

        html += '<div class="panel questions-panel">';
        html += '<div class="panel-header questions-panel-header">';
        html += '<span>' + question.title + '</span>';
        html += '</div>';

        if (question.type == 'text') {
            html += '<div class="padded-holder">';
            html += '<textarea id="question-' + question.id + '"s class="free-text"></textarea>';
            html += '</div>';
        }

        html += '</div>';
    });

    $('#questions-injector').html(html);
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
    $('#tag-title').html(getTagsHelpText(mood));

    if (model.form.type == 'simple') {
        enableQuestionsPanel();
        enableSubmitPanel();
    }
    else {
        enableTagsPanel();
    }
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
        contentType: "application/json"
    });

    promise.then(function (response) {
        endSubmitLoading();
        updateResultCache();
        window.location.href = 'success';
    }, function (response) {

        if (response.status == 400) {

            var json = JSON.parse(response.responseText);

            for (var key in json) {
                toastr.error(json[key]);
            }
        }
        else {
            toastr.error('Desculpe, não foi possível enviar a sua avaliação.');
        }

        endSubmitLoading();
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

function updateResultCache() {
    localStorage.setItem('my_moods_app_result', postModel.mood);
}