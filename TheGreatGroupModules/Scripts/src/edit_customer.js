$(function () {
    $("#loadIndicator").dxLoadIndicator({
        visible: true
    });
  
    var dataProvince = [];
    var dataDistrict = [];
    var dataSubDistrict = [];

   
    var dataProvince = [];
    var dataDistrict = [];
    var dataSubDistrict = [];
    var dataZone = [];

    var CustomerID = getUrlParameter('CustomerID');
    $.get("../Customers/GetCustomerID/" + CustomerID)
    .done(function (data) {
        if (data.success == true) {
            data.dataCustomer.CustomerID = CustomerID;
            datasourceCustomer = data.dataCustomer;
            dataZone = data.dataZone;

            LoadForm_CustomerInfo(datasourceCustomer,dataZone);
            $("#loadIndicator").dxLoadIndicator({
                visible: false
            });
        }


    });

    
    $("#button").dxButton({
        text: "แก้ไขข้อมูลลูกค้า",
        type: "success",
        useSubmitBehavior: true,
        validationGroup: "customerData",
        onClick: function (e) {
            datasourceCustomer = $("#form").dxForm("instance").option('formData');
            datasourceCustomer.CustomerID = getUrlParameter('CustomerID');

            var result = $("#form").dxForm("instance").validate();
            if (result.isValid) {

                $.ajax({
                    url: '../Customers/EditCustomers',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(datasourceCustomer),
                    success: function (data) {

                        DevExpress.ui.notify(data.data);
                     
                    },
                    error: function () {
                        console.log("error");
                    }
                });
            }

        }
    });


});