var selectedPerformanceId = "";
var selectedTemplateType = "";
var lastClickedElementView;
var currentSelectedElementView;
var currentParentNode = "";
var isLineManager = false;

$(".toggle-nav").trigger("click");

var portalId = $('#GlobalPortalId').val();

$(document).ready(function () {

    $("#User").kendoDropDownList({
        dataTextField: "Name",
        dataValueField: "Id",
        optionLabel: 'Select',
        change: onUserChange,
        dataSource: {
            serverFiltering: true,
            transport: {
                read: "/PMS/PerformanceDiagram/GetUserList",
            }
        }
    });

 

   
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


    isLineManager =  $("#User").data("kendoDropDownList").dataSource.data().length > 1
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

function createPerformaceDiagram(id) {
    //id = '73866122-aa72-4ed8-9901-8c4d3df865bc';
    graph.clear();
    ShowLoader($('#dcontent'));
    currentParentNode = id;
    $.ajax({
        url: '/pms/performanceDiagram/GetPerformanceDiagram?performanceDocumentId=' + id,
        dataType: "json",
        success: function (result) {
            nodesdrp = [];
            var x = 20, y = 10;
            selectedPerformanceId = id;
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

        if (childList[i].Type == "SERVICE") {
            bcolor = '#22577a';
            hcolor = 'white';
        } else if (childList[i].Type == "GOAL_ROOT") {
            bcolor = '#38a3a5';
            hcolor = 'white';
        } else if (childList[i].Type == "COMPENTENCY_ROOT") {
            bcolor = '#57cc99';
            hcolor = 'white';
        } else if (childList[i].Type == "GOAL") {
            bcolor = '#80ed99';
            hcolor = 'white';
        } else if (childList[i].Type == "PMS_COMPENTENCY_ADHOC_TASK") {
            bcolor = '#c7f9cc';
            hcolor = 'white';
        } else if (childList[i].Type == "PMS_GOAL_ADHOC_TASK") {
            bcolor = '#ffa62b';
            hcolor = 'white';
        } else if (childList[i].Type == "ADHOCROOT") {
            bcolor = '#240046';
            hcolor = 'white';
        } else if (childList[i].Type == "STEPROOT") {
            bcolor = '#8ac926';
            hcolor = 'black';
        } else if (childList[i].Type == "SUBTASKROOT") {
            bcolor = '#aacc00';
            hcolor = 'black';
        } else {
            bcolor = '#f35b04';
            hcolor = 'white';
        }

        var dataModel = childList[i];
        if (childList[i].NodeShape == 0) {
            var circle = new joint.shapes.standard.Circle();
            circle.resize(80, 80);
            circle.position(150, 10);
            circle.attr('root/title', wraptext);
            circle.attr('label/text', wraptext);
            circle.attr('body/fill', bcolor);
            circle.attr('label/fill', hcolor);
            circle.attr('refId', childList[i].Id);
            circle.attr('type', childList[i].Type);
            circle.attr('parentId', childList[i].ParentId);
            circle.attr('isHavingChild', isHavingChild);
            circle.attr('collapsed', false);
            circle.attr('pNodeId', parent ? parent.id : null);
            var cp = Object.values(circle.id).join('');
            if (parent) {
                circle.attr('collapsedPath', parent.attributes.attrs.collapsedPath + cp + "|");
            }
            circle.attr('dataModel', dataModel);
            graph.addCell(circle);

            graphLayout();
            y = y + 150;
            if (parent != null) {
                createlink(parent, circle);
            }
            pushNodeDrp(wraptext, circle.id);
            createChildNode(subchild.filter(x => x != undefined), x, y, result, circle);
        } else if (childList[i].NodeShape == 4) {
            var ellipse = new joint.shapes.standard.Ellipse();
            ellipse.resize(180, 80);
            ellipse.position(20, 150);
            ellipse.attr('root/title', wraptext);
            ellipse.attr('label/text', wraptext);
            ellipse.attr('body/fill', bcolor);
            ellipse.attr('label/fill', hcolor);
            ellipse.attr('refId', childList[i].Id);
            ellipse.attr('type', childList[i].Type);
            ellipse.attr('parentId', childList[i].ParentId);
            ellipse.attr('isHavingChild', isHavingChild);
            ellipse.attr('collapsed', false);
            ellipse.attr('pNodeId', parent ? parent.id : null);
            var cp = Object.values(ellipse.id).join('');
            if (parent) {
                ellipse.attr('collapsedPath', parent.attributes.attrs.collapsedPath + cp + "|");
            }
            ellipse.attr('dataModel', dataModel);
            graph.addCell(ellipse);

            graphLayout();
            y = y + 150;
            if (parent != null) {
                createlink(parent, ellipse);
            }
            pushNodeDrp(wraptext, ellipse.id);
            createChildNode(subchild.filter(x => x != undefined), x, y, result, ellipse);
        } else if (childList[i].NodeShape == 1) {
            var rectangle = new joint.shapes.standard.Rectangle();
            rectangle.resize(180, 50);
            rectangle.position(150, 270);
            rectangle.attr('root/title', wraptext);
            rectangle.attr('label/text', wraptext);
            rectangle.attr('body/fill', bcolor);
            rectangle.attr('label/fill', hcolor);
            rectangle.attr('refId', childList[i].Id);
            rectangle.attr('type', childList[i].Type);
            rectangle.attr('parentId', childList[i].ParentId);
            rectangle.attr('isHavingChild', isHavingChild);
            rectangle.attr('collapsed', false);
            rectangle.attr('pNodeId', parent ? parent.id : null);
            var cp = Object.values(rectangle.id).join('');
            if (parent) {
                rectangle.attr('collapsedPath', parent.attributes.attrs.collapsedPath + cp + "|");
            }
            rectangle.attr('dataModel', dataModel);

            graph.addCell(rectangle);

            graphLayout();
            y = y + 150;
            if (parent != null) {
                createlink(parent, rectangle);
            }
            pushNodeDrp(wraptext, rectangle.id);
            createChildNode(subchild.filter(x => x != undefined), x, y, result, rectangle);

        } else if (childList[i].NodeShape == 5) {
            var rectangle = new joint.shapes.standard.Rectangle();
            rectangle.resize(180, 60);
            rectangle.position(150, 270);
            rectangle.attr('root/title', wraptext);
            rectangle.attr('label/text', wraptext);
            rectangle.attr('body/fill', bcolor);
            rectangle.attr('body/rx', 50);
            rectangle.attr('body/ry', 50);
            rectangle.attr('label/fill', hcolor);
            rectangle.attr('refId', childList[i].Id);
            rectangle.attr('type', childList[i].Type);
            rectangle.attr('parentId', childList[i].ParentId);
            rectangle.attr('isHavingChild', isHavingChild);
            rectangle.attr('collapsed', false);
            rectangle.attr('pNodeId', parent ? parent.id : null);
            var cp = Object.values(rectangle.id).join('');
            if (parent) {
                rectangle.attr('collapsedPath', parent.attributes.attrs.collapsedPath + cp + "|");
            }
            rectangle.attr('dataModel', dataModel);

            graph.addCell(rectangle);

            graphLayout();
            y = y + 150;
            if (parent != null) {
                createlink(parent, rectangle);
            }
            pushNodeDrp(wraptext, rectangle.id);
            createChildNode(subchild.filter(x => x != undefined), x, y, result, rectangle);

        } else if (childList[i].NodeShape == 2) {
            var polygon = new joint.shapes.standard.Polygon();
            polygon.resize(180, 120);
            polygon.position(20, 270);
            polygon.attr('root/title', wraptext);
            polygon.attr('label/text', wraptext);
            polygon.attr('body/refPoints', '0,10 10,0 20,10 10,20');
            polygon.attr('body/fill', bcolor);
            polygon.attr('refId', childList[i].Id);
            polygon.attr('type', childList[i].Type);
            polygon.attr('parentId', childList[i].ParentId);
            polygon.attr('isHavingChild', isHavingChild);
            polygon.attr('collapsed', false);
            polygon.attr('pNodeId', parent ? parent.id : null);
            var cp = Object.values(polygon.id).join('');
            if (parent) {
                polygon.attr('collapsedPath', parent.attributes.attrs.collapsedPath + cp + "|");
            }
            polygon.attr('dataModel', dataModel);

            graph.addCell(polygon);

            graphLayout();
            y = y + 150;
            if (parent != null) {
                createlink(parent, polygon);
            }
            createChildNode(subchild.filter(x => x != undefined), x, y, result, polygon);
        }
        else if (childList[i].NodeShape == 3) {
            var node = new joint.shapes.standard.HeaderedRectangle();
            node.resize(180, 100);
            node.position(x, y);
            node.attr('root/tabindex', 12);
            node.attr('root/title', wraptext);
            node.attr('header/fill', bcolor);
            node.attr('header/fillOpacity', 0.5);
            node.attr('headerText/text', wraptext);
            node.attr('body/fill', hcolor);
            node.attr('body/fillOpacity', 0.5);
            node.attr('body/fontSize', 10);
            node.attr('bodyText/text', wraptextD);
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
            } else {
                node.attr('collapsedPath', cp + "|");
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
        } else {
            var node = new joint.shapes.standard.HeaderedRectangle();
            node.resize(180, 100);
            node.position(x, y);
            node.attr('root/tabindex', 12);
            node.attr('root/title', wraptext);
            node.attr('header/fill', 'red');
            node.attr('header/fillOpacity', 0.5);
            node.attr('headerText/text', wraptext);
            node.attr('body/fill', hcolor);
            node.attr('body/fillOpacity', 0.5);
            node.attr('body/fontSize', 10);
            node.attr('bodyText/text', wraptextD);
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
    if (selectedPerformanceId != "") {
        createPerformaceDiagram(selectedPerformanceId);
        setTimeout(function () { graphLayout(); }, 3000);
    } else {
        alert("Please select performance.");
    }
}

function closeDiagram() {
    graph.clear();
    selectedPerformanceId = "";
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


function showFullDetails() {
    
    var id = lastClickedElementView.model.attributes.attrs.refId;
    if (lastClickedElementView.model.attributes.attrs.type == "GOAL" || lastClickedElementView.model.attributes.attrs.type == "COMPENTENCY") {
        onService(id);
    } else if (lastClickedElementView.model.attributes.attrs.type != "STEPROOT" && lastClickedElementView.model.attributes.attrs.type != "ADHOCROOT"
        && lastClickedElementView.model.attributes.attrs.type != "GOAL_ROOT" && lastClickedElementView.model.attributes.attrs.type != "COMPENTENCY_ROOT") {
        onTask(id, lastClickedElementView.model.attributes.attrs.type);
    }
}

function onTask(id, type) {
    id = id.split("_AdhocTask")[0];
    //alert(type);
    var url = "";
    if (type == "PMS_GOAL_ADHOC_TASK") {
        url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterServiceCreate&source=Versioning&dataAction=Edit&templateCodes=PMS_GOAL_ADHOC_TASK&portalId=' + portalId + '&recordId=' + id;
    } else if (type == "PMS_COMPENTENCY_ADHOC_TASK") {
        url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterServiceCreate&source=Versioning&dataAction=Edit&templateCodes=PMS_COMPENTENCY_ADHOC_TASK&portalId=' + portalId + '&recordId=' + id;
    } else if (type == "PMS_DEVELOPMENT_ADHOC_TASK") {
        var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Versioning&dataAction=Edit&templateCodes=PMS_DEVELOPMENT_ADHOC_TASK&portalId=' + portalId + '&recordId=' + id;
    }
    else if (type == "PMS_REVIEW_TASK") {
        var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Versioning&dataAction=Edit&templateCodes=PMS_REVIEW_TASK&portalId=' + portalId + '&recordId=' + id;
    }
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: 'Edit Task', Width: 1200, Height: 600 });
    return false;
}


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



var createdProject = "";
var selectedProject = "";
function OnAfterServiceCreate(project) {
    
    OnPerformnaceSelection();
}

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
    debugger;
    menu.style.left = `${origin.left}px`;
    menu.style.top = `${origin.top}px`;
    toggleMenu("show");
}

this.paper.on('element:contextmenu', (node, x, y) => {
    var type = "";
    if (node.model.attributes.attrs.type == "ADHOCROOT") {
        var a = graph.toJSON().cells.filter(function (el) {
            if (el.attrs.refId != null && el.attrs.refId != undefined) {
                var refId = Object.values(el.attrs.refId).join('');
                if (refId == node.model.attributes.attrs.parentId) {
                    console.log(Object.values(el.attrs.type).join(''));
                    return Object.values(el.attrs.type).join('');
                }
            }
        });
        type = Object.values(a[0].attrs.type).join('');
    }
    //alert(type);
    //console.log(node);
    $("#menulist li").remove();
    if (node.model.attributes.attrs.type == 'GOAL_ROOT') {
        $("#menulist").append("<li class='menu-option' onclick='onAddGoal(\"Add Goal\",\"" + selectedPerformanceId + "\");'>Add Goal</li>"); //same template page
    } else if (node.model.attributes.attrs.type == 'COMPENTENCY_ROOT') {
        $("#menulist").append("<li class='menu-option' onclick='onAddCompetency(\"Add Competency\",\"" + selectedPerformanceId + "\");'>Add Competency</li>"); //same template page
    } else if (node.model.attributes.attrs.type == 'DEVELOPMENT_ROOT') {
        $("#menulist").append("<li class='menu-option' onclick='onAddDevelopment(\"Add Goal\",\"" + selectedPerformanceId + "\");'>Add Development</li>"); //same template page
    } else if (node.model.attributes.attrs.type == 'PEERREVIEW_ROOT') {
        $("#menulist").append("<li class='menu-option' onclick='onAddReview(\"Add Competency\",\"" + selectedPerformanceId + "\");'>Add Review</li>"); //same template page
    }
    else if (node.model.attributes.attrs.type == 'ADHOCROOT' && type == "COMPENTENCY") {
        $("#menulist").append("<li class='menu-option' onclick='onAddCompetencyTask(\"Add Task\",\"" + node.model.attributes.attrs.parentId + "\");'>Add Task</li>"); //same template page
    } else if (node.model.attributes.attrs.type == 'ADHOCROOT' && type == "GOAL") {
        $("#menulist").append("<li class='menu-option' onclick='onAddGoalTask(\"Add Task\",\"" + node.model.attributes.attrs.parentId + "\");'>Add Task</li>"); //same template page
    } else if (node.model.attributes.attrs.type == 'ADHOCROOT' && type == "DEVELOPMENT") {
        $("#menulist").append("<li class='menu-option' onclick='onAddDevelopmentTask(\"Add Task\",\"" + node.model.attributes.attrs.parentId + "\");'>Add Task</li>"); //same template page
    } else if (node.model.attributes.attrs.type == 'ADHOCROOT' && type == "PEERREVIEW") {
        $("#menulist").append("<li class='menu-option' onclick='onAddPeerReviewTask(\"Add Task\",\"" + node.model.attributes.attrs.parentId + "\");'>Add Task</li>"); //same template page
    }
    else if (node.model.attributes.attrs.type == "PMS_GOAL_ADHOC_TASK") {
        $("#menulist").append("<li class='menu-option' onclick='onAddSubGoalTask(\"Add Sub Task\",\"" + node.model.attributes.attrs.refId.split("_AdhocTask")[0] + "\");'>Add Sub Task</li>"); //same template page
    }
    else if (node.model.attributes.attrs.type == "PMS_COMPENTENCY_ADHOC_TASK") {
        $("#menulist").append("<li class='menu-option' onclick='onAddSubCompetencyTask(\"Add Sub Task\",\"" + node.model.attributes.attrs.refId.split("_AdhocTask")[0] + "\");'>Add Sub Task</li>"); //same template page
    }
    else if (node.model.attributes.attrs.type == "PMS_DEVELOPMENT_ADHOC_TASK") {
        $("#menulist").append("<li class='menu-option' onclick='onAddSubDocTask(\"Add Sub Task\",\"" + node.model.attributes.attrs.refId.split("_AdhocTask")[0] + "\");'>Add Sub Task</li>"); //same template page
    }
    else if (node.model.attributes.attrs.type == "PMS_REVIEW_TASK") {
        $("#menulist").append("<li class='menu-option' onclick='onAddSubReviewTask(\"Add Sub Task\",\"" + node.model.attributes.attrs.refId.split("_AdhocTask")[0] + "\");'>Add Sub Task</li>"); //same template page
    }

    //else if (node.model.attributes.attrs.type == 'PMS_GOAL_ADHOC_TASK' || node.model.attributes.attrs.type == 'PMS_COMPENTENCY_ADHOC_TASK'
    //    || node.model.attributes.attrs.type == 'PMS_DEVELOPMENT_ADHOC_TASK' || node.model.attributes.attrs.type == 'PMS_REVIEW_TASK') {
    //    $("#menulist").append("<li class='menu-option' onclick='onAddSubTask(\"Add Task\",\"" + node.model.attributes.attrs.refId + "\");'>Add Sub Task</li>"); //same template page
    //}

    //else if (node.model.attributes.attrs.type == "PMS_GOAL_ADHOC_TASK") {
    //    $("#menulist").append("<li class='menu-option' onclick='onTask(" + node.model.attributes.attrs.refId + "," + node.model.attributes.attrs.type + ")>Edit Task</li>"); //same template page
    //} else if (node.model.attributes.attrs.type == "PMS_COMPENTENCY_ADHOC_TASK") {

    //}

    var origin = {
        left: x.pageX,
        top: x.pageY
    };
    setPosition(origin);

    return false;
});

//function onAddSubTask(title, id) {
//    var prms = encodeURIComponent('parentServiceId=' + selectedPerformanceId + '&parentTaskId=' + id);
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
    var prms = encodeURIComponent('parentServiceId=' + selectedPerformanceId + '&parentTaskId=' + id);
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&templateCodes=PMS_GOAL_ADHOC_TASK&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 800, Height: 600 });
    return false;
}

function onAddSubCompetencyTask(title, id) {
    var prms = encodeURIComponent('parentServiceId=' + selectedPerformanceId + '&parentTaskId=' + id);
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&templateCodes=PMS_COMPENTENCY_ADHOC_TASK&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 800, Height: 600 });
    return false;
}

