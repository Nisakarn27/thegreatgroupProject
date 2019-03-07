$(function () {
    //$("#gridContainer").dxDataGrid({
    //    dataSource: customers,

    //    paging: {
    //        pageSize: 10
    //    },
    //    pager: {
    //        showPageSizeSelector: true,
    //        allowedPageSizes: [5, 10, 20],
    //        showInfo: true
    //    },


    //    scrolling: {
    //        mode: "virtual"
    //    },


    //    allowColumnReordering: true,

    //    columns: ["CompanyName", "City", "State", "Phone", "Fax"]
    //}).dxDataGrid('instance');


    $("#gridReportContainer").dxDataGrid({
        dataSource: customers,
        showColumnLines: true,
        showRowLines: true,
        rowAlternationEnabled: true,
        showBorders: true,
        columnsAutoWidth: true,
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "ค้นหา..."
        },
        filterRow: {
            visible: true,
            applyFilter: "auto"
        },
        export: {
            enabled: true,
            fileName: "TrainingResults",
        },
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20],
            showInfo: true
        },
        columns: [{
            dataField: "No",
            caption: "No.",
            width: 50,
            allowFiltering: false
        }, {
            dataField: "CompanyName",
            caption: "ชื่อ-นามสกุล"
        },
          {
              dataField: "Address",
              caption: "ที่อยู่"
          },
        {
            dataField: "Phone",
            caption: "เบอร์โทร"
        },

          {
              dataField: "ID",
              caption: "ทำบัตร",
              alignment: 'center',
              cellTemplate: function (container, options) {
                  console.log(options.key);
                  $("<div>")
                      .append("<a href='../Customers/CustomerCard'  class='btn btn-info btn-sm  btn-fill' style='font-size:18px;padding: 0px 10px;'>" + 'ทำบัตร' + "</a>")
                      .appendTo(container);
              }

          },
          {
              dataField: "ID",
              caption: "ทำสัญญา",
              alignment: 'center',
              cellTemplate: function (container, options) {
                  console.log(options.key);
                  $("<div>")
                      .append("<a href='../Customers/CustomerContract'  class='btn btn-info btn-sm btn-fill'  style='font-size:18px;padding: 0px 10px;'>" + 'ทำสัญญา' + "</a>")
                      .appendTo(container);
              }
          },
           {
               dataField: "ID",
               caption: "ทำข้อมูลสินเชื่อ",
               alignment: 'center',
               cellTemplate: function (container, options) {
                   console.log(options.key);
                   $("<div>")
                       .append("<a href='../Customers/CustomerOrder' class='btn btn-info btn-sm btn-fill' style='font-size:18px;padding: 0px 10px;'>" + 'ทำข้อมูลสินเชื่อ' + "</a>")
                       .appendTo(container);
               }

           },
        ]
    });
});



var customers = [{
    "No": 1,
    "ID": 1,
    "CompanyName": "นายธนคุป ชูปลื้ม",
    "Address": "702 SW 8th Street",
    "City": "Bentonville",
    "State": "Arkansas",
    "Zipcode": 72716,
    "Phone": "(800) 555-2797",
    "Fax": "(800) 555-2171",
    "Website": "http://www.nowebsitesupermart.com"
},
{
    "No": 2,
    "ID": 2,
    "CompanyName": "นายธนคุป ชูปลื้ม",
    "Address": "702 SW 8th Street",
    "City": "Bentonville",
    "State": "Arkansas",
    "Zipcode": 72716,
    "Phone": "(800) 555-2797",
    "Fax": "(800) 555-2171",
    "Website": "http://www.nowebsitesupermart.com"
}];