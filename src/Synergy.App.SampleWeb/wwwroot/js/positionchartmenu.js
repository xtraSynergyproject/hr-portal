
var menu, trgt, hasPos, hasEmp, posId, posHierarchyId, ru, url, empId, noteId
    , userId, empNumber, jdReference, hasUser, hasOrg, orgId, reportingLine, orgReportingLine, lvl
    , hc, posTypeId, jobId, pPosId, pJobId, nodeType, stdWidth = 1200, stdHeight = 650, gridWidth = 1400
    , gridHeight = 650, assignmentId, type1, posHierarchynoteId, posName, hierarchyId;

function SetNodeValues(hdn) {
    posId = hdn.attr("posid");
    posName = hdn.attr("posName");
    noteId = hdn.attr("noteId");
    posHierarchynoteId = hdn.attr("posHierarchynoteId");
    hierarchyId = hdn.attr("hierarchyid");
    empId = hdn.attr("empId") == 'null' ? "" : hdn.attr("empId");
    userId = hdn.attr("userId") == 'null' ? "" : hdn.attr("userId");
    loggedInUser = hdn.attr("loggedInUser") == 'null' ? "" : hdn.attr("loggedInUser");
    empNumber = hdn.attr("EmpNumber");
    orgId = hdn.attr("orgId");
    posHierarchyId = hdn.attr("posHierarchyId");
    lvl = hdn.attr("lvl");
    hc = hdn.attr("hc");
    jdReference = hdn.attr("jd");
    reportingLine = hdn.attr("rl");
    orgReportingLine = hdn.attr("orl");
    //hasUser = (userId != 'null' && userId != 0 && userId != '' && chartMode == 'Normal');
    hasUser = hdn.attr("hasUser");   
  
    //hasEmp = (empId != 'null' && empId != 0 && empId != '' && chartMode == 'Normal');
    hasPos = (posId != 'null' && posId != 0 && posId != '' && chartMode == 'Normal');
    hasOrg = (orgId != 'null' && orgId != 0 && orgId != '');
    posTypeId = hdn.attr("pt");
    jobId = hdn.attr("jobId");
    jobNId = hdn.attr("jobNId");
    empNId = hdn.attr("empNId");
    hasEmp = (empNId != 'null' && empNId != 0 && empNId != '');
    posNId = hdn.attr("posNId");
    assNId = hdn.attr("assNId");
    pPosId = hdn.attr("pPosId");
    nodeType = hdn.attr("nt");
    pJobId = hdn.attr("pjobId");
    assignmentId = hdn.attr("amId");
}
function OnMenuClick(e) {
    
    SetNodeValues(trgt);
    var command = $(e.item).attr('id');
    if (command == "CreateNewPosition") {
        //ru = encodeURIComponent("/hrs/positionchart/index?a=1");
        //if (type1 == "PositionExcel") {
        //    ru = encodeURIComponent("/hrs/PositionChartExcel/index?a=1");
        //}
        //url = "/hrs/position/create?parentPositionId=" + posId + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate + "&ru=" + ru;        
        //window.location.href = url;
        var portalId = $('#GlobalPortalId').val();
        var prms = encodeURIComponent('ParentPositionId=' + posId);
        var roudfs = encodeURIComponent('ParentPositionId=true');
        var url = '/Cms/Page?lo=Popup&source=Create&cbm=OnAfterCreate&dataAction=Create&pageName=Position&portalId=' + portalId + '&udfs=' + prms + '&roudfs=' + roudfs;       
        //LoadCmsPartialView(url, 'Note', true, 1200, 650, 'Position');        
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Position', Width: 1200, Height: 650 });
    }
    else if (command == "AddExistingPosition") {
        var url = "/chr/PositionHierarchy/AddExistingPosition?parentPostionId=" + posId + "&parentPositionName=" + posName + "&hierarchyId=" + hierarchyId;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Add Existing Position', Width: 800, Height: 400 });
    }
    else if (command == "Select") {
        SelectCallBack(posId, jobId, empId, orgId);
    }
    else if (command == "EditPosition") {       
         
        var portalId = $('#GlobalPortalId').val();
        var roudfs = encodeURIComponent('ParentPositionId=true');
        var url = '/Cms/Page?lo=Popup&source=Versioning&cbm=OnAfterCreate&dataAction=Edit&pageName=Position&portalId=' + portalId + '&recordId=' + noteId + '&roudfs=' + roudfs;
                
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Position', Width: 1200, Height: 650 });
    }
    else if (command == "EditPositionHierarchy") {
      
        var note = posHierarchynoteId;
        var portalId = $('#GlobalPortalId').val();
        var roudfs = encodeURIComponent('PositionId=true&HierarchyId=true');
        var url = '/Cms/Page?lo=Popup&source=Versioning&cbm=OnAfterCreate&dataAction=Edit&templateCodes=PositionHierarchy&portalId=' + portalId + '&recordId=' + posHierarchynoteId + '&roudfs=' + roudfs;
        if (note == "null") {
            
            return false;
        }
        else {
            var win = GetMainWindow();
            win.iframeOpenUrl = url;
            win.OpenWindow({ Title: 'Manage Position Hierarchy', Width: 1200, Height: 650 });
        }
       
    }
    else if (command == "AssignEmployee") {
        var portalId = $('#GlobalPortalId').val();
        var udfs = encodeURIComponent('PositionId=' + posId + '&JobId=' + jobId + '&DepartmentId=' + orgId);
        var roudfs = encodeURIComponent('PositionId=true&JobId=true&DepartmentId=true');
        var url = '/Cms/Page?lo=Popup&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=HRAssignment&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Assign Employee', Width: 1200, Height: 650 });
    }
    else if (command == "ManageAssignment") {
      
      
        var portalId = $('#GlobalPortalId').val();
        var prms = encodeURIComponent('ParentPositionId=' + posId);
        var url = '/Cms/Page?lo=Popup&source=Versioning&cbm=OnAfterCreate&dataAction=Edit&templateCodes=HRAssignment&portalId=' + portalId + '&recordId=' + assNId;

        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Assignment', Width: 1200, Height: 650 });
    }
    else if (command == "RenameJob") {
        
         
        var portalId = $('#GlobalPortalId').val();
        //var prms = encodeURIComponent('ParentPositionId=' + posId);
        var url = '/Cms/Page?lo=Popup&source=Versioning&cbm=OnAfterCreate&dataAction=Edit&templateCodes=HRJob&portalId=' + portalId + '&recordId=' + jobNId;

        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Job', Width: 1200, Height: 650 });
    }
   
    else if (command == "HRDirect") {       
        var customurl = '/chr/hrdirect/EmployeeProfile?personId=' + empId +'&lo=Popup' ;
       var win = GetMainWindow();
        win.iframeOpenUrl = customurl;
        win.OpenWindow({ Title: 'Profile', Width: 1200, Height: 650 });
        
    }
    else if (command == "ManageDocument") {
        var url = "/CHR/HRDirect/EmployeeDocument?userId=" + userId+"&lo=Popup";
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Manage Document', Width: 1200, Height: 650 });
    }
    else if (command == "ManagePerson") {
       
        var portalId = $('#GlobalPortalId').val();
        var url = '/Cms/Page?lo=Popup&source=Versioning&cbm=OnAfterCreate&dataAction=Edit&templateCodes=HRPerson&portalId=' + portalId + '&recordId=' + empNId;

        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Person', Width: 1200, Height: 650 });
    }
    else if (command == "ManageUser") {
        var portalId = $('#GlobalPortalId').val();
        var url = '/Cms/User/EditUser?Id=' + userId;

        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'User', Width: 1200, Height: 650 });
        
    }
    else if (command == "CreateNote") {
        var portalId = $('#GlobalPortalId').val();
        var url = '/cms/NtsNote/SelectNoteTemplate?templateCode=&categoryCode=&moduleCodes=&userId=' + userId;//'/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=Project&portalId=' + portalId /*+ '&prms=' + prms*/;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Create Note', Width: 1000, Height: 600, Position: 'Right1' });
       // return false;
    }
    else if (command == "CreateTask") {
        var portalId = $('#GlobalPortalId').val();

        var url = '/cms/NtsTask/SelectTaskTemplate?templateCode=&categoryCode=&moduleCodes=&userId=' + userId;//'/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=Project&portalId=' + portalId /*+ '&prms=' + prms*/;

        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: "Create Task", Width: 1000, Height: 600 });
       // return false;
    }
    else if (command == "CreateService") {       
        var portalId = $('#GlobalPortalId').val();

        var url = "/cms/NtsService/SelectServiceTemplate?templateCode=&categoryCode=&moduleCodes=&userId=" + userId;//'/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=Project&portalId=' + portalId /*+ '&prms=' + prms*/;

        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Create Service', Width: 1000, Height: 600, Position: 'Right1' });
        //return false;
    }
    else if (command == "WorkListDashboard") {
        //var portalId = $('#GlobalPortalId').val();

        var url = "/cms/WorkListDashboard?userId=" + userId+"&lo=Popup";//'/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=Project&portalId=' + portalId /*+ '&prms=' + prms*/;

        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'WorkList Dashboard', Width: 1000, Height: 600, Position: 'Right1' });
        //return false;
    }
    else if (command == "NoteHome") {
        var url = "/cms/NtsNote/NoteHome?noteStatus=&mode=&lo=Popup&moduleCodes=&userId=" + userId;
        var win = GetMainWindow();
        //var url = '/Cms/Page?popup=true&lo=Popup&pageType=Custom&source=Main&dataAction=None&pageName=ServiceHome&portalId=' + portalId /*+ '&prms=' + prms*/;
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Notes', Width: 1000, Height: 1000 });
        //return false;
    }
    else if (command == "ServiceHome") {
        var url = "/cms/NtsService/ServiceHome?serviceStatus=&mode=&lo=Popup&moduleCodes=&userId=" + userId;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Services', Width: 1000, Height: 1000 });
        //return false;
    }
    else if (command == "TaskHome") {
        var url = "/cms/NtsTask/TaskHome?taskStatus=&mode=&lo=Popup&moduleCodes=&userId=" + userId;
        var win = GetMainWindow();
        //var url = '/Cms/Page?popup=true&lo=Popup&pageType=Custom&source=Main&dataAction=None&pageName=ServiceHome&portalId=' + portalId /*+ '&prms=' + prms*/;
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Tasks', Width: 1000, Height: 1000 });
        //return false;
    }
    else if (command == "PersonDocument") {
        ru = encodeURIComponent("/hrs/positionchart/index?a=1");
        if (type1 == "PositionExcel") {
            ru = encodeURIComponent("/hrs/PositionChartExcel/index?a=1");
        }
        iframeOpenUrl = "/nts/note/GetDocument?type=PERSON_DOCUMENT&tagtotype=Person&tagtoid=" + empId + "&ru=" + ru + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate;
        OpenIframePopup(stdWidth, stdHeight, "Person Document");
    }
    else if (command == "PositionDocument") {
        ru = encodeURIComponent("/hrs/positionchart/index?a=1");
        if (type1 == "PositionExcel") {
            ru = encodeURIComponent("/hrs/PositionChartExcel/index?a=1");
        }
        iframeOpenUrl = "/nts/note/GetDocument?type=POSITION_DOCUMENT&tagtotype=Position&tagtoid=" + posId + "&ru=" + ru + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate;
        OpenIframePopup(stdWidth, stdHeight, "Position Document");
    }
    else if (command == "JobDocument") {
        ru = encodeURIComponent("/hrs/positionchart/index?a=1");
        if (type1 == "PositionExcel") {
            ru = encodeURIComponent("/hrs/PositionChartExcel/index?a=1");
        }
        iframeOpenUrl = "/nts/note/GetDocument?type=JOB_DOCUMENT&tagtotype=Job&tagtoid=" + jobId + "&userId=" + userId + "&ru=" + ru + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate;
        OpenIframePopup(stdWidth, stdHeight, "Job Document");
    }
    else if (command == "OrganizationDocument") {
        ru = encodeURIComponent("/hrs/positionchart/index?a=1");
        if (type1 == "PositionExcel") {
            ru = encodeURIComponent("/hrs/PositionChartExcel/index?a=1");
        }
        iframeOpenUrl = "/nts/note/GetDocument?type=ORGANIZATION_DOCUMENT&tagtotype=Organization&tagtoid=" + orgId + "&ru=" + ru + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate;
        OpenIframePopup(stdWidth, stdHeight, "Department Document");
    }
    else if (command == "PerformanceDocument") {
        ru = encodeURIComponent("/hrs/positionchart/index?a=1");
        if (type1 == "PositionExcel") {
            ru = encodeURIComponent("/hrs/PositionChartExcel/index?a=1");
        }
        url = "/pms/performancedocument/manage?userId=" + userId + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate + "&ru=" + ru;
        window.location.href = url;
    }
    else if (command == "PerformanceDashboard") {
        url = "/pms/pmshome/index?userId=" + userId + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate;
        window.location.href = url;
    }
    else if (command == "SuccessionPlanning") {
        ru = encodeURIComponent("/hrs/positionchart/index?a=1");
        if (type1 == "PositionExcel") {
            ru = encodeURIComponent("/hrs/PositionChartExcel/index?a=1");
        }
        url = "/sps/successionplanning/manage?userId=" + userId + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate + "&ru=" + ru;
        window.location.href = url;
    }
    else if (command == "SuccessionDashboard") {
        url = "/sps/spshome/index?userId=" + userId + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate;
        window.location.href = url;
    }
    else if (command == "WorkStructure") {
        window.location.href = "/hrs/WorkStructure/index?positionId=" + posId;
    }
    else if (command == "GoToOrgChart") {
        var canGo = hasOrg && orgReportingLine.indexOf("," + orgId + ",") > -1 && orgReportingLine.indexOf("," + orgAllowedParentId + ",") > -1;
        if (canGo) {
            window.location.href = "/hrs/organizationchart/index?orgid=" + orgId;
        }
        else {
            alert("The Department for this position is not found in Department Chart");
            return true;
        }
    }
    else if (command == "ExpandAll") {
        MultiLevelExpandCollapse(posId, 1000);
    }
    else if (command == "Expand2") {
        MultiLevelExpandCollapse(posId, 2);
    }
    else if (command == "Expand3") {
        MultiLevelExpandCollapse(posId, 3);
    }
    else if (command == "Expand4") {
        MultiLevelExpandCollapse(posId, 4);
    }
    else if (command == "Expand5") {
        MultiLevelExpandCollapse(posId, 5);
    }
    else if (command == "CollapseAll") {
        CollapseAll(posId);
    }
   
    else if (command == "ManageUserHierarchy") {
        var url = "/cms/UserHierarchy/UpdateHierarchy?hierarchyId=" + hierarchyId + "&users=" + posId;
        var win = GetMainWindow();        
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Update Hierarchy Level', Width: 1100, Height: 600 });
    }
    else if (command == "AddExistingItem") {
       // 
        var url = "/cms/UserHierarchy/UpdateUserHierarchy?hierarchyId=" + hierarchyId + "&userId=" + userId + "&levelNo=" + $("#levelNo").val() + "&optionNo=" + $("#optionNo").val() ;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Add Existing Item', Width: 1100, Height: 600 });
    }
    else if (command == "ManagePerformanceObjective") {
        
        var url = "/pms/PerformanceTask/EmployeePerformanceDetails?userId=" + userId + "&layout=2";
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Manage Performance Objective', Width: 1300, Height: 700 });
        setTimeout(loader, 3000);

    }
    menu.hide();
}

