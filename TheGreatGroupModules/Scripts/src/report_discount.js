
$(function () {

    $("#gridshow").hide();
    LoadFormSearch();

});
var formdata = {
    TypeDate: 1,
    FromDate: new Date(),
    ToDate: new Date(),
    Month: 1,
    Year: new Date().getFullYear()
};
var TypeDate = [
            {
                ID: 1,
                Name: "เลือก วัน เดือน ปี"

            },
            {
                ID: 2,
                Name: "เลือก เดือน ปี"
            },
            {
                  ID: 3,
                  Name: "เลือก ปี "
            },
            {
                  ID: 4,
                  Name: "เลือก ช่วงวันที่ "
            }
];

function LoadFormSearch() {

    var list_zone = (function () {
        var json = null;
        $.ajax({
            'async': false,
            'global': false,
            'url': "../Staffs/GetZone/0",
            'dataType': "json",
            'success': function (data) {
                json = data.data;
                console.log(json);
            }
        });
        return json;
    })();
    $("#gridshow").show();
    var formInstance = $("#form").dxForm({
        colCount: 1,
        formData: formdata,
        showColonAfterLabel: true,
        showValidationSummary: false,
        width:30+"%",
        items: [{
            dataField: "ZoneID",

            label: {
                text: "สายบริการ",
            },
            width: 200,
            editorType: "dxSelectBox",
            editorOptions: {
                dataSource: list_zone,
                valueExpr: 'ID',
                displayExpr: 'Value',
                value: list_zone[0].ID,
                disabled: false

            },

        },
             {
                 dataField: "TypeDate",
                 label: {
                     text: "เงื่อนไขวันที่"
                 },
                 editorType: "dxSelectBox",
                 editorOptions: {
                     items: TypeDate,
                     displayExpr: "Name",
                     valueExpr: "ID",
                     onValueChanged: function (e) {
                        
                         if (e.value == 1) {
                             formInstance.itemOption('FromDate', 'visible', true);
                             formInstance.itemOption('ToDate', 'visible', false);
                             formInstance.itemOption('Month', 'visible', false);
                             formInstance.itemOption('Year', 'visible', false);
                         } else if(e.value == 2){
                             formInstance.itemOption('FromDate', 'visible', false);
                             formInstance.itemOption('ToDate', 'visible', false);
                             formInstance.itemOption('Month', 'visible', true);
                             formInstance.itemOption('Year', 'visible', true);
                         }
                         else if (e.value == 3) {
                             formInstance.itemOption('FromDate', 'visible', false);
                             formInstance.itemOption('ToDate', 'visible', false);
                             formInstance.itemOption('Month', 'visible', false);
                             formInstance.itemOption('Year', 'visible', true);
                         }
                         else if (e.value == 4) {
                             formInstance.itemOption('FromDate', 'visible', true);
                             formInstance.itemOption('ToDate', 'visible', true);
                             formInstance.itemOption('Month', 'visible', false);
                             formInstance.itemOption('Year', 'visible', false);
                         }
                         //alert(e.value)
                     }
                 },
             },
            {
                dataField: "FromDate",
                editorType: "dxDateBox",
                label: {
                    text: "วันที่"
                },
                editorOptions: {
                    width: "100%",
                    displayFormat: "dd/MM/yyyy"
                },
              
            },
            {
                dataField: "ToDate",
                editorType: "dxDateBox",
                label: {
                    text: "ถึงวันที่"
                },
                visible:false,
                editorOptions: {
                    width: "100%",
                    displayFormat: "dd/MM/yyyy",

                },
            },

        {
        dataField: "Month",
        label: {
            text: "เลือกเดือน "
        },
        visible: false,
        editorType: "dxSelectBox",
        editorOptions: {
            items: Months,
            displayExpr: "Name",
            valueExpr: "ID",
            
        },
        },

        {
            dataField: "Year",
            label: {
                text: "เลือกปี "
            },
            visible: false,
            editorType: "dxSelectBox",
            editorOptions: {
                items: Years,
                

            },
        },
        ]
   }).dxForm("instance");
    
 

}


function SearchData() {
    $("#gridshow").show();
    $("#loadIndicator").dxLoadIndicator({
        visible: true
    });
    var DataSearch = $("#form").dxForm("instance").option('formData');

    $.ajax({
        url: '../Report/GetDiscountReport',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(DataSearch),
        success: function (data) {

            if (data.success == true) {
               
                Load_DataGrid(data.data);
                $("#loadIndicator").dxLoadIndicator({
                    visible: false
                });

            } else {

                swal("ผิดพลาด!!", data.data, "error");
                $("#loadIndicator").dxLoadIndicator({
                    visible: false
                });
            }


        },
        error: function () {
            console.log("error");

        }
    });
}


function Load_DataGrid(data) {
 
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
            enabled: true,
            fileName: "รายงานส่วนลด",
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
                dataField: "ContractCloseDate_Text",
                caption: "วันที่ปิดบัญชี",
                alignment: 'center',
               
            },
          
            {
                dataField: "CustomerName",
                caption: "ชื่อ-นามสกุล ลูกค้า",
                alignment: 'left',
                
            },
            {
                dataField: "ContractNumber",
                caption: "เลขที่สัญญา",
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
                      dataField: "ContractDiscount_Text",
                      caption: "ส่วนลด",
                      alignment: 'right',
                 },
                  {
                      dataField: "ContractAccountStatusName",
                      caption: "สภาพบัญชี",
                      alignment: 'right',
                  },
                  {
                      dataField: "ContractTimes",
                      caption: "ครั้งที่ ",
                      alignment: 'right',
                  },




        ],

    });

}


function ClearData() {


    $("#gridshow").hide();
    var formInstance = $("#form").dxForm("instance");
    formInstance.option('formData.TypeDate', 1);
    formInstance.option('formData.FromDate', new Date());
    formInstance.itemOption('FromDate', 'visible', true);
    formInstance.itemOption('ToDate', 'visible', false);
    formInstance.itemOption('Month', 'visible', false);
    formInstance.itemOption('Year', 'visible', false);
}
