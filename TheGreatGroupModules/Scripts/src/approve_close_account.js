

$(function () {

    Call_Grid("");


})
function SearchStaff() {


    Call_Grid($("#CustomerIdCard").val());
}
function Call_Grid(data) {
    $("#loadIndicator").dxLoadIndicator({
        visible: true
    });

    $.ajax({
        url: '../Contract/GetApproveOpen_CloseContract?custpmerIDCard=' + data,
        type: 'GET',
        contentType: 'application/json',
        success: function (data) {

            if (data.success) {

                Load_DataGrid1(data.dataOpen);
                Load_DataGrid2(data.dataClose);
                $("#loadIndicator").dxLoadIndicator({
                    visible: false
                });
            } else {
                alertError(data.errMsg);

            }


        },
        error: function () {
            console.log("error");
        }
    });
}

function btnClear() {

    $("#CustomerIdCard").val("");
    Call_Grid("");
}

function Load_DataGrid1(data) {


    $("#gridContainer").dxDataGrid({
        dataSource: data,
        showColumnLines: true,
        showRowLines: true,

        rowAlternationEnabled: true,
        showBorders: true,
        selection: {
            mode: "single"
        },
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
        height: 300,
        columnFixing: {
            enabled: true
        },
        columns: [

            {
                dataField: "ContractNumber",
                caption: "เลขที่สัญญา",
                alignment: 'left',
                width: 100,
                fixed: false,
                fixedPosition: 'left',
            },
            {
                dataField: "CustomerName",
                caption: "ชื่อ-นามสกุล ลูกค้า",
                alignment: 'left',
                width: 150,
            },
               {
                   dataField: "ContractCreateDate_Text",
                   caption: "วันที่ทำสัญญา",
                   alignment: 'center',


               },
                {
                    dataField: "ContractExpDate_Text",
                    caption: "วันที่สิ้นสุดสัญญา",
                    alignment: 'center',


                },
                 {
                     dataField: "TotalSales_Text",
                     caption: "จำนวนเงินทั้งหมด",
                     alignment: 'right',
                 },
                 {
                     dataField: "PriceReceipts_Text",
                     caption: "ชำระแล้ว",
                     alignment: 'right',
                 },
                  {
                      dataField: "Balance_Text",
                      caption: "คงเหลือ",
                      alignment: 'right',
                  },


            {
                dataField: "ContractID",
                caption: "ให้ส่วนลด",
                alignment: 'center',
                width: 100,
                fixed: true,
                fixedPosition: 'right',
                verticalAlignment: 'middle',
                cellTemplate: function (container, options) {
                    $("<div>")
                         .append("<button type='link' onclick='Show_PopupEdit("
                         + '"' + options.data.CustomerName
                         + '","' + options.data.Balance_Text
                         + '",' + options.data.ContractID
                         + ',' + options.data.CustomerID
                           + ',' + options.data.Balance
                          + ")' title='ให้ส่วนลด'  class='btn btn-info btn-circle btn-sm' ><i class='fa fa-money'></i></button>")
                         .appendTo(container);
                }
            },

            {
                dataField: "ContractID",
                caption: "ปิดบัญชี",
                alignment: 'center',
                width: 100,
                fixed: true,
                fixedPosition: 'right',
                verticalAlignment: 'middle',
                cellTemplate: function (container, options) {
                    $("<div>")
                        .append("<button type='link' onclick='Show_PopupCloseAccount("
                        + '"' + options.data.CustomerName
                        + '","' + options.data.Balance_Text
                        + '",' + options.data.ContractID
                        + ',' + options.data.CustomerID
                        + ',' + options.data.Balance
                        + ")' title='ปิดบัญชี'  class='btn btn-danger btn-circle btn-sm' ><i class='fa fa-close'></i></button>")
                        .appendTo(container);
                }
            },

        ],

    });

}

