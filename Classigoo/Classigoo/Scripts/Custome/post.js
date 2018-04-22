


$(".fileuploading").find(".btnFucEdit").click(function () {
    $(this).parent().find('input:file').click();
    //$("#fucThird").click();
});

$("#fucFirst").change(function () {
    //readURL(this);
    var fileuploaded = $("#fucFirst").get(0).files;

    alert(fileuploaded[0].name);

    if (fileuploaded[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgFucFirst').attr('src', e.target.result);
        }
        reader.readAsDataURL(fileuploaded[0]);
    }

    $("#btnFucFirstEdit").parent().css('display', 'none');
    $("#btnFucFirstDelete").parent().css('display', 'block');
    $("#btnFucFirstDelete").parent().css('text-align', 'center');

});
$("#fucSecond").change(function () {
    //readURL(this);
    alert("2");

    var fileuploaded = $("#fucSecond").get(0).files;
    alert(fileuploaded[0].name);
    if (fileuploaded[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgFucSecond').attr('src', e.target.result);
        }
        reader.readAsDataURL(fileuploaded[0]);
    }

    $("#btnFucSecondEdit").parent().css('display', 'none');
    $("#btnFucSecondDelete").parent().css('display', 'block');
    $("#btnFucSecondDelete").parent().css('text-align', 'center');

});

$("#fucThird").change(function () {
    //readURL(this);
    var fileuploaded = $("#fucThird").get(0).files;
    if (fileuploaded[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgFucThird').attr('src', e.target.result);
        }
        reader.readAsDataURL(fileuploaded[0]);
    }

    $("#btnFucThirdEdit").parent().css('display', 'none');
    $("#btnFucThirdDelete").parent().css('display', 'block');
    $("#btnFucThirdDelete").parent().css('text-align', 'center');

});

$("#fucFour").change(function () {
    alert('four');
    //readURL(this);
    var fileuploaded = $("#fucFour").get(0).files;
    if (fileuploaded[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imgFucFour').attr('src', e.target.result);
        }
        reader.readAsDataURL(fileuploaded[0]);
    }

    $("#btnFucFourEdit").parent().css('display', 'none');
    $("#btnFucFourDelete").parent().css('display', 'block');
    $("#btnFucFourDelete").parent().css('text-align', 'center');

});

$("#btnFucFirstDelete").click(function () {

    $("#fucFirst").replaceWith($("#fucFirst").val('').clone(true));
    $('#imgFucFirst').attr('src', '/images/cg.jpg');

    $("#btnFucFirstEdit").parent().css('display', 'block');
    $("#btnFucFirstEdit").parent().css('text-align', 'center');

    $("#btnFucFirstDelete").parent().css('display', 'none');

});

$("#btnFucThirdDelete").click(function () {

    $("#fucThird").replaceWith($("#fucThird").val('').clone(true));
    $('#imgFucThird').attr('src', '/images/cg.jpg');

    $("#btnFucThirdEdit").parent().css('display', 'block');
    $("#btnFucThirdEdit").parent().css('text-align', 'center');

    $("#btnFucThirdDelete").parent().css('display', 'none');

});

$("#btnFucSecondDelete").click(function () {

    $("#fucSecond").replaceWith($("#fucSecond").val('').clone(true));
    $('#imgFucSecond').attr('src', '/images/cg.jpg');

    $("#btnFucSecondEdit").parent().css('display', 'block');
    $("#btnFucSecondEdit").parent().css('text-align', 'center');

    $("#btnFucSecondDelete").parent().css('display', 'none');

});
$("#btnFucFourDelete").click(function () {

    $("#fucFour").replaceWith($("#fucFour").val('').clone(true));
    $('#imgFucFour').attr('src', '/images/cg.jpg');

    $("#btnFucFourEdit").parent().css('display', 'block');
    $("#btnFucFourEdit").parent().css('text-align', 'center');

    $("#btnFucFourDelete").parent().css('display', 'none');

});

//function readURL(input) {

//    alert("he");

//    if (input.files && input.files[0]) {
//        var reader = new FileReader();

//        reader.onload = function (e) {
//            $('#imgFucThird').attr('src', e.target.result);
//        }

//        reader.readAsDataURL(input.files[0]);
//    }
//}



$("#categoryUL li").click(function (e, args) {
    var selectedCate = $(this).text();
    $("#categories").val("test" + ">");

});

