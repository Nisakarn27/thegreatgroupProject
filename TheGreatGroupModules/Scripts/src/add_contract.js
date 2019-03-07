$(function () {

 
    $("#loadIndicator").dxLoadIndicator({
        visible: true
    });

    var products = [];
    $("#product_amount").dxTextBox({
        placeholder: 'ใส่จำนวน',
        value: 1
    });


    LoadContract();
    LoadDataGrid();


    $("#myPopup").dxPopup({
        title: 'พิมพ์เอกสาร',
        visible: false,
    });

    if (getUrlParameter('ContractID') == 0) {

        $("#btnPrintCard").prop("disabled", true);
        $("#btnPrintReceipt").prop("disabled", true);
        $("#btnPrintContract").prop("disabled", true);
        $("#btnAddContractAmountLast").prop("disabled", true);
        
    } else {

        $("#btnPrintCard").prop("disabled", false);
        $("#btnPrintReceipt").prop("disabled", false);
        $("#btnPrintContract").prop("disabled", false);
        $("#btnAddContractAmountLast").prop("disabled", false);
    }
  
});



function LoadContract() {



    var contract = {

    };
    var data = [{}];
    var listproducts = Array();
    var CustomerSuretyData1 = Object();
    var CustomerSuretyData2 = Object();
    var CustomerPartnerData = Object();
    var dataProvince = [];
    var dataDistrict = [];
    var dataSubDistrict = [];

    var days = [{
        "ID": 1,
        "Name": "ทุกวัน"
    },
        { "ID": 2, "Name": "ยกเว้นวันเสาร์อาทิตย์" },
    ];

    $.get("../Contract/GetContract?CustomerID=" + getUrlParameter('CustomerID')
 + "&ContractID=" + getUrlParameter('ContractID'))
  .done(function (result) {
      products = result.dataProduct;
      /* ================= ข้อมูลสัญญา =====================*/
      var ListContract = [];
      if (getUrlParameter('ContractID') != 0) {
          result.data.forEach(function (element) {

              element.ContractCustomerID = getUrlParameter('CustomerID');
              element.ContractCreateDate = new Date(element.ContractCreateDate_Text);
              element.ContractStartDate = new Date(element.ContractStartDate_Text);
              element.ContractExpDate = new Date(element.ContractExpDate_Text);
              element.ContractExpDate_Text = convertDate(element.ContractExpDate);
              element.ContractCreateDate_Text = convertDate(element.ContractCreateDate);
              element.ContractStartDate_Text = convertDate(element.ContractStartDate);
              ListContract.push(element);

              if (ListContract[0].CustomerSuretyData1 !== null) {

                  CustomerSuretyData1 = ListContract[0].CustomerSuretyData1;
              }
              if (ListContract[0].CustomerSuretyData2 !== null) {

                  CustomerSuretyData2 = ListContract[0].CustomerSuretyData2;
              }
              if (ListContract[0].CustomerPartnerData !== null) {

                  CustomerPartnerData = ListContract[0].CustomerPartnerData;
              }

              if (ListContract[0].IsContractAmountLast == 1) {


                  $("#btnAddContractAmountLast").prop("disabled", true);

              } else {



                  $("#btnAddContractAmountLast").prop("disabled", false);
              }
          });

      } else { ListContract.push({}); }

    

      $("#product_name").dxLookup({
          dataSource: products,
          displayExpr: 'ProductDetail',
          valueExpr: 'ProductID',
          title: 'เลือกสินค้า',
          placeholder: 'เลือกสินค้า',
          cancelButtonText: "ยกเลิก",
          seachPlaceholder: 'เลือกสินค้า',
      });

      $("#loadIndicator").dxLoadIndicator({
          visible: false
      });
      $("#form").dxForm({
          colCount: 3,
          formData: ListContract[0],
          showColonAfterLabel: true,
          showValidationSummary: false,
          items: [{
              dataField: "ContractNumber",
              label: {
                  text: "เลขที่สัญญา"
              },
          }, {
              dataField: "ContractRefNumber",
              label: {
                  text: "เลขที่ใบกำกับภาษี"
              },
          }, {
              dataField: "ContractCreateDate",
              editorType: "dxDateBox",
              label: {
                  text: "วันที่ทำสัญญา"
              },
              editorOptions: {
                  width: "100%",
                  displayFormat: "dd/MM/yyyy"
              },
              isRequired: true,
              validationRules: [{
                  type: "required",
                  message: "โปรดระบุวันที่เริ่มจ่าย"
              }]
          }, {
              dataField: "ContractExpDate",
              editorType: "dxDateBox",
              label: {
                  text: "วันที่สิ้นสุดสัญญา"
              },
              
              editorOptions: {
                  width: "100%",
                  displayFormat: "dd/MM/yyyy",
                  disabled: true
              },
          }, {
              dataField: "ContractStartDate",
              editorType: "dxDateBox",
              label: {
                  text: "วันที่เริ่มจ่าย"
              },
              editorOptions: {
                  width: "100%",
                  displayFormat: "dd/MM/yyyy"
              },
              isRequired: true,
              validationRules: [{
                        type: "required",
                        message: "โปรดระบุวันที่เริ่มจ่าย"
              }],
          },
          {
              dataField: "ContractType",
              label: {
                  text: "ประเภทลูกค้า"
              },
              editorType: "dxSelectBox",
              editorOptions: {
                  items: [ "G","GP"],
              },
          },
          {
              dataField: "ContractPeriod",
              label: {
                  text: "จำนวนงวด/งวด"
              },
              editorOptions: {
                  disabled: true
              },
         
          },
          {
              dataField: "ContractAmount",
              label: {
                  text: "งวดละ/บาท"
              },
              isRequired: true,
              validationRules: [{
                  type: "required",
                  message: "โปรดระบุค่างวด/บาท"
              }]
          },
          {
              dataField: "ContractAmountLast",
              label: {
                  text: "งวดสุดท้าย/บาท"
              },
              editorOptions: {
                  disabled: true
              }
          }, {
              dataField: "ContractInterest",
              label: {
                  text: "อัตราดอกเบี้ย(%)"
              },

          },
          {
              dataField: "ContractPayEveryDay",
              label: {
                  text: "วันที่ผ่อนชำระ"
              },
              editorType: "dxSelectBox",
              editorOptions: {
                  items: days,
                  displayExpr: "Name",
                  valueExpr: "ID",

              },
              isRequired: true,
              validationRules: [{
                  type: "required",
                  message: "โปรดระบุวันที่ผ่อนชำระ"
              }],
          },
           {
               dataField: "ContractSpecialholiday",
               label: {
                   text: "ยกเว้นวันหยุดพิเศษ"
               },
               editorType: "dxCheckBox",

           },
           {
               dataField: "ContractReward",
               label: {
                   text: "ค่ากำเหน็จ/บาท"
               },
               isRequired: true,
               validationRules: [{
                   type: "required",
                   message: "โปรดระบุค่ากำเหน็จ(บาท)"
               }]
           },
          ]
      });

      // ==================== ข้อมูลผู้ค้ำประกัน =====================
      $("#form1").dxForm({
          colCount: 3,
          formData: CustomerSuretyData1,
          showColonAfterLabel: true,
          showValidationSummary: false,
          items: [
              {
                  dataField: "CustomerSuretyTitle",
                  label: {
                      text: "คำนำหน้า"
                  },
                  editorType: "dxSelectBox",
                  editorOptions: {
                      items: title,
                  },
              }, {
                  dataField: "CustomerSuretyFirstName",
                  label: {
                      text: "ชื่อผู้ค้ำประกันคนที่ 1"
                  },
              },


          {
              dataField: "CustomerSuretyLastName",
              label: {
                  text: "นามสกุล"
              },
          },
          {
              dataField: "CustomerSuretyAddress",
              label: {
                  text: "ที่อยู่"
              },

          },
          {
              dataField: "CustomerSuretySubDistrictId",
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
              dataField: "CustomerSuretyDistrictId",
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
               dataField: "CustomerSuretyProvinceId",
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
            dataField: "CustomerSuretyZipCode",
            label: {
                text: "รหัสไปรษณีย์"
            },

        },
          {
              dataField: "CustomerSuretyIdCard",
              label: {
                  text: "เลขประจำตัวประชาชน"
              },
              isRequired: true
                    , validationRules: [{
                        type: "required",
                        message: "ต้องการเลขประจำตัวประชาชน"
                    },{
              type: "stringLength",
      max: 13,
      min: 13,
      message: "รหัสประจำตัวประชาชนต้องมี 13 หลัก"
  },],
          },
           {
               dataField: "CustomerSuretyTelephone",
               label: {
                   text: "เบอร์โทรศัพท์"
               },

           },
            {
                dataField: "CustomerSuretyMobile",
                label: {
                    text: "เบอร์มือถือ"
                },
                isRequired: true,
                   validationRules: [{
                        type: "required",
                        message: "ต้องการเบอร์มือถือ"
                    }],
            },
          ]
      });


      $("#form2").dxForm({
          colCount: 3,
          formData: CustomerSuretyData2,
          showColonAfterLabel: true,
          showValidationSummary: false,
          items: [
              {
                  dataField: "CustomerSuretyTitle",
                  label: {
                      text: "คำนำหน้า"
                  },
                  editorType: "dxSelectBox",
                  editorOptions: {
                      items: title,
                  },
              }, {
                  dataField: "CustomerSuretyFirstName",
                  label: {
                      text: "ชื่อผู้ค้ำประกันคนที่ 2"
                  },
              },


          {
              dataField: "CustomerSuretyLastName",
              label: {
                  text: "นามสกุล"
              },
          },
          {
              dataField: "CustomerSuretyAddress",
              label: {
                  text: "ที่อยู่"
              },

          },
          {
              dataField: "CustomerSuretySubDistrictId",
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
              dataField: "CustomerSuretyDistrictId",
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
               dataField: "CustomerSuretyProvinceId",
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
            dataField: "CustomerSuretyZipCode",
            label: {
                text: "รหัสไปรษณีย์"
            },

        },
          {
              dataField: "CustomerSuretyIdCard",
              label: {
                  text: "เลขประจำตัวประชาชน"
              },
              validationRules: [ {
                  type: "stringLength",
                  max: 13,
                  min: 13,
                  message: "รหัสประจำตัวประชาชนต้องมี 13 หลัก"
              }, ],
          },
           {
               dataField: "CustomerSuretyTelephone",
               label: {
                   text: "เบอร์โทรศัพท์"
               },

           },
            {
                dataField: "CustomerSuretyMobile",
                label: {
                    text: "เบอร์มือถือ"
                },

            },
          ]
      });

      $("#form4").dxForm({
          colCount: 3,
          formData: CustomerPartnerData,
          showColonAfterLabel: true,
          showValidationSummary: false,
          items: [
              {
                  dataField: "CustomerPartnerTitle",
                  label: {
                      text: "คำนำหน้า"
                  },
                  editorType: "dxSelectBox",
                  editorOptions: {
                      items: title,
                  },
              }, {
                  dataField: "CustomerPartnerFirstName",
                  label: {
                      text: "ชื่อผู้ซื้อร่วม"
                  },
              },


          {
              dataField: "CustomerPartnerLastName",
              label: {
                  text: "นามสกุล"
              },
          },
          {
              dataField: "CustomerPartnerAddress",
              label: {
                  text: "ที่อยู่"
              },

          },
          {
              dataField: "CustomerPartnerSubDistrictId",
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
              dataField: "CustomerPartnerDistrictId",
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
               dataField: "CustomerPartnerProvinceId",
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
            dataField: "CustomerPartnerZipCode",
            label: {
                text: "รหัสไปรษณีย์"
            },

        },
          {
              dataField: "CustomerPartnerIdCard",
              label: {
                  text: "เลขประจำตัวประชาชน"
              },
              validationRules: [{
                  type: "stringLength",
                  max: 13,
                  min: 13,
                  message: "รหัสประจำตัวประชาชนต้องมี 13 หลัก"
              }, ],
          },
           {
               dataField: "CustomerPartnerTelephone",
               label: {
                   text: "เบอร์โทรศัพท์"
               },

           },
            {
                dataField: "CustomerPartnerMobile",
                label: {
                    text: "เบอร์มือถือ"
                },

            },
          ]
      });
      //   $("#form").dxForm("instance").validate();
      /* ===========================ข้อมูลลูกค้า  =============================*/
      var response = result.dataCustomers[0];
      if (result.success == true) {
          $("#customerName").html(' <h4> &nbsp;<b>ชื่อลูกค้า : </b>' + response.CustomerName + '</h4>');
          $("#customerAddress").html('  <p class="text-muted m-l-5">ที่อยู่ : ' + response.CustomerAddress1 + '</p>');
      }


      /* =========================== ข้อมูลสินค้า =============================*/
      
      var i = listproducts.length;
      var sum = 0;
      if (result.dataProductSelect.length > 0) {

          $('#gridData').dxDataGrid('instance').option('dataSource', result.dataProductSelect);
      

              $("#btnPrintCard").prop("disabled", false);
              $("#btnPrintReceipt").prop("disabled", false);
              $("#btnPrintContract").prop("disabled", false);
          
      }
      /* ================================================================*/

  });

}


