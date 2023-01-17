
var windowUrl;
var orgId;
var orgName;
var parentId;
var orgHierarchyId;
var empId;
var dh; // Direct head count
var reportingLine;
var ccHolderId;
var lvl;
var hc;
var isNormalMode;
var stdWidth = 1200;
var stdHeight = 650;
var gridWidth = 1400;
var gridHeight = 650;
var type1;
var trgt;
var noteId;
var orgHierarchyNoteId;
var hierarchyId;
debugger;

function SetNodeValues(hdn) {
    empId = null;
    positionId = null;
    orgId = hdn.attr("orgId");
    parentId = hdn.attr("parentId");
    orgName = hdn.attr("orgName");
    noteId = hdn.attr("noteId");
    dh = hdn.attr("dh");
    reportingLine = hdn.attr("rl");
    lvl = hdn.attr("lvl");
    hc = hdn.attr("hc");
    ccHolderId = hdn.attr("ccHolderId");
    orgHierarchyId = hdn.attr("orgHierarchyId");
    orgHierarchyNoteId = hdn.attr("orgHierarchyNoteId");
    hierarchyId = hdn.attr("hierarchyId");
}
function OnMenuClick(e) {
    SetNodeValues(trgt);
    var command = $(e.item).attr('id');
    debugger;
    var type = trgt[0].getAttribute("type");
    var WorkflowServiceId = trgt[0].getAttribute("id").split("hr-org-menu-")[1];
    var parentId = trgt[0].getAttribute("parentId");
    var Count = trgt[0].getAttribute("count");
    var Sequence = trgt[0].getAttribute("SequenceOrder");
    var WorkspaceId = trgt[0].getAttribute("parentId");
    var bookId = trgt[0].getAttribute("bookId");
    var pageId = trgt[0].getAttribute("pageid");
    var id = trgt[0].getAttribute("id").split("hr-org-menu-")[1];
    var NodeId = id;

    switch (command) {
        case 'createcategory':
            onCreateCategory();
            break;
        case 'delete':
            OnDeleteBook(WorkflowServiceId, parentId, 2);
            break;
        case 'deleteCategory':
            OnDeleteCategory(WorkflowServiceId, parentId, 2);
            break;
        case 'share':
            var win = GetMainWindow();
            win.iframeOpenUrl = '/Cms/NtsServiceShared?serviceId=' + WorkflowServiceId + '&IsSharingEnabled=true';
            win.OpenWindow({ Title: 'Share With', Width: 1200, Height: 600 });
            break;
        case 'edit':
            var portalId = window.parent.$('#GlobalPortalId').val();

            var url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterServiceCreate&source=Versioning&dataAction=Edit&templateCodes=TEC_PROCESS_CATEGORY&portalId=' + portalId + '&recordId=' + WorkflowServiceId;
            var win = GetMainWindow();
            win.iframeOpenUrl = url;
            win.OpenWindow({ Title: 'Edit Category', Width: 1200, Height: 600 });
            break;
        case 'createbook':
            if (Count == null) {
                Count = 0;
            }
            onCreateGroup(WorkflowServiceId, parseInt(Count) + 1);
            break;
        case 'createbookbelow':
            if (Sequence == null) {
                Sequence = 0;
            }
            onCreateGroup(parentId, Sequence + 1);
            break;
        case 'existingpage':
            debugger;
            if (Count == null) {
                Count = 0;
            }
            var sequence = parseInt(Count) + 1;
            var url = '/Cms/NtsService/AddExistingPage?categoryId=' + WorkspaceId + '&bookId=' + id + '&sequenceOrder=' + sequence + '&actualPages=true';
            var win = GetMainWindow();
            win.iframeOpenUrl = url;
            win.OpenWindow({ Title: 'Add Existing Page', Width: 750, Height: 450 });
            break;
        case 'editgroup':
            debugger;
            var portalId = window.parent.$('#GlobalPortalId').val();
            var url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterServiceCreate&source=Versioning&dataAction=Edit&templateCodes=TEC_PROCESS_GROUP&portalId=' + portalId + '&recordId=' + WorkflowServiceId;
            var win = GetMainWindow();
            win.iframeOpenUrl = url;
            win.OpenWindow({ Title: 'Edit Book', Width: 1200, Height: 600 });
            break;
        case 'page':
            debugger
            if (Count == null) {
                Count = 0;
            }
            onCreatePage(WorkflowServiceId, parseInt(Count) + 1);
            break;
        case 'editpage':
            debugger;
            var portalId = window.parent.$('#GlobalPortalId').val();
            var url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterServiceCreate&source=Versioning&dataAction=Edit&templateCodes=TEC_PROCESS_ITEM&portalId=' + portalId + '&recordId=' + WorkflowServiceId;
            var win = GetMainWindow();
            win.iframeOpenUrl = url;
            win.OpenWindow({ Title: 'Edit Page', Width: 1200, Height: 600 });
            break;
        case 'view':            
            onReadMore(bookId,"", pageId)
            break;
        case 'readBook':           
            onReadMore(bookId,"", pageId)
            break;
        case 'createpagebelow':
            if (Sequence == null) {
                Sequence = 0;
            }
            onCreatePage(parentId, Sequence + 1);
            break;

        case 'deletepage':
            kendo.confirm("Are you sure that you want to proceed?").then(function () {
                $.ajax({
                    url: '/cms/NtsService/DeleteBokPageMapping?bookId=' + WorkspaceId + '&pageId=' + NodeId,
                    type: "GET",
                    contentType: "application/json",
                    dataType: "JSON",
                    success: function (response) {
                        if (response.success) {
                            ShowNotification("Deleted Successfully");
                            OnAfterServiceCreate();
                        }

                    }
                });

            }, function () {

            });
            break;
        default:
    }

    //if (command == "CreateNewUser") {

    //    var portalId = $('#GlobalPortalId').val();
    //    //var udfs = encodeURIComponent('ParentDepartmentId=' + orgId);
    //    //var roudfs = encodeURIComponent('ParentDepartmentId=true');
    //    //var url = '/Cms/Page?lo=Popup&source=Create&cbm=OnAfterCreate&dataAction=Create&pageName=Department&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;  

    //    var url = '/portalAdmin/User/CreateUser?portalId=' + portalId;
    //    var win = GetMainWindow();
    //    win.iframeOpenUrl = url;
    //    win.OpenWindow({ Title: 'User', Width: 1200, Height: 650 });
    //}
    //else if (command == "EditUser") {
    //    debugger;
    //    var portalId = $('#GlobalPortalId').val();
    //    //var roudfs = encodeURIComponent('ParentDepartmentId=true');
    //    //var url = '/Cms/Page?lo=Popup&source=Versioning&cbm=OnAfterCreate&dataAction=Edit&pageName=Department&portalId=' + portalId + '&recordId=' + noteId + '&roudfs=' + roudfs; 
    //    var url = '/PortalAdmin/User/EditUser?Id=' + noteId + "&portalId=" + portalId;
    //    var win = GetMainWindow();
    //    win.iframeOpenUrl = url;
    //    win.OpenWindow({ Title: 'User', Width: 1200, Height: 650 });
    //}
    //else if (command == "EditUserHierarchy") {
    //    debugger;
    //    var portalId = $('#GlobalPortalId').val();
    //    var roudfs = encodeURIComponent('UserId=true&HierarchyId=true');
    //    var url = '/Cms/Page?lo=Popup&source=Versioning&cbm=OnAfterCreate&dataAction=Edit&templateCodes=USER_HIERARCHY&portalId=' + portalId + '&recordId=' + orgHierarchyNoteId + '&roudfs=' + roudfs;
    //    var win = GetMainWindow();
    //    win.iframeOpenUrl = url;
    //    win.OpenWindow({ Title: 'User Hierarchy', Width: 1200, Height: 650 });
    //}

    //else if (command == "AddExistingUser") {
    //    var url = "/portalAdmin/UserHierarchyChart/AddExistingUser?parentUserId=" + orgId + "&parentUserName=" + orgName + "&hierarchy=" + hierarchyId;
    //    var win = GetMainWindow();
    //    win.iframeOpenUrl = url;
    //    win.OpenWindow({ Title: 'Add User', Width: 800, Height: 600 });
    //}
    //else if (command == "AddExistingObject") {
    //    var url = "/portalAdmin/ObjectHierarchyChart/AddExistingObject?parentUserId=" + orgId + "&parentUserName=" + orgName + "&hierarchy=" + hierarchyId;
    //    var win = GetMainWindow();
    //    win.iframeOpenUrl = url;
    //    win.OpenWindow({ Title: 'Add User', Width: 800, Height: 600 });
    //}
    //else if (command == "RemoveFromHierarchy") {
    //    var flag = confirm('Do you really want to delete?');
    //    var url = "/portalAdmin/UserHierarchyChart/DeleteHierarchy?noteId=" + orgHierarchyNoteId;
    //    if (flag) {
    //        $.ajax({
    //            url: url,
    //            type: 'GET',
    //            dataType: 'json',
    //            success: function (result) {
    //                if (result.success) {
    //                    alert("Removed Successfully");
    //                    OnAfterCreate();
    //                }
    //                else {

    //                    alert(result.error);
    //                }
    //            },
    //            error: function (ert) {


    //            }
    //        });
    //    }
    //}

    //else if (command == "GoToPositionChart") {
    //    if (ccHolderId == '' || eval(ccHolderId) <= 0) {
    //        alert("There is no cost center holder for this organization");
    //        return true;
    //    }
    //    else {
    //        window.location.href = "/positionchart/index?posid=" + ccHolderId;
    //    }

    //}
    //else if (command == "ExpandAll") {
    //    MultiLevelExpandCollapse(orgId, 1000);
    //}
    //else if (command == "Expand2") {
    //    MultiLevelExpandCollapse(orgId, 2);
    //}
    //else if (command == "Expand3") {
    //    MultiLevelExpandCollapse(orgId, 3);
    //}
    //else if (command == "Expand4") {
    //    MultiLevelExpandCollapse(orgId, 4);
    //}
    //else if (command == "Expand5") {
    //    MultiLevelExpandCollapse(orgId, 5);
    //}
    //else if (command == "CollapseAll") {
    //    CollapseAll(orgId);
    //}
    menu.hide();
}




