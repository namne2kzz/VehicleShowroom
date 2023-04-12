var about = {
    init: function () {
        about.registerEvents();
        about.deleteCustomer();
    },
    registerEvents: function () {
        $('.ui-btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');

            $.ajax({
                url: "/Showroom/About/ChangeStatus",
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


    },
    deleteCustomer: function () {
        $('.btnDelete').each(function () {
            $(this).off('click').on('click', function (e) {
                e.preventDefault();
                Swal.fire({
                    title: 'Delete About Information',
                    text: "Are you sure to delete this About Information?",
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
                            url: '/Showroom/About/Delete',
                            data: { id: $(this).data('id') },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {
                                    Swal.fire(
                                        'Successfully!',
                                        'Delete successfully this About Information',
                                        'success'
                                    );
                                    window.location.href = "/Showroom/About/Index"
                                }

                            }
                        })

                    }
                })
            })
        })

    },

}
about.init();