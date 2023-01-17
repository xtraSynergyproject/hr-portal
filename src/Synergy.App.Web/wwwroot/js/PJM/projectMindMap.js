var selectedProjectId = "";
var selectedTemplateType = "";
var lastClickedElementView;
var currentSelectedElementView;
var currentParentNode = "";
var isLineManager = false;

$(".toggle-nav").trigger("click");

var portalId = $('#GlobalPortalId').val();
var projectId = $('#ProjectId').val();
$(document).ready(function () {
    var homogeneous = new kendo.data.HierarchicalDataSource({
        transport: {
            read: {
                url: "/pjm/projectTask/GetBannerProjectsList",
                dataType: "json"
            }
        },
        schema: {
            model: {
                id: "Id",
                hasChildren: "HasChildren"
            }
        }
    });

    $("#projects").kendoDropDownTree({
        dataTextField: "Name",
        dataValueField: "Id",
        optionLabel: "Select Project",
        filter: "contains",
        value: projectId,
        change: onProjectSelect,
        dataSource: homogeneous
    });
    //$("#projects").kendoDropDownList({
    //    dataTextField: "Name",
    //    dataValueField: "Id",
    //    optionLabel: "Select Project",
    //    filter: "contains",
    //    value: projectId,
    //    change: onProjectSelect,
    //    dataSource:
    //    {
    //        transport:
    //        {
    //            read:
    //            {
    //                url: "/PJM/ProjectTask/GetProjectsList",
    //            }
    //        }
    //    }
    //});

    if (projectId != null) {
        onProjectSelect();
    }




    //var categories = $("#categories").kendoDropDownList({
    //    optionLabel: "Select category...",
    //    dataTextField: "CategoryName",
    //    dataValueField: "CategoryID",
    //    dataSource: {
    //        type: "odata",
    //        serverFiltering: true,
    //        transport: {
    //            read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Categories"
    //        }
    //    }
    //}).data("kendoDropDownList");

    //var products = $("#products").kendoDropDownList({
    //    autoBind: false,
    //    cascadeFrom: "categories",
    //    optionLabel: "Select product...",
    //    dataTextField: "ProductName",
    //    dataValueField: "ProductID",
    //    dataSource: {
    //        type: "odata",
    //        serverFiltering: true,
    //        transport: {
    //            read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
    //        }
    //    }
    //}).data("kendoDropDownList");


    isLineManager = $("#User").data("kendoDropDownList").dataSource.data().length > 1
});


// Canvas where sape are dropped
var graph = new joint.dia.Graph,
    paper = new joint.dia.Paper({
        el: $('#paper'),
        model: graph,
        height: 2000,
        width: 10000
    });

//createPerformaceDiagram("");

function createProjectDiagram(id) {
    
    //id = '73866122-aa72-4ed8-9901-8c4d3df865bc';
    $('#projects').val(id);
    ShowLoader($('#mainContent'));
    if ( id == "") {
        id = $('#projects').data('kendoDropDownTree').value();
        pId = id;
    }

    if (typeof pId != "string") {
        pId = $('#projects').data('kendoDropDownTree').value();
    }
    graph.clear();
    currentParentNode = id;
    $.ajax({
        url: '/PJM/Project/GetWBSItem?projectId=' + id,
        dataType: "json",
        success: function (result) {
            nodesdrp = [];
            var x = 20, y = 10;
            selectedProjectId = id;
            var parentNode = result.filter(x => x.Id == id);
            var childList = [];
            childList.push(parentNode[0]);
            createChildNode(childList.filter(x => x != undefined), x, y, result, null);
            moveCenter();
            setNodeDrp();
            HideLoader($('#dcontent'));
        }
    });
}

var nodesdrp = [];

function pushNodeDrp(title, id) {
    nodesdrp.push({ text: title, value: id });
}

function setNodeDrp() {
    roots = [];
    list_to_tree();
    $("#nodeDrpList").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: nodesdrp,
        index: 0,
        //change: onChangeNodeFocus
    });


    $("#nodeDrp").kendoDropDownTree({
        placeholder: "Select ...",
        dataTextField: "text",
        dataValueField: "value",
        dataSource: roots,
        //filter: "startswith",
        animation: {
            close: {
                effects: "fadeOut zoom:out",
                duration: 300
            },
            open: {
                effects: "fadeIn zoom:in",
                duration: 300
            }
        },
        change: onChangeTreeView,
    });
}



var roots = [];
function list_to_tree() {

    var list = graph.getElements();

    var dataList = [];

    list.map(x => {

        var obj = { text: x.attributes.attrs.root.title, value: x.id }
        dataList.push(obj);
    });

    //list = list.filter(x => x.type != "standard.Link");
    var map = {}, node, i;

    for (i = 0; i < dataList.length; i += 1) {
        map[dataList[i].value] = i; // initialize the map
        dataList[i].items = []; // initialize the children
    }

    for (i = 0; i < dataList.length; i += 1) {
        node = dataList[i];
        var a = list.filter(x => x.id == node.value)[0];
        if (a.attributes.attrs.parentId !== null) {
            // if you have dangling branches check that map[node.parentId] exists
            dataList[map[a.attributes.attrs.pNodeId]].items.push(node);
        } else {
            roots.push(node);
        }
    }
    console.log("root");
    console.log(roots);
    // roots;
}

function onChangeTreeView() {
    var treeValue = $("#nodeDrp").data("kendoDropDownTree").value();
    $("#nodeDrpList").data("kendoDropDownList").value(treeValue);
    //$("#nodeDrp").show();
    $("#movebtn").show();

    //document.getElementById("movebtn").style.display = '';
    //$("#nodeDrpList").data("kendoDropDownList").trigger("change");
}


function onChangeNodeFocus() {

    $("g").removeClass("nodeHighlight");
    var value = $("#nodeDrpList").val();
    var id = $("g").find("[model-id='" + value + "']")[0].id;
    document.getElementById(id).scrollIntoView({ behavior: "smooth", block: "center", inline: "center" });
    var element = document.getElementById(id);
    element.classList.add("nodeHighlight");
}


function createChildNode(childList, x, y, result, parent) {

    for (var i = 0; i < childList.length; i++) {


        var subchild = result.filter(x => x.ParentId == childList[i].Id);

        var childCount = subchild.filter(x => x != undefined).length;

        var isHavingChild = subchild.filter(x => x != undefined).length > 0 ? true : false;



        var wraptext = ""
        if (childList[i].Title != null && childList[i].Title != "") {
            childList[i].Title = childList[i].Title + " (" + childCount + ")";
            wraptext = joint.util.breakText(childList[i].Title, {
                width: 120,
                height: 30
            });
        }
        var wraptextD = "";
        if (childList[i].Description != null && childList[i].Description != "") {
            wraptextD = joint.util.breakText(childList[i].Description, {
                width: 120,
                height: 120
            });
        }
         
        var bcolor = 'black';
        var hcolor = '#E0E2D2';
        var bodyText = "";
        if (childList[i].Type == "PROJECT") {
            bcolor = '#89b0d8';
            hcolor = 'white';
            bodyText = 'S: ' + formatDate(childList[i].Start) + '\nE: ' + formatDate(childList[i].End) + '\n\nStatus: ' + childList[i].NtsStatus;
        } else if (childList[i].Type == "STAGE") {
            bcolor = '#6c119c';
            hcolor = 'white';
            bodyText = 'S: ' + formatDate(childList[i].Start) + '\nE: ' + formatDate(childList[i].End) + '\n\nStatus: ' + childList[i].NtsStatus;
        } else if (childList[i].Type == "TASK") {
            bcolor = '#008bde';
            hcolor = 'white';
            bodyText = 'S: ' + formatDate(childList[i].Start) + '\nE: ' + formatDate(childList[i].End) + '\n\nStatus: ' + childList[i].NtsStatus + "\n\nAssigned To: " + childList[i].UserName;
        } else if (childList[i].Type == "SUBTASK") {
            bcolor = '#cced00';
            hcolor = 'white';
            bodyText = 'S: ' + formatDate(childList[i].Start) + '\nE: ' + formatDate(childList[i].End) + '\n\nStatus: ' + childList[i].NtsStatus + "\n\nAssigned To: " + childList[i].UserName;
        }  else {
            bcolor = '#f35b04';
            hcolor = 'white';
        }

        var dataModel = childList[i];
        
            
            //alert(1);
            var node = new joint.shapes.standard.HeaderedRectangle();
            node.resize(200, 200);
            node.position(x, y);
            node.attr('root/tabindex', 12);
            node.attr('root/title', wraptext);
            node.attr('header/fill', bcolor);
            node.attr('header/fillOpacity', 0.5);
            node.attr('headerText/text', wraptext);
            node.attr('body/fill', hcolor);
            node.attr('body/fillOpacity', 0.5);
            node.attr('body/fontSize', 10);
            node.attr('bodyText/text', bodyText);
            node.attr('bodyText/fontSize', 13);
            node.attr('bodyText/color', '#FF0000');
            node.attr('refId', childList[i].Id);
            node.attr('type', childList[i].Type);
            node.attr('parentId', childList[i].ParentId);
            node.attr('isHavingChild', isHavingChild);
            node.attr('collapsed', false);
            node.attr('pNodeId', parent ? parent.id : null);
            var cp = Object.values(node.id).join('');
        if (parent) {
            
                node.attr('collapsedPath', parent.attributes.attrs.collapsedPath + cp + "|");
            }
            node.attr('dataModel', dataModel);
            graph.addCell(node);

            graphLayout();
            y = y + 150;
            if (parent != null) {
                createlink(parent, node);
            }
            pushNodeDrp(wraptext, node.id);
            createChildNode(subchild.filter(x => x != undefined), x, y, result, node);
        
    }


    roundedCornerRect();
    graphLayout();
}

function graphLayout() {
    joint.layout.DirectedGraph.layout(graph, {
        setLinkVertices: false,
        nodeSep: 100,
        edgeSep: 80,
        rankDir: "TB",
        dagre: dagre,
        graphlib: dagre.graphlib,
        marginX: 100,
        marginY: 50
    });

}

