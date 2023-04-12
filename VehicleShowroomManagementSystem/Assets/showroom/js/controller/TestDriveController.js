var testDrive = {
    init: function () {
        testDrive.registerEvents();
        testDrive.deleteCustomer();
    },
    registerEvents: function () {
        
    },
    deleteCustomer: function () {
        $('.btnDelete').each(function () {
            $(this).off('click').on('click', function (e) {
                e.preventDefault();
                Swal.fire({
                    title: 'Delete Test-Drive',
                    text: "Are you sure to delete this Test-Drive Information?",
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
                            url: '/Showroom/TestDrive/Delete',
                            data: { id: $(this).data('id') },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {
                                    Swal.fire(
                                        'Successfully!',
                                        'Delete successfully this Test-Drive Information',
                                        'success'
                                    );
                                    window.location.href = "/Showroom/TestDrive/Index"
                                }

                            }
                        })

                    }
                })
            })
        })

    },

}
testDrive.init();