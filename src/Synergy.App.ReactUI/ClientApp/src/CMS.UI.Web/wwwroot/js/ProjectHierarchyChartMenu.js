
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
}
function OnMenuClick(e) {
    SetNodeValues(trgt);
    var command = $(e.item).attr('id');
    
    var type = trgt[0].getAttribute("type");
    var WorkflowServiceId = trgt[0].getAttribute("id").split("hr-org-menu-")[1];
    var parentId = trgt[0].getAttribute("parentId");
    var Count = trgt[0].getAttribute("count");
    var Sequence = trgt[0].getAttribute("SequenceOrder");
    var WorkspaceId = trgt[0].getAttribute("parentId");
    var id = trgt[0].getAttribute("id").split("hr-org-menu-")[1];
    var NodeId = id;

    switch (command) {
        case 'createProject':
            var portalId = $('#GlobalPortalId').val();
            var url = '/Cms/Page?lo=Popup&cbm=OnAfterCreate&source=Create&dataAction=Create&pageName=Project&portalId=' + portalId;
            var win = GetMainWindow();
            win.iframeOpenUrl = url;
            win.OpenWindow({ Title: 'Create Project', Width: 1200, Height: 600 });
            return false;
            break;
        case 'edit':
            Edit(id);
            break;
        case 'manage':
            ManageBook(id);
            break;
        case 'view':
            View(id);
            break;
        case 'createStage':
            createStage(id);
            break;
        case 'viewStage':
            View(id);
            break;
        case 'createTask':
            createTask(id);
            break;
        case 'viewTask':
            viewTask(id);
            break;
        case 'createTaskForUser':
            createTaskForUser(id);
            break;
        default:
    }
    menu.hide();
}

function createTaskForUser(id) {
    var ouId = id.split("$")[0];
    var serviceId = id.split("$")[1];
    var prms = encodeURIComponent('&assignedToUserId=' + ouId + '&parentServiceId=' + serviceId);
    var portalId = $('#GlobalPortalId').val();
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterCreate&source=Create&dataAction=Create&pageName=ProjectTask&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: 'Create Task', Width: 1200, Height: 600 });
    return false;
}

function viewTask(id) {
    var portalId = $('#GlobalPortalId').val();
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterCreate&source=Versioning&dataAction=Edit&pageName=ProjectTask&portalId=' + portalId + '&recordId=' + id;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: 'View Task', Width: 1200, Height: 600 });
    return false;
}

function createTask(id) {
    
   // var portalId = $('#GlobalPortalId').val();
    // var prms = encodeURIComponent('parentServiceId=' + id);
    //var prms = encodeURIComponent('subject=' + subject + '&ownerUserId=' + fromid + '&assignedToUserId=' + '@_userContext.UserId');
    //var udfs = encodeURIComponent('MessageId=' + messageid);
    //var url = '/Cms/Page?lo=Popup&cbm=OnAfterCreate&source=Create&dataAction=Create&templateCodes=EMAIL_TASK&portalId=' + portalId + '&prms=' + prms + '&udfs=' + udfs;
    var portalId = $('#GlobalPortalId').val();
    var prms = encodeURIComponent('parentServiceId=' + id);
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterCreate&source=Create&dataAction=Create&pageName=ProjectTask&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: 'Create Task', Width: 1200, Height: 600 });
    return false;
}

function createStage(id) {
    var prms = encodeURIComponent('parentServiceId=' + id);
    var portalId = $('#GlobalPortalId').val();
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterCreate&source=Create&dataAction=Create&pageName=ProjectStage&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: 'Create Stage', Width: 1200, Height: 600 });
    return false;
}

function Edit(id) {
    var portalId = $('#GlobalPortalId').val();
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterCreate&source=Edit&dataAction=Edit&pageName=Project&portalId=' + portalId + '&recordId=' + id;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: 'Edit Project', Width: 1200, Height: 600 });
    return false;
}

function ManageBook(id) {
    var portalId = $('#GlobalPortalId').val();
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterCreate&source=View&dataAction=View&pageName=Project&portalId=' + portalId + '&recordId=' + id;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: 'Manage Book', Width: 1200, Height: 600 });
    return false;
}

function View(id) {
    var url = "/pjm/project/projectdashboard?projectId=" + id;
    LoadPartailView(url, $('#cms-content'));
}


function OpenContextMenu(e, y, x) {
    
    menu = $("#menu");
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
        //return {
        if (permissions != null && permissions.includes("CREATE_PROJECT")) {
            menu.find("#createProject").show();
        } else {
            menu.find("#createProject").hide();
        }
        menu.find("#edit").hide();
        menu.find("#manage").hide();
        menu.find("#view").hide();
        menu.find("#viewStage").hide();
        menu.find("#createTask").hide();
        menu.find("#viewTask").hide();
        menu.find("#createTaskForUser").hide();
        menu.find("#createStage").hide();


    }
    else if (type == "Project")
    {
        menu.find("#createProject").hide();

        menu.find("#createStage").show();
        menu.find("#edit").show();
        menu.find("#manage").show();
        menu.find("#view").show();

        menu.find("#viewStage").hide();
        menu.find("#createTask").show();
        menu.find("#viewTask").hide();
        menu.find("#createTaskForUser").hide();



    } else if (type == "Stage") {
        menu.find("#viewStage").show();
        menu.find("#createTask").show();

        menu.find("#createProject").hide();
        menu.find("#createStage").hide();
        menu.find("#edit").hide();
        menu.find("#manage").hide();
        menu.find("#view").hide();
        menu.find("#viewTask").hide();
        menu.find("#createTaskForUser").hide();


    } else if (type == "TaskUser") {
        menu.find("#createTaskForUser").show();
        menu.find("#viewTask").hide();

        menu.find("#createProject").hide();
        menu.find("#edit").hide();
        menu.find("#manage").hide();
        menu.find("#view").hide();
        menu.find("#createStage").hide();

        menu.find("#viewStage").hide();
        menu.find("#createTask").hide();
    }
    else if (type == "Task") {
        menu.find("#viewTask").show();
        menu.find("#createTaskForUser").hide();

        menu.find("#createProject").hide();
        menu.find("#edit").hide();
        menu.find("#manage").hide();
        menu.find("#view").hide();
        menu.find("#createStage").hide();

        menu.find("#viewStage").hide();
        menu.find("#createTask").hide();
    }
    else {
        menu.find("#createTaskForUser").hide();

        menu.find("#createProject").hide();
        menu.find("#edit").hide();
        menu.find("#manage").hide();
        menu.find("#view").hide();
        menu.find("#createStage").hide();

        menu.find("#viewStage").hide();
        menu.find("#createTask").hide();

        menu.find("#viewTask").hide();
    }
    var result = true;
    if (result) {
        menu.css({ top: event.pageY - y, left: event.pageX - x, position: 'absolute' });
        menu.show();
    }
    return false;
}