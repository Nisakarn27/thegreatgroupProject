



$(function () {

    $("#gridshow").hide();
    LoadForm();
    Load_DataGrid({});
});
function Load_DataGrid(data) {

    //$.ajax({
    //    url: '../Staffs/GetStaffData?staffID=0&staffroleId=0',
    //    type: 'GET',
    //    contentType: 'application/json',
    //    success: function (data) {
            var rownum = 0;
            $("#gridContainer").dxDataGrid({
                dataSource:data.data,
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
                height: 500,
                columnFixing: {
                    enabled: true
                },
                columns: [
                    {
                        dataField: "ContractCreateDate",
                        caption: "วันที่เปิดบัญชี",
                        alignment: 'center',
                        fixed: false,
                        fixedPosition: 'left',
                    },
                    {
                        dataField: "CustomerName",
                        caption: "ชื่อ-นามสกุล",
                        alignment: 'left',


                    },
                    {
                        dataField: "ContractNumber",
                        caption: "เลขที่สัญญา",
                        alignment: 'center',


                    },
                    {
                        dataField: "ContractExpDate",
                        caption: "สิ้นสุดสัญญา",
                        alignment: 'center',


                    },
                   
                    {
                        dataField: "ContractPayment",
                        caption: "ยอดขาย",
                        alignment: 'center',


                    },
                    {
                        dataField: "NPL",
                        caption: "ยอดค้าง NPL",
                        alignment: 'center',


                    },
                    {
                        dataField: "NPLDay",
                        caption: "จำนวนวัน",
                        alignment: 'center',


                    },
                ],

            });

    //    },
    //    error: function () {
    //        console.log("error");
    //    }
    //});



}

function LoadForm() {



    var formdata = {
        //   ZoneID:1,
        TypeDate: 1,
        FromDate: new Date(),
        ToDate: new Date(),
        Month: new Date().getMonth(),
        Year: new Date().getFullYear(),
        NPLDay:3
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
        width: 30 + "%",
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
                    } else if (e.value == 2) {
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
            visible: false,
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
        {
            dataField: "NPLDay",

            label: {
                text: "จำนวน NPL(วัน)",
            },
            width: 200,
            editorType: "dxSelectBox",
            editorOptions: {
                dataSource: [3, 7, 15, 30],


            },

        },

        ]
    }).dxForm("instance");



}
function searchData() {


    var url = "../Report/GetNPLReport";

    $.post(url, {
        NPLDay: 3,
    StaffID:5 })
        .done(function (data) {
            console.log(data);
            if (data.success == true) {

                Load_DataGrid(data);

                
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