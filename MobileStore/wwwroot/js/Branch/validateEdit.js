function validateForm() {
    $.ajax({
        type: "POST",
        url: "/Branches/ValidateEdit",
        data: $('#EditForm').serialize(),
        success: function (data) {
            if (data.loc == null & data.name == null) {
                $('#EditForm').submit();
            }

            if (data.loc != null) {
                $('#locspan').empty();
                $('#locspan').html(data.loc)
            }

            if (data.name != null) {
                $('#namespan').empty();
                $('#namespan').html(data.name)
            }
        },
        error: function (jqXHR, exception) {
            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'Not connect.\n Verify Network.';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.\n' + jqXHR.responseText;
            }
            console.log(msg);
        },
        dataType: 'json'
    });
}

$(function () {

    $('#EditButton').on('click', function (e) {
        e.preventDefault();
        validateForm();
    });
});