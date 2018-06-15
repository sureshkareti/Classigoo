


$(".fileuploading").find(".btnFucEdit").click(function () {
    $(this).parent().find('input:file').click();
    //$("#fucThird").click();
});

$("#fucFirst").change(function () {
    //readURL(this);
    var fileuploaded = $("#fucFirst").get(0).files;

    //alert(fileuploaded[0].name);

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

    //validation
    removeerrorImg1();

});
$("#fucSecond").change(function () {
    //readURL(this);


    var fileuploaded = $("#fucSecond").get(0).files;

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

    if (isEdit) {
        var imgUrl = $('#imgFucFirst').attr('src');
        console.log(addId);
        var id = addId;
        var category = $("#hdnCateFristLevel").val();
        console.log(imgUrl + id + category);

        DeleteImage(imgUrl, category, "1", id);
    }
    else {
        $("#fucFirst").replaceWith($("#fucFirst").val('').clone(true));
        $('#imgFucFirst').attr('src', '/images/upimglogo1.png');

        $("#btnFucFirstEdit").parent().css('display', 'block');
        $("#btnFucFirstEdit").parent().css('text-align', 'center');

        $("#btnFucFirstDelete").parent().css('display', 'none');

        $("#divimgFucFirst").css("border", "none");
        $("#divimgFucFirstError").css("display", "none");

        //validation
        showerrorImg1();
    }

});

$("#btnFucThirdDelete").click(function () {

    if (isEdit) {
        var imgUrl = $('#imgFucThird').attr('src');
        console.log(addId);
        var id = addId;
        var category = $("#hdnCateFristLevel").val();
        console.log(imgUrl + id + category);

        DeleteImage(imgUrl, category, "3", id);
    }
    else {

        $("#fucThird").replaceWith($("#fucThird").val('').clone(true));
        $('#imgFucThird').attr('src', '/images/upimglogo1.png');

        $("#btnFucThirdEdit").parent().css('display', 'block');
        $("#btnFucThirdEdit").parent().css('text-align', 'center');

        $("#btnFucThirdDelete").parent().css('display', 'none');

    }

});

$("#btnFucSecondDelete").click(function () {

    if (isEdit) {
        var imgUrl = $('#imgFucSecond').attr('src');
        console.log(addId);
        var id = addId;
        var category = $("#hdnCateFristLevel").val();
        console.log(imgUrl + id + category);

        DeleteImage(imgUrl, category, "2", id);
    }
    else {
        $("#fucSecond").replaceWith($("#fucSecond").val('').clone(true));
        $('#imgFucSecond').attr('src', '/images/upimglogo1.png');

        $("#btnFucSecondEdit").parent().css('display', 'block');
        $("#btnFucSecondEdit").parent().css('text-align', 'center');

        $("#btnFucSecondDelete").parent().css('display', 'none');
    }
    

});

$("#btnFucFourDelete").click(function () {

    if (isEdit) {
        var imgUrl = $('#imgFucFour').attr('src');
        console.log(addId);
        var id = addId;
        var category = $("#hdnCateFristLevel").val();
        console.log(imgUrl + id + category);

        DeleteImage(imgUrl, category, "4", id);
    }
    else {
        $("#fucFour").replaceWith($("#fucFour").val('').clone(true));
        $('#imgFucFour').attr('src', '/images/upimglogo1.png');

        $("#btnFucFourEdit").parent().css('display', 'block');
        $("#btnFucFourEdit").parent().css('text-align', 'center');

        $("#btnFucFourDelete").parent().css('display', 'none');
    }
    

});

