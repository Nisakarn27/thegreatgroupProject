function Load_DataGrid() {
  
    $.ajax({
        url: '../Staffs/GetStaffData?staffID=0&staffroleId=0',
        type: 'GET',
        contentType: 'application/json',
        success: function (data) {
            console.log(data)
            var rownum = 0;
            $("#gridListStaffs").dxDataGrid({
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
                        dataField: "StaffID",
                        caption: "ลำดับ",
                        width: 50,
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
                        dataField: "StaffCode",
                        caption: "รหัสพนักงาน",
                        alignment: 'center',
                        width: 100,
                        fixed: false,
                        fixedPosition: 'left',
                    },
                    {
                        dataField: "StaffFirstName",
                        caption: "ชื่อ",
                        alignment: 'left',


                    },
                    {
                        dataField: "StaffLastName",
                        caption: "นามสกุล",
                        alignment: 'left',


                    },
                    {
                        dataField: "StaffRoleName",
                        caption: "ตำแหน่ง",
                        alignment: 'left',


                    },
                       {
                           dataField: "Activated",
                           caption: "สถานะใช้งาน",
                           alignment: 'center',
                           width: 100,
                           fixed: true,
                           fixedPosition: 'right',
                           cellTemplate: function (container, options) {
                               var staffid = options.data;

                               if (staffid.Activated == 1) {

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
                           dataField: "ID",
                           caption: "เปิด/ปิด ใช้งาน",
                           alignment: 'center',
                           width: 120,
                           fixed: true,
                           fixedPosition: 'right',
                           cellTemplate: function (container, options) {
                               var staffid = options.data;
                               
                               if (staffid.Activated == 1) {

                               $("<div>")
                                   .append("<a  title='ปิดใช้งาน' onclick='ActivateStaff(" + staffid.StaffID + ")'  class='btn btn-info btn-circle btn-sm' ><i class='fa fa-toggle-on'></i></a>")
                                   .appendTo(container);
                               } else {
                               $("<div>")
                                  .append("<a  title='เปิดใช้งาน' onclick='ActivateStaff(" + staffid.StaffID + ")'  class='btn btn-warning btn-circle btn-sm' ><i class='fa fa-toggle-off'></i></a>")
                                  .appendTo(container);
                               }
                           }
                       },
                    {
                        dataField: "StaffID",
                        caption: "แก้ไข",
                        alignment: 'center',
                        width: 60,
                        fixed: true,
                        fixedPosition: 'right',
                        cellTemplate: function (container, options) {
                            var staffid = options.data;
                            $("<div>")
                                .append("<a href='\EditStaff?staffID=" + staffid.StaffID + "' title='แก้ไขพนักงาน'  class='btn btn-info btn-circle btn-sm' ><i class='fa fa-pencil'></i></a>")
                                .appendTo(container);
                        }
                    },
                    {
                        dataField: "ID",
                        caption: "ลบ",
                        alignment: 'center',
                        width: 60,
                        fixed: true,
                        fixedPosition: 'right',
                        cellTemplate: function (container, options) {
                            var staffid = options.data;
                            $("<div>")
                                .append("<a  title='ลบพนักงาน' onclick='DeteteStaff(" + staffid.StaffID + ")' class='btn btn-info btn-circle btn-sm' ><i class='fa fa-trash'></i></a>")
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


function ActivateStaff(id) {
    $.ajax({
        url: '../Staffs/ActivatedStaffs?StaffID=' + id,
        type: 'GET',
        contentType: 'application/json',
        success: function (data) {
            if (data.success) {

                swal("สำเร็จ !", "บันทึกข้อมูลสำเร็จ !!", "success");
            }
            // เรียก data grid 
            Load_DataGrid();
          
        },
        error: function () {
            console.log("error");
        }
    });

}


function DeteteStaff(id) {
    swal({
        title: "",
        text: "ต้องการลบพนักงานหรือไม่ ?",
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


        Detete(id)


    } else {
        swal.close();
        e.preventDefault();
    }
});


}


function Detete(id) {
    $.ajax({
        url: '../Staffs/DeletedStaffs?StaffID=' + id,
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


}