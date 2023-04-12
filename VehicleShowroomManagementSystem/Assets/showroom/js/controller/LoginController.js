var login = {
    init: function () {
        login.registerEvents();
    },
    registerEvents: function () {
        $('#btnSubmit').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                url: '/Showroom/Login/ForgotPassword',
                data: { email: $('#txtEmail').val() },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.Status == true) {
                        window.location.href = '/Showroom/Login/Index';
                    }
                    else {
                        Swal.fire(
                            'Is The Email correct?',
                            'OOP Something went wrong with your Email. Please Check again!',
                            'question'
                        )
                    }
                },
            })
        });

        $('#txtUserName').focusout(function () {
            $.ajax({
                url: '/Showroom/Login/FillPassword',
                data: { userName: $('#txtUserName').val() },
                type: 'POST',
                dataType: 'json',
                success: function (res) {
                    if (res.Status == true) {
                        $('#txtPassword').val(res.Data);                      
                    }
                }
            })
        })
    }
}
login.init();