function loader() {
    HideLoader($("#global-overlay"));
}

function OnExcelMenuClick(e) {
    // SetNodeValues(trgt);
    var command = $(e.item).attr('id');
    if (command == "CreateNewPosition") {
        ru = encodeURIComponent("/hrs/positionchart/index?a=1");
        if (type1 == "PositionExcel") {
            ru = encodeURIComponent("/hrs/PositionChartExcel/index?a=1");
        }
        url = "/hrs/position/create?parentPositionId=" + posId + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate + "&ru=" + ru;
        window.location.href = url;
    }
    else if (command == "AddExistingPosition") {
        ru = encodeURIComponent("/hrs/positionchart/index?a=1");
        if (type1 == "PositionExcel") {
            ru = encodeURIComponent("/hrs/PositionChartExcel/index?a=1");
        }
        url = "/hrs/Positionhierarchy/AddToHierarchy?parentPosId=" + posId + "&ru=" + ru + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate;
        window.location.href = url;
    }
    else if (command == "Select") {
        SelectCallBack(posId, jobId, empId, orgId);
    }
    else if (command == "EditPosition") {
        ru = encodeURIComponent("/hrs/positionchart/index?a=1");
        if (type1 == "PositionExcel") {
            ru = encodeURIComponent("/hrs/PositionChartExcel/index?a=1");
        }
        iframeOpenUrl = "/hrs/Position/history?empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate + "&ru=" + ru;
        OpenIframePopup(stdWidth, stdHeight, "Manage Position");
    }
    else if (command == "EditPositionHierarchy") {
        iframeOpenUrl = "/hrs/Positionhierarchy/history?PositionHierarchyId=" + posHierarchyId + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate;
        OpenIframePopup(stdWidth, stdHeight, "Manage Position Hierarchy");
    }
    else if (command == "AssignEmployee") {
        var openUrl = "/hrs/Assignment/GetFutureDatedAssignment?empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate;
        var response = AjaxCall(openUrl);
        if (response != null && response != '') {
            alert("An employee is assigned to this position effective from " + response + ". You cannot hire to this position now");
        } 
        else {
            ru = encodeURIComponent("/hrs/positionchart/index?a=1");
            if (type1 == "PositionExcel") {
                ru = encodeURIComponent("/hrs/PositionChartExcel/index?a=1");
            }
            url = "/hrs/assignment/create?empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate + "&ru=" + ru;
            window.location.href = url;
        }
    }
    else if (command == "ManageAssignment") {
        iframeOpenUrl = "/hrs/Assignment/history?assgmntId=" + assignmentId + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate + "&ru=" + ru;
        OpenIframePopup(stdWidth, stdHeight, "Manage Assignment");
    }
    else if (command == "RenameJob") {       
        iframeOpenUrl = "/hrs/Assignment/RenameJob?JobId=" + jobId + "&ParentJobId=" + pJobId + "&PositionId=" + posId ;
       
        OpenIframePopup(400, 400, "Rename Job");
    }
   
    else if (command == "EmployeeProfile") {
        ru = encodeURIComponent("/hrs/positionchart/index?a=1");
        if (type1 == "PositionExcel") {
            ru = encodeURIComponent("/hrs/PositionChartExcel/index?a=1");
        }
        window.location.href = "/Hrs/Assignment/PersonProfile?PersonId=" + empId + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate + "&ru=" + ru;
    }
    else if (command == "ManagePerson") {
        alert("ManagePerson Excel");
        iframeOpenUrl = "/hrs/Person/history?PersonId=" + empId + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate;
        OpenIframePopup(stdWidth, stdHeight, "Manage Person");
    }
    else if (command == "ManageUser") {
        ru = encodeURIComponent("/hrs/positionchart/index?a=1");
        if (type1 == "PositionExcel") {
            ru = encodeURIComponent("/hrs/PositionChartExcel/index?a=1");
        }
        url = "/Admin/User/Create?empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&ru=" + ru;
        if (userId != 0 && userId != 'null' && userId != null) {
            url = "/Admin/User/Correct?empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&userId=" + userId + "&ru=" + ru;
        }
        else {
            url = "/Admin/User/Create?empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&ru=" + ru;
        }
        window.location.href = url;
    }
    else if (command == "CreateNote") {
        iframeOpenUrl = "/nts/note/Create?empId=" + empId + " & posId=" + posId + " & hierarchyId=" + hierarchyId + " & orgId=" + orgId + " & date=" + asonDate;
        OpenIframePopup(stdWidth, stdHeight, "Create Note");
    }
    else if (command == "CreateTask") {
        iframeOpenUrl = "/nts/task/Create?empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate;
        OpenIframePopup(stdWidth, stdHeight, "Create Task");
    }
    else if (command == "CreateService") {
        iframeOpenUrl = "/nts/service/Create?empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate;
        OpenIframePopup(stdWidth, stdHeight, "Create Service");
    }
    else if (command == "PersonDocument") {
        ru = encodeURIComponent("/hrs/positionchart/index?a=1");
        if (type1 == "PositionExcel") {
            ru = encodeURIComponent("/hrs/PositionChartExcel/index?a=1");
        }
        iframeOpenUrl = "/nts/note/GetDocument?type=PERSON_DOCUMENT&tagtotype=Person&tagtoid=" + empId + "&ru=" + ru + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate;
        OpenIframePopup(stdWidth, stdHeight, "Person Document");
    }
    else if (command == "PositionDocument") {
        ru = encodeURIComponent("/hrs/positionchart/index?a=1");
        if (type1 == "PositionExcel") {
            ru = encodeURIComponent("/hrs/PositionChartExcel/index?a=1");
        }
        iframeOpenUrl = "/nts/note/GetDocument?type=POSITION_DOCUMENT&tagtotype=Position&tagtoid=" + posId + "&ru=" + ru + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate;
        OpenIframePopup(stdWidth, stdHeight, "Position Document");
    }
    else if (command == "JobDocument") {
        ru = encodeURIComponent("/hrs/positionchart/index?a=1");
        if (type1 == "PositionExcel") {
            ru = encodeURIComponent("/hrs/PositionChartExcel/index?a=1");
        }
        iframeOpenUrl = "/nts/note/GetDocument?type=JOB_DOCUMENT&tagtotype=Job&tagtoid=" + jobId + "&userId=" + userId + "&ru=" + ru + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate;
        OpenIframePopup(stdWidth, stdHeight, "Job Document");
    }
    else if (command == "OrganizationDocument") {
        ru = encodeURIComponent("/hrs/positionchart/index?a=1");
        if (type1 == "PositionExcel") {
            ru = encodeURIComponent("/hrs/PositionChartExcel/index?a=1");
        }
        iframeOpenUrl = "/nts/note/GetDocument?type=ORGANIZATION_DOCUMENT&tagtotype=Organization&tagtoid=" + orgId + "&ru=" + ru + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate;
        OpenIframePopup(stdWidth, stdHeight, "Department Document");
    }
    else if (command == "PerformanceDocument") {
        ru = encodeURIComponent("/hrs/positionchart/index?a=1");
        if (type1 == "PositionExcel") {
            ru = encodeURIComponent("/hrs/PositionChartExcel/index?a=1");
        }
        url = "/pms/performancedocument/manage?userId=" + userId + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate + "&ru=" + ru;
        window.location.href = url;
    }
    else if (command == "PerformanceDashboard") {
        url = "/pms/pmshome/index?userId=" + userId + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate;
        window.location.href = url;
    }
    else if (command == "SuccessionPlanning") {
        ru = encodeURIComponent("/hrs/positionchart/index?a=1");
        if (type1 == "PositionExcel") {
            ru = encodeURIComponent("/hrs/PositionChartExcel/index?a=1");
        }
        url = "/sps/successionplanning/manage?userId=" + userId + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate + "&ru=" + ru;
        window.location.href = url;
    }
    else if (command == "SuccessionDashboard") {
        url = "/sps/spshome/index?userId=" + userId + "&empId=" + empId + "&posId=" + posId + "&hierarchyId=" + hierarchyId + "&orgId=" + orgId + "&date=" + asonDate;
        window.location.href = url;
    }
    else if (command == "GoToOrgChart") {
        var canGo = hasOrg && orgReportingLine.indexOf("," + orgId + ",") > -1 && orgReportingLine.indexOf("," + orgAllowedParentId + ",") > -1;
        if (canGo) {
            window.location.href = "/hrs/organizationchart/index?orgid=" + orgId;
        }
        else {
            alert("The organization for this position is not found in Organization Chart");
            return true;
        }
    }
    else if (command == "WorkStructure") {     
        window.location.href = "/hrs/WorkStructure/index?positionId=" + posId;
    }
    else if (command == "ActivateSps") {
        AddBackendService(userId);
    }
    else if (command == "RevokeSps") {
        RevokeBackendService();
    }
   // else if (command == "GoToOrgChart") {
   //     if (hasOrg) {
   //         window.location.href = "/hrs/organizationchart/index?orgid=" + orgId;
   //     }
   //     else {
   //         alert("No Organization found for this position");
   //         return true;
   //     }
   // }
    menu.hide();
}

