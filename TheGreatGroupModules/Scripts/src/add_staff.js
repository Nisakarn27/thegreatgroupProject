
$(function () {

});

function NewStaff() {
    window.location = "\AddStaff";
}

var
staffs,
dataProvince,
dataDistrict ,
dataSubDistrict ,
dataStaffRole ;


if (getUrlParameter('staffID') != 0) {
    $("#panelshow").show();
    $("#loadIndicator").dxLoadIndicator({
        visible: true
    });
    $.get("../Staffs/GetListStaffs?staffID=" + getUrlParameter('staffID'))
    .done(function (result) {
      
        if (result.success) {

      
            dataStaffRole = result.dataStaffRole;

            if (getUrlParameter('staffID') != 0) {
                staffs = result.data[0];
                LoadFormEdit(staffs, dataStaffRole);
                LoadFormEditPassword(staffs);

            } else {
                staffs.push({});
               
            }
           
        } else {
            alertError(result.data);

        }
        $("#loadIndicator").dxLoadIndicator({
            visible: false
        });
    });
} else {
    $("#panelshow").hide();
    $("#loadIndicator").dxLoadIndicator({
        visible: true
    });
    $.get("../Staffs/GetListStaffs?staffID=0")
      .done(function (result) {

          if (result.success) {
             
              staffs = { "StaffID": 0, "StaffRoleID": 0, "StaffRoleName": null, "StaffCode": "", "StaffPassword": "", "StaffTitleName": "นาย", "StaffFirstName": "", "StaffLastName": "", "StaffName": null, "StaffAddress1": "", "StaffAddress2": "", "StaffSubDistrictId": 0, "StaffDistrictId": 0, "StaffProvinceId":0, "StaffZipCode": "", "StaffTelephone": "", "StaffMobile": "", "StaffEmail": ""};
              
              dataStaffRole = result.dataStaffRole;
              LoadForm(staffs, dataStaffRole);
              $("#loadIndicator").dxLoadIndicator({
                  visible: false
              });
          } else {
              alertError(result.data);

          }

      });
}
function LoadForm(staffs,dataStaffRole) {

 
    // ==================== ข้อมูลพนักงาน =====================
    $("#form").dxForm({
        colCount: 2,
        formData: staffs,
        showColonAfterLabel: true,
        showValidationSummary: true,
        items: [
            {
                dataField: "StaffTitleName",
                label: {
                    text: "คำนำหน้า"
                },
                editorType: "dxSelectBox",
                editorOptions: {
                    items: title,
                },
                isRequired: true
                    , validationRules: [{
                        type: "required",
                        message: "ต้องการคำนำหน้า"
                    }],
            }, {
                dataField: "StaffFirstName",
                label: {
                    text: "ชื่อ"
                },
                isRequired: true
                    , validationRules: [{
                        type: "required",
                        message: "ต้องการชื่อ"
                    }],
            },


        {
            dataField: "StaffLastName",
            label: {
                text: "นามสกุล"
            },
            isRequired: true
                    , validationRules: [{
                        type: "required",
                        message: "ต้องการนามสกุล"
                    }],
        },
          {
              dataField: "StaffRoleID",
              label: {
                  text: "สิทธิ์พนังงาน"
              },
              editorType: "dxLookup",
              editorOptions: {
                  dataSource: dataStaffRole,
                  valueExpr: 'StaffRoleID',
                  displayExpr: 'StaffRoleName'
              },
              isRequired: true
                    , validationRules: [{
                        type: "required",
                        message: "ต้องการสิทธิ์พนังงาน"
                    }],
          },
        {
            dataField: "StaffCode",
            label: {
                text: "ชื่อเข้าใช้งาน"
            },
            isRequired: true
                    , validationRules: [{
                        type: "required",
                        message: "ต้องการชื่อเข้าใช้งาน"
                    }],
        },


        {
            dataField: "StaffPassword",
            label: {
                text: "รหัสผ่าน"
            },
            editorOptions: {
                mode: "password"
            },
            isRequired: true
                    , validationRules: [{
                        type: "required",
                        message: "ต้องการรหัสผ่าน"
                    }],
        },

      
        {
            dataField: "StaffAddress1",
            label: {
                text: "ที่อยู่"
            },

        },
        {
            dataField: "StaffSubDistrictId",
            label: {
                text: "ตำบล"
            },
             editorType: "dxLookup",
            editorOptions: {
                dataSource: addressList.dataSubDistrict,
                valueExpr: 'SubDistrictID',
                displayExpr: 'SubDistrictName'
            },
        }, {
            dataField: "StaffDistrictId",
            label: {
                text: "อำเภอ"
            },
            editorType: "dxLookup",
            editorOptions: {
                dataSource: addressList.dataDistrict,
                valueExpr: 'DistrictID',
                displayExpr: 'DistrictName'
            },
        },
         {
             dataField: "StaffProvinceId",
             label: {
                 text: "จังหวัด"
             },
             editorType: "dxLookup",
             editorOptions: {
                 dataSource: addressList.dataProvince,
                 valueExpr: 'ProvinceID',
                 displayExpr: 'ProvinceName'
             },
         },
      {
          dataField: "StaffZipCode",
          label: {
              text: "รหัสไปรษณีย์"
          },

      },

         {
             dataField: "StaffTelephone",
             label: {
                 text: "เบอร์โทรศัพท์"
             },

         },
          {
              dataField: "StaffMobile",
              label: {
                  text: "เบอร์มือถือ"
              },
              isRequired: true
                    , validationRules: [{
                        type: "required",
                        message: "ต้องการเบอร์มือถือ"
                    }],
          },
             {
                 dataField: "StaffEmail",
                 label: {
                     text: "อีเมลล์"
                 },
               
             },
        ]
    });


}


