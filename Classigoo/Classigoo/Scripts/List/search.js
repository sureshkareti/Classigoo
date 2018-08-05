var categoryColl = new Array();
var locationColl = new Array();
var category = "";
FillCategories();
FillLocations();
function FillCategories() {
    $.ajax({
        url: '/Scripts/Json/categories.json',
        dataType: 'json',
        async: false,
        success: function (data) {
            $.each(data, function (i, field) {
                // if (field.name !== "Cars Models" && field.name !== "Bikes Models")
                categoryColl.push(field);
            });
        }
    });
    $.each(categoryColl, function (i, field) {
        if (field.name !== "Cars Models" && field.name !== "Bikes Models") {
            $("#listing_catagory_list").append("<option value=\"" + field.name + "\">" + field.name + "</option>");
            $("#listing_catagory").append("<option value=\"" + field.name + "\">" + field.name + "</option>");
        }
        // searchSource.push(field.name);
        // var VehicleTypeColl = field.VehicleType;
        //  $.each(VehicleTypeColl, function (j, vehicleType) {
        //searchSource.push(vehicleType.name);
        //  var vehicleModelColl = vehicleType.VehicleModel;
        //  $.each(vehicleModelColl, function (k, vehicleModel) {
        // searchSource.push(vehicleModel.name);
        // });
        // });
    });


    //$("#listing_catagory_list").selectron();


    //$("#listing_rent_list").selectron();
}
function FillLocations() {
    var locationSource = new Array();
    $.ajax({
        url: '/Scripts/Json/location1.json',
        dataType: 'json',
        async: false,
        success: function (data) {
            $.each(data, function (i, field) {
                locationSource.push(field);
            });
        }
    });
    $.each(locationSource, function (i, field) {
        locationColl.push(field.name);
        var districtColl = field.District;
        $.each(districtColl, function (j, district) {
            locationColl.push(district.name);
          var mondalColl = district.Mondal;
            $.each(mondalColl, function (k, mondal) {
              locationColl.push(mondal.name);
               
            });
        });
    });

    jQuery.getScript("/Scripts/ScriptsNew/jquery-ui.min.js", function (data, status, jqxhr) {
        locationColl = jQuery.unique(locationColl);
        $('#listing_location_list').autocomplete({
            source: function (request, response) {
                var filteredArray = $.map(locationColl, function (item) {
                    if (item.toLowerCase().startsWith(request.term.toLowerCase())) {
                        return item;
                    }
                    else {
                        return null;
                    }
                });
                response(filteredArray);
            },
            minLength: 1,
            scroll: true,
            close: function () {
                //$(this).blur();
                //$(this).focusout();
            }
        }).
            focus(function () {
                $(this).data("uiAutocomplete").search('te');

            }).focusout(data, function (event) {
                category = $("#listing_catagory").val();
                // ShowCategoryFilter(category);
                $(".loader-wrap").show();
                //filterAdds("", 1, false);
                if ($("#divFilter").is(":visible")) {
                    Filter();
                }
                else {
                    filterAdds("", 1, false);
                }
            });
    });
}
function ShowCategoryFilter(category) {
    if (category == "Select Category") {
        $("#divFilter").hide();
        $("#clearfilter").hide();
    }
    else {
        HideDivs();
        $("#divFilter").show();
        $("#clearfilter").show();
        if (category == "Real Estate") {
            $("#re").show();
        }
        else if (category == "Passenger Vehicles") {
            $("#pv").show();
        }
        else {
            $("#allv").show();
        }
        var selectedVehicle = categoryColl.filter(a=>a.name == category);
        $("#allvSubCategory").empty();
        $("#pvSubCategory").empty();
        $("#allvSubCategory").append("<option>" + "All" + "</option>");
        $("#pvSubCategory").append("<option>" + "All" + "</option>");
        $("#allvCompany").empty();
        $("#pvCompany").empty()
        $("#allvCompany").append("<option>" + "All" + "</option>");
        $("#pvCompany").append("<option>" + "All" + "</option>");
        $.each(selectedVehicle[0].VehicleType, function (i, field) {
            {
                if (category == "Construction Vehicles" || category == "Agricultural Vehicles" || category == "Transportation Vehicles") {
                    $("#allvSubCategory").append("<option>" + field.name + "</option>");
                }
                else if (category == "Passenger Vehicles") {
                    $("#pvSubCategory").append("<option>" + field.name + "</option>");
                }

            }

        });
    }
    //$(".loader-wrap").show();
    //filterAdds("", 1, false);
}
function HideDivs() {
    $("#re").hide();
    $("#pv").hide();
    $("#allv").hide();
}
$("#listing_keword").focusout(function () {
    $(".loader-wrap").show();
    category = $("#listing_catagory").val();
    // ShowCategoryFilter(category);
    // filterAdds("", 1, false);
    if ($("#divFilter").is(":visible")) {
        Filter();
    }
    else {
        filterAdds("", 1, false);
    }
});
$("#listing_catagory").change(function () {
    $(".loader-wrap").show();
    category = $("#listing_catagory").val();
    ShowCategoryFilter(category);
    ShowSubCatImgs();
   filterAdds("", 1, false);
});
$("#listing_rent_listGeneral").change(function () {
    $(".loader-wrap").show();
    category = $("#listing_catagory").val();
   // ShowCategoryFilter(category); 
    if ($("#divFilter").is(":visible")) {
        Filter();
    }
    else
    {
       filterAdds("", 1, false);
    }
   
});
$("#allvSubCategory, #pvSubCategory").change(function () {
    FillCompany($(this)[0].id, $(this).val());
});
$("#pvCompany").change(function () {
    var subCategory = $("#pvSubCategory").val();
    var selectedVehicle = "";
    if (subCategory == "Bikes" || subCategory == "Cars") {
        if (subCategory == "Bikes") {
            selectedVehicle = categoryColl.filter(a=>a.name == "Bikes Models");
        }
        else if (subCategory == "Cars") {
            selectedVehicle = categoryColl.filter(a=>a.name == "Cars Models");
        }
        var selectedVType = selectedVehicle[0].VehicleType.filter(v=>v.name == $(this).val());
        $("#model").empty();
        $("#model").append("<option>" + "All" + "</option>");
        if (selectedVType.length != 0) {
            $.each(selectedVType[0].VehicleModel, function (i, field) {
                {
                    $("#model").append("<option>" + field.name + "</option>");
                }
            });
        }
    }

});
$("#clearfilter").click(function () {
    $('select').prop('selectedIndex', 0);
});
$("#divFilter select").change(function () {
    Filter();
});
function filterAdds(selectedValue, pageNum) {

    $.ajax({
        url: '/List/ApplyFilter',
        type: 'GET',
        dataType: "html",
        data: { "filterOptions": JSON.stringify(selectedValue), "pageNum": pageNum, "category": $("#listing_catagory").val(), "location": $("#listing_location_list").val(), "keyword": $("#listing_keword").val(), "type": $("#listing_rent_listGeneral").val(), "isSearchFrmHomePage": false },
        success: function (data) {
            $("#content").html(data);
            $(".bloglisting").css("display", "block");
            $(".loader-wrap").hide();
        },
        failure: function (response) {
            console.log(response.responseText);
            $(".loader-wrap").hide();
        },
        error: function (response) {
            console.log(response.responseText);
            $(".loader-wrap").hide();
        },
        complete: function (data) {

        }

    });
}
$('.scrollimg').click(function () {
   var currentCat = $(this)[0].getAttribute("data-category");
    var currentSubCat = $(this)[0].getAttribute("data-subcategory");
    category = $("#listing_catagory").val();   
    if (category = "Select Category")
    {
        $('#listing_catagory').val(currentCat).attr("selected", "selected");
        category = $("#listing_catagory").val();
        ShowCategoryFilter(category);
        ShowSubCatImgs();
    }
   SetSubCatValue(currentSubCat);
  Filter();
});