function OpenContextMenu(e, y, x) {
    
    menu = $("#menu");
    trgt = $(e);
    SetNodeValues(trgt);
    var showassignee = true;
    var showemp = false;
    var showuser = false;
    if (hasUser=="true") {

        showassignee = false;
    }
    if (hasEmp == true) {

        showemp = true;
    }
    if (userId != '' && userId!=null) {
        showuser = true;
    }
  //  SetObjecttiveMenu();
    SetAssignEmployeeMenu(showassignee);
    SetEmployeeMenu(showemp);
    SetUserMenu(showuser);
    var isEnabled = true;
    var result1 = SetAddMenu(isEnabled);
    var result2 = SetIfYesMenu(isEnabled);
    var result3 = SetIfNoMenu(isEnabled);
    var result4 = SetEditMenu(isEnabled);
    var result5 = SetRemoveMenu(isEnabled); 
    var result = result1 || result2 || result3 || result4 || result5;   
    if (result) {
    /* menu.css({ top: event.pageY - 375, left: event.pageX - 297, position: 'absolute' });*/
     
        menu.css({ top: event.pageY-y, left: event.pageX-x, position: 'absolute'});
        menu.show();

    }
    return false;
}
function SetSelectMenu(isEnabled) {   
    var result = isEnabled;
    if (isEnabled) {
        menu.find("#Select").show();
    }
    else {
        menu.find("#Select").hide();
    }
    return result;
}