function toggleCollapse(group) {
    
    if (group.model.attributes.attrs.isHavingChild == true) {

        if (group.model.attributes.attrs.collapsed == false) {
            group.addTools(toolsView);
        } else {
            group.addTools(toolsViewExpand);
        }
        if (group.model.attributes.attrs.collapsed == false) {
            var cells = graph.toJSON().cells;
            // for element
            cells = cells.filter(x => x.type != "standard.Link");
            ele = graph.getCell(group.model.id);
            var selectedChilds = [];
            for (var i = 0; i <= cells.length - 1; i++) {
                if (cells[i].attrs.collapsedPath) {
                    var cp = Object.values(cells[i].attrs.collapsedPath).join('');
                    if (cp.includes(ele.id) && cells[i].id != ele.id) {
                        selectedChilds.push(cells[i]);
                    }
                }
            }
            for (var i = 0; i <= selectedChilds.length - 1; i++) {
                var id = $("[model-id=" + selectedChilds[i].id + "]")[0].id;
                document.getElementById(id).style.display = "none";
            }
            //for link
            cells = graph.toJSON().cells;
            cells = cells.filter(x => x.type == "standard.Link");
            ele = graph.getCell(group.model.id);
            var selectedChilds = [];
            for (var i = 0; i <= cells.length - 1; i++) {
                if (cells[i].attrs.collapsedPath) {
                    var cp = Object.values(cells[i].attrs.collapsedPath).join('');
                    if (cp.includes(ele.id) && cells[i].id != ele.id) {
                        selectedChilds.push(cells[i]);
                    }
                }
            }
            for (var i = 0; i <= selectedChilds.length - 1; i++) {
                var id = $("[model-id=" + selectedChilds[i].id + "]")[0].id;
                document.getElementById(id).style.display = "none";
            }
            ele.attr('collapsed', true);
        } else {
            var cells = graph.toJSON().cells;
            // for element
            cells = cells.filter(x => x.type != "standard.Link");
            ele = graph.getCell(group.model.id);
            var selectedChilds = [];
            for (var i = 0; i <= cells.length - 1; i++) {
                if (cells[i].attrs.collapsedPath) {
                    var cp = Object.values(cells[i].attrs.collapsedPath).join('');
                    if (cp.includes(ele.id) && cells[i].id != ele.id) {
                        selectedChilds.push(cells[i]);
                    }
                }
            }
            for (var i = 0; i <= selectedChilds.length - 1; i++) {
                var id = $("[model-id=" + selectedChilds[i].id + "]")[0].id;
                document.getElementById(id).style.display = "";
            }
            //for link

            cells = graph.toJSON().cells;
            cells = cells.filter(x => x.type == "standard.Link");
            ele = graph.getCell(group.model.id);
            var selectedChilds = [];
            for (var i = 0; i <= cells.length - 1; i++) {
                if (cells[i].attrs.collapsedPath) {
                    var cp = Object.values(cells[i].attrs.collapsedPath).join('');
                    if (cp.includes(ele.id) && cells[i].id != ele.id) {
                        selectedChilds.push(cells[i]);
                    }
                }
            }
            for (var i = 0; i <= selectedChilds.length - 1; i++) {
                var id = $("[model-id=" + selectedChilds[i].id + "]")[0].id;
                document.getElementById(id).style.display = "";
            }
            ele.attr('collapsed', false);
        }
    } else {
        elementView.addTools(onlyBasicTool);
        elementView.showTools();
    }
}

function createlink(parent, child) {

    if (parent && child) {
        // Create a connection between elements.
        var link = new joint.shapes.standard.Link();
        link.source(parent);
        link.target(child);
        link.attr('collapsedPath', parent.attributes.attrs.collapsedPath + "|" + link.id);
        link.router('manhattan');
        link.connector('rounded');
        link.addTo(graph);
    }
}

function roundedCornerRect() {
    var ele = document.getElementsByTagName("rect");
    for (var x = 0; x <= ele.length - 1; x++) {
        ele[x].setAttribute("rx", 5);
        ele[x].setAttribute("ry", 5);
    }
}

function OnSelect() {

}

function onExpand() {

}

function AddData() {

}

function onRefreshDiagram() {
    if (selectedProjectId != "") {
        createPerformaceDiagram(selectedProjectId);
        setTimeout(function () { graphLayout(); }, 3000);
    } else {
        alert("Please select performance.");
    }
}

function closeDiagram() {
    graph.clear();
    selectedProjectId = "";
    selectedTemplateType = "";
    $("#User").data("kendoDropDownList").value("");
    $("#performance").data("kendoDropDownList").value("");
}


// Common Configuration
var $sx = $('#sx');
var $sy = 0;
var $w = $('#width');
var $h = $('#height');
var draggedElement;

$sx.on('input change', function () {
    var size = paper.getComputedSize();
    paper.translate(0, 0);
    paper.scale(parseFloat(this.value), parseFloat(this.value));
    paper.fitToContent({
        padding: 50,
    });
    graphLayout();
});

$w.on('input change', function () {
    paper.setDimensions(parseInt(this.value, 10), parseInt($h.val(), 10));
});

$h.on('input change', function () {
    paper.setDimensions(parseInt($w.val(), 10), parseInt(this.value, 10));
});

this.paper.on({

    'element:mouseenter': function (elementView) {

        var ofx = ((elementView.model.attributes.size.width / 2) + 10) * -1; //== elementView.model.attributes.size.width; //? -50 : -100;
        if (elementView.model.attributes.attrs.isHavingChild == true) {
            if (elementView.model.attributes.attrs.collapsed == false) {
                toolsView.options.tools[1].options.offset.x = ofx;
                elementView.addTools(toolsView);
            } else {
                toolsViewExpand.options.tools[1].options.offset.x = ofx;
                elementView.addTools(toolsViewExpand);
            }
            elementView.showTools();
        } else {
            elementView.addTools(onlyBasicTool);
            elementView.showTools();
        }
    },
    'cell:pointerclick': function (cellView) {
        // your logic goes here
    },
    'element:pointerclick': function (elementView) {
        if (lastClickedElementView != undefined) {
            joint.highlighters.mask.remove(lastClickedElementView);
        }

        currentSelectedElementView = elementView;

        var json = graph.toJSON();
        cells = json.cells;
        joint.highlighters.mask.add(elementView, { selector: 'root' }, 'my-element-highlight', {
            deep: true,
            attrs: {
                'stroke': '#FF4365',
                'stroke-width': 3,
                'padding': 10
            }
        });
        lastClickedElementView = elementView;

        showFullDetails();
    },
    'element:mouseleave': function (elementView) {
        if (elementView.model.attributes.attrs.isHavingChild == true) {
            if (elementView.model.attributes.attrs.collapsed == false) {
                elementView.addTools(toolsView);
            } else {
                elementView.addTools(toolsViewExpand);
            }
            elementView.hideTools();
        } else {
            elementView.addTools(onlyBasicTool);
            elementView.hideTools();
        }
    },
    'element:pointerdown': function (elementView, evt) {
        evt.data = elementView.model.position();
    },
    'element:pointerup': function (elementView, evt, x, y) {
        var coordinates = new g.Point(x, y);
        var elementAbove = elementView.model;
        var elementBelow = this.model.findModelsFromPoint(coordinates).find(function (el) {
            return (el.id !== elementAbove.id);
        });

        if (elementView.model.attributes.attrs.isHavingChild == true) {
            if (elementView.model.attributes.attrs.collapsed == false) {
                elementView.addTools(toolsView);
            } else {
                elementView.addTools(toolsViewExpand);
            }
            elementView.showTools();
        } else {
            elementView.addTools(onlyBasicTool);
            elementView.showTools();
        }


        if (elementBelow && elementAbove) {
        }
    }
});


//function showFullDetails() {
//    
//    var id = lastClickedElementView.model.attributes.attrs.refId;
//    if (lastClickedElementView.model.attributes.attrs.type == "STAGE" || lastClickedElementView.model.attributes.attrs.type == "PROJECT") {
//        onService(id);
//    } else if (lastClickedElementView.model.attributes.attrs.type != "TASK" || lastClickedElementView.model.attributes.attrs.type != "SUBTASK") {
//        onTask(id, lastClickedElementView.model.attributes.attrs.type);
//    }
//}

//function onTask(id, type) {
//    id = id.split("_AdhocTask")[0];
//    //alert(type);
//    var url = "";
//    if (type == "PMS_GOAL_ADHOC_TASK") {
//        url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterServiceCreate&source=Versioning&dataAction=Edit&templateCodes=PMS_GOAL_ADHOC_TASK&portalId=' + portalId + '&recordId=' + id;
//    } else if (type == "PMS_COMPENTENCY_ADHOC_TASK") {
//        url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterServiceCreate&source=Versioning&dataAction=Edit&templateCodes=PMS_COMPENTENCY_ADHOC_TASK&portalId=' + portalId + '&recordId=' + id;
//    } else if (type == "PMS_DEVELOPMENT_ADHOC_TASK") {
//        var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Versioning&dataAction=Edit&templateCodes=PMS_DEVELOPMENT_ADHOC_TASK&portalId=' + portalId + '&recordId=' + id;
//    }
//    else if (type == "PMS_REVIEW_TASK") {
//        var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Versioning&dataAction=Edit&templateCodes=PMS_REVIEW_TASK&portalId=' + portalId + '&recordId=' + id;
//    }
//    var win = GetMainWindow();
//    win.iframeOpenUrl = url;
//    win.OpenWindow({ Title: 'Edit Task', Width: 1200, Height: 600 });
//    return false;
//}


function onService(id) {
    if (id != "" && id != undefined) {
        var url = '/cms/page?lo=popup&cbm=OnAfterServiceCreate&source=Versioning&dataAction=Edit&pageName=PerformanceDocument&portalId=' + portalId + '&recordid=' + id;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Service', Width: 1200, Height: 600 });
        return false;
    } else {
        var url = '/cms/page?lo=popup&cbm=OnAfterServiceCreate&source=Versioning&dataAction=create&pageName=PerformanceDocument&portalId=' + portalId /*+ '&prms=' + prms*/;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Service', Width: 1200, Height: 600 });
        return false;
    }
}



//var createdProject = "";
//var selectedProject = "";
//function OnAfterServiceCreate(project) {
//    
//    OnPerformnaceSelection();
//}

//Tools------------------------------------------------------------------------
joint.elementTools.collapseButton = joint.elementTools.Button.extend({
    name: 'expand-collapse-button',
    options: {
        markup: [{
            tagName: 'defs',
            attributes: {
            },
            children: [{
                tagName: 'clipPath',
                attributes: {
                    id: 'myCircle',
                },
                children: [{
                    tagName: 'circle',
                    selector: 'button',
                    attributes: {
                        cx: "0",
                        cy: "10",
                        r: "100",
                        fill: "#FFFFFF"
                    },
                }]
            }]

        }, {
            tagName: 'image',
            attributes: {
                id: "expandCollapseImage",
                width: "20",
                height: "20",
                'object-fit': 'fill',
                href: "https://garance-beyrouth.com/wp-content/uploads/2020/05/plus-minus-icon-png-8-original.png",//"http://pngimg.com/uploads/plus/small/plus_PNG95.png",
                'clip-path': "url(#myCircle)"
            },

        }],
        x: '100%',
        y: '100%',
        offset: {
            x: -100,
            y: -20
        },
        rotate: true,
        action: function (evt) {
            toggleCollapse(this);
        }
    }
});

var collapseButton = new joint.elementTools.collapseButton();

var boundaryTool = new joint.elementTools.Boundary();

var removeButton = new joint.elementTools.Remove();

var toolsView = new joint.dia.ToolsView({
    tools: [
        boundaryTool,
        //removeButton,
        collapseButton
    ]
});

joint.elementTools.expandButton = joint.elementTools.Button.extend({
    name: 'expand-collapse-button',
    options: {
        markup: [{
            tagName: 'defs',
            attributes: {
            },
            children: [{
                tagName: 'clipPath',
                attributes: {
                    id: 'myCircle',
                },
                children: [{
                    tagName: 'circle',
                    selector: 'button',
                    attributes: {
                        cx: "0",
                        cy: "10",
                        r: "100",
                        fill: "#FFFFFF"
                    },
                }]
            }]

        }, {
            tagName: 'image',
            attributes: {
                id: "expandCollapseImage",
                width: "20",
                height: "20",
                'object-fit': 'fill',
                href: "http://pngimg.com/uploads/plus/small/plus_PNG95.png",
                'clip-path': "url(#myCircle)"
            },

        }],
        x: '100%',
        y: '100%',
        offset: {
            x: -100,
            y: -20
        },
        rotate: true,
        action: function (evt) {
            toggleCollapse(this);
        }
    }
});

var expandButton = new joint.elementTools.expandButton();

var toolsViewExpand = new joint.dia.ToolsView({
    tools: [
        boundaryTool,
        //removeButton,
        expandButton
    ]
});

var onlyBasicTool = new joint.dia.ToolsView({
    tools: [
        boundaryTool,
        //removeButton,
    ]
});

