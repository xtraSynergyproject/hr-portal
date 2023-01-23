
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
    depttype = hdn.attr("type");
}
function OnMenuClick(e) {    
        
    SetNodeValues(trgt);
    var command = $(e.item).attr('id');  
    var portalId = $('#GlobalPortalId').val();

    if (command == "CreateNewUser") {

        var portalId = $('#GlobalPortalId').val();
        //var udfs = encodeURIComponent('ParentDepartmentId=' + orgId);
        //var roudfs = encodeURIComponent('ParentDepartmentId=true');
        //var url = '/Cms/Page?lo=Popup&source=Create&cbm=OnAfterCreate&dataAction=Create&pageName=Department&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;  

        var url = '/portalAdmin/User/CreateUser?portalId=' + portalId;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'User', Width: 1200, Height: 650 });
    }
    else if (command == "EditUser") {
        
        var portalId = $('#GlobalPortalId').val();
        //var roudfs = encodeURIComponent('ParentDepartmentId=true');
        //var url = '/Cms/Page?lo=Popup&source=Versioning&cbm=OnAfterCreate&dataAction=Edit&pageName=Department&portalId=' + portalId + '&recordId=' + noteId + '&roudfs=' + roudfs; 
        var url = '/PortalAdmin/User/EditUser?Id=' + noteId + "&portalId=" + portalId;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'User', Width: 1200, Height: 650 });
    }
    else if (command == "EditUserHierarchy") {
        
        var portalId = $('#GlobalPortalId').val();
        var roudfs = encodeURIComponent('UserId=true&HierarchyId=true');
        var url = '/Cms/Page?lo=Popup&source=Versioning&cbm=OnAfterCreate&dataAction=Edit&templateCodes=USER_HIERARCHY&portalId=' + portalId + '&recordId=' + orgHierarchyNoteId + '&roudfs=' + roudfs;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'User Hierarchy', Width: 1200, Height: 650 });
    }

    else if (command == "AddExistingUser") {
        var url = "/portalAdmin/UserHierarchyChart/AddExistingUser?parentUserId=" + orgId + "&parentUserName=" + orgName + "&hierarchy=" + hierarchyId;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Add User', Width: 800, Height: 600 });
    }
    else if (command == "AddExistingObject") {
        var url = "/portalAdmin/ObjectHierarchyChart/AddExistingObject?parentUserId=" + orgId + "&parentUserName=" + orgName + "&hierarchy=" + hierarchyId;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Add User', Width: 800, Height: 600 });
    }
    else if (command == "RemoveFromHierarchy") {
        var flag = confirm('Do you really want to delete?');
        var url = "/portalAdmin/UserHierarchyChart/DeleteHierarchy?noteId=" + orgHierarchyNoteId;
        if (flag) {
            $.ajax({
                url: url,
                type: 'GET',
                dataType: 'json',
                success: function (result) {
                    if (result.success) {
                        alert("Removed Successfully");
                        OnAfterCreate();
                    }
                    else {

                        alert(result.error);
                    }
                },
                error: function (ert) {


                }
            });
        }
    }

    else if (command == "GoToPositionChart") {
        if (ccHolderId == '' || eval(ccHolderId) <= 0) {
            alert("There is no cost center holder for this organization");
            return true;
        }
        else {
            window.location.href = "/positionchart/index?posid=" + ccHolderId;
        }

    }
    else if (command == "ExpandAll") {
        MultiLevelExpandCollapse(orgId, 1000);
    }
    else if (command == "Expand2") {
        MultiLevelExpandCollapse(orgId, 2);
    }
    else if (command == "Expand3") {
        MultiLevelExpandCollapse(orgId, 3);
    }
    else if (command == "Expand4") {
        MultiLevelExpandCollapse(orgId, 4);
    }
    else if (command == "Expand5") {
        MultiLevelExpandCollapse(orgId, 5);
    }
    else if (command == "CollapseAll") {
        CollapseAll(orgId);
    }
    else if (command == "createPerson") {

        var udfs = "";
        var rudfs = "";
        const myArray = noteId.split("$");
        const udf = ["OrgLevel1Id", "OrgLevel2Id", "OrgLevel3Id", "OrgLevel4Id", "BrandId", "MarketId", "ProvinceId", "CareerLevelId", "DepartmentId", "JobId"];
        for (i = 0; i < myArray.length; i++) {
            udfs = udfs.concat(udf[i], "=", myArray[i], "&");
            rudfs = rudfs.concat(udf[i], "=true&");
        }

        var udfs1 = encodeURIComponent('' + udfs);
        var roudfs = encodeURIComponent('' + rudfs);
        var url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=NEW_PERSON_REQ&portalId=' + portalId + '&udfs=' + udfs1 + '&roudfs=' + roudfs;

        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Create Person', Width: 1200, Height: 650 });

    }
    else if (command == "editPerson") {

        const myArray = noteId.split("$");
        var len = myArray.length;
        var perId = myArray[len - 1];
        var data;

        $.ajax({
            url: "/CHR/HRCore/GetPersonDetails?personId=" + perId,
            type: 'GET',
            dataType: 'json',
            success: function (result) {

                if (result.success) {

                    data = result.perdata;

                    var udfs = encodeURIComponent('EmployeeId=' + data.Id + '&TitleId=' + data.TitleId + '&FirstName=' + data.FirstName + '&LastName=' + data.LastName + '&EmailId=' + data.PersonalEmail + '&GenderId=' + data.GenderId + '&DateOfBirth=' + data.DateOfBirth + '&DateOfJoin=' + data.DateOfJoin + '&NationalityId=' + data.NationalityId + '&ReligionId=' + data.ReligionId + '&MaritalStatusId=' + data.MaritalStatusId + '&LineManagerId=' + data.LineManagerId);

                    var roudfs = encodeURIComponent('EmployeeId=true&TitleId=true&FirstName=true&LastName=true&EmailId=true&GenderId=true&DateOfBirth=true&DateOfJoin=true&NationalityId=true&ReligionId=true&LineManagerId=true&MaritalStatusId=true');

                    var url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=EDIT_PERSON_REQ&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;

                    var win = GetMainWindow();
                    win.iframeOpenUrl = url;
                    win.OpenWindow({ Title: 'Edit Person', Width: 1200, Height: 650 });

                }
                else {
                    alert(result.error);
                }
            },
            error: function (ert) {

            }
        });


    }
    else if (command == "changeLM") {

        const myArray = noteId.split("$");
        var len = myArray.length;
        var perId = myArray[len - 1];
        var data;

        $.ajax({
            url: "/CHR/HRCore/GetPersonDetails?personId=" + perId,
            type: 'GET',
            dataType: 'json',
            success: function (result) {

                if (result.success) {

                    data = result.perdata;

                    var udfs = encodeURIComponent('EmployeeId=' + data.Id + '&CurrentLineManagerId=' + data.LineManagerId);

                    var roudfs = encodeURIComponent('EmployeeId=true&CurrentLineManagerId=true');

                    var url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=CHANGE_LM_REQ&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;

                    var win = GetMainWindow();
                    win.iframeOpenUrl = url;
                    win.OpenWindow({ Title: 'Change Line Manager', Width: 1200, Height: 650 });

                }
                else {
                    alert(result.error);
                }
            },
            error: function (ert) {

            }
        });

    }
    else if (command == "changeAssignment") {

        const myArray = noteId.split("$");
        var len = myArray.length;
        var perId = myArray[len - 1];
        var data;
        $.ajax({
            url: "/CHR/HRCore/GetAssignmentDetails?personId=" + perId,
            type: 'GET',
            dataType: 'json',
            success: function (result) {

                if (result.success) {

                    data = result.assdata;

                    var udfs = encodeURIComponent('UserId=' + data.PersonId + '&DepartmentId=' + data.DepartmentId + '&JobId=' + data.JobId + '&PositionId=' + data.PositionId + '&LocationId=' + data.LocationId + '&AssignmentGradeId=' + data.AssignmentGradeId + '&AssignmentTypeId=' + data.AssignmentTypeId + '&DateOfJoin=' + data.DateOfJoin + '&OrgLevel1Id=' + data.OrgLevel1Id
                        + '&OrgLevel2Id=' + data.OrgLevel2Id + '&OrgLevel3Id=' + data.OrgLevel3Id + '&OrgLevel4Id=' + data.OrgLevel4Id + '&BrandId=' + data.BrandId + '&MarketId=' + data.MarketId + '&ProvinceId=' + data.ProvinceId + '&CareerLevelId=' + data.CareerLevelId);

                    var roudfs = encodeURIComponent('UserId=true&DepartmentId=true&JobId=true&PositionId=true&LocationId=true&AssignmentGradeId=true&AssignmentTypeId=true&DateOfJoin=true&OrgLevel1Id=true&OrgLevel2Id=true&OrgLevel3Id=true&OrgLevel4Id=true&BrandId=true&MarketId=true&ProvinceId=true&CareerLevelId=true');

                    var url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=CHANGE_ASSIGNMENT_REQ&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;

                    var win = GetMainWindow();
                    win.iframeOpenUrl = url;
                    win.OpenWindow({ Title: 'Change Assignment', Width: 1200, Height: 650 });

                }
                else {
                    alert(result.error);
                }
            },
            error: function (ert) {

            }
        });


    }
    else if (command == "renameJobRequest") {

        const myArray = noteId.split("$");
        var len = myArray.length;
        var perId = myArray[len - 1];
        var data;

        $.ajax({
            url: "/CHR/HRCore/GetAssignmentDetails?personId=" + perId,
            type: 'GET',
            dataType: 'json',
            success: function (result) {

                if (result.success) {

                    data = result.assdata;

                    var udfs = encodeURIComponent('EmployeeId=' + data.PersonId + '&CurrentJobId=' + data.JobId);

                    var roudfs = encodeURIComponent('EmployeeId=true&CurrentJobId=true');

                    var url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=RENAME_JOB_REQ&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;

                    var win = GetMainWindow();
                    win.iframeOpenUrl = url;
                    win.OpenWindow({ Title: 'Rename Job Request', Width: 1200, Height: 650 });

                }
                else {
                    alert(result.error);
                }
            },
            error: function (ert) {

            }
        });

    }
    else if (command == "renameDeptRequest") {
        
        const myArray = noteId.split("$");
        var len = myArray.length;
        var depId = myArray[len - 1];

        var udfs = encodeURIComponent('CurrentDepartmentId=' + depId);

        var roudfs = encodeURIComponent('CurrentDepartmentId=true');
        var url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=RENAME_DEPARTMENT_REQ&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;

        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Rename Department Request', Width: 1200, Height: 650 });
    }
    else if (command == "newChild") {
        
        const myArray = noteId.split("$");
        var len = myArray.length;
        var depId = myArray[len - 1];

        var udfs = encodeURIComponent('BusinessHierarchyParentId=' + depId);

        var roudfs = encodeURIComponent('BusinessHierarchyParentId=true');
        var url = '/Cms/Page?lo=Popup&pageType=Note&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=HRDepartment&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;

        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Add New Child Item', Width: 1200, Height: 650 });
    }
    else if (command == "existingChildItem") {
        const myArray = noteId.split("$");
        var len = myArray.length;
        var depId = myArray[len - 1];

        
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Add Existing Child Item', Width: 1200, Height: 650 });
    }
    menu.hide();
}