function onAddSubDocTask(title, id) {
    var prms = encodeURIComponent('parentServiceId=' + selectedPerformanceId + '&parentTaskId=' + id);
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&templateCodes=PMS_DEVELOPMENT_ADHOC_TASK&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 800, Height: 600 });
    return false;
}


function onAddSubReviewTask(title, id) {
    var prms = encodeURIComponent('parentServiceId=' + selectedPerformanceId + '&parentTaskId=' + id);
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
    var prms = encodeURIComponent('parentServiceId=' + selectedPerformanceId);
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=PerformanceGoal&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 1200, Height: 600 });
    return false;
}


function onAddDevelopment(title, id) {
    var prms = encodeURIComponent('parentServiceId=' + selectedPerformanceId);
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=PerformanceDevelopment&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 1200, Height: 600 });
    return false;
}

function onAddReview(title, id) {
    var prms = encodeURIComponent('parentServiceId=' + selectedPerformanceId);
    var url = '/Cms/Page?lo=Popup&cbm=OnAfterServiceCreate&source=Create&dataAction=Create&pageName=PerformancePeerReview&portalId=' + portalId + '&prms=' + prms;
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 1200, Height: 600 });
    return false;
}

function onAddCompetency(title, id) {
    var prms = encodeURIComponent('parentServiceId=' + selectedPerformanceId);
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
    selectedPerformanceId = $("#performance").data("kendoDropDownList").value();// $("#performance").val();
    createPerformaceDiagram(selectedPerformanceId);
   
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

