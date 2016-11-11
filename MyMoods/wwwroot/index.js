$('#btn-want-to-use').click(function () {
    setTimeout(function () {
        $('#company').focus();
    }, 500);
});

$('#form-register').submit(function (e) {

    e.preventDefault();
    startSubmitLoading();

    var model = {
        company: $('#company').val(),
        name: $('#name').val(),
        email: $('#email').val(),
        password: $('#password').val()
    };

    var promise = $.ajax({
        type: 'POST',
        url: '/api/register',
        data: JSON.stringify(model),
        dataType: 'text',
        contentType: "application/json",
        success: function (response) {
            endSubmitLoading();
            window.location.href = 'analytics';
        },
        error: function (response) {
            //TODO: Tratar erros!
            endSubmitLoading();
            alert('Desculpe, não foi possível realizar o seu cadastro.');
        }
    });
});

function startSubmitLoading() {
    $('#btn-register').addClass('disabled');
    $('#btn-register').html('Cadastrando');
}

function endSubmitLoading() {
    $('#btn-register').removeClass('disabled');
    $('#btn-register').html('Cadastrar');
}