function SetAddMenu(isEnabled) {
    var result = isEnabled;
    if (result) {
        menu.find('#Add').show();
    }
    else {
        menu.find('#Add').hide();
    }
    return result;
}
function SetWorkStructureMenu(isEnabled) {

    var result = isEnabled;
   
    if (result) {
        menu.find("#WorkStructure").show();
    }
    else {
        menu.find("#WorkStructure").hide();
    }
    return result;
}
function SetAddExistingPositionMenu(isEnabled) {
    var result = isEnabled;
    if (result) {
        menu.find("#AddExistingPosition").show();
    }
    else {
        menu.find("#AddExistingPosition").hide();
    }
    return result;
}
function SetCreateNewPositionMenu(isEnabled) {

    var result = isEnabled ;
    if (result) {
        menu.find("#CreateNewPosition").show();
    }
    else {
        menu.find("#CreateNewPosition").hide();
    }
    return result;
}
function SetEditPositionMenu(isEnabled) {

    var result = isEnabled ;
    if (result) {
        menu.find("#EditPosition").show();
    }
    else {
        menu.find("#EditPosition").hide();
    }
    return result;
}
function SetAssignEmployeeMenu(isEnabled) {
    var result = isEnabled;
    if (result) {
        menu.find("#AssignEmployee").show();
    }
    else {
        menu.find("#AssignEmployee").hide();
    }
    return result;
}