function DeleteImage(imageUrl, category, position, id) {

    $.ajax({
        type: "POST",
        url: '/Post/DeleteImageEdit',
        contentType: "application/json; charset=utf-8",
        processData: false,
        dataType: 'json',
        async: false,
        data: JSON.stringify({ 'imgUrl': imageUrl, 'category': category, 'position': position, 'id': id }),
        success: function (msg) {
            alert(msg);
            if (msg == "error") {
                alert("there is problem with deleting image");
            }
            else {

                var imgUrl1 = msg[0];
                var imgUrl2 = msg[2];
                var imgUrl3 = msg[3];
                var imgUrl4 = msg[4];

                console.log(imgUrl1);
                console.log(imgUrl2);
                console.log(imgUrl3);
                console.log(imgUrl4);

                if (imgUrl1 != "") {

                    $('#imgFucFirst').attr('src', "'" + imgUrl1 + "'");
                }
                else {

                    $("#fucFirst").replaceWith($("#fucFirst").val('').clone(true));
                    $('#imgFucFirst').attr('src', '/images/upimglogo1.png');

                    $("#btnFucFirstEdit").parent().css('display', 'block');
                    $("#btnFucFirstEdit").parent().css('text-align', 'center');

                    $("#btnFucFirstDelete").parent().css('display', 'none');
                }


                if (imgUrl2 != "") {

                    $('#imgFucSecond').attr('src', "'" + imgUrl2 + "'");
                }
                else {               
                    $("#fucSecond").replaceWith($("#fucSecond").val('').clone(true));
                    $('#imgFucSecond').attr('src', '/images/upimglogo1.png');

                    $("#btnFucSecondEdit").parent().css('display', 'block');
                    $("#btnFucSecondEdit").parent().css('text-align', 'center');

                    $("#btnFucSecondDelete").parent().css('display', 'none');


                }


                if (imgUrl3 != "") {

                    $('#imgFucThird').attr('src', "'" + imgUrl3 + "'");
                }
                else {
                    $("#fucThird").replaceWith($("#fucThird").val('').clone(true));
                    $('#imgFucThird').attr('src', '/images/upimglogo1.png');

                    $("#btnFucThirdEdit").parent().css('display', 'block');
                    $("#btnFucThirdEdit").parent().css('text-align', 'center');

                    $("#btnFucThirdDelete").parent().css('display', 'none');

                }


                if (imgUrl4 != "") {

                    $('#imgFucFour').attr('src', "'" + imgUrl4 + "'");
                }
                else {
                    $("#fucFour").replaceWith($("#fucFour").val('').clone(true));
                    $('#imgFucFour').attr('src', '/images/upimglogo1.png');

                    $("#btnFucFourEdit").parent().css('display', 'block');
                    $("#btnFucFourEdit").parent().css('text-align', 'center');

                    $("#btnFucFourDelete").parent().css('display', 'none');

                }

                //if (position == "1") {

                //    $("#fucFirst").replaceWith($("#fucFirst").val('').clone(true));
                //    $('#imgFucFirst').attr('src', '/images/upimglogo1.png');

                //    $("#btnFucFirstEdit").parent().css('display', 'block');
                //    $("#btnFucFirstEdit").parent().css('text-align', 'center');

                //    $("#btnFucFirstDelete").parent().css('display', 'none');

                //    $("#divimgFucFirst").css("border", "none");
                //    $("#divimgFucFirstError").css("display", "none");

                //    //SetDefalutImage();
                //}
                //else if (position == "2") {

                //}
                //else if (position == "3") {

                //}
                //else if (position == "4") {

                //}

                alert("delete img successfully");
            }

        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3 + " " + p4;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).Message;
            console.log(err);
        }
    });
};


window.SetDefalutImage = function () {

    var imgFirstUrl = $('#imgFucFirst').attr('src');
    var imgSecondUrl = $('#imgFucSecond').attr('src');
    var imgThirdUrl = $('#imgFucThird').attr('src');
    var imgFourUrl = $('#imgFucFour').attr('src');

    if (imgSecondUrl != "/images/upimglogo1.png") {

        $("#divSetDefalutSecond").css("display", "block");
    }
    if (imgThirdUrl != "/images/upimglogo1.png") {

        $("#divSetDefalutThird").css("display", "block");
    }
    if (imgFourUrl != "/images/upimglogo1.png") {

        $("#divSetDefalutFour").css("display", "block");
    }
}

$("#btnSetDefalutSecond").click(function () {

    var id = addId;
    var category = $("#hdnCateFristLevel").val();
    ChangeDefaultImage(category, "2", id);
});

