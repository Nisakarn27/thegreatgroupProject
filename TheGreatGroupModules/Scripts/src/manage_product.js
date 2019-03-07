

$(function () {

    Load_DataGrid()

});

function Load_DataGrid() {
  
    $.ajax({
        url: '../Products/GetListProduct/0',
        type: 'GET',
        contentType: 'application/json',
        success: function (data) {
            console.log(data);
            var rownum = 0;
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
                        dataField: "ID",
                        caption: "ลำดับ",
                        width: 50+"%",
                        alignment: 'center',
                        allowFiltering: false,
                        fixed: false,
                        fixedPosition: 'left',
                        cellTemplate: function (container, options) {
                            rownum = rownum + 1;
                            $("<div>")
                                .append(rownum)
                                .appendTo(container);
                        }
                    },
                    {
                        dataField: "ProductGroupName",
                        caption: "ประเภทสินค้า",
                        alignment: 'left',
                        width: 200 + "%",
                        fixed: false,
                        fixedPosition: 'left',
                    },
                    {
                        dataField: "ProductName",
                        caption: "ชื่อสินค้า",
                        alignment: 'left',
                        width: 250 + "%",
                    },
                       {
                           dataField: "UnitAmount",
                           caption: "จำนวน",
                           alignment: 'right',
                           width: 120 + "%",
                       },
                        {
                            dataField: "UnitName",
                            caption: "หน่วย",
                            alignment: 'left',
                            width: 120 + "%",
                        },
                          {
                              dataField: "ProductPrice_Text",
                              caption: "ราคา/บาท",
                              alignment: 'right',
                              width: 120 + "%",
                          },
                       {
                           dataField: "Activated",
                           caption: "สถานะใช้งาน",
                           alignment: 'center',
                           width: 100 + "%",
                           //fixed: true,
                           //fixedPosition: 'right',
                           cellTemplate: function (container, options) {
                               var pro = options.data;

                               if (pro.Activated == 1) {

                                   $("<div>")
                                      .append("<span class='badge badge-success'> เปิดใช้งาน </span>")
                                      .appendTo(container);
                               } else {
                                   $("<div>")
                                        .append("<span class='badge badge-error'> ปิดใช้งาน </span>")
                                        .appendTo(container);

                               }

                           }
                       },

                          {
                              dataField: "ProductID",
                              caption: "เปิดใช้งาน",
                              alignment: 'center',
                              width: 60 + "%",
                              fixed: true,
                              fixedPosition: 'right',
                              cellTemplate: function (container, options) {
                                  var pro = options.data;
                                  $("<div>")
                                      .append("<a  title='เปิดใช้งาน' onclick='ActivatedProduct(" + pro.ProductID + ")' class='btn btn-info btn-circle btn-sm' ><i class='fa fa-toggle-on'></i></a>")
                                      .appendTo(container);
                              }
                          },
                       
                    {
                        dataField: "ProductID",
                        caption: "แก้ไข",
                        alignment: 'center',
                        width: 60 + "%",
                        fixed: true,
                        fixedPosition: 'right',
                        cellTemplate: function (container, options) {
                            var pro = options.data;
                            $("<div>")
                                .append("<a href='\EditProduct?ProductID=" + pro.ProductID + "' title='แก้ไขพนักงาน'  class='btn btn-info btn-circle btn-sm' ><i class='fa fa-pencil'></i></a>")
                                .appendTo(container);
                        }
                    },
                    {
                        dataField: "ProductID",
                        caption: "ลบ",
                        alignment: 'center',
                        width: 60 + "%",
                        fixed: true,
                        fixedPosition: 'right',
                        cellTemplate: function (container, options) {
                            var pro = options.data;
                            $("<div>")
                                .append("<a  title='ลบพนักงาน' onclick='DeteteProduct(" + pro.ProductID + ")' class='btn btn-info btn-circle btn-sm' ><i class='fa fa-trash'></i></a>")
                                .appendTo(container);
                        }
                    },

                ],

            });

        },
        error: function () {
            console.log("error");
        }
    });

    

}

function DeteteProduct(id){
    swal({
        title: "",
        text: "ต้องการลบสินค้าหรือไม่ ?",
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



        $.ajax({
            url: '../Products/DeletedProduct/' + id,
            type: 'GET',
            contentType: 'application/json',
            success: function (data) {
                if (data.success) {

                    swal("สำเร็จ !", "ลบข้อมูลสำเร็จ !!", "success");
                }

                // เรียก data grid 
                Load_DataGrid();

            },
            error: function () {
                console.log("error");
            }
        });




    } else {
        swal.close();
        e.preventDefault();
    }
});






}


function ActivatedProduct(id) {

    swal({
        title: "",
        text: "ต้องการเปิด/ปิดใช้งาน  สินค้านี้หรือไม่ ?",
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





        $.ajax({
            url: '../Products/ActivatedProduct/' + id,
            type: 'GET',
            contentType: 'application/json',
            success: function (data) {
                if (data.success) {

                    swal("สำเร็จ !", "เปิดใช้งานข้อมูลสำเร็จ !!", "success");
                }

                // เรียก data grid 
                Load_DataGrid();

            },
            error: function () {
                console.log("error");
            }
        });





    } else {
        swal.close();
        e.preventDefault();
    }
});






}


function NewProduct() {

    window.location = "\AddProduct";
}