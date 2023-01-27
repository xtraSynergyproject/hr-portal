
// Canvas where sape are dropped
var graph = new joint.dia.Graph,
    paper = new joint.dia.Paper({
        el: $('#paper'),
        model: graph,
        height: 9000,
        width: 7000
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

//var stheaderedRectangle = new joint.shapes.standard.HeaderedRectangle();
//stheaderedRectangle.resize(80, 80);
//stheaderedRectangle.position(20, 10);
//stheaderedRectangle.attr('root/tabindex', 12);
//stheaderedRectangle.attr('root/title', 'Stage');
//stheaderedRectangle.attr('header/fill', '#6c119c');
//stheaderedRectangle.attr('header/fillOpacity', 0.5);
//stheaderedRectangle.attr('headerText/text', 'Stage');
//stheaderedRectangle.attr('body/fillOpacity', 0.5);
//stheaderedRectangle.attr('body/fontSize', 10);
//stheaderedRectangle.attr('bodyText/fontSize', 13);

//var theaderedRectangle = new joint.shapes.standard.HeaderedRectangle();
//theaderedRectangle.resize(80, 80);
//theaderedRectangle.position(20, 100);
//theaderedRectangle.attr('root/tabindex', 12);
//theaderedRectangle.attr('root/title', 'Task');
//theaderedRectangle.attr('header/fill', '#008bde');
//theaderedRectangle.attr('header/fillOpacity', 0.5);
//theaderedRectangle.attr('headerText/text', 'Task');
//theaderedRectangle.attr('body/fillOpacity', 0.5);
//theaderedRectangle.attr('body/fontSize', 10);
//theaderedRectangle.attr('bodyText/fontSize', 13);

//var subtheaderedRectangle = new joint.shapes.standard.HeaderedRectangle();
//subtheaderedRectangle.resize(80, 80);
//subtheaderedRectangle.position(20, 190);
//subtheaderedRectangle.attr('root/tabindex', 12);
//subtheaderedRectangle.attr('root/title', 'Sub Task');
//subtheaderedRectangle.attr('header/fill', '#cced00');
//subtheaderedRectangle.attr('header/fillOpacity', 0.5);
//subtheaderedRectangle.attr('headerText/text', 'Sub Task');
//subtheaderedRectangle.attr('body/fillOpacity', 0.5);
//subtheaderedRectangle.attr('body/fontSize', 10);
//subtheaderedRectangle.attr('bodyText/fontSize', 13);

//// Stencils-------------------------------------------------------------------------
//stencilGraph.addCells([stheaderedRectangle, theaderedRectangle, subtheaderedRectangle]);


   

////Tools------------------------------------------------------------------------

//joint.elementTools.collapseButton = joint.elementTools.Button.extend({
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
//                attributes: {
//                id: "expandCollapseImage",
//                width: "20",
//                height: "20",
//                'object-fit': 'fill',
//                    href: "https://garance-beyrouth.com/wp-content/uploads/2020/05/plus-minus-icon-png-8-original.png",//"http://pngimg.com/uploads/plus/small/plus_PNG95.png",
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

var boundaryTool = new joint.elementTools.Boundary();
var removeButton = new joint.elementTools.Remove();

//var toolsView = new joint.dia.ToolsView({
//    tools: [
//        boundaryTool,
//        removeButton,
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
//        removeButton,
//        expandButton
//    ]
//});

var onlyBasicTool = new joint.dia.ToolsView({
    tools: [
        boundaryTool,
        removeButton,
    ]
});

/////

////events---------------------------------------------------------------------------------



var lastClickedElementView;
var currentSelectedElementView;

this.paper.on('blank:pointerdown', (evt, x, y) => {
    //this.paperScroller.startPanning(evt);
});

paper.on('link:mouseenter', function (linkView) {
    linkView.addTools(linktools);
});

paper.on('link:mouseleave', function (linkView) {
    linkView.removeTools();
});
paper.on({
    
    'element:mouseenter': function (elementView) {      
        console.log(elementView);
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
    },
    'element:pointerclick': function (elementView) {
        debugger
        //document.getElementById("myForm").style.display = "block";
        //if (lastClickedElementView != undefined) {
        //    joint.highlighters.mask.remove(lastClickedElementView);
        //}

        //currentSelectedElementView = elementView;

        //var json = graph.toJSON();
        //cells = json.cells;
        //for (var i = 0; i <= cells.length - 1; i++) {
        //    if (cells[i].type != "standard.Link") {
        //        if (linkData.filter(e => e.value === cells[i].id).length == 0) {
        //            linkData.push({ text: cells[i].attrs.headerText.text, value: cells[i].id })
        //        }
        //    }
        //}

        //var index = linkData.findIndex(e => e.value === elementView.model.id);
        //linkData.splice(index, 1);

        //setPredAndSucc();

        //joint.highlighters.mask.add(elementView, { selector: 'root' }, 'my-element-highlight', {
        //    deep: true,
        //    attrs: {
        //        'stroke': '#FF4365',
        //        'stroke-width': 3,
        //        'padding': 10
        //    }
        //});
        //if (elementView.model.attributes.attrs.headerText.text != "Project" && elementView.model.attributes.attrs.headerText.text != "Stage"
        //    && elementView.model.attributes.attrs.headerText.text != "Task" && elementView.model.attributes.attrs.headerText.text != "Sub Task") {
        //    var el = elementView.model.attributes.attrs;
        //    var data = el.root.title.split("\n");
        //    var title = data[0].trim();
        //    var owner = data[2].split(":")[1] ? data[2].split(":")[1].trim() : '';
        //    var sdate = data[4].split("S:")[1].trim();
        //    var edate = data[5].split("E:")[1].trim();
        //    var assignee = data[9].split("Assignee:")[1].trim();

        //    var userData = $('#OwnerUserId').data('kendoDropDownList').dataSource.data();
        //    var oUserId = userData.filter(x => x.Name == owner);
        //    var aUserId = userData.filter(x => x.Name == assignee);

        //    var svalue = new Date(Date.parse(sdate));
        //    var evalue = new Date(Date.parse(edate));

        //    $('#Start').data('kendoDateTimePicker').value(svalue);
        //    $('#End').data('kendoDateTimePicker').value(evalue);
        //    $('#Title').data('kendoTextBox').value(title);
        //    if (oUserId[0]) {
        //        $('#OwnerUserId').data('kendoDropDownList').value(oUserId[0].Id);
        //    }
        //    if (aUserId[0]) {
        //        $('#AssigneeUserId').data('kendoDropDownList').value(aUserId[0].Id);
        //    }
        //}

        //lastClickedElementView = elementView;
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
            elementView.showTools();
        }
    },
    'element:pointerdown': function (elementView, evt) {
        debugger
        evt.data = elementView.model.position();
    },
    'element:button:pointerdown': function (elementView, evt) {
        debugger
        evt.stopPropagation(); // stop any further actions with the element view (e.g. dragging)

        var model = elementView.model;

        if (model.attr('body/visibility') === 'visible') {
            model.attr('body/visibility', 'hidden');
            model.attr('label/visibility', 'hidden');
            model.attr('buttonLabel/text', 'ï¼‹'); // fullwidth plus

        } else {
            model.attr('body/visibility', 'visible');
            model.attr('label/visibility', 'visible');
            model.attr('buttonLabel/text', 'ï¼¿'); // fullwidth underscore
        }
    },
    'element:pointerup': function (elementView, evt, x, y) { 
        debugger
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

        //$('#Start').data('kendoDateTimePicker').value("");
        //$('#End').data('kendoDateTimePicker').value("");
        //$('#Title').data('kendoTextBox').value("");
        //$('#OwnerUserId').data('kendoDropDownList').text('Select');
        //$('#AssigneeUserId').data('kendoDropDownList').text('Select');

        if (elementBelow && elementAbove) {
            if ((elementBelow.attributes.attrs.header.fill == "#89b0d8" && elementAbove.attributes.attrs.header.fill == "#6c119c") ||
                (elementBelow.attributes.attrs.header.fill == "#89b0d8" && elementAbove.attributes.attrs.header.fill == "#008bde") ||
                (elementBelow.attributes.attrs.header.fill == "#6c119c" && elementAbove.attributes.attrs.header.fill == "#008bde") ||
                (elementBelow.attributes.attrs.header.fill == "#008bde" && elementAbove.attributes.attrs.header.fill == "#cced00") ||
                (elementBelow.attributes.attrs.header.fill == "#cced00" && elementAbove.attributes.attrs.header.fill == "#cced00")) {

                if (elementBelow && graph.getNeighbors(elementBelow).indexOf(elementAbove) === -1) {

                    elementAbove.position(evt.data.x, evt.data.y);

                    // Create a connection between elements.
                    var link = new joint.shapes.standard.Link();
                    link.source(elementBelow);
                    link.target(elementAbove);
                    link.addTo(graph);

                    // Add remove button to the link.
                    var tools = new joint.dia.ToolsView({
                        tools: [new joint.linkTools.Remove()]
                    });
                    link.findView(this).addTools(tools);
                }
            }
            else {
                elementAbove.position(evt.data.x, evt.data.y);
            }
        }
    }
});

//function createlink(parent, child) {

//    if (parent && child) {
//        // Create a connection between elements.
//        var link = new joint.shapes.standard.Link();
//        link.source(parent);
//        link.target(child);
//        link.attr('collapsedPath', parent.attributes.attrs.collapsedPath + "|" + link.id);
//        link.router('manhattan');
//        link.connector('jumpover');
//        link.addTo(graph);
//    }
//}

// Add remove button to the link.
var linktools = new joint.dia.ToolsView({
    tools: [new joint.linkTools.Remove()]
});

var xp = 100;
var yp = 100;
var type = '';
var parent = '';

//var project = new joint.shapes.standard.HeaderedRectangle();
//project.resize(180, 180);
//project.position(250, 10);
//project.attr('root/tabindex', 12);
//project.attr('root/title', "Project");
//project.attr('header/fill', '#89b0d8');
//project.attr('header/fillOpacity', 0.5);
//project.attr('headerText/text', "Project");
//project.attr('body/fillOpacity', 0.5);
//project.attr('body/fontSize', 10);
//project.attr('bodyText/fontSize', 13);
//project.attr('refId', '');
//project.attr('collapsed', false);
//graph.addCell(project);

//var parentVal;
//var dataLib = [];
//function createChild(child, result, parent) {
//    xp = 50

//    for (var i = 0; i < child.length; i++) {

//        var title = child[i].Title;
//        if (title.length > 20) title = title.substring(0, 20);

//        var mainTitle = child[i].Title + "\n\nOwner: " + child[i].UserName + '\n\nS:   ' + child[i].Start + '\nE: ' + child[i].End + '\n\nStatus: Draft\n\nAssignee: ' + child[i].UserName;

//        var chs = result.filter(x => x.ParentId == child[i].Id);
//        var isHavingChild = chs.length > 0 ? true : false;

//        if (child[i].Type == "PROJECT") {

//            var sheaderedRectangle = new joint.shapes.standard.HeaderedRectangle();
//            sheaderedRectangle.resize(120, 120);
//            sheaderedRectangle.position(xp, yp);
//            sheaderedRectangle.attr('root/tabindex', 12);
//            sheaderedRectangle.attr('root/title', mainTitle);
//            sheaderedRectangle.attr('header/fill', '#89b0d8');
//            sheaderedRectangle.attr('header/fillOpacity', 0.5);
//            sheaderedRectangle.attr('headerText/text', child[i].Title);
//            sheaderedRectangle.attr('body/fill', '#ffafb1');
//            sheaderedRectangle.attr('body/fillOpacity', 0.5);
//            sheaderedRectangle.attr('body/fontSize', 10);
//            sheaderedRectangle.attr('bodyText/text', 'S: ' + formatDate(child[i].Start) + '\nE: ' + formatDate(child[i].End) + '\n\nStatus: ' + child[i].NtsStatus);
//            sheaderedRectangle.attr('bodyText/fontSize', 13);
//            sheaderedRectangle.attr('bodyText/color', '#FF0000');
//            sheaderedRectangle.attr('refId', child[i].Id);
//            sheaderedRectangle.attr('collapsed', false);
//            sheaderedRectangle.attr('collapsedPath', parent.attributes.attrs.collapsedPath + "|");
//            sheaderedRectangle.attr('type', child[i].Type);
//            sheaderedRectangle.attr('predeccessor', null);
//            projectN.attr('isHavingChild', isHavingChild);
//            graph.addCell(sheaderedRectangle);

//            if (parentVal != child[i].ParentId) {
//                xp = 50;
//                type = child[i].Type;
//                parentVal = child[i].ParentId;
//            }
//            xp = xp + 150;
//            createlink(parent.id, sheaderedRectangle.id);
//            dataLib.push({ id: child[i].Id, cell: sheaderedRectangle });
//        } else if (child[i].Type == "STAGE") {

//            var stheaderedRectangle = new joint.shapes.standard.HeaderedRectangle();
//            stheaderedRectangle.resize(180, 180);
//            stheaderedRectangle.position(xp, yp);
//            stheaderedRectangle.attr('root/tabindex', 12);
//            stheaderedRectangle.attr('root/title', mainTitle);
//            stheaderedRectangle.attr('header/fill', '#6c119c');
//            stheaderedRectangle.attr('header/fillOpacity', 0.5);
//            stheaderedRectangle.attr('headerText/text', title);
//            stheaderedRectangle.attr('body/fillOpacity', 0.5);
//            stheaderedRectangle.attr('body/fontSize', 10);
//            stheaderedRectangle.attr('bodyText/text', 'S: ' + formatDate(child[i].Start) + '\nE: ' + formatDate(child[i].End) + '\n\nStatus: ' + child[i].NtsStatus + "\nCount: " + chs.length);
//            stheaderedRectangle.attr('bodyText/fontSize', 13);
//            stheaderedRectangle.attr('refId', child[i].Id);
//            stheaderedRectangle.attr('collapsed', false);
//            stheaderedRectangle.attr('type', child[i].Type);
//            stheaderedRectangle.attr('predeccessor', null);
//            projectN.attr('isHavingChild', isHavingChild);
//            var cp = Object.values(stheaderedRectangle.id).join('');

//            stheaderedRectangle.attr('collapsedPath', parent.attributes.attrs.collapsedPath + cp + "|");

//            graph.addCell(stheaderedRectangle);

//            if (parentVal != child[i].ParentId) {
//                type = child[i].Type;
//                parentVal = child[i].ParentId;
//            }
//            xp = xp + 150;
//            createlink(parent, stheaderedRectangle);
//            dataLib.push({ id: child[i].Id, cell: stheaderedRectangle });
//        } else if (child[i].Type == "TASK") {

//            var theaderedRectangle = new joint.shapes.standard.HeaderedRectangle();
//            theaderedRectangle.resize(180, 180);
//            theaderedRectangle.position(xp, yp);
//            theaderedRectangle.attr('root/tabindex', 12);
//            theaderedRectangle.attr('root/title', mainTitle);
//            theaderedRectangle.attr('header/fill', '#008bde');
//            theaderedRectangle.attr('header/fillOpacity', 0.5);
//            theaderedRectangle.attr('headerText/text', title);
//            theaderedRectangle.attr('body/fillOpacity', 0.5);
//            theaderedRectangle.attr('body/fontSize', 10);
//            theaderedRectangle.attr('bodyText/text', '\n\Owner: ' + child[i].UserName + '\nS: ' + formatDate(child[i].Start) + '\nE: ' + formatDate(child[i].End) + '\n\nStatus: ' + child[i].NtsStatus + '\n\Assiged To: ' + child[i].UserName + "\nCount: " + chs.length);
//            theaderedRectangle.attr('bodyText/fontSize', 13);
//            theaderedRectangle.attr('refId', child[i].Id);
//            theaderedRectangle.attr('collapsed', false);
//            var cp = Object.values(theaderedRectangle.id).join('');
//            theaderedRectangle.attr('collapsedPath', parent.attributes.attrs.collapsedPath + cp + "|");
//            theaderedRectangle.attr('type', child[i].Type);
//            theaderedRectangle.attr('predeccessor', null);
//            projectN.attr('isHavingChild', isHavingChild);
//            graph.addCell(theaderedRectangle);
//            if (parentVal != child[i].ParentId) {
//                type = child[i].Type;
//                parentVal = child[i].ParentId;
//            }
//            xp = xp + 150;

//            createlink(parent, theaderedRectangle);
//            dataLib.push({ id: child[i].Id, cell: theaderedRectangle });
//        } else if (child[i].Type == "SUBTASK") {

//            var theaderedRectangle = new joint.shapes.standard.HeaderedRectangle();
//            theaderedRectangle.resize(180, 180);
//            theaderedRectangle.position(xp, yp);
//            theaderedRectangle.attr('root/tabindex', 12);
//            theaderedRectangle.attr('root/title', mainTitle);
//            theaderedRectangle.attr('header/fill', '#cced00');
//            theaderedRectangle.attr('header/fillOpacity', 0.5);
//            theaderedRectangle.attr('headerText/text', title);
//            theaderedRectangle.attr('body/fillOpacity', 0.5);
//            theaderedRectangle.attr('body/fontSize', 10);
//            theaderedRectangle.attr('bodyText/text', '\n\Owner: ' + child[i].UserName + '\nS: ' + formatDate(child[i].Start) + '\nE: ' + formatDate(child[i].End) + '\n\nStatus: ' + child[i].NtsStatus + '\n\Assiged To: ' + child[i].UserName + "\nCount: " + chs.length);
//            theaderedRectangle.attr('bodyText/fontSize', 13);
//            theaderedRectangle.attr('refId', child[i].Id);
//            theaderedRectangle.attr('collapsed', false);
//            var cp = Object.values(theaderedRectangle.id).join('');
//            theaderedRectangle.attr('collapsedPath', parent.attributes.attrs.collapsedPath + cp + "|");
//            theaderedRectangle.attr('type', child[i].Type);
//            theaderedRectangle.attr('predeccessor', null);
//            projectN.attr('isHavingChild', isHavingChild);
//            graph.addCell(theaderedRectangle);
//            if (parentVal != child[i].ParentId) {
//                type = child[i].Type;
//                parentVal = child[i].ParentId;
//            }
//            xp = xp + 150;

//            createlink(parent, theaderedRectangle);
//            dataLib.push({ id: child[i].Id, cell: theaderedRectangle });
//        }
//        joint.layout.DirectedGraph.layout(graph, {
//            setLinkVertices: false, nodeSep: 50,
//            edgeSep: 80,
//            rankDir: "TB",
//            dagre: dagre,
//            graphlib: dagre.graphlib,
//            marginX: 100,
//            marginY: 50
//        });
//    }

//    for (var i = 0; i < child.length; i++) {
//        var chs = result.filter(x => x.ParentId == child[i].Id);
//        var pt = dataLib.filter(x => x.id == child[i].Id)[0].cell;
//        createChild(chs, result, pt);
//    }
//}

function ShowLoader(target) {
    kendo.ui.progress(target, true);
}
function HideLoader(target) {
    kendo.ui.progress(target, false);
}


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

function openForm() {
    document.getElementById("myForm").style.display = "block";
}

function closeForm() {
    document.getElementById("myForm").style.display = "none";
}

function SaveDiagram() {
    var json = graph.toJSON();
    cells = json.cells;
    var data = [];
    var type = "";
    var isOk = true;
    for (var i = 0; i < cells.length; i++) {
        var parentId = "";
        var parent = cells.filter(x => x.type == "standard.Link" && x.target.id == cells[i].id);
        if (parent.length > 0) {
            parentId = parent[0].source.id;
        }

        var predeccessor = cells[i].attrs.predeccessor;

        if (predeccessor == undefined) {
            predeccessor = [];
        }

        console.log(predeccessor);
        if (cells[i].type == "standard.HeaderedRectangle") {
            if (cells[i].attrs.header.fill == "#89b0d8") {
                type = "Service"
            } else if (cells[i].attrs.header.fill == "#6c119c") {
                type = "Stage"
            } else if (cells[i].attrs.header.fill == "#008bde") {
                type = "Task"
            } else if (cells[i].attrs.header.fill == "#cced00") {
                type = "SubTask"
            }
            debugger;

            var userData = $('#OwnerUserId').data('kendoDropDownList').dataSource.data();

            var el = cells[i].attrs;
            var d = el.root.title.split("\n");
            var title = d[0].trim();
            if (title == "") {
                kendo.alert("Please add title");
                isOk = false;
                break;
            }
            var owner = d[2].split(":")[1].trim();
            if (owner == "") {
                //kendo.alert("Please select owner");
                //isOk = false;

            }
            var sdate = d[4].split("S:")[1].trim();
            if (sdate == "") {
                kendo.alert("Please add start date");
                isOk = false;
                break;
            }
            var edate = d[5].split("E:")[1].trim();
            if (edate == "") {
                kendo.alert("Please add end date");
                isOk = false;
                break;
            }
            var assignee = d[9].split("Assignee:")[1].trim();
            if (assignee || assignee == "") {
                //kendo.alert("Please select assignee");
                //isOk = false;
            }

            var userData = $('#OwnerUserId').data('kendoDropDownList').dataSource.data();
            var oUserId = userData.filter(x => x.Name == owner);
            var aUserId = userData.filter(x => x.Name == assignee);

            var svalue = kendo.toString(sdate, 'yyyy/MM/dd HH:mm');
            var evalue = kendo.toString(edate, 'yyyy/MM/dd HH:mm');

            var refId = null;

            if (el.refId) {
                refId = Object.values(el.refId).join('');
            }

            var obj = {
                "Id": cells[i].id,
                "ParentId": parentId,
                "UserId": aUserId[0] ? aUserId[0].Id : "",
                "OwnerUserId": oUserId[0] ? oUserId[0].Id : "",
                "Title": title,
                "Start": svalue,
                "End": evalue,
                "Type": type,
                "RefId": refId,
                "Predeccessor": predeccessor
            };
            data.push(obj);
        }
    }
    if (isOk) {
        ShowLoader($('#mainContent'));
        debugger;
        $.ajax({
            type: "POST",
            url: "/PJM/Project/SaveMindMap",
            data: { Json: JSON.stringify(data) },
            dataType: "json",
            success: function (result) {
                kendo.alert("Saved");
            },
            error: function (xhr, httpStatusMessage, customErrorMessage) {
                HideLoader($('#mainContent'));
                ShowNotification("Saved Successfully", "success");
            }
        });

        joint.layout.DirectedGraph.layout(graph, {
            setLinkVertices: false, nodeSep: 50,
            edgeSep: 80,
            rankDir: "TB",
            dagre: dagre,
            graphlib: dagre.graphlib,
            marginX: 100,
            marginY: 100
        });
    }
}

function saveProperties() {
    ShowLoader($('#mainContent'));
    if (lastClickedElementView) {
        var sdate = kendo.toString($('#Start').data('kendoDateTimePicker').value(), 'yyyy/MM/dd HH:mm');
        var edate = kendo.toString($('#End').data('kendoDateTimePicker').value(), 'yyyy/MM/dd HH:mm');
        var owner = $('#OwnerUserId').data('kendoDropDownList').text();
        var title = $('#Title').data('kendoTextBox').value() + "\n\nOwner: " + owner + '\n\nS:   ' + sdate + '\nE: ' + edate + '\n\nStatus: Draft\n\nAssignee: ' + $('#AssigneeUserId').data('kendoDropDownList').text();
        if (owner.length > 10) owner = owner.substring(0, 10);
        var assignee = $('#AssigneeUserId').data('kendoDropDownList').text();
        if (assignee.length > 10) assignee = assignee.substring(0, 10);
        var cell = graph.getCell(lastClickedElementView.model.id);
        if (cell.isElement()) {
            var btext = $('#Title').data('kendoTextBox').value();
            if (btext.length > 10) btext = btext.substring(0, 10);
            var body = "\nOwner: " + owner + '\n\nS:' + sdate + '\nE: ' + edate + '\n\nStatus: Draft\n\nAssignee: ' + assignee;
            cell.attr('headerText/text', btext);
            cell.attr('bodyText/text', body);
            cell.attr('headerText/fontSize', 12);
            cell.attr('root/title', title);
            cell.attr('predeccessor', $("#pred").data("kendoMultiSelect").value());
        }
        console.log(lastClickedElementView);
        $('#Start').data('kendoDateTimePicker').value("");
        $('#End').data('kendoDateTimePicker').value("");
        $('#Title').data('kendoTextBox').value("");
        $('#OwnerUserId').data('kendoDropDownList').text('Select');
        $('#AssigneeUserId').data('kendoDropDownList').text('Select');
        $('#pred').data('kendoMultiSelect').value('');
        $('#succ').data('kendoMultiSelect').value('');
        ShowNotification("Properties updated", "success");
        closeForm();
    } else {
        kendo.alert("Select any node to change property");
    }
    HideLoader($('#mainContent'));
}


function convertDateTime(m) {
    var m = new Date(m);
    return m.getUTCFullYear() + "/" + (m.getUTCMonth() + 1) + "/" + m.getUTCDate() + " " + m.getUTCHours() + ":" + m.getUTCMinutes() + ":" + m.getUTCSeconds();
}
var zoomLevel = 0;

function onZoomIn() {
    //paperScroller.zoom(0.2, { max: 4 });
    zoomLevel = Math.min(3, zoomLevel + 0.2);
    var size = paper.getComputedSize();
    paper.translate(0, 0);
    paper.scale(zoomLevel, zoomLevel, size.width / 2, size.height / 2);
}

function onZoomOut() {
    //paperScroller.zoom(-0.2, { min: 0.2 });
    zoomLevel = Math.max(0.2, zoomLevel - 0.2);
    var size = paper.getComputedSize();
    paper.translate(0, 0);
    paper.scale(zoomLevel, zoomLevel, size.width / 2, size.height / 2);
}


var linkData = [];

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
//    console.log("ready!");
//    debugger;
//    $.ajax({
//        url: '/Cms/Template/GetNtsTemplateTreeList',
//        dataType: "json",
//        success: function (result) {
//            debugger;
//            for (var i = 0; i < result.length; i++) {
//                var subtheaderedRectangle = new joint.shapes.standard.HeaderedRectangle();
//                subtheaderedRectangle.resize(80, 80);
//                subtheaderedRectangle.position(20, 190);
//                subtheaderedRectangle.attr('root/tabindex', 12);
//                subtheaderedRectangle.attr('root/title', result[i].Name);
//                subtheaderedRectangle.attr('header/fill', '#cced00');
//                subtheaderedRectangle.attr('header/fillOpacity', 0.5);
//                subtheaderedRectangle.attr('headerText/text', result[i].Name);
//                subtheaderedRectangle.attr('body/fillOpacity', 0.5);
//                subtheaderedRectangle.attr('body/fontSize', 10);
//                subtheaderedRectangle.attr('bodyText/fontSize', 13);

//                // Stencils-------------------------------------------------------------------------
//                stencilGraph.addCells.push(subtheaderedRectangle);
//            }
//        }
//    });


//    $(".toggle-nav").trigger("click");
//    //var projectId = document.getElementById("ProjectId").value;
//    //if (projectId) {
//    //    $('#projects').data('kendoDropDownList').value(projectId);
//    //    onProjectSelection();
//    //}
//});

//function onProjectSelection() {
//    ShowLoader($('#mainContent'));
//    var pId = $('#projects').data('kendoDropDownList').value();
//    graph.clear();
//    $.ajax({
//        url: '/PJM/Project/GetWBSItem?projectId=' + pId,
//        dataType: "json",
//        success: function (result) {
//            console.log(result);
//            var ids;
//            var project = result.filter(x => x.Type == "PROJECT");
//            var child = result.filter(x => x.ParentId == project[0].Id);
//            var isHavingChild = child.length > 0 ? true : false;
//            var mainTitle = project[0].Title + "\n\nOwner: " + project[0].UserName + '\n\nS:   ' + project[0].Start + '\nE: ' + project[0].End + '\n\nStatus: Draft\n\nAssignee: ' + project[0].UserName;

//            var projectN = new joint.shapes.standard.HeaderedRectangle();

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
//            debugger;
//            graph.addCell(projectN);

//            if (type != project.Type) {
//                xp = 50;
//                yp = yp + 150;
//                type = project[0].Type;
//            }
//            xp = xp + 150;

//            createChild(child, result, projectN);
//            HideLoader($('#mainContent'));
//        }
//    });
//}


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
//    debugger;
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
//    debugger;
//    succ.value(succValue);
//    succ.enable(false);
//}





//var childEl = [];
//var childLinks = [];
//function toggleCollapse(group) {
//    if (elementView.model.attributes.attrs.isHavingChild == true) {

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
//            debugger;
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



//var paperScroller = new joint.ui.PaperScroller({
//    paper: paper,
//    cursor: 'grab'
//});

//$('#paper-container').append(paperScroller.render().el);



function loadDataonClickOfTree(id)
{   
    //Canvas from which you take shapes
    var stencilGraph = new joint.dia.Graph,
        stencilPaper = new joint.dia.Paper({
            el: $('#stencil'),
            width: 200,
            height:700,
            model: stencilGraph,
            interactive: false
        });
    $.ajax({
        url: '/Cms/ProcessDiagram/GetPageByMenuId?menuId=' + id,
        dataType: "json",
        success: function (result) {   
            var cell = [];
            stencilGraph.addCells([]);
            var pos = 10;
            for (var i = 0; i < result.length; i++) {
                var subtheaderedRectangle = new joint.shapes.standard.HeaderedRectangle();
                debugger
                subtheaderedRectangle.resize(120, 120);
                subtheaderedRectangle.position(20, pos);
                subtheaderedRectangle.attr('root/tabindex', 12);
                subtheaderedRectangle.attr('root/title', result[i].Name);
                subtheaderedRectangle.attr('header/fill', '#cced00');
                subtheaderedRectangle.attr('header/fillOpacity', 0.5);
                subtheaderedRectangle.attr('headerText/text', result[i].Name);
                subtheaderedRectangle.attr('body/fillOpacity', 0.5);
                subtheaderedRectangle.attr('body/fontSize', 8);
                subtheaderedRectangle.attr('bodyText/fontSize', 8);
                subtheaderedRectangle.attr('headerText/fontSize', 10);
                subtheaderedRectangle.attr('id', result[i].Id);
                subtheaderedRectangle.attr('type', "PAGE");
                // Stencils-------------------------------------------------------------------------
               
                cell.push(subtheaderedRectangle);
                pos += 90;
            }
            stencilGraph.addCells(cell);
        }
    });   
    stencilPaper.on('cell:pointerdown', function (cellView, e, x, y) {
        debugger
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
                console.log(s);
                graph.addCell(s);
                console.log(flyShape);
                //Add Template Along with Page 
                if (flyShape.attributes.attrs.type == "PAGE") {
                    var pageId = flyShape.attributes.attrs.id;
                    $.ajax({
                        url: '/Cms/ProcessDiagram/GetTemplateByPageId?pageId=' + pageId,
                        dataType: "json",
                        success: function (result) {
                          debugger
                            //            var ids;
                            //            var project = result.filter(x => x.Type == "PROJECT");
                            //            var child = result.filter(x => x.ParentId == project[0].Id);
                            //            var isHavingChild = child.length > 0 ? true : false;
                            //            var mainTitle = project[0].Title + "\n\nOwner: " + project[0].UserName + '\n\nS:   ' + project[0].Start + '\nE: ' + project[0].End + '\n\nStatus: Draft\n\nAssignee: ' + project[0].UserName;
                            if (flyShape.attributes.attrs.type == "PAGE") {
                                xp = 50;
                                yp = yp + 150;
                                //type = project[0].Type;
                            }
                            var projectN = new joint.shapes.standard.HeaderedRectangle();
                            projectN.resize(180, 180);
                            projectN.position(xp, yp);
                            projectN.attr('root/tabindex', 12);
                            projectN.attr('root/title', result[0].Name);
                            projectN.attr('header/fill', '#89b0d8');
                            projectN.attr('header/fillOpacity', 0.5);
                            projectN.attr('headerText/text', result[0].Name);
                            projectN.attr('body/fillOpacity', 0.5);
                            projectN.attr('body/fontSize', 10);
                            projectN.attr('bodyText/fontSize', 13);
                            // projectN.attr('bodyText/text', result.Name);
                            // projectN.attr('refId', project[0].Id);
                            projectN.attr('parentId', pageId);
                            projectN.attr('id', result[0].Id);
                            //  projectN.attr('collapsed', false);
                            // var cp = Object.values(projectN.id).join('');
                            //projectN.attr('collapsedPath', cp + "|");
                            projectN.attr('type', "TEMPLATE");
                            // projectN.attr('predeccessor', $("#pred").data("kendoMultiSelect").value());
                            // projectN.attr('isHavingChild', isHavingChild);
                            //            debugger;
                            graph.addCell(projectN);

                                        
                            //            xp = xp + 150;

                            //            createChild(child, result, projectN);
                            //            HideLoader($('#mainContent'));
                            var link = new joint.shapes.standard.Link();
                            link.source(s);
                            link.target(projectN);
                            //link.addTo(flyGraph);
                            link.addTo(graph);
                        }
                    });
                }
            }
           
            $('body').off('mousemove.fly').off('mouseup.fly');
            flyShape.remove();
            $('#flyPaper').remove();
        });
    });
}


//function OnSelect(e) {
//    //  e.preventDefault();
//    debugger;
//    var dataItem = this.dataItem(e.node);
//    loadDataonClickOfTree(dataItem.Id);
   
//}

$(document).ready(function () {
    console.log("ready!");
    debugger;
    $(".toggle-nav").trigger("click");
    //var projectId = document.getElementById("ProjectId").value;
    //if (projectId) {
    //    $('#projects').data('kendoDropDownList').value(projectId);
    //    onProjectSelection();
    //}
});
