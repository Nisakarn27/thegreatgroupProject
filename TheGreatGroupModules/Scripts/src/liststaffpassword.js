
$(function () {
    var data = [];
    Call_Grid();
});


function Call_Grid() {

    $.ajax({
        url: '../staffs/GetStaffSettingOTP',
        type: 'GET',
        contentType: 'application/json',
        success: function (data) {

            if (data.success) {
                Load_DataGrid(data);
            } else {
                DevExpress.ui.notify(data.data);

            }

        },
        error: function () {
            console.log("error");
        }
    });
}

function Load_DataGrid(data) {

    $("#gridList").dxDataGrid({
        dataSource: data.data,
        showColumnLines: true,
        showRowLines: false,
        //  rowAlternationEnabled: true,
        showBorders: true,
        selection: {
            mode: "multiple"
        },
        selectedRowKeys: data.dataSelect,
        paging: {
            enabled: false
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
        selectedRowKeys: data.dataSelect,
        allowColumnReordering: true,
        allowColumnResizing: true,
        columnAutoWidth: true,
        height: 500,
        columnFixing: {
            enabled: true
        },
        keyExpr: "StaffID",


        columns: [
            {
                dataField: "StaffRoleName",
                caption: "กลุ่มพนักงาน",
                width: 100 + "%",
            },
            {
                dataField: "StaffCode",
                caption: "รหัสพนักงาน",
                width: 300 + "%",
                alignment: 'left',
                allowFiltering: false
            },
    
        {
            dataField: "StaffName",
            caption: "ชื่อ-นามสกุล พนักงาน",
            width: 300 + "%",
            alignment: 'left',
            allowFiltering: false
        },
        {
            dataField: "OTP",
            caption: "Password",
            width: 100 + "%",
            alignment: 'center',
        },



        ],
    });

}
function AddPassword() {

    var ItemSelect = $("#gridList").dxDataGrid("instance").option('selectedRowKeys');
    console.log($("#gridList").dxDataGrid("instance").option('selectedRowKeys'));
    $.ajax({
        url: '../staffs/AddPermission',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(ItemSelect),
        success: function (data) {
            if (data.success == true) {

                DevExpress.ui.notify({
                    message: "สร้างรหัสผ่านสำเร็จ !!!",
                }, "success", 3000);
                Call_Grid();
            } else {

                DevExpress.ui.notify({
                    message: data.data,
                }, "error", 3000);
                Call_Grid();
            }
        },
        error: function () {
            console.log("error");
        }
    });

}


function DeletePassword() {

    var ItemSelect = $("#gridList").dxDataGrid("instance").option('selectedRowKeys');
    console.log($("#gridList").dxDataGrid("instance").option('selectedRowKeys'));
    $.ajax({
        url: '../staffs/DeletePassword',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(ItemSelect),
        success: function (data) {
            if (data.success == true) {

                DevExpress.ui.notify({
                    message: "ลบรหัสผ่านสำเร็จ !!!",
                }, "success", 3000);
                Call_Grid();
            } else {

                DevExpress.ui.notify({
                    message: data.data,
                }, "error", 3000);
                Call_Grid();
            }
        },
        error: function () {
            console.log("error");
        }
    });

}