//Tools------------------------------------------------------------------------

// Context Menu----------------------------------------------------------------

var menu = document.querySelector(".menu");
var menuOption = document.querySelector(".menu-option");
var menuVisible = false;

function toggleMenu(command) {
    menu.style.display = command === "show" ? "block" : "none";
    menuVisible = !menuVisible;
}

window.addEventListener("click", e => {
    if (menuVisible) toggleMenu("hide");
});

function setPosition(origin) {

    menu.style.left = `${origin.left}px`;
    menu.style.top = `${origin.top}px`;
    toggleMenu("show");
}

this.paper.on('element:contextmenu', (node, x, y) => {
    
    var type = "";
    //if (node.model.attributes.attrs.type == "ADHOCROOT") {
        //var a = graph.toJSON().cells.filter(function (el) {
        //    if (el.attrs.refId != null && el.attrs.refId != undefined) {
        //        var refId = Object.values(el.attrs.refId).join('');
        //        if (refId == node.model.attributes.attrs.parentId) {
        //            console.log(Object.values(el.attrs.type).join(''));
        //            return Object.values(el.attrs.type).join('');
        //        }
        //    }
        //});
    var refId = node.model.attributes.attrs.refId;
   // type = node.model.attributes.attrs.type;
    //}
    //alert(type);
    //console.log(node);
    $("#menulist li").remove();
    if (node.model.attributes.attrs.type == 'STAGE') {
        $("#menulist").append("<li class='menu-option' onclick='onView(\"Stage\",\"" + refId + "\");'>View Stage</li>"); //same template page
        $("#menulist").append("<li class='menu-option' onclick='onDelete(\"Stage\",\"" + refId + "\");'>Remove Stage</li>"); //same template page
    } else if (node.model.attributes.attrs.type == 'TASK' || node.model.attributes.attrs.type == 'SUBTASK' ) {
        $("#menulist").append("<li class='menu-option' onclick='onView(\"Task\",\"" + refId + "\",\"" + node.model.attributes.attrs.parentId + "\");'>View Task</li>"); //same template page
        $("#menulist").append("<li class='menu-option' onclick='onDelete(\"Task\",\"" + refId + "\",\"" + node.model.attributes.attrs.parentId + "\");'>Remove Task</li>"); //same template page
    }
    else if (node.model.attributes.attrs.type == 'PROJECT') {
        $("#menulist").append("<li class='menu-option' onclick='onView(\"Project\",\"" + refId + "\",\"" + node.model.attributes.attrs.parentId + "\");'>View Project</li>"); //same template page
    } 

    var origin = {
        left: x.pageX,
        top: x.pageY
    };
    setPosition(origin);

    return false;
});

function onView(type, id, parentId) {
    if (type == "Stage" || type == "Project") {
        onService(id);
    } else if (type == "Task") {
        onTask(id,)
    }
}

function onDelete(type, id) {
    if (type == "Stage") {
        onRemoveService(id);
    } else if (type == "Task") {
        DeleteTask(id, )
    }
}


function DeleteTask(id, serviceId) {
    var flag = confirm("Are you sure that you want to proceed?");
    if (flag) {
        $.ajax({
            url: '/cms/NtsTask/DeleteTask?taskId=' + id + '&serviceId=' + serviceId,
            type: 'POST',
            data: {},
            dataType: 'json',
            success: function (result) {
                alert("Deleted Successfully");
                OnAfterServiceCreate();
            },
            error: function (ert) {
                OnAfterServiceCreate();
            }
        });
        return false;
    }
    return false;
}



function onRemoveService(id) {
    var flag = confirm("Are you sure that you want to proceed?");
    if (flag) {
        $.ajax({
            url: "/cms/NtsService/DeleteService?serviceId=" + id,
            type: 'POST',
            dataType: 'json',
            success: function (data) {
                if (data.success) {
                    alert("Deleted Successfully");
                    OnAfterServiceCreate();
                    //$('#contentTreeView').jstree(true).refresh();
                }
                else {
                    var msg = ExtractError(data.errors);
                    ShowNotification(msg);
                }

            },
            error: function (ert) {
                alert('error');
            }
        });
        return false;
    }


}


//function onAddSubTask(title, id) {
//    var prms = encodeURIComponent('parentServiceId=' + selectedProjectId + '&parentTaskId=' + id);
//    var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=ProjectTask&portalId=' + portalId + '&prms=' + prms;
//    var win = GetMainWindow();
//    win.iframeOpenUrl = url;
//    win.OpenWindow({ Title: title, Width: 800, Height: 600 });
//    return false;
//}
function onAddGoalTask(title, id) {
    var prms = encodeURIComponent('parentServiceId=' + id);
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=PerformanceGoalTask&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 800, Height: 600 });
    return false;
}

function onAddSubGoalTask(title, id) {
    var prms = encodeURIComponent('parentServiceId=' + selectedProjectId + '&parentTaskId=' + id);
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&templateCodes=PMS_GOAL_ADHOC_TASK&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 800, Height: 600 });
    return false;
}

function onAddSubCompetencyTask(title, id) {
    var prms = encodeURIComponent('parentServiceId=' + selectedProjectId + '&parentTaskId=' + id);
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&templateCodes=PMS_COMPENTENCY_ADHOC_TASK&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 800, Height: 600 });
    return false;
}

function onAddSubDocTask(title, id) {
    var prms = encodeURIComponent('parentServiceId=' + selectedProjectId + '&parentTaskId=' + id);
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&templateCodes=PMS_DEVELOPMENT_ADHOC_TASK&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 800, Height: 600 });
    return false;
}


function onAddSubReviewTask(title, id) {
    var prms = encodeURIComponent('parentServiceId=' + selectedProjectId + '&parentTaskId=' + id);
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&templateCodes=PMS_REVIEW_TASK&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 800, Height: 600 });
    return false;
}

function onAddDevelopmentTask(title, id) {
    var prms = encodeURIComponent('parentServiceId=' + id);
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=PerformanceDevelopmentTask&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 800, Height: 600 });
    return false;
}

function onAddPeerReviewTask(title, id) {
    var prms = encodeURIComponent('parentServiceId=' + id);
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=PerformanceReviewTask&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 800, Height: 600 });
    return false;
}


function onAddCompetencyTask(title, id) {
    var prms = encodeURIComponent('parentServiceId=' + id);
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=PerformanceCompetencyTask&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 800, Height: 600 });
    return false;
}

function onAddGoal(title, id) {
    var prms = encodeURIComponent('parentServiceId=' + selectedProjectId);
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=PerformanceGoal&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 1200, Height: 600 });
    return false;
}


function onAddDevelopment(title, id) {
    var prms = encodeURIComponent('parentServiceId=' + selectedProjectId);
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=PerformanceDevelopment&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 1200, Height: 600 });
    return false;
}

function onAddReview(title, id) {
    var prms = encodeURIComponent('parentServiceId=' + selectedProjectId);
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=PerformancePeerReview&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 1200, Height: 600 });
    return false;
}

function onAddCompetency(title, id) {
    var prms = encodeURIComponent('parentServiceId=' + selectedProjectId);
    var url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=PerformanceCompetency&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 1200, Height: 600 });
    return false;
}





function filterPerformance() {
    return {
        userId: $("#User").data("kendoDropDownList").value()// $("#User").val()
    };
}

function onUserChange() {
    document.getElementById("performance").style.display = "none";
    document.getElementById("performance").innerHTML = "";
    $("#performance").kendoDropDownList({
        autoBind: false,
        cascadeFrom: "User",
        dataTextField: "Name",
        dataValueField: "Id",
        optionLabel: 'Select',
        change: OnPerformnaceSelection,
        dataSource: {
            serverFiltering: true,
            transport: {
                read: "/PMS/PerformanceDiagram/GetPerformaceListByUser?userId=" + $("#User").data("kendoDropDownList").value(),
            }
        }
    }).data("kendoDropDownList");

}

function OnPerformnaceSelection() {
    selectedProjectId = $('#projects').data('kendoDropDownTree');// $("#performance").val();
    createProjectDiagram(selectedProjectId);

}


function moveCenter() {
    var div = document.getElementById("paper");
    div.scrollIntoView({
        behavior: "smooth",
        block: "center",
        inline: "start"
    });
}

function downloadDiagramPng() {

    var svgElements = document.body.querySelectorAll('svg');
    svgElements.forEach(function (item) {
        item.setAttribute("width", item.getBoundingClientRect().width);
        item.setAttribute("height", item.getBoundingClientRect().height);
        item.style.width = null;
        item.style.height = null;
    });
    /// alert("dsd");

    html2canvas(document.getElementById("paper"),
        {
            allowTaint: true,
            useCORS: true
        }).then(function (canvas) {

            var anchorTag = document.createElement("a");
            document.body.appendChild(anchorTag);
            //document.getElementById("previewImg").appendChild(canvas);
            anchorTag.download = "Diagram.jpg";
            anchorTag.href = canvas.toDataURL();
            anchorTag.target = '_blank';
            anchorTag.click();
        });
}

function downloadaAsPdf() {
    var svgElements = document.body.querySelectorAll('svg');
    svgElements.forEach(function (item) {
        item.setAttribute("width", item.getBoundingClientRect().width);
        item.setAttribute("height", item.getBoundingClientRect().height);
        item.style.width = null;
        item.style.height = null;
    });

    html2canvas(document.getElementById("paper")).then(function (canvas) {
        var data = canvas.toDataURL();
        var width = canvas.width;
        var height = canvas.height;
        var docDefinition = {
            content: [{
                image: data,
                width: width,
                height: height
            }]
        };
        pdfMake.createPdf(docDefinition).download(name);

    });
}






//// Canvas where sape are dropped
//var graph = new joint.dia.Graph,
//    paper = new joint.dia.Paper({
//        el: $('#paper'),
//        model: graph,
//        height: 2000,
//        width: 3000
//    });

//var portalId = $('#GlobalPortalId').val();

var stencilGraph = new joint.dia.Graph,
    stencilPaper = new joint.dia.Paper({
        el: $('#stencil'),
        width: 120,
        model: stencilGraph,
        interactive: false
    });

////Service configuration
//var service = new joint.shapes.standard.Rectangle();
//service.position(20, 10);
//service.resize(100, 40);
//service.attr({
//    body: {
//        fill: '#ffffff',
//        rx: 20,
//        ry: 20,
//        strokeWidth: 2
//    },
//    label: {
//        text: 'Service',
//        fill: '#E74C3C',
//        fontSize: 18,
//    }
//});


////Task Configuration
//var task = new joint.shapes.standard.Rectangle();
//task.position(20, 60);
//task.resize(100, 40);
//task.attr({
//    body: {
//        fill: '#ffffff',
//        rx: 5,
//        ry: 5,
//        strokeWidth: 2
//    },
//    label: {
//        text: 'Task',
//        fill: '#3498DB',
//        fontSize: 18,
//    },
//});

//var sheaderedRectangle = new joint.shapes.standard.HeaderedRectangle();
//sheaderedRectangle.resize(80, 80);
//sheaderedRectangle.position(20, 10);
//sheaderedRectangle.attr('root/tabindex', 12);
//sheaderedRectangle.attr('root/title', 'Service');
//sheaderedRectangle.attr('header/fill', '#89b0d8');
//sheaderedRectangle.attr('header/fillOpacity', 0.5);
//sheaderedRectangle.attr('headerText/text', 'Service');
//sheaderedRectangle.attr('body/fill', '#ffafb1');
//sheaderedRectangle.attr('body/fillOpacity', 0.5);
//sheaderedRectangle.attr('body/fontSize', 10);
//sheaderedRectangle.attr('bodyText/fontSize', 13);

