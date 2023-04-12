var vehicleRegistration = {
    init: function () {
        vehicleRegistration.registerEvents();
        vehicleRegistration.deleteEmployee();
    },
    registerEvents: function () {
        $('.ui-btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');

            $.ajax({
                url: "/BranchDealer/VehicleRegistration/ChangeStatus",
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
    deleteEmployee: function () {
        $('.btnDelete').each(function () {
            $(this).off('click').on('click', function (e) {
                e.preventDefault();
                Swal.fire({
                    title: 'Delete Vehicle Registration Data',
                    text: "Are you sure to delete this Vehicle Registration Data?",
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
                            url: '/BranchDealer/VehicleRegistration/Delete',
                            data: { id: $(this).data('id') },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {
                                    Swal.fire(
                                        'Successfully!',
                                        'Delete successfully this Vehicle Registration Data',
                                        'success'
                                    );
                                    window.location.href = "/BranchDealer/VehicleRegistration/Index"
                                }

                            }
                        })

                    }
                })
            })
        })

    },

}
vehicleRegistration.init();