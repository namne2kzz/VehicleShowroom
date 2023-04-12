var login = {
    init: function () {
        login.registerEvents();
    },
    registerEvents: function () {       

        $('#txtUserName').focusout(function () {
            $.ajax({
                url: '/Customer/FillPassword',
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