//function OnExcelMenuClick(e) {
//    var command = $(e.item).attr('id');

//    if (command == "CreateNewOrganization") {
//        var rsv = encodeURIComponent("/hrs/organizationchart/index?a=1");

//        if (type1 == "OrgExcel") {
//            rsv = encodeURIComponent("/hrs/organizationchartexcel/index?a=1");
//        }
//        var url = "/hrs/organization/create?parentOrganizationId=" + orgId + "&hierarchyid=2&rs=" + rsv;
//        window.location.href = url;
//    }
//    else if (command == "EditOrganization") {
//        var rsv = encodeURIComponent("/hrs/organizationchart/index?a=1");
//        if (type1 == "OrgExcel") {
//            rsv = encodeURIComponent("/hrs/organizationchartexcel/index?a=1");
//        }
//        iframeOpenUrl = "/hrs/organization/history?OrgId=" + orgId + "&hierarchyid=2&rs=" + rsv;
//        OpenIframePopup(stdWidth, stdHeight, "Manage Department");
//    }
//    else if (command == "EditOrganizationHierarchy") {
//        iframeOpenUrl = "/hrs/Organizationhierarchy/history?OrganizationHierarchyId=" + orgHierarchyId + "&hierarchyId=" + hierarchyId;
//        OpenIframePopup(stdWidth, stdHeight, "Manage Department");
//    }

