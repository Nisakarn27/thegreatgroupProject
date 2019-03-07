


    $("#gridContainer").dxDataGrid({
      //  dataSource: data,
        showColumnLines: true,
        showRowLines: true,
        rowAlternationEnabled: true,
        showBorders: true,
        searchPanel: {
            visible: true,
            width: 300,
            placeholder: "ค้นหา..."
        },
        filterRow: {
            visible: true,
            applyFilter: "auto"
        },
        export: {
            enabled: true,
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
             //    dataField: "CustomerAddress1",
                 caption: "ลำดับที่",
                 width: 60,

             }, {
         //   dataField: "CustomerCode",
            caption: "วันที่ซื้อ",
            width: 120,
            allowFiltering: false
        }, {
        //    dataField: "CustomerName",
            caption: "รายการสินค้า",
            width: 240,
        },
         
          {
          //    dataField: "CustomerSubDistrict",
              caption: "จำนวน (หน่วย)",
              width: 120,
          },
        {
        //    dataField: "CustomerDistrict",
            caption: "ราคาต่อหน่วย",
            width: 120,
        },

            {
          //      dataField: "CustomerProvince",
                caption: "จำนวนเงิน",
                width: 120,

            },
         {
             //      dataField: "CustomerProvince",
             caption: "สถานะการชำระ",
             width: 120,

         },
          {
              dataField: "CustomerID",
              caption: "ข้อมูลสัญญา",
              alignment: 'center',
              width: 150,
              allowFiltering: false,
              cellTemplate: function (container, options) {
                  console.log(options.key);
                  $("<div>")
                      .append("<a href='\PurchaseOrder'  class='btn btn-info btn-circle btn-sm' ><i class='fa fa-shopping-cart'></i></a>")
                      .appendTo(container);
              }

          }
       
        ],
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
                        DevExpress.ui.notify("Export PDF Successful!");
                    }
                }
            })
        }
    });

