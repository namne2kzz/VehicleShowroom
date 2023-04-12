
var dt = new Date();

GetReportBrandVehicleSell(dt.getMonth() + 1);
GetReportBrandVehicleAvailable();
GetReportDealerVehicleSell(dt.getMonth() + 1);
GetReportAllotmentVehicleRegistration();
GetReportAllotmentContact(dt.getMonth() + 1);
GetReportCustomerStatus();
GetReportDealerStatus();


var temp = dt.getMonth()+1;
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
    GetReportDealerVehicleSell(month);
    GetReportAllotmentContact(month);
    
});

function GetReportBrandVehicleSell(month) {
    $.get('/Showroom/Home/Report_BrandVehicleSell?month=' + month, function (res) {
        LoadBrandVehicleSell(res);
    });
};

function GetReportBrandVehicleAvailable() {
    $.get('/Showroom/Home/Report_BrandVehicleAvailable', function (res) {
        LoadBrandVehicleAvailable(res);
    });
};

function GetReportDealerVehicleSell(month) {
    $.get('/Showroom/Home/Report_DealerVehicleSell?month=' + month, function (res) {
        LoadDealerVehicleSell(res);
    });
};

function GetReportAllotmentVehicleRegistration() {
    $.get('/Showroom/Home/Report_AllotmentVehicleRegistration', function (res) {
        LoadAllotmentVehicleRegistration(res);
    });
};

function GetReportAllotmentContact(month) {
    $.get('/Showroom/Home/Report_AllotmentContact?month=' + month, function (res) {
        LoadAllotmentContact(res);
    });
};

function GetReportCustomerStatus() {
    $.get('/Showroom/Home/Report_CustomerStatus', function (res) {
        LoadCustomerStatus(res);
    });
};

function GetReportDealerStatus() {
    $.get('/Showroom/Home/Report_DealerStatus', function (res) {
        LoadDealerStatus(res);
     
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
                text: "Report about The Sold Vehicle on each of Brand in the Month"
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
                text: "Report about The Available Vehicle on each of Brand"
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

function LoadDealerVehicleSell(lData) {
    $('canvas#ChartDealerVehicleSell').remove();
    $('#divChartDealerVehicleSell').html('<canvas id="ChartDealerVehicleSell" style="height:300px;"> </canvas>');

    var lstLable = [];
    var lstDataSource = [];

    $.each(lData, function (index, item) {
        lstLable.push(item.DealerName);
        lstDataSource.push(item.Quantity)
    });
    var ctx = document.getElementById("ChartDealerVehicleSell");
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
                text: "Report about The Sold Vehicle on each of Dealer in the Month"
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

function LoadAllotmentVehicleRegistration(lData) {
   

    var lstLable = [];
    var lstDataSource = [];

    $.each(lData, function (index, item) {
        lstLable.push(item.Province);
        lstDataSource.push(item.Quantity)
    });
    var ctx = document.getElementById("ChartAllotmentVehicleRegistration");
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
                text: "Report about The Allotment Details on each of Vehicle Registration Information"
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

function LoadAllotmentContact(lData) {  
    $('canvas#ChartAllotmentContact').remove();
    $('#divChartAllotmentContact').html('<canvas id="ChartAllotmentContact" style="height:300px;"> </canvas>');

    var lstLable = [];
    var lstDataSource = [];

    $.each(lData, function (index, item) {
        lstLable.push(item.Province);
        lstDataSource.push(item.Quantity)
    });
    var ctx = document.getElementById("ChartAllotmentContact");
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
                text: "Report about The Allotment Details on each of Contact Information"
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

function LoadCustomerStatus(lData) {  
  
    var ctx = document.getElementById("ChartCustomerStatus");
    new Chart(ctx, {
        type: "pie",
        data: {
            labels: ["Active","Lock"],
            datasets: [{
                backgroundColor: ["#4e73df", "#b91d47",],
                data: [lData[0], lData[1]]

            }]
        },
        options: {
            maintainAspectRatio: false,
            title: {
                display: true,
                text: "Report about The Customer Account Status in Website"
            },                
        }
    }
    )

}

function LoadDealerStatus(lData) {

    var ctx = document.getElementById("ChartDealerStatus");
    new Chart(ctx, {
        type: "pie",
        data: {
            labels: ["Active", "Lock", "Waiting"],
            datasets: [{
                backgroundColor: ["#4e73df", "#b91d47", "#00aba9"],
                data: [lData[0], lData[1], lData[2]],

            }]
        },
        options: {
            maintainAspectRatio: false,
            title: {
                display: true,
                text: "Report about The Dealer Account Status in Website"
            },
        }
    }
    )

}