var stheaderedRectangle = new joint.shapes.standard.HeaderedRectangle();
stheaderedRectangle.resize(80, 80);
stheaderedRectangle.position(20, 10);
stheaderedRectangle.attr('root/tabindex', 12);
stheaderedRectangle.attr('root/title', 'Stage');
stheaderedRectangle.attr('header/fill', '#6c119c');
stheaderedRectangle.attr('header/fillOpacity', 0.5);
stheaderedRectangle.attr('headerText/text', 'Stage');
stheaderedRectangle.attr('body/fillOpacity', 0.5);
stheaderedRectangle.attr('body/fontSize', 10);
stheaderedRectangle.attr('bodyText/fontSize', 13);
stheaderedRectangle.attr('type', 'STAGE');
stheaderedRectangle.attr('collapsedPath', '');


var theaderedRectangle = new joint.shapes.standard.HeaderedRectangle();
theaderedRectangle.resize(80, 80);
theaderedRectangle.position(20, 100);
theaderedRectangle.attr('root/tabindex', 12);
theaderedRectangle.attr('root/title', 'Task');
theaderedRectangle.attr('header/fill', '#008bde');
theaderedRectangle.attr('header/fillOpacity', 0.5);
theaderedRectangle.attr('headerText/text', 'Task');
theaderedRectangle.attr('body/fillOpacity', 0.5);
theaderedRectangle.attr('body/fontSize', 10);
theaderedRectangle.attr('bodyText/fontSize', 13);
theaderedRectangle.attr('type', 'TASK');
theaderedRectangle.attr('collapsedPath', '');



var subtheaderedRectangle = new joint.shapes.standard.HeaderedRectangle();
subtheaderedRectangle.resize(80, 80);
subtheaderedRectangle.position(20, 190);
subtheaderedRectangle.attr('root/tabindex', 12);
subtheaderedRectangle.attr('root/title', 'Sub Task');
subtheaderedRectangle.attr('header/fill', '#cced00');
subtheaderedRectangle.attr('header/fillOpacity', 0.5);
subtheaderedRectangle.attr('headerText/text', 'Sub Task');
subtheaderedRectangle.attr('body/fillOpacity', 0.5);
subtheaderedRectangle.attr('body/fontSize', 10);
subtheaderedRectangle.attr('bodyText/fontSize', 13);
subtheaderedRectangle.attr('type', 'SUBTASK');
subtheaderedRectangle.attr('collapsedPath', '');


// Stencils-------------------------------------------------------------------------
stencilGraph.addCells([stheaderedRectangle, theaderedRectangle, subtheaderedRectangle]);

stencilPaper.on('cell:pointerdown', function (cellView, e, x, y) {
    $('body').append('<div id="flyPaper" style="position:fixed;z-index:100;opacity:.7;pointer-event:none;"></div>');
    var flyGraph = new joint.dia.Graph,
        flyPaper = new joint.dia.Paper({
            el: $('#flyPaper'),
            model: flyGraph,
            interactive: false
        }),
        flyShape = cellView.model.clone(),
        pos = cellView.model.position(),
        offset = {
            x: x - pos.x,
            y: y - pos.y
        };

    flyShape.position(0, 0);
    flyGraph.addCell(flyShape);
    $("#flyPaper").offset({
        left: e.pageX - offset.x,
        top: e.pageY - offset.y
    });
    $('body').on('mousemove.fly', function (e) {
        $("#flyPaper").offset({
            left: e.pageX - offset.x,
            top: e.pageY - offset.y
        });
    });
    $('body').on('mouseup.fly', function (e) {
        var x = e.pageX,
            y = e.pageY,
            target = paper.$el.offset();

        if (x > target.left && x < target.left + paper.$el.width() && y > target.top && y < target.top + paper.$el.height()) {
            var s = flyShape.clone();
            s.position(x - target.left - offset.x, y - target.top - offset.y);
            s.resize(180, 180);
            var val = s.position();
            var parentNode = rectOverlap(val.x, val.y);
            
            if (parentNode) {
                console.log(parentNode.attributes.attrs.type);
                pType = parentNode.attributes.attrs.type;
                cType = s.attributes.attrs.type;


                switch (cType) {
                    case "STAGE":
                        if (pType == "PROJECT" || pType == "STAGE") {
                            graph.addCell(s);
                            if (s && parentNode) {
                                dragLink(s, parentNode);
                                createStage(parentNode.attributes.attrs.refId);
                            }
                        } else {
                            kendo.alert("Stage can only be a child of Project or Stage");
                        }
                        break;
                    case "TASK":
                        if (pType == "PROJECT" || pType == "STAGE") {
                            graph.addCell(s);
                            if (s && parentNode) {
                                deugger;
                                dragLink(s, parentNode);
                                createTask(parentNode.attributes.attrs.refId, pType);
                            }
                        } else {
                            kendo.alert("Task can only be a child of Project or Stage");
                        }
                        break;
                    case "SUBTASK":
                        if (pType == "TASK") {
                            graph.addCell(s);
                            if (s && parentNode) {
                                dragLink(s, parentNode);
                                createSubTask(parentNode.attributes.attrs.refId);
                            }
                        } else {
                            kendo.alert("Subtask can only be a child of Task or Subtask");
                        }
                        break;
                    default:
                }
              
            } else {
                kendo.alert("Please select valid parent");
            }
            
            //var coordinates = new g.Point(x, y);
            //var elementAbove = s.findView(paper).model; 

            //var elementBelow = this.model.findModelsFromPoint(coordinates).find(function (el) {
            //    return (el.id !== elementAbove.id);
            //});
            //alert("yes");

            //roundedCornerRect();

        }
        $('body').off('mousemove.fly').off('mouseup.fly');
        flyShape.remove();
        $('#flyPaper').remove();
    });
});

////Tools------------------------------------------------------------------------

//joint.elementTools.collapseButton = joint.elementTools.Button.extend({
//    name: 'expand-collapse-button',
//    options: {
//        markup: [{
//            tagName: 'defs',
//            attributes: {
//            },
//            children: [{
//                tagName: 'clipPath',
//                attributes: {
//                    id: 'myCircle',
//                },
//                children: [{
//                    tagName: 'circle',
//                    selector: 'button',
//                    attributes: {
//                        cx: "0",
//                        cy: "10",
//                        r: "100",
//                        fill: "#FFFFFF"
//                    },
//                }]
//            }]

//        }, {
//            tagName: 'image',
//            attributes: {
//                id: "expandCollapseImage",
//                width: "20",
//                height: "20",
//                'object-fit': 'fill',
//                href: "https://garance-beyrouth.com/wp-content/uploads/2020/05/plus-minus-icon-png-8-original.png",//"http://pngimg.com/uploads/plus/small/plus_PNG95.png",
//                'clip-path': "url(#myCircle)"
//            },

//        }],
//        x: '100%',
//        y: '100%',
//        offset: {
//            x: -100,
//            y: -20
//        },
//        rotate: true,
//        action: function (evt) {
//            toggleCollapse(this);
//        }
//    }
//});


//var collapseButton = new joint.elementTools.collapseButton();

//var boundaryTool = new joint.elementTools.Boundary();
//var removeButton = new joint.elementTools.Remove();

//var toolsView = new joint.dia.ToolsView({
//    tools: [
//        boundaryTool,
//        //removeButton,
//        collapseButton
//    ]
//});

//joint.elementTools.expandButton = joint.elementTools.Button.extend({
//    name: 'expand-collapse-button',
//    options: {
//        //markup: [{
//        //    tagName: 'path',
//        //    selector: 'icon',
//        //    attributes: {
//        //        'd': 'M -2 4 2 4 M 0 3 0 0 M -2 -1 1 -1 M -1 -4 1 -4',
//        //        'fill': 'none',
//        //        'stroke': '#FFFFFF',
//        //        'stroke-width': 2,
//        //        'pointer-events': 'none'
//        //    }
//        //}],
//        markup: [{
//            tagName: 'defs',
//            attributes: {
//            },
//            children: [{
//                tagName: 'clipPath',
//                attributes: {
//                    id: 'myCircle',
//                },
//                children: [{
//                    tagName: 'circle',
//                    selector: 'button',
//                    attributes: {
//                        cx: "0",
//                        cy: "10",
//                        r: "100",
//                        fill: "#FFFFFF"
//                    },
//                }]
//            }]

//        }, {
//            tagName: 'image',
//            attributes: {
//                id: "expandCollapseImage",
//                width: "20",
//                height: "20",
//                'object-fit': 'fill',
//                href: "http://pngimg.com/uploads/plus/small/plus_PNG95.png",
//                'clip-path': "url(#myCircle)"
//            },

//        }],
//        x: '100%',
//        y: '100%',
//        offset: {
//            x: -100,
//            y: -20
//        },
//        rotate: true,
//        action: function (evt) {
//            toggleCollapse(this);
//        }
//    }
//});


//var expandButton = new joint.elementTools.expandButton();

//var toolsViewExpand = new joint.dia.ToolsView({
//    tools: [
//        boundaryTool,
//        //removeButton,
//        expandButton
//    ]
//});

//var onlyBasicTool = new joint.dia.ToolsView({
//    tools: [
//        boundaryTool,
//        //removeButton,
//    ]
//});

////events---------------------------------------------------------------------------------



//var lastClickedElementView;
//var currentSelectedElementView;

//this.paper.on('blank:pointerdown', (evt, x, y) => {
//});

//paper.on('link:mouseenter', function (linkView) {
//    //linkView.addTools(linktools);
//});

//paper.on('link:mouseleave', function (linkView) {
//    //linkView.removeTools();
//});
//paper.on({

//    'element:mouseenter': function (elementView) {
//        if (elementView.model.attributes.attrs.isHavingChild == true) {
//            if (elementView.model.attributes.attrs.collapsed == false) {
//                elementView.addTools(toolsView);
//            } else {
//                elementView.addTools(toolsViewExpand);
//            }
//            elementView.showTools();
//        } else {
//            elementView.addTools(onlyBasicTool);
//            elementView.showTools();
//        }
//    },
//    'element:pointerclick': function (elementView) {
//        //if (lastClickedElementView != undefined) {
//        //    joint.highlighters.mask.remove(lastClickedElementView);
//        //}

//        currentSelectedElementView = elementView;

//        var json = graph.toJSON();
//        cells = json.cells;
//        for (var i = 0; i <= cells.length - 1; i++) {
//            if (cells[i].type != "standard.Link") {
//                if (linkData.filter(e => e.value === cells[i].id).length == 0) {
//                    linkData.push({ text: cells[i].attrs.headerText.text, value: cells[i].id })
//                }
//            }
//        }

//        var index = linkData.findIndex(e => e.value === elementView.model.id);
//        linkData.splice(index, 1);

//        //joint.highlighters.mask.add(elementView, { selector: 'root' }, 'my-element-highlight', {
//        //    deep: true,
//        //    attrs: {
//        //        'stroke': '#FF4365',
//        //        'stroke-width': 3,
//        //        'padding': 10
//        //    }
//        //});
//        lastClickedElementView = elementView;
//        showFullDetails();
//       // openNode();
//    },
//    'element:mouseleave': function (elementView) {
//        if (elementView.model.attributes.attrs.isHavingChild == true) {
//            if (elementView.model.attributes.attrs.collapsed == false) {
//                elementView.addTools(toolsView);
//            } else {
//                elementView.addTools(toolsViewExpand);
//            }
//            elementView.hideTools();
//        } else {
//            elementView.addTools(onlyBasicTool);
//            elementView.hideTools();
//        }
//    },
//    'element:pointerdown': function (elementView, evt) {
//        evt.data = elementView.model.position();
//    },
//    'element:button:pointerdown': function (elementView, evt) {
//        evt.stopPropagation(); // stop any further actions with the element view (e.g. dragging)

