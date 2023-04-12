var dealer = {
    init: function () {
        dealer.registerEvents();
        dealer.deleteEmployee();
    },
    registerEvents: function () { 
    
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
                    title: 'Delete Dealer',
                    text: "Are you sure to delete this dealer?",
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
                            url: '/Showroom/Dealer/Delete',
                            data: { id: $(this).data('id') },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {
                                    Swal.fire(
                                        'Successfully!',
                                        'Delete successfully this dealer',
                                        'success'
                                    );
                                    window.location.href = "/Showroom/Dealer/Index"
                                }

                            }
                        })

                    }
                })
            })
        })

    },

}
dealer.init();