function LoadDataGrid() {
    $("#gridData").dxDataGrid({
        //  dataSource: productsdatasource,
        showColumnLines: true,
        showRowLines: true,
        rowAlternationEnabled: true,
        showBorders: true,
        columns: [
            {
                dataField: "No",
                caption: "ลำดับที่",
                width: 80 + "%",
                alignment: 'center',
                allowFiltering: false,
                allowSorting: false
            },
            {
                dataField: "ProductName",
                caption: "รายการสินค้า",
                width: 300 + "%",
                alignment: 'left',
                allowFiltering: false,
                allowSorting: false
            }, {
                dataField: "Unit_Text",
                caption: "จำนวน(หน่วย)",
                width: 160 + "%",
                alignment: 'right',
                allowSorting: false
            },
          {
              dataField: "ProductPrice_Text",
              caption: "ราคาต่อหน่วย",
              width: 160 + "%",
              alignment: 'right',
              allowSorting: false

          },
         {
             dataField: "TotalPrice_Text",
             caption: "จำนวนเงิน",
             alignment: 'right',
             width: 160 + "%",
             allowSorting: false
         },
        ],
        summary: {

        },
        onRowUpdated: function (e) {
            console.log(e);
        },
        onRowRemoving: function (e) {
            console.log(e);
        },
        onRowRemoved: function (e) {
            console.log(e)

        }
    });
}

