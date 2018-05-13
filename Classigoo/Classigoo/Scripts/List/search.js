    $(document).ready(function () {
        var categoryColl = new Array();
        var locationColl = new Array();
        var searchSource=new Array();
        FillCategories();
        FillLocations();
        FillSearchBox();
        function FillSearchBox()
        {
            searchSource = jQuery.unique(searchSource);
            jQuery.getScript("/Scripts/ScriptsNew/jquery-ui.min.js", function (data, status, jqxhr) {
                $('#listing_keword').autocomplete({
                    source: searchSource, 
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
                    $(this).data("uiAutocomplete").search('te');
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
                        categoryColl.push(field);
                    });
                }
            });
            $.each(categoryColl, function (i, field) {
                $("#listing_catagory_list").append("<option>" + field.name + "</option>");
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
            $.ajax({
                url: '/Scripts/Json/location1.json',
                dataType: 'json',
                async: false,
                success: function (data) {
                    $.each(data, function (i, field) {
                        locationColl.push(field);
                    });
                }
            });
            $.each(locationColl, function (i, field) {
               // searchSource.push(field.name);
                locationColl.push(field.name);
                var districtColl=field.District;
                $.each(districtColl,function(j,district)
                {
                   // searchSource.push(district.name);
                    locationColl.push(district.name);
                    var mondalColl=district.Mondal;
                    $.each(mondalColl,function(k,mondal)
                    {
                       // searchSource.push(mondal.name);
                        locationColl.push(mondal.name);
                    });
                });
            });

            jQuery.getScript("/Scripts/ScriptsNew/jquery-ui.min.js", function (data, status, jqxhr) {
                locationColl = jQuery.unique(locationColl);
                $('#listing_location_list').autocomplete({
                    source: locationColl,
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
                    $(this).data("uiAutocomplete").search('te');
                });;
            });
        }
    });