function Filter() {
    var filterObj = {};
    if (category == "Real Estate") {
        var bedRooms = $("#bedRooms").val();
        //var priceFrom = $('[id="priceFrom"]').filter(':visible').val();
        //var priceTo = $('[id="priceTo"]').filter(':visible').val();
        //if (priceFrom == "Price From") {
        //    priceFrom = 0;
        //}
        //else {
        //    priceFrom = priceFrom.substring(1, priceFrom.length);
        //    priceFrom = priceFrom.replace(/,/g, '');
        //}
        //if (priceTo == "Price To") {
        //    priceTo = 0;
        //}
        //else {
        //    priceTo = priceTo.substring(1, priceTo.length);
        //    priceTo = priceTo.replace(/,/g, '');
        //}
    }
    switch (category) {
        case "Real Estate":
            filterObj.subCategory = $("#reSubCategory").val();
            filterObj.availability = $("#consructionStatus").val();
            filterObj.listedBy = $("#listedBy").val();
          //  filterObj.priceFrom = priceFrom;
          //  filterObj.priceTo = priceTo;
            filterObj.bedRooms = bedRooms;

            break;
        case "Construction Vehicles":
        case "Transportation Vehicles":
        case "Agricultural Vehicles":
            filterObj.subCategory = $("#allvSubCategory").val();
            filterObj.company = $("#allvCompany").val();
            break;
        case "Passenger Vehicles":
            filterObj.subCategory = $("#pvSubCategory").val();
            filterObj.company = $("#pvCompany").val();
            break;
    }
    $(".loader-wrap").show();
    filterAdds(filterObj, 1, false);
}

