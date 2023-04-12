var image = {
    init: function () {
        image.registerEvents();
        image.deleteEmployee();
    },
    registerEvents: function () {
        $('.ui-btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');

            $.ajax({
                url: "/BranchDealer/Image/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        btn.text('Main');
                    }
                    else {
                        btn.text('Extra');
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
    deleteEmployee: function () {
        $('.btnDelete').each(function () {
            $(this).off('click').on('click', function (e) {
                e.preventDefault();
                Swal.fire({
                    title: 'Delete Image',
                    text: "Are you sure to delete this Image?",
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
                            url: '/BranchDealer/Image/Delete',
                            data: { id: $(this).data('id') },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {                                 
                                    Swal.fire(
                                        'Successfully!',
                                        'Delete successfully this Image',
                                        'success'
                                    );
                                    window.location.href = "/BranchDealer/Image?vehicleId=" + res.vehicleId;
                                }

                            }
                        })

                    }
                })
            })
        })

    },

}
image.init();