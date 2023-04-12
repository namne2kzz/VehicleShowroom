var dt = new Date();

GetReportBrandVehicleSell(dt.getMonth() + 1);
GetReportBrandVehicleAvailable();


var temp = dt.getMonth() + 1;
var mySelect = document.getElementById('ddlMonth');

for (var i, j = 0; i = mySelect.options[j]; j++) {
    if (i.value == temp) {
        mySelect.selectedIndex = j;
        break;
    }
}

$('#ddlMonth').on('change', function () {
    var month = $(this).val();
    GetReportBrandVehicleSell(month);    

});

function GetReportBrandVehicleSell(month) {
    $.get('/BranchDealer/Home/Report_BrandVehicleSell?month=' + month, function (res) {
        LoadBrandVehicleSell(res);
    });
};

function GetReportBrandVehicleAvailable() {
    $.get('/BranchDealer/Home/Report_BrandVehicleAvailable', function (res) {
        LoadBrandVehicleAvailable(res);
    });
};

function LoadBrandVehicleSell(lData) {
    $('canvas#ChartBrandVehicleSell').remove();
    $('#divChartBrandVehicleSell').html('<canvas id="ChartBrandVehicleSell" style="height:300px;"> </canvas>');

    var lstLable = [];
    var lstDataSource = [];

    $.each(lData, function (index, item) {
        lstLable.push(item.BrandName);
        lstDataSource.push(item.VehicleQuantity)
    });
    var ctx = document.getElementById("ChartBrandVehicleSell");
    new Chart(ctx, {
        type: "bar",
        data: {
            labels: lstLable,
            datasets: [{
                label: "Quantity",
                backgroundColor: "#4e73df",
                hoverBackgroundColor: "#2e59d9",
                borderColor: "#4e73df",
                data: lstDataSource

            }]
        },
        options: {
            maintainAspectRatio: false,
            title: {
                display: true,
                text: "Report about The Sold Vehicle on each of Brand of Branch Dealer in the Month"
            },
            layout: {
                padding: {
                    left: 10,
                    right: 25,
                    top: 25,
                    bottom: 0
                }
            },
            scales: {
                xAxes: [{
                    time: {
                        unit: 'month'
                    },
                    gridLines: {
                        display: false,
                        drawBorder: false
                    },

                }],
            },
        }
    }
    )

}

function LoadBrandVehicleAvailable(lData) {

    var lstLable = [];
    var lstDataSource = [];

    $.each(lData, function (index, item) {
        lstLable.push(item.BrandName);
        lstDataSource.push(item.VehicleQuantity)
    });
    var ctx = document.getElementById("ChartBrandVehicleAvailable");
    new Chart(ctx, {
        type: "bar",
        data: {
            labels: lstLable,
            datasets: [{
                label: "Quantity",
                backgroundColor: "#4e73df",
                hoverBackgroundColor: "#2e59d9",
                borderColor: "#4e73df",
                data: lstDataSource

            }]
        },
        options: {
            maintainAspectRatio: false,
            title: {
                display: true,
                text: "Report about The Available Vehicle on each of Brand of Branch Dealer"
            },
            layout: {
                padding: {
                    left: 10,
                    right: 25,
                    top: 25,
                    bottom: 0
                }
            },
            scales: {
                xAxes: [{
                    time: {
                        unit: 'month'
                    },
                    gridLines: {
                        display: false,
                        drawBorder: false
                    },

                }],
            },
        }
    }
    )

}