//    else if (command == "AddExistingOrganization") {
//        var rsv = encodeURIComponent("/hrs/organizationchart/index?a=1");
//        if (type1 == "OrgExcel") {
//            rsv = encodeURIComponent("/hrs/organizationchartexcel/index?a=1");
//        }
//        var url = "/hrs/Organizationhierarchy/CreateHierarchy?parentOrgId=" + orgId + "&hierarchyid=2&rs=" + rsv;
//        window.location.href = url;
//    }
//    else if (command == "OrganizationDocument") {
//        var ru = encodeURIComponent("/hrs/organizationchart/index?orgId=" + orgId);
//        if (type1 == "OrgExcel") {
//            ru = encodeURIComponent("/hrs/organizationchartexcel/index?a=1");
//        }
//        iframeOpenUrl = "/nts/note/GetDocument?type=ORGANIZATION_DOCUMENT&tagtotype=Organization&tagtoid=" + orgId + "&ru=" + ru;
//        OpenIframePopup(stdWidth, stdHeight, "Department Document");
//    }
//    else if (command == "RenameOrganization") {
//        var ru = encodeURIComponent("/hrs/organizationchart/index?orgId=" + orgId);
//        if (type1 == "OrgExcel") {
//            ru = encodeURIComponent("/hrs/organizationchartexcel/index?a=1");
//        }
//        iframeOpenUrl = "/hrs/Assignment/RenameOrg?orgId=" + orgId;
//        OpenIframePopup(450, 400, "Rename Department");
//    }
//    //else if (command == "ViewDocument") {
//    //    var rsv = encodeURIComponent("/hrs/organizationchart/index?a=1");
//    //    if (type1 == "OrgExcel") {
//    //        rsv = encodeURIComponent("/hrs/organizationchartexcel/index?a=1");
//    //    }
//    //    OpenRadIframeWindow2("../general/OrganizationDocument/ViewDocument?parentOrganizationId=" + orgId + "&hierarchyid=2&rs="+rsv, null, "Manage Department", stdWidth, stdHeight);
//    //}

