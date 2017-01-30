$(document).ready(function () {
    activate();
});

function activate() {
    $('#mood-injector').html('<img src="/assets/emojis/' + getResultCache() + '.png" class="initial-panel-img" />');
}

function getResultCache() {
    return localStorage.getItem('my_moods_app_result');
}