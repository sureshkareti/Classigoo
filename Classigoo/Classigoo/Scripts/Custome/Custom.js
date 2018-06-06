function CheckLoginType()
{
    var emailorphone = $('input[name="email-phone"]').val();
    var EmailReg = /[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$/;
    var IndNumReg = /^\d{10}$/;
    var LoginType = "";
    if (emailorphone != '') {
        if (EmailReg.test(emailorphone)) {
            LoginType = "email";
            $("#logintype").val(LoginType);
            
        }
        else if (IndNumReg.test(emailorphone)) {
            LoginType = "phone";
            $("#logintype").val(LoginType);
           
        }
    }
}
function CheckUser(profile, type) {
    jQuery.support.cors = true;
    var user = {};
    var id="";
    if (type =='Gmail')
    {
        user.Name=profile.getName();
        user.Type= type;
        user.Email = profile.getEmail();
        id= user.Email;
        // url = 'http://localhost:51797/api/UserApi/CheckUser/?id=' + user.Email + '&type=Gmail';
    }
    else if (type == 'Fb')
    {
        user.Name=profile.name;
        user.Type = type;
        user.FbId = profile.id;
        id= user.FbId;
        // url = 'http://localhost:51797/api/UserApi/CheckUser/?id=' + user.FbId + '&type=Fb';
    }
    $.ajax({
        url: '/User/IsUserExist/',
        type: 'GET',
        data:{"id":id,"type":user.Type},
        success: function (data) {
            if (data == "00000000-0000-0000-0000-000000000000")
            {
                RegisterUser(user);  
            }
            else
            {
                console.log("already registered");
                window.location.href = "/User/Home";
            }
        },
        error: function (response) {
            console.log(response.responseText);
        },
    });
}
function RegisterUser(user)
{
    $.ajax({
        url: '/User/AddUser/',
        type: 'GET',
        data: user,
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            if (data =="True") {
                window.location.href = "/User/Home";
            }
        },
        error: function (x, y, z) {
            console.log(x + '\n' + y + '\n' + z);
        }
    });
}