function SetObjecttiveMenu() {
   
    if (userId == loggedInUser) {
        menu.find("#ManagePerformanceObjective").hide();
    }
    else {
        menu.find("#ManagePerformanceObjective").show();
    }
   
}

function SetEmployeeMenu(isEnabled) {
    var result = isEnabled;
    if (result) {
        menu.find("#Employee").show();
        menu.find("#HRDirect").show();
        menu.find("#Worklist").show();
    }
    else {
        menu.find("#Employee").hide();
        menu.find("#HRDirect").hide();
        menu.find("#Worklist").hide();
    }
    return result;
}
function SetEditPositionHierarchyMenu(isEnabled) {
    var result = (pPosId != null && pPosId != 'null') && isEnabled ;
    if (result) {
        menu.find("#EditPositionHierarchy").show();
    }
    else {
        menu.find("#EditPositionHierarchy").hide();
    }
    return result;
}
function SetManageAssignment(isEnabled) {
    var result = isEnabled ;
    if (result) {
        menu.find("#ManageAssignment").show();
    }
    else {
        menu.find("#ManageAssignment").hide();
    }
    return result;
}

function SetRenameJob(isEnabled) {
    var result = isEnabled ;
    if (result) {
        menu.find("#RenameJob").show();
    }
    else {
        menu.find("#RenameJob").hide();
    }
    return result;
}

