
var data = [];

$("#gridshow").hide();


function SearchStaff() {

 
    $("#loadIndicator").dxLoadIndicator({
        visible: true
    });
    if ($("#zoneid").val() == '') {

        $("#toast").dxToast({
            message: "กรุณาเลือกสาย",
            type: "error",
            displayTime: 3000
        })
        $("#toast").dxToast("show");
        $("#loadIndicator").dxLoadIndicator({
            visible: false
        });
        return;
    }

    if ($("#StaffID").val() == '') {

        $("#toast").dxToast({
            message: "กรุณาเลือกรายชื่อพนักงาน",
            type: "error",
            displayTime: 3000
        })
        $("#toast").dxToast("show");
        $("#loadIndicator").dxLoadIndicator({
            visible: false
        });
        return;
    }
    var date = $("#DateAsOf").datepicker({ dateFormat: 'dd-mm-yy' }).val();
    if (date == '' || date == null) {

        $("#toast").dxToast({
            message: "กรุณาเลือกวันที่",
            type: "error",
            displayTime: 3000
        })
        $("#toast").dxToast("show");
        $("#loadIndicator").dxLoadIndicator({
            visible: false
        });
        return;
    }
        
   

    var url = "../ManagePayment/GetDailyReceiptsReport?staffId=" + $("#StaffID").val() +
        "&dateAsOf=" + $('#DateAsOf').val();

    $.get(url)
.done(function (data) {
    console.log(data);
    if (data.success == true) {

        Load_DataGrid(data);


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

function btnSaveData() {

    $("#loadIndicator").dxLoadIndicator({
        visible: true
    });

    var url = "../ManagePayment/SaveActivateDailyReceipts?staffId=" + $("#StaffID").val() +
        "&dateAsOf=" + $('#DateAsOf').val();
    
    $.get(url)
.done(function (data) {

    if (data.success == true) {
        SearchStaff();
        DevExpress.ui.notify("บันทึกการตรวจสอบสำเร็จ !!!");
        $("#loadIndicator").dxLoadIndicator({
            visible: false
        });

    } else {
        DevExpress.ui.notify(data.errMsg);
        $("#loadIndicator").dxLoadIndicator({
            visible: false
        });

    }

});
}

function Load_DataGrid(data) {


    $("#gridContainer").dxDataGrid({
        dataSource: data.data,
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
            enabled: true,
            fileName: "File",
        },

        allowColumnReordering: true,
        allowColumnResizing: true,
   //     columnAutoWidth: true,
        height: 500,
        columnFixing: {
            enabled: true
        },
        columns: [
          {
            dataField: "ContractNumber",
            caption: "เลขที่สัญญา",
            width: 120+"%",
            alignment: 'left',
            allowFiltering: false,
    
        },
        {
            dataField: "CustomerName",
            caption: "ชื่อ-นามสกุล",
            width: 230+"%",
          
        },
         {
             dataField: "ContractCreateDate_Text",
             caption: "วันที่ทำสัญญา",
             alignment: 'center',
             width: 120+"%",

         },
          {
              dataField: "ContractExpDate_Text",
              caption: "วันที่หมดสัญญา",
              alignment: 'center',
              width: 120+"%",
          },

        {
            dataField: "ContractAmount_Text",
            caption: "งวดละ",
            alignment: 'right',
            width: 100+"%",
        },
             {
                 dataField: "PriceReceipts_Text",
                 caption: "ยอดที่ชำระ",
                 alignment: 'right',
                 width: 100 + "%",
                 fixed: false,
                 fixedPosition: 'right',
                 cellTemplate: function (container, options) {
                     var dataObj = options.data;

                     if (dataObj.PriceReceipts>0) {

                         $("<div>")
                            .append("<span class='badge badge-success'> " + dataObj.PriceReceipts_Text + "</span>")
                            .appendTo(container);
                     } else {
                         $("<div>")
                              .append(dataObj.PriceReceipts_Text)
                              .appendTo(container);
                     }
                 }
             },
            {
                dataField: "Balance_Text",
                caption: "ยอดคงเหลือ",
                width: 120 + "%",
                alignment: 'right',
                fixed: false,
                fixedPosition: 'right',
            },
      
            {
                dataField: "Status",
                caption: "สถานะ" ,
                alignment: 'center',
                width: 100 + "%",
                fixed: false,
                fixedPosition: 'right',
                cellTemplate: function (container, options) {
                    var dataObj = options.data;

                    if (Number(dataObj.Status)==-2) {

                        $("<div>")
                            .append("<span class='badge badge-warning'> จ่ายล่วงหน้า </span>")
                           .appendTo(container);
                    } else if (Number(dataObj.Status) == 0) {
                        $("<div>")
                            .append("<span class='badge badge-danger'> ชำระแล้ว </span>")
                             .appendTo(container);

                    }
                    else if (Number(dataObj.Status) == -1) {
                            $("<div>")
                                    .append("<span class='badge badge-default'> ยังไม่ชำระ </span>")
                                    .appendTo(container);

                        }

                }
            },
              {
                  dataField: "Remark",
                  caption: "หมายเหตุ",
                  alignment: 'center',
                  width: 120 + "%",
                  fixed: false,
                  fixedPosition: 'right',
                  //cellTemplate: function (container, options) {
                  //    $("<div>")
                  //        .append("<button  title='ระบุหมายเหตุ' class='btn btn-info btn-circle btn-sm' ><i class='fa fa-pencil'></i></a>")
                  //        .appendTo(container);
                  //}
            },

              {
                  dataField: "Remark",
                  caption: "ยกเลิก",
                  alignment: 'center',
                  width: 120 + "%",
                  fixed: false,
                  fixedPosition: 'right',
                  cellTemplate: function (container, options) {
                      console.log(options);
                      $("<div>")
                          .append("<button  title='ยกเลิก' class='btn btn-danger btn-circle btn-sm' ><i class='fa fa-close'></i></a>")
                          .appendTo(container);
                  }
              },
        ],
        summary: {
            totalItems: [
                { column: 'CustomerName', displayFormat: 'จำนวนลูกค้าทั้งหมด ' + data.countData + ' คน' },
                 { column: 'ContractExpDate_Text', displayFormat: 'ยอดรวม' },
                   { column: 'PriceReceipts_Text', displayFormat: data.SumData },
                    { column: 'ContractAmount_Text', displayFormat: data.SumDataContractAmount },
                    { column: 'Balance_Text', displayFormat: data.SumDataBalance }
            ],
        },
        onToolbarPreparing: function (e) {
            e.toolbarOptions.items.push({
                location: "before",
                widget: "dxButton",
                options: {
                    icon: "export",
                    //text: "",
                    onInitialized: function (e) {
                        clearFilterButton = e.component;
                    },
                    onClick: function (e) {
                        window.open('/Report/DailyReport.aspx?staffID=' + $("#StaffID").val() +
                        "&date=" + $('#DateAsOf').val(), '_blank');
                        //window.location.href = '/Report/ReportPage1.aspx?staffID=' + $("#StaffID").val() +
                        //"&date=" + $('#DateAsOf').val();
                        DevExpress.ui.notify("Export PDF Successful!");
                    }
                }
            })
        }
    });

}



   
