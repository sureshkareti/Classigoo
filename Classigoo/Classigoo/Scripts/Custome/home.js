
$(document).ready(function () {
    var urlParams = new URLSearchParams(window.location.search);
    var requestedItem = urlParams.get('rd');
    if (requestedItem != null) {
        $('.categoryId li').removeClass('active');
        $("[data-link='" + requestedItem + "']").addClass('active');
        $('.category-link').css('display', 'none');
        $('#' + requestedItem).css('display', 'block');
    }


    console.log(); // true
    
});




$('.categoryId li').on('click', function (e) {
    e.preventDefault();
    $('.categoryId li').removeClass('active');
    $(this).addClass('active');
    $('.category-link').css('display', 'none');  
    $('#'+$(this).attr("data-link")).css('display', 'block');
});