var order = {
    init: function () {
        order.registerEvents();
        order.deleteEmployee();
    },
    registerEvents: function () {
        
    },
    deleteEmployee: function () {
        $('.btnDelete').each(function () {
            $(this).off('click').on('click', function (e) {
                e.preventDefault();
                Swal.fire({
                    title: 'Delete Order',
                    text: "Are you sure to delete this Order?",
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
                            url: '/BranchDealer/Order/Delete',
                            data: { id: $(this).data('id') },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {
                                    Swal.fire(
                                        'Successfully!',
                                        'Delete successfully this Order',
                                        'success'
                                    );
                                    window.location.href = "/BranchDealer/Order/Index"
                                }

                            }
                        })

                    }
                })
            })
        })

    },

}
order.init();