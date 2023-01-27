
var elementPaper = document.getElementById("paperd");
var zoomLevel = 1;

var startTime = new Date();
ASYNC = false;

// Canvas where sape are dropped
var graph = new joint.dia.Graph,
    paper = new joint.dia.Paper({
        el: elementPaper,
        model: graph,
        height: 2000,
        width: 10000,
        async: ASYNC,
    });



function showResult() {
    var duration = (new Date() - startTime) / 1000;
    $('#perf').text('This diagram is rendered in ' + duration + 's');
}




//var paperScroller = new joint.ui.PaperScroller({
//    paper: paper,
//    cursor: 'grab'
//});

//$('#paper-container').append(paperScroller.render().el);

//initializeNavigator();

// Canvas from which you take shapes
var stencilGraph = new joint.dia.Graph,
    stencilPaper = new joint.dia.Paper({
        el: $('#stencil'),
        width: 120,
        model: stencilGraph,
        interactive: false
    });



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
            console.log(s);
            graph.addCell(s);
        }
        $('body').off('mousemove.fly').off('mouseup.fly');
        flyShape.remove();
        $('#flyPaper').remove();
    });
});

//get Data
$.ajax({
    url: '/Cms/content/GetPortalDiagramData',
    dataType: "json",
    success: function (result) {

        console.log(result);
        var portal = result.filter(x => x.Type == "PORTAL");
        //for (var i = 0; i < portal.length; i++) {
        var child = result.filter(x => x.ParentId == portal[0].Id);

        var node = createNode(portal[0], result, null, "#e57373");
        graph.addCell(node);

        if (type != node.Type) {
            xp = 50;
            yp = yp + 150;
            //type = node[0].Type;
        }
        xp = xp + 150;

        createChild(child, result, node);
        //}
    }
});

var xp = 100;
var yp = 100;
var type = '';
var parent = '';
var parentVal;
var dataLib = [];

