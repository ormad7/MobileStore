function getRecommendedProducts(productId) {

    var container = $("#recommended");
    $.get("/Products/GetRecommendedProducts", { prodID: productId }, function (data) { container.html(data); });

}

$(function () {
    $('.product-preview').on('click', function (e) {
        e.preventDefault();
        getRecommendedProducts($(this).attr("productid"));
    });

    $('#product-main-img .slick-next').on('click', function (e) {
        e.preventDefault();
        getRecommendedProducts($('#product-main-img .slick-active').attr("productid"));
    });

    $('#product-main-img .slick-prev').on('click', function (e) {
        e.preventDefault();
        getRecommendedProducts($('#product-main-img .slick-active').attr("productid"));
    });

    $('#product-imgs .slick-next').on('click', function (e) {
        e.preventDefault();
        getRecommendedProducts($('#product-main-img .slick-active').attr("productid"));
    });

    $('#product-imgs .slick-prev').on('click', function (e) {
        e.preventDefault();
        getRecommendedProducts($('#product-main-img .slick-active').attr("productid"));
    });
});
