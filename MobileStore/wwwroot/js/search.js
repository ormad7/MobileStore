$(function () {
    $('#myForm').submit(function (e) {

        var form = $('#myForm');
        var url = form.attr('action');

        $.ajax(
            {
                type: 'POST',
                url: url,
                data: form.serialize(),
                dataType: 'html',
                success: function (html) {

                    var newDoc = document.open("text/html", "replace");
                    newDoc.write(html);
                    newDoc.close();
                }

            });

        e.preventDefault();

    })
});