function PrintReceipt() {


    window.open('/Report/ReceiptReport.aspx?CustomerID=' + getUrlParameter('CustomerID')
       + "&ContractID=" + getUrlParameter('ContractID'), '_blank');
    DevExpress.ui.notify("Export PDF Successful!");

}
function PrintCard() {

    //var a = $("<a>").attr("href", "../Customers/ExportCard?ContractID=" + getUrlParameter('ContractID')
    //    + "&CustomerID=" + getUrlParameter('CustomerID')).attr("download", "img1.png").appendTo("body");
    //a[0].click();
    //a.remove();
    


    window.open('/Report/CustomerCard.aspx?CustomerID=' + getUrlParameter('CustomerID')
       + "&ContractID=" + getUrlParameter('ContractID'), '_blank');
    DevExpress.ui.notify("Export PDF Successful!");
}

function PrintContract() {
    


    window.open('/Report/ContractBookReport.aspx?CustomerID=' + getUrlParameter('CustomerID')
       + "&ContractID=" + getUrlParameter('ContractID'), '_blank');
    //window.location.href = '/Report/ReportPage1.aspx?staffID=' + $("#StaffID").val() +
    //"&date=" + $('#DateAsOf').val();
    DevExpress.ui.notify("Export PDF Successful!");
}

