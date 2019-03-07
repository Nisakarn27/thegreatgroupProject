$(function () {

    Call_Grid()

});

function Call_Grid() {

    $.ajax({
        url: '../Setting/GetZone',
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


    $("#gridListZone").dxDataGrid({
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

        allowColumnReordering: false,
        allowColumnResizing: true,
        columnAutoWidth: true,
        height: 500,
        columnFixing: {
            enabled: true
        },
        columns: [
            {
                dataField: "ID",
                caption: "No.",
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
                dataField: "Code",
                caption: "รหัสพื้นที่",
                alignment: 'center',
                width: 100,
                fixed: false,
                fixedPosition: 'left',
            },
            {
                dataField: "Value",
                caption: "ชื่อพื้นที่",
                alignment: 'left',
                

            },
            {
                dataField: "ID",
                caption: "กำหนดพนักงาน",
                alignment: 'center',
                width: 100,
                fixed: true,
                fixedPosition: 'right',
                verticalAlignment: 'middle',
                cellTemplate: function (container, options) {

                    $("<div>")
                        .append("<a href='\ManageStaffZone?zoneId=" + options.key.ID + "'  title='กำหนดพนักงาน'  class='btn btn-info btn-circle btn-sm' ><i class='fa fa-user' aria-hidden='true'></i></a>")
                        .appendTo(container);
                }
            },
            {
                dataField: "ID",
                caption: "แก้ไข",
                alignment: 'center',
                width: 60,
                fixed: true,
                fixedPosition: 'right',
                cellTemplate: function (container, options) {
                    console.log(options.data);
                    //      Load_Popup(options.data)
                    var zone = JSON.stringify(options.data);
                    $("<div>")
                        .append("<button type='link' onclick='Show_PopupEdit("+zone+")' title='แก้ไขพื้นที่'  class='btn btn-info btn-circle btn-sm' ><i class='fa fa-pencil'></i></button>")
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

                    $("<div>")
                        .append("<a  title='ลบพื้นที่'  class='btn btn-info btn-circle btn-sm' onclick='Show_PopupDelete(" + '"' + options.data.ID + '","' + options.data.Code + '","' + options.data.Value + '"' + ")'><i class='fa fa-trash'></i></a>")
//                        .append("<button type='link' onclick='Show_PopupDelete(" + '"' + options.data.CustomerName + '","' + options.data.Balance_Text + '","' + options.data.ContractID + '"' + ")' title='แก้ไขพื้นที่'  class='btn btn-info btn-circle btn-sm' ><i class='fa fa-trash'></i></button>")
                        .appendTo(container);

                   
                }
            },

        ],

    });

}
/*function NewZone() {
    alert("Success");
}*/
function Load_Popup(zone) {
    $("#modalAddEditZone").dxPopup({
        title: 'เพิ่มพื้นที่',
        visible: false,
        width: 700,
        height: 500,
        contentTemplate: function () {
            return $("<div />").append(

                $("<div class='modal-body' role='dialog'>").append(
                    //$("<div class='modal-body'>"),
                    $("<div>").append(
                        $("<div class='form-group'>").append(
                            $("<label for='recipient-name' class='col-form-label'>รหัสพื้นที่</label>"),
                            $("<input type='text' class='form-control' id='zoneCode'  >")
                        ),
                        $("</div>"),
                        $("<div class='form-group'>").append(
                            $("<label for='message-text' class='col-form-label'>ชื่อพื้นที่</label>"),
                            $("<input type='text' class='form-control' id='zoneName'  >"),
                        ),
                        $("</div>"),
                    ),
                    $("</div>"),
                    $("<div class='modal-footer float-lg-left' style='border: hidden !important;'>").append(
                        $("<button type='link' id='btnAddZone'  class='btn btn-success' onClick='AddZone();'>บันทึก</button>"),
                        $("<button type='link'onclick='hide_popup()' class='btn btn-secondary' data-dismiss='modal'>ยกเลิก</button>")
                    ),
                    $("</div>"),
                ),
                $("</div>"),

 //               $("<p class='col-md-30'>รหัสพื้นที่: <span> " + "</+ span></p>"),
 //               $("<p class='col-md-12'>ชื่อพื้นที่: <span>" + "</span></p>"),
 //               $("<div class='text-center'><button type='button' class='btn btn-success '>บันทึก</button></div>")
            );
        },
        showTitle: true,
        title: "เพิ่มพื้นที่",
        visible: false,
        dragEnabled: false,
        closeOnOutsideClick: true
    });
}

function Show_Popup(zone) {
    $("#modalAddEditZone").dxPopup("instance").show();
    $('#btnAddZone').data('data-zone', 0);
}

function Show_PopupEdit(zone) {
    console.log(zone)

    //   set ค่าใน pop up 
    $("#modalAddEditZone").dxPopup("instance").show();
    $("#zoneCode").val(zone.Code)
    $("#zoneName").val(zone.Value)
    $('#btnAddZone').data('data-zone', zone.ID);
}


function AddZone() {

    if ($("#zoneCode").val() == null || $("#zoneCode").val()=='') {
        alert("กรุณากรอกรหัสพื้นที่");
        return;
    }

    var zone = {
        ZoneID: $("#btnAddZone").data("data-zone"),
        ZoneCode: $("#zoneCode").val(),
        ZoneName: $("#zoneName").val(),
        Activated:1
    };

    if ($("#btnAddZone").data("data-zone") == 0) {

        $.ajax({
            url: '../Setting/GetAddZone',
            type: 'POST',
            data: JSON.stringify(zone),
            contentType: 'application/json',
            success: function (data) {

                //สำเร็จ
                if (data.success == true) {

                    $("#modalAddEditZone").dxPopup("instance").hide();
                    //  alert(data.data);
                    Call_Grid();
                }
                else //ไม่สำเร็จ
                {
                    alert(JSON.stringify(data));
                }
            },
            error: function () {
                console.log("error");
            }
        });
    } else {
        $.ajax({
            url: '../Setting/GetEditZone',
            type: 'POST',
            data: JSON.stringify(zone),
            contentType: 'application/json',
            success: function (data) {

                //สำเร็จ
                if (data.success == true) {

                    $("#modalAddEditZone").dxPopup("instance").hide();
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
    $("#modalAddEditZone").dxPopup("instance").hide();
    $('#btnAddZone').data('data-zone', 0);
}

function Show_PopupDelete(ID, Code, Value) {

    swal({
        title: "",
        text: "ต้องการลบพื้นที่ทำงาน รหัส \"" + Code + "\" ชื่อ \"" + Value + "\" ใช่หรือไม่ ",
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
                    url: '../Setting/GetDeleteZone?zoneID=' + ID,
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