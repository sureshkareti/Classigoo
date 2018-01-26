
$(document).ready(function () {

    $('#frmChangePassword').bootstrapValidator({

        fields: {
            txtConfirmPasscode: {
                validators: {
                    identical: {
                        field: 'txtPasscode',
                        message: 'The password and its confirm are not the same'
                    }
                }
            }
        }
    });
});