function Submit_Click() {

   
    



}


function AddContractAmountLast() {


    if (getUrlParameter('ContractID') > 0) {
        //Update
        var Contract = $("#form").dxForm("instance").option('formData');

        Contract.ContractID = getUrlParameter('ContractID');
        Contract.ContractCustomerID = getUrlParameter('CustomerID');

        $.ajax({
            url: '../Contract/PostAddContractAmountLast',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(Contract),
            success: function (data) {
                if (data.success == true) {

                    swal("สำเร็จ!!", "บันทึกค่างวดแรกสำเร็จ !!", "success");

                } else {

                    DevExpress.ui.notify(data.data);
                }
            },
            error: function () {
                console.log("error");
            }
        });
        
    } else {

        swal("ผิดพลาด!!", "กรุณาบันทึกข้อมูลสัญญาก่อนบันทึกค่างวดแรก !!", "error");
    }



}

function AddProduct() {
    if ($("#product_name").dxLookup("instance").option("value") != null) {

        var lookup = $("#product_name").data("dxLookup");
        var selectedValue = lookup.option("value");
        var dataProduct = {
            "Unit": $("#product_amount").dxTextBox('option', 'value'),
                "ProductID":$("#product_name").dxLookup("instance").option("value")}
    

        $.ajax({
            url: '../Products/PostAddProduct?CustomerID=' + getUrlParameter('CustomerID') + '&ContractID=' + getUrlParameter('ContractID'),
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(dataProduct),
            success: function (data) {
                LoadContract();
              //  window.location = "../Customers/Contract?CustomerID=" + getUrlParameter('CustomerID') +
              //"&ContractID=" +getUrlParameter('ContractID') ;
            },
            error: function () {
                console.log("error");
            }
        });


    } else {
        DevExpress.ui.notify("โปรดเลือกสินค้า !!");

    }



}


