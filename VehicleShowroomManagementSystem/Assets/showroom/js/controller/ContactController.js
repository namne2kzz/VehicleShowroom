
var contact = {
    init: function () {
        contact.registerEvents();
        contact.deleteCustomer();
       
    },
    registerEvents: function () {
        $('.ui-btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');

            $.ajax({
                url: "/Showroom/Contact/ChangeStatus",
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
                    title: 'Delete Contact Information',
                    text: "Are you sure to delete this Contact Information?",
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
                            url: '/Showroom/Contact/Delete',
                            data: { id: $(this).data('id') },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {
                                    Swal.fire(
                                        'Successfully!',
                                        'Delete successfully this Contact Information',
                                        'success'
                                    );
                                    window.location.href = "/Showroom/Contact/Index"
                                }

                            }
                        })

                    }
                })
            })
        })

    },

}
contact.init();