function Load_DataGrid2(data) {


    $("#gridContainer2").dxDataGrid({
        dataSource: data,
        showColumnLines: true,
        showRowLines: true,

        //  rowAlternationEnabled: true,
        showBorders: true,
        selection: {
            mode: "single"
        },
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
        height: 300,
        columnFixing: {
            enabled: true
        },
        columns: [

          {
              dataField: "ContractNumber",
              caption: "เลขที่สัญญา",
              alignment: 'left',
              width: 100,
              fixed: false,
              fixedPosition: 'left',
          },
          {
              dataField: "CustomerName",
              caption: "ชื่อ-นามสกุล ลูกค้า",
              alignment: 'left',
              width: 150,
          },
             {
                 dataField: "ContractCreateDate_Text",
                 caption: "วันที่ทำสัญญา",
                 alignment: 'center',


             },
              {
                  dataField: "ContractExpDate_Text",
                  caption: "วันที่สิ้นสุดสัญญา",
                  alignment: 'center',


              },
               {
                   dataField: "TotalSales_Text",
                   caption: "จำนวนเงินทั้งหมด",
                   alignment: 'right',
               },
               {
                   dataField: "PriceReceipts_Text",
                   caption: "ชำระแล้ว",
                   alignment: 'right',
               },
                {
                    dataField: "Balance_Text",
                    caption: "ให้ส่วนลด",
                    alignment: 'right',
               },
                {
                   // dataField: "Balance_Text",
                    caption: "สภาพบัญชี",
                    alignment: 'right',
                },
                //{
                //    dataField: "Balance_Text",
                //    caption: "ยกเลิกการให้ส่วนลด",
                //    alignment: 'center',
                //    width: 100,
                //    fixed: true,
                //    fixedPosition: 'right',
                //    verticalAlignment: 'middle',
                //    cellTemplate: function (container, options) {
                //        $("<div>")
                //          .append("<button type='link' onclick='Show_PopupEdit("
                //          + '"' + options.data.CustomerName
                //          + '","' + options.data.Balance_Text
                //          + '",' + options.data.ContractID
                //          + ',' + options.data.CustomerID
                //          + ',' + options.data.Balance
                //          + ")' title='ยกเลิกการให้ส่วนลด' "+
                //          +" class='btn btn-warning btn-circle btn-sm'>"
                //          + "<i class='fa fa-close'></i>"
                //          + "</button>")
                //          .appendTo(container);
                //    }
                //},
        ],
    });

}

function Show_PopupEdit(CustomerName, Discount, contractId, customerId, balance) {

    //alertConfirm("ต้องการให้ส่วนลดคุณ ธิดา ชัยชา  จำนวน " + data + " บาท ใช่หรือไม่ ? ", "ให้ส่วนลดสำเร็จ", "ยกเลิกการให้ส่วนลด");
    swal({
        title: "",
        text: "ต้องการให้ส่วนลด " + CustomerName + "  จำนวน " + Discount + " บาท ใช่หรือไม่ ",
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



        var data = {
            CustomerID: customerId,
            ContractID: contractId,
            PriceReceipts: balance,
        }


        $.post("../Contract/PostAddDiscount", data)
    .done(function (data) {

        if (data.success == true) {
            swal("สำเร็จ !", "", "success");

            $("#loadIndicator").dxLoadIndicator({
                visible: false
            });
        } else {
            $("#loadIndicator").dxLoadIndicator({
                visible: false
            });
            swal("เกิดข้อผิดพลาด !", "", "error");
        }
        SearchStaff();
    });


    } else {
        swal.close();
        e.preventDefault();
    }
});

}



function Show_PopupCloseAccount(CustomerName, Discount, contractId, customerId, balance) {

    var text1 = "ต้องการปิดบัญชีของ  " + CustomerName + "  ซึ่งมียอดค้างชำระ " + Discount + " บาท ใช่หรือไม่ "
    swal({
        title: "",
        text: text1,
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



                var data = {
                    CustomerID: customerId,
                    ContractID: contractId,
                    PriceReceipts: balance,
                }


                $.post("../Contract/PostAddDiscount", data)
                    .done(function (data) {

                        if (data.success == true) {
                            swal("สำเร็จ !", "", "success");

                            $("#loadIndicator").dxLoadIndicator({
                                visible: false
                            });
                        } else {
                            $("#loadIndicator").dxLoadIndicator({
                                visible: false
                            });
                            swal("เกิดข้อผิดพลาด !", "", "error");
                        }
                        SearchStaff();
                    });


            } else {
                swal.close();
                e.preventDefault();
            }
        });

}