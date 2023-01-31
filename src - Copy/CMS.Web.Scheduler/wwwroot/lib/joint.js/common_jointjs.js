

////events---------------------------------------------------------------------------------

//var lastClickedElementView;
//var currentSelectedElementView;

//this.paper.on('blank:pointerdown', (evt, x, y) => {
//    //this.paperScroller.startPanning(evt);
//});

//paper.on('link:mouseenter', function (linkView) {
//    linkView.addTools(linktools);
//});

//paper.on('link:mouseleave', function (linkView) {
//    linkView.removeTools();
//});
//paper.on({

//    'element:mouseenter': function (elementView) {
//        console.log(elementView);
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

//        document.getElementById("myForm").style.display = "block";
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

//        if (lastClickedElementView != undefined) {
//            joint.highlighters.mask.remove(lastClickedElementView);
//        }

//        currentSelectedElementView = elementView;

//        joint.highlighters.mask.add(elementView, { selector: 'root' }, 'my-element-highlight', {
//            deep: true,
//            attrs: {
//                'stroke': '#FF4365',
//                'stroke-width': 3,
//                'padding': 10
//            }
//        });

//        lastClickedElementView = elementView;

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

     

//        if (elementBelow && elementAbove) {
//            if ((elementBelow.attributes.attrs.header.fill == "#89b0d8" && elementAbove.attributes.attrs.header.fill == "#6c119c") ||
//                (elementBelow.attributes.attrs.header.fill == "#89b0d8" && elementAbove.attributes.attrs.header.fill == "#008bde") ||
//                (elementBelow.attributes.attrs.header.fill == "#6c119c" && elementAbove.attributes.attrs.header.fill == "#008bde") ||
//                (elementBelow.attributes.attrs.header.fill == "#008bde" && elementAbove.attributes.attrs.header.fill == "#cced00") ||
//                (elementBelow.attributes.attrs.header.fill == "#cced00" && elementAbove.attributes.attrs.header.fill == "#cced00")) {

//                if (elementBelow && graph.getNeighbors(elementBelow).indexOf(elementAbove) === -1) {

//                    elementAbove.position(evt.data.x, evt.data.y);

//                    // Create a connection between elements.
//                    var link = new joint.shapes.standard.Link();
//                    link.source(elementBelow);
//                    link.target(elementAbove);
//                    link.addTo(graph);

//                    // Add remove button to the link.
//                    var tools = new joint.dia.ToolsView({
//                        tools: [new joint.linkTools.Remove()]
//                    });
//                    link.findView(this).addTools(tools);
//                }
//            }
//            else {
//                elementAbove.position(evt.data.x, evt.data.y);

//            }
//        }
//    }
//});



////Tools------------------------------------------------------------------------

//// Add remove button to the link.
//var linktools = new joint.dia.ToolsView({
//    tools: [new joint.linkTools.Remove()]
//});


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
//        removeButton,
//        collapseButton
//    ]
//});



//joint.elementTools.expandButton = joint.elementTools.Button.extend({
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

//var onlyBasicTool = new joint.dia.ToolsView({
//    tools: [
//        boundaryTool,
//        removeButton,
//    ]
//});

/////

////Toggle///////////////////////////////////////////////////////////////////
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
//        group.addTools(onlyBasicTool);
//        group.showTools();
//    }
  
//}


//// for rounded corner 
//function roundedCornerRect() {
//    debugger;
//    var ele = document.getElementsByTagName("rect");
//    for (var x = 0; x <= ele.length - 1; x++) {
//        ele[x].setAttribute("rx", 5);
//        ele[x].setAttribute("ry", 5);
//    }
//}