function SetIfYesMenu(isEnabled) {
    var result = isEnabled ;
    if (result) {
        
    }
    if (result) {
        menu.find("#IfYes").show();
    }
    else {
        menu.find("#IfYes").hide();
    }
    return result;
}
function SetHRDirectMenu(isEnabled) {
    var result = isEnabled && isAsOnDate && PositionAuthorize(2104, 271, 272, 273, 274, 275, empId, posId, reportingLine, orgId, orgReportingLine, pPosId);
    if (isEnabled) {
        menu.find("#HRDirect").show();
    }
    else {
        menu.find("#HRDirect").hide();
    }
    return result;
}
function SetEmployeeProfileMenu(isEnabled) {
    var result = isEnabled && isAsOnDate && PositionAuthorize(2104, 271, 272, 273, 274, 275, empId, posId, reportingLine, orgId, orgReportingLine, pPosId);
    if (result) {
        menu.find("#EmployeeProfile").show();
    }
    else {
        menu.find("#EmployeeProfile").hide();
    }
    return result;
}
function SetPersonMenu(isEnabled) {
    var result = isEnabled && isAsOnDate && PositionAuthorize(861, 276, 277, 278, 279, 280, empId, posId, reportingLine, orgId, orgReportingLine, pPosId);
    if (result) {
        menu.find("#ManagePerson").show();
    }
    else {
        menu.find("#ManagePerson").hide();
    }
    return result;
}
function SetUserMenu(isEnabled) {
    //var result = isEnabled && isAsOnDate && PositionAuthorize(0, 281, 282, 283, 284, 285, empId, posId, reportingLine, orgId, orgReportingLine, pPosId);
   
    if (isEnabled) {
        menu.find("#ManageUser").show();
    }
    else {
        menu.find("#ManageUser").hide();
    }
    return isEnabled;
}