//        var model = elementView.model;

//        if (model.attr('body/visibility') === 'visible') {
//            model.attr('body/visibility', 'hidden');
//            model.attr('label/visibility', 'hidden');
//            model.attr('buttonLabel/text', 'ï¼‹'); // fullwidth plus

//        } else {
//            model.attr('body/visibility', 'visible');
//            model.attr('label/visibility', 'visible');
//            model.attr('buttonLabel/text', 'ï¼¿'); // fullwidth underscore
//        }
//    },
//    'element:pointerup': function (elementView, evt, x, y) {

//        var coordinates = new g.Point(x, y);
//        var elementAbove = elementView.model;
//        var elementBelow = this.model.findModelsFromPoint(coordinates).find(function (el) {
//            return (el.id !== elementAbove.id);
//        });

//        if (elementView.model.attributes.attrs.isHavingChild == true) {
//            if (elementView.model.attributes.attrs.collapsed == false) {
//                elementView.addTools(toolsView);
//            } else {
//                elementView.addTools(toolsViewExpand);
//            }
//            elementView.showTools();
//        } else {
//            elementView.addTools(onlyBasicTool);
//            elementView.showTools();
//        }

//        $('#Start').data('kendoDateTimePicker').value("");
//        $('#End').data('kendoDateTimePicker').value("");
//        $('#Title').data('kendoTextBox').value("");
//        //$('#OwnerUserId').data('kendoDropDownList').text('Select');
//        //$('#AssigneeUserId').data('kendoDropDownList').text('Select');
//        //$("#OwnerUserId").data("kendoDropDownList").text('');
//        //$("#AssigneeUserId").data("kendoDropDownList").text('');        

//        if (elementBelow && elementAbove) {
//            if ((elementBelow.attributes.attrs.type == "PROJECT" && elementAbove.attributes.attrs.type == "STAGE") ||
//                (elementBelow.attributes.attrs.type == "STAGE" && elementAbove.attributes.attrs.type == "TASK") ||
//                (elementBelow.attributes.attrs.type == "TASK" && elementAbove.attributes.attrs.type == "TASK") ||
//                (elementBelow.attributes.attrs.type == "TASK" && elementAbove.attributes.attrs.type == "SUBTASK") ||
//                (elementBelow.attributes.attrs.type == "SUBTASK" && elementAbove.attributes.attrs.type == "SUBTASK")) {

//                if (elementBelow && graph.getNeighbors(elementBelow).indexOf(elementAbove) === -1) {

//                    elementAbove.position(evt.data.x, evt.data.y);

//                    // Create a connection between elements.
//                    var link = new joint.shapes.standard.Link();
//                    link.source(elementBelow);
//                    link.target(elementAbove);
//                    link.addTo(graph);
                    
//                    var col = elementBelow.attributes.attrs.collapsedPath + "|" + link.id;
//                    elementView.model.attributes.attrs.collapsedPath = col;
//                    // Add remove button to the link.
//                    //var tools = new joint.dia.ToolsView({
//                    //    tools: [new joint.linkTools.Remove()]
//                    //});
//                    //link.findView(this).addTools(tools);

//                    joint.layout.DirectedGraph.layout(graph, {
//                        setLinkVertices: false, nodeSep: 50,
//                        edgeSep: 80,
//                        rankDir: "TB",
//                        dagre: dagre,
//                        graphlib: dagre.graphlib,
//                        marginX: 100,
//                        marginY: 50
//                    });

//                }
//            }
//            else {
//                elementAbove.position(evt.data.x, evt.data.y);

//            }
//        }
//    }
//});

//function createlink(parent, child) {
    
//    console.log(parent);
//    console.log(child);
//    if (parent && child) {
//        // Create a connection between elements.
//        var link = new joint.shapes.standard.Link();
//        link.source(parent);
//        link.target(child);
//        link.attr('collapsedPath', parent.attributes.attrs.collapsedPath + "|" + link.id);
//        link.router('manhattan');
//        link.connector('rounded');
//        link.addTo(graph);
//    }
//}

//// Add remove button to the link.
//var linktools = new joint.dia.ToolsView({
//    tools: [new joint.linkTools.Remove()]
//});

//var xp = 100;
//var yp = 100;
//var type = '';
//var parent = '';
//var isNewProject = false;
function createNewProject() {
    isNewProject = true;
    
    graph.clear();

    var project = new joint.shapes.standard.HeaderedRectangle();
    project.resize(180, 180);
    project.position(250, 10);
    project.attr('root/tabindex', 12);
    project.attr('root/title', "Project");
    project.attr('header/fill', '#89b0d8');
    project.attr('header/fillOpacity', 0.5);
    project.attr('headerText/text', "Project");
    project.attr('body/fillOpacity', 0.5);
    project.attr('body/fontSize', 10);
    project.attr('bodyText/fontSize', 13);
    project.attr('refId', '');
    project.attr('collapsed', false);
    project.attr('type', 'PROJECT');
    project.attr('collapsedPath', '');
    graph.addCell(project);
    roundedCornerRect();
    onService("");
}

//var parentVal;
//var dataLib = [];


//function createChildNode(childList, x, y, result, parent) {
//    isNewProject = false;

//    for (var i = 0; i < childList.length; i++) {


//        var subchild = result.filter(x => x.ParentId == childList[i].Id);

//        var isHavingChild = subchild.filter(x => x != undefined).length > 0 ? true : false;



//        var wraptext = ""
//        if (childList[i].Title != null && childList[i].Title != "") {
//            wraptext = joint.util.breakText(childList[i].Title, {
//                width: 120,
//                height: 150
//            });
//        }
//        var wraptextD = "";
//        if (childList[i].Description != null && childList[i].Description != "") {

//            wraptextD = joint.util.breakText(childList[i].Description, {
//                width: 120,
//                height: 150
//            });
//        }

//        var bcolor = 'black';
//        var hcolor = '#E0E2D2';

//        wraptext = joint.util.breakText(childList[i].Title, {
//            width: 120,
//            height: 120
//        });

//        if (childList[i].UserName == null) {
//            childList[i].UserName = "";
//        }

//        wraptextD = childList[i].Title;

//        if (childList[i].Type == "PROJECT") {
//            bcolor = '#89b0d8';
//            hcolor = 'white';
//        } else if (childList[i].Type == "STAGE") {
//            bcolor = '#6c119c';
//            hcolor = 'white';
//            wraptextD = 'S: ' + formatDate(childList[i].Start) + '\nE: ' + formatDate(childList[i].End) + '\n\nStatus: ' + childList[i].NtsStatus + "\nCount: " + subchild.length;
//        } else if (childList[i].Type == "TASK") {
//            bcolor = '#008bde';
//            hcolor = 'white';
//            wraptextD = '\n\Owner: ' + childList[i].OwnerName + '\nS: ' + formatDate(childList[i].Start) + '\nE: ' + formatDate(childList[i].End) + '\n\nStatus: ' + childList[i].NtsStatus + '\n\Assiged To: ' + childList[i].UserName + "\nCount: " + subchild.length
//        } else if (childList[i].Type == "SUBTASK") {
//            bcolor = '#cced00';
//            hcolor = 'white';
//            wraptextD = '\n\Owner: ' + childList[i].OwnerName + '\nS: ' + formatDate(childList[i].Start) + '\nE: ' + formatDate(childList[i].End) + '\n\nStatus: ' + childList[i].NtsStatus + '\n\Assiged To: ' + childList[i].UserName + "\nCount: " + subchild.length
//        }  else {
//            bcolor = '#f35b04';
//            hcolor = 'white';
//        }

//        var mainTitle = childList[i].Title + "\n\nOwner: " + childList[i].OwnerName + '\n\nS:   ' + childList[i].Start + '\nE: ' + childList[i].End + '\n\nStatus: ' + childList[i].NtsStatus+'\n\nAssignee: ' + childList[i].UserName;


     

//        var dataModel = childList[i];
//        var node = new joint.shapes.standard.HeaderedRectangle();
//        node.resize(180, 180);
//        node.position(x, y);
//        node.attr('root/tabindex', 12);
//        node.attr('root/title', mainTitle);
//        node.attr('header/fill', bcolor);
//        node.attr('header/fillOpacity', 0.5);
//        node.attr('headerText/text', wraptext);
//        node.attr('body/fill', hcolor);
//        node.attr('body/fillOpacity', 0.5);
//        node.attr('body/fontSize', 10);
//        node.attr('bodyText/text', wraptextD);
//        node.attr('bodyText/fontSize', 13);
//        node.attr('bodyText/color', '#FF0000');
//        node.attr('refId', childList[i].Id);
//        node.attr('type', childList[i].Type);
//        node.attr('parentId', childList[i].ParentId);
//        node.attr('isHavingChild', isHavingChild);
//        node.attr('collapsed', false);
//        var cp = Object.values(node.id).join('');
//        if (parent) {
//            node.attr('collapsedPath', parent.attributes.attrs.collapsedPath + cp + "|");
//        } else {
//            node.attr('collapsedPath', cp + "|");
//        }
//        node.attr('dataModel', dataModel);

//        graph.addCell(node);

//        graphLayout();
//        y = y + 150;
//        if (parent != null) {
//            createlink(parent, node);
//        }
//        createChildNode(subchild.filter(x => x != undefined), x, y, result, node);
//    }


//    roundedCornerRect();
//    graphLayout();
//}



////function createChild(child, result, parent) {
////    xp = 50

////    for (var i = 0; i < child.length; i++) {

////        var title = child[i].Title;
////        if (title.length > 20) title = title.substring(0, 20);

////        var mainTitle = child[i].Title + "\n\nOwner: " + child[i].UserName + '\n\nS:   ' + child[i].Start + '\nE: ' + child[i].End + '\n\nStatus: Draft\n\nAssignee: ' + child[i].UserName;

////        var chs = result.filter(x => x.ParentId == child[i].Id);
////        var isHavingChild = chs.length > 0 ? true : false;

////        var node = new joint.shapes.standard.HeaderedRectangle();
////        node.resize(180, 100);
////        node.position(x, y);
////        node.attr('root/tabindex', 12);
////        node.attr('root/title', wraptext);
////        node.attr('header/fill', bcolor);
////        node.attr('header/fillOpacity', 0.5);
////        node.attr('headerText/text', wraptext);
////        node.attr('body/fill', hcolor);
////        node.attr('body/fillOpacity', 0.5);
////        node.attr('body/fontSize', 10);
////        node.attr('bodyText/text', wraptextD);
////        node.attr('bodyText/fontSize', 13);
////        node.attr('bodyText/color', '#FF0000');
////        node.attr('refId', childList[i].Id);
////        node.attr('type', childList[i].Type);
////        node.attr('parentId', childList[i].ParentId);
////        node.attr('isHavingChild', isHavingChild);
////        node.attr('collapsed', false);
////        var cp = Object.values(node.id).join('');
////        if (parent) {
////            node.attr('collapsedPath', parent.attributes.attrs.collapsedPath + cp + "|");
////        } else {
////            node.attr('collapsedPath', cp + "|");
////        }
////        node.attr('dataModel', dataModel);

////        graph.addCell(node);

////        graphLayout();
////        y = y + 150;
////        if (parent != null) {
////            createlink(parent, node);
////        }
////        createChildNode(subchild.filter(x => x != undefined), x, y, result, node);
////        //createChild