function createChild(child, result, parent) {
    xp = 50

    for (var i = 0; i < child.length; i++) {

        var chs = result.filter(x => x.ParentId == child[i].Id);

        if (child[i].Type == "PORTAL") {

            var node = createNode(child[i], result, parent, "#e57373");
            graph.addCell(node);

            if (parentVal != child[i].ParentId) {
                xp = 50;
                type = child[i].Type;
                parentVal = child[i].ParentId;
            }
            xp = xp + 150;
            createlink(parent, node);
            dataLib.push({ id: child[i].Id, cell: node });
        } else if (child[i].Type == "MODULE") {

            var node = createNode(child[i], result, parent, "#ba68c8");
            graph.addCell(node);

            if (parentVal != child[i].ParentId) {
                type = child[i].Type;
                parentVal = child[i].ParentId;
            }
            xp = xp + 150;

            createlink(parent, node);
            dataLib.push({ id: child[i].Id, cell: node });
        } else if (child[i].Type == "SUBMODULE") {

            var node = createNode(child[i], result, parent, "#7986cb");
            graph.addCell(node);

            if (parentVal != child[i].ParentId) {
                type = child[i].Type;
                parentVal = child[i].ParentId;
            }
            xp = xp + 150;

            createlink(parent, node);
            dataLib.push({ id: child[i].Id, cell: node });
        }
        else if (child[i].Type == "MENUGROUP") {

            var node = createNode(child[i], result, parent, "#4dd0e1");
            graph.addCell(node);

            if (parentVal != child[i].ParentId) {
                type = child[i].Type;
                parentVal = child[i].ParentId;
            }
            xp = xp + 150;

            createlink(parent, node);
            dataLib.push({ id: child[i].Id, cell: node });
        } else if (child[i].Type == "MENU") {

            var node = createNode(child[i], result, parent, "#aed581");
            graph.addCell(node);

            if (parentVal != child[i].ParentId) {
                type = child[i].Type;
                parentVal = child[i].ParentId;
            }
            xp = xp + 150;

            createlink(parent, node);
            dataLib.push({ id: child[i].Id, cell: node });
        }
        else if (child[i].Type == "PAGETYPE") {

            var node = createNode(child[i], result, parent, "#fff176");
            graph.addCell(node);

            if (parentVal != child[i].ParentId) {
                type = child[i].Type;
                parentVal = child[i].ParentId;
            }
            xp = xp + 150;

            createlink(parent, node);
            dataLib.push({ id: child[i].Id, cell: node });
        }
        else if (child[i].Type == "TEMPLATE") {

            var node = createNode(child[i], result, parent, "#ffb74d");
            graph.addCell(node);

            if (parentVal != child[i].ParentId) {
                type = child[i].Type;
                parentVal = child[i].ParentId;
            }
            xp = xp + 150;

            createlink(parent, node);
            dataLib.push({ id: child[i].Id, cell: node });
        }
        else if (child[i].Type == "TEMPLATETYPE") {

            var node = createNode(child[i], result, parent, "#a1887f");
            graph.addCell(node);

            if (parentVal != child[i].ParentId) {
                type = child[i].Type;
                parentVal = child[i].ParentId;
            }
            xp = xp + 150;

            createlink(parent, node);
            dataLib.push({ id: child[i].Id, cell: node });
        }

        else if (child[i].Type == "TYPEUDF") {

            var node = createNode(child[i], result, parent, "#00b8d4");
            graph.addCell(node);

            if (parentVal != child[i].ParentId) {
                type = child[i].Type;
                parentVal = child[i].ParentId;
            }
            xp = xp + 150;

            createlink(parent, node);
            dataLib.push({ id: child[i].Id, cell: node });
        }
        else if (child[i].Type == "TYPETABLEMETADATA") {

            var node = createNode(child[i], result, parent, "#00c853");
            graph.addCell(node);

            if (parentVal != child[i].ParentId) {
                type = child[i].Type;
                parentVal = child[i].ParentId;
            }
            xp = xp + 150;

            createlink(parent, node);
            dataLib.push({ id: child[i].Id, cell: node });
        } else if (child[i].Type == "UDFTEMPLATE") {

            var node = createNode(child[i], result, parent, "#ffd600");
            graph.addCell(node);

            if (parentVal != child[i].ParentId) {
                type = child[i].Type;
                parentVal = child[i].ParentId;
            }
            xp = xp + 150;

            createlink(parent, node);
            dataLib.push({ id: child[i].Id, cell: node });
        }

        else if (child[i].Type == "COLMETADATA") {

            var node = createNode(child[i], result, parent, "#ff8a80");
            graph.addCell(node, parent);

            if (parentVal != child[i].ParentId) {
                type = child[i].Type;
                parentVal = child[i].ParentId;
            }
            xp = xp + 150;

            createlink(parent, node);
            dataLib.push({ id: child[i].Id, cell: node });
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
    }

    for (var i = 0; i < child.length; i++) {
        var chs = result.filter(x => x.ParentId == child[i].Id);
        var pt = dataLib.filter(x => x.id == child[i].Id)[0].cell;
        createChild(chs, result, pt);
    }
    roundedCornerRect();
}

function createlink(parent, child) {

    if (parent && child) {
        var link = new joint.shapes.standard.Link();
        link.source(parent);
        link.target(child);
        link.attr('collapsedPath', parent.attributes.attrs.collapsedPath + "|" + link.id);
        link.router('manhattan');
        link.connector('rounded');
        link.addTo(graph);
    }
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
    return node;
}

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


//paper.on('render:done', showResult);

//function initializeNavigator() {

//    var navigator = this.navigator = new joint.ui.Navigator({
//        width: 240,
//        height: 115,
//        paperScroller: paperScroller,
//        zoom: {
//            grid: 0.2,
//            min: 0.2,
//            max: 5
//        },
//        paperOptions: {
//            async: true,
//            sorting: joint.dia.Paper.sorting.NONE,
//            elementView: joint.shapes.NavigatorElementView,
//            linkView: joint.shapes.NavigatorLinkView,
//            cellViewNamespace: { /* no other views are accessible in the navigator */ }
//        }
//    });

//    this.$('.navigator-container').append(navigator.el);
//    navigator.render();
//}

//joint.shapes.NavigatorElementView = joint.dia.ElementView.extend({

//    body: null,

//    markup: [{
//        tagName: 'rect',
//        selector: 'body',
//        attributes: {
//            'fill': '#31d0c6'
//        }
//    }],

//    initFlag: ['RENDER', 'UPDATE', 'TRANSFORM'],

//    presentationAttributes: {
//        size: ['UPDATE'],
//        position: ['TRANSFORM'],
//        angle: ['TRANSFORM']
//    },

//    confirmUpdate: function (flags) {

//        if (this.hasFlag(flags, 'RENDER')) this.render();
//        if (this.hasFlag(flags, 'UPDATE')) this.update();
//        if (this.hasFlag(flags, 'TRANSFORM')) this.updateTransformation();
//    },

//    render: function () {
//        var doc = joint.util.parseDOMJSON(this.markup);
//        this.body = doc.selectors.body;
//        this.el.appendChild(doc.fragment);
//    },

//    update: function () {
//        var size = this.model.size();
//        this.body.setAttribute('width', size.width);
//        this.body.setAttribute('height', size.height);
//    }
//});

//joint.shapes.NavigatorLinkView = joint.dia.LinkView.extend({

//    initialize: joint.util.noop,

//    render: joint.util.noop,

//    update: joint.util.noop
//});