function ChangeDefaultImage(category, position, id) {

    $.ajax({
        type: "POST",
        url: '/Post/ChangeDefaultImage',
        contentType: "application/json; charset=utf-8",
        processData: false,
        dataType: 'json',
        async: false,
        data: JSON.stringify({ 'category': category, 'position': position, 'id': id }),
        success: function (msg) {

            if (msg = "error") {
                alert("there is problem with deleting image");
            }
            else {
                var imgUrl1 = msg[0];
                var imgUrl2 = msg[2];
                var imgUrl3 = msg[3];
                var imgUrl4 = msg[4];

                if (imgUrl1 != "") {
                    $('#imgFucFirst').attr('src', "'" + imgUrl1 + "'");
                }
                else {
                    $('#imgFucFirst').attr('src', '/images/upimglogo1.png');
                }


                if (imgUrl2 != "") {

                    $('#imgFucSecond').attr('src', "'" + imgUrl2 + "'");
                }
                else {

                    $('#imgFucSecond').attr('src', '/images/upimglogo1.png');
                    $("#divSetDefalutSecond").css("display", "none");
                }


                if (imgUrl3 != "") {

                    $('#imgFucThird').attr('src', "'" + imgUrl3 + "'");
                }
                else {
                    $('#imgFucThird').attr('src', '/images/upimglogo1.png');
                    $("#divSetDefalutThird").css("display", "none");
                }


                if (imgUrl4 != "") {

                    $('#imgFucFour').attr('src', "'" + imgUrl4 + "'");
                }
                else {
                    $('#imgFucFour').attr('src', '/images/upimglogo1.png');
                    $("#divSetDefalutFour").css("display", "none");

                }

            }


        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3 + " " + p4;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).Message;
            console.log(err);
        }
    });
}


/*---------------------------------file uploading functionality---------------------*/

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

    //DisplayRespectiveFields("Properties");
    DisplayRespectiveFields();

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

    //DisplayRespectiveFields("Construction Vehicles");
    DisplayRespectiveFields();
});

//Agricultural Vehicles childs
$("#tabAV a").click(function () {
    var secondLevel = $(this).text();

    $("#cat-nav-l2 span").text(secondLevel);
    $("#hdnCateSecondLevel").val(secondLevel);
    $("#cat-nav-l2").css("display", "inline");

    $("#cat-nav-l4").css("display", "inline");

    $("#divCategories").css('display', 'none');
    //DisplayRespectiveFields("Agricultural Vehicles");
    DisplayRespectiveFields();
});

//Transportation Vehicles
$("#tabTV a").click(function () {
    var secondLevel = $(this).text();

    $("#cat-nav-l2 span").text(secondLevel);
    $("#hdnCateSecondLevel").val(secondLevel);
    $("#cat-nav-l2").css("display", "inline");

    $("#cat-nav-l4").css("display", "inline");

    $("#divCategories").css('display', 'none');
    //DisplayRespectiveFields("Transportation Vehicles");
    DisplayRespectiveFields();
});

