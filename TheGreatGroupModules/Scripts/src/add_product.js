$(function () {

    if (getUrlParameter('ProductID') == undefined) {

        $.ajax({
            url: '../Products/GetListProduct/0',
            type: 'GET',
            contentType: 'application/json',
            success: function (data) {
                var dataGroup = {
                    ProductGroupID: 1,
                    ProductName: "",
                    ProductPrice: 0,
                    UnitAmount: 0,
                    UnitName: "",
                };
                Load_form(dataGroup, data.dataProductGroup);
            },
            error: function () {
                console.log("error");
            }
        });

    } else {
        $.ajax({
            url: '../Products/GetListProduct/' + getUrlParameter('ProductID'),
            type: 'GET',
            contentType: 'application/json',
            success: function (data) {
                Load_form(data.data[0], data.dataProductGroup);
            },
            error: function () {
                console.log("error");
            }
        });


    }

});



function Load_form(data, dataProductGroup) {

    $("#form").dxForm({
        colCount: 1,
        formData: data,
        showColonAfterLabel: true,
        showValidationSummary: true,
        width: 500,
        items: [
            {
                dataField: "ProductGroupID",
                label: {
                    text: "ชื่อกลุ่มสินค้า"
                },
                editorType: "dxSelectBox",
                editorOptions: {
                    dataSource: dataProductGroup,
                    valueExpr: 'ProductGroupID',
                    displayExpr: 'ProductGroupName'
                },
                isRequired: true
                    , validationRules: [{
                        type: "required",
                        message: "โปรดเลือกชื่อกลุ่มสินค้า"
                    }],
            },
            {
                dataField: "ProductName",
                label: {
                    text: "ชื่อสินค้า"
                },
                isRequired: true
                    , validationRules: [{
                        type: "required",
                        message: "ต้องการชื่อสินค้า"
                    }],
            },
        {
            dataField: "UnitAmount",
            label: {
                text: "จำนวน"
            },
            isRequired: true
                    , validationRules: [{
                        type: "required",
                        message: "ต้องการจำนวน"
                    }],
        },

        {
            dataField: "UnitName",
            label: {
                text: "ชื่อหน่วย"
            },
            isRequired: true
                    , validationRules: [{
                        type: "required",
                        message: "ต้องการชื่อหน่วย"
                    }],
        },
           {
               dataField: "ProductPrice",
               label: {
                   text: "ราคา(บาท)"
               },
               isRequired: true
                    , validationRules: [{
                        type: "required",
                        message: "ต้องการราคา(บาท)"
                    }],
           },
        ]
    });


}


$("#myButton").dxButton({
    text: 'บันทึก',
    type: 'success',
    useSubmitBehavior: true,
    onClick: function () {

        var product = $("#form").dxForm("instance").option('formData');
        var result = $("#form").dxForm("instance").validate();


        if (result.isValid) {

            if (getUrlParameter('ProductID') > 0) {
                product.ProductID = getUrlParameter('ProductID');
                $.ajax({
                    url: '../Products/EditProduct',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(product),
                    success: function (data) {

                        if (data.success == true) {
                            swal("สำเร็จ!!", "แก้ไขข้อมูลสำเร็จ !!", "success");
                            window.location = "\Index";
                        } else {
                            swal("สำเร็จ!!", data.data, "success");
                        }
                    },
                    error: function () {
                        console.log("error");
                    }
                });

            } else {
                $.ajax({
                    url: '../Products/AddProduct',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(product),
                    success: function (data) {

                        if (data.success == true) {
                            swal("สำเร็จ!!", "เพิ่มข้อมูลสำเร็จ !!", "success");
                            window.location = "\Index";
                        } else {
                            swal("สำเร็จ!!", data.data, "success");
                        }
                    },
                    error: function () {
                        console.log("error");

                    }
                });

            }


        }
    }
});
