
  

    $(function () {
        $("#IssueType").kendoDropDownList({
            dataTextField: "Name",
            dataValueField: "Id",
            optionLabel: "-- Select --",
            //change: function (e) {
            //    /*$("#ItemCategory").data("kendoDropDownList").dataSource.read({ ItemTypeId: $("#ItemHead").data("kendoDropDownList").value() });*/
            //},
			change :onIssueTypeChange,
        dataSource: {
            transport: {
                read: {
                    url: "/Cms/LOV/GetListOfValueList?type=IMS_ISSUE_TYPE",
                    }
            }
        }
        });
        $("#Department").kendoDropDownList({
            dataTextField: "Name",
            dataValueField: "Id",
            optionLabel: "--All--",
            autoBind:false,
			
            //change: function (e) {
            //    $("#ItemSubCategory").data("kendoDropDownList").dataSource.read({ categoryId: $("#ItemCategory").data("kendoDropDownList").value() });
            //},
        //dataSource: {
        //    transport: {
        //        read: {
        //            url: "/IMS/InventoryManagement/GetItemCategoryList",
        //            }
        //    }
        //}
        });
        $("#Employee").kendoDropDownList({
            dataTextField: "Name",
            dataValueField: "Id",
            optionLabel: "--All--",
            autoBind: false,
            //change: function (e) {
            //    $("#Item").data("kendoDropDownList").dataSource.read({ subCategoryId: $("#ItemSubCategory").data("kendoDropDownList").value() });
            //},
			
        //dataSource: {
        //    transport: {
        //        read: {
        //            url: "/IMS/InventoryManagement/GetItemSubCategoryList",
        //            }
        //    }
        //}
    });
    $("#IssueTo").kendoDropDownList({
        dataTextField: "CustomerName",
        dataValueField: "Id",
        optionLabel: "-- Select --",
        filter: "contains",
        dataSource: {
            transport: {
                read: {
                    url: "/IMS/InventoryManagement/GetCustomerList",
                }
            }
        }
    });
        $("#IssuedOn").kendoDatePicker({
            value: new Date(),
            //format: "dd.MM.yyyy"
        });
       
    });

function onIssueTypeChange() {
    /*alert("Hello changes");*/
    /*debugger;*/
    var issueto = $("#IssueTo").data("kendoDropDownList");
    var search = {
        issueType: $("#IssueType").data("kendoDropDownList").value(),
    }
    issueto.dataSource.read(search);
    /*$("#IssueTo").data("kendoDropDownList").dataSource.read({ issueType: $("#IssueType").data("kendoDropDownList").value() });*/
}



   
        var onAjaxSuccess = function (res) {
            if (res.success) {
                closeNav()
        }
            else {
               
          
                //showError(res.error);
                $(".text-danger").html(res.error);
                $(".text-danger").css("display", "block");
        }
    };


    function closeNav() {

        var win = GetMainWindow();
        win.CloseWindow({ MethodName:"OnAfterNoteCreate1"});
        return false;
    }

    function onFileUploadSuccess(e) {
        //
        if (e.response.success) {
            // alert(e.response.fileId);
            //console.log(e);
            // set file id to hdden fileid proprty
            $.ajax({
                url: "/user/ChangeUserProfilePhoto?photoId=" + e.response.fileId,
                type: "GET",
                contentType: "application/json",
                dataType: "JSON",
                success: function (response) {
                   //
                    $(".avatar-myProfile").attr("src", "/cms/Document/GetImageMongo?id=" + e.response.fileId);
                    $("#PhotoId").val(e.response.fileId);
                }
            });

        }
        else {
            //var msg = ExtractError(e.response.errors, true);
            //alert(msg);
        }
        return true;
    } 
    function confirmDelete() {
     //
        //        alert("Banner Delete");
        $(".avatar-myProfile").attr("src", "/images/200.png");
        $("#PhotoId").val('');
        var logoupload = $("#files").data("kendoUpload");
        //console.log(logoupload);
        logoupload.clearFile(function (file) { return true; });
    }
function SaveData(e) {
    debugger
    var selectedRows = [];
    var entityGrid = $("#itemsGrid").data("JSGrid");
    if (entityGrid._editingRow != null) {
        entityGrid.updateItem();
    }
    var sum = 0.0;
    var issued = 0;
    for (var i = 0; i < entityGrid.data.length; i++) {
        if (entityGrid.data[i].Select == true) {
            selectedRows.push(entityGrid.data[i]);
            if (parseFloat(entityGrid.data[i].IssuedQuantity) > parseFloat(entityGrid.data[i].BalanceQuantity))
            {
                $(".text-danger").removeClass("validation-summary-valid");
                $(".text-danger").addClass("validation-summary-errors");
                ShowNotification("[Invalid Input]Current issued Quantity is exceeding balance quantity", "error");
                e.preventDefault();
                return false
                break;
            }
            else {
                issued = parseInt(entityGrid.data[i].IssuedQuantity);
                sum = parseFloat(sum) + parseFloat(entityGrid.data[i].IssuedQuantity);
                $(".text-danger").removeClass("validation-summary-errors");
                $(".text-danger").addClass("validation-summary-valid");
            }
            
        }
    }
    if (parseFloat(sum) > parseFloat(ItemQuantity))
    {
        $(".text-danger").removeClass("validation-summary-valid");
        $(".text-danger").addClass("validation-summary-errors");
        ShowNotification("[Invalid Input]Sum of Current issued Quantity and already issued quantity is exceeding requisition quantity", "error");
        e.preventDefault();
        return false
    }
    if (selectedRows.length == 0) {
        $(".text-danger").removeClass("validation-summary-valid");
        $(".text-danger").addClass("validation-summary-errors");
        $(".text-danger").html("select at least one item");
        e.preventDefault();
        return false;
    }


    if (IsSerializable == "4")
    {
        var selectedSerials = [];
        var serialGrid = $("#SerialNoGrid").data("JSGrid");
        if (serialGrid._editingRow != null) {
            serialGrid.updateItem();
        }
        for (var i = 0; i < serialGrid.data.length; i++) {
            if (serialGrid.data[i].Select == true) {
                selectedSerials.push(serialGrid.data[i].Id); 
            }
        }
        if (selectedSerials.length < issued) {
            $(".text-danger").removeClass("validation-summary-valid");
            $(".text-danger").addClass("validation-summary-errors");
            $(".text-danger").html(issued+" serials No are required");
            e.preventDefault();
            return false;
        }
        $("#SerialNoIds").val(JSON.stringify(selectedSerials));
    }
    if ($("#IssueType").data("kendoDropDownList").value() == "" || $("#IssueType").data("kendoDropDownList").value() == undefined) {
        $(".text-danger").removeClass("validation-summary-valid");
        $(".text-danger").addClass("validation-summary-errors");
        $(".text-danger").html("Issue Type is required");
        e.preventDefault();
        return false;
    }
    if ($("#IssueTo").data("kendoDropDownList").value() == "" || $("#IssueTo").data("kendoDropDownList").value() == undefined) {
        $(".text-danger").removeClass("validation-summary-valid");
        $(".text-danger").addClass("validation-summary-errors");
        $(".text-danger").html("Issue To is required");
        e.preventDefault();
        return false;
    }
    $("#Items").val(JSON.stringify(selectedRows));
    console.log(selectedRows);
    //e.preventDefault();
}