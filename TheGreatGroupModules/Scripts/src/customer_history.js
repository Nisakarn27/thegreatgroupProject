
$(function () {

    $("#gridshow").hide();

    var listCustomer = (function () {
        var json = null;
        $.ajax({
            'async': false,
            'global': false,
            'url': '../Customers/GetListCustomers/0',
            'dataType': "json",
            'success': function (data) {
                json = data;
            }
        });
        return json;
    })();
    var listZone = (function () {
        var json = null;
        $.ajax({
            'async': false,
            'global': false,
            'url': '../Staffs/GetZone',
            'dataType': "json",
            'success': function (data) {
                json = data;
            }
        });
        return json;
    })();

    console.log(listCustomer.data, listZone.data)
    LoadFormSearch(listCustomer.data,listZone.data);
    
});
//var TypeDate = [
//            {
//                ID: 1,
//                Name: "เลือก วัน เดือน ปี"

//            },
//            {
//                ID: 2,
//                Name: "เลือก เดือน ปี"
//            },
//            {
//                ID: 3,
//                Name: "เลือก ปี "
//            },
//            {
//                ID: 4,
//                Name: "เลือก ช่วงวันที่ "
//            }
//];


var zone, cust;
function LoadFormSearch(lscustomers,lszone) {
    var data = {};
  
    var form = $("#form-container").dxForm({
          colCount: 1,
          width: 500,
        formData: data,
        items: [
            {
                dataField: "ZoneID",
                label: {
                    text: "เลือกสาย",
                },
                placeholder: "โปรดเลือกสาย",
                editorType: "dxSelectBox",
                editorOptions: {
                    items: lszone,
                    valueExpr: 'ID',
                    displayExpr: 'Value',
                    disabled: false,
                     onValueChanged: function (e) {
                         //   form.itemOption("Contacts.phone", "visible", e.value);
                         console.log(e);

                         var custSelect = (function () {
                             var json = null;
                             $.ajax({
                                 'async': false,
                                 'global': false,
                                 'url': '../Setting/GetListCustomersByZone/'+e.value,
                                 'dataType': "json",
                                 'success': function (data) {
                                     json = data;
                                 }
                             });
                             return json;
                         })();
                         console.log(custSelect)
                         form.itemOption("CustomerID", "editorOptions", {
                         

                             items: custSelect.data,
                             valueExpr: 'CustomerID',
                             displayExpr: 'CustomerName',
                             disabled: false
                         });
                    }

                },
                validationRules: [{
                    type: "required",
                    message: "โปรดเลือกสาย"
                }]
            }, {
                dataField: "ContractID",
                label: {
                    text: "เลือกลูกค้า",
                },
                placeholder: "โปรดเลือกลูกค้า",
                editorType: "dxLookup",
                editorOptions: {
                    items: lscustomers,
                    valueExpr: 'ContractID',
                    displayExpr: 'CustomerName',
                    disabled: false

                },
                validationRules: [{
                    type: "required",
                    message: "โปรดระบุ รหัสงานเย็บ"
                }]
            },

          
        ],
        //onFieldDataChanged: function (data) {
      
        //    if (data.dataField === "ZoneID") {
           
        //        form.option("CustomerID", "editorOptions", { placeholder: "Test" });
        //        form.option("CustomerID", "label", { text: "Test" });
        //    }
        //}
    }).dxForm("instance");

   
}