function OnExcelMenuClick(e) {
    var command = $(e.item).attr('id');

    if (command == "CreateNewOrganization") {
        var rsv = encodeURIComponent("/hrs/organizationchart/index?a=1");

        if (type1 == "OrgExcel") {
            rsv = encodeURIComponent("/hrs/organizationchartexcel/index?a=1");
        }
        var url = "/hrs/organization/create?parentOrganizationId=" + orgId + "&hierarchyid=2&rs=" + rsv;
        window.location.href = url;
    }
    else if (command == "EditOrganization") {
        var rsv = encodeURIComponent("/hrs/organizationchart/index?a=1");
        if (type1 == "OrgExcel") {
            rsv = encodeURIComponent("/hrs/organizationchartexcel/index?a=1");
        }
        iframeOpenUrl = "/hrs/organization/history?OrgId=" + orgId + "&hierarchyid=2&rs=" + rsv;
        OpenIframePopup(stdWidth, stdHeight, "Manage Department");
    }
    else if (command == "EditOrganizationHierarchy") {
        iframeOpenUrl = "/hrs/Organizationhierarchy/history?OrganizationHierarchyId=" + orgHierarchyId + "&hierarchyId=" + hierarchyId;
        OpenIframePopup(stdWidth, stdHeight, "Manage Department");
    }

    else if (command == "AddExistingOrganization") {
        var rsv = encodeURIComponent("/hrs/organizationchart/index?a=1");
        if (type1 == "OrgExcel") {
            rsv = encodeURIComponent("/hrs/organizationchartexcel/index?a=1");
        }
        var url = "/hrs/Organizationhierarchy/CreateHierarchy?parentOrgId=" + orgId + "&hierarchyid=2&rs=" + rsv;
        window.location.href = url;
    }
    else if (command == "OrganizationDocument") {
        var ru = encodeURIComponent("/hrs/organizationchart/index?orgId=" + orgId);
        if (type1 == "OrgExcel") {
            ru = encodeURIComponent("/hrs/organizationchartexcel/index?a=1");
        }
        iframeOpenUrl = "/nts/note/GetDocument?type=ORGANIZATION_DOCUMENT&tagtotype=Organization&tagtoid=" + orgId + "&ru=" + ru;
        OpenIframePopup(stdWidth, stdHeight, "Department Document");
    }
    else if (command == "RenameOrganization") {
        var ru = encodeURIComponent("/hrs/organizationchart/index?orgId=" + orgId);
        if (type1 == "OrgExcel") {
            ru = encodeURIComponent("/hrs/organizationchartexcel/index?a=1");
        }
        iframeOpenUrl = "/hrs/Assignment/RenameOrg?orgId=" + orgId;
        OpenIframePopup(450, 400, "Rename Department");
    }
    //else if (command == "ViewDocument") {
    //    var rsv = encodeURIComponent("/hrs/organizationchart/index?a=1");
    //    if (type1 == "OrgExcel") {
    //        rsv = encodeURIComponent("/hrs/organizationchartexcel/index?a=1");
    //    }
    //    OpenRadIframeWindow2("../general/OrganizationDocument/ViewDocument?parentOrganizationId=" + orgId + "&hierarchyid=2&rs="+rsv, null, "Manage Department", stdWidth, stdHeight);
    //}

    else if (command == "GoToPositionChart") {
        if (ccHolderId == '' || eval(ccHolderId) <= 0) {
            alert("There is no cost center holder for this department");
            return true;
        }
        else {
            window.location.href = "/positionchart/index?posid=" + ccHolderId;
        }

    }
    else if (command == "ExpandAll") {
        MultiLevelExpandCollapse(orgId, 1000);
    }
    else if (command == "Expand2") {
        MultiLevelExpandCollapse(orgId, 2);
    }
    else if (command == "Expand3") {
        MultiLevelExpandCollapse(orgId, 3);
    }
    else if (command == "Expand4") {
        MultiLevelExpandCollapse(orgId, 4);
    }
    else if (command == "Expand5") {
        MultiLevelExpandCollapse(orgId, 5);
    }
    else if (command == "CollapseAll") {
        CollapseAll(orgId);
    }
}