//    else if (command == "GoToPositionChart") {
//        if (ccHolderId == '' || eval(ccHolderId) <= 0) {
//            alert("There is no cost center holder for this department");
//            return true;
//        }
//        else {
//            window.location.href = "/positionchart/index?posid=" + ccHolderId;
//        }

//    }
//    else if (command == "ExpandAll") {
//        MultiLevelExpandCollapse(orgId, 1000);
//    }
//    else if (command == "Expand2") {
//        MultiLevelExpandCollapse(orgId, 2);
//    }
//    else if (command == "Expand3") {
//        MultiLevelExpandCollapse(orgId, 3);
//    }
//    else if (command == "Expand4") {
//        MultiLevelExpandCollapse(orgId, 4);
//    }
//    else if (command == "Expand5") {
//        MultiLevelExpandCollapse(orgId, 5);
//    }
//    else if (command == "CollapseAll") {
//        CollapseAll(orgId);
//    }
//}

function OpenContextMenu(e, y, x) {

    menu = $("#menu");
    trgt = $(e);   
    SetNodeValues(trgt);
    var isEnabled = true;
    debugger;
    var bookId = trgt[0].getAttribute("bookId");
    var type = trgt[0].getAttribute("type");
    var permissions = trgt[0].getAttribute("permission");
    var isAdminstr = trgt[0].getAttribute("isadmin");
    var isAdmin = false;
    //if (isAdminstr == "True") {
    //    isAdmin = true;
    //}
     if (type == "Document") {
        // if (permissions != null && permissions.includes("CanManageBook") || isAdmin) {

        menu.find("#readBook").show();
       //  onReadMore(bookId)


    }
    else {
        menu.find("#page").hide();
        menu.find("#existingpage").hide();
        menu.find("#createbookbelow").hide();
        menu.find("#share").hide();
        menu.find("#delete").hide();
        menu.find("#createcategory").hide();
        menu.find("#readBook").hide();

    }
    /* var result1 = SetOrganizationMenu(isEnabled);*/
    //var result2 = SetGoToPositionChart(false);
    //var result3 = SetExportToExcelMenu(false);
    //var result4 = SetExpandCollapse(false);
    //var result5 = SetSelect(false);
    //var result6 = SetSynergyMenu(false);
    // var result = result1 || result2 || result3 || result4 || result5 || result6;
    var result = true;
    if (result) {
      // var pos= trgt.position();
        menu.css({ top: event.pageY - 150, left: event.pageX - 500, position: 'absolute' });
        menu.show();
       // menu.show();

    }
    return false;
}

function onCreateCategory() {
    var portalId = window.parent.$('#GlobalPortalId').val();

    var url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&templateCodes=TEC_PROCESS_CATEGORY&portalId=' + portalId;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: 'Create Category', Width: 1200, Height: 600 });
}

//function SetExpandCollapse(isEnabled) {
//    var result = false;
//    if (hc > 0 && isEnabled) {
//        result = true;
//        menu.find("#CollapseAll").show();
//        menu.find("#Expand").show();
//        if (lvl > 5) {
//            menu.find("#Expand2").hide();
//            menu.find("#Expand3").hide();
//        }
//    }
//    else {
//        menu.find("#CollapseAll").hide();
//        menu.find("#Expand").hide();
//    }
//    return result && isEnabled;
//}
//function SetExportToExcelMenu(isEnabled) {
//        /*var result = isEnabled && OrganizationAuthorize(403, 412, 410, 408, 3091, 3094, 3095, orgId, ccHolderId, reportingLine)*/;
//    var result = isEnabled;
//    if (result) {
//        menu.find("#ExportToExcel").show();
//    }
//    else {
//        menu.find("#ExportToExcel").hide();
//    }
//    return result;
//}


