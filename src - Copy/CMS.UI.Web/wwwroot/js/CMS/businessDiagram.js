var isloadedNote = false;
var isloadedTask = false;
var isloadedService = false;
var selectedTemplateId = "";
var selectedTemplateType = "";
var lastClickedElementView;
var currentSelectedElementView;
var currentParentNode = "";

$(document).ready(function () {

    var tempId = document.getElementById("tempId").value;
    var lo = document.getElementById("lo").value;

    if (lo == "Popup") {
        if (tempId != null && tempId != "") {
            loadDiagram(tempId);
        }
    }

    else {
        //var treeview = $("#documentTypeTreeView").data("kendoTreeView");
        //treeview.collapse(".k-item");
    }



    $(".toggle-nav").trigger("click");
    var acc = document.getElementsByClassName("accordion");
    var i;

    for (i = 0; i < acc.length; i++) {
        acc[i].addEventListener("click", function () {
            this.classList.toggle("active");
            var panel = this.nextElementSibling;
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = panel.scrollHeight + 50 + "px";
            }
        });
    }

    var acc2 = document.getElementsByClassName("accordion2");
    var i;

    for (i = 0; i < acc2.length; i++) {
        acc2[i].addEventListener("click", function () {
            this.classList.toggle("active");
            var panel = this.nextElementSibling;
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = panel.scrollHeight + 50 + "px";
            }
        });
    }
    document.getElementById("font-size").innerText = document.getElementById('tfontsize').value + "px";

});

const menu = document.querySelector(".menu");
const menuOption = document.querySelector(".menu-option");
let menuVisible = false;



function OnDelete(id) {


    kendo.confirm("Are you sure that you want to proceed?").then(function () {
        confirmDelete(id);
    }, function () {

    });

}

function confirmDelete(id) {


    $.ajax({

        url: '/cms/Template/DeleteTemplate?Id=' + id,
        type: "GET",
        contentType: "application/json",
        dataType: "JSON",
        success: function (response) {

            ShowNotification("Deleted Successfully", "success");
            //$("#documentTypeTreeView").data("kendoTreeView").dataSource.read();
            $('#documentTypeTreeView').jstree(true).refresh();
        }
    });


}

function Export(id) {
    var url = '/cms/template/ManageExport?Id=' + id;
    window.open(url, '_blank');
    return false;

}


function getTemplateNote() {

    if (!isloadedNote) {
        ShowLoader($('#stencil2'));
        isloadedNote = true;
        template("Note");
    }

}
function getTemplateTask() {
    if (!isloadedTask) {
        ShowLoader($('#stencil3'));
        isloadedTask = true;
        template("Task");
    }
}
function getTemplateService() {
    if (!isloadedService) {
        ShowLoader($('#stencil4'));
        isloadedService = true;
        template("Service");
    }
}
function TemplateChangeCallback(prms) {

    if (prms != null && prms != undefined && prms.TemplateId != null && prms.TemplateId != undefined) {
        //$("#documentTypeTreeView").data("kendoTreeView").dataSource.read();
        $('#documentTypeTreeView').jstree(true).refresh();
        createTemplateDiagram(prms.TemplateId);
    }

}
function TemplateCategoryCallback() {
    //$("#documentTypeTreeView").data("kendoTreeView").dataSource.read();
    $('#documentTypeTreeView').jstree(true).refresh();
}
function createNTSTemplate(catId) {
    url = "/cms/Template/Template?categoryId=" + catId + "&lo=Popup&cbm=TemplateChangeCallback";
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: 'Create Template', Width: 1200, Height: 650 });
}

function editNTSTemplate(id) {
    url = "/cms/Template/Template?templateId=" + id + "&lo=Popup&cbm=TemplateChangeCallback";
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: 'Edit Template', Width: 1200, Height: 650 });
}

function createNtsCategory(type) {
    url = "/cms/TemplateCategory/Create?type=" + type + "&lo=Popup&cbm=TemplateCategoryCallback";// + "&layout=Popup";
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: 'Create Catgeory', Width: 600, Height: 600 });
}

function editNtsCategory(id) {
    url = "/cms/TemplateCategory/Edit?id=" + id + "&lo=Popup&cbm=TemplateCategoryCallback";// + "&layout=Popup";
    var win = GetMainWindow();
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: 'Edit Catgeory', Width: 600, Height: 600 });
}

function importTemplate(parentId, selectParentId) {
    var win = GetMainWindow();
    var url = "/cms/template/ManageImport?categoryId=" + parentId + "&type=" + selectParentId;
    win.iframeOpenUrl = url;
    win.OpenWindow({ Title: 'Import', Width: 450, Height: 950 });
}

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

function template(n) {
    $.ajax({
        url: '/cms/template/GetTemplateByType?type=' + n,
        dataType: "json",
        success: function (result) {
            var x = 20, y = 10;
            for (var i = 0; i < result.length; i++) {
                createTemplate(result[i], n, x, y)
                y = y + 150;
            }
            if (n == "Note") {
                HideLoader($('#stencil2'));
            }
            else if (n == "Task") {
                HideLoader($('#stencil3'));
            }
            else if (n == "Service") {
                HideLoader($('#stencil4'));
            }

        }
    });
}

function closeDiagram() {
    if (selectedTemplateId != "") {
        var flag = confirm('Do you really want to close the diagram without save.');
        if (flag) {
            graph.clear();
            selectedTemplateId = "";
            selectedTemplateType = "";
        }
    }
}


function createTemplate(result, type, x, y) {
    var wraptext = joint.util.breakText(result.DisplayName, {
        width: 120,
        height: 120
    });

    var wraptextD = joint.util.breakText('TC: ' + result.TemplateCategoryName + '\n ' + result.Description, {
        width: 120,
        height: 120
    });

    var bcolor = '#89b0d8';
    var hcolor = '#E0E2D2';

    if (type == "Note") {
        bcolor = "#64C5EB";
    } else if (type == "Task") {
        bcolor = "#E84D8A";
    } else if (type == "Service") {
        bcolor = "#FEB326";
    }

    var node = new joint.shapes.standard.HeaderedRectangle();
    node.resize(120, 100);
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
    node.attr('refId', result.Id);
    node.attr('templateType', type);
    node.attr('type', "TEMPLATE");
    node.attr('parentIds', "");
    node.attr('isHavingChild', true);
    node.attr('collapsed', false);
    node.attr('collapsedPath', node.id + "|");

    if (type == "Note") {
        stencilGraph2.addCells(node);
    } else if (type == "Task") {
        stencilGraph3.addCells(node);
    } else if (type == "Service") {
        stencilGraph4.addCells(node);
    }
}

// Canvas where sape are dropped
var graph = new joint.dia.Graph,
    paper = new joint.dia.Paper({
        el: $('#paper'),
        model: graph,
        height: 2000,
        width: 3000
    });

//stencil
var stencilGraph = new joint.dia.Graph,
    stencilPaper = new joint.dia.Paper({
        el: $('#stencil'),
        model: stencilGraph,
        interactive: false,
        height: 1184,
        width: 200
    });

