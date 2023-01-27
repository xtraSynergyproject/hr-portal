
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
var refId;
var refType;
var ntsId;
var portalId;


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
    refId = hdn.attr("refId");
    refType = hdn.attr("reftype");
    ntsId = hdn.attr("ntsid");
    scode = hdn.attr("serStatus");
}
selectedObject = null;

function OnMenuClick(e, command, bulkRequestId) {
    SetNodeValues(e);
    //selectedObject = e;

    // var command = $(e.item).attr('id');  
    
    portalId = $('#GlobalPortalId').val();
    if (portalId == "") {
        portalId = window.parent.parent.$('#GlobalPortalId').val()
    }
    var title, url, udfs, roudfs;
    if (command == "ExpandAll") {
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
    else if (command == "OrgLevel1") {
        AddNewItem('LEVEL1', 'Add New Level 1 Department', bulkRequestId);
    }
    else if (command == "OrgLevel2") {
        AddNewItem('LEVEL2', 'Add New Level 2 Department', bulkRequestId);
    }
    else if (command == "OrgLevel3") {
        AddNewItem('LEVEL3', 'Add New Level 3 Department', bulkRequestId);
    }
    else if (command == "OrgLevel4") {
        AddNewItem('LEVEL4', 'Add New Level 4 Department', bulkRequestId);
    }
    else if (command == "Brand") {
        AddNewItem('BRAND', 'Add New Brand', bulkRequestId);
    }
    else if (command == "Market") {
        AddNewItem('MARKET', 'Add New Market', bulkRequestId);
    }
    else if (command == "Province") {
        AddNewItem('PROVINCE', 'Add New Province', bulkRequestId);
    }
    else if (command == "Department") {
        AddNewItem('DEPARTMENT', 'Add New Department', bulkRequestId);
    }
    else if (command == "CareerLevel") {
        url = '/Cms/Page?lo=Popup&pageType=Note&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=NEW_CAREERLEVEL_CREATE&portalId=' + portalId;
        title = "Add New Career Level";
        var udfparam = 'BusinessHierarchyParentId=' + orgId + '&BulkRequestId=' + bulkRequestId + '&BusinessHierarchyReferenceType=CAREER_LEVEL';
        udfs = encodeURIComponent(udfparam);
        url = url + '&udfs=' + udfs;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: title, Width: 1200, Height: 650 });
    }
    else if (command == "Job") {
        url = '/Cms/Page?lo=Popup&pageType=Note&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=NEW_JOB_CREATE&portalId=' + portalId;
        title = "Add New Job";
        var udfparam = 'BusinessHierarchyParentId=' + orgId + '&BulkRequestId=' + bulkRequestId + '&BusinessHierarchyReferenceType=JOB';
        udfs = encodeURIComponent(udfparam);
        url = url + '&udfs=' + udfs;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: title, Width: 1200, Height: 650 });
    }

    //else if (command == "newChild") {
    //    
    //    title = 'Add New Item';
    //    url = '/Cms/Page?lo=Popup&pageType=Note&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=NEW_DEPARTMENT_CREATE&portalId=' + portalId;
    //    var udfparam = 'BusinessHierarchyParentId=' + orgId + '&BulkRequestId=' + bulkRequestId;
    //    var lovCode = '';
    //    switch (refType) {
    //        case 'ROOT':
    //            lovCode = 'LEVEL1';
    //            title = "Add New Level 1 Department";
    //            break;
    //        case 'LEVEL1':
    //            lovCode = 'LEVEL2';
    //            title = "Add New Level 2 Department";
    //            break;
    //        case 'LEVEL2':
    //            lovCode = 'LEVEL3';
    //            title = "Add NewLevel 3 Department";
    //            break;
    //        case 'LEVEL3':
    //            lovCode = 'LEVEL4';
    //            title = "Add New Level 4 Department";
    //            break;
    //        case 'LEVEL4':
    //            lovCode = 'BRAND';
    //            title = "Add New Brand";
    //            break;
    //        case 'BRAND':
    //            lovCode = 'MARKET';
    //            title = "Add New Market";
    //            break;
    //        case 'MARKET':
    //            lovCode = 'PROVINCE';
    //            title = "Add New Province";
    //            break;
    //        case 'PROVINCE':
    //            lovCode = 'DEPARTMENT';
    //            title = "Add New Department";
    //            break;
    //        case 'DEPARTMENT':
    //            url = '/Cms/Page?lo=Popup&pageType=Note&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=NEW_CAREERLEVEL_CREATE&portalId=' + portalId;
    //            title = "Add New Career Level";
    //            break;
    //        case 'CAREER_LEVEL':
    //            url = '/Cms/Page?lo=Popup&pageType=Note&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=NEW_JOB_CREATE&portalId=' + portalId
    //            title = "Add New Job";
    //            break;
    //        default:
    //            break;
    //    }
    //    if (lovCode==='') {
    //        //udfs = encodeURIComponent(udfparam);
    //        //url = url + '&udfs=' + udfs;
    //        var win = GetMainWindow();
    //        win.iframeOpenUrl = url;
    //        win.OpenWindow({ Title: title, Width: 1200, Height: 650 });
    //    }
    //    else {
    //        $.ajax({
    //            url: "/Cms/LOV/GetLOVDetailsByCode?code=" + lovCode,
    //            type: 'GET',
    //            dataType: 'json',
    //            success: function (result) {
    //                
    //                if (lovCode === 'LEVEL1' || lovCode === 'LEVEL2' || lovCode === 'LEVEL3' || lovCode === 'LEVEL4') {
    //                    udfparam = udfparam + '&DepartmentLevelId=' + result.Id;
    //                    udfs = encodeURIComponent(udfparam);
    //                    roudfs = encodeURIComponent('DepartmentLevelId=true');
    //                }
    //                else {
    //                    udfparam = udfparam + '&DepartmentTypeId=' + result.Id;
    //                    udfs = encodeURIComponent(udfparam);
    //                    roudfs = encodeURIComponent('DepartmentTypeId=true');
    //                }
              
    //                url = url + '&udfs=' + udfs + '&roudfs='+roudfs;
    //                var win = GetMainWindow();
    //                win.iframeOpenUrl = url;
    //                win.OpenWindow({ Title: title, Width: 1200, Height: 650 });                      
    //            },
    //            error: function (ert) {
    //                alert('Error while fetching LOV details');
    //            }
    //        });
    //    }
    //}

    else if (command == "exisOrgLevel1") {
        AddExistingItem("&level=LEVEL1", 'Add Existing Level 1 Department', bulkRequestId);
    }
    else if (command == "exisOrgLevel2") {
        AddExistingItem("&level=LEVEL2", 'Add Existing Level 2 Department', bulkRequestId);
    }
    else if (command == "exisOrgLevel3") {
        AddExistingItem("&level=LEVEL3", 'Add Existing Level 3 Department', bulkRequestId);
    }
    else if (command == "exisOrgLevel4") {
        AddExistingItem("&level=LEVEL4", 'Add Existing Level 4 Department', bulkRequestId);
    }
    else if (command == "exisBrand") {
        AddExistingItem("&type=BRAND", 'Add Existing Brand', bulkRequestId);
    }
    else if (command == "exisMarket") {
        AddExistingItem("&type=MARKET", 'Add Existing Market', bulkRequestId);
    }
    else if (command == "exisProvince") {
        AddExistingItem("&type=PROVINCE", 'Add Existing Province', bulkRequestId);
    }
    else if (command == "exisDepartment") {
        AddExistingItem("&type=DEPARTMENT", 'Add Existing Department', bulkRequestId);
    }
    else if (command == "exisCareerLevel") {
        url = "/chr/businessHierarchy/AddExistingCareerLevel?parentHierarchyId=" + orgId + "&type=CAREER_LEVEL";
        title = "Add Existing Career Level";
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: title, Width: 500, Height: 600 });
    }
    else if (command == "exisJob") {
        url = "/chr/businessHierarchy/AddExistingJob?parentHierarchyId=" + orgId + "&type=JOB";
        title = "Add Existing Job";
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: title, Width: 500, Height: 600 });
    }
    else if (command == "exisEmployee") {
        url = "/chr/businessHierarchy/AddExistingEmployee?parentHierarchyId=" + orgId + "&type=Employee";
        title = "Add Existing Employee";
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: title, Width: 500, Height: 600 });
    }
    else if (command == "exisPos") {
        url = "/chr/businessHierarchy/AddExistingPosition?parentHierarchyId=" + orgId + "&type=Position";
        title = "Add Existing Position";
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: title, Width: 500, Height: 600 });
    }

    //else if (command == "existingChildItem") {
    //    title = 'Add Existing Item';
    //    url = "/chr/businessHierarchy/AddExistingDepartment?parentHierarchyId=" + orgId + '&BulkRequestId=' + bulkRequestId;
    //    switch (refType) {
    //        case 'ROOT':
    //            url = url + "&type=ROOT";
    //        case 'LEVEL1':
    //            url = url + "&level=LEVEL1";
    //            title = "Add Existing Level 1 Department";
    //            break;
    //        case 'LEVEL2':
    //            url = url + "&level=LEVEL2";
    //            title = "Add Existing Level 2  Department";
    //            break;
    //        case 'LEVEL3':
    //            url = url + "&level=LEVEL3";
    //            title = "Add Existing Level 3 Department";
    //            break;
    //        case 'LEVEL4':
    //            url = url + "&level=LEVEL4";
    //            title = "Add Existing Level4 Department";
    //            break;
    //        case 'BRAND':
    //            url = url + "&type=BRAND";
    //            title = "Add Existing Brand";
    //            break;
    //        case 'MARKET':
    //            url = url + "&type=MARKET";
    //            title = "Add Existing Market";
    //            break;
    //        case 'PROVINCE':
    //            url = url + "&type=PROVINCE";
    //            title = "Add Existing Province";
    //            break;
    //        case 'DEPARTMENT':
    //            url = url + "&type=DEPARTMENT";
    //            title = "Add Existing Department";
    //            break;
    //        case 'CAREER_LEVEL':
    //            url = "/chr/businessHierarchy/AddExistingCareerLevel?parentHierarchyId=" + orgId + "&type=CAREER_LEVEL";
    //            title = "Add Existing Career Level";
    //            break;
    //        case 'JOB':
    //            url = "/chr/businessHierarchy/AddExistingJob?parentHierarchyId=" + orgId + "&type=JOB";
    //            title = "Add Existing Job";
    //            break;
    //        default:
    //            break;
    //    }
    //    var win = GetMainWindow();
    //    win.iframeOpenUrl = url;
    //    win.OpenWindow({ Title: title, Width: 500, Height: 600 });
    //}

    else if (command == "reqOrg_LEVEL1" || command == "reqOrg_LEVEL2" || command == "reqOrg_LEVEL3" || command == "reqOrg_LEVEL4" || command == "req_BRAND" || command == "req_MARKET" || command == "req_PROVINCE" || command =="req_DEPARTMENT") {

        $.ajax({
            url: "/Cms/LOV/GetLOVDetailsByCode?code=" + command.split("_").pop(),
            type: 'GET',
            dataType: 'json',
            success: function (result) {

                levelid = result.Id;

                if (command == "reqOrg_LEVEL1" || command == "reqOrg_LEVEL2" || command == "reqOrg_LEVEL3" || command == "reqOrg_LEVEL4") {
                    udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&DepartmentLevelId=' + levelid + '&BusinessHierarchyReferenceType=' + command.split("_").pop());
                    roudfs = encodeURIComponent('DepartmentLevelId=true');
                }
                else if (command == "req_BRAND" || command == "req_MARKET" || command == "req_PROVINCE" || command == "req_DEPARTMENT") {
                    udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&DepartmentTypeId=' + levelid + '&BusinessHierarchyReferenceType=' + command.split("_").pop());
                    roudfs = encodeURIComponent('DepartmentTypeId=true');
                }

                url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=DepartmentRequest&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;

                var win = GetMainWindow();
                win.iframeOpenUrl = url;
                win.OpenWindow({ Title: 'Add New Child Request', Width: 1200, Height: 650 });
            },
            error: function (ert) {

            }
        });

    }
    else if (command == "reqCareerLevel") {
        udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&BusinessHierarchyReferenceType=CAREER_LEVEL');
        url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=CareerLevelRequest&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;

        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Add New Child Request', Width: 1200, Height: 650 });
    }
    else if (command == "reqJob") {
        udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&BusinessHierarchyReferenceType=JOB');
        url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=JobRequest&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;

        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Add New Child Request', Width: 1200, Height: 650 });
    }

    //else if (command == "newChildReq") {
    //    const myArray = noteId.split("$");
    //    var len = myArray.length;
    //    var depId = myArray[len - 1];
    //    var tempCode;
    //    reqType = "";
    //    const udf = ["LEVEL1", "LEVEL2", "LEVEL3", "LEVEL4", "BRAND", "MARKET", "PROVINCE", "", "DEPARTMENT"];

    //    if (lvl < 7 || lvl == 8) {

    //        $.ajax({
    //            url: "/Cms/LOV/GetLOVDetailsByCode?code=" + udf[lvl],
    //            type: 'GET',
    //            dataType: 'json',
    //            success: function (result) {

    //                levelid = result.Id;

    //                if (lvl <= 3) {
    //                    udfs = encodeURIComponent('BusinessHierarchyParentId=' + depId + '&DepartmentLevelId=' + levelid);
    //                    roudfs = encodeURIComponent('DepartmentLevelId=true');
    //                } else if ((lvl > 3 && lvl <= 6) || lvl == 8) {
    //                    udfs = encodeURIComponent('BusinessHierarchyParentId=' + depId + '&DepartmentTypeId=' + levelid);
    //                    roudfs = encodeURIComponent('DepartmentTypeId=true');
    //                }

    //                url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=DepartmentRequest&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;

    //                var win = GetMainWindow();
    //                win.iframeOpenUrl = url;
    //                win.OpenWindow({ Title: 'Add New Child Request', Width: 1200, Height: 650 });
    //            },
    //            error: function (ert) {

    //            }
    //        });
    //    }
    //    else if (lvl >= 7 && lvl != 8 && lvl != 10) {
    //        if (lvl == 7) {
    //            tempCode = "CareerLevelRequest";
    //        }
    //        else if (lvl == 9) {
    //            tempCode = "JobRequest";
    //        }

    //        udfs = encodeURIComponent('BusinessHierarchyParentId=' + depId);
    //        url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=' + tempCode + '&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;

    //        var win = GetMainWindow();
    //        win.iframeOpenUrl = url;
    //        win.OpenWindow({ Title: 'Add New Child Request', Width: 1200, Height: 650 });
    //    }
    //    if (lvl == 10) {
    //        $.ajax({
    //            url: "/CHR/BusinessHierarchy/GetHybridHierarchyDetails?hybridHierarchyId=" + depId,
    //            type: 'GET',
    //            dataType: 'json',
    //            success: function (result) {

    //                udfs = encodeURIComponent('BusinessHierarchyParentId=' + depId + '&JobId=' + result.data);
    //                roudfs = encodeURIComponent('JobId=true');

    //                url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=NEW_PERSON_REQ&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;

    //                var win = GetMainWindow();
    //                win.iframeOpenUrl = url;
    //                win.OpenWindow({ Title: 'Add New Child Request', Width: 1200, Height: 650 });
    //            },
    //            error: function (ert) {
    //            }
    //        });
    //    }
    //}
    
    else if (command == "newPos") {

        const myArray = noteId.split("$");
        var len = myArray.length;
        var depId = myArray[len - 1];

        udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&BulkRequestId=' + bulkRequestId + '&BusinessHierarchyReferenceType=POSITION');
        url = '/Cms/Page?lo=Popup&pageType=Note&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=NEW_POSITION_CREATE&portalId=' + portalId + '&udfs=' + udfs;

        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Add New Position', Width: 1200, Height: 650 });
    }
    else if (command == "newPosReq") {

        const myArray = noteId.split("$");
        var len = myArray.length;
        var depId = myArray[len - 1];

        udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&BusinessHierarchyReferenceType=POSITION');

        url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=NEW_POSITION_REQ&portalId=' + portalId + '&udfs=' + udfs;

        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Add New Position Request', Width: 1200, Height: 650 });
    }
    else if (command == "addEmployee") {
        
        $.ajax({
            url: "/CHR/HRCore/GetPositionDetails?Id=" + refId,
            type: 'GET',
            dataType: 'json',
            success: function (result) {
                
                var data = result.posdata;
                if (refType == 'POSITION') {
                    udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&PositionId=' + refId + '&DepartmentId=' + data.DepartmentId + '&JobId=' + data.JobId + '&BulkRequestId=' + bulkRequestId + '&BusinessHierarchyReferenceType=EMPLOYEE');
                    roudfs = encodeURIComponent('DepartmentId=true&JobId=true&PositionId=true');
                } else if (refType == 'JOB') {
                    udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&JobId=' + refId + '&BulkRequestId=' + bulkRequestId + '&BusinessHierarchyReferenceType=EMPLOYEE');
                    roudfs = encodeURIComponent('JobId=true');
                } else {
                    udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&DepartmentId=' + refId + '&BulkRequestId=' + bulkRequestId + '&BusinessHierarchyReferenceType=EMPLOYEE');
                    roudfs = encodeURIComponent('DepartmentId=true');
                }                

                url = '/Cms/Page?lo=Popup&pageType=Note&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=EMPLOYEE_CREATE&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;

                var win = GetMainWindow();
                win.iframeOpenUrl = url;
                win.OpenWindow({ Title: 'Add New Employee', Width: 1200, Height: 650 });

            },
            error: function (ert) {

            }
        });

    }
    else if (command == "addEmployeeRequest") {
 
        $.ajax({
            url: "/CHR/HRCore/GetPositionDetails?Id=" + refId,
            type: 'GET',
            dataType: 'json',
            success: function (result) {
                
                var data = result.posdata;

                if (refType == 'POSITION') {
                    udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&PositionId=' + refId + '&DepartmentId=' + data.DepartmentId + '&JobId=' + data.JobId + '&BusinessHierarchyReferenceType=EMPLOYEE');
                    roudfs = encodeURIComponent('DepartmentId=true&JobId=true&PositionId=true');
                }
                else if (refType == 'JOB') {
                    udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&JobId=' + refId + '&BulkRequestId=' + bulkRequestId + '&BusinessHierarchyReferenceType=EMPLOYEE');
                    roudfs = encodeURIComponent('JobId=true');
                } else {
                    udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&DepartmentId=' + refId + '&BulkRequestId=' + bulkRequestId + '&BusinessHierarchyReferenceType=EMPLOYEE');
                    roudfs = encodeURIComponent('DepartmentId=true');
                }   
                
                url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=EMPLOYEE_CREATE_REQUEST&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;

                var win = GetMainWindow();
                win.iframeOpenUrl = url;
                win.OpenWindow({ Title: 'Add New Employee Request', Width: 1200, Height: 650 });

            },
            error: function (ert) {

            }
        });
      //  udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId);
    }
   
    else if (command == "changeJob") {
        $.ajax({
            url: "/CHR/HRCore/GetAssignmentDetails?personId=" + refId,
            type: 'GET',
            dataType: 'json',
            success: function (result) {

                if (result.success) {

                    data = result.assdata;

                    udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&PersonId=' + refId + '&ExistingJobId=' + data.JobId + '&BulkRequestId=' + bulkRequestId);
                    var url = '/Cms/Page?lo=Popup&pageType=Note&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=Change_Job&portalId=' + portalId + '&udfs=' + udfs;//+ '&roudfs=' + roudfs;
                    var win = GetMainWindow();
                    win.iframeOpenUrl = url;
                    win.OpenWindow({ Title: 'Change Job', Width: 1200, Height: 650 });
                }
                else {
                    alert(result.error);
                }
            },
            error: function (ert) {

            }
        });
    }
    else if (command == "changeJobRequest") {
        $.ajax({
            url: "/CHR/HRCore/GetAssignmentDetails?personId=" + refId,
            type: 'GET',
            dataType: 'json',
            success: function (result) {

                if (result.success) {

                    data = result.assdata;

                    udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&PersonId=' + refId + '&ExistingJobId=' + data.JobId + '&BulkRequestId=' + bulkRequestId);
                    var url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=Change_Job_Request&portalId=' + portalId + '&udfs=' + udfs;//+ '&roudfs=' + roudfs;
                    var win = GetMainWindow();
                    win.iframeOpenUrl = url;
                    win.OpenWindow({ Title: 'Change Job Request', Width: 1200, Height: 650 });
                }
                else {
                    alert(result.error);
                }
            },
            error: function (ert) {

            }
        });
    }
    else if (command == "renameJob") {
        var udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&CurrentJobId=' + refId + '&BulkRequestId=' + bulkRequestId);
        var roudfs = encodeURIComponent('CurrentJobId=true');
        var url = '/Cms/Page?lo=Popup&pageType=Note&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=RENAME_JOB&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Rename Job', Width: 1200, Height: 650 });
    }
    else if (command == "renameJobRequest") {
        var udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&CurrentJobId=' + refId);
        var roudfs = encodeURIComponent('CurrentJobId=true');
        var url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=RENAME_JOB_REQ&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;

        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Rename Job Request', Width: 1200, Height: 650 });
    }

    else if (command == "changeDepartment") {

        $.ajax({
            url: "/CHR/HRCore/GetAssignmentDetails?personId=" + refId,
            type: 'GET',
            dataType: 'json',
            success: function (result) {

                if (result.success) {

                    data = result.assdata;

                    udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&PersonId=' + refId + '&ExistingDepartmentId=' + data.DepartmentId + '&BulkRequestId=' + bulkRequestId);
                    var url = '/Cms/Page?lo=Popup&pageType=Note&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=Change_Department&portalId=' + portalId + '&udfs=' + udfs;//+ '&roudfs=' + roudfs;
                    var win = GetMainWindow();
                    win.iframeOpenUrl = url;
                    win.OpenWindow({ Title: 'Change Department', Width: 1200, Height: 650 });
                }
                else {
                    alert(result.error);
                }
            },
            error: function (ert) {

            }
        });

    }
    else if (command == "changeDepartmentRequest") {

        $.ajax({
            url: "/CHR/HRCore/GetAssignmentDetails?personId=" + refId,
            type: 'GET',
            dataType: 'json',
            success: function (result) {

                if (result.success) {

                    data = result.assdata;

                    udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&PersonId=' + refId + '&ExistingDepartmentId=' + data.DepartmentId);
                    var url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=Change_Department_Request&portalId=' + portalId + '&udfs=' + udfs;//+ '&roudfs=' + roudfs;
                    var win = GetMainWindow();
                    win.iframeOpenUrl = url;
                    win.OpenWindow({ Title: 'Change Department Request', Width: 1200, Height: 650 });
                }
                else {
                    alert(result.error);
                }
            },
            error: function (ert) {

            }
        });

    }
    else if (command == "renameDept") {
      
        var udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&CurrentDepartmentId=' + refId + '&BulkRequestId=' + bulkRequestId);
        var roudfs = encodeURIComponent('CurrentDepartmentId=true');

        var url = '/Cms/Page?lo=Popup&pageType=Note&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=RENAME_DEPARTMENT&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;

        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Rename Department', Width: 1200, Height: 650 });
    }
    else if (command == "renameDeptRequest") {

        var udfs = encodeURIComponent('BusinessHierarchyParentId=' + orgId + '&CurrentDepartmentId=' + refId);
        var roudfs = encodeURIComponent('CurrentDepartmentId=true');

        var url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=RENAME_DEPARTMENT_REQ&portalId=' + portalId + '&udfs=' + udfs + '&roudfs=' + roudfs;

        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Rename Department Request', Width: 1200, Height: 650 });
    }
  
    else if (command == "changeLM") {
        reqType = "";
        $.ajax({
            url: "/CHR/HRCore/GetPersonDetails?personId=" + refId,
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
        reqType = "";
        $.ajax({
            url: "/CHR/HRCore/GetAssignmentDetails?personId=" + refId,
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
    else if (command == "removeFromHierarchy") {
        var flag = confirm('Do you really want to delete?');
        var url = "/CHR/BusinessHierarchy/DeleteFromHierarchy?hybridHierarchyId=" + orgId;
        if (flag) {
            $.ajax({
                url: url,
                type: 'GET',
                dataType: 'json',
                success: function (result) {
                    if (result.success) {
                        alert("Removed Successfully");

                        //OnAfterCreate();
                        var rootli = $('.root-li');
                        var parent = trgt[0].parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.children[0].children[1];
                        var pop = trgt[0].parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.children[0].children[1];

                        Collapse(pop, rootli);
                        Expand(pop, rootli);
                        Expand(parent, rootli);
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
    else if (command == "moveTo") {
        
        url = "/chr/businessHierarchy/MoveToOtherParent?curParentId=" + parentId + "&refType=" + refType + "&curNodeId=" + orgId;
        title = "Move To";
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: title, Width: 550, Height: 500 });
    }
    else if (command == "viewHieDetails") {
        
        url = "/chr/businessHierarchy/BusinessHierarchyDetails?businessHierarchyItemId=" + orgId;
        title = "View Hierarchy Details";
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: title, Width: 1200, Height: 650 });
    }

    else if (command == "openContextMenu") {
        var templateCode = "";
        // reqType = "RenameJob";
        // var udfs = encodeURIComponent('BusinessHierarchyParentId=' + noteId + '&CurrentJobId=' + refId);
        // var roudfs = encodeURIComponent('CurrentJobId=true');
        if (refType == "DEPARTMENT_SERVICE") {
            templateCode = "DepartmentRequest";
        }
        else if (refType == "JOB_SERVICE") {
            templateCode = "JobRequest";
        }
        else if (refType == "CAREER_LEVEL_SERVICE") {
            templateCode = "CareerLevelRequest";
        }
        else if (refType == "PERSON_SERVICE") {
            templateCode = "NEW_PERSON_REQ";
        }
        else if (refType == "POSITION_SERVICE") {
            templateCode = "NEW_POSITION_REQ";
        }
        var url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Edit&dataAction=Edit&portalId=' + portalId + '&recordId=' + ntsId + '&templateCodes=' + templateCode;

        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Open Request', Width: 1200, Height: 650 });
    }
    else if (command == "manageAOR") {
        
        //      var portalId = $('#GlobalPortalId').val();
        //  window.location.href = "/CHR/BusinessHierarchy/BusinessHierarchyAOR?businessHierarchyId=" + ntsId;
        //   window.location.href = "/Cms/Page?pageId=8ac3d720-29b4-498e-9a7f-1a94632bbcee&portalId=8edc86b3-9934-46e3-95de-d76c816404b4&pageType=Custom";
        //var portalId = $('#GlobalPortalId').val();

       // var customurl = encodeURIComponent('businessHierarchyId=' + orgId);
        //var url = '/Cms/Page?lo=Popup&pageName=BusinessHierarchyAOR&portalId=' + portalId + '&customUrl=' + customurl;
        var url = '/chr/BusinessHierarchy/BusinessHierarchyAOR?businessHierarchyId=' + orgId + '&lo=Popup';
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Manage AOR', Width: 1200, Height: 650 });

    }
    else if (command == "managePermission") {
        //var portalId = $('#GlobalPortalId').val();
        //  var customurl = encodeURIComponent('businessHierarchyId=' + orgId);
        var url = '/chr/BusinessHierarchy/BHPermissionIndex?lo=Popup'; //+ '&customUrl=' + customurl;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Permissions', Width: 1200, Height: 650 });
    }

    else if (command == "newPer") {
        udfs = encodeURIComponent('BusinessHierarchyParentId=' + parentId + '&BulkRequestId=' + bulkRequestId);
        url = '/Cms/Page?lo=Popup&pageType=Note&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=NEW_PERSON_CREATE&portalId=' + portalId + '&udfs=' + udfs;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Add New Employee', Width: 1200, Height: 600 });
    }
    else if (command == "newPerReq") {
        title = 'New Employee Request';
        url = '/Cms/Page?lo=Popup&pageType=Service&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=NEW_PERSON_REQ&portalId=' + portalId;
        udfs = encodeURIComponent('BusinessHierarchyParentId=' + parentId);;
        switch (refType) {
            case 'Root':
                break;
            case 'OrgLevel1':
            case 'OrgLevel2':
            case 'OrgLevel3':
            case 'OrgLevel4':
            case 'Brand':
            case 'Market':
            case 'Province':
            case 'Department':
                udfs = encodeURIComponent('BusinessHierarchyParentId=' + parentId + '&DepartmentId=' + refId);
                roudfs = encodeURIComponent('DepartmentId=true');
                break;
            case 'Job':
                udfs = encodeURIComponent('BusinessHierarchyParentId=' + parentId + '&JobId=' + refId);
                roudfs = encodeURIComponent('JobId=true');
            default:
        }
        url = url + '&udfs=' + udfs + '&roudfs=' + roudfs;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: title, Width: 1200, Height: 650 });
    }
    else if (command == "editPerson") {
        reqType = "";
        $.ajax({
            url: "/CHR/HRCore/GetPersonDetails?personId=" + refId,
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
    menu.hide();
}

function AddNewItem(lovCode, title, bulkRequestId) {
    
    url = '/Cms/Page?lo=Popup&pageType=Note&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=NEW_DEPARTMENT_CREATE&portalId=' + portalId;
    var udfparam = 'BusinessHierarchyParentId=' + orgId + '&BulkRequestId=' + bulkRequestId;

    if (lovCode === '') {
        //udfs = encodeURIComponent(udfparam);
        //url = url + '&udfs=' + udfs;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: title, Width: 1200, Height: 650 });
    }
    else {
        $.ajax({
            url: "/Cms/LOV/GetLOVDetailsByCode?code=" + lovCode,
            type: 'GET',
            dataType: 'json',
            success: function (result) {
                
                if (lovCode === 'LEVEL1' || lovCode === 'LEVEL2' || lovCode === 'LEVEL3' || lovCode === 'LEVEL4') {
                    udfparam = udfparam + '&DepartmentLevelId=' + result.Id + '&BusinessHierarchyReferenceType=' + lovCode;
                    udfs = encodeURIComponent(udfparam);
                    roudfs = encodeURIComponent('DepartmentLevelId=true');
                }
                else {
                    udfparam = udfparam + '&DepartmentTypeId=' + result.Id + '&BusinessHierarchyReferenceType=' + lovCode;
                    udfs = encodeURIComponent(udfparam);
                    roudfs = encodeURIComponent('DepartmentTypeId=true');
                }

                url = url + '&udfs=' + udfs + '&roudfs=' + roudfs;
                var win = GetMainWindow();
                win.iframeOpenUrl = url;
                win.OpenWindow({ Title: title, Width: 1200, Height: 650 });

            },
            error: function (ert) {
                alert('Error while fetching LOV details');
            }
        });
    }
}

function AddExistingItem(level,title, bulkRequestId) {
    
    url = "/chr/businessHierarchy/AddExistingDepartment?parentHierarchyId=" + orgId + '&BulkRequestId=' + bulkRequestId + level;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 500, Height: 600 });

}


function HybridContextMenu(e, y, x) {

    menu = $("#hybridHierarchychartmenu");
    trgt = $(e);
    SetNodeValues(trgt);
    var isEnabled = true;

    var lvltype = trgt[0].getAttribute("lvl");

    selectedObject = trgt[0].parentNode.parentNode.parentNode.parentNode.children[1];
    if (selectedObject == undefined) {
        let s = document.createElement("span");
        s.className = "hr-org-collapse";
        s.className = "hr-ec";
        s.onclick = function () {/* OnExpandCollapseClick(s)*/ alert("jhfgdh") };;

        let c = document.createElement("span");
        c.className = "hr-dhc";

        trgt[0].parentNode.parentNode.parentNode.parentNode.appendChild(s);
        trgt[0].parentNode.parentNode.parentNode.parentNode.appendChild(c);
        selectedObject = trgt[0].parentNode.parentNode.parentNode.parentNode.children[1];
    }
    //var result1 = SetOrganizationMenu(isEnabled);

    if (refType == "null" && refType != "Employee") {
        menu.find("#openContextMenu").hide();
        menu.find("#newChild").show();
        menu.find("#existingChildItem").show();
        menu.find("#newChildReq").show();
        menu.find("#removeFromHierarchy").hide();
        menu.find("#Expand").show();
        menu.find("#CollapseAll").show();
        menu.find("#editPerson").hide();
        menu.find("#changeLM").hide();
        menu.find("#changeAssignment").hide();
        menu.find("#renameJob").hide();
        menu.find("#renameJobRequest").hide();
        menu.find("#renameDept").hide();
        menu.find("#renameDeptRequest").hide();
        menu.find("#changeJobRequest").hide();
        menu.find("#changeDepartmentRequest").hide();
        menu.find("#changeJob").hide();
        menu.find("#changeDepartment").hide();
        menu.find("#newPer").hide();
        menu.find("#newPerReq").hide();
        menu.find("#newPos").hide();
        menu.find("#newPosReq").hide();
        menu.find("#exisPos").hide();
        menu.find("#addEmployee").hide();
    }
    else if ((refType == "Department" || refType == "OrgLevel1" || refType == "OrgLevel2" || refType == "OrgLevel3" || refType == "OrgLevel4" || refType == "Brand" || refType == "Market" || refType == "Province") && scode == "SERVICE_STATUS_COMPLETE" && refType != "Employee" && refType != "Position") {
        menu.find("#openContextMenu").hide();
        menu.find("#newChild").show();
        menu.find("#existingChildItem").show();
        menu.find("#newChildReq").show();
        menu.find("#removeFromHierarchy").show();
        menu.find("#Expand").show();
        menu.find("#CollapseAll").show();
        menu.find("#editPerson").hide();
        menu.find("#changeLM").hide();
        menu.find("#changeAssignment").hide();
        menu.find("#renameJob").hide();
        menu.find("#renameJobRequest").hide();
        menu.find("#renameDept").show();
        menu.find("#renameDeptRequest").show();
        menu.find("#changeJob").hide();
        menu.find("#changeDepartment").hide();
        menu.find("#changeJobRequest").hide();
        menu.find("#changeDepartmentRequest").hide();
        menu.find("#newPer").show();
        menu.find("#newPerReq").show();
        menu.find("#newPos").show();
        menu.find("#newPosReq").show();
        menu.find("#exisPos").show();
        menu.find("#addEmployee").hide();
        menu.find("#managePermission").show();
        menu.find("#manageAOR").show();
    }
    else if (refType == "Job" && scode == "SERVICE_STATUS_COMPLETE" && refType != "Employee" && refType != "Position") {
        menu.find("#openContextMenu").hide();
        menu.find("#newChild").hide();
        menu.find("#existingChildItem").show();
        menu.find("#newChildReq").hide();
        menu.find("#removeFromHierarchy").show();
        menu.find("#Expand").show();
        menu.find("#CollapseAll").show();
        menu.find("#editPerson").hide();
        menu.find("#changeLM").hide();
        menu.find("#changeAssignment").hide();
        menu.find("#renameJob").show();
        menu.find("#renameJobRequest").show();
        menu.find("#renameDept").hide();
        menu.find("#renameDeptRequest").hide();
        menu.find("#changeJob").hide();
        menu.find("#changeDepartment").hide();
        menu.find("#changeJobRequest").hide();
        menu.find("#changeDepartmentRequest").hide();
        menu.find("#newPer").show();
        menu.find("#newPerReq").show();
        menu.find("#newPos").show();
        menu.find("#newPosReq").show();
        menu.find("#exisPos").show();
        menu.find("#addEmployee").hide();
        menu.find("#managePermission").hide();
        menu.find("#manageAOR").hide();
    }
    else if (refType == "Employee" && refType != "PERSON_SERVICE" && refType != "Position") {
        menu.find("#openContextMenu").hide();
        menu.find("#newChild").hide();
        menu.find("#existingChildItem").hide();
        menu.find("#newChildReq").hide();
        menu.find("#removeFromHierarchy").show();
        menu.find("#Expand").hide();
        menu.find("#CollapseAll").hide();
        menu.find("#editPerson").show();
        menu.find("#changeLM").show();
        menu.find("#changeJob").show();
        menu.find("#changeDepartment").show();
        menu.find("#changeJobRequest").show();
        menu.find("#changeDepartmentRequest").show();
        menu.find("#changeAssignment").show();
        menu.find("#renameJob").hide();
        menu.find("#renameJobRequest").hide();
        menu.find("#renameDept").hide();
        menu.find("#renameDeptRequest").hide();
        menu.find("#newPer").hide();
        menu.find("#newPerReq").hide();
        menu.find("#newPos").hide();
        menu.find("#newPosReq").hide();
        menu.find("#exisPos").hide();
        menu.find("#addEmployee").hide();
        menu.find("#managePermission").hide();
        menu.find("#manageAOR").hide();
    }
    else if (refType == "DEPARTMENT_SERVICE" || refType == "JOB_SERVICE" || refType == "CAREER_LEVEL_SERVICE" || refType == "PERSON_SERVICE" || refType == "POSITION_SERVICE" && refType != "Employee" && refType != "Position") {
        menu.find("#openContextMenu").show();
        menu.find("#newChild").hide();
        menu.find("#existingChildItem").hide();
        menu.find("#newChildReq").hide();
        menu.find("#removeFromHierarchy").hide();
        menu.find("#Expand").hide();
        menu.find("#CollapseAll").hide();
        menu.find("#editPerson").hide();
        menu.find("#changeLM").hide();
        menu.find("#changeJob").hide();
        menu.find("#changeDepartment").hide();
        menu.find("#changeJobRequest").hide();
        menu.find("#changeDepartmentRequest").hide();
        menu.find("#changeAssignment").hide();
        menu.find("#renameJob").hide();
        menu.find("#renameJobRequest").hide();
        menu.find("#renameDept").hide();
        menu.find("#renameDeptRequest").hide();
        menu.find("#newPer").hide();
        menu.find("#newPerReq").hide();
        menu.find("#newPer").hide();
        menu.find("#newPerReq").hide();
        menu.find("#newPos").hide();
        menu.find("#newPosReq").hide();
        menu.find("#exisPos").hide();
        menu.find("#addEmployee").hide();
        menu.find("#managePermission").hide();
        menu.find("#manageAOR").hide();
    }
    else if (refType == "Position") {
        menu.find("#openContextMenu").hide();
        menu.find("#newChild").hide();
        menu.find("#existingChildItem").hide();
        menu.find("#newChildReq").hide();
        menu.find("#removeFromHierarchy").show();
        menu.find("#Expand").show();
        menu.find("#CollapseAll").show();
        menu.find("#editPerson").hide();
        menu.find("#changeLM").hide();
        menu.find("#changeJob").hide();
        menu.find("#changeDepartment").hide();
        menu.find("#changeJobRequest").hide();
        menu.find("#changeDepartmentRequest").hide();
        menu.find("#changeAssignment").hide();
        menu.find("#renameJob").hide();
        menu.find("#renameJobRequest").hide();
        menu.find("#renameDept").hide();
        menu.find("#renameDeptRequest").hide();
        menu.find("#newPer").hide();
        menu.find("#newPerReq").hide();
        menu.find("#newPer").hide();
        menu.find("#newPerReq").hide();
        menu.find("#newPos").hide();
        menu.find("#newPosReq").hide();
        menu.find("#exisPos").hide();
        menu.find("#addEmployee").show();
        menu.find("#managePermission").hide();
        menu.find("#manageAOR").hide();
    }
    else {
        menu.find("#openContextMenu").hide();
        menu.find("#newChild").show();
        menu.find("#existingChildItem").show();
        menu.find("#newChildReq").show();
        menu.find("#removeFromHierarchy").show();
        menu.find("#Expand").show();
        menu.find("#CollapseAll").show();
        menu.find("#editPerson").hide();
        menu.find("#changeLM").hide();
        menu.find("#changeAssignment").hide();
        menu.find("#renameJob").hide();
        menu.find("#renameJobRequest").hide();
        menu.find("#renameDept").hide();
        menu.find("#renameDeptRequest").hide();
        menu.find("#changeJobRequest").hide();
        menu.find("#changeDepartmentRequest").hide();
        menu.find("#changeJob").hide();
        menu.find("#changeDepartment").hide();
        menu.find("#newPer").show();
        menu.find("#newPerReq").show();
        menu.find("#newPos").show();
        menu.find("#newPosReq").show();
        menu.find("#exisPos").show();
        menu.find("#addEmployee").hide();
        menu.find("#managePermission").hide();
        menu.find("#manageAOR").hide();
    }

    var result = true;
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
    else {
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







