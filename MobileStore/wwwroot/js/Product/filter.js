function loadScript (url) {
    jQuery.ajax({
        url: url,
        dataType: 'script',
        async: false
    });
}

$(function () {

    var form = $('#FilterForm');
    $('#FilterButton').on('click', function () {
        // filter thumb imgs
        $.ajax({
            url: '/Products/FilterThumbImgs',
            data: form.serialize(),
            success: function (partialviewresult) {
                $('#thumb-imgs').html(partialviewresult);
            },
            async: false
        });

        // load slick event
        loadScript('/js/main.js');

        // reload recommended products after filter
        loadScript('/js/Product/recommend.js');

        // filter main img
        $.ajax({
            url: '/Products/FilterMainImgs',
            data: form.serialize(),
            success: function (partialviewresult) {
                $('#main-imgs').html(partialviewresult);
            },
            async: false
        });

        // load slick event
        loadScript('/js/main.js');

        // reload recommended products after filter
        loadScript('/js/Product/recommend.js');

        // get recommended products for current product
        if ($('#product-main-img .slick-active').length) {
            getRecommendedProducts($('#product-main-img .slick-active').attr("productid"));
        } else {
            $("#recommended").html(null);
        }
    });
});