//prop
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
// Add remove button to the link.
var linktools = new joint.dia.ToolsView({
    tools: [new joint.linkTools.Remove()]
});
//var borderedImage = new joint.shapes.standard.BorderedImage();
//borderedImage.resize(80, 80);
//borderedImage.position(20, 10);
//borderedImage.attr('root/title', 'joint.shapes.standard.BoarderedImage');
//borderedImage.attr('label/text', 'Bordered\nImage');
//borderedImage.attr('border/rx', 5);
//borderedImage.attr('image/xlinkHref', 'image.png');
//borderedImage.attr('type', 'standard');
//var cylinder = new joint.shapes.standard.Cylinder();
//cylinder.resize(80, 80);
//cylinder.position(20, 150);
//cylinder.attr('root/title', 'joint.shapes.standard.Cylinder');
//cylinder.attr('body/fill', 'lightgray');
//cylinder.attr('top/fill', 'gray');
//cylinder.attr('label/text', 'Cylinder');
//cylinder.topRy('10%');
////cylinder.attr('type', 'standard');
//var headeredRectangle = new joint.shapes.standard.HeaderedRectangle();
//headeredRectangle.resize(80, 80);
//headeredRectangle.position(150, 150);
//headeredRectangle.attr('root/title', 'joint.shapes.standard.HeaderedRectangle');
//headeredRectangle.attr('header/fill', 'lightgray');
//headeredRectangle.attr('headerText/text', 'Header');
//headeredRectangle.attr('bodyText/text', 'Headered\nRectangle');
//headeredRectangle.attr('type', 'standard');

var circle = new joint.shapes.standard.Circle();
circle.resize(80, 80);
circle.position(20, 10);
circle.attr('root/title', 'Circle');
circle.attr('label/text', 'Circle');
circle.attr('body/fill', 'lightblue');
circle.attr('type', 'standard');

var ellipse = new joint.shapes.standard.Ellipse();
ellipse.resize(120, 50);
ellipse.position(20, 110);
ellipse.attr('root/title', 'Ellipse');
ellipse.attr('label/text', 'Ellipse');
ellipse.attr('body/fill', 'lightblue');
ellipse.attr('type', 'standard');


var polygon = new joint.shapes.standard.Polygon();
polygon.resize(80, 80);
polygon.position(20, 170);
polygon.attr('root/title', 'Polygon');
polygon.attr('label/text', 'Polygon');
polygon.attr('body/refPoints', '0,10 10,0 20,10 10,20');
polygon.attr('type', 'standard');

var rectangle = new joint.shapes.standard.Rectangle();
rectangle.resize(80, 80);
rectangle.position(20, 270);
rectangle.attr('root/title', 'Rectangle');
rectangle.attr('label/text', 'Rectangle');
rectangle.attr('body/fill', 'lightblue');
rectangle.attr('type', 'standard');
stencilGraph.addCells([circle, ellipse, polygon, rectangle]);

var roundedRectangle = new joint.shapes.standard.Rectangle();
roundedRectangle.resize(150, 50);
roundedRectangle.position(20, 270);
roundedRectangle.attr('root/title', 'Rectangle');
roundedRectangle.attr('label/text', 'Rectangle');
roundedRectangle.attr('body/fill', 'lightblue');
roundedRectangle.attr('body/rx', 20);
roundedRectangle.attr('body/ry', 20);
roundedRectangle.attr('type', 'standard');

//var atomic = new joint.shapes.devs.Atomic();
//atomic.resize(80, 80);
//atomic.position(20, 400);
//var erdRelationship = new joint.shapes.erd.Relationship();
//erdRelationship.resize(80, 80);
//erdRelationship.position(150, 400);
//var orgMember = new joint.shapes.org.Member();
//orgMember.resize(100, 100);
//orgMember.position(20, 500);
//orgMember.attr('root/dataTooltip', 'Member');
//orgMember.attr('root/dataTooltipPosition', 'left');
//orgMember.attr('root/dataTooltipPositionSelector', '.joint-stencil');
//orgMember.attr('.rank/text', 'Rank');
//orgMember.attr('.rank/fill', '#f6f6f6');
//orgMember.attr('.rank/font-family', 'Roboto Condensed');
//orgMember.attr('.name/text', 'Person');
//orgMember.attr('.name/fill', '#f6f6f6');
//orgMember.attr('.card/fill', '#31d0c6');
//orgMember.attr('image/width', 32);
//orgMember.attr('image/height', 32);
//orgMember.attr('image/x', 16);
//orgMember.attr('image/y', 13);
//orgMember.attr('image/ref', null);
//orgMember.attr('image/ref-x', null);
//orgMember.attr('image/ref-y', null);
//orgMember.attr('image/y-alignment', null);
//orgMember.attr('image/xlink:href', 'http://pngimg.com/uploads/plus/small/plus_PNG95.png');
var start = new joint.shapes.standard.Ellipse();
start.resize(80, 80);
start.position(20, 10);
start.attr('root/title', 'Ellipse');
start.attr('label/text', 'Start');
start.attr('body/fill', 'lightblue');
start.attr('isHavingChild', false);
start.attr('type', "WF_START");
var stop = new joint.shapes.standard.Ellipse();
stop.resize(80, 80);
stop.position(150, 10);
stop.attr('root/title', 'Ellipse');
stop.attr('label/text', 'stop');
stop.attr('body/fill', 'lightgray');
stop.attr("isHavingChild", false);
stop.attr('type', "WF_STOP");
var decision = new joint.shapes.standard.Polygon();
decision.resize(80, 80);
decision.position(20, 150);
decision.attr('root/title', 'Polygon');
decision.attr('label/text', 'Decision Script');
decision.attr('body/fill', 'lightblue');
decision.attr("isHavingChild", false);
decision.attr('type', "WF_DECISION");
var step = new joint.shapes.standard.Rectangle();
step.resize(80, 80);
step.position(150, 150);
step.attr('root/title', 'Rectangle');
step.attr('label/text', 'Step Task');
step.attr('body/fill', 'lightgray');
step.attr('type', "WF_STEP_TASK");
step.attr("isHavingChild", false);
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

            if (s.attributes.attrs.root.title == "Ellipse") {

            } else {
                s.resize(120, 120);
            }
            draggedElement = s;
            s.attr('collapsedPath', s.id + "|");
            graph.addCell(s);
            document.getElementById("prop").style.display = "";

        }
        $('body').off('mousemove.fly').off('mouseup.fly');
        flyShape.remove();
        $('#flyPaper').remove();
    });
});

function createTemplateNodes(node) {
    ShowLoader($('#dcontent'));


    currentParentNode = node.attributes.attrs.refId;
    $.ajax({
        url: '/cms/template/GetTemplateBusinessDiagram?templateId=' + node.attributes.attrs.refId,
        dataType: "json",
        success: function (result) {
            var x = 20, y = 10;
            debugger;

            var childList = result.list.map(function (el) {
                if (el.ParentIdList != null) {
                    var l = el.ParentIdList.filter(x => x == currentParentNode);
                    if (l.length > 0) {
                        return el;
                    }
                }
            });


            createChildNode(childList.filter(x => x != undefined), x, y, result.list, node);
            HideLoader($('#dcontent'));
        }
    });
}


function createTemplateDiagram(id) {
    graph.clear();
    ShowLoader($('#dcontent'));
    currentParentNode = id;
    $.ajax({
        url: '/cms/template/GetTemplateBusinessDiagram?templateId=' + id,
        dataType: "json",
        success: function (result) {
            debugger;

            setDaigramDetails(result.diagramDetails);

            var standard = JSON.parse(result.standard);
            if (standard) {
                if (standard.cells) {
                    for (var n = 0; n <= standard.cells.length - 1; n++) {

                        graph.addCell(standard.cells[n]);
                    }
                }
            }


            var x = 20, y = 10;
            selectedTemplateId = id;
            var parentNode = result.list.filter(x => x.Type == "TEMPLATE");
            selectedTemplateType = parentNode[0].TemplateType;
            //  alert(selectedTemplateType);
            var childList = [];

            childList.push(parentNode[0]);

            createChildNode(childList.filter(x => x != undefined), x, y, result.list, null);
            HideLoader($('#dcontent'));
        }
    });
}

