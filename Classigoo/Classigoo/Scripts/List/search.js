    $(document).ready(function () {
        var categoryColl = new Array();
        var locationColl = new Array();
        var searchSource = new Array();
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
    });