function OpenContextMenu(e, y, x) {
    
        menu = $("#menu");
        trgt = $(e);
        SetNodeValues(trgt);
        var isEnabled = true;

        var result1 = SetOrganizationMenu(isEnabled);
        //var result2 = SetGoToPositionChart(false);
        //var result3 = SetExportToExcelMenu(false);
        //var result4 = SetExpandCollapse(false);
        //var result5 = SetSelect(false);
        //var result6 = SetSynergyMenu(false);
        // var result = result1 || result2 || result3 || result4 || result5 || result6;
        var result = result1;
        if (result) {
            menu.css({ top: event.pageY-y, left: event.pageX-x, position: 'absolute' });
            //menu.css({ top: event.pageY, left: event.pageX, position: 'absolute' });
            menu.show();

        }
        return false;
}

function HybridContextMenu(e, y, x) {

    menu = $("#hybridHierarchychartmenu");
    trgt = $(e);
    SetNodeValues(trgt);
    var isEnabled = true;

    var result1 = SetOrganizationMenu(isEnabled);
    
    var result = result1;
    if (result) {
        menu.css({ top: event.pageY - y, left: event.pageX - x, position: 'absolute' });
        //menu.css({ top: event.pageY, left: event.pageX, position: 'absolute' });
        menu.show();

    }
    return false;
}
  
    function SetExpandCollapse(isEnabled) {
        var result = false;
        if (hc > 0 && isEnabled) {
            result = true;
            menu.find("#CollapseAll").show();
            menu.find("#Expand").show();
            if (lvl > 5) {
                menu.find("#Expand2").hide();
                menu.find("#Expand3").hide();
            }
        }
        else {
            menu.find("#CollapseAll").hide();
            menu.find("#Expand").hide();
        }
        return result && isEnabled;
    }
    function SetExportToExcelMenu(isEnabled) {
        /*var result = isEnabled && OrganizationAuthorize(403, 412, 410, 408, 3091, 3094, 3095, orgId, ccHolderId, reportingLine)*/;
        var result = isEnabled;
        if (result) {
            menu.find("#ExportToExcel").show();
        }
        else {
            menu.find("#ExportToExcel").hide();
        }
        return result;
    }

    function SetOrganizationMenu(isEnabled) {
        var result = isEnabled;
        if (result) {
            var result1 = SetCreateNewOrganizationMenu(result);
            //var result2 = SetAddExistingOrganizationMenu(result);
            var result3 = SetEditOrganizationMenu(result);
            var result4 = SetRemoveUserHierarchyMenu(result);
            //var result5 = SetOrganizationDocumentMenu(result);
            //var result6 = SetRenameOrganizationMenu(result);
            //result = result1 || result2 || result3 || result4 || result6;
            result = result1 || result3 
        }
        if (result) {
            menu.find("#User").show();
        }
        else {
            menu.find("#User").hide();
        }
        return result;

    }

   
    function SetEditOrganizationHierarchyMenu(isEnabled) {
        /*var result = isEnabled && asonDate && OrganizationAuthorize(1041, 451, 452, 453, 454, 455,456, orgId, ccHolderId, reportingLine)*/;
        var result = isEnabled;
        if (result) {
            menu.find("#EditUserHierarchy").show();
        }
        else {
            menu.find("#EditUserHierarchy").hide();
        }
        return result;
}
function SetRemoveUserHierarchyMenu(isEnabled) {
       
    var result = isEnabled;
    if (parentId == null || parentId == undefined || parentId == "" || parentId == "null") {
        menu.find("#RemoveFromHierarchy").hide();
    }
    else
    {
        if (result) {
            menu.find("#RemoveFromHierarchy").show();
        }
        else {
            menu.find("#RemoveFromHierarchy").hide();
        }
    }
    
    return result;
}
   
    function SetCreateNewOrganizationMenu(isEnabled) {
        /*var result = isEnabled && asonDate && OrganizationAuthorize(663, 421, 422, 423, 424, 425,426, orgId, ccHolderId, reportingLine)*/;
        var result = isEnabled;

        if (result) {
            menu.find("#CreateNewUser").show();
        }
        else {
            menu.find("#CreateNewUser").hide();
        }
        return result;
    }

    function SetEditOrganizationMenu(isEnabled) {
        /*var result = isEnabled && asonDate && OrganizationAuthorize(681, 441, 442, 443, 444, 445,446, orgId, ccHolderId, reportingLine)*/;
        var result = isEnabled;
        if (result) {
            menu.find("#EditUser").show();
        }
        else {
            menu.find("#EditUser").hide();
        }
        return result;
    }

    function SetAddExistingOrganizationMenu(isEnabled) {
       /* var result = isEnabled && OrganizationAuthorize(1023, 451, 452, 453, 454, 455,456, orgId, ccHolderId, reportingLine)*/;
        var result = isEnabled;
        if (result) {
            menu.find("#AddExistingUser").show();
        }
        else {
            menu.find("#AddExistingUser").hide();
        }
        return result;
    }
    function SetOrganizationDocumentMenu(isEnabled) {
        var result = isEnabled;
        if (result) {
            menu.find("#OrganizationDocument").show();
        }
        else {
            menu.find("#OrganizationDocument").hide();
        }
        return result;
}
function SetRenameOrganizationMenu(isEnabled) {
    var result = isEnabled;
    if (result) {
        menu.find("#RenameOrganization").show();
    }
    else {
        menu.find("#RenameOrganization").hide();
    }
    return result;
}

    //function SetViewDocument(isEnabled) {
    //    var result = isEnabled && OrganizationAuthorize(418, 471, 472, 473, 474, 475, 476, orgId, ccHolderId, reportingLine);
    //    if (result) {
    //        menu.find("ViewDocument").show();
    //    }
    //    else {
    //        menu.find("ViewDocument").hide();
    //    }
    //    return result;
    //}

    function SetGoToPositionChart(isEnabled) {
        var result = isEnabled && HasPermission(300);
        if (result) {
            menu.find("#GoToPositionChart").show();
        }
        else {
            menu.find("#GoToPositionChart").hide();
        }
        return result;
    }