////        if (child[i].Type == "PROJECT") {

////            var sheaderedRectangle = new joint.shapes.standard.HeaderedRectangle();
////            sheaderedRectangle.resize(120, 120);
////            sheaderedRectangle.position(xp, yp);
////            sheaderedRectangle.attr('root/tabindex', 12);
////            sheaderedRectangle.attr('root/title', mainTitle);
////            sheaderedRectangle.attr('header/fill', '#89b0d8');
////            sheaderedRectangle.attr('header/fillOpacity', 0.5);
////            sheaderedRectangle.attr('headerText/text', child[i].Title);
////            sheaderedRectangle.attr('body/fill', '#ffafb1');
////            sheaderedRectangle.attr('body/fillOpacity', 0.5);
////            sheaderedRectangle.attr('body/fontSize', 10);
////            sheaderedRectangle.attr('bodyText/text', 'S: ' + formatDate(child[i].Start) + '\nE: ' + formatDate(child[i].End) + '\n\nStatus: ' + child[i].NtsStatus);
////            sheaderedRectangle.attr('bodyText/fontSize', 13);
////            sheaderedRectangle.attr('bodyText/color', '#FF0000');
////            sheaderedRectangle.attr('refId', child[i].Id);
////            sheaderedRectangle.attr('collapsed', false);
////            sheaderedRectangle.attr('collapsedPath', parent.attributes.attrs.collapsedPath + "|");
////            sheaderedRectangle.attr('type', child[i].Type);
////            sheaderedRectangle.attr('predeccessor', null);
////            sheaderedRectangle.attr('isHavingChild', isHavingChild);
////            graph.addCell(sheaderedRectangle);

////            //if (isHavingChild) {
////            //    var elementView = sheaderedRectangle.findView(paper);
////            //    elementView.addTools(toolsViewCollapse);
////            //    elementView.showTools();
////            //}
////            if (parentVal != child[i].ParentId) {
////                xp = 50;
////                type = child[i].Type;
////                parentVal = child[i].ParentId;
////            }
////            xp = xp + 150;
////            createlink(parent, sheaderedRectangle);
////            dataLib.push({ id: child[i].Id, cell: sheaderedRectangle });
////        } else if (child[i].Type == "STAGE") {

////            var stheaderedRectangle = new joint.shapes.standard.HeaderedRectangle();
////            stheaderedRectangle.resize(180, 180);
////            stheaderedRectangle.position(xp, yp);
////            stheaderedRectangle.attr('root/tabindex', 12);
////            stheaderedRectangle.attr('root/title', mainTitle);
////            stheaderedRectangle.attr('header/fill', '#6c119c');
////            stheaderedRectangle.attr('header/fillOpacity', 0.5);
////            stheaderedRectangle.attr('headerText/text', title);
////            stheaderedRectangle.attr('body/fillOpacity', 0.5);
////            stheaderedRectangle.attr('body/fontSize', 10);
////            stheaderedRectangle.attr('bodyText/text', 'S: ' + formatDate(child[i].Start) + '\nE: ' + formatDate(child[i].End) + '\n\nStatus: ' + child[i].NtsStatus + "\nCount: " + chs.length);
////            stheaderedRectangle.attr('bodyText/fontSize', 13);
////            stheaderedRectangle.attr('refId', child[i].Id);
////            stheaderedRectangle.attr('collapsed', false);
////            stheaderedRectangle.attr('type', child[i].Type);
////            stheaderedRectangle.attr('predeccessor', null);
////            stheaderedRectangle.attr('isHavingChild', isHavingChild);
////            var cp = Object.values(stheaderedRectangle.id).join('');

////            stheaderedRectangle.attr('collapsedPath', parent.attributes.attrs.collapsedPath + cp + "|");

////            graph.addCell(stheaderedRectangle);
////            if (parentVal != child[i].ParentId) {
////                type = child[i].Type;
////                parentVal = child[i].ParentId;
////            }
////            xp = xp + 150;
////            createlink(parent, stheaderedRectangle);
////            dataLib.push({ id: child[i].Id, cell: stheaderedRectangle });
////        } else if (child[i].Type == "TASK") {

////            var theaderedRectangle = new joint.shapes.standard.HeaderedRectangle();
////            theaderedRectangle.resize(180, 180);
////            theaderedRectangle.position(xp, yp);
////            theaderedRectangle.attr('root/tabindex', 12);
////            theaderedRectangle.attr('root/title', mainTitle);
////            theaderedRectangle.attr('header/fill', '#008bde');
////            theaderedRectangle.attr('header/fillOpacity', 0.5);
////            theaderedRectangle.attr('headerText/text', title);
////            theaderedRectangle.attr('body/fillOpacity', 0.5);
////            theaderedRectangle.attr('body/fontSize', 10);
////            theaderedRectangle.attr('bodyText/text', '\n\Owner: ' + child[i].UserName + '\nS: ' + formatDate(child[i].Start) + '\nE: ' + formatDate(child[i].End) + '\n\nStatus: ' + child[i].NtsStatus + '\n\Assiged To: ' + child[i].UserName + "\nCount: " + chs.length);
////            theaderedRectangle.attr('bodyText/fontSize', 13);
////            theaderedRectangle.attr('refId', child[i].Id);
////            theaderedRectangle.attr('collapsed', false);
////            var cp = Object.values(theaderedRectangle.id).join('');
////            theaderedRectangle.attr('collapsedPath', parent.attributes.attrs.collapsedPath + cp + "|");
////            theaderedRectangle.attr('type', child[i].Type);
////            theaderedRectangle.attr('predeccessor', null);
////            theaderedRectangle.attr('isHavingChild', isHavingChild);
////            graph.addCell(theaderedRectangle);

////            if (parentVal != child[i].ParentId) {
////                type = child[i].Type;
////                parentVal = child[i].ParentId;
////            }
////            xp = xp + 150;

////            createlink(parent, theaderedRectangle);
////            dataLib.push({ id: child[i].Id, cell: theaderedRectangle });
////        } else if (child[i].Type == "SUBTASK") {
////            
////            var theaderedRectangle = new joint.shapes.standard.HeaderedRectangle();
////            theaderedRectangle.resize(180, 180);
////            theaderedRectangle.position(xp, yp);
////            theaderedRectangle.attr('root/tabindex', 12);
////            theaderedRectangle.attr('root/title', mainTitle);
////            theaderedRectangle.attr('header/fill', '#cced00');
////            theaderedRectangle.attr('header/fillOpacity', 0.5);
////            theaderedRectangle.attr('headerText/text', title);
////            theaderedRectangle.attr('body/fillOpacity', 0.5);
////            theaderedRectangle.attr('body/fontSize', 10);
////            theaderedRectangle.attr('bodyText/text', '\n\Owner: ' + child[i].UserName + '\nS: ' + formatDate(child[i].Start) + '\nE: ' + formatDate(child[i].End) + '\n\nStatus: ' + child[i].NtsStatus + '\n\Assiged To: ' + child[i].UserName + "\nCount: " + chs.length);
////            theaderedRectangle.attr('bodyText/fontSize', 13);
////            theaderedRectangle.attr('refId', child[i].Id);
////            theaderedRectangle.attr('collapsed', false);
////            var cp = Object.values(theaderedRectangle.id).join('');
////            theaderedRectangle.attr('collapsedPath', parent.attributes.attrs.collapsedPath + cp + "|");
////            theaderedRectangle.attr('type', child[i].Type);
////            theaderedRectangle.attr('predeccessor', null);
////            theaderedRectangle.attr('isHavingChild', isHavingChild);
////            graph.addCell(theaderedRectangle);

////            //if (isHavingChild) {
////            //    var elementView = theaderedRectangle.findView(paper);
////            //    elementView.addTools(toolsViewCollapse);
////            //    elementView.showTools();
////            //}
////            if (parentVal != child[i].ParentId) {
////                type = child[i].Type;
////                parentVal = child[i].ParentId;
////            }
////            xp = xp + 150;

////            createlink(parent, theaderedRectangle);
////            dataLib.push({ id: child[i].Id, cell: theaderedRectangle });
////        }
////        joint.layout.DirectedGraph.layout(graph, {
////            setLinkVertices: false, nodeSep: 50,
////            edgeSep: 80,
////            rankDir: "TB",
////            dagre: dagre,
////            graphlib: dagre.graphlib,
////            marginX: 100,
////            marginY: 50
////        });
////    }

////    for (var i = 0; i < child.length; i++) {
////        var chs = result.filter(x => x.ParentId == child[i].Id);
////        var pt = dataLib.filter(x => x.id == child[i].Id)[0].cell;
////        createChild(chs, result, pt);
////    }
////    roundedCornerRect();

////}

//function ShowLoader(target) {
//    kendo.ui.progress(target, true);
//}
//function HideLoader(target) {
//    kendo.ui.progress(target, false);
//}


function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [day, month, year].join('-');
}


//function SaveDiagram() {
//    var json = graph.toJSON();
//    cells = json.cells;
//    var data = [];
//    var type = "";
//    var isOk = true;
//    for (var i = 0; i < cells.length; i++) {
//        var parentId = "";
//        var parent = cells.filter(x => x.type == "standard.Link" && x.target.id == cells[i].id);
//        if (parent.length > 0) {
//            parentId = parent[0].source.id;
//        }

//        var predeccessor = cells[i].attrs.predeccessor;

//        if (predeccessor == undefined) {
//            predeccessor = [];
//        }

//        if (cells[i].type == "standard.HeaderedRectangle") {
//            if (cells[i].attrs.header.fill == "#89b0d8") {
//                type = "Service"
//            } else if (cells[i].attrs.header.fill == "#6c119c") {
//                type = "Stage"
//            } else if (cells[i].attrs.header.fill == "#008bde") {
//                type = "Task"
//            } else if (cells[i].attrs.header.fill == "#cced00") {
//                type = "SubTask"
//            }

//            var userData = $('#OwnerUserId').data('kendoDropDownList').dataSource.data();

//            var el = cells[i].attrs;
//            var d = el.root.title.split("\n");
//            var title = d[0].trim();
//            if (title == "") {
//                kendo.alert("Please add title");
//                isOk = false;
//                break;
//            }
//            var owner = d[2].split(":")[1].trim();
//            if (owner == "") {
//                //kendo.alert("Please select owner");
//                //isOk = false;

//            }
//            var sdate = d[4].split("S:")[1].trim();
//            if (sdate == "") {
//                kendo.alert("Please add start date");
//                isOk = false;
//                break;
//            }
//            var edate = d[5].split("E:")[1].trim();
//            if (edate == "") {
//                kendo.alert("Please add end date");
//                isOk = false;
//                break;
//            }
//            var assignee = d[9].split("Assignee:")[1].trim();
//            if (assignee || assignee == "") {
//                //kendo.alert("Please select assignee");
//                //isOk = false;
//            }

//            var userData = $('#OwnerUserId').data('kendoDropDownList').dataSource.data();
//            var oUserId = userData.filter(x => x.Name == owner);
//            var aUserId = userData.filter(x => x.Name == assignee);

//            var svalue = kendo.toString(sdate, 'yyyy/MM/dd HH:mm');
//            var evalue = kendo.toString(edate, 'yyyy/MM/dd HH:mm');

//            var refId = null;

//            if (el.refId) {
//                refId = Object.values(el.refId).join('');
//            }