function ShowSubCatImgs()
{
    $(".scrollHide").css("display", "none");
    if (category == "Passenger Vehicles") {

        $("#scrollPV").css("display", "block");

        $('.maincategoryPV').carouFredSel({
            prev: '#prevcatPV',
            next: '#nextcatPV',
            width: '100%',
            height: "120px",

            scroll: 1,
            items: {
                //	width: 200,
                //	height: '30%',	//	optionally resize item-height
                visible: {
                    min: 2,
                    max: 8
                }
            }
        });
    }
    else if (category == "Transportation Vehicles") {
        $("#scrollTV").css("display", "block");

        $('.maincategoryTV').carouFredSel({
            prev: '#prevcatTV',
            next: '#nextcatTV',
            width: '100%',
            height: "120px",

            scroll: 1,
            items: {
                //	width: 200,
                //	height: '30%',	//	optionally resize item-height
                visible: {
                    min: 2,
                    max: 8
                }
            }
        });
    }
    else if (category == "Construction Vehicles") {

        $("#scrollCV").css("display", "block");

        $('.maincategoryCV').carouFredSel({
            prev: '#prevcatCV',
            next: '#nextcatCV',
            width: '100%',
            height: "120px",

            scroll: 1,
            items: {
                //	width: 200,
                //	height: '30%',	//	optionally resize item-height
                visible: {
                    min: 2,
                    max: 8
                }
            }
        });
    }
    else if (category == "Agricultural Vehicles") {

        $("#scrollAV").css("display", "block");

        $('.maincategoryAV').carouFredSel({
            prev: '#prevcatAV',
            next: '#nextcatAV',
            width: '100%',
            height: "120px",

            scroll: 1,
            items: {
                //	width: 200,
                //	height: '30%',	//	optionally resize item-height
                visible: {
                    min: 2,
                    max: 8
                }
            }
        });
    }
    else if (category == "Real Estate") {

        $("#scrollRE").css("display", "block");

        $('.maincategoryRE').carouFredSel({
            prev: '#prevcatRE',
            next: '#nextcatRE',
            width: '100%',
            height: "120px",

            scroll: 1,
            items: {
                //	width: 200,
                //	height: '30%',	//	optionally resize item-height
                visible: {
                    min: 2,
                    max: 8
                }
            }
        });
    }
    else {
        $("#scrollAll").css("display", "block");

        $('.maincategory').carouFredSel({
            prev: '#prevcat',
            next: '#nextcat',
            width: '100%',
            height: "120px",

            scroll: 1,
            items: {
                //	width: 200,
                //	height: '30%',	//	optionally resize item-height
                visible: {
                    min: 2,
                    max: 8
                }
            }
        });
    }

}

function FillCompany(currentSubCategoryType, currentSubCategoryValue)
{
    // var currentSubCategory = $(this)[0].id;currentSubCategoryValue$(this).val()
    category = $("#listing_catagory").val();
    var selectedVehicle = categoryColl.filter(a=>a.name == category);
    var selectedModel = selectedVehicle[0].VehicleType.filter(v=>v.name == currentSubCategoryValue);
    $("#allvCompany").empty();
    $("#pvCompany").empty();
    $("#model").empty();
    $("#model").append("<option>" + "All" + "</option>");
    $("#allvCompany").append("<option>" + "All" + "</option>");
    $("#pvCompany").append("<option>" + "All" + "</option>");
    if (selectedModel.length != 0) {
        $.each(selectedModel[0].VehicleModel, function (i, field) {
            {
                if (currentSubCategoryType == "allvSubCategory") {
                    $("#allvCompany").append("<option>" + field.name + "</option>");
                }
                else if (currentSubCategoryType == "pvSubCategory") {
                    $("#pvCompany").append("<option>" + field.name + "</option>");
                }
            }
        });
    }
}

function SetSubCatValue(currentSubCat)
{
    category = $("#listing_catagory").val();
    if (category =="Agricultural Vehicles" ||category=="Construction Vehicles" ||category=="Transportation Vehicles" )
    {
        $('#allvSubCategory').val(currentSubCat.trim()).attr("selected", "selected");
        FillCompany("allvSubCategory", currentSubCat.trim());
    }
    else if (category == "Passenger Vehicles") {
        $('#pvSubCategory').val(currentSubCat.trim()).attr("selected", "selected");
        FillCompany("pvSubCategory", currentSubCat.trim());
    }
    else if (category == "Real Estate")
    {
        $('#reSubCategory').val(currentSubCat.trim()).attr("selected", "selected");
        $('#consructionStatus').val("Construction Status").attr("selected", "selected");
        $('#listedBy').val("Listed By").attr("selected", "selected");
        $('#bedRooms').val("Bed Rooms").attr("selected", "selected");

    }
}

