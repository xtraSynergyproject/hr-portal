
var windowUrl;
var orgId;
var orgName;
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

       if (command == "CreateNewOrganization") {
            //var rsv = encodeURIComponent("/hrs/organizationchart/index?a=1");
            
            //if (type1 == "OrgExcel") {                
            //    rsv = encodeURIComponent("/hrs/organizationchartexcel/index?a=1");
            //}
            //var url = "/hrs/organization/create?parentOrganizationId=" + orgId + "&hierarchyid=2&ru="+rsv;           
           // window.location.href = url;
           var portalId = $('#GlobalPortalId').val();
           var udfs = encodeURIComponent('ParentDepartmentId=' + orgId);
           var roudfs = encodeURIComponent('ParentDepartmentId=true');
           var url = '/Cms/Page?lo=Popup&source=Create&cbm=OnAfterCreate&dataAction=Create&pageName=Department&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;  

           //LoadCmsPartialView(url, 'Note', true, 1200, 650, 'Organization');
           var win = GetMainWindow();
           win.iframeOpenUrl = url;
           win.OpenWindow({ Title: 'Department', Width: 1200, Height: 650 });
        }
        else if (command == "EditOrganization") {
           
           var portalId = $('#GlobalPortalId').val();
           var roudfs = encodeURIComponent('ParentDepartmentId=true');
           var url = '/Cms/Page?lo=Popup&source=Versioning&cbm=OnAfterCreate&dataAction=Edit&pageName=Department&portalId=' + portalId + '&recordId=' + noteId + '&roudfs=' + roudfs; 

           var win = GetMainWindow();
           win.iframeOpenUrl = url;
           win.OpenWindow({ Title: 'Department', Width: 1200, Height: 650 });
        }
       else if (command == "EditOrganizationHierarchy") {     
    
           var portalId = $('#GlobalPortalId').val();
           var roudfs = encodeURIComponent('DepartmentId=true&HierarchyId=true');
           var url = '/Cms/Page?lo=Popup&source=Versioning&cbm=OnAfterCreate&dataAction=Edit&templateCodes=HRDepartmentHierarchy&portalId=' + portalId + '&recordId=' + orgHierarchyNoteId + '&roudfs=' + roudfs;
           var win = GetMainWindow();
           win.iframeOpenUrl = url;
           win.OpenWindow({ Title: 'Department Hierarchy', Width: 1200, Height: 650 });
        }
       
        else if (command == "AddExistingOrganization") {
           var url = "/chr/OrganizationHierarchy/AddExistingDepartment?parentDepartmentId=" + orgId + "&parentDepartmentName=" + orgName + "&hierarchy=" + hierarchyId;
           var win = GetMainWindow();
           win.iframeOpenUrl = url;
           win.OpenWindow({ Title: 'Add Department', Width: 800, Height: 600 });
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
           iframeOpenUrl = "/hrs/Assignment/RenameOrg?orgId=" + orgId ;
           OpenIframePopup(450, 400, "Rename Department");
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
       else if (command == "DepartmentGoal") {          
                          
           var url = '/Pms/Performance/DepartmentGoals?lo=popup&departmentId='+orgId;           
           var win = GetMainWindow();
           win.iframeOpenUrl = url;
           win.OpenWindow({ Title: 'Department Goals', Width: 1200, Height: 650 });
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

function OpenOrgContextMenu(e,y,x) {
    //  
    debugger;
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
            //var result4 = SetEditOrganizationHierarchyMenu(result);
            //var result5 = SetOrganizationDocumentMenu(result);
            //var result6 = SetRenameOrganizationMenu(result);
            //result = result1 || result2 || result3 || result4 || result6;
            result = result1 || result3 
        }
        if (result) {
            menu.find("#Organization").show();
        }
        else {
            menu.find("#Organization").hide();
        }
        return result;

    }

   
    function SetEditOrganizationHierarchyMenu(isEnabled) {
        /*var result = isEnabled && asonDate && OrganizationAuthorize(1041, 451, 452, 453, 454, 455,456, orgId, ccHolderId, reportingLine)*/;
        var result = isEnabled;
        if (result) {
            menu.find("#EditOrganizationHierarchy").show();
        }
        else {
            menu.find("#EditOrganizationHierarchy").hide();
        }
        return result;
    }
   
    function SetCreateNewOrganizationMenu(isEnabled) {
        /*var result = isEnabled && asonDate && OrganizationAuthorize(663, 421, 422, 423, 424, 425,426, orgId, ccHolderId, reportingLine)*/;
        var result = isEnabled;

        if (result) {
            menu.find("#CreateNewOrganization").show();
        }
        else {
            menu.find("#CreateNewOrganization").hide();
        }
        return result;
    }

    function SetEditOrganizationMenu(isEnabled) {
        /*var result = isEnabled && asonDate && OrganizationAuthorize(681, 441, 442, 443, 444, 445,446, orgId, ccHolderId, reportingLine)*/;
        var result = isEnabled;
        if (result) {
            menu.find("#EditOrganization").show();
        }
        else {
            menu.find("#EditOrganization").hide();
        }
        return result;
    }

    function SetAddExistingOrganizationMenu(isEnabled) {
       /* var result = isEnabled && OrganizationAuthorize(1023, 451, 452, 453, 454, 455,456, orgId, ccHolderId, reportingLine)*/;
        var result = isEnabled;
        if (result) {
            menu.find("#AddExistingOrganization").show();
        }
        else {
            menu.find("#AddExistingOrganization").hide();
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