function SetIfNoMenu(isEnabled) {
    var result = isEnabled;

    if (result) {
      
    }
    if (result) {
        menu.find("#IfNo").show();
    }
    else {
        menu.find("#IfNo").hide();
    }
    return result;

}

function SetSpsEnableMenu(isEnabled) {
    var result = spsCount === 0 && isEnabled && hasEmp && hasUser && PositionAuthorize(0, 2216, 2217, 2218, 2219, 2220, empId, posId, reportingLine, orgId, orgReportingLine, pPosId);
    if (result) {
        menu.find("#ActivateSps").show();
    }
    else {
        menu.find("#ActivateSps").hide();
    }
    return result;

}

function SetSpsRevokeMenu(isEnabled) {
    var result = spsCount > 0 && isEnabled && hasEmp && hasUser && PositionAuthorize(0, 2216, 2217, 2218, 2219, 2220, empId, posId, reportingLine, orgId, orgReportingLine, pPosId);
    if (result) {
        menu.find("#RevokeSps").show();
    }
    else {
        menu.find("#RevokeSps").hide();
    }
    return result;

}

function SetSpsMenu(isEnabled) {
    var result = isEnabled && hasEmp && hasUser;

    if (result) {
        var result1 = SetSuccessionPlanningMenu();
        var result2 = SetSuccessionDashboardMenu();
        result = result1 || result2;
    }
    if (result) {
        menu.find("#Sps").show();
    }
    else {
        menu.find("#Sps").hide();
    }
    return result;

}
function SetPerformanceDocumentMenu() {
    var result = hasEmp && PositionAuthorize(2073, 301, 302, 303, 304, 305, empId, posId, reportingLine, orgId, orgReportingLine, pPosId);

    if (result) {
        menu.find("#PerformanceDocument").show();
    }
    else {
        menu.find("#PerformanceDocument").hide();
    }
    return result;
}
function SetPerformanceDashboardMenu() {
    var result = hasEmp && PositionAuthorize(2071, 306, 307, 308, 309, 310, empId, posId, reportingLine, orgId, orgReportingLine, pPosId);

    if (result) {
        menu.find("#PerformanceDashboard").show();
    }
    else {
        menu.find("#PerformanceDashboard").hide();
    }
    return result;
}