$("#tabCategories a").click(function () {

    //var selectedCate = $(this).text();
    var selectedCate = $(this).attr("id");

    if (selectedCate == "catPro") {

        PathSetFirstLevel("Real Estate");
      
        $("#idtabPro").css("display", "block");
        $("#idtabPro").addClass("active");
        $("#tabPro").addClass("active");

    }
    else if (selectedCate == "catCV") {

        PathSetFirstLevel("Construction Vehicles");    

        $("#idtabCV").css("display", "block");
        $("#idtabCV").addClass("active");
        $("#tabCV").addClass("active");

    }
    else if (selectedCate == "catTV") {

        PathSetFirstLevel("Transportation Vehicles");

        $("#idtabTV").css("display", "block");
        $("#idtabTV").addClass("active");
        $("#tabTV").addClass("active");
    }
    else if (selectedCate == "catAV") {

        PathSetFirstLevel("Agricultural Vehicles");

        $("#idtabAV").css("display", "block");
        $("#idtabAV").addClass("active");
        $("#tabAV").addClass("active");

    }
    else if (selectedCate == "catPV") {

        PathSetFirstLevel("Passenger Vehicles");

        $("#idtabPV").css("display", "block");
        $("#idtabPV").addClass("active");
        $("#tabPV").addClass("active");
    }
    
});

//properties childs
$(".tabProChilds a").click(function () {

    //var secondLevl = $(this).parent().attr('id');

    //var thirdLevel = $(this).text();

    //if (secondLevl == "tabProForSale") {
    //    $("#cat-nav-l2 span").text("ForSale");
    //    $("#hdnCateSecondLevel").val("ForSale");

    //}
    //else {
    //    $("#cat-nav-l2 span").text("ForRent");
    //    $("#hdnCateSecondLevel").val("ForRent");

    //}
    //$("#cat-nav-l2").css("display", "inline");

    //$("#cat-nav-l3 span").text(thirdLevel);
    //$("#hdnCateThirdLevel").val(thirdLevel);

    //$("#cat-nav-l3").css("display", "inline");

    //$("#cat-nav-l4").css("display", "inline");

    //$("#divCategories").css('display', 'none');

    



    var secondLevel = $(this).text();

    $("#cat-nav-l2 span").text(secondLevel);
    $("#hdnCateSecondLevel").val(secondLevel);
    $("#cat-nav-l2").css("display", "inline");

    $("#cat-nav-l4").css("display", "inline");

    $("#divCategories").css('display', 'none');

    DisplayRespectiveFields("Properties");

});

//cars childs
$("#tabCarsModel a").click(function () {
    var thirdLevel = $(this).text();

    $("#cat-nav-l2 span").text("Model");
    $("#hdnCateSecondLevel").val("Model");

    $("#cat-nav-l2").css("display", "inline");

    $("#cat-nav-l3 span").text(thirdLevel);
    $("#hdnCateThirdLevel").val(thirdLevel);
    $("#cat-nav-l3").css("display", "inline");

    $("#cat-nav-l4").css("display", "inline");

    $("#divCategories").css('display', 'none');

    DisplayRespectiveFields("Cars");

});

//Construction vehicles childs
$("#tabCV a").click(function () {
    var secondLevel = $(this).text();

    $("#cat-nav-l2 span").text(secondLevel);
    $("#hdnCateSecondLevel").val(secondLevel);
    $("#cat-nav-l2").css("display", "inline");

    $("#cat-nav-l4").css("display", "inline");

    $("#divCategories").css('display', 'none');

    DisplayRespectiveFields("Construction Vehicles");
});

//Agricultural Vehicles childs
$("#tabAV a").click(function () {
    var secondLevel = $(this).text();

    $("#cat-nav-l2 span").text(secondLevel);
    $("#hdnCateSecondLevel").val(secondLevel);
    $("#cat-nav-l2").css("display", "inline");

    $("#cat-nav-l4").css("display", "inline");

    $("#divCategories").css('display', 'none');
    DisplayRespectiveFields("Agricultural Vehicles");
});

//Transportation Vehicles
$("#tabTV a").click(function () {
    var secondLevel = $(this).text();

    $("#cat-nav-l2 span").text(secondLevel);
    $("#hdnCateSecondLevel").val(secondLevel);
    $("#cat-nav-l2").css("display", "inline");

    $("#cat-nav-l4").css("display", "inline");

    $("#divCategories").css('display', 'none');
    DisplayRespectiveFields("Transportation Vehicles");
});

//Transportation Vehicles
$("#tabPV a").click(function () {
    var secondLevel = $(this).text();

    $("#cat-nav-l2 span").text(secondLevel);
    $("#hdnCateSecondLevel").val(secondLevel);
    $("#cat-nav-l2").css("display", "inline");

    $("#cat-nav-l4").css("display", "inline");

    $("#divCategories").css('display', 'none');
    DisplayRespectiveFields("Passenger Vehicles");
});


