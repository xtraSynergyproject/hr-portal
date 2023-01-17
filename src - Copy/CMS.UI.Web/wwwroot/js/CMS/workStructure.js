var isloadedNote = false;
var isloadedTask = false;
var isloadedService = false;
var selectedPersonId = "";
var selectedTemplateType = "";
var lastClickedElementView;
var currentSelectedElementView;
var currentParentNode = "";
var portalId = $('#GlobalPortalId').val();

// Canvas where sape are dropped
var graph = new joint.dia.Graph,
    paper = new joint.dia.Paper({
        el: $('#paper'),
        model: graph,
        height: 700,
        width: 3000
    });


//prop
var $sx = $('#sx');
var $sy = 0;
var $w = $('#width');
var $h = $('#height');
var draggedElement;

selectedPersonId = document.getElementById("loggedInPersonId").getAttribute("value");
if (selectedPersonId != null) {
    createWorkStructureDiagram(selectedPersonId);
}

$(document).ready(function () {
    $("#Person").data("kendoDropDownList").value(selectedPersonId);
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


function createChildNode(childList, x, y, result, parent) {


    for (var i = 0; i < childList.length; i++) {


        var subchild = result.filter(x => x.ParentId == childList[i].Id);

        var childCount = subchild.filter(x => x != undefined).length;

        var isHavingChild = subchild.filter(x => x != undefined).length > 0 ? true : false;



        var wraptext = ""
        if (childList[i].Title != null && childList[i].Title != "") {
            childList[i].Title = childList[i].Title; //+ " (" + childCount + ")";
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

        if (childList[i].Type == "ROOT_USER") {
            bcolor = '#00b4d8';
            hcolor = 'white';
        } else if (childList[i].Type == "USER_DATA") {
            bcolor = '#00b4d8';
            hcolor = 'white';
        } else if (childList[i].Type == "USER_ASSIGNMENT") {
            bcolor = '#00b4d8';
            hcolor = 'white';
        } else if (childList[i].Type == "USER_CONTRACT") {
            bcolor = '#00b4d8';
            hcolor = 'white';
        } else if (childList[i].Type == "USER_SALARYINFO") {
            bcolor = '#00b4d8';
            hcolor = 'white';
        } else if (childList[i].Type == "CONTRACT_SPONSER") {
            bcolor = '#aacc00';
            hcolor = 'white';
        } else if (childList[i].Type == "ASSIGNMENT_JOB") {
            bcolor = '#aacc00';
            hcolor = 'white';
        } else if (childList[i].Type == "ASSIGNMENT_JOB_GRADE") {
            bcolor = '#aacc00';
            hcolor = 'white';
        } else if (childList[i].Type == "ASSIGNMENT_DEPARTMENT") {
            bcolor = '#aacc00';
            hcolor = 'white';
        } else if (childList[i].Type == "ASSIGNMENT_DEPARTMENT_COSTCENTER") {
            bcolor = '#aacc00';
            hcolor = 'white';
        } else if (childList[i].Type == "ASSIGNMENT_LOCATION") {
            bcolor = '#aacc00';
            hcolor = 'white';
        } else if (childList[i].Type == "ASSIGNMENT_POSITION") {
            bcolor = '#aacc00';
            hcolor = 'white';
        } else if (childList[i].Type == "USER_DATA_LAH_YES") {
            bcolor = '#aacc00';
            hcolor = 'white';
        } else if (childList[i].Type == "USER_DATA_LAH_NO") {
            bcolor = 'grey';
            hcolor = 'white';
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
            node.attr('body/fill', bcolor);
            node.attr('body/fillOpacity', 0.5);
            node.attr('body/fontSize', 10);
            node.attr('bodyText/text', wraptextD);
            node.attr('bodyText/fontSize', 13);
            node.attr('refId', childList[i].ReferenceId);
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
            node.attr('header/fill', bcolor);
            node.attr('header/fillOpacity', 0.5);
            node.attr('headerText/text', wraptext);
            node.attr('body/fill', bcolor);
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
//createWorkStructureDiagram("");
function createWorkStructureDiagram(id) {
    graph.clear();
    ShowLoader($('#dcontent'));
    currentParentNode = id;
    $.ajax({
        url: '/chr/workStructure/GetWorkStructureDiagram?personId=' + id,// + id,
        dataType: "json",
        success: function (result) {

          
            var x = 20, y = 10;
            selectedPersonId = id;
            var parentNode = result.filter(x => x.Type == "ROOT_USER");
            var childList = [];

            childList.push(parentNode[0]);

            createChildNode(childList.filter(x => x != undefined), x, y, result, null);
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
    for (var x = 0; x <= ele.length - 1; x++) {
        ele[x].setAttribute("rx", 5);
        ele[x].setAttribute("ry", 5);
    }
}


function loadDiagram(id) {
    createWorkStructureDiagram(id);
    setTimeout(function () {
        graphLayout();

    }, 3000);
}

function onRefreshDiagram() {
    if (selectedPersonId != "") {
        createWorkStructureDiagram(selectedPersonId);
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
        if (a.attributes.attrs.parentId !== null) {
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


function OnPersonSelection() {
    selectedPersonId = $("#Person").data("kendoDropDownList").value();
    createWorkStructureDiagram(selectedPersonId);
}

function onNodeClick(id, type) {
    url = "";
    if (id != null) {
        if (type == "USER_DATA") {
            url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterNoteCreate&source=View&dataAction=View&templateCodes=HRPerson&portalId=' + portalId + '&recordId=' + id;
            openPopup(url, "Person Details");
        } else if (type == "USER_ASSIGNMENT") {
            url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterNoteCreate&source=View&dataAction=View&templateCodes=HRAssignment&portalId=' + portalId + '&recordId=' + id;
            openPopup(url, "Assignments Details");
        }

        else if (type == "USER_SALARYINFO") {
            url = "/pay/salaryInfo/Create?salaryInfoId=" + id;
            openPopup(url, "Salary Details");
        }

        else if (type == "USER_CONTRACT") {
            url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterNoteCreate&source=View&dataAction=View&templateCodes=HRContract&portalId=' + portalId + '&recordId=' + id;
            openPopup(url, "Contract Details");
        }

        //else if (type == "USER_DATA_LAH_YES") {
        //    hid = id.split("$$")[1];
        //    id = id.split("$$")[0];
        //    url = '/CHR/HRDirect/LeaveApproveHierarchyByUserId?userId=' + id + "&hierarchyId=" + hid;
        //    openPopup(url, "Approval Details");
        //}

        //else if (type == "ASSIGNMENT_JOB") {
        //    url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterNoteCreate&source=View&dataAction=View&templateCodes=HRJob&portalId=' + portalId + '&recordId=' + id;
        //} else if (type == "ASSIGNMENT_DEPARTMENT") {
        //    url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterNoteCreate&source=View&dataAction=View&templateCodes=HRDepartment&portalId=' + portalId + '&recordId=' + id;
        //} else if (type == "ASSIGNMENT_LOCATION") {
        //    url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterNoteCreate&source=View&dataAction=View&templateCodes=HRLocation&portalId=' + portalId + '&recordId=' + id;
        //} else if (type == "ASSIGNMENT_POSITION") {
        //    url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterNoteCreate&source=View&dataAction=View&templateCodes=HRPosition&portalId=' + portalId + '&recordId=' + id;
        //} else if (type == "ASSIGNMENT_JOB_GRADE") {.js

        //    url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterNoteCreate&source=View&dataAction=View&templateCodes=HRGrade&portalId=' + portalId + '&recordId=' + id;
        //}
       
    } else {
        if (type == "USER_DATA") {
            url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterNoteCreate&source=View&dataAction=View&templateCodes=HRPerson&portalId=' + portalId + '&recordId=' + id;
            openPopup(url, "Person Details");
        } else if (type == "USER_ASSIGNMENT") {
            url = '/Cms/Page?popup=true&lo=Popup&cbm=OnAfterNoteCreate&source=create&dataAction=create&templateCodes=HRAssignment&portalId=' + portalId + '&recordId=' + id;
            openPopup(url, "Assignment Details");
        }
       
    }
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