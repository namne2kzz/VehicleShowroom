var category = {
    init: function () {
        category.registerEvents();
        category.deleteCustomer();
    },
    registerEvents: function () {
        $('.ui-btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');

            $.ajax({
                url: "/Showroom/Category/ChangeStatus",
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
                    title: 'Delete Category',
                    text: "Are you sure to delete this category?",
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
                            url: '/Showroom/Category/Delete',
                            data: { id: $(this).data('id') },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {
                                    Swal.fire(
                                        'Successfully!',
                                        'Delete successfully this category',
                                        'success'
                                    );
                                    window.location.href = "/Showroom/Category/Index"
                                }

                            }
                        })

                    }
                })
            })
        })

    },

}
category.init();