function LoadFormEdit(staffs, dataStaffRole) {


    // ==================== ข้อมูลพนักงาน =====================
    $("#form").dxForm({
        colCount:2,
        formData: staffs,
        showColonAfterLabel: true,
        showValidationSummary: true,
        items: [
            {
                dataField: "StaffTitleName",
                label: {
                    text: "คำนำหน้า"
                },
                editorType: "dxSelectBox",
                editorOptions: {
                    items: title,
                },
                isRequired: true
                    , validationRules: [{
                        type: "required",
                        message: "ต้องการคำนำหน้า"
                    }],
            }, {
                dataField: "StaffFirstName",
                label: {
                    text: "ชื่อ"
                },
                isRequired: true
                    , validationRules: [{
                        type: "required",
                        message: "ต้องการชื่อ"
                    }],
            },


        {
            dataField: "StaffLastName",
            label: {
                text: "นามสกุล"
            },
            isRequired: true
                    , validationRules: [{
                        type: "required",
                        message: "ต้องการนามสกุล"
                    }],
        },
          {
              dataField: "StaffRoleID",
              label: {
                  text: "สิทธิ์พนังงาน"
              },
              editorType: "dxLookup",
              editorOptions: {
                  dataSource: dataStaffRole,
                  valueExpr: 'StaffRoleID',
                  displayExpr: 'StaffRoleName'
              },
              isRequired: true
                    , validationRules: [{
                        type: "required",
                        message: "ต้องการสิทธิ์พนังงาน"
                    }],
          },
        {
            dataField: "StaffCode",
            label: {
                text: "ชื่อเข้าใช้งาน"
            },
            isRequired: true
                    , validationRules: [{
                        type: "required",
                        message: "ต้องการชื่อเข้าใช้งาน"
                    }],
        },

        {
            dataField: "StaffAddress1",
            label: {
                text: "ที่อยู่"
            },

        },
        {
            dataField: "StaffSubDistrictId",
            label: {
                text: "ตำบล"
            },
            editorType: "dxLookup",
            editorOptions: {
                dataSource: addressList.dataSubDistrict,
                valueExpr: 'SubDistrictID',
                displayExpr: 'SubDistrictName'
            },
        }, {
            dataField: "StaffDistrictId",
            label: {
                text: "อำเภอ"
            },
            editorType: "dxLookup",
            editorOptions: {
                dataSource: addressList.dataDistrict,
                valueExpr: 'DistrictID',
                displayExpr: 'DistrictName'
            },
        },
         {
             dataField: "StaffProvinceId",
             label: {
                 text: "จังหวัด"
             },
             editorType: "dxLookup",
             editorOptions: {
                 dataSource: addressList.dataProvince,
                 valueExpr: 'ProvinceID',
                 displayExpr: 'ProvinceName'
             },
         },
      {
          dataField: "StaffZipCode",
          label: {
              text: "รหัสไปรษณีย์"
          },

      },

         {
             dataField: "StaffTelephone",
             label: {
                 text: "เบอร์โทรศัพท์"
             },

         },
          {
              dataField: "StaffMobile",
              label: {
                  text: "เบอร์มือถือ"
              },
              isRequired: true
                    , validationRules: [{
                        type: "required",
                        message: "ต้องการเบอร์มือถือ"
                    }],
          },
             {
                 dataField: "StaffEmail",
                 label: {
                     text: "อีเมลล์"
                 },

             },
        ]
    });


}



