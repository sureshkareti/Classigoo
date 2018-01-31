function RegisterUser(profile,type) {
    jQuery.support.cors = true;
    var user = {};
    if (type = "Gmail")
    {
      
        user.Name=profile.getName();
        user.Type= type;
        user.Email = profile.getEmail();
       
    }
    else if (type = "Fb")
    {
        
           user.Name=profile.name;
           user.Type = type;
           user.FbId = profile.id;
       
    }   
    $.ajax({
        url: 'http://localhost:51797/api/UserApi',
        type: 'POST',
        data: JSON.stringify(user),
        contentType: "application/json;charset=utf-8",
        success: function (data) {

           
            window.location.href = '<%= Url.Action("User", "Home") %>';
           
        },
        error: function (x, y, z) {
            console.log(x + '\n' + y + '\n' + z);
        }
    });
}