function SearchData() {
    
    $("#gridshow").show();
    $("#loadIndicator").dxLoadIndicator({
        visible: true
    });
    var item = $("#form-container").dxForm("instance").option('formData');

    if (item.ContractID > 0) {
        $.ajax({
            url: '../Report/GetPaymentReportByCustomer',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(item),
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

    } else {


        swal("ผิดพลาด!!", "กรุณาเลือกชื่อลูกค้า", "error");
        $("#loadIndicator").dxLoadIndicator({
            visible: false
        });
    }

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
            fileName: "รายงานประวัติลูกค้า",
        },
        paging: {
            enabled: false,
        },
        pager: {
            enabled: false,
        },
        allowColumnReordering: true,
        allowColumnResizing: true,
        columnAutoWidth: true,
        height: 530,
        columnFixing: {
            enabled: true
        },
        columns: [

            {
                dataField: "Day",
                caption: "วันที่",
                alignment: 'center',

            },
            {
                dataField: "Month1_Str",
                caption: "มกราคม",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.key.Month1 > 0) {

                        $("<div>")
                            .append("<span class='badge badge-success'> " + options.key.Month1_Str + " </span>")
                            .appendTo(container);
                    } else {


                        $("<div>")
                            .append(options.key.Month1_Str)
                            .appendTo(container);
                    }

                }
            },
            {
                dataField: "Month2_Str",
                caption: "กุมภาพันธ์",
                alignment: 'right',
                cellTemplate: function (container, options) {
                    if (options.key.Month2 > 0) {

                        $("<div>")
                            .append("<span class='badge badge-success'> " + options.key.Month2_Str + " </span>")
                            .appendTo(container);
                    } else {


                        $("<div>")
                            .append(options.key.Month2_Str)
                            .appendTo(container);
                    }

                }
            },
            {
                dataField: "Month3_Str",
                caption: "มีนาคม",
                alignment: 'right',
                cellTemplate: function (container, options) {
                    if (options.key.Month3 > 0) {

                        $("<div>")
                            .append("<span class='badge badge-success'> " + options.key.Month3_Str + " </span>")
                            .appendTo(container);
                    } else {


                        $("<div>")
                            .append(options.key.Month3_Str)
                            .appendTo(container);
                    }

                }
            },
            {
                dataField: "Month4_Str",
                caption: "เมษายน",
                alignment: 'right',
                cellTemplate: function (container, options) {
                    if (options.key.Month4 > 0) {

                        $("<div>")
                            .append("<span class='badge badge-success'> " + options.key.Month4_Str + " </span>")
                            .appendTo(container);
                    } else {


                        $("<div>")
                            .append(options.key.Month4_Str)
                            .appendTo(container);
                    }

                }
            },

            {
                dataField: "Month5_Str",
                caption: "พฤษภาคม",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.key.Month5 > 0) {

                        $("<div>")
                            .append("<span class='badge badge-success'> " + options.key.Month5_Str + " </span>")
                            .appendTo(container);
                    } else {


                        $("<div>")
                            .append(options.key.Month5_Str)
                            .appendTo(container);
                    }

                }
            },
            {
                dataField: "Month6_Str",
                caption: "มิถุนายน",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.key.Month6 > 0) {

                        $("<div>")
                            .append("<span class='badge badge-success'> " + options.key.Month6_Str + " </span>")
                            .appendTo(container);
                    } else {


                        $("<div>")
                            .append(options.key.Month6_Str)
                            .appendTo(container);
                    }

                }
            },

            {
                dataField: "Month7_Str",
                caption: "กรกฎาคม",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.key.Month7 > 0) {

                        $("<div>")
                            .append("<span class='badge badge-success'> " + options.key.Month7_Str + " </span>")
                            .appendTo(container);
                    } else {


                        $("<div>")
                            .append(options.key.Month7_Str)
                            .appendTo(container);
                    }

                }
            },

            {
                dataField: "Month8_Str",
                caption: "สิงหาคม",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.key.Month8 > 0) {

                        $("<div>")
                            .append("<span class='badge badge-success'> " + options.key.Month8_Str + " </span>")
                            .appendTo(container);
                    } else {


                        $("<div>")
                            .append(options.key.Month8_Str)
                            .appendTo(container);
                    }

                }
            },

            {
                dataField: "Month9_Str",
                caption: "กันยายน",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.key.Month9 > 0) {

                        $("<div>")
                            .append("<span class='badge badge-success'> " + options.key.Month9_Str + " </span>")
                            .appendTo(container);
                    } else {


                        $("<div>")
                            .append(options.key.Month9_Str)
                            .appendTo(container);
                    }

                }
            },

            {
                dataField: "Month7_Str",
                caption: "ตุลาคม",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.key.Month10 > 0) {

                        $("<div>")
                            .append("<span class='badge badge-success'> " + options.key.Month10_Str + " </span>")
                            .appendTo(container);
                    } else {


                        $("<div>")
                            .append(options.key.Month10_Str)
                            .appendTo(container);
                    }

                }
            },

            {
                dataField: "Month8_Str",
                caption: "พฤศจิกายน",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.key.Month11 > 0) {

                        $("<div>")
                            .append("<span class='badge badge-success'> " + options.key.Month11_Str + " </span>")
                            .appendTo(container);
                    } else {


                        $("<div>")
                            .append(options.key.Month11_Str)
                            .appendTo(container);
                    }

                }
            },

            {
                dataField: "Month9_Str",
                caption: "ธันวาคม",
                alignment: 'center',
                cellTemplate: function (container, options) {
                    if (options.key.Month12 > 0) {

                        $("<div>")
                            .append("<span class='badge badge-success'> " + options.key.Month12_Str + " </span>")
                            .appendTo(container);
                    } else {


                        $("<div>")
                            .append(options.key.Month12_Str)
                            .appendTo(container);
                    }

                }
            },
        ],
        onToolbarPreparing: function (e) {

           // console.log($("#form").dxForm("instance").option('formData.ContractID'));
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
                        window.open('/Report/CustomerHistory.aspx?ContractID=' + $("#form-container").dxForm("instance").option('formData.ContractID'));

                        DevExpress.ui.notify("Export PDF Successful!");
                    }
                }
            })
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