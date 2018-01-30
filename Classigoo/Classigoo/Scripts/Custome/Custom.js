function RegisterUser(profile) {
    jQuery.support.cors = true;
    var user = {
        
        Name: profile.getName(),
        Type:'Gmail',
        Email: profile.getEmail()
    };

    $.ajax({
        url: 'http://localhost:51797/api/UserApi',
        type: 'POST',
        data: JSON.stringify(user),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            WriteResponse(data);
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}