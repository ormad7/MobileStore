$(function () {
    $("#searchProduct").autocomplete({
        source: "/Products/SearchAutoComplete",
        minLength: 1,
        select: function (event, ui) {
            location.href = '/Products/ViewProduct?productid=' + ui.item.id;
        }
    });
});