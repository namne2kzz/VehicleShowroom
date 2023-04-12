var blog = {
    init: function () {
        blog.registerEvents();
        blog.deleteEmployee();
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
                    title: 'Delete Blog',
                    text: "Are you sure to delete this Blog?",
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
                            url: '/Showroom/Blog/Delete',
                            data: { id: $(this).data('id') },
                            type: 'POST',
                            dataType: 'json',
                            success: function (res) {
                                if (res.status == true) {
                                    Swal.fire(
                                        'Successfully!',
                                        'Delete successfully this Blog',
                                        'success'
                                    );
                                    window.location.href = "/Showroom/Blog/Index"
                                }

                            }
                        })

                    }
                })
            })
        })

    },

}
blog.init();