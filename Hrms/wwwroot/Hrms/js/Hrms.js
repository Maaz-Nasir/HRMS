OnBegin = function () {
}
OnSubmit = function () {
}
OnComplete = function () {
}
OnFailed = function (response) {
    console.log(response);
    toastr.error(response.message, "Error!");
}
OnSuccess = function (response) {
    console.log(response);
    if (response.success) {
        toastr.success(response.message, "Success", {
            timeOut: "5000"
        });
        setTimeout(function () {
            window.location.href = response.targetURL;
        }, 2000);
    }
    else {
        toastr.error(response.message, "Error!");
    }
}
function DisabledView() {
    $('input').attr('disabled', true);
    $('textarea').attr('disabled', true);
    $('select').attr('disabled', true);
    $('checkbox').attr('disabled', true);
    $('radio').attr('disabled', true);
}
$(function () {
    $('.dateselect').datepicker({
        format: 'dd-M-yyyy',
        autoclose: true,
        todayHighlidht: true,
    });
    $('select').select2();
});
$(document).on("change", "#subscription_input", function () {
    var current_context = $(this);
    $.ajax({
        type: "POST",
        url: current_context.attr("data-href"),
        data: "subscriberID=" + current_context.val(),
        success: function (response) {
            console.log(response);
            window.location.href = response.targetURL;
        },
        error: function (data) {
            console.log(data);
        }
    });
});
var AjaxGenericCalls = {
    ajaxRequest: function (paramObj) {
        return new Promise(function (resolve, reject) {
            $.ajax(paramObj).done(function (response) {
                resolve(response);
            }).fail(function (response) {
                reject(response);
            })
        });
    }
};