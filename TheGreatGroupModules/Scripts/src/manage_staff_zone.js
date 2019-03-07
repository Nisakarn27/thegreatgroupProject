$(function () {
    $.fn.datepicker.defaults.language = 'th';


    $.get("../Staffs/GetZone")
        .done(function (data) {
            if (data.success == true) {

                $.each(data.data, function (key, value) {
                    if (value.ID == getUrlParameter('zoneId'))
                        $("#zoneId").html(value.Value);

                });
            }
            SearchCustomer();
        });

    //              $.get("../Staffs/GetStaffs?staffroleid=5&zoneId=" + getUrlParameter('zoneId'))
    $.get("../Staffs/GetStaffData?staffID=0&staffroleId=0")
        .done(function (data) {
            console.log(data);
            if (data.success == true) {

                $.each(data.data, function (key, value) {
                    $("#CustomerID").append('<option value="' + value.StaffID + '"' + ">" + value.StaffName + "</option>");
                });
            }
        });




});


function SearchCustomer() {
    var rownum = 0;

    $.get("../Staffs/GetStaffs?staffroleid=5&zoneId=" + getUrlParameter('zoneId'))
        .done(function (data) {
            if (data.success == true) {
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
                            dataField: "ID",
                            caption: "ลำดับ",
                            width: 20 + "%",
                            alignment: 'center',
                            allowFiltering: false,
                            fixed: false,
                            cellTemplate: function (container, options) {
                                rownum = rownum + 1;
                                $("<div>")
                                    .append(rownum)
                                    .appendTo(container);
                            }
                        },
                        {
                            dataField: "Value",
                            caption: "ชื่อ-นามสกุล",
                            width: 230 + "%",
                            fixed: false,
                            fixedPosition: 'left',
                        },
                        {
                            dataField: "ID",
                            caption: "ลบ",
                            alignment: 'center',
                            width: 120 + "%",
                            cellTemplate: function (container, options) {
                                $("<div>")
                                    .append("<a onclick='DeleteStaffZone(" + options.key.ID + ")' title='ลบ' class='btn btn-info btn-circle btn-sm' ><i class='fa fa-trash'></i></a>")
                                    .appendTo(container);
                            }
                        },
                    ]
                });
            }
        });
}

function DeleteStaffZone(id) {

    var staffzone = {

        StaffID: id,
        ZoneID: getUrlParameter('zoneId')
    };
    $.post("../Setting/GetDeleteStaffZone", staffzone)
        .done(function (data) {
            console.log(data);
            if (data.success == true) {

                alert(data.data);
                SearchCustomer();
            }
        });

}
function AddStaffZone() {
    if ($("#CustomerID").val() != "") {

        var staffzone = {

            StaffID: $("#CustomerID").val(),
            ZoneID: getUrlParameter('zoneId')
        };
        $.post("../Setting/GetAddStaffZone", staffzone)
            .done(function (data) {
                console.log(data);
                if (data.success == true) {

                    alert(data.data);
                    SearchCustomer();
                } else {

                    alert(data.data);
                    SearchCustomer();
                }
            });


    } else {
        alert("กรุณาเลือกพนักงาน");

    }


}