//Transportation Vehicles
$("#tabPV a").click(function () {


    var secondLevel = $(this).text();

    $("#cat-nav-l2 span").text(secondLevel);
    $("#hdnCateSecondLevel").val(secondLevel);
    $("#cat-nav-l2").css("display", "inline");

    $("#cat-nav-l4").css("display", "inline");

    $("#divCategories").css('display', 'none');
    //DisplayRespectiveFields("Passenger Vehicles");
    DisplayRespectiveFields();
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

    $("#cat-nav-l4").css("display", "none");
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



    $("#divCategories").css("border", "1px solid #a94442");
    $("#divCategories").css("box-shadow", "0 1px 1px rgba(0,0,0,.075)");

    $("#categoryValidationid").css("display", "block");
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

//function DisplayRespectiveFields(selectedcategory) 
function DisplayRespectiveFields() {


    $(".forallcollapse").css('display', 'none');

    //var selectedCate = selectedcategory;
    var selectedCate = $("#hdnCateFristLevel").val().trim();


    if (selectedCate == "Real Estate") {

        var selectedPrtoperty = $("#hdnCateSecondLevel").val().trim();

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
        $("#forCV").css('display', 'block');
        fillFromJson("CV");

    }
    else if (selectedCate == "Transportation Vehicles") {
        $("#forTV").css('display', 'block');
        fillFromJson("TV");
    }
    else if (selectedCate == "Agricultural Vehicles") {
        $("#forAV").css('display', 'block');
        fillFromJson("AV");

        var selectedSubCategory = $("#hdnCateSecondLevel").val();
        if (selectedSubCategory == "Borewell Machine") {
            $("#divAVPrice").css('display', 'none');
        }
        else {
            $("#divAVPrice").css('display', 'block');
        }

    }
    else if (selectedCate == "Passenger Vehicles") {
        $("#forPV").css('display', 'block');
        fillFromJson("PV");

        var selectedSubCategory = $("#hdnCateSecondLevel").val();
        if (selectedSubCategory == "Cars") {
            $("#pv-model").css('display', 'block');
            $("#pv-year").css('display', 'block');
            $("#pv-fueltype").css('display', 'block');
            $("#pv-kmdriven").css('display', 'block');
        }
        else if (selectedSubCategory == "Bikes") {
            $("#pv-model").css('display', 'block');
            $("#pv-year").css('display', 'block');
            $("#pv-fueltype").css('display', 'none');
            $("#pv-kmdriven").css('display', 'block');
        }
        else {
            $("#pv-model").css('display', 'none');
            $("#pv-year").css('display', 'none');
            $("#pv-fueltype").css('display', 'none');
            $("#pv-kmdriven").css('display', 'none');
        }
    }


    $("#divCategories").css("border", "none");

    $("#categoryValidationid").css("display", "none");

}

function showothercompany() {

    var avSelectedCompnay = $("#AVCompany_list").val();
    if (avSelectedCompnay == "Other") {
        $("#av-othercompany").css("display", "block");
    }
    else {
        $("#av-othercompany").css("display", "none");
    }

    var tvSelectedCompnay = $("#TVCompany_list").val();
    if (tvSelectedCompnay == "Other") {
        $("#tv-othercompany").css("display", "block");
    }
    else {
        $("#tv-othercompany").css("display", "none");
    }


    var cvSelectedCompnay = $("#CVCompany_list").val();
    if (cvSelectedCompnay == "Other") {
        $("#cv-othercompany").css("display", "block");
    }
    else {
        $("#cv-othercompany").css("display", "none");
    }


    var pvSelectedCompnay = $("#PVCompany_list").val();
    if (pvSelectedCompnay == "Other") {
        $("#pv-othercompany").css("display", "block");
    }
    else {
        $("#pv-othercompany").css("display", "none");
    }


}

//--------------------binding json data ---------------------------------------//

function fillFromJson(selectedType) {

    var selectedCategory = $("#hdnCateFristLevel").val();
    var selectedSubCategory = $("#hdnCateSecondLevel").val();


    $("#AVCompany_list").find('option').remove();
    $("#AVCompany_list").append('<option value="">Select</option>');

    $("#CVCompany_list").find('option').remove();
    $("#CVCompany_list").append('<option value="">Select</option>');

    $("#TVCompany_list").find('option').remove();
    $("#TVCompany_list").append('<option value="">Select</option>');

    $("#PVCompany_list").find('option').remove();
    $("#PVCompany_list").append('<option value="">Select</option>');

    var VehiclesColl = new Array();


    $.ajax({
        url: '/Scripts/Json/categories.json',
        dataType: 'json',
        async: false,
        success: function (data) {
            $.each(data, function (i, field) {
                VehiclesColl.push(field);
            });
        }
    });

    //$.get('/Scripts/Json/categories.json', function (data) {
    //    $.each(data, function (i, field) {
    //        VehiclesColl.push(field);
    //        console.log(field.name);
    //    });

    //});


    var selectedVehicle = VehiclesColl.filter(a=>a.name == selectedCategory);

    var selectedModel = selectedVehicle[0].VehicleType.filter(v=>v.name == selectedSubCategory);


    $.each(selectedModel[0].VehicleModel, function (i, field) {
        {
            if (selectedType == "AV") {

                $("#AVCompany_list").append("<option>" + field.name + "</option>");
            }
            else if (selectedType == "CV") {

                $("#CVCompany_list").append("<option>" + field.name + "</option>");
            }
            else if (selectedType == "TV") {

                $("#TVCompany_list").append("<option>" + field.name + "</option>");
            }
            else if (selectedType == "PV") {

                $("#PVCompany_list").append("<option>" + field.name + "</option>");
            }

        }
    });
}

function fillModels() {

    var selectedSubCategory = $("#hdnCateSecondLevel").val();
    var selectedCategory = "";

    if (selectedSubCategory == "Cars") {

        selectedCategory = "Cars Models";
    }
    else if (selectedSubCategory == "Bikes") {

        selectedCategory = "Bikes Models";
    }


    var selectedSubCategory = $("#PVCompany_list").val();

    $("#PVModel_list").find('option').remove();
    $("#PVModel_list").append('<option value="">Select</option>');


    var VehiclesColl = new Array();


    $.ajax({
        url: '/Scripts/Json/categories.json',
        dataType: 'json',
        async: false,
        success: function (data) {
            $.each(data, function (i, field) {
                VehiclesColl.push(field);
            });
        }
    });


    var selectedVehicle = VehiclesColl.filter(a=>a.name == selectedCategory);

    var selectedModel = selectedVehicle[0].VehicleType.filter(v=>v.name == selectedSubCategory);


    $.each(selectedModel[0].VehicleModel, function (i, field) {
        {
            $("#PVModel_list").append("<option>" + field.name + "</option>");

        }
    });
}

//---------------------binding json data---------------------------------------//


//---------------------Binding edit options---------------------------------------//
window.BindEdit = function () {

    $(".forallcollapse").css('display', 'none');


    var selectedCate = $("#hdnCateFristLevel").val().trim();
    var secondCategory = $("#hdnCateSecondLevel").val().trim();

    $("#cat-nav-l1 span").text(selectedCate);

    $("#cat-nav-l1").css("display", "inline");

    $("#cat-nav-l2 span").text(secondCategory);
    $("#cat-nav-l2").css("display", "inline");

    $("#cat-nav-l4").css("display", "inline");

    $("#divCategories").css('display', 'none');



    if (selectedCate == "Real Estate") {

        var selectedPrtoperty = $("#hdnCateSecondLevel").val().trim();

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
        $("#forCV").css('display', 'block');
        fillFromJson("CV");

    }
    else if (selectedCate == "Transportation Vehicles") {
        $("#forTV").css('display', 'block');
        fillFromJson("TV");
    }
    else if (selectedCate == "Agricultural Vehicles") {
        $("#forAV").css('display', 'block');
        fillFromJson("AV");

        var selectedSubCategory = $("#hdnCateSecondLevel").val();
        if (selectedSubCategory == "Borewell Machine") {
            $("#divAVPrice").css('display', 'none');
        }
        else {
            $("#divAVPrice").css('display', 'block');
        }

    }
    else if (selectedCate == "Passenger Vehicles") {
        $("#forPV").css('display', 'block');
        fillFromJson("PV");

        var selectedSubCategory = $("#hdnCateSecondLevel").val();
        if (selectedSubCategory == "Cars") {
            $("#pv-model").css('display', 'block');
            $("#pv-year").css('display', 'block');
            $("#pv-fueltype").css('display', 'block');
            $("#pv-kmdriven").css('display', 'block');
        }
        else if (selectedSubCategory == "Bikes") {
            $("#pv-model").css('display', 'block');
            $("#pv-year").css('display', 'block');
            $("#pv-fueltype").css('display', 'none');
            $("#pv-kmdriven").css('display', 'block');
        }
        else {
            $("#pv-model").css('display', 'none');
            $("#pv-year").css('display', 'none');
            $("#pv-fueltype").css('display', 'none');
            $("#pv-kmdriven").css('display', 'none');
        }
    }


    $("#divCategories").css("border", "none");

    $("#categoryValidationid").css("display", "none");

    showothercompany();
    ShowPlaceData();
}


function ShowPlaceData() {
    $("#user-district").css("display", "block");
    $("#user-mandal").css("display", "block");
    $("#user-localarea").css("display", "block");
}

window.loadDistrict = function () {
    var selectedState = $("#State").val();

    if (selectedState != "") {


        //var selectedModel = selectedVehicle[0].VehicleType.filter(v=>v.name == selectedSubCategory);   

        var locations = new Array();
        var districs = new Array();
        $.ajax({
            url: '/Scripts/Json/location1.json',
            dataType: 'json',
            async: false,
            success: function (data) {
                $.each(data, function (i, field) {
                    locations.push(field);
                });
            }
        });

        var selectedVehicle = locations.filter(a=>a.name == selectedState);

        $.each(selectedVehicle[0].District, function (i1, field1) {

            districs.push(field1.name);
        });

        $("#District").autocomplete({
            source: districs

            //    function (request, response) {
            //    var filteredArray = $.map(districs, function (item) {
            //        if (item.toLowerCase().startsWith(request.term.toLowerCase())) {
            //            return item;
            //        }
            //        else {
            //            return null;
            //        }
            //    });
            //    response(filteredArray);
            //}


            ,
            minLength: 1,
            autoFocus: true,
            scroll: true,
            matchContains: true,
            minChars: 2,
            autoFill: true,
            mustMatch: false,
            cacheLength: 20,
            max: 20,
            close: function () {
                $(this).blur();
            }
        }).focus(function () {
            $(this).data("uiAutocomplete").search('e');
        });

        $("#user-district").css("display", "block");

    }
    else {
        $("#user-district").css("display", "none");
        $("#District").val("");
        $("#user-mandal").css("display", "none");
        $("#Mandal").val("");
        $("#user-localarea").css("display", "none");
        $("#LocalArea").val("");
    }
}

window.loadMandal = function () {

    var selectedDistric = $("#District").val();

    if (selectedDistric != "") {


        //var selectedModel = selectedVehicle[0].VehicleType.filter(v=>v.name == selectedSubCategory);   

        var locations = new Array();
        var mandals = new Array();
        $.ajax({
            url: '/Scripts/Json/location1.json',
            dataType: 'json',
            async: false,
            success: function (data) {
                $.each(data, function (i, field) {
                    $.each(field.District, function (i1, field1) {
                        locations.push(field1);

                    });

                });
            }
        });

        var selectedStae = locations.filter(a=>a.name == selectedDistric);

        //var selectedVehicle = selectedStae[0].District.filter(a=>a.name == selectedDistric);

        $.each(selectedStae[0].Mondal, function (i1, field1) {

            mandals.push(field1.name);
        });

        $("#Mandal").autocomplete({
            source: mandals

            //function (request, response) {
            //var filteredArray = $.map(mandals, function (item) {
            //    if (item.toLowerCase().startsWith(request.term.toLowerCase())) {
            //        return item;
            //    }
            //    else {
            //        return null;
            //    }
            //});
            //response(filteredArray);
            //}
            ,
            minLength: 1,
            autoFocus: true,
            scroll: true,
            matchContains: true,
            minChars: 2,
            autoFill: true,
            mustMatch: false,
            cacheLength: 20,
            max: 20,
            close: function () {
                $(this).blur();
            }
        }).focus(function () {
            $(this).data("uiAutocomplete").search('e');
        });

        $("#user-mandal").css("display", "block");

    }
    else {
        $("#user-mandal").css("display", "none");
        $("#Mandal").val("");
        $("#user-localarea").css("display", "none");
        $("#LocalArea").val("");
    }
}

//---------------------Binding edit options---------------------------------------//


//---------------------States districts and mandals binding---------------------------------------//

window.loadStates = function () {

    var locations = new Array();
    var states = new Array();
    $.ajax({
        url: '/Scripts/Json/location1.json',
        dataType: 'json',
        async: false,
        success: function (data) {
            $.each(data, function (i, field) {
                locations.push(field);
            });
        }
    });

    $.each(locations, function (i1, field1) {
        states.push(field1.name);
    });

    $("#State").autocomplete({
        source: states

        //function (request, response) {
        //var filteredArray = $.map(states, function (item) {
        //    if (item.toLowerCase().startsWith(request.term.toLowerCase())) {
        //        return item;
        //    }
        //    else {
        //        return null;
        //    }
        //});
        //response(filteredArray);

        //}
    ,
        minLength: 1, autoFocus: true,
        scroll: true,
        matchContains: true,
        minChars: 2,
        autoFill: true,
        mustMatch: false,
        cacheLength: 20,
        max: 20,
        close: function () {
            //$(this).blur();
            $("#District").focus();
        }
    }).focus(function () {
        $(this).data("uiAutocomplete").search('e');
    });
}

function getDistricts() {

    var selectedState = $("#State").val();
    $("#District").val("");
    if (selectedState != "") {


        //var selectedModel = selectedVehicle[0].VehicleType.filter(v=>v.name == selectedSubCategory);   

        var locations = new Array();
        var districs = new Array();
        $.ajax({
            url: '/Scripts/Json/location1.json',
            dataType: 'json',
            async: false,
            success: function (data) {
                $.each(data, function (i, field) {
                    locations.push(field);
                });
            }
        });

        var selectedVehicle = locations.filter(a=>a.name == selectedState);

        $.each(selectedVehicle[0].District, function (i1, field1) {

            districs.push(field1.name);
        });

        $("#District").autocomplete({
            source: districs

            //    function (request, response) {
            //    var filteredArray = $.map(districs, function (item) {
            //        if (item.toLowerCase().startsWith(request.term.toLowerCase())) {
            //            return item;
            //        }
            //        else {
            //            return null;
            //        }
            //    });
            //    response(filteredArray);
            //}


            ,
            minLength: 1,
            autoFocus: true,
            scroll: true,
            matchContains: true,
            minChars: 2,
            autoFill: true,
            mustMatch: false,
            cacheLength: 20,
            max: 20,
            close: function () {
                //$(this).blur();
                $("#Mandal").focus();
            }
        }).focus(function () {
            $(this).data("uiAutocomplete").search('e');
        });

        $("#user-district").css("display", "block");

    }
    else {
        $("#user-district").css("display", "none");
        $("#District").val("");
        $("#user-mandal").css("display", "none");
        $("#Mandal").val("");
        $("#user-localarea").css("display", "none");
        $("#LocalArea").val("");
    }
    //var selectedState = $("#State").val();
    //if (selectedState != "") {
    //    $("#user-district").css("display", "block");
    //}
    //else {

    //    $("#user-district").css("display", "none");
    //    $("#user-mandal").css("display", "none");
    //    $("#user-localarea").css("display", "none");
    //    $("#LocalArea").val("");
    //}
}

function getMandal() {


    var selectedDistric = $("#District").val();
    $("#Mandal").val("");
    if (selectedDistric != "") {


        //var selectedModel = selectedVehicle[0].VehicleType.filter(v=>v.name == selectedSubCategory);   

        var locations = new Array();
        var mandals = new Array();
        $.ajax({
            url: '/Scripts/Json/location1.json',
            dataType: 'json',
            async: false,
            success: function (data) {
                $.each(data, function (i, field) {
                    $.each(field.District, function (i1, field1) {
                        locations.push(field1);

                    });

                });
            }
        });

        var selectedStae = locations.filter(a=>a.name == selectedDistric);

        //var selectedVehicle = selectedStae[0].District.filter(a=>a.name == selectedDistric);

        $.each(selectedStae[0].Mondal, function (i1, field1) {

            mandals.push(field1.name);
        });

        $("#Mandal").autocomplete({
            source: mandals

            //function (request, response) {
            //var filteredArray = $.map(mandals, function (item) {
            //    if (item.toLowerCase().startsWith(request.term.toLowerCase())) {
            //        return item;
            //    }
            //    else {
            //        return null;
            //    }
            //});
            //response(filteredArray);
            //}
            ,
            minLength: 1,
            autoFocus: true,
            scroll: true,
            matchContains: true,
            minChars: 2,
            autoFill: true,
            mustMatch: false,
            cacheLength: 20,
            max: 20,
            close: function () {

                $("#LocalArea").focus();
                //$(this).blur();
            }
        }).focus(function () {
            $(this).data("uiAutocomplete").search('e');
        });

        $("#user-mandal").css("display", "block");

    }
    else {
        $("#user-mandal").css("display", "none");
        $("#Mandal").val("");
        $("#user-localarea").css("display", "none");
        $("#LocalArea").val("");
    }

    //var selectedState = $("#District").val();
    //if (selectedState != "") {
    //    $("#user-mandal").css("display", "block");
    //}
    //else {

    //    $("#user-mandal").css("display", "none");
    //    $("#user-localarea").css("display", "none");
    //    $("#LocalArea").val("");
    //}
}

function getLocal() {
    $("#LocalArea").val("");
    var selectedState = $("#Mandal").val();
    if (selectedState != "") {
        $("#user-localarea").css("display", "block");
    }
    else {
        $("#LocalArea").val("");
        $("#user-localarea").css("display", "none");
    }
}
//---------------------States districts and mandals binding---------------------------------------//


//-------------------------- for properties validation -------------------------------//
//$("#btnSubmit").click(function (e) 
function testFunction() {

    var isValid = "true";

    var hiddenElements = $(':hidden');

    hiddenElements.each(function () {
        $(this).prop('required', false);
        $(this).removeAttr('pattern', false);
    })



    var selectedCategory = $("#hdnCateFristLevel").val();
    var selectedSubCategory = $("#hdnCateSecondLevel").val();


    if ($("#txtTitle").val() != "") {
        if (selectedCategory == "" || selectedSubCategory == "") {

            $("#divCategories").css("border", "1px solid #a94442");
            $("#divCategories").css("box-shadow", "0 1px 1px rgba(0,0,0,.075)");
            $("#divCategories").focusin();

            $("#categoryValidationid").css("display", "block");
            $("#categoryValidationid").css("color", "#a94442");

            //$('html, body').animate({
            //    scrollTop: $("#divCategories").offset().top
            //}, 2000);

            isValid = "false";

            $(window).scrollTop($('#scrolltoCat').offset().top);

            alert("test");
            return;

        }
    }


    if ($("#txtAddDetails").val() != "") {

        var firstImgvalue = $("#imgFucFirst").attr('src');

        if (firstImgvalue == "/images/upimglogo1.png") {

            showerrorImg1();

            isValid = "false";

            return false;

        }
    }

    var inputElements = $('input:not(:hidden)');

    inputElements.each(function () {
        if ($(this).attr("id") != "LocalArea") {
            if ($(this).val() == "") {
                isValid = "false";
                $(this).focus();
            }
        }

    });

    var selectElements = $('select:not(:hidden)');

    selectElements.each(function () {
        if ($(this).val() == "") {
                  
            isValid = "false";
          
        }

    });

    if ($("#txtAddDetails").val() == "") {

        isValid = "false";
    }

    if (isValid == "true") {


        var inputElements1 = $('input');

        inputElements1.each(function () {
            $(this).removeAttr('required');

        });

        var selectElements1 = $('select');

        selectElements1.each(function () {
            $(this).removeAttr('required');
          
        });

        alert("hei");
        //$(".loader-wrap").css("display", "block");
    }
    else {
        return false;
    }


    //alert($('#forProperties').css('display') == 'none');
};

function showerrorImg1() {
    $("#divimgFucFirst").css("border", "1px solid #a94442");
    $("#divimgFucFirst").css("box-shadow", "0 1px 1px rgba(0,0,0,.075)");
    $("#divimgFucFirst").focusin();

    $("#divimgFucFirstError").css("display", "block");
}

function removeerrorImg1() {

    $("#divimgFucFirst").css("border", "none");
    $("#divimgFucFirstError").css("display", "none");
}

//-------------------------- for properties validation -------------------------------//