function LoadFormEditPassword(staffs) {
 
    // ==================== ข้อมูลพนักงาน =====================
    var formWidget = $("#form1").dxForm({
        colCount: 1,
        formData: staffs,
        showColonAfterLabel: true,
        showValidationSummary: true,
        items: [
        {
            dataField: "StaffPassword",
            label: {
                text: "รหัสผ่าน"
            },

            editorOptions: {
                mode: "password"
            },
            validationRules: [{
                type: "required",
                message: "จำเป็นต้องใส่รหัสผ่าน"
            }]
        }, {
            dataField: "StaffPasswordConfirm",
            label: {
                text: "ยืนยันรหัสผ่าน"
            },
            editorType: "dxTextBox",
            editorOptions: {
                mode: "password"
            },
            validationRules: [{
                type: "required",
                message: "จำเป็นต้องยืนยันรหัสผ่าน"
            }, ]
        }
         
        ]
    });


}




if (getUrlParameter('staffID') != 0) {

    

    $("#myButton").dxButton({
        text: 'แก้ไขข้อมูลพนักงาน',
        type: 'success',
        useSubmitBehavior: true,
        onClick: function () {
            var staff = $("#form").dxForm("instance").option('formData');
            var result = $("#form").dxForm("instance").validate();
            if (result.isValid) {

                $.ajax({
                    url: '../Staffs/EditStaffs',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(staff),
                    success: function (data) {
                        if (data.success == true) {

                            DevExpress.ui.notify({
                                message: "บันทึกข้อมูลพนักงานสำเร็จ !!!",
                            }, "success", 3000);

                            window.location = "\ListStaff";
                        } else {

                            DevExpress.ui.notify(data.data);
                        }
                    },
                    error: function () {
                        console.log("error");
                    }
                });
            }
        }
    });


    $("#myButton1").dxButton({
        text: 'แก้ไขรหัสผ่าน',
        type: 'success',
        useSubmitBehavior: true,
        onClick: function () {
            var staff1 = $("#form1").dxForm("instance").option('formData');
            var result1 = $("#form1").dxForm("instance").validate();
  
            if (result1.isValid) {
                if (staff1.StaffPassword == staff1.StaffPasswordConfirm) {

                    $.ajax({
                        url: '../Staffs/EditStaffPassword',
                        type: 'POST',
                        contentType: 'application/json',
                        data: JSON.stringify(staff1),
                        success: function (data) {
                            if (data.success == true) {

                                DevExpress.ui.notify({
                                    message: "แก้ไขรหัสผ่านสำเร็จ !!!",
                                }, "success", 3000);

                           //     window.location = "\ListStaff";
                            } else {

                                DevExpress.ui.notify(data.data);
                            }
                        },
                        error: function () {
                            console.log("error");
                        }
                    });
                } else {



                    DevExpress.ui.notify("รหัสผ่านกับยืนยันรหัสผ่าน ไม่เท่ากัน");
                }
            }
        }
    });

}
else {


    $("#myButton").dxButton({
        text: 'เพิ่มพนักงาน',
        type: 'success',
        useSubmitBehavior: true,
        onClick: function () {
            var staff = $("#form").dxForm("instance").option('formData');
            var result = $("#form").dxForm("instance").validate();
            if (result.isValid) {

                $.ajax({
                    url: '../Staffs/AddStaffs',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(staff),
                    success: function (data) {
                        if (data.success == true) {

                            DevExpress.ui.notify({
                                message: "บันทึกข้อมูลพนักงานสำเร็จ !!!",
                            }, "success", 3000);

                            window.location = "\ListStaff";
                        } else {

                            DevExpress.ui.notify(data.data);
                        }
                    },
                    error: function () {
                        console.log("error");
                    }
                });
            }
        }
    });

}


