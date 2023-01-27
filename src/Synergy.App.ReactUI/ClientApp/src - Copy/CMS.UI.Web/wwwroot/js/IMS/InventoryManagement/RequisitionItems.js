
    var columnDefs = [
        {
            field: "ItemName",
            headerName: "Item Name"
        },
        //{
        //    field: "ProposalDate",
        //    headerName: "Proposal Date"
        //},
        {
            field: "ItemDescription",
            headerName: "Item Specification"
        },
        {
            field: "ItemQuantity",
            headerName: "Quantity",

        },
        {
            field: "PurchaseRate",
            headerName: "Purchase Rate",

        },
        {
            field: "Amount",
            headerName: "Item Amount",

        },
        {
            headerName: "Actions",
            field: "NoteId",
            cellRenderer: params => {
                return "<div class='btn-group grid-menu' id='tree-menuItem' data-idvalue='" + params.value + "' data-status=\"0\" data-noteid=\"#:NoteId#\" ><i class='fas fa-ellipsis-v'></i></div>"
            }
        }
    ];

    $(function () {
        //$("#ItemHead").kendoDropDownList({
        //    dataTextField: "Name",
        //    dataValueField: "Id",
        //    optionLabel: "--Select--",
        //    change: function (e) {
        //        $("#ItemCategory").data("kendoDropDownList").dataSource.read({ ItemTypeId: $("#ItemHead").data("kendoDropDownList").value() });
        //    },
			
        //dataSource: {
        //    transport: {
        //        read: {
        //            url: "/Cms/LOV/GetListOfValueList?type=IMS_ITEM_TYPE",
        //            }
        //    }
        //}
        //});
        $("#ItemCategory").kendoDropDownList({
            dataTextField: "Name",
            dataValueField: "Id",
            optionLabel: "--Select--",
            autoBind:true,
			
            change: function (e) {
                $("#ItemSubCategory").data("kendoDropDownList").dataSource.read({ categoryId: $("#ItemCategory").data("kendoDropDownList").value() });
            },
        dataSource: {
            transport: {
                read: {
                    url: "/IMS/InventoryManagement/GetItemCategoryList?ItemTypeId=" + $("#ItemHead").val(),
                    }
            }
        }
        });
        $("#ItemSubCategory").kendoDropDownList({
            dataTextField: "Name",
            dataValueField: "Id",
            optionLabel: "--Select--",
            autoBind: false,
            change: function (e) {
                $("#Item").data("kendoDropDownList").dataSource.read({ subCategoryId: $("#ItemSubCategory").data("kendoDropDownList").value() });
            },
			
        dataSource: {
            transport: {
                read: {
                    url: "/IMS/InventoryManagement/GetItemSubCategoryList",
                    }
            }
        }
    });
    $("#Item").kendoDropDownList({
        dataTextField: "ItemName",
        dataValueField: "Id",
        optionLabel: "--Select--",
        filter: "contains",
        change: OnItemChange,
        dataSource: {
            transport: {
                read: {
                    url: "/IMS/InventoryManagement/GetItemList",
                }
            }
        }
    });
        $("#ItemsGrid").jsGrid({
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
                        url: "/IMS/InventoryManagement/ReadRequisitionItemsData?requisitionId=" + $("#RequisitionId").val(),
                        data: filter,
                        dataType: "json"
                    });
                },
                updateItem: function (item) {
                    item.DataAction = "Edit";
                    //if (item.Id!=null ) { }
                    return $.ajax({
                        type: "POST",
                        url: "/IMS/InventoryManagement/ManageRequisitionItems",
                        data: item,
                        //dataType: "json",
                        //success: function (result) {
                        //    alert("Success", "updated successfully");
                        //},
                        //error: function (err) {
                        //    alert(err, "Error updating");
                        //}
                    });
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
                    name: "ItemQuantity",
                    title: "Quantity",
                    type: "text",
                    editing: true
                },
                {
                    name: "PurchaseRate",
                    title: "Purchase Rate",
                    type: "text",
                    editing: true

                },
                {
                    name: "Amount",
                    title: "Item Amount",
                    type: "text",
                    editing: false
                },
                {

                    type: "control",

                },
                //{
                //    headerName: "Actions",
                //    field: "NoteId",
                //    cellRenderer: params => {
                //        return "<div class='btn-group grid-menu' id='tree-menuItem' data-idvalue='" + params.value + "' data-status=\"0\" data-noteid=\"#:NoteId#\" ><i class='fas fa-ellipsis-v'></i></div>"
                //    }
                //}
            ]
        });
        //getItemsData();
    //$.contextMenu({
    //    selector: '#tree-menuItem',
    //    trigger: 'left',
    //    build: function ($trigger, e) {


    //        var id = $trigger.data('idvalue');

    //        var status = $trigger.data('status');

    //        switch (0) {
    //            case 0:

    //                return {
    //                    callback: function (key, options) {
    //                        switch (key) {
    //                            case 'delete':
    //                                onDeleteItem(id);
    //                                break;



    //                            default:
    //                        }
    //                    },
    //                    items: {
    //                        "delete": { name: "Delete", icon: "fas fa-trash" }
    //                    }
    //                };


    //        }
    //    }
    //});
    });
    function onDeleteItem(id) {
        var flag = confirm('Do you really want to delete this item?');

        if (flag) {
            $.ajax({
                url:'/IMS/InventoryManagement/DeleteRequisitionItem?NoteId=' + id,
                type: 'POST',
                data: {},
                dataType: 'json',
                success: function (result) {
                    debugger;
                    var grid = $("#ItemsGrid").data("JSGrid");
                    if (result.success) {
                         grid.render();
                        //getItemsData();
                        kendo.alert("Deleted Successfully.");
                    } else {
                        grid.render();
                        //getItemsData();
                        var err = result.errors.BinderCountError.errors[0];
                        kendo.alert(err);
                    }
                },
                error: function (ert) {
                    //getItemsData();
                    var grid = $("#ItemsGrid").data("JSGrid");
                    grid.render();
                }
            });
            return false;
        }
    }
    function AddItem()
    {
        var win = GetMainWindow();
        var url = '/Cms/Page?lo=Popup&cbm=OnAfterItemCreate&source=Create&dataAction=Create&templateCodes=N_IMS_IMS_ITEM_MASTER';
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Add Item', Width: 1000, Height: 800 });
        return false;
    }
    function getItemsData() {
        document.getElementById("ItemsGrid").innerHTML = "";
        gridConfig(
            "ItemsGrid",
            "/IMS/InventoryManagement/ReadRequisitionItemsData?requisitionId="+$("#RequisitionId").val(),
            columnDefs,
            false,
            true,
            true,
            true,
            1,
            true,
            10);
    }

function OnItemChange(e)
{
    debugger;
    $.ajax({
        url: "/IMS/InventoryManagement/GetItemUnitDetails?itemId="+$("#Item").data("kendoDropDownList").value(),
        type: "GET",
        contentType: "application/json",
        dataType: "JSON",
        success: function (response) {
            $("#ItemUnitName").val(response.ItemUnitName);
        }
    });
}
   
        var onAjaxSuccess = function (res) {
            if (res.success) {
                closeNav()
        }
        else {
            alert(res)
            showError(res.error);
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
    //function OnSave(event) {
    //    debugger;
    //    var multiSelect = $("#UserRoles").data("kendoMultiSelect");
    //    var value = multiSelect.value();

    //   // $("#UserRoles").kendoMultiSelect.Data(Read);

    //}
