var finance = {
    init: function () {
        finance.registerEvents();
        finance.loadBrand();
    },

    registerEvents: function () {
        $('#sltBrand').off('change').on('change', function () {
            var id = $(this).val();         
            if (id != '') {
                finance.loadVehicle(id)
            }
            else {
                $('#sltVehicle').html('');
            }
        });

        $('#sltVehicle').off('change').on('change', function () {
            $.ajax({
                url: '/Vehicle/Finance',
                data: { vehicleId: $(this).val() },
                type: 'POST',
                dataType: 'json',
                success: function (res) {
                    if (res.status == true) {
                        $('#txtPrice').html(res.price);
                        $('#txtRegistrationFee').html(res.registrationFee);
                        $('#txtRoadMaintentanceFee').html(res.roadMaintenanceFee);
                        $('#txtCilviInsuranceFee').html(res.civilLiabilityInsuranceFee);
                        $('#txtTestingFee').html(res.testingFee);
                        $('#txtRegistrationPlateFee').html(res.registrationPlateFee);
                        $('#txtTotal').html(res.total);
                        $('#image').attr('src', '/Assets/dealer/img/vehicle/' + res.image);
                    }
                }

            });
        });

    },

    loadBrand: function() {
        $.ajax({
            url: '/Vehicle/LoadBrand',
            type: 'POST',
            dataType: 'json',
            success: function (res) {
                if (res.status == true) {
                    var html = '<option value="" selected="true" disabled="disabled">---Select Brand---</option>';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.ID + '">' + item.Name + '</option>'
                    })
                    $('#sltBrand').html(html);
                }
            }

        });
    },

    loadVehicle:function(id) {
        $.ajax({
            url: '/Vehicle/LoadVehicle',
            data: { brandId: id },
            type: 'POST',
            dataType: 'json',
            success: function (res) {
                if (res.status == true) {
                    var html = '<option value="" selected="true" disabled="disabled">---Select Vehicle---</option>';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.ID + '">' + item.Name + '</option>'
                    })
                    $('#sltVehicle').html(html);
                }               
            }

        });
    },

};
finance.init();