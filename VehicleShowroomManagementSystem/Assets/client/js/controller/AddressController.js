var address = {
    init: function () {
        address.registerEvents();
        address.deleteEmployee();
        address.loadProvince();
    },
    registerEvents: function () {
        $('.btnStatus').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');

            $.ajax({
                url: "/Address/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        btn.text('Default');
                    }
                    else {
                        btn.text('Extra');
                    }
                }
            });
        });
        $('#ddlProvince').off('change').on('change', function () {
            var id = $(this).val();
            console.log(id);
            if (id != '') {
                address.loadDistrict(id)
              
            }
            else {
                $('#ddlDistrict').html('');
            }
        });
     
    },
    loadProvince: function () {

        $.ajax({
            url: '/Address/LoadProvince',
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

    loadDistrict: function (id) {

        $.ajax({
            url: '/Address/LoadDistrict',
            data: { provinceId: id },
            type: 'POST',
            dataType: 'json',
            success: function (res) {
                if (res.status == true) {
                    var html = '<option value="" selected="true" disabled="disabled">---Select District---</option>';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.Name + '">' + item.Name + '</option>'
                    })
                    $('#ddlDistrict').html(html);
                }
            }

        });
    },
    deleteEmployee: function () {
        $('.btnDelete').each(function () {
            $(this).off('click').on('click', function (e) {
                e.preventDefault();
                Swal.fire({
                    title: 'Delete Address',
                    text: "Are you sure to delete this Address?",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes',
                    cancelButtonText: 'Cancle',
                    customClass: 'swal-wide'
                }).then((result) => {
                    if (result.isConfirmed) {

                        $.ajax({
                            url: '/Address/Delete',
                            data: { id: $(this).data('id') },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {
                                    Swal.fire(
                                        'Successfully!',
                                        'Delete successfully this Address',
                                        'success'
                                    );
                                    window.location.href = "/Address/Index";
                                }

                            }
                        })

                    }
                })
            })
        })

    },

}
address.init();