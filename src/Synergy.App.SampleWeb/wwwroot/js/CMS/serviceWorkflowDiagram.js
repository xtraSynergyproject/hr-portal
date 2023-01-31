var isloadedNote = false;
var isloadedTask = false;
var isloadedService = false;
var selectedPortalId = "";
var selectedTemplateType = "";
var lastClickedElementView;
var currentSelectedElementView;
var currentParentNode = "";
var portalId = $('#GlobalPortalId').val();
selectedPortalId = portalId;
var SWIMLANE_WIDTH = 300;
var SWIMLANE_HEIGHT = 600;

// Canvas where sape are dropped
var graph = new joint.dia.Graph,
    paper = new joint.dia.Paper({
        el: document.getElementById('paper'),
        model: graph,
        width: 1000,
        height: 600,
        gridSize: 10,
        interactive: true,
        async: true,
        frozen: true,
        //sorting: new joint.dia.Paper.sorting.APPROX,
        background: { color: '#F3F7F6' },
        defaultConnectionPoint: { name: 'boundary', args: { stroke: true } }
    });


//prop
var $sx = $('#sx');
var $sy = 0;
var $w = $('#width');
var $h = $('#height');
var draggedElement;



$(document).ready(function () {
    selectedPortalId = portalId;

    //selectedPortalId = document.getElementById("portalId").getAttribute("value");
    //if (selectedPortalId != null) {
    //    createWorkStructureDiagram(selectedPortalId);
    //}
    //$("#Portal").data("kendoDropDownList").value(selectedPortalId);
});

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
// Add remove button to the link.
var linktools = new joint.dia.ToolsView({
    tools: [new joint.linkTools.Remove()]
});

var xp = 100;
var yp = 100;
var type = '';
var parent = '';
var parentVal;
var dataLib = [];
function createChild(child, result, parent) {
    xp = 50

    //for (var i = 0; i < child.length; i++) {
    var ab = child.map(ch => {
        if (ch.Type == "PORTAL") {

            var node = createNode(ch, result, parent, "#e57373");
            graph.addCell(node);

            if (parentVal != ch.ParentId) {
                xp = 50;
                type = ch.Type;
                parentVal = ch.ParentId;
            }
            xp = xp + 150;
            createlink(parent, node);
            dataLib.push({ id: ch.Id, cell: node });
        }
        else {

            var node = createNode(ch, result, parent, "#ff8a80");
            //var el10 = node.clone().set('group', 0);
            graph.addCell(node, parent);

            if (parentVal != ch.ParentId) {
                type = ch.Type;
                parentVal = ch.ParentId;
            }
            xp = xp + 150;

            createlink(parent,node);
            dataLib.push({ id: ch.Id, cell: node });
        }

        joint.layout.DirectedGraph.layout(graph, {
            setLinkVertices: false, nodeSep: 50,
            edgeSep: 100,
            rankDir: "TR",
            dagre: dagre,
            graphlib: dagre.graphlib,
            marginX: 100,
            marginY: 50
        });
        //}
    });
    var a = child.map(ch => {

        var chs = result.filter(x => x.ParentId == ch.Id);
        var pt = dataLib.filter(x => x.id == ch.Id)[0].cell;
        createChild(chs, result, pt);

    });
    //for (var i = 0; i < child.length; i++) {
    //    var chs = result.filter(x => x.ParentId == child[i].Id);
    //    var pt = dataLib.filter(x => x.id == child[i].Id)[0].cell;
    //    createChild(chs, result, pt);
    //}
    roundedCornerRect();
    graphLayout();
}

function createNode(obj, result, parent, color) {

    var chs = result.filter(x => x.ParentId == obj.Id);
    var isHavingChild = chs.length > 0 ? true : false;
    var collapsedPath;
    if (parent) {
        collapsedPath = parent.attributes.attrs.collapsedPath;
    } else {
        collapsedPath = "";
    }


    var title = "TITLE";
    if (obj.Name.length > 20) {
        title = obj.Name.substring(0, 20);
    } else {
        title = obj.Name;
    }


    var node = new joint.shapes.standard.HeaderedRectangle();
    node.resize(180, 100);
    node.position(xp, yp);
    node.attr('root/tabindex', 12);
    node.attr('root/title', obj.Name);
    node.attr('header/fill', color);
    node.attr('header/fillOpacity', 0.5);
    node.attr('headerText/text', title);
    node.attr('headerText/fontSize', 13);
    node.attr('body/fillOpacity', 0.5);
    node.attr('body/fontSize', 10);
    node.attr('bodyText/fontSize', 13);
    node.attr('collapsed', false);
    node.attr('collapsedPath', collapsedPath + node.id + "|");
    node.attr('type', obj.Type);
    node.attr('predeccessor', null);
    node.attr('isHavingChild', isHavingChild);
    node.attr('pNodeId', parent ? parent.id : null);
    node.attr('refId', obj.Id);
    pushNodeDrp(title, node.id);
    return node;
}

