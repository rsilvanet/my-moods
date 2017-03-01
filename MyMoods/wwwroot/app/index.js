$(document).ready(function () {
    activate();
});

var model = null;
var formId = null;
var selected = null;
var postArray = [];

function activate() {
    readIdFromUrl();
    getForm();
}

function readIdFromUrl() {
    formId = location.href.split('#')[1].replace('/', '');
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

function loadForm(data) {

    model = data;

    injectCompany();
    injectMoods();

    if (model.form.type == 'simple') {
        hideTagsPanel();
    }
    else {
        injectTags();
    }

    injectQuestions();
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

function injectCompany() {

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

function injectMoods() {

    var html = '';

    $('#moods-main-question').html(model.form.mainQuestion);

    model.moods.forEach(function (mood) {
        html += '<img id="mood-' + mood.value + '" src="' + mood.image + '" class="mood-image" onclick="selectMood(\'' + mood.value + '\')" />';
    });

    $('#moods-injector').html(html);
}

function injectTags() {

    var html = '';

    model.tags.forEach(function (tag) {

        var isSelectedForActualMood = false;
        var isSelectedForAnotherMood = false;

        if (postArray && postArray.length) {

            isSelectedForActualMood = _.some(selected.tags, function (item) {
                return item == tag.id;
            });

            postArray.forEach(function (item) {
                if (!isSelectedForAnotherMood) {
                    if (item.mood != selected.mood) {
                        isSelectedForAnotherMood = _.some(item.tags, function (item2) {
                            return item2 == tag.id;
                        });
                    }
                }
            });
        }

        if (!isSelectedForAnotherMood) {

            var cssClass = 'tag';

            if (isSelectedForActualMood) {
                cssClass += ' tag-selected';
            }

            html += '<div id="tag-' + tag.id + '" class="' + cssClass + '" onclick="selectTag(\'' + tag.id + '\')">' + tag.title + '</div>';
        }
    });

    $('#tags-injector').html(html);
}

function injectQuestions() {

    var html = '';
    var cssClass = "panel questions-panel";
    var hasTagSelectedForActualMood = selected && selected.tags && selected.tags.length > 0;

    if (!hasTagSelectedForActualMood) {
        cssClass += " disabled";
    }

    model.questions.forEach(function (question) {

        html += '<div class="' + cssClass + '">';
        html += '<div class="panel-header questions-panel-header">';
        html += '<span>' + question.title + '</span>';
        html += '</div>';

        var answer = null;

        if (selected) {
            answer = _.find(selected.answers, function (item) {
                return item.question == question.id;
            });
        }

        if (question.type == 'text') {
            html += '<div class="padded-holder">';
            html += '<textarea id="question-' + question.id + '" class="free-text" onkeyup="updateAnswer(\'' + question.id + '\')" onchange="updateAnswer(\'' + question.id + '\')">' + (answer != null ? answer.value : '') + '</textarea>';
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

function cleanAnswers() {

    if (selected && selected.answers) {
        selected.answers.forEach(function (item) {
            item.value = '';
            $('#question-' + item.question).val('');
        });
    }
}

function selectMood(mood) {

    if (selected && selected.mood) {
        $('#mood-' + selected.mood).removeClass('mood-image-selected');
    }

    selected = _.find(postArray, function (item) {
        return item.mood == mood;
    });

    if (selected == null) {

        selected = {
            mood: mood,
            tags: [],
            answers: []
        };

        postArray.push(selected);
    }

    $('#mood-' + mood).addClass('mood-image-selected');
    $('#tag-title').html(getTagsHelpText(mood));

    if (model.form.type == 'simple') {
        enableQuestionsPanel();
        enableSubmitPanel();
    }
    else {
        injectTags();
        injectQuestions();
        enableTagsPanel();
    }
}

function selectTag(id) {

    var tag = _.first(model.tags, function (item) {
        return item.id == id;
    });

    var isSelected = _.some(selected.tags, function (item) {
        return item == id;
    });

    if (isSelected) {

        selected.tags = _.reject(selected.tags, function (item) {
            return item == id;
        });

        $('#tag-' + id).removeClass('tag-selected');
    }
    else {
        selected.tags.push(id);
        $('#tag-' + id).addClass('tag-selected');
    }

    if (_.some(selected.tags)) {
        enableQuestionsPanel();
    }
    else {
        disableQuestionsPanel();
        cleanAnswers();
    }

    var isAnyTagSelected = false;

    if (postArray && postArray.length) {

        postArray.forEach(function (item) {
            if (!isAnyTagSelected) {
                isAnyTagSelected = item.tags && item.tags.length > 0;
            }
        });
    }

    if (isAnyTagSelected) {
        enableSubmitPanel();
    }
    else {
        disableSubmitPanel();
    }
}

function updateAnswer(questionId) {

    var question = _.find(model.questions, function (item) {
        return item.id == questionId;
    });

    if (selected && question) {

        if (!selected.answers) {
            selected.answers = [];
        }

        var answer = _.find(selected.answers, function (item) {
            return item.question == questionId;
        });

        if (answer == null) {

            answer = {
                question: questionId
            };

            selected.answers.push(answer);
        }

        if (question.type == 'text') {
            answer.value = $('#question-' + questionId).val();
        }
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