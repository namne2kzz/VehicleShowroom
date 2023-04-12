var cart = {
    init: function () {
        cart.regEvents();
        cartListCheck = [];
        service = [];
    },
    regEvents: function () {

        //ajax btnContinue
        $('.btn-continue').off('click').on('click', function (e) {
            e.preventDefault();
            window.location.href = "/";

        });     

        //ajax Remove item
        $('.remove-item').each(function () {
            $(this).on('click', function (e) {
                e.preventDefault();
                Swal.fire({
                    title: 'Remove Vehicle?',
                    text: "Are you sure to remove this Vehicle!",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes, remove it!',
                    customClass: 'swal-wide',
                }).then((result) => {
                    if (result.isConfirmed) {
                        console.log($(this).data('id'));
                        $.ajax({
                            url: '/Cart/Remove',
                            data: { id: $(this).data('id') },
                            dataType: 'json',
                            type: 'POST',
                            success: function (res) {
                                if (res.status == true) {
                                    window.location.href = "/Cart/Index";
                                }
                            }
                        });
                    }
                })
            })
        });

        //ajax Clear item
        $('#empty_cart_button').off('click').on('click', function (e) {
            e.preventDefault();
            Swal.fire({
                title: 'Clear All Vehicle?',
                text: "Are you sure to clear All Vehicle!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, clear it!',
                customClass: 'swal-wide',
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        url: '/Cart/ClearAll',
                        dataType: 'json',
                        type: 'POST',
                        success: function (res) {
                            if (res.status == true) {
                                window.location.href = "/Cart/Index";
                            }
                        }
                    });
                }
            })


        });

        //item cart selected
        var totalDiscount = 0;
        var totalAmount = 0;
        $('#totalDiscount').html(totalDiscount);
        $('#totalAmount').html(totalAmount);

        $('.check-vehicle').each(function () {

            $(this).on('change', function () {

                if ($(this).is(":checked")) {

                    totalDiscount += $(this).data('discount') * $(this).data('price');
                    totalAmount += $(this).data('price');

                    $('#totalDiscount').html(totalDiscount.toString("N0"));
                    $('#totalAmount').html((totalAmount - totalDiscount).toString("N0"));
                }
                else {

                    totalDiscount -= $(this).data('discount') * $(this).data('price');
                    totalAmount -= $(this).data('price');

                    $('#totalDiscount').html(totalDiscount.toString("N0"));
                    $('#totalAmount').html((totalAmount - totalDiscount).toString("N0"));

                }

            });
        });

        //service

        var service = [];
        var cartListCheck = [];
        $('#btnContinueCheckout').off('click').on('click', function () {

            $('.check-vehicle').each(function () {

                if ($(this).is(":checked")) {

                    cartListCheck.push($(this).data('id') + '-' + $(this).data('image') + '-' + $(this).data('discount'));
                }
            });
            $('.check-service').each(function () {

                if ($(this).is(":checked")) {
                    service.push($(this).data('id'));
                }

            });
            $.ajax({
                url: '/Cart/PrepareCheckout',
                data: { vehicle: cartListCheck, service: service },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = '/Order/Index';
                    }
                    else {
                        Swal.fire(
                            'Error!',
                            'Please select Item',
                            'error'
                        );
                        window.location.href = '/Cart/Index';
                    }
                }
            });

        });

    }
}
cart.init();




