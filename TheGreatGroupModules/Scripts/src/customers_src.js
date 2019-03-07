
var data = [];
$("#gridshow").hide();
  
  

    function SearchCustomer() {
 
        $("#loadIndicator").dxLoadIndicator({
            visible: true
        });

        $.post("../Customers/GetCustomers", {
            CustomerFirstName: $("#CustomerFirstName").val(),
            CustomerLastName: $("#CustomerLastName").val(),
            CustomerMobile: $("#CustomerMobile").val(),
            CustomerIdCard: $("#CustomerIdCard").val()

        })
    .done(function (data) {
      
        if (data.success == true)
        {
            Load_DataGrid(data.data);
            $("#gridshow").show();
            $("#loadIndicator").dxLoadIndicator({
                visible: false
            });
        } else {
            $("#loadIndicator").dxLoadIndicator({
                visible: false
            });
            DevExpress.ui.notify(data.errMsg);
        }

    });

    }


    function btnClear() {
        $("#gridshow").hide();
        $("#CustomerFirstName").val('');
        $("#CustomerLastName").val('');
        $("#CustomerMobile").val('');
        $("#CustomerIdCard").val('');
    }
    function Load_DataGrid(data) {

        $("#gridContainer").dxDataGrid({
            dataSource: data,
            showColumnLines: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            showBorders: true,
        
            //editing: {
            //allowUpdating:true},
            searchPanel: {
                visible: true,
                width: 300,
                placeholder: "ค้นหา..."
            },
            filterRow: {
                visible: false,
                applyFilter: "auto"
            },
            export: {
                enabled: false,
                fileName: "File",
            },
        
            allowColumnReordering: true,
            allowColumnResizing: true,
            columnAutoWidth: true,
            height:500,
            columnFixing: {
                enabled: true
            },
            columns: [{
                dataField: "CustomerIdCard",
                caption: "รหัสประจำตัวประชาชน",
                width: 120,
                alignment: 'center',
                allowFiltering: false
            }, {
                dataField: "CustomerName",
                caption: "ชื่อ-นามสกุล",
                width: 200,
            },
              {
                  dataField: "CustomerAddress1",
                  caption: "ที่อยู่",
                  //width: 280,
                     
              },
                {
                    dataField: "CustomerTelephone",
                    caption: "เบอร์โทรศัพท์",
                    width: 150,
                    alignment: 'center',
                },
             {
                 dataField: "CustomerMobile",
                 caption: "เบอร์มือถือ",
                 width: 150,
                 alignment: 'center',
             },
           
           
              {
                  dataField: "CustomerID",
                  caption: "ซื้อสินค้า",
                  alignment: 'center',
                  allowFiltering: false,
                  width:100,
                  fixed: true,
                  fixedPosition: 'right',
                  cellTemplate: function (container, options) {
                  
                      $("<div>")
                          .append("<a href='\ListContract?CustomerID=" + options.key.CustomerID + "'  title='ซื้อสินค้า'  class='btn btn-info btn-circle btn-sm' ><i class='fa fa-shopping-cart'></i></a>")
                          .appendTo(container);
                  }

              },
                 {
                     dataField: "CustomerID",
                     caption: "แก้ไขข้อมูล",
                     alignment: 'center',
                     allowFiltering: false,
                     fixed: true,
                     fixedPosition: 'right',
                     width: 100,
                     cellTemplate: function (container, options) {
                         $("<div>")
                             .append("<a href='\EditCustomer?CustomerID=" + options.key.CustomerID + "' title='แก้ไขข้อมูลลูกค้า' class='btn btn-info btn-circle btn-sm' ><i class='fa fa-pencil'></i></a>")
                             .appendTo(container);
                     }

                 },
              {
                  dataField: "CustomerID",
                  caption: "ลบข้อมูล",
                  alignment: 'center',
                  allowFiltering: false,
                  fixed: true,
                  fixedPosition: 'right',
                  width: 100,
                  cellTemplate: function (container, options) {
                      $("<div>")
                          .append("<a onclick='DeletedCustomer(" + '"' + options.data.CustomerID + '","' + options.data.CustomerName + '"' + ")' title='ลบข้อมูลลูกค้า' class='btn btn-info btn-circle btn-sm' ><i class='fa fa-trash'></i></a>")
                          .appendTo(container);
                  }

              },
              
            
            ],
        });

    }

    function AddCustomer() {

        var data = {
            CustomerTitleName: $('#CustomerTitleName').val(),
            CustomerFirstName: $('#CustomerFirstName').val(),
            CustomerLastName: $('#CustomerLastName').val(),
            CustomerNickName: $('#CustomerNickName').val(),
            CustomerIdCard: $('#CustomerIdCard').val(),
            CustomerAddress1: $('#CustomerAddress1').val(),
            CustomerAddress2: $('#CustomerAddress2').val(),
            CustomerProvinceId: $('#CustomerProvince').val(),
            CustomerDistrictId: $('#CustomerDistrict').val(),
            CustomerSubDistrictId: $('#CustomerSubDistrict').val(),
            CustomerZipCode: $('#CustomerZipCode').val(),
            CustomerMobile: $('#CustomerMobile').val(),
            CustomerEmail: $('#CustomerEmail').val(),
        }

        $("#loadIndicator").dxLoadIndicator({
            visible: true
        });

        $.post("../Customers/AddCustomers", data)
    .done(function (data) {
       
        if (data.success == true) {

            DevExpress.ui.notify(" เพิ่มลูกค้าเรียบร้อยแล้ว ");
            $("#loadIndicator").dxLoadIndicator({
                visible: false
            });

        } else {

            $("#loadIndicator").dxLoadIndicator({
                visible: false
            });

            DevExpress.ui.notify(data.errMsg);
        }


    });
    }


    function DeletedCustomer(id,Name) {

        swal({
            title: "",
            text: "ต้องการลบข้อมูลลูกค้า " + Name + " ใช่หรือไม่ ",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: '#DD6B55',
            confirmButtonText: 'ตกลง',
            cancelButtonText: "ยกเลิก",
            closeOnConfirm: false,
            closeOnCancel: false
        },
       function (isConfirm) {

           if (isConfirm) {
             

               $.post("../Customers/DeleteCustomers?CustomerID=" + id)
            .done(function (data) {

                if (data.success == true) {

                    swal("ลบข้อมูลลูกค้าเรียบร้อยแล้ว !", "", "success");
                    $("#loadIndicator").dxLoadIndicator({
                        visible: false
                    });
                    SearchCustomer();
                } else {

                    $("#loadIndicator").dxLoadIndicator({
                        visible: false
                    });

                    DevExpress.ui.notify(data.errMsg);
                }


            });


           } else {
               swal.close();
               e.preventDefault();
           }
       });

      

    }