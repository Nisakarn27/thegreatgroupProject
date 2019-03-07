$(function () {
    $("#loadIndicator").dxLoadIndicator({
        visible: true
    });
var employee = {

};

var dataProvince = [];
var dataDistrict = [];
var dataSubDistrict = [];
var dataZone = [];

    $.get("../Customers/GetCustomerID/0")
.done(function (data) {
    if (data.success == true) {
      
     
        datasourceCustomer = data.dataCustomer;
        dataZone = data.dataZone;
        console.log(dataZone);

        LoadForm_CustomerInfo(datasourceCustomer, dataZone);
       

        $("#loadIndicator").dxLoadIndicator({
            visible: false
        });
    }

  
});
$("#button").dxButton({
    text: "เพิ่มข้อมูลลูกค้า",
    type: "success",
    useSubmitBehavior: true,
    validationGroup: "customerData",
    onInitialized: function (e) {


    }
});


$("#form").on("submit", function (e) {
    console.log(e);
    DevExpress.ui.notify({
        message: "You have submitted the form",
        position: {
            my: "center top",
            at: "center top"
        }
    }, "success", 3000);

    e.preventDefault();
});
});