//home buttion on categories navigation
$("#cat-nav-l0 a").click(function () {
    $("#cat-nav-l4").css("display", "none");
    RefreshCategories();
});

//edit buttion on categories navigation
$("#cat-nav-l4 a").click(function () {
    $("#cat-nav-l4").css("display", "none");
    RefreshCategories();
});

//back button categories list
$(".backHome").click(function () {

    RefreshCategories();
});

function RefreshCategories() {
    $("#divCategories").css('display', 'block');

    $("#hdnCateFristLevel").val("");
    $("#hdnCateSecondLevel").val("");
    $("#hdnCateThirdLevel").val("");

    $("#cat-nav-l1").css("display", "none");
    $("#cat-nav-l2").css("display", "none");
    $("#cat-nav-l3").css("display", "none");

    $(".idtabCategoriesul li").removeClass("active");
    $(".idtabCategoriesul li").css("display", "none");

    $('.idtabCategoriesul li').each(function (i) {
        $($(this).find('a').attr('href')).removeClass("active");
    });

    $("#idtabCategories").css("display", "block");
    $("#idtabCategories").addClass("active");
    $("#tabCategories").addClass("active");
}

function PathSetFirstLevel(firstlevel) {

    $("#hdnCateFristLevel").val(firstlevel);

    $("#cat-nav-l1 span").text(firstlevel);

    $("#cat-nav-l1").css("display", "inline");

    $(".idtabCategoriesul li").removeClass("active");
    $(".idtabCategoriesul li").css("display", "none");
    $('.idtabCategoriesul li').each(function (i) {
        $($(this).find('a').attr('href')).removeClass("active");
    });
}

$("#tstButton").click(function () {
    alert($("#hdnCateFristLevel").val());
    alert($("#hdnCateSecondLevel").val());
    alert($("#hdnCateThirdLevel").val());
});

function DisplayRespectiveFields(selectedcategory) {

    $(".forallcollapse").css('display', 'none');

    var selectedCate = selectedcategory;
    if (selectedCate == "Properties") {

        var selectedPrtoperty = $("#hdnCateThirdLevel").val().trim();

        $("#forProperties").css('display', 'block');


        if (selectedPrtoperty == "Agricultural Land") {
            $("#pro-availability").css('display', 'block');
            $("#pro-acres").css('display', 'block');

            $("#pro-sqarefeets").css('display', 'none');
            $("#pro-furnishing").css('display', 'none');
            $("#pro-bedrooms").css('display', 'none');
            $("#pro-squareyards").css('display', 'none');
        }

        if (selectedPrtoperty == "Plots/Land") {
            $("#pro-availability").css('display', 'block');
            $("#pro-squareyards").css('display', 'block');

            $("#pro-sqarefeets").css('display', 'none');
            $("#pro-furnishing").css('display', 'none');
            $("#pro-bedrooms").css('display', 'none');
            $("#pro-acres").css('display', 'none');
        }



        if (selectedPrtoperty == "Shops & Offices") {
            $("#pro-availability").css('display', 'block');
            $("#pro-sqarefeets").css('display', 'block');
            $("#pro-furnishing").css('display', 'block');

            $("#pro-bedrooms").css('display', 'none');
            $("#pro-acres").css('display', 'none');
            $("#pro-squareyards").css('display', 'none');
        }

        if (selectedPrtoperty == "Apartments" || selectedPrtoperty == "Independent Houses & Villas") {
            $("#pro-availability").css('display', 'block');
            $("#pro-sqarefeets").css('display', 'block');
            $("#pro-furnishing").css('display', 'block');
            $("#pro-bedrooms").css('display', 'block');

            $("#pro-acres").css('display', 'none');
            $("#pro-squareyards").css('display', 'none');
        }


    }
    else if (selectedCate == "Construction Vehicles") {
      //  $("#forCV").css('display', 'block');
    }
    else if (selectedCate == "Transportation Vehicles") {
      //  $("#forTV").css('display', 'block');
    }
    else if (selectedCate == "Agricultural Vehicles") {
      //  $("#forAV").css('display', 'block');
    }
    else if (selectedCate == "Passenger Vehicles") {
      //  $("#forPV").css('display', 'block');
    }

    
}


//-------------------------- for properties validation -------------------------------

$("#btnSubmit").click(function () {

    alert($('#forProperties').css('display') == 'none');
});