function OpenBusinessChartContextMenu(e, y, x) {

    menu = $("#businesschartmenu");
    trgt = $(e);
    SetNodeValues(trgt);
    var isEnabled = true;
    

    var type = trgt[0].getAttribute("type");
    var permissions = trgt[0].getAttribute("permission");
    var isAdminstr = trgt[0].getAttribute("isadmin");
    var isAdmin = false;
    if (isAdminstr == "True") {
        isAdmin = true;
    }
    if (type == "null") {
        menu.find("#createPerson").hide();
        menu.find("#editPerson").hide();
        menu.find("#changeLM").hide();
        menu.find("#changeAssignment").hide();
        menu.find("#renameJobRequest").hide();
        menu.find("#renameDeptRequest").hide(); 
    }
    else if (type != "Employee" && type != "Department" && type != "Job") {
        
        menu.find("#createPerson").show();
        menu.find("#editPerson").hide();
        menu.find("#changeLM").hide();
        menu.find("#changeAssignment").hide();
        menu.find("#renameJobRequest").hide();
        menu.find("#renameDeptRequest").hide();     

    }
    else if (type == "Department") {
        menu.find("#createPerson").show();
        menu.find("#editPerson").hide();
        menu.find("#changeLM").hide();
        menu.find("#changeAssignment").hide();
        menu.find("#renameJobRequest").hide();
        menu.find("#renameDeptRequest").show(); 

    }
    else if (type == "Job"){
        menu.find("#createPerson").show();
        menu.find("#editPerson").hide();
        menu.find("#changeLM").hide();
        menu.find("#changeAssignment").hide();
        menu.find("#renameJobRequest").hide();
        menu.find("#renameDeptRequest").hide(); 
    }
    else if (type == "Employee") {
        menu.find("#createPerson").hide();
        menu.find("#editPerson").show();
        menu.find("#changeLM").show();
        menu.find("#changeAssignment").show();
        menu.find("#renameJobRequest").show();
        menu.find("#renameDeptRequest").hide();
    }
    
    var result = true;
    if (result) {
        menu.css({ top: event.pageY - y, left: event.pageX - x, position: 'absolute' });
        //menu.css({ top: event.pageY, left: event.pageX, position: 'absolute' });
        menu.show();

    }
    return false;
}




