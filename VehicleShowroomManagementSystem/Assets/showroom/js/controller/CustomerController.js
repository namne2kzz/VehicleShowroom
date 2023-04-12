var customer = {
    init: function () {
        customer.registerEvents();
        customer.deleteCustomer();
    },
    registerEvents: function () {
        $('.ui-btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');

            $.ajax({
                url: "/Showroom/Customer/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        btn.text('Active');
                    }
                    else {
                        btn.text('Lock');
                    }
                }
            });
        });

        $('#loadImg').change(function (evt) {
            if (this.files && this.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#showImg').attr('src', e.target.result);
                }
                reader.readAsDataURL(this.files[0]);
            }

        });
    },
    deleteCustomer: function () {
        $('.btnDelete').each(function () {
            $(this).off('click').on('click', function (e) {
                e.preventDefault();
                Swal.fire({
                    title: 'Delete Customer',
                    text: "Are you sure to delete this customer?",
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
                            url: '/Showroom/Customer/Delete',
                            data: { id: $(this).data('id') },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {
                                    Swal.fire(
                                        'Successfully!',
                                        'Delete successfully this customer',
                                        'success'
                                    );
                                    window.location.href = "/Showroom/Customer/Index"
                                }

                            }
                        })

                    }
                })
            })
        })

    },

}
customer.init();