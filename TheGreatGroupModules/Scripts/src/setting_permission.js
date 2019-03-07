
$(function () {
    var data = [];
    Call_Grid();
});


function Call_Grid() {

    $.ajax({
        url: '../Staffs/GetStaffPermission?staffroleID=' + getUrlParameter('staffID'),
        type: 'GET',
        contentType: 'application/json',
        success: function (data) {

            if (data.success) {
                Load_DataGrid(data);
                $("#lblStaffRoleName").html("<h4> กลุ่มพนักงาน : "+data.dataStaffRole[0].StaffRoleName+"</h4>");
                console.log(data.dataSelect);
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

    $("#gridContainer").dxDataGrid({
        dataSource: data.data,
        keyExpr: "StaffPermissionID",


        showColumnLines: true,
        showRowLines: false,
        //  rowAlternationEnabled: true,
        showBorders: true,
        selection: {
            mode: "multiple"
        },
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


        columns: [{
            dataField: "StaffPermissionName",
            caption: "สิทธิ์พนักงาน",
            width: 300 +"%",
            alignment: 'left',
            allowFiltering: false
        }, {
            dataField: "StaffPermissionGroupName",
            caption: "กลุ่ม",
            groupIndex: 0,
            width: 100 + "%",
        },
          
            

        ],
    });

}



$("#myButton").dxButton({
    text: 'บันทึกข้อมูล',
    type: 'success',
    useSubmitBehavior: true,
    onClick: function () {
       
        var ItemSelect = $("#gridContainer").dxDataGrid("instance").option('selectedRowKeys');
        console.log( $("#gridContainer").dxDataGrid("instance").option('selectedRowKeys'));
            $.ajax({
                url: '../staffs/GetAddStaffPermission?staffRoleID='+ getUrlParameter('staffID'),
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(ItemSelect),
                success: function (data) {
                    if (data.success == true) {

                        DevExpress.ui.notify({
                            message: "บันทึกข้อมูลสำเร็จ !!!",
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
});