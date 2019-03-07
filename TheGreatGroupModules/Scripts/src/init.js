$(function () {
    //LoadMenu();

    $("#side-menu").html($("#side-menu").attr("data-menu"));
 
});

var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};


var customerListstatus = [
     "หย่าร้าง",
     "สมรส",
      "โสด",
];
var title = [
      "",
      "นาย",
      "นาง",
      "นางสาว",
];
var Career = [
    "รับราชการ",
     "รัฐวิสาหกิจ",
     "บริษัทเอกชน",
     "ธุรกิจส่วนตัว",
     "อาชีพอิสระ",
      "อื่นๆ",
];


var Months = [
            {
                ID: 1,
                Name: "มกราคม"

            },
            {
                ID: 2,
                Name: "กุมภาพันธ์"
            },
            {
                ID: 3,
                Name: "มีนาคม "
            },
            {
                ID: 4,
                Name: "เมษายน "
            },
             {
                 ID: 5,
                 Name: "พฤษภาคม "

             },
            {
                ID: 6,
                Name: "มิถุนายน"
            },
            {
                ID: 7,
                Name: "กรกฏาคม"
            },
            {
                ID: 8,
                Name: "สิงหาคม"
            },
            {
                ID: 9,
                Name: "กันยายน"
            },
            {
                ID: 10,
                Name: "ตุลาคม"
            },
            {
            ID: 11,
            Name: "พฤศจิกายน"
            },
            {
                ID: 12,
                Name: "ธันวาคม"
            }

];