//            var obj = {
//                "Id": cells[i].id,
//                "ParentId": parentId,
//                "UserId": aUserId[0] ? aUserId[0].Id : "",
//                "OwnerUserId": oUserId[0] ? oUserId[0].Id : "",
//                "Title": title,
//                "Start": svalue,
//                "End": evalue,
//                "Type": type,
//                "RefId": refId,
//                "Predeccessor": predeccessor
//            };
//            data.push(obj);
//        }
//    }
//    if (isOk) {
//        ShowLoader($('#mainContent'));
//        $.ajax({
//            type: "POST",
//            url: "/PJM/Project/SaveMindMap",
//            data: { Json: JSON.stringify(data) },
//            dataType: "json",
//            success: function (result) {
//                kendo.alert("Saved");
//            },
//            error: function (xhr, httpStatusMessage, customErrorMessage) {
//                HideLoader($('#mainContent'));
//                ShowNotification("Saved Successfully", "success");
//            }
//        });

//        joint.layout.DirectedGraph.layout(graph, {
//            setLinkVertices: false, nodeSep: 50,
//            edgeSep: 80,
//            rankDir: "TB",
//            dagre: dagre,
//            graphlib: dagre.graphlib,
//            marginX: 100,
//            marginY: 100
//        });
//    }
//}

//function saveProperties() {
//    ShowLoader($('#mainContent'));
//    if (lastClickedElementView) {
//        var sdate = kendo.toString($('#Start').data('kendoDateTimePicker').value(), 'yyyy/MM/dd HH:mm');
//        var edate = kendo.toString($('#End').data('kendoDateTimePicker').value(), 'yyyy/MM/dd HH:mm');
//        var owner = $('#OwnerUserId').data('kendoDropDownList').text();
//        var title = $('#Title').data('kendoTextBox').value() + "\n\nOwner: " + owner + '\n\nS:   ' + sdate + '\nE: ' + edate + '\n\nStatus: Draft\n\nAssignee: ' + $('#AssigneeUserId').data('kendoDropDownList').text();
//        if (owner.length > 10) owner = owner.substring(0, 10);
//        var assignee = $('#AssigneeUserId').data('kendoDropDownList').text();
//        if (assignee.length > 10) assignee = assignee.substring(0, 10);
//        var cell = graph.getCell(lastClickedElementView.model.id);
//        if (cell.isElement()) {
//            var btext = $('#Title').data('kendoTextBox').value();
//            if (btext.length > 10) btext = btext.substring(0, 10);
//            var body = "\nOwner: " + owner + '\n\nS:' + sdate + '\nE: ' + edate + '\n\nStatus: Draft\n\nAssignee: ' + assignee;
//            cell.attr('headerText/text', btext);
//            cell.attr('bodyText/text', body);
//            cell.attr('headerText/fontSize', 12);
//            cell.attr('root/title', title);
//            cell.attr('predeccessor', $("#pred").data("kendoMultiSelect").value());
//        }
//        $('#Start').data('kendoDateTimePicker').value("");
//        $('#End').data('kendoDateTimePicker').value("");
//        $('#Title').data('kendoTextBox').value("");
//        $('#OwnerUserId').data('kendoDropDownList').text('Select');
//        $('#AssigneeUserId').data('kendoDropDownList').text('Select');
//        $('#pred').data('kendoMultiSelect').value('');
//        $('#succ').data('kendoMultiSelect').value('');
//        ShowNotification("Properties updated", "success");
//    } else {
//        kendo.alert("Select any node to change property");
//    }
//    HideLoader($('#mainContent'));
//}


//function convertDateTime(m) {
//    var m = new Date(m);
//    return m.getUTCFullYear() + "/" + (m.getUTCMonth() + 1) + "/" + m.getUTCDate() + " " + m.getUTCHours() + ":" + m.getUTCMinutes() + ":" + m.getUTCSeconds();
//}

//var zoomLevel = 0;
//function onZoomIn() {
//    zoomLevel = Math.min(0.2, zoomLevel + 0.2);
//    var size = paper.getComputedSize();
//    paper.translate(0, 0);
//    paper.scale(zoomLevel, zoomLevel, size.width / 2, size.height / 2);
//}

//function onZoomOut() {
//    zoomLevel = Math.max(4, zoomLevel - 0.2);
//    var size = paper.getComputedSize();
//    paper.translate(0, 0);
//    paper.scale(zoomLevel, zoomLevel, size.width / 2, size.height / 2);
//}

//var $sx = $('#sx');
//var $sy = 0;
//var $w = $('#width');
//var $h = $('#height');

//$sx.on('input change', function () {
//    var size = paper.getComputedSize();
//    paper.translate(0, 0);
//    paper.scale(parseFloat(this.value), parseFloat(this.value));
//    paper.fitToContent({
//        padding: 50,
//    });
//    graphLayout();
//});
//$w.on('input change', function () {
//    paper.setDimensions(parseInt(this.value, 10), parseInt($h.val(), 10));
//});
//$h.on('input change', function () {
//    paper.setDimensions(parseInt($w.val(), 10), parseInt(this.value, 10));
//});

//paper.on({
//    //resize: function (width, height) {
//    //    $w.val(width).next().text(Math.round(width));
//    //    $h.val(height).next().text(Math.round(height));
//    //}
//});



//var linkData = [];

//// create DropDownList from input HTML element
//$("#link").kendoDropDownList({
//    dataTextField: "text",
//    dataValueField: "value",
//    dataSource: linkData,
//    select: onSelectLink
//});


//var preData = [];

//// create DropDownList from input HTML element
//$("#pred").kendoMultiSelect({
//    dataTextField: "text",
//    dataValueField: "value",
//    dataSource: preData,
//});

//var succData = [];

//// create DropDownList from input HTML element
//$("#succ").kendoMultiSelect({
//    dataTextField: "text",
//    dataValueField: "value",
//    dataSource: succData,
//});


//function onSelectLink(e) {

//    createlink(lastClickedElementView.model.attributes.id, e.dataItem.value);
//};

//function isNew() {
//    $("#saveDiagram").show();
//    $("#stencil").show();
//}

//function isExisting() {
//    $("#saveDiagram").hide();
//    $("#stencil").hide();
//}

//$(document).ready(function () {
//    $(".toggle-nav").trigger("click");
//    var projectId = document.getElementById("ProjectId").value;
//    if (projectId) {
//        $('#projects').data('kendoDropDownList').value(projectId);
//        onProjectSelection();
//    }

//    $("#projects").kendoDropDownList({
//        dataTextField: "Name",
//        dataValueField: "Id",
//        filter: "contains",
//        optionLabel: "select project",
//        change: onProjectSelect,
//        dataSource:
//        {
//            transport:
//            {
//                read:
//                {
//                    url: "/PJM/ProjectTask/GetProjectsList",
//                }
//            }
//        }
//    });

//    $("#AssigneeUserId").kendoDropDownList({
//        dataTextField: "Name",
//        dataValueField: "Id",
//        filter: "contains",
//        optionLabel: "select",
//        dataSource:
//        {
//            transport:
//            {
//                read:
//                {
//                    url: "/Cms/User/GetUserIdNameList",
//                }
//            }
//        }
//    });

//    $("#OwnerUserId").kendoDropDownList({
//        dataTextField: "Name",
//        dataValueField: "Id",
//        filter: "contains",
//        optionLabel: "select",
//        dataSource:
//        {
//            transport:
//            {
//                read:
//                {
//                    url: "/Cms/User/GetUserIdNameList",
//                }
//            }
//        }
//    });


//});


function onProjectSelect() {
    isNewProject = false;
    var pId = $('#projects').val();
    selectedProject = pId;
    createProjectDiagram(pId);
}

//function onProjectSelection(pId) {
//    $('#projects').val(pId);
//    ShowLoader($('#mainContent'));
//    if (typeof pId != "string") {
//        pId = $('#projects').data('kendoDropDownList').value();
//    }
//    graph.clear();
    
//    $.ajax({
//        url: '/PJM/Project/GetWBSItem?projectId=' + pId,
//        dataType: "json",
//        success: function (result) {
//            var ids;
//            var project = result.filter(x => x.Type == "PROJECT");
//            var child = result.filter(x => x.ParentId == project[0].Id);
//            var isHavingChild = child.length > 0 ? true : false;
//            var mainTitle = project[0].Title + "\n\nOwner: " + project[0].OwnerName + '\n\nS:   ' + project[0].Start + '\nE: ' + project[0].End + '\n\nStatus: Draft\n\nAssignee: ' + project[0].UserName;

//            var projectN = new joint.shapes.standard.HeaderedRectangle();
//            if (project[0].Title.length > 20) project[0].Title = project[0].Title.substring(0, 20);

//            projectN.resize(180, 180);
//            projectN.position(xp, yp);
//            projectN.attr('root/tabindex', 12);
//            projectN.attr('root/title', mainTitle);
//            projectN.attr('header/fill', '#89b0d8');
//            projectN.attr('header/fillOpacity', 0.5);
//            projectN.attr('headerText/text', project[0].Title);
//            projectN.attr('body/fillOpacity', 0.5);
//            projectN.attr('body/fontSize', 10);
//            projectN.attr('bodyText/fontSize', 13);
//            projectN.attr('bodyText/text', 'S: ' + formatDate(project[0].Start) + '\nE: ' + formatDate(project[0].End) + '\n\nStatus: ' + project[0].NtsStatus + "\nCount: " + child.length);
//            projectN.attr('refId', project[0].Id);
//            projectN.attr('collapsed', false);
//            var cp = Object.values(projectN.id).join('');
//            projectN.attr('collapsedPath', cp + "|");
//            projectN.attr('type', "PROJECT");
//            projectN.attr('predeccessor', $("#pred").data("kendoMultiSelect").value());
//            projectN.attr('isHavingChild', isHavingChild);
//            graph.addCell(projectN);

//            if (type != project.Type) {
//                xp = 50;
//                yp = yp + 150;
//                type = project[0].Type;
//            }
//            xp = xp + 150;

//            createChildNode(child, xp, yp, result, projectN);
//            HideLoader($('#mainContent'));
//        }

//    });
//    HideLoader($('#mainContent'));
//}


function graphLayout() {
    joint.layout.DirectedGraph.layout(graph, {
        setLinkVertices: false,
        nodeSep: 100,
        edgeSep: 80,
        rankDir: "TB",
        dagre: dagre,
        graphlib: dagre.graphlib,
        marginX: 100,
        marginY: 50
    });

}



//function setPredAndSucc() {

//    var selectedNodeId = currentSelectedElementView.model.attributes.id;
//    var pred = $("#pred").data("kendoMultiSelect");
//    var succ = $("#succ").data("kendoMultiSelect");
//    var a = [];
//    var b = [];
//    var succValue = [];
//    var predValue = [];
//    var json = graph.toJSON();
//    cells = json.cells;
    
