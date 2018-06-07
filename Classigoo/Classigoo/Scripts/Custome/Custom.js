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
               // console.log("already registered");
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
var clicked = false;//Global Variable
function ClickLogin() {
    clicked = true;
}
function onSignIn(googleUser) {
   if (clicked) 
    CheckUser(googleUser.getBasicProfile(), 'Gmail');
}
// This is called with the results from from FB.getLoginStatus().
function statusChangeCallback(response) {
   // console.log('statusChangeCallback');
    // console.log(response);
    // The response object is returned with a status field that lets the
    // app know the current login status of the person.
    // Full docs on the response object can be found in the documentation
    // for FB.getLoginStatus().
    if (response.status === 'connected') {
        console.log('connected');
        // Logged into your app and Facebook.
        testAPI();
    } else if (response.status === 'not_authorized') {
        console.log('not_authorized');
        // The person is not logged into your app or we are unable to tell.
        //document.getElementById('status').innerHTML = 'Please log ' +
        //'into this app.';
    }
}

// This function is called when someone finishes with the Login
// Button.  See the onlogin handler attached to it in the sample
// code below.
function checkLoginState() {
    FB.getLoginStatus(function (response) {
        statusChangeCallback(response);
    });
}

window.fbAsyncInit = function () {
    FB.init({
        appId: '1786143171680016',
        cookie: true,  // enable cookies to allow the server to access
        // the session
        xfbml: true,  // parse social plugins on this page
        version: 'v2.8' // use graph api version 2.8
    });

    // Now that we've initialized the JavaScript SDK, we call
    // FB.getLoginStatus().  This function gets the state of the
    // person visiting this page and can return one of three states to
    // the callback you provide.  They can be:
    //
    // 1. Logged into your app ('connected')
    // 2. Logged into Facebook, but not your app ('not_authorized')
    // 3. Not logged into Facebook and can't tell if they are logged into
    //    your app or not.
    //
    // These three cases are handled in the callback function.

    //FB.getLoginStatus(function (response) {
    //    statusChangeCallback(response);
    //});

};

// Load the SDK asynchronously
(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "https://connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

// Here we run a very simple test of the Graph API after login is
// successful.  See statusChangeCallback() for when this call is made.
function testAPI() {
    console.log('Welcome!  Fetching your information.... ');
    FB.api('/me', function (response) {

        CheckUser(response, 'Fb');
    });
}