var nodesdrp = [];
function pushNodeDrp(title, id) {
    nodesdrp.push({ text: title, value: id });
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
createWorkStructureDiagram("");
function createWorkStructureDiagram(id) {
    graph.clear();
    ShowLoader($('#dcontent'));
    currentParentNode = id;
    $.ajax({
        url: '/cms/ntsService/GetStepTaskListByServiceId?id=2ba636ac-99f0-4683-8014-acd8e0dee405', ///+ id,// + id,
        dataType: "json",
        success: function (result) {
            

            var x = 20, y = 10;
            selectedPortalId = id;
            var parentNode = result.filter(x => x.Type == "SERVICE");
            var childList = [];

            childList.push(parentNode[0]);

            createChild(childList.filter(x => x != undefined), result, null);
            moveCenter();
            setNodeDrp();
            HideLoader($('#dcontent'));
        }
    });
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
    var arr = Array.prototype.slice.call(ele)
    //for (var x = 0; x <= ele.length - 1; x++) {
    //    ele[x].setAttribute("rx", 5);
    //    ele[x].setAttribute("ry", 5);
    //}

    var a = arr.map(e => {
        e.setAttribute("rx", 5);
        e.setAttribute("ry", 5);
    });
}


function loadDiagram(id) {
    createWorkStructureDiagram(id);
    setTimeout(function () {
        graphLayout();

    }, 3000);
}

function onRefreshDiagram() {
    if (selectedPortalId != "") {
        createWorkStructureDiagram(selectedPortalId);
        setTimeout(function () { graphLayout(); }, 3000);
    } else {
        alert("Please select user.");
    }
}


this.paper.on('link:mouseenter', function (linkView) {
    //linkView.addTools(linktools);
});

this.paper.on('link:mouseleave', function (linkView) {
    //linkView.removeTools();
});

function refreshDiagram(node) {
    graph.clear();
    graph.addCell(node);
    createWorkStructureDiagram(node);
}


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
        //alert("cell");
    },
    'element:pointerclick': function (elementView) {
        //alert("dsd");
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
        
        //if (elementView.model.attributes.attrs.type == "standard") {
        //    document.getElementById("prop").style.display = "";
        //} else {
        //    document.getElementById("prop").style.display = "none";
        //}
        
        onNodeClick(elementView.model.attributes.attrs.refId, elementView.model.attributes.attrs.type)
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

            if (elementBelow && graph.getNeighbors(elementBelow).indexOf(elementAbove) === -1) {
                if (elementBelow.attributes.attrs.type == "WF_STOP") {

                }
                else {

                    if (elementAbove.attributes.attrs.type == "WF_START" && (elementBelow.attributes.attrs.type == "WF_STOP"
                        || elementBelow.attributes.attrs.type == "WF_DECISION" || elementBelow.attributes.attrs.type == "WF_STEP_TASK"
                        || elementBelow.attributes.attrs.type == "WF_START" || elementBelow.attributes.attrs.type == "WF_DECISION_TRUE" || elementBelow.attributes.attrs.type == "WF_DECISION_FALSE")) {

                    }
                    else {
                        elementAbove.position(evt.data.x, evt.data.y);
                        // Create a connection between elements.
                        var link = new joint.shapes.standard.Link();
                        link.source(elementBelow);
                        link.target(elementAbove);
                        link.attr('type', "standard");
                        link.addTo(graph);

                        var col = elementBelow.attributes.attrs.collapsedPath + "|" + link.id;
                        elementView.model.attributes.attrs.collapsedPath = col;
                        // Add remove button to the link.
                        var tools = new joint.dia.ToolsView({
                            tools: [new joint.linkTools.Remove()]
                        });
                        link.findView(this).addTools(tools);
                    }

                }
                //joint.layout.DirectedGraph.layout(graph, {
                //    setLinkVertices: false, nodeSep: 50,
                //    edgeSep: 80,
                //    rankDir: "TB",
                //    dagre: dagre,
                //    graphlib: dagre.graphlib,
                //    marginX: 100,
                //    marginY: 50
                //});

            }

        }
    }
});
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
//var removeButton = new joint.elementTools.Remove();
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
joint.elementTools.removeButton = joint.elementTools.Button.extend({
    name: 'remove-button',
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
                id: "removeimage",
                width: "20",
                height: "20",
                'object-fit': 'fill',
                href: "https://img.icons8.com/cotton/2x/delete-sign--v2.png",
                'clip-path': "url(#myCircle)"
            },

        }],
        x: '100%',
        y: '100%',
        offset: {
            x: -150,
            y: -20
        },
        rotate: true,
        action: function (evt) {
            console.log(evt);
            
            //.remove(); 
            //toggleCollapse(this);
        }
    }
});
var removeButton = new joint.elementTools.removeButton();

