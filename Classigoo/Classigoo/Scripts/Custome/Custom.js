
function CheckLoginType()
{
    var emailorphone = $('input[name="email-phone"]').val();
    var EmailReg = /[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}/igm;
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
    var url="";
    if (type =='Gmail')
    {
        user.Name=profile.getName();
        user.Type= type;
        user.Email = profile.getEmail();
        url = 'http://localhost:51797/api/UserApi/CheckUser/?id=' + user.Email + '&type=Gmail';
    }
    else if (type == 'Fb')
    {
        user.Name=profile.name;
        user.Type = type;
        user.FbId = profile.id;
        url = 'http://localhost:51797/api/UserApi/CheckUser/?id=' + user.FbId + '&type=Fb';
    }
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            console.log(data);
            if(data)
            {
                console.log("already registered");
                window.location.href = "/User/Home";
            }
            else
            {
                RegisterUser(user);
            }
        },
        error: function (x, y, z) {
            console.log(x + '\n' + y + '\n' + z);
        }
    });
}
function RegisterUser(user)
{
    $.ajax({
        url: 'http://localhost:51797/api/UserApi/AddUser/?id=' + user,
        type: 'POST',
        data: JSON.stringify(user),
        contentType: "application/json;charset=utf-8",
        success: function (data) {

            window.location.href = "/User/Home";
           
        },
        error: function (x, y, z) {
            console.log(x + '\n' + y + '\n' + z);
        }
    });
}
function CheckEmail()
{
    var email = $('input[name="email"]').val();
    var emailReg = '/^([w-.]+@([w-]+.)+[w-]{2,4})?$/';
    if(!emailReg.test(email) || email == '')
    {
        console.log('Please enter a valid email address.');
        return false;
    }
}
function CheckPhoneNo()
{
    var phone = $('input[name="phone"]').val();
   var intRegex = /[0-9 -()+]+$/;
    if ((phone.length < 6) || (!intRegex.test(phone))) {
        alert('Please enter a valid phone number.');
        return false;
    }
}