function back() {

    window.location = "../Customers/ListContract?CustomerID=" + getUrlParameter('CustomerID');

}

$("#myButton").dxButton({
    text: 'บันทึก',
    type: 'success',
    useSubmitBehavior: true,
    onClick: function () {
        
        var Contract = $("#form").dxForm("instance").option('formData');

        Contract.ContractID = getUrlParameter('ContractID');
        Contract.ContractCustomerID = getUrlParameter('CustomerID');
        Contract.CustomerPartnerData = $("#form4").dxForm("instance").option('formData');
        Contract.CustomerSuretyData1 = $("#form1").dxForm("instance").option('formData');
        Contract.CustomerSuretyData2 = $("#form2").dxForm("instance").option('formData');
     
        var result = $("#form").dxForm("instance").validate();
        var result1 = $("#form1").dxForm("instance").validate();
      
        if (result.isValid && result1.isValid ) {

            if (getUrlParameter('ContractID') == 0) {
                //Update


                $.ajax({
                    url: '../Contract/PostAdd_NewContract',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(Contract),
                    success: function (data) {
                        if (data.success == true) {
                            DevExpress.ui.notify(data.data);
                            window.location = "../Customers/Contract?CustomerID=" + getUrlParameter('CustomerID') +
                       "&ContractID=" + data.ContractID;

                        } else {

                            DevExpress.ui.notify(data.data);
                        }
                    },
                    error: function () {
                        console.log("error");
                    }
                });
                //  
            } else {

                $.ajax({
                    url: '../Contract/PostEdit_Contract',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(Contract),
                    success: function (data) {

                        if (data.success == true) {
                            swal("สำเร็จ!!", "แก้ไขข้อมูลสำเร็จ !!", "success");
                            LoadContract();

                        } else {

                            swal("สำเร็จ!!", data.data, "success");

                        }


                    },
                    error: function () {
                        console.log("error");

                    }
                });

            }


        }
    }
});