function setDaigramDetails(data) {
    document.getElementById("businessTitleLabel").innerText = data.TaskSubject;
}

function createChildNode(childList, x, y, result, parent) {

    for (var i = 0; i < childList.length; i++) {


        var subchild = result.map(function (el) {
            if (el.ParentIdList != null) {
                if (el.ParentIdList && childList[i]) {
                    var l = el.ParentIdList.filter(x => x == childList[i].Id);
                    if (l.length > 0) {
                        return el;
                    }
                }
            }
        });

        isParentAlreadyCreated = false
        var childNode;
        var parentNodes = [];
        var childNo;
        var cells = graph.toJSON().cells;
        cells = cells.filter(x => x.type != "standard.Link");
        debugger;

        for (var c = 0; c <= cells.length - 1; c++) {
            var val = Object.values(cells[c].attrs.refId).join('')
            if (val == childList[i].Id) {
                isParentAlreadyCreated = true;
                childNode = cells[c];
            }
            //for (var p = 0; p <= childList[i].ParentIdList.length - 1; p++) {
            //    if (val == childList[i].ParentIdList[p]) {
            //        parentNodes.push(cells[c])
            //    }
            //}
        }
        console.log(isParentAlreadyCreated);

        var isHavingChild = subchild.filter(x => x != undefined).length > 0 ? true : false;
       


            var wraptext = ""
            if (childList[i].Title != null && childList[i].Title != "") {
                wraptext = joint.util.breakText(childList[i].Title, {
                    width: 120,
                    height: 120
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
            if (childList[i].Type == "TEMPLATE") {
                bcolor = "#64C5EB";
            } else if (childList[i].Type == "UDF_ROOT") {
                bcolor = "#bee3db";
            } else if (childList[i].Type == "ACTION_ROOT") {
                bcolor = "#bee3db";
            } else if (childList[i].Type == "PRE_ACTION_ROOT") {
                bcolor = "#f5cac3";
            } else if (childList[i].Type == "POST_ACTION_ROOT") {
                bcolor = "#f28482";
            } else if (childList[i].Type == "UDF") {
                bcolor = "#264653";
            } else if (childList[i].Type == "PRE_ACTION") {
                bcolor = "#52b69a";
            } else if (childList[i].Type == "POST_ACTION") {
                bcolor = "#168aad";
            } else if (childList[i].Type == "WF_ROOT") {
                bcolor = "#bee3db";
            } else if (childList[i].Type == "WF_START" || childList[i].Type == "BR_START") {
                hcolor = "black";
                bcolor = "#FFD57E";
            } else if (childList[i].Type == "WF_STOP" || childList[i].Type == "BR_STOP") {
                hcolor = "white";
                bcolor = "#f15a42";
            } else if (childList[i].Type == "WF_STEP_TASK") {
                bcolor = "#9163cb";
            } else if (childList[i].Type == "WF_DECISION" || childList[i].Type == "BR_DECISION") {
                bcolor = "#f9c74f";
            }


            if (childList[i].Title == "True") {
                bcolor = "#90be6d";
                hcolor = "white";
            }

            if (childList[i].Title == "False") {
                bcolor = "#f94144";
                hcolor = "white";
            }

            //if (childList[i].Title == "Start") {
            //    bcolor = "#88d4ab";
            //    hcolor = "white";
            //}

            //if (childList[i].Title == "Stop") {
            //    bcolor = "#bf0603";
            //    hcolor = "white";
            //}

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
                circle.attr('parentIds', childList[i].ParentIdList);
                circle.attr('isHavingChild', isHavingChild);
                circle.attr('collapsed', false);
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
                ellipse.attr('parentIds', childList[i].ParentIdList);
                ellipse.attr('isHavingChild', isHavingChild);
                ellipse.attr('collapsed', false);
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
                rectangle.attr('parentIds', childList[i].ParentIdList);
                rectangle.attr('isHavingChild', isHavingChild);
                rectangle.attr('collapsed', false);
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
                rectangle.attr('parentIds', childList[i].ParentIdList);
                rectangle.attr('isHavingChild', isHavingChild);
                rectangle.attr('collapsed', false);
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
                polygon.attr('parentIds', childList[i].ParentIdList);
                polygon.attr('isHavingChild', isHavingChild);
                polygon.attr('collapsed', false);
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
                if (isParentAlreadyCreated == false) {
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
                    node.attr('parentIds', childList[i].ParentIdList);
                    node.attr('isHavingChild', isHavingChild);
                    node.attr('collapsed', false);
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

                    createChildNode(subchild.filter(x => x != undefined), x, y, result, node);
                } else {
                    debugger;
                    node = graph.getCell(childNode.id)
                    createlink(parent, node);
                    roundedCornerRect();
                    graphLayout();
                }
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
                node.attr('parentIds', childList[i].ParentIdList);
                node.attr('isHavingChild', isHavingChild);
                node.attr('collapsed', false);
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

var removeByAttr = function (arr, attr, value) {
    var i = arr.length;
    while (i--) {
        if (arr[i]
            && arr[i].hasOwnProperty(attr)
            && (arguments.length > 2 && arr[i][attr] === value)) {

            arr.splice(i, 1);

        }
    }
    return arr;
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

function checkIfTemplateALreadyonGraph() {
    if (graph) {
        var g = graph.toJSON();
        for (var i = 0; i <= g.cells.length - 1; i++) {
            var cellType = Object.values(g.cells[i].attrs.type).join('');
            if (cellType == "TEMPLATE") {
                return true;
            }
        }
    }
    return false;
}

function manageBusiness() {
    ShowLoader($('#dcontent'));

    var json = graph.toJSON();
    cells = json.cells;

    var standardShape = [];
    var templateId;
    for (var x = 0; x <= cells.length - 1; x++) {
        console.log(cells[x].type);
        if (cells[x].attrs.type) {
            var c = Object.values(cells[x].attrs.type).join('');
            if (c == "standard") {
                standardShape.push(cells[x])
            }
            if (c == "TEMPLATE") {
                templateId = cells[x].attrs.refId;
            }
        }
    }
    if (templateId) {

        //onTitleEdit();

        var name = document.getElementById("businessTitleLabel").innerText == "" ? "Business Diagram" : document.getElementById("businessTitleLabel").innerText;
        kendo.prompt("Business Diagram Title", name)
            .done(function (data) {
                document.getElementById("businessTitleLabel").innerText = data;
                var standardCells = { cells: standardShape }

                var tempId = Object.values(templateId).join('');

                var diagram = {
                    "TaskSubject": document.getElementById("businessTitleLabel").innerText,
                    "diagramJson": JSON.stringify(standardCells),
                    "diagramTemplateId": tempId
                };

                $.ajax({
                    type: "POST",
                    url: "/cms/BusinessDiagram/SaveBusinessDiagram",
                    data: diagram,
                    dataType: "json",
                    success: function (result) {
                        createTemplateDiagram(selectedTemplateId);
                        HideLoader($('#dcontent'));
                        ShowNotification("Saved Successfully", "success");
                    },
                    error: function (xhr, httpStatusMessage, customErrorMessage) {
                        HideLoader($('#mainContent'));
                        ShowNotification("Saved Successfully", "success");
                    }
                });
                console.log("User accepted with text: " + data);
            })
            .fail(function (data) {
                console.log("User rejected with text: " + data);
            });



    } else {
        HideLoader($('#dcontent'));
        ShowNotification("Template is not loaded", "error");
    }

}

function loadDiagram(tempId) {
    createTemplateDiagram(tempId);
    setTimeout(function () {
        graphLayout();

    }, 3000);
}

function onRefreshDiagram() {
    if (selectedTemplateId != "") {
        createTemplateDiagram(selectedTemplateId);
        setTimeout(function () { graphLayout(); }, 3000);
    } else {
        alert("Please select business diagram.");
    }
}

function OnSelectTemplate(e) {
    e.preventDefault();

}

function configureNode(s) {
   


}

function onCloseConfigureNode(config) {
    console.log(config);
    CloseIframePopup();
}

this.paper.on('element:pointerup', (elementView, x, y) => {
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

    var valStandard = Object.values(elementView.model.attributes.attrs.type).join('');
    if (valStandard == "standard") {
        document.getElementById("prop").style.display = "";
        onNodeSelection();
    } else {
        document.getElementById("prop").style.display = "none";
    }

});

paper.on('cell:pointerclick',
    function (cellView, evt, x, y) {
        var id = cellView.model.id;
    }
);



this.paper.on('element:contextmenu', (node, x, y) => {
    // 
    //console.log(node);
    $("#menulist li").remove();
    if (node.model.attributes.attrs.type == 'TEMPLATE') {
        $("#menulist").append("<li class='menu-option' onclick='onEditTemplate(\"Template - Edit Template\",\"" + node.model.attributes.attrs.refId + "\");'>Edit Template</li>"); //same template page
    } else if (node.model.attributes.attrs.type == 'UDF_ROOT') {
        $("#menulist").append("<li class='menu-option' onclick='onEditTemplate(\"Template - Add/Edit UDF\",\"" + selectedTemplateId + "\");'>Add/Edit UDF</li>"); // same template page
    } else if (node.model.attributes.attrs.type == 'UDF') {
        $("#menulist").append("<li class='menu-option' onclick='onEditTemplate(\"Template - Edit Udf\",\"" + selectedTemplateId + "\");'>Edit Udf</li>"); //same template page
    } else if (node.model.attributes.attrs.type == 'ACTION_ROOT') {
    } else if (node.model.attributes.attrs.type == 'PRE_ACTION_ROOT') {
        $("#menulist").append("<li class='menu-option' onclick='onEditPreBusinessRule(\"" + selectedTemplateId + "\",\"" + selectedTemplateType + "\");'>Create Business Rule</li>"); //Business Rule
    } else if (node.model.attributes.attrs.type == 'POST_ACTION_ROOT') {
        $("#menulist").append("<li class='menu-option' onclick='onEditPostBusinessRule(\"" + selectedTemplateId + "\",\"" + selectedTemplateType + "\");'>Create Business Rule</li>"); //BUsiness Rule
    } else if (node.model.attributes.attrs.type == 'PRE_ACTION') {
        $("#menulist").append("<li class='menu-option' onclick='onEditPreBusinessRule(\"" + selectedTemplateId + "\",\"" + selectedTemplateType + "\", \"" + node.model.attributes.attrs.dataModel.ReferenceId + "\");'>Edit Business Rule</li>"); //Business Rule
        $("#menulist").append("<li class='menu-option' onclick='OnRemoveNode(\"" + selectedTemplateId + "\",\"" + node.model.attributes.attrs.dataModel.ReferenceId + "\", \"RemoveBusinessRule\");'>Remove Business Rule</li>");
    } else if (node.model.attributes.attrs.type == 'POST_ACTION') {
        $("#menulist").append("<li class='menu-option' onclick='onEditPostBusinessRule(\"" + selectedTemplateId + "\",\"" + selectedTemplateType + "\", \"" + node.model.attributes.attrs.dataModel.ReferenceId + "\");'>Edit Business Rule</li>"); //BUsiness RUle
        $("#menulist").append("<li class='menu-option' onclick='OnRemoveNode(\"" + selectedTemplateId + "\",\"" + node.model.attributes.attrs.dataModel.ReferenceId + "\", \"RemoveBusinessRule\");'>Remove Business Rule</li>");
    } else if (node.model.attributes.attrs.type == 'WF_ROOT') {
        if (node.model.attributes.attrs.isHavingChild != true) {
            $("#menulist").append("<li class='menu-option' onclick='onCreateWorkFlow(\"" + node.model.attributes.attrs.refId + "\");'>Create Workflow</li>"); // WorkFlow
        }
        else {

            $("#menulist").append("<li class='menu-option' onclick='OnRemoveNode(\"" + selectedTemplateId + "\",\"\",\"RemoveWorkflow\");'>Remove Workflow</li>");
        }

    } else if (node.model.attributes.attrs.type == 'BR_START') {
        $("#menulist").append("<li class='menu-option' onclick='onCreateDecision(\"" + selectedTemplateId + "\",\"" + node.model.attributes.attrs.dataModel.Id + "\",\"" + node.model.attributes.attrs.dataModel.ReferenceId + "\");'>Add Decision</li>");
        $("#menulist").append("<li class='menu-option' onclick='onCreateProcess(\"" + selectedTemplateId + "\",\"" + node.model.attributes.attrs.dataModel.Id + "\",\"" + node.model.attributes.attrs.dataModel.ReferenceId + "\");'>Add Process</li>");
        if (node.model.attributes.attrs.isHavingChild != true) {
            $("#menulist").append("<li class='menu-option' onclick='onCreateStop(\"" + node.model.attributes.attrs.dataModel.Id + "\",\"" + node.model.attributes.attrs.dataModel.ReferenceId + "\");'>Add Stop</li>");
        }
    } else if (node.model.attributes.attrs.type == 'BR_DECISION') {
        $("#menulist").append("<li class='menu-option' onclick='onCreateDecision(\"" + selectedTemplateId + "\",\"" + node.model.attributes.attrs.dataModel.Id + "\",\"" + node.model.attributes.attrs.dataModel.ReferenceId + "\",true);'>Edit Decision</li>");
        $("#menulist").append("<li class='menu-option' onclick='OnRemoveNode(\"" + selectedTemplateId + "\",\"" + node.model.attributes.attrs.dataModel.Id + "\", \"RemoveBusinessRuleNode\");'>Remove Decision</li>");
    } else if (node.model.attributes.attrs.type == 'BR_TRUE') {
        $("#menulist").append("<li class='menu-option' onclick='onCreateDecision(\"" + selectedTemplateId + "\",\"" + node.model.attributes.attrs.dataModel.Id + "\",\"" + node.model.attributes.attrs.dataModel.ReferenceId + "\");'>Add Decision</li>");
        $("#menulist").append("<li class='menu-option' onclick='onCreateProcess(\"" + selectedTemplateId + "\",\"" + node.model.attributes.attrs.dataModel.Id + "\",\"" + node.model.attributes.attrs.dataModel.ReferenceId + "\");'>Add Process</li>");
        if (node.model.attributes.attrs.isHavingChild != true) {
            $("#menulist").append("<li class='menu-option' onclick='onCreateStop(\"" + node.model.attributes.attrs.dataModel.Id + "\",\"" + node.model.attributes.attrs.dataModel.ReferenceId + "\");'>Add Stop</li>");
        }
    } else if (node.model.attributes.attrs.type == 'BR_FALSE') {
        $("#menulist").append("<li class='menu-option' onclick='onCreateDecision(\"" + selectedTemplateId + "\",\"" + node.model.attributes.attrs.dataModel.Id + "\",\"" + node.model.attributes.attrs.dataModel.ReferenceId + "\");'>Add Decision</li>");
        $("#menulist").append("<li class='menu-option' onclick='onCreateProcess(\"" + selectedTemplateId + "\",\"" + node.model.attributes.attrs.dataModel.Id + "\",\"" + node.model.attributes.attrs.dataModel.ReferenceId + "\");'>Add Process</li>");
        if (node.model.attributes.attrs.isHavingChild != true) {
            $("#menulist").append("<li class='menu-option' onclick='onCreateStop(\"" + node.model.attributes.attrs.dataModel.Id + "\",\"" + node.model.attributes.attrs.dataModel.ReferenceId + "\");'>Add Stop</li>");
        }
    } else if (node.model.attributes.attrs.type == 'BR_PROCESS') {
        $("#menulist").append("<li class='menu-option' onclick='onCreateDecision(\"" + selectedTemplateId + "\",\"" + node.model.attributes.attrs.dataModel.Id + "\",\"" + node.model.attributes.attrs.dataModel.ReferenceId + "\");'>Add Decision</li>");
        $("#menulist").append("<li class='menu-option' onclick='onCreateProcess(\"" + selectedTemplateId + "\",\"" + node.model.attributes.attrs.dataModel.Id + "\",\"" + node.model.attributes.attrs.dataModel.ReferenceId + "\",true);'>Edit Process</li>");
        $("#menulist").append("<li class='menu-option' onclick='onCreateProcess(\"" + selectedTemplateId + "\",\"" + node.model.attributes.attrs.dataModel.Id + "\",\"" + node.model.attributes.attrs.dataModel.ReferenceId + "\");'>Add Process</li>");
        if (node.model.attributes.attrs.isHavingChild != true) {
            $("#menulist").append("<li class='menu-option' onclick='onCreateStop(\"" + node.model.attributes.attrs.dataModel.Id + "\",\"" + node.model.attributes.attrs.dataModel.ReferenceId + "\");'>Add Stop</li>");
        }
        $("#menulist").append("<li class='menu-option' onclick='OnRemoveNode(\"" + selectedTemplateId + "\",\"" + node.model.attributes.attrs.dataModel.Id + "\", \"RemoveBusinessRuleNode\");'>Remove Process</li>");
    } else if (node.model.attributes.attrs.type == 'BR_STOP') {
        $("#menulist").append("<li class='menu-option' onclick='OnRemoveNode(\"" + selectedTemplateId + "\",\"" + node.model.attributes.attrs.dataModel.Id + "\", \"RemoveBusinessRuleNode\");'>Remove Stop</li>");
    } else if (node.model.attributes.attrs.type == 'WF_START') {
        var nodeData = JSON.stringify(node.model);
        //node.model.attributes.attrs.isHavingChild = true;
        $("#menulist").append("<li class='menu-option' onclick='CreateNode(" + nodeData + ",1);'>Add Decision Script</li><li class='menu-option' onclick='CreateNode(" + nodeData + ",2);'>Add Step Task</li><li class='menu-option' onclick='CreateNode(" + nodeData + ",3);'>Stop Workflow</li>");
    } else if (node.model.attributes.attrs.type == 'WF_FALSE' || node.model.attributes.attrs.type == 'WF_TRUE') {
        var nodeData = JSON.stringify(node.model);
        $("#menulist").append("<li class='menu-option' onclick='CreateNode(" + nodeData + ",1);'>Add Decision Script</li><li class='menu-option' onclick='CreateNode(" + nodeData + ",2);'>Add Step Task</li><li class='menu-option' onclick='CreateNode(" + nodeData + ",3);'>Stop Workflow</li>");
        // $("#menulist").append("<li>If yes, Create<ul ><li class='menu-option' onclick='CreateNode(" + nodeData + ", 1);'>Decision</li><li class='menu-option'  onclick='CreateNode(" + nodeData + ", 2);'>Process</li><li class='menu-option'  onclick='CreateNode(" + nodeData + ", 3);'>Stop</li></ul></li><li >If No, Create<ul ><li class='menu-option' onclick='CreateNode(" + nodeData + ", 1);'>Decision</li><li class='menu-option' onclick='CreateNode(" + nodeData +", 2);'>Process</li><li class='menu-option' onclick='CreateNode(" + nodeData +", 3);'>Stop</li></ul></li><li>View Details</li>");
    } else if (node.model.attributes.attrs.type == 'WF_STEP_TASK') {
        var nodeData = JSON.stringify(node.model);
        console.log(node);
        $("#menulist").append("<li class='menu-option' onclick='onStepTaskDiagram(\"" + node.model.attributes.attrs.dataModel.ReferenceId + "\");'>Open Step Task Diagram</li><li class='menu-option' onclick='CreateNode(" + nodeData + ", 4);'>Add Existing Component</li>");
        $("#menulist").append("<li class='menu-option' onclick='CreateNode(" + nodeData + ", 2,true);'>Edit Step Task</li><li class='menu-option' onclick='CreateNode(" + nodeData + ", 1);'>Add Decision Script</li><li class='menu-option' onclick='CreateNode(" + nodeData + ", 2);'>Add Step Task</li><li class='menu-option' onclick='CreateNode(" + nodeData + ", 3);'>Stop Workflow</li>");
        $("#menulist").append("<li class='menu-option' onclick='OnRemoveNode(\"" + selectedTemplateId + "\",\"" + node.model.attributes.attrs.dataModel.Id + "\", \"RemoveComponent\");'>Remove Step Task</li>");
    } else if (node.model.attributes.attrs.type == 'WF_STOP') {
        $("#menulist").html('');
        $("#menulist").append("<li class='menu-option' onclick='OnRemoveNode(\"" + selectedTemplateId + "\",\"" + node.model.attributes.attrs.dataModel.Id + "\", \"RemoveComponent\");'>Remove Stop Workflow</li>");
    } else if (node.model.attributes.attrs.type == 'WF_DECISION') {
        var nodeData = JSON.stringify(node.model);
        $("#menulist").append("<li class='menu-option' onclick='CreateNode(" + nodeData + ", 1,true);'>Edit Decision Script</li>");
        $("#menulist").append("<li class='menu-option' onclick='OnRemoveNode(\"" + selectedTemplateId + "\",\"" + node.model.attributes.attrs.dataModel.Id + "\", \"RemoveComponent\");'>Remove Decision</li>");
    }

    const origin = {
        left: x.pageX,
        top: x.pageY
    };
    setPosition(origin);

    return false;
});

this.paper.on('link:mouseenter', function (linkView) {
    //linkView.addTools(linktools);
});

this.paper.on('link:mouseleave', function (linkView) {
    //linkView.removeTools();
});

function refreshDiagram(node) {
    graph.clear();
    graph.addCell(node);
    createTemplateNodes(node);
}

function CallBackMethod() {

    var json = graph.toJSON();
    cells = json.cells;
    var rootNode = cells[0];
    var tst = paper.findViewByModel(rootNode);
    refreshDiagram(tst.model);
}

function onCreateWorkFlow(id) {
    var json = graph.toJSON();
    cells = json.cells;
    //var template=cells.filter(getTemplate);
    var rootNode = cells[0];
    var cp = Object.values(rootNode.attrs.type).join('');
    //if (cp == "Service") {
    if (cp == "TEMPLATE") {
        var templateId = Object.values(rootNode.attrs.refId).join('');
        $.ajax({
            url: '/cms/ProcessDesign/CreateProcessDesign?templateId=' + templateId,
            dataType: "json",
            success: function (result) {
                if (result.success) {
                    var tst = paper.findViewByModel(rootNode);
                    refreshDiagram(tst.model);
                }
            }
        });
    }

}

function CreateNode(node, type, edit)// type=1:Decision ;type=2:Step;type=3:Stop
{
    var cellView = paper.findViewByModel(node);
    if (type == "1") {
        var templateId = "";
        var json = graph.toJSON();
        cells = json.cells;
        var rootNode = cells[0];
        var cp = Object.values(rootNode.attrs.type).join('');
        if (cp == "TEMPLATE") {
            templateId = Object.values(rootNode.attrs.refId).join('');
        }
        var parentId = Object.values(node.attrs.refId).join('');
        var win = GetMainWindow();
        if (edit == true) {
            win.iframeOpenUrl = '/bre/BusinessRule/RuleBuilder?id=' + parentId + '&templateId=' + templateId + '&decisionParentId=' + parentId + '&isWorkFlow=true';
            win.OpenWindow({ Title: 'View Details', Width: 800, Height: 600 });
        }
        else {
            win.iframeOpenUrl = '/bre/BusinessRule/RuleBuilder?nodeId=&ruleId=&templateId=' + templateId + '&decisionParentId=' + parentId + '&isWorkFlow=true';//+ NodeId;
            win.OpenWindow({ Title: 'View Details', Width: 800, Height: 600 });
        }

        //var decision1 = new joint.shapes.standard.Polygon();
        //decision1.resize(80, 80);
        //decision1.position(20, 150);
        //decision1.attr('root/title', 'joint.shapes.standard.Polygon');
        //decision1.attr('label/text', 'Decision');
        //decision1.attr('body/fill', 'lightblue');
        //decision1.attr('type', "WF_DECISION");
        //decision1.attr('refId', '');      
        //decision1.attr('parentIds', node.id);
        //decision1.attr('isHavingChild', false);
        //decision1.attr('collapsed', false);
        //var cp = Object.values(node.id).join('');
        //decision1.attr('collapsedPath', node.attrs.collapsedPath + cp + "|");
        //graph.addCell(decision1);
        //var link = new joint.shapes.standard.Link();
        //link.source(node);
        //link.target(decision1); 
        //link.attr('collapsedPath', node.attrs.collapsedPath + "|" + link.id);
        //link.addTo(graph);       

        //// left child
        //var trueDecision = new joint.shapes.standard.Rectangle();
        //trueDecision.resize(80, 80);
        //trueDecision.position(20, 10);
        //trueDecision.attr('root/title', 'joint.shapes.standard.Rectangle');
        //trueDecision.attr('label/text', 'True');
        //trueDecision.attr('body/fill', 'green');
        //trueDecision.attr('type', "WF_DECISION_TRUE");
        //trueDecision.attr('refId', '');
        //trueDecision.attr('parentIds', decision1.id);
        //trueDecision.attr('isHavingChild', false);
        //trueDecision.attr('collapsed', false);
        //var cp = Object.values(decision1.id).join('');
        //trueDecision.attr('collapsedPath', decision1.attributes.attrs.collapsedPath + cp + "|");
        //graph.addCell(trueDecision);
        //var link = new joint.shapes.standard.Link();
        //link.source(decision1);
        //link.target(trueDecision);   
        //link.attr('collapsedPath', decision1.attributes.attrs.collapsedPath + "|" + link.id);
        //link.addTo(graph);
        //// right child
        //var falseDecision = new joint.shapes.standard.Rectangle();
        //falseDecision.resize(80, 80);
        //falseDecision.position(20, 150);
        //falseDecision.attr('root/title', 'joint.shapes.standard.Rectangle');
        //falseDecision.attr('label/text', 'false');
        //falseDecision.attr('body/fill', 'red');
        //falseDecision.attr('type', "WF_DECISION_FALSE");
        //falseDecision.attr('refId', '');
        //falseDecision.attr('parentIds', decision1.id);
        //falseDecision.attr('isHavingChild', false);
        //falseDecision.attr('collapsed', false);
        //var cp = Object.values(decision1.id).join('');
        //falseDecision.attr('collapsedPath', decision1.attributes.attrs.collapsedPath + cp + "|");
        //graph.addCell(falseDecision);
        //var link = new joint.shapes.standard.Link();
        //link.source(decision1);
        //link.target(falseDecision);   
        //link.attr('collapsedPath', decision1.attributes.attrs.collapsedPath + "|" + link.id);
        //link.addTo(graph);
    }
    if (type == "2") {

        var templateId = "";
        var json = graph.toJSON();
        cells = json.cells;
        var rootNode = cells[0];
        var cp = Object.values(rootNode.attrs.type).join('');
        if (cp == "TEMPLATE") {
            // if (cp == "TEMPLATE") {
            templateId = Object.values(rootNode.attrs.refId).join('');
        }
        //var ProcessDesignId = Object.values(cells[i].attrs.processDesignId).join('');
        var parentId = Object.values(node.attrs.refId).join('');
        var win = GetMainWindow();
        if (edit == true) {
            win.iframeOpenUrl = '/cms/ProcessDesign/ManageStepTask?id=' + parentId + '&serviceTemplateId=' + templateId + '&parentId=' + parentId + '&isDiagram=true&lo=Popup';
            win.OpenWindow({ Title: 'Step Task', Width: 800, Height: 600 });
        }
        else {
            win.iframeOpenUrl = '/cms/ProcessDesign/ManageStepTask?serviceTemplateId=' + templateId + '&parentId=' + parentId + '&isDiagram=true&lo=Popup';
            win.OpenWindow({ Title: 'Step Task', Width: 800, Height: 600 });
        }

        return false;
        //var step1 = new joint.shapes.standard.Rectangle();
        //step1.resize(80, 80);
        //step1.position(20, 150);
        //step1.attr('root/title', 'joint.shapes.standard.Rectangle');
        //step1.attr('label/text', 'Step Task');
        //step1.attr('body/fill', 'lightgray');
        //step1.attr('type', "WF_STEP_TASK");
        //step1.attr('refId', '');
        //step1.attr('parentIds', node.id);
        //step1.attr('isHavingChild', false);
        //step1.attr('collapsed', false);
        //var cp = Object.values(node.id).join('');
        //step1.attr('collapsedPath', node.attrs.collapsedPath + cp + "|");
        //graph.addCell(step1);



        //var link = new joint.shapes.standard.Link();
        //link.source(node);
        //link.target(step1);
        //link.attr('collapsedPath', node.attrs.collapsedPath + "|" + link.id);       
        //link.addTo(graph);
    }
    if (type == "3") {
        var templateId = "";
        var json = graph.toJSON();
        cells = json.cells;
        var rootNode = cells[0];
        var cp = Object.values(rootNode.attrs.type).join('');
        if (cp == "TEMPLATE") {
            // if (cp == "TEMPLATE") {
            templateId = Object.values(rootNode.attrs.refId).join('');
        }
        // var ProcessDesignId = Object.values(cells[i].attrs.processDesignId).join('');
        var parentId = Object.values(node.attrs.refId).join('');
        $.ajax({
            url: '/cms/ProcessDesign/CreateComponent',
            type: "POST",
            data:
            {
                ComponentType: "Stop",
                Name: "Stop WorkFlow",
                ParentId: parentId,
                TemplateId: templateId
                //ProcessDesignId: ProcessDesignId
            },
            dataType: "json",
            success: function (result) {
                if (result.success) {
                    var tst = paper.findViewByModel(rootNode);
                    refreshDiagram(tst.model);
                }
            }
        });
        //var ellipse1 = new joint.shapes.standard.Ellipse();
        //ellipse1.resize(80, 80);
        //ellipse1.position(20, 10);
        //ellipse1.attr('root/title', 'joint.shapes.standard.Ellipse');
        //ellipse1.attr('label/text', 'Stop');
        //ellipse1.attr('body/fill', 'lightgray');
        //ellipse1.attr('type', "WF_STOP");
        //ellipse1.attr('refId', '');
        //ellipse1.attr('parentIds', node.id);
        //ellipse1.attr('isHavingChild', false);
        //ellipse1.attr('collapsed', false);
        //var cp = Object.values(node.id).join('');
        //ellipse1.attr('collapsedPath', node.attrs.collapsedPath + cp + "|");
        //graph.addCell(ellipse1);

        //var link = new joint.shapes.standard.Link();
        //link.source(node);
        //link.target(ellipse1);
        //link.attr('collapsedPath', node.attrs.collapsedPath + "|" + link.id);
        ////link.addTo(flyGraph);
        //link.addTo(graph);
    }
    if (type == "4") {
        var templateId = "";
        var json = graph.toJSON();
        cells = json.cells;
        var rootNode = cells[0];
        var cp = Object.values(rootNode.attrs.type).join('');
        if (cp == "TEMPLATE") {
            // if (cp == "TEMPLATE") {
            templateId = Object.values(rootNode.attrs.refId).join('');
        }

        var parentId = Object.values(node.attrs.refId).join('');
        var win = GetMainWindow();
        win.iframeOpenUrl = '/cms/ProcessDesign/AddExistingComponent?parentId=' + parentId + '&templateId=' + templateId;
        win.OpenWindow({ Title: 'Add Existing Component', Width: 500, Height: 450 });
        return false;

    }
    cellView.model.attributes.attrs.isHavingChild = true;
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

        if (elementView.model.attributes.attrs.type == "standard") {
            document.getElementById("prop").style.display = "";
        } else {
            document.getElementById("prop").style.display = "none";
        }
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
        removeButton
    ]
});
var onlyBasicTool = new joint.dia.ToolsView({
    tools: [
        boundaryTool,
        //removeButton,
        // removeButton
    ]
});


function onLevelChange() {
    var el;
    if (lastClickedElementView) {
        el = lastClickedElementView.model;
    }
    else {
        el = draggedElement.attributes;
    }
    var cell = graph.getCell(el.id);
    var level = $("#level").data("kendoDropDownList").value();
    var l = Object.values(level).join('');
    cell.attr('level', l);
}

function onLevelChangeFilter() {

    if (lastClickedElementView) {
        joint.highlighters.mask.remove(lastClickedElementView);
    }

    document.getElementById("removeFilter").style.display = "";
    var level = $("#levelFilter").data("kendoDropDownList").value();
    var cells = graph.toJSON().cells;
    var selectedChilds = [];
    for (var i = 0; i <= cells.length - 1; i++) {
        if (cells[i].attrs.level) {
            var cp = Object.values(cells[i].attrs.level).join('');
            var id = $("[model-id=" + cells[i].id + "]")[0].id;
            if (cp != level) {
                document.getElementById(id).style.display = "none";
            } else {
                document.getElementById(id).style.display = "";
            }
        } else {
            var id = $("[model-id=" + cells[i].id + "]")[0].id;
            document.getElementById(id).style.display = "none";
        }
    }
    //for (var i = 0; i <= selectedChilds.length - 1; i++) {
    //    var id = $("[model-id=" + selectedChilds[i].id + "]")[0].id;
    //    document.getElementById(id).style.display = "none";
    //}
}

function removeLevelFilter() {

    document.getElementById("removeFilter").style.display = "none";
    $("#levelFilter").data("kendoDropDownList").value("");
    var cells = graph.toJSON().cells;
    for (var i = 0; i <= cells.length - 1; i++) {
        var id = $("[model-id=" + cells[i].id + "]")[0].id;
        document.getElementById(id).style.display = "";
    }
}

function onNodeFill() {
    var el;
    if (lastClickedElementView) {
        el = lastClickedElementView.model;
    }
    else {
        el = draggedElement.attributes;
    }
    var cell = graph.getCell(el.id);
    var fill = $("#nfill").data("kendoColorPicker").value();
    cell.attr('body/fill', fill);
}

function onNodeOutline() {
    var el;
    if (lastClickedElementView) {
        el = lastClickedElementView.model;
    }
    else {
        el = draggedElement.attributes;
    }
    var cell = graph.getCell(el.id);
    var noutline = $("#noutline").data("kendoColorPicker").value();
    cell.attr('body/stroke', noutline);
}

function onborderThickness() {
    var el;
    if (lastClickedElementView) {
        el = lastClickedElementView.model;
    }
    else {
        el = draggedElement.attributes;
    }
    var cell = graph.getCell(el.id);
    var nborderthickness = $("#borderThickness").val();
    cell.attr('body/strokeWidth', nborderthickness);
}

function onWidthHeightChange() {
    var el;
    if (lastClickedElementView) {
        el = lastClickedElementView.model;
    }
    else {
        el = draggedElement.attributes;
    }
    var cell = graph.getCell(el.id);
    var nwidth = $("#nwidth").data("kendoNumericTextBox").value();
    var nheight = $("#nheight").data("kendoNumericTextBox").value();
    cell.size(nwidth, nheight)
    graphLayout();
}

function onTextChange() {
    var el;
    if (lastClickedElementView) {
        el = lastClickedElementView.model;
    }
    else {
        el = draggedElement.attributes;
    }
    var cell = graph.getCell(el.id);

    var ndisplaytext = $("#ndisplaytext").data("kendoTextBox").value();
    var wraptextLabel = joint.util.breakText(ndisplaytext, {
        width: cell.size().width,
        height: cell.size().height
    });
    cell.attr('root/title', wraptextLabel);
    cell.attr('label/text', wraptextLabel);
}

function changeFontSize() {
    document.getElementById("font-size").innerText = document.getElementById('tfontsize').value + "px";
    var el;
    if (lastClickedElementView) {
        el = lastClickedElementView.model;
    }
    else {
        el = draggedElement.attributes;
    }
    var cell = graph.getCell(el.id);
    var tfontsize = $("#tfontsize").val();
    cell.attr('label/fontSize', tfontsize);

}

function onChangeTextColor() {
    document.getElementById("font-size").innerText = document.getElementById('tfontsize').value + "px";
    var el = draggedElement.attributes;
    var el;
    if (lastClickedElementView) {
        el = lastClickedElementView.model;
    }
    else {
        el = draggedElement.attributes;
    }
    var cell = graph.getCell(el.id);
    tcolor = $("#tcolor").data("kendoColorPicker").value();
    cell.attr('label/fill', tcolor);
}

function onUpdate() {
    var el = lastClickedElementView.model.attributes;
    level = $("#level").data("kendoDropDownList").value();
    nfill = $("#nfill").data("kendoColorPicker").value();
    noutline = $("#noutline").data("kendoColorPicker").value();
    nborderthickness = $("#borderThickness").val();
    nwidth = $("#nwidth").data("kendoNumericTextBox").value();
    nheight = $("#nheight").data("kendoNumericTextBox").value();
    ndisplaytext = $("#ndisplaytext").data("kendoTextBox").value();
    tfontsize = $("#tfontsize").val();
    tcolor = $("#tcolor").data("kendoColorPicker").value();
    var wraptextLabel = joint.util.breakText(ndisplaytext, {
        width: el.size.width,
        height: el.size.height
    });
    var cell = graph.getCell(lastClickedElementView.model.id);
    console.log(cell);
    if (cell.attributes.attrs.type == "standard") {
        cell.attr('level', level);
        cell.attr('label/text', wraptextLabel);
        cell.attr('label/fill', tcolor);
        cell.attr('label/fontSize', tfontsize);
        cell.attr('body/fill', nfill);
        cell.attr('body/stroke', noutline);
        cell.attr('body/strokeWidth', nborderthickness);
    }
    console.log(lastClickedElementView);


}

function onNodeSelection() {
    var el = lastClickedElementView.model.attributes;
    var cell = graph.getCell(lastClickedElementView.model.id);
    $("#level").data("kendoDropDownList").value(cell.attributes.attrs.level);
    $("#nfill").data("kendoColorPicker").value(cell.attributes.attrs.body.fill);
    $("#noutline").data("kendoColorPicker").value(cell.attributes.attrs.body.stroke);
    $("#borderThickness").val(cell.attributes.attrs.body.strokeWidth);
    $("#nwidth").data("kendoNumericTextBox").value(cell.attributes.size.width);
    $("#nheight").data("kendoNumericTextBox").value(cell.attributes.size.height);
    $("#ndisplaytext").data("kendoTextBox").value(cell.attributes.attrs.label.text);
    $("#tfontsize").val(cell.attributes.attrs.label.fontSize);
    $("#tcolor").data("kendoColorPicker").value(cell.attributes.attrs.label.fill);

}

function onClickTask(prm) {
    alert("Hello Task : " + prm);

}

function onEditTemplate(title, tempid) {
    //alert("Template Id :" + tempid);
    var win = GetMainWindow();
    win.iframeOpenUrl = '/Cms/Template/Template?templateId=' + tempid + "&lo=Popup&cbm=TemplateChangeCallback";
    win.OpenWindow({ Title: title, Width: 1000, Height: 650 });
    return false;
}
function onEditPreBusinessRule(templateId, temptype, ruleId) {
    // alert(temptype);
    var win = GetMainWindow();
    win.iframeOpenUrl = '/Bre/BusinessRule/CreateBusinessRule?templateId=' + templateId + '&templateType=' + temptype + '&type=PreSubmit' + '&businessRuleId=' + ruleId;
    win.OpenWindow({ Title: 'BusinessRule', Width: 500, Height: 600 });
    return false;
}
function onEditPostBusinessRule(templateId, temptype, ruleId) {
    var win = GetMainWindow();
    win.iframeOpenUrl = '/Bre/BusinessRule/CreateBusinessRule?templateId=' + templateId + '&templateType=' + temptype + '&type=PostSubmit' + '&businessRuleId=' + ruleId;
    win.OpenWindow({ Title: 'BusinessRule', Width: 500, Height: 600 });
    return false;
}
function OnRemoveNode(tempid, nodeId, operation) {
    var flag = confirm('Are you sure you want to delete this data?');
    if (flag) {
        ShowLoader($('#dcontent'));
        $.ajax({
            url: '/Bre/BusinessRuleDiagram/RemoveDiagramNode?templateId=' + tempid + '&nodeId=' + nodeId + '&operation=' + operation,
            // data: dataModel,
            dataType: "json",
            success: function (result) {
                if (result.success) {
                    //alert(result.nodeId);
                    /*refreshDiagram(tst.model);*/
                    HideLoader($('#dcontent'));
                    CallBackMethod();
                }
            }
        });
    }
}
var isBackbutton = false;
var parentServiceId = "";
function onStepTaskDiagram(refId) {
    document.getElementById("bckbtn").style.display = "block";
    isBackbutton = true;
    parentServiceId = selectedTemplateId;
    createTemplateDiagram(refId);
}

function onbackButton() {
    document.getElementById("bckbtn").style.display = "none";
    createTemplateDiagram(parentServiceId);
}

function onCreateDecision(tempid, nodeid, ruleid, edit) {

    //alert("Decision");
    //var nodeid = "60ae382a102bd3d53a6eabdf";
    //var ruleid = "973afa06-a58a-4238-9972-1c7a6d9f8935";
    //var tempid = "e478fef5-8ed5-43df-9386-93122b7eab46";
    var win = GetMainWindow();
    if (edit == true) {
        win.iframeOpenUrl = '/Bre/BusinessRule/RuleBuilder?id=' + nodeid + '&ruleId=' + ruleid + '&templateId=' + tempid + '&decisionParentId=' + nodeid + '&isBusinessDiagram=true';
        win.OpenWindow({ Title: 'Decision', Width: 800, Height: 600 });
    }
    else {
        win.iframeOpenUrl = '/Bre/BusinessRule/RuleBuilder?decisionParentId=' + nodeid + '&ruleId=' + ruleid + '&templateId=' + tempid + '&isBusinessDiagram=true';
        win.OpenWindow({ Title: 'Decision', Width: 800, Height: 600 });
    }

    return false;
}

function onCreateProcess(tempid, rulenodeid, ruleid, edit) {
    //alert("Process");
    //var rulenodeid = "60ae2fc393051a65d68bcbd9";
    //var tempid = "96c082f5-59d5-4fc9-b95e-0e5b161f4101";
    var win = GetMainWindow();
    if (edit == true) {
        win.iframeOpenUrl = '/Bre/Bre/BreResultViewDetails?businessRuleNodeId=' + rulenodeid + '&templateId=' + tempid + "&ruleId=" + ruleid + "&isBusinessDiagram=true";
        win.OpenWindow({ Title: 'Process', Width: 800, Height: 600 });
    }
    else {
        win.iframeOpenUrl = '/Bre/Bre/BreResultViewDetails?parentId=' + rulenodeid + '&templateId=' + tempid + "&ruleId=" + ruleid + "&isBusinessDiagram=true";
        win.OpenWindow({ Title: 'Process', Width: 800, Height: 600 });
    }


    return false;
}

function onCreateStop(nodeid, ruleid) {
    //alert("Stop");
    //win.iframeOpenUrl = '';
    //win.OpenWindow({ Title: 'Stop', Width: 800, Height: 600 });
    var dataModel = {
        Name: "Stop Business Rule",
        BusinessRuleId: ruleid,
        Type: "Terminator",
        IsStarter: false,
        SourceId: nodeid,
    };
    $.ajax({
        url: '/Bre/BusinessRuleDiagram/CreateNode',
        data: dataModel,
        dataType: "json",
        success: function (result) {
            if (result.success) {
                //alert(result.nodeId);
                /*refreshDiagram(tst.model);*/
                CallBackMethod();
            }
        }
    });
    return false;
}

if (selectedTemplateId != null) {
    //document.getElementById("edit_title").style.display = "none";
}

function onTitleEdit() {
    //document.getElementById("titleLabel").style.display = "none";
    //document.getElementById("titleText").style.display = "";
    var name = document.getElementById("businessTitleLabel").innerText == "" ? "Business Diagram" : document.getElementById("businessTitleLabel").innerText;
    kendo.prompt("Business Diagram Title", name)
        .done(function (data) {
            document.getElementById("businessTitleLabel").innerText = data;
            console.log("User accepted with text: " + data);
        })
        .fail(function (data) {
            console.log("User rejected with text: " + data);
        });
}

var splitter = $('.splitter-container').height(200).split({
    orientation: 'vertical',
    limit: 10,
    position: '20%', // if there is no percentage it interpret it as pixels
    onDrag: function (event) {
        console.log(splitter.position());
    }
});