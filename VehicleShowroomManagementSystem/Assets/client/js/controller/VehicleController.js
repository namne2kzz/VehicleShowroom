var vehicle = {
    init: function () {
        vehicle.registerEvents();
    },
    registerEvents: function () {
        $('#btnCompare').on('click', function () {
            window.location.href = '/Vehicle/Compare';
        });

        $('#btnClear').on('click', function () {
            $.post("ClearCompare", "Vehicle");
        });

        $('.btnRemoveCompare').each(function () {
            $(this).on('click', function (e) {               
                e.preventDefault();
                $.ajax({
                    url: '/Vehicle/RemoveCompare',
                    data: { vehicleId: $(this).data('id') },
                    dataType: 'json',
                    type: 'POST',
                    success: function (res) {
                        if (res.status == true) {
                            window.location.href = '/Vehicle/Compare';
                        }
                    }
                });
            });
        });
    }
}
vehicle.init();