var Years = GetYear();
    
    function  GetYear() {
    var yearArr = new Array();
    var YearNow = new Date().getFullYear()
    for (var i = 0; i <10  ; i++) {
        YearNow = YearNow - i;
        yearArr.push(YearNow);

    }
    return yearArr;
}
  //  [2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023, 2024, 2025, 2026];

    function LoadForm_CustomerInfo(
        datasourceCustomer,
        dataZone
        ) {


        $("#form").dxForm({
            formData: datasourceCustomer,
            readOnly: false,
            showColonAfterLabel: true,
            showValidationSummary: true,
            validationGroup: "customerData",
            items: [
            {

                itemType: "group",
                colCount: 3,
                items: [{
                    itemType: "group",
                    caption: "ข้อมูลลูกค้า",
                    items: [{
                        dataField: "CustomerTitleName",
                        editorType: "dxSelectBox",
                        editorOptions: {
                            items: title
                        },
                        label: {
                            text: "คำนำหน้า"
                        },
                        isRequired: true
                    },
                    {
                        dataField: "CustomerFirstName",
                        label: {
                            text: "ชื่อ"
                        }, isRequired: true
                        , validationRules: [{
                            type: "required",
                            message: "ต้องการชื่อลูกค้า"
                        }],
                  
                    }, {
                        dataField: "CustomerLastName",

                        label: {
                            text: "นามสกุล"
                        }
                        , isRequired: true,
                        validationRules: [{
                            type: "required",
                            message: "ต้องการนามสกุลลูกค้า"
                        }],
                    },
                    {
                        dataField: "CustomerNickName",
                        label: {
                            text: "ชื่อเล่น"
                        },
                    
                    }, {
                        dataField: "CustomerIdCard",
                        label: {
                            text: "รหัสประจำตัวประชาชน"
                        },
                        isRequired: true,

                        validationRules: [{
                            type: "stringLength",
                            max: 13,
                            min: 13,
                            message: "รหัสประจำตัวประชาชนต้องมี 13 หลัก"
                        }, ],
                    },
                    {
                        dataField: "CustomerTelephone",

                        label: {
                            text: "เบอร์โทรศัพท์"
                        },
                        isRequired: false
                    },
                     {
                         dataField: "CustomerMobile",
                         label: {
                             text: "เบอร์มือถือ"
                         },
                         isRequired: true,
                         validationRules: [{
                             type: "required",
                             message: "ต้องการเบอร์มือถือลูกค้า"
                         }],
                     }, {
                         dataField: "CustomerStatus",
                         editorOptions: {
                             items: customerListstatus
                         },
                         editorType: "dxLookup",
                         label: {
                             text: "สถานภาพ"
                         }
                        , isRequired: true
                     }, {
                         dataField: "CustomerEmail",
                         label: {
                             text: "อีเมลล์"
                         }
                     },
                    ]
                }, {
                    itemType: "group",
                    caption: "ที่อยู่ลูกค้า",
                    items: [
                        {
                            dataField: "CustomerAddress1",
                            label: {
                                text: "ที่อยู่ปัจจุบัน"
                            },
                            isRequired: true
                        },

                    {
                        dataField: "CustomerSubDistrictId",
                        label: {
                            text: "ตำบล/แขวง"
                        },
                        editorType: "dxLookup",
                        editorOptions: {
                            dataSource: addressList.dataSubDistrict,
                            valueExpr: 'SubDistrictID',
                            displayExpr: 'SubDistrictName'
                        },
                        isRequired: true
                    }, {
                        dataField: "CustomerDistrictId",
                        editorType: "dxLookup",
                        editorOptions: {
                            dataSource: addressList.dataDistrict,
                            valueExpr: 'DistrictID',
                            displayExpr: 'DistrictName'
                        },
                        label: {
                            text: "อำเภอ/เขต"
                        },
                        isRequired: true
                    },
                      {
                          dataField: "CustomerProvinceId",
                          editorType: "dxLookup",
                          editorOptions: {
                              dataSource: addressList.dataProvince,
                              valueExpr: 'ProvinceID',
                              displayExpr: 'ProvinceName'
                          },
                          label: {
                              text: "จังหวัด"
                          },
                          isRequired: true
                      }, {
                          dataField: "CustomerZipCode",
                          label: {
                              text: "รหัสไปรษณีย์"
                          }
                      }, ]
                },
                {
                    itemType: "group",
                    caption: "ข้อมูลที่ทำงาน",
                    items: [
                        {
                            dataField: "CustomerCareer",
                            label: {
                                text: "สถานภาพการทำงาน"
                            },
                            editorType: "dxSelectBox",
                            editorOptions: {
                                items: Career
                            },
                            isRequired: true
                        }, {
                            dataField: "CustomerJob",
                            label: {
                                text: "ลักษณะงาน"
                            },
                            isRequired: true
                        }, {
                            dataField: "CustomerSalary",
                            label: {
                                text: "รายได้"
                            },
                            isRequired: true
                        }, {
                            dataField: "CustomerJobYear",
                            label: {
                                text: "จำนวนปีที่ทำงาน"
                            },
                            isRequired: true
                        },
                        {
                            dataField: "CustomerJobAddress",
                            label: {
                                text: "สถานที่ทำงาน"
                            },
                            isRequired: false,
                        },

                       {
                           dataField: "CustomerJobSubDistrictId",
                           label: {
                               text: "ตำบล/แขวง"
                           },
                           editorType: "dxLookup",
                           editorOptions: {
                               dataSource: addressList.dataSubDistrict,
                               valueExpr: 'SubDistrictID',
                               displayExpr: 'SubDistrictName'
                           },
                           isRequired: false,
                       }, {
                           dataField: "CustomerJobDistrictId",
                           editorType: "dxLookup",
                           editorOptions: {
                               dataSource: addressList.dataDistrict,
                               valueExpr: 'DistrictID',
                               displayExpr: 'DistrictName'
                           },
                           label: {
                               text: "อำเภอ/เขต"
                           },
                           isRequired: false,
                       },
                      {
                          dataField: "CustomerJobProvinceId",
                          editorType: "dxLookup",
                          editorOptions: {
                              dataSource: addressList.dataProvince,
                              valueExpr: 'ProvinceID',
                              displayExpr: 'ProvinceName'
                          },
                          label: {
                              text: "จังหวัด"
                          },
                          isRequired: false,
                      },
                      {
                          dataField: "CustomerJobZipCode",
                          label: {
                              text: "รหัสไปรษณีย์"
                          }
                      }, ]
                }, ]
            },
             {

                 itemType: "group",
                 colCount: 3,
                 items: [{
                     itemType: "group",
                     caption: "ข้อมูลคู่สมรส",
                     items: [{
                         dataField: "CustomerSpouseTitle",
                         editorType: "dxSelectBox",
                         editorOptions: {
                             items: title
                         },

                         label: {
                             text: "คำนำหน้า"
                         },
                         isRequired: false
                     },
                    {
                        dataField: "CustomerSpouseFirstName",
                        label: {
                            text: "ชื่อ"
                        },
                        isRequired: false
                    }, {
                        dataField: "CustomerSpouseLastName",

                        label: {
                            text: "นามสกุล"
                        }
                        , isRequired: false
                    },
                    {
                        dataField: "CustomerSpouseNickName",
                        label: {
                            text: "ชื่อเล่น"
                        },
                        isRequired: false
                    }, {
                        dataField: "CustomerSpouseTelephone",
                        label: {
                            text: "เบอร์โทรศัพท์"
                        },

                    },
                     {
                         dataField: "CustomerSpouseMobile",
                         label: {
                             text: "เบอร์มือถือ"
                         },
                         isRequired: false

                     }, {
                         dataField: "CustomerSpouseAddress",
                         label: {
                             text: "ที่อยู่"
                         }
                     },
                       {
                           dataField: "CustomerSpouseSubDistrictId",
                           label: {
                               text: "ตำบล/แขวง"
                           },
                           editorType: "dxLookup",
                           editorOptions: {
                               dataSource: addressList.dataSubDistrict,
                               valueExpr: 'SubDistrictID',
                               displayExpr: 'SubDistrictName'
                           },
                           isRequired: false,
                       }, {
                           dataField: "CustomerSpouseDistrictId",
                           editorType: "dxLookup",
                           editorOptions: {
                               dataSource: addressList.dataDistrict,
                               valueExpr: 'DistrictID',
                               displayExpr: 'DistrictName'
                           },
                           label: {
                               text: "อำเภอ/เขต"
                           },
                           isRequired: false,
                       },
                      {
                          dataField: "CustomerSpouseProvinceId",
                          editorType: "dxLookup",
                          editorOptions: {
                              dataSource: addressList.dataProvince,
                              valueExpr: 'ProvinceID',
                              displayExpr: 'ProvinceName'
                          },
                          label: {
                              text: "จังหวัด"
                          },
                          isRequired: false,
                      },
                    {
                        dataField: "CustomerSpouseZipCode",
                        label: {
                            text: "รหัสไปรษณีย์"
                        }
                    }, ]
                 },
                        {
                            itemType: "group",
                            caption: "บุคคลที่ติดต่อได้ในกรณีฉุกเฉิน",
                            items: [{
                                dataField: "CustomerEmergencyTitle",
                                editorType: "dxSelectBox",
                                editorOptions: {
                                    items: title
                                },
                                label: {
                                    text: "คำนำหน้า"
                                }
                            }, {
                                dataField: "CustomerEmergencyFirstName",
                                label: {
                                    text: "ชื่อ"
                                }
                            }, {
                                dataField: "CustomerEmergencyLastName",
                                label: {
                                    text: "นามสกุล"
                                }
                            }, {
                                dataField: "CustomerEmergencyRelation",
                                label: {
                                    text: "ความสัมพันธ์"
                                }
                            }, {
                                dataField: "CustomerEmergencyTelephone",
                                isRequired: false,
                                label: {
                                    text: "เบอร์โทรศัพท์"
                                }
                            },
                       {
                           dataField: "CustomerEmergencyMobile",
                           label: {
                               text: "เบอร์มือถือ"
                           },
                           isRequired: false

                       }, ]
                        },
                       {
                           itemType: "group",
                           caption: "ข้อมูลผู้ขาย",
                           items: [{
                               dataField: "SaleID",
                               editorType: "dxLookup",
                               editorOptions: {
                                   items: dataZone,
                                   valueExpr: 'StaffID',
                                   displayExpr: 'StaffName'
                               },
                                  validationRules: [{
                                   type: "required",
                                   message: "ต้องการชื่อพนักงานขาย"
                               }],
                               isRequired: true,
                               label: {
                                   text: "พนักงานขาย"
                               },
                           },
                             ],
                       
                         
                           },
                           ]
                      

                
             },

           


            ]
        });

    }

    function convertDate(inputFormat) {
        function pad(s) { return (s < 10) ? '0' + s : s; }
        var d = new Date(inputFormat);
        return [pad(d.getDate()), pad(d.getMonth()+1), d.getFullYear()].join('/');
    }

    function formatDate(date) {
        var monthNames = [
          "มกราคม", "กุมภาพันธ์", "มีนาคม",
          "เมษายน", "พฤษภาคม", "มิถุนายน", "กรกฏาคม",
          "สิงหาคม", "กันยายน", "ตุลาคม",
          "พฤศจิกายน", "ธันวาคม"
        ];

        var day = date.getDate();
        var monthIndex = date.getMonth();
        var year = date.getFullYear();

        return day + ' ' + monthNames[monthIndex] + ' ' + year;
    }

    function LoadMenu() {
        //var menuStr = "";
        //$.ajax({
        //    url: '../Staffs/GetMenu?staffroleID=1',
        //    type: 'GET',
        //    contentType: 'application/json',
        //    success: function (result) {
         
        //        if (result.success) {
            
        //            result.data.forEach(function (element) {

        //                menuStr += ' <li> <a href="javascript:void(0)" class="waves-effect"><i data-icon="/" class="fa fa-users"></i><span class="hide-menu"> ' + element.StaffPermissionGroupName + '<span class="fa arrow"></span></a>'
        //                menuStr += '<ul class="nav nav-second-level"> '
        //                element.ListPermission.forEach(function (element1) {
        //                    menuStr += ' <li><a href="' + element1.StaffPermissionUrl + '"><i class="fa fa-user-plus"></i><span class="hide-menu"> ' + element1.StaffPermissionName + '</span></a></li> '
        //                });
        //                menuStr += ' </ul></li>';
              


        //            });

        //            console.log(menuStr)
        //   //         $("#side-menu").html(' <li> <a href="javascript:void(0)" class="waves-effect"><i data-icon="/" class="fa fa-users"></i><span class="hide-menu"> ระบบจัดการข้อมูลลูกค้า<span class="fa arrow"></span></a>' +
        //   //'<ul class="nav nav-second-level">  <li><a href="../Customers/Index"><i class="fa fa-search"></i><span class="hide-menu"> ค้นหาข้อมูลลูกค้า</span></a></li>' +
        //   //                     '<li><a href="../Customers/AddCustomer"><i class="fa fa-user-plus"></i><span class="hide-menu"> ลูกค้าใหม่</span></a></li></ul></li>');

        //            //    $("#side-menu").html(menuStr);

        //  //          $("#side-menu").html(' <li> <a href="javascript:void(0)" class="waves-effect"><i data-icon="/" class="fa fa-users"></i><span class="hide-menu"> ระบบจัดการค่างวด<span class="fa arrow"></span></a><ul class="nav nav-second-level">' +
        //  //'<li><a href="../ManagePayment/Installment"><i class="fa fa-user-plus"></i><span class="hide-menu"> บันทึกค่างวดรายวัน</span></a></li> </ul></li>');

        //        } else {
        //            DevExpress.ui.notify(data.data);

        //        }

        //    },
        //    error: function () {
        //        console.log("error");
        //    }
        //});

        // Load menu


        //  $("#side-menu").html(menuStr);

    }


    function alerSuccess(message) {

        swal("สำเร็จ!!", message, "success");

    }

    function alerError(message) {

        swal("เกิดข้อผิดพลาด!", message, "error");
    }

    function alertConfirm(messageQuestion,messageYes,messageNo) {


        swal({
            title: "Are you sure?",
            text: messageQuestion,
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
             swal("Successful!", messageYes, "success");

         } else {
             swal("Cancelled", messageNo, "error");
             e.preventDefault();
         }
     });

    }

    var addressList = (function () {
        var json = null;
        $.ajax({
            'async': false,
            'global': false,
            'url': '../Setting/GetLocation/',
            'dataType': "json",
            'success': function (data) {
                json = data;
            }
        });
        return json;
    })();


    //console.log(addressList);