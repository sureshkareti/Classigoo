﻿    $(document).ready(function () {
        var categoryColl = new Array();
        var locationColl = new Array();
        var searchSource = new Array();
        var category = "";
        FillCategories();
        FillLocations();
        FillSearchBox();
        function FillSearchBox()
        {
            searchSource = jQuery.unique(searchSource);
            jQuery.getScript("/Scripts/ScriptsNew/jquery-ui.min.js", function (data, status, jqxhr) {
                $('#listing_keword').autocomplete({
                    source: function (request, response) {
                        var filteredArray = $.map(searchSource, function (item) {
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
                    matchContains: true,
                    minChars: 2,
                    autoFill: true,
                    mustMatch: false,
                    cacheLength: 20,
                    max: 20
                }).focus(function(){            
                    // The following works only once.
                    // $(this).trigger('keydown.autocomplete');
                    // As suggested by digitalPBK, works multiple times
                    // $(this).data("autocomplete").search($(this).val());
                    // As noted by Jonny in his answer, with newer versions use uiAutocomplete
                    $(this).data("uiAutocomplete").search('ma');
                });;
            });

        }
        function FillCategories()
        {
            $.ajax({
                url: '/Scripts/Json/categories.json',
                dataType: 'json',
                async: false,
                success: function (data) {
                    $.each(data, function (i, field) {
                        if (field.name !== "Cars Models" && field.name !== "Bikes Models")
                        categoryColl.push(field);
                    });
                }
            });
            $.each(categoryColl, function (i, field) {
                $("#listing_catagory_list").append("<option>" + field.name + "</option>");
                $("#listing_catagory").append("<option>" + field.name + "</option>");
               // searchSource.push(field.name);
                var VehicleTypeColl=field.VehicleType;
                $.each(VehicleTypeColl,function(j,vehicleType)
                {
                    searchSource.push(vehicleType.name);
                    var vehicleModelColl=vehicleType.VehicleModel;
                    $.each(vehicleModelColl,function(k,vehicleModel)
                    {
                        searchSource.push(vehicleModel.name);
                    });
                });
            });


            $("#listing_catagory_list").selectron();
           

            $("#listing_rent_list").selectron();
        }
        function FillLocations()
        {
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
               // searchSource.push(field.name);
                locationColl.push(field.name);
               // locationColl.push({name:field.name, type:"state"});

                var districtColl=field.District;
                $.each(districtColl,function(j,district)
                {
                   // searchSource.push(district.name);
                    locationColl.push(district.name);
                    //locationColl.push({ name: district.name, type: "district" });
                    var mondalColl=district.Mondal;
                    $.each(mondalColl,function(k,mondal)
                    {
                       // searchSource.push(mondal.name);
                        locationColl.push(mondal.name);
                        //locationColl.push({ name: mondal.name, type: "mondal" });
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
     

                }).

                    focus(function () {
                    // The following works only once.
                    // $(this).trigger('keydown.autocomplete');
                    // As suggested by digitalPBK, works multiple times
                    // $(this).data("autocomplete").search($(this).val());
                    // As noted by Jonny in his answer, with newer versions use uiAutocomplete
                    $(this).data("uiAutocomplete").search('te');
                })
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
                        if (category == "Construction Vehicles" || category == "Agricultural Vehicles" || category == "Transportation Vehicles")
                        {
                            $("#allvSubCategory").append("<option>" + field.name + "</option>");
                        }
                        else if (category == "Passenger Vehicles")
                        {
                            $("#pvSubCategory").append("<option>" + field.name + "</option>");
                        }
                      
                    }

                });
            }
             filterAdds("",1);
        }
        function HideDivs() {
            $("#re").hide();
            $("#pv").hide();
            $("#allv").hide();
        }
        $("#listing_catagory").change(function () {
            category = $("#listing_catagory").val();
           ShowCategoryFilter(category);
        });
        $("#allvSubCategory, #pvSubCategory").change(function ()
        {
            var currentSubCategory = $(this)[0].id;
            var selectedVehicle = categoryColl.filter(a=>a.name == category);
            var selectedModel = selectedVehicle[0].VehicleType.filter(v=>v.name == $(this).val());
            $("#allvCompany").empty();
            $("#pvCompany").empty();
            $("#allvCompany").append("<option>" + "All" + "</option>");
            $("#pvCompany").append("<option>" + "All" + "</option>");
            $.each(selectedModel[0].VehicleModel, function (i, field) {
                {
                    if (currentSubCategory == "allvSubCategory") {
                        $("#allvCompany").append("<option>" + field.name + "</option>");
                    }
                    else if (currentSubCategory == "pvSubCategory")
                    {
                        $("#pvCompany").append("<option>" + field.name + "</option>");
                    }  
                }
                });
                
        });
        $("#clearfilter").click(function () {
            $('select').prop('selectedIndex', 0);
        });
        $("#divFilter select").change(function () {
            var filterObj = {};
            var priceFrom = $("#priceFrom").val();
            var priceTo=$("#priceTo").val();
            if (priceFrom == "Price From")
            {
                priceFrom = 0;
            }
            else
            {
                priceFrom = priceFrom.substring(1, priceFrom.length);
            }
            if (priceTo == "Price To")
            {
                priceTo = 0;
            }
            else
            {
                priceTo = priceTo.substring(1, priceTo.length);
            }
            
            switch (category) {
                case "Real Estate":
                    filterObj.subCategory = $("#reSubCategory").val();
                    filterObj.furnishing = $("#furnishing").val();
                    filterObj.availability = $("#consructionStatus").val();
                    filterObj.listedBy = $("#listedBy").val();
                    filterObj.squareFeets = $("#builtupArea").val();
                    filterObj.priceFrom = priceFrom;
                    filterObj.priceTo = priceTo;
                    filterObj.bedRooms = $("#bedRooms").val();

                    break;
                case "Construction Vehicles":
                case "Transportation Vehicles":
                case "Agricultural Vehicles":
                    filterObj.priceFrom = priceFrom;
                    filterObj.priceTo = priceTo;
                    filterObj.subCategory = $("#allvSubCategory").val();
                    filterObj.company = $("#allvCompany").val();
                    break;
                case "Passenger Vehicles":
                    filterObj.subCategory = $("#pvSubCategory").val();
                    filterObj.company = $("#pvCompany").val();
                    filterObj.priceFrom = priceFrom;
                    filterObj.PriceTo = priceTo;
                    filterObj.yearFrom = $("#yearFrom").val();
                    filterObj.yearTo = $("#yearTo").val();
                    filterObj.kmFrom = $("#kmFrom").val();
                    filterObj.kmTo = $("#kmTo").val();
                    filterObj.model = $("#model").val();
                    break;
            }
             filterAdds(filterObj,1);
        });
        function filterAdds(selectedValue, pageNum) {

            $.ajax({
                url: '/List/ApplyFilter',
                type: 'GET',
                dataType: "html",
                data: { "filterOptions": JSON.stringify(selectedValue), "pageNum": pageNum, "category": category, "location": $("#listing_location_list").val(), "keyword": $("#listing_keword").val(), "type": $("#listing_rent_listGeneral").val() },
                success: function (data) {
                    $("#content").html(data);
                    $(".bloglisting").css("display", "block");
                    $(".loader-wrap").hide();
                },
                failure: function (response) {
                    console.log(response.responseText);
                },
                error: function (response) {
                    console.log(response.responseText);
                },
                complete: function (data) {

                }

            });
        }

    });