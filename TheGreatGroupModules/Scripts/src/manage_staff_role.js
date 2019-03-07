$(function () {

    Call_Grid()

});

function Call_Grid() {

    $.ajax({
        url: '../Staffs/GetListStaffRole?staffroleID=0',
        type: 'GET',
        contentType: 'application/json',
        success: function (data) {

            Load_DataGrid(data);
            Load_Popup(data);
            // เรียก data grid เพื่อใส่ข้อมูล

        },
        error: function () {
            console.log("error");
        }
    });
}

function Load_DataGrid(data) {

    $("#gridListStaffRole").dxDataGrid({
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
                dataField: "StaffRoleID",
                caption: "ลำดับ",
                width: 50,
                alignment: 'center',
                allowFiltering: true,
                fixed: false,
                fixedPosition: 'left',
            },
            {
                dataField: "StaffRoleName",
                caption: "ชื่อกลุ่มพนักงาน",
                alignment: 'left',
                fixed: false,
                fixedPosition: 'left',
            },
            {
                dataField: "Activated",
                caption: "สถานะ",
                alignment: 'center',
                width: 200,
                cellTemplate: function (container, options) {
                    $("<div>")
                        .append(options.value == 1 ? "เปิดใช้งาน" : "ปิดใช้งาน")
                        .appendTo(container);
                }


            },
               {
                   dataField: "StaffID",
                   caption: "กำหนดสิทธิ์พนักงาน",
                   alignment: 'center',
                   width: 130,
                   fixed: true,
                   fixedPosition: 'right',
                   cellTemplate: function (container, options) {

                       var staffRole = JSON.stringify(options.data);
                       $("<div>")
                           .append("<a href='../Staffs/SettingPermission?staffID=" + options.data.StaffRoleID + "' title='กำหนดสิทธิ์พนักงาน'  class='btn btn-info btn-circle btn-sm' ><i class='fa fa-gear'></i></a>")
                           .appendTo(container);
                   }
               },
            {
                dataField: "StaffID",
                caption: "แก้ไข",
                alignment: 'center',
                width: 50,
                fixed: true,
                fixedPosition: 'right',
                cellTemplate: function (container, options) {

                    var staffRole = JSON.stringify(options.data);
                    $("<div>")
                        .append("<button type='link' onclick='Show_PopupEdit(" + staffRole + ")' title='แก้ไขสิทธิ์พนักงาน'  class='btn btn-info btn-circle btn-sm' ><i class='fa fa-pencil'></i></button>")
                        .appendTo(container);
                }
            },
            {
                dataField: "ID",
                caption: "ลบ",
                alignment: 'center',
                width: 50,
                fixed: true,
                fixedPosition: 'right',
                cellTemplate: function (container, options) {

                    $("<div>")
                        .append("<a  title='ลบกลุ่มพนักงาน'  class='btn btn-info btn-circle btn-sm' onclick='Show_PopupDelete(" + '"' + options.data.StaffRoleID + '","' + options.data.StaffRoleName + '"' + ")'><i class='fa fa-trash'></i></a>")
                        .appendTo(container);
                }
            },

        ],

    });
}

function toggleState(item) {
    if (item.className == "on") {
        item.className = "off";
    } else {
        item.className = "on";
    }
}

function Load_Popup(staffRole) {
    $("#modalAddEditStaffRole").dxPopup({
        title: 'เพิ่มพื้นที่',
        visible: false,
        width: 700,
        height: 500,
        contentTemplate: function () {
            return $("<div />").append(
                "<div class='modal-body' role='dialog'>"+
               " <div><div class='form-group'>"+
              " <label for='recipient-name' class='col-form-label'>ชื่อกลุ่มพนักงาน</label>"+
           " <input type='text' class='form-control' id='StaffRoleName'  >"+
           "</div>"+
          "</div>"+
          "<div class='modal-footer float-lg-left' style='border: hidden !important;'>"+
         " <button type='link' id='btnAddStaffRole'  class='btn btn-success' onClick='AddStaffRole();'>บันทึก</button>"+
         "<button type='link'onclick='hide_popup()' class='btn btn-secondary' data-dismiss='modal'>ยกเลิก</button>"+
         "</div>"+
          "</div>"

            );
        },
        showTitle: true,
        title: "เพิ่มกลุ่มพนักงาน",
        visible: false,
        dragEnabled: false,
        closeOnOutsideClick: true
    });
}

function Show_PopupAdd(staffRole) {
    $("#modalAddEditStaffRole").dxPopup("instance").show();
    $('#btnAddStaffRole').data('data-zone', 0);
}

function Show_PopupEdit(staffRole) {

    //   set ค่าใน pop up 
    $("#modalAddEditStaffRole").dxPopup("instance").show();
    $("#StaffRoleName").val(staffRole.StaffRoleName)
    $('#btnAddStaffRole').data('data-zone', staffRole.StaffRoleID);
}

function AddStaffRole() {

    if ($("#StaffRoleName").val() == null || $("#StaffRoleName").val() == '') {
        alert("กรุณากรอกสิทธิ์พนักงาน");
        return;
    }

    var staffRole = {
        staffRoleID: $("#btnAddStaffRole").data("data-zone"),
        StaffRoleName: $("#StaffRoleName").val(),
        Activated: 1
    };

    if ($("#btnAddStaffRole").data("data-zone") == 0) {

        $.ajax({
            url: '../Staffs/AddStaffRole',
            type: 'POST',
            data: JSON.stringify(staffRole),
            contentType: 'application/json',
            success: function (data) {

                //สำเร็จ
                if (data.success == true) {

                    $("#modalAddEditStaffRole").dxPopup("instance").hide();
                    //  alert(data.data);
                    Call_Grid();
                }
                else //ไม่สำเร็จ
                {
                    alert(data);
                }
            },
            error: function () {
                console.log("error");
            }
        });
    } else {
        $.ajax({
            url: '../Staffs/EditStaffRole',
            type: 'POST',
            data: JSON.stringify(staffRole),
            contentType: 'application/json',
            success: function (data) {

                //สำเร็จ
                if (data.success == true) {

                    $("#modalAddEditStaffRole").dxPopup("instance").hide();
                    //  alert(data.data);
                    Call_Grid();
                }
                else //ไม่สำเร็จ
                {
                    alert(data);
                }
            },
            error: function () {
                console.log("error");
            }
        });

    }


}

function hide_popup(zone) {
    $("#modalAddEditStaffRole").dxPopup("instance").hide();
    $('#btnAddStaffRole').data('data-zone', 0);
}

function Show_PopupDelete(StaffRoleID, StaffRoleName) {

    swal({
        title: "",
        text: "ต้องการลบกลุ่มพนักงาน \"" + StaffRoleName + "\" ใช่หรือไม่ ",
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
                swal("สำเร็จ !", "", "success");

                $.ajax({
                    url: '../Staffs/DeleteStaffRole?staffroleId=' + StaffRoleID,
                    type: 'GET',
                    contentType: 'application/json',
                    success: function (data) {
                        Call_Grid()

                    },
                    error: function () {
                        console.log("error");
                    }
                });

            } else {
                swal.close();
                // swal("ยกเลิก", "", "error");
                e.preventDefault();
            }
        });

}