var toolsViewExpand = new joint.dia.ToolsView({
    tools: [
        boundaryTool,
        //removeButton,
        expandButton,
        //removeButton
    ]
});
var onlyBasicTool = new joint.dia.ToolsView({
    tools: [
        boundaryTool,
        //removeButton,
        // removeButton
    ]
});


var roots = [];
function list_to_tree() {

    var list = graph.getElements();

    var dataList = [];

    list.map(x => {

        var obj = { text: x.attributes.attrs.root.title, value: x.id }
        dataList.push(obj);
    });
    var map = {}, node, i;

    for (i = 0; i < dataList.length; i += 1) {
        map[dataList[i].value] = i; // initialize the map
        dataList[i].items = []; // initialize the children
    }
    
    for (i = 0; i < dataList.length; i += 1) {
        node = dataList[i];
        var a = list.filter(x => x.id == node.value)[0];
        if (a.attributes.attrs.pNodeId !== null) {
            dataList[map[a.attributes.attrs.pNodeId]].items.push(node);
        } else {
            roots.push(node);
        }
    }
    console.log("root");
    console.log(roots);
}


function setNodeDrp() {
    roots = [];
    list_to_tree();
    $("#nodeDrpList").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: nodesdrp,
        index: 0,
    });

    
    $("#nodeDrp").kendoDropDownTree({
        placeholder: "Select ...",
        dataTextField: "text",
        dataValueField: "value",
        dataSource: roots,
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


function onChangeTreeView() {
    var treeValue = $("#nodeDrp").data("kendoDropDownTree").value();
    $("#nodeDrpList").data("kendoDropDownList").value(treeValue);
    $("#movebtn").show();
}




function onChangeNodeFocus() {
    
    $("g").removeClass("nodeHighlight");
    var value = $("#nodeDrpList").val();
    var id = $("g").find("[model-id='" + value + "']")[0].id;
    document.getElementById(id).scrollIntoView({ behavior: "smooth", block: "center", inline: "center" });
    var element = document.getElementById(id);
    element.classList.add("nodeHighlight");
}


function moveCenter() {
    var div = document.getElementById("paper");
    div.scrollIntoView({
        behavior: "smooth",
        block: "center",
        inline: "start"
    });
}


function OnPortalSelection() {
    selectedPortalId = $("#Portal").data("kendoDropDownList").value();
    createWorkStructureDiagram(selectedPortalId);
}

function onNodeClick(id, type) {
    
   
}

function openPopup(url, title) {
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 1200, Height: 600 });
    return false;
}

function OnAfterNoteCreate(data) {

}

function onPortalDigram() {

}

const menu = document.querySelector(".menu");
const menuOption = document.querySelector(".menu-option");
let menuVisible = false;


this.paper.on('element:contextmenu', (node, x, y) => {
    $("#menulist li").remove();
    if (node.model.attributes.attrs.type == 'PORTAL') {
        $("#menulist").append("<li class='menu-option' onclick='onEditPortal(\"Edit Portal\",\"" + node.model.attributes.attrs.refId + "\");'>Edit Portal</li>"); //same template page
        $("#menulist").append("<li class='menu-option' onclick='onCreateMenuGroup(\"Create Menu Group\",\"" + node.model.attributes.attrs.refId + "\");'>Create Menu Group</li>"); //same template page
    }

   
    const origin = {
        left: x.pageX,
        top: x.pageY
    };
    setPosition(origin);

    return false;
});

function setPosition(origin) {
    menu.style.left = `${origin.left}px`;
    menu.style.top = `${origin.top}px`;
    toggleMenu("show");
}

function toggleMenu(command) {
    menu.style.display = command === "show" ? "block" : "none";
    menuVisible = !menuVisible;
}

window.addEventListener("click", e => {
    if (menuVisible) toggleMenu("hide");
});

function openPopup(url, title) {
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: title, Width: 1200, Height: 600 });
    return false;
}
