$(function () {



    Call_Grid()

});

function Call_Grid() {

    $.ajax({
        url: '../Setting/GetListHolidays?HolidayID=0',
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

    var rownum = 0;

    $("#gridListHoliday").dxDataGrid({
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
                width: 50,
                alignment: 'center',
                allowFiltering: true,
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
                dataField: "HolidayName",
                caption: "ชื่อวันหยุด",
                alignment: 'left',
                fixed: false,
                fixedPosition: 'left',
            },
            {
                dataField: "Date_str",
                caption: "วัน/เดือน/ปี",
                alignment: 'center',
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
                dataField: "ID",
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
                        .append("<a  title='ลบวันหยุดนักขัตฤกษ์'  class='btn btn-info btn-circle btn-sm' onclick='Show_PopupDelete(" + '"' + options.data.ID + '","' + options.data.HolidayName + '"' + ")'><i class='fa fa-trash'></i></a>")
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
    $("#modalAddEditHoliday").dxPopup({
        title: 'เพิ่มพื้นที่',
        visible: false,
        width: 700,
        height: 500,
        contentTemplate: function () {
            return $("<div />").append(
                "<div class='modal-body' role='dialog'>" +
                " <div><div class='form-group'>" +
                " <label for='recipient-name' class='col-form-label'>ชื่อวันหยุดนักขัตฤกษ์</label>" +
                " <input type='text' class='form-control' id='HolidayName'  >" +
                " <br />" +
                " <label for='recipient-name' class='col-form-label'>วัน/เดือน/ปี</label>" +
                " <input type='text' id='DateAsOf' class='form-control' placeholder='วัน/เดือน/ปี' value=''>" +

                "</div>" +
                "</div>" +
                "<div class='modal-footer float-lg-left' style='border: hidden !important;'>" +
                " <button type='link' id='btnAddHoliday'  class='btn btn-success' onClick='AddHoliday();'>บันทึก</button>" +
                "<button type='link'onclick='hide_popup()' class='btn btn-secondary' data-dismiss='modal'>ยกเลิก</button>" +
                "</div>" +
                "</div>"

            );
        },
        showTitle: true,
        title: "เพิ่มวันหยุดนักขัตฤกษ์",
        visible: false,
        dragEnabled: false,
        closeOnOutsideClick: true
    });
}

function Show_PopupAdd(staffRole) {
    $("#modalAddEditHoliday").dxPopup("instance").show();
    $('#btnAddHoliday').data('data-zone', 0);



}

function Show_PopupEdit(staffRole) {

    //   set ค่าใน pop up 
    $("#modalAddEditHoliday").dxPopup("instance").show();
    $("#StaffRoleName").val(staffRole.StaffRoleName)
    $('#btnAddHoliday').data('data-zone', staffRole.StaffRoleID);
}

function AddHoliday() {

    if ($("#HolidayName").val() == null || $("#HolidayName").val() == '') {
        alert("กรุณากรอกวันหยุดนักขัตฤกษ์");
        return;
    }

    if ($("#DateAsOf").val() == null || $("#DateAsOf").val() == '') {
        alert("กรุณาเลือก วันที่/เดือน/ปี");
        return;
    }

    var holiday = {
        ID: $("#btnAddHoliday").data("data-zone"),
        HolidayName: $("#HolidayName").val(),
        Date: $("#DateAsOf").val(),
        Activated: 1
    };

    if ($("#btnAddHoliday").data("data-zone") == 0) {

        $.ajax({
            url: '../Setting/AddHoliday',
            type: 'POST',
            data: JSON.stringify(holiday),
            contentType: 'application/json',
            success: function (data) {

                //สำเร็จ
                if (data.success == true) {

                    $("#modalAddEditHoliday").dxPopup("instance").hide();
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
            url: '../Setting/EditHoliday',
            type: 'POST',
            data: JSON.stringify(staffRole),
            contentType: 'application/json',
            success: function (data) {

                //สำเร็จ
                if (data.success == true) {

                    $("#modalAddEditHoliday").dxPopup("instance").hide();
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
    $("#modalAddEditHoliday").dxPopup("instance").hide();
    $('#btnAddHoliday').data('data-zone', 0);
}

function Show_PopupDelete(ID, HolidayName) {

    console.log(ID);
    swal({
        title: "",
        text: "ต้องการลบ \"" + HolidayName + "\" ใช่หรือไม่ ",
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
                    url: '../Setting/DeleteHoliday?holidayId=' + ID,
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