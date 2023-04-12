var contact = {
    init: function () {
        contact.loadProvince();
    },
   
    loadProvince: function () {

        $.ajax({
            url: '/Contact/LoadProvince',
            type: 'POST',
            dataType: 'json',
            success: function (res) {
                if (res.status == true) {
                    var html = '<option value="" selected="true" disabled="disabled">---Select Province---</option>';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.Name + '">' + item.Name + '</option>'
                    })
                    $('#ddlProvince').html(html);
                }
            }

        });
    },  

}
contact.init();