//    for (var i = 0; i <= cells.length - 1; i++) {
//        if (cells[i].type == "standard.Link") {
//            if (cells[i].source.id == selectedNodeId) {
//                if (succData.filter(e => e.value === cells[i].id).length == 0) {
//                    var text = cells.filter(e => e.id == cells[i].target.id);
//                    b.push({ text: text[0].attrs.headerText.text, value: cells[i].target.id });
//                    succValue.push(cells[i].target.id);
//                }
//            } else if (cells[i].target.id == selectedNodeId) {
//                if (preData.filter(e => e.value === cells[i].id).length == 0) {
//                    var text = cells.filter(e => e.id == cells[i].source.id);
//                    predValue.push(cells[i].source.id);
//                }
//            }
//        } else {
//            if (currentSelectedElementView.model.attributes.attrs.type == "PROJECT") {
//            } if (currentSelectedElementView.model.attributes.attrs.type == "STAGE") {
//                if (cells[i].attrs.type) {
//                    var node = Object.values(cells[i].attrs.type).join('');
//                    if (node == "PROJECT" || node == "STAGE") {
//                        a.push({ text: cells[i].attrs.headerText.text, value: cells[i].id });
//                    }
//                }
//            } if (currentSelectedElementView.model.attributes.attrs.type == "TASK") {
//                if (cells[i].attrs.type) {
//                    var node = Object.values(cells[i].attrs.type).join('');
//                    if (node == "PROJECT" || node == "STAGE") {
//                        a.push({ text: cells[i].attrs.headerText.text, value: cells[i].id });
//                    }
//                }
//            } if (currentSelectedElementView.model.attributes.attrs.type == "SUBTASK") {
//                if (cells[i].attrs.type) {
//                    var node = Object.values(cells[i].attrs.type).join('');
//                    if (node == "TASK" || node == "SUBTASK") {
//                        a.push({ text: cells[i].attrs.headerText.text, value: cells[i].id });
//                    }
//                }
//            }
//        }
//    }


//    pred.dataSource.data(a);
//    pred.value(predValue);
//    succ.dataSource.data(b);
    
//    succ.value(succValue);
//    succ.enable(false);
//}





//var childEl = [];
//var childLinks = [];
//function toggleCollapse(group) {
//    if (group.model.attributes.attrs.isHavingChild == true) {

//        if (group.model.attributes.attrs.collapsed == false) {
//            group.addTools(toolsView);
//        } else {
//            group.addTools(toolsViewExpand);
//        }
//        if (group.model.attributes.attrs.collapsed == false) {
//            var cells = graph.toJSON().cells;
//            // for element
//            cells = cells.filter(x => x.type != "standard.Link");
//            ele = graph.getCell(group.model.id);
//            var selectedChilds = [];
//            for (var i = 0; i <= cells.length - 1; i++) {
//                var cp = Object.values(cells[i].attrs.collapsedPath).join('');
//                if (cp.includes(ele.id) && cells[i].id != ele.id) {
//                    selectedChilds.push(cells[i]);
//                }
//            }
//            for (var i = 0; i <= selectedChilds.length - 1; i++) {
//                var id = $("[model-id=" + selectedChilds[i].id + "]")[0].id;
//                document.getElementById(id).style.display = "none";
//            }
//            //for link
//            cells = graph.toJSON().cells;
//            cells = cells.filter(x => x.type == "standard.Link");
//            ele = graph.getCell(group.model.id);
//            var selectedChilds = [];
//            for (var i = 0; i <= cells.length - 1; i++) {
//                var cp = Object.values(cells[i].attrs.collapsedPath).join('');
//                if (cp.includes(ele.id) && cells[i].id != ele.id) {
//                    selectedChilds.push(cells[i]);
//                }
//            }
//            for (var i = 0; i <= selectedChilds.length - 1; i++) {
//                var id = $("[model-id=" + selectedChilds[i].id + "]")[0].id;
//                document.getElementById(id).style.display = "none";
//            }
//            ele.attr('collapsed', true);
//        } else {
//            var cells = graph.toJSON().cells;
//            // for element
//            cells = cells.filter(x => x.type != "standard.Link");
//            ele = graph.getCell(group.model.id);
//            var selectedChilds = [];
//            for (var i = 0; i <= cells.length - 1; i++) {
//                var cp = Object.values(cells[i].attrs.collapsedPath).join('');
//                if (cp.includes(ele.id) && cells[i].id != ele.id) {
//                    selectedChilds.push(cells[i]);
//                }
//            }
//            for (var i = 0; i <= selectedChilds.length - 1; i++) {
//                var id = $("[model-id=" + selectedChilds[i].id + "]")[0].id;
//                document.getElementById(id).style.display = "";
//            }
//            //for link
            
//            cells = graph.toJSON().cells;
//            cells = cells.filter(x => x.type == "standard.Link");
//            ele = graph.getCell(group.model.id);
//            var selectedChilds = [];
//            for (var i = 0; i <= cells.length - 1; i++) {
//                var cp = Object.values(cells[i].attrs.collapsedPath).join('');
//                if (cp.includes(ele.id) && cells[i].id != ele.id) {
//                    selectedChilds.push(cells[i]);
//                }
//            }
//            for (var i = 0; i <= selectedChilds.length - 1; i++) {
//                var id = $("[model-id=" + selectedChilds[i].id + "]")[0].id;
//                document.getElementById(id).style.display = "";
//            }
//            ele.attr('collapsed', false);
//        }
//    } else {
//        elementView.addTools(onlyBasicTool);
//        elementView.showTools();
//    }
//}

function showFullDetails() {
    
    var id = lastClickedElementView.model.attributes.attrs.refId;
    if (lastClickedElementView.model.attributes.attrs.type == "PROJECT" || lastClickedElementView.model.attributes.attrs.type == "STAGE") {
        onService(id);
    } else if (lastClickedElementView.model.attributes.attrs.type == "TASK" || lastClickedElementView.model.attributes.attrs.type == "SUBTASK") {
        onTask(id);
    }
}

function onTask(id) {
    var url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterServiceCreate&source=Versioning&dataAction=Edit&pageName=ProjectTask&portalId=' + portalId + '&recordId=' + id;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: 'Edit Task', Width: 1200, Height: 600 });
    return false;
}


function onService(id) {
    if (id != "" && id != undefined) {
        var url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterServiceCreate&source=Versioning&dataAction=Edit&pageName=Project&portalId=' + portalId + '&recordId=' + id;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Service', Width: 1200, Height: 600 });
        return false;
    } else {
        var prms = encodeURIComponent('TemplateCode=PROJECT_SUPER_SERVICE');
        var url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=Project&portalId=' + portalId + '&prms=' + prms;
        var win = GetMainWindow();
        win.iframeOpenUrl = url;
        win.OpenWindow({ Title: 'Create Project', Width: 1200, Height: 600 });
        return false;
    }
}
var createdProject = "";
var selectedProject = "";
function OnAfterServiceCreate(project) {
    
    if (!isNewProject) {
      
         selectedProject = $('#projects').val();
        if (selectedProject == "" || selectedProject == null) {
            $('#projects').val(project.ServiceId);
        }
        createProjectDiagram($('#projects').val());
    } else {
        isNewProject = false;
        selectedProject = project.ServiceId;
        createProjectDiagram(project.ServiceId);
    }
}

//var lastProject;

//function OnAfterTaskCreate(task) {
//    console.log(project);
//    console.log("project");
//    //alert("jhdsj");
//    //$('#projects').data('kendoDropDownList').value(lastProject);
//    //onProjectSelection();
//    var url = "/PJM/Project/ProjectMindMap?projectId=" + project.Id;
//    LoadPartailView(url, $('#cms-content'));
//}

//// for rounded corner 
//function roundedCornerRect() {
    
//    var ele = document.getElementsByTagName("rect");
//    for (var x = 0; x <= ele.length - 1; x++) {
//        ele[x].setAttribute("rx", 5);
//        ele[x].setAttribute("ry", 5);
//    }
//}


//function isNodeOverlap(postion) {

//    var nodes = graph.attributes.cells.models;
//    for (var i = 0; i <= nodes.length - 1; i++) {
//        var sNode = nodes[i].position;
//        // If one rectangle is on left side of other
//        if (postion.x >= r2.x || l2.x >= postion.x) {
//            return false;
//        }

//        // If one rectangle is above other
//        if (postion.y <= r2.y || l2.y <= postion.y) {
//            return false;
//        }
//    }
//    // If one rectangle is on left side of other
//    if (postion.x >= r2.x || l2.x >= postion.x) {
//        return false;
//    }

//    // If one rectangle is above other
//    if (postion.y <= r2.y || l2.y <= postion.y) {
//        return false;
//    }
//    return true;
//}


function valueInRange(value, min, max) { return (value >= min) && (value <= max); }

function rectOverlap(x, y) {
    var nodes = graph.attributes.cells.models;
    for (var i = 0; i <= nodes.length - 1; i++) {
        var sNode = nodes[i].position();
        var xOverlap = valueInRange(x, sNode.x, sNode.x + 180) ||
            valueInRange(sNode.x, x, x + 180);

        var yOverlap = valueInRange(y, sNode.y, sNode.y + 180) ||
            valueInRange(sNode.y, y, y + 180);

        var res = xOverlap && yOverlap;
        if (res) {
            return nodes[i]
        }
    }
    return false;
}

function dragLink(elementBelow, elementAbove) {
    var link = new joint.shapes.standard.Link();
    link.source(elementAbove);
    link.target(elementBelow);
    link.addTo(graph);
    var col = elementBelow.attributes.attrs.collapsedPath + "|" + link.id;
    elementAbove.attributes.attrs.collapsedPath = col;
    joint.layout.DirectedGraph.layout(graph, {
        setLinkVertices: false, nodeSep: 50,
        edgeSep: 80,
        rankDir: "TB",
        dagre: dagre,
        graphlib: dagre.graphlib,
        marginX: 100,
        marginY: 50
    });
}

function createStage(id) {

    var parentId = id;
    var projectId = $('#projects').val();
    $.ajax({
        url: '/pjm/projecttask/GetServiceSequenceOrder',
        type: 'GET',
        data: { parentId: parentId },
        success: function (res) {
            var prms = encodeURIComponent('parentServiceId=' + parentId + '&servicePlusId=' + projectId+'&sequenceOrder=' + res.count);

            var url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=ProjectStage&portalId=' + portalId + '&prms=' + prms;
            var win = GetMainWindow();
            win.iframeOpenUrl = url;
            win.OpenWindow({ Title: 'Stage', Width: 1200, Height: 600 });

        }
    });

    //var prms = encodeURIComponent('parentServiceId=' + parentId);
 
    return false;
}

function createTask(id, type) {
    
    var parentId = id;
    var projectId = $('#projects').val();
    //if (type == "TASK") {
    //    var prms = encodeURIComponent('parentServiceId=' + parentId + '&parentTaskId=' + id);
    //    var url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=ProjectTask&portalId=' + portalId + '&prms=' + prms;
    //    var win = GetMainWindow();
    //    win.iframeOpenUrl = url;
    //    win.OpenWindow({ Title: 'Create Task', Width: 1200, Height: 600 });
    //    return false;
    //} else {
    $.ajax({
        url: '/pjm/projecttask/GetTaskSequenceOrder',
        type: 'GET',
        data: { parentId: parentId },
        success: function (res) {
            var prms = encodeURIComponent('parentServiceId=' + parentId + '&servicePlusId=' + projectId+'&sequenceOrder=' + res.count);

            var url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=ProjectTask&portalId=' + portalId + '&prms=' + prms;
            var win = GetMainWindow();
            win.iframeOpenUrl = url;
            win.OpenWindow({ Title: 'Create Task', Width: 1200, Height: 600 });

        }
    });
        
       
        return false;
    //}
}

function createSubTask(taskId) {
    
    var parentId = $('#projects').val();
    $.ajax({
        url: '/pjm/projecttask/GetSubTaskSequenceOrder',
        type: 'GET',
        data: { parentId: taskId },
        success: function (res) {
            var prms = encodeURIComponent('parentServiceId=' + parentId + '&parentTaskId=' + taskId + '&servicePlusId=' + parentId+'&sequenceOrder=' + res.count);

            var url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=ProjectTask&portalId=' + portalId + '&prms=' + prms;
            var win = GetMainWindow();
            win.iframeOpenUrl = url;
            win.OpenWindow({ Title: 'Create Sub Task', Width: 1200, Height: 600 });

        }
    });
   
       
    return false;
}


//function openNode() {

//}