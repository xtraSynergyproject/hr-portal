
  

    $(function () {
        $("#IssueType").kendoDropDownList({
            dataTextField: "Name",
            dataValueField: "Id",
            optionLabel: "--Select--",
            //change: function (e) {
            //    $("#ItemCategory").data("kendoDropDownList").dataSource.read({ ItemTypeId: $("#ItemHead").data("kendoDropDownList").value() });
            //},
			
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
            optionLabel: "--Select--",
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
            optionLabel: "--Select--",
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
        optionLabel: "--Select--",
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
            value: new Date()
        });
        $("#itemsGrid").jsGrid({
            width: "100%",
            height: "600px",

            inserting: false,
            editing: true,
            sorting: true,
            paging: true,
            autoload: true,
            pageSize: 14,
            pageButtonCount: 5,
            deleteConfirm: "Do you really want to delete client?",

            //data: clients,
            controller: {
                loadData: function (filter) {
                    return $.ajax({
                        type: "GET",
                        url: "/IMS/InventoryManagement/ReadRequisitionItemsToIssue?requisitionId=" + $("#RequisitionId").val(),
                        data: filter,
                        dataType: "json"
                    });
                },
                updateItem: function (item) {
                    
                },
                deleteItem: function (item) {
                    onDeleteItem(item.NoteId);
                }
            },
            //onItemEditing: function (e) {
            //    if (e.grid._container.find(".jsgrid-edit-row")[0] != undefined) {
            //        //  debugger;
            //        e.grid.updateItem();
            //    }
            //},
            fields: [
                {
                    name: "Select",
                    title: "Select",
                    type: "checkbox",
                    editing: true
                },
                {
                    name: "ItemName",
                    title: "Item Name",
                    type: "text",
                    editing: false
                },
                //{
                //    field: "ProposalDate",
                //    headerName: "Proposal Date"
                //},
                {
                    name: "ItemSpecification",
                    title: "Item Specification",
                    type: "text",
                    editing: false
                },
                {
                    name: "InventoryQuantity",
                    title: "Available Quantity",
                    type: "number",
                    editing: false
                },
                {
                    name: "ItemQuantity",
                    title: "Requisition Quantity",
                    type: "number",
                    editing: false
                },
                {
                    name: "ApprovedQuantity",
                    title: "Approved Quantity",
                    type: "number",
                    editing: false

                },
                {
                    name: "IssuedQuantity",
                    title: "Issued Quantity",
                    type: "number",
                    editing: false

                },
                {
                    name: "BalanceQuantity",
                    title: "Balance Quantity",
                    type: "number", editcss: "team-edit",
                    editing: false,
                    //itemTemplate: function (val, item)
                    //{
                    //    return item.ApprovedQuantity - item.IssuedQuantity;
                    //}
                    
                },
                {
                    name: "CurrentIssueQuantity",
                    title: "Current Issue Quantity",
                    type: "number",
                    editing: true,
                    //editTemplate: function (value) {
                    //    debugger;
                    //    var balance = this._grid.fields[7].valueOf();
                    //    var inventory = this._grid.fields[3].valueOf();
                      
                    //    var $editControl = jsGrid.fields.number.prototype.editTemplate.call(this, value);

                    //    var changeCriteria = function () {
                    //        var BalanceQuantity = balance.editValue();
                    //        var InventoryQuantity = inventory.editValue();
                    //        var CurrentQuantity = this.value;
                    //        if (BalanceQuantity < InventoryQuantity) {
                    //            if (CurrentQuantity > BalanceQuantity) {
                    //                alert("Current Quantity cannot exceed Balance Quantity");
                    //                this.value = 0;
                    //                $editControl.val('0');
                    //            }
                    //        }
                    //        else
                    //        {
                    //            if (CurrentQuantity > InventoryQuantity) {
                    //                alert("Current Quantity cannot exceed available stock Quantity");
                    //                this.value = 0;
                    //                $editControl.val('0');
                    //            }
                    //        }
                          
                    //    };

                    //    $editControl.on("change", changeCriteria);
                    //    changeCriteria();

                    //    return $editControl;
                    //}
                },
                {

                    type: "control", deleteButton: false,      

                },

            ]
        });
    });
    
   


   
        var onAjaxSuccess = function (res) {
            if (res.success) {
                closeNav()
        }
            else {
               
          debugger
                //showError(res.error);
                $(".text-danger").html(res.error);
                $(".text-danger").css("display", "block");
        }
    };


    function closeNav() {

        var win = GetMainWindow();
        win.CloseWindow();
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
    debugger;
    var selectedRows = [];
    var entityGrid = $("#itemsGrid").data("JSGrid");
  //  entityGrid.updateItem();
    for (var i = 0; i < entityGrid.data.length; i++) {
        if (entityGrid.data[i].Select == true) {
            selectedRows.push(entityGrid.data[i]);
        }
    }
    if (selectedRows.length == 0) {
        $(".text-danger").removeClass("validation-summary-valid");
        $(".text-danger").addClass("validation-summary-errors");
        $(".text-danger").html("select at least one item");
        e.preventDefault();
        return false;
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