function SetSuccessionPlanningMenu() {
    var result = hasEmp && hasUser && PositionAuthorize(2124, 2129, 2130, 2131, 2132, 2133, empId, posId, reportingLine, orgId, orgReportingLine, pPosId);

    if (result) {
        menu.find("#SuccessionPlanning").show();
    }
    else {
        menu.find("#SuccessionPlanning").hide();
    }
    return result;
}
function SetSuccessionDashboardMenu() {
    var result = hasEmp && hasUser && PositionAuthorize(2116, 2134, 2135, 2136, 2137, 2138, empId, posId, reportingLine, orgId, orgReportingLine, pPosId);

    if (result) {
        menu.find("#SuccessionDashboard").show();
    }
    else {
        menu.find("#SuccessionDashboard").hide();
    }
    return result;
}

function SetEditMenu(isEnabled) {
    var result = isEnabled;
    if (result) {
        
    }
    if (result) {
        menu.find("#Edit").show();
    }
    else {
        menu.find("#Edit").hide();
    }
    return result;

}
function SetNoteMenu() {
    var result = hasEmp && PositionAuthorize(2049, 286, 287, 288, 289, 290, empId, posId, reportingLine, orgId, orgReportingLine, pPosId);
    if (result) {
        menu.find("#CreateNote").show();
        menu.find("#Notes").show();
    }
    else {
        menu.find("#CreateNote").hide();
        menu.find("#Notes").hide();
    }
    return result;
}
function SetTaskMenu() {
    var result = hasEmp && hasUser && PositionAuthorize(2065, 291, 292, 293, 294, 295, empId, posId, reportingLine, orgId, orgReportingLine, pPosId);
    if (result) {
        menu.find("#CreateTask").show();
        menu.find("#Tasks").show();
    }
    else {
        menu.find("#CreateTask").hide();
        menu.find("#Tasks").hide();
    }
    return result;
}
function SetServiceMenu() {
    var result = hasEmp && hasUser && PositionAuthorize(2068, 296, 297, 298, 299, 300, empId, posId, reportingLine, orgId, orgReportingLine, pPosId);
    if (result) {
        menu.find("#CreateService").show();
        menu.find("#Services").show();
    }
    else {
        menu.find("#CreateService").hide();
        menu.find("#Services").hide();
    }
    return result;
}

function SetRemoveMenu(isEnabled) {
  
    var result = isEnabled;
    if (result) {
        
    }
    if (result) {
        menu.find("#Remove").show();
    }
    else {
        menu.find("#Remove").hide();
    }
    if (posHierarchynoteId == "null") {
        menu.find("#EditPositionHierarchy").hide();
    }
    else {
        menu.find("#EditPositionHierarchy").show();
    }
    return result;

}
function SetPersonDocumentMenu() {
    var result = hasEmp && PositionAuthorize(0, 2139, 2140, 2141, 2142, 2143, empId, posId, reportingLine, orgId, orgReportingLine, pPosId);
    if (result) {
        menu.find("#PersonDocument").show();
    }
    else {
        menu.find("#PersonDocument").hide();
    }
    return result;
}
function SetPositionDocumentMenu() {
    var result = PositionAuthorize(0, 2143, 2144, 2145, 2146, 2147, empId, posId, reportingLine, orgId, orgReportingLine, pPosId);
    if (result) {
        menu.find("#PositionDocument").show();
    }
    else {
        menu.find("#PositionDocument").hide();
    }
    return result;
}
function SetJobDocumentMenu() {
    var result = PositionAuthorize(0, 2149, 2150, 2151, 2152, 2153, empId, posId, reportingLine, orgId, orgReportingLine, pPosId);
    if (result) {
        menu.find("#JobDocument").show();
    }
    else {
        menu.find("#JobDocument").hide();
    }
    return result;
}
function SetOrganizationDocumentMenu() {
    var result = PositionAuthorize(0, 2154, 2155, 2156, 2157, 2158, empId, posId, reportingLine, orgId, orgReportingLine, pPosId);
    if (result) {
        menu.find("#OrganizationDocument").show();
    }
    else {
        menu.find("#OrganizationDocument").hide();
    }
    return result;
}

function SetGoToOrgMenu(isEnabled) {
    menu.find("#GoToOrgChart").hide();
    return false;
    var result = isEnabled && HasPermission(411);
    if (result) {
        menu.find("#GoToOrgChart").show();
    }
    else {
        menu.find("#GoToOrgChart").hide();
    }
    return result;
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

