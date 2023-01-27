ej.diagrams.Diagram.Inject(ej.diagrams.DataBinding, ej.diagrams.ComplexHierarchicalTree, ej.diagrams.LineDistribution);
var items;
var diagram;
var nodeId = 0;
var level = 0;
$.ajax({
    url: "/cms/processDesign/GetChart?processdesignId=" + document.getElementById("posId").innerText,
    contentType: "application/json",
    cache: false,
    success: function (result) {
        
        console.log(result);
        items = new ej.data.DataManager(result);
        var snapSettings = {
            // Define the Constraints for gridlines and snapping
            constraints: "None",
        };
        diagram = new ej.diagrams.Diagram({
            width: '100%',
            height: '590px',
            //Uses layout to auto-arrange nodes on the diagram page
            layout: {
                //Sets layout type
                type: 'ComplexHierarchicalTree',
                connectionPointOrigin: ej.diagrams.ConnectionPointOrigin.DifferentPoint,
                horizontalSpacing: 40, verticalSpacing: 40, horizontalAlignment: "Left", verticalAlignment: "Top",
                margin: { left: 0, right: 0, top: 0, bottom: 0 },
                orientation: 'TopToBottom'
            },//Configures data source for diagram
            dataSourceSettings: {
                id: 'Id',
                parentId: 'Parents',
                dataManager: items
            },//Sets the default properties for nodes
            getNodeDefaults: (obj) => {
                var bgcolor;
                var pdisplay;
                if (obj.data.ComponentType == 1) {
                    bgcolor = "#900c3e";
                    pdisplay = "none";
                } else if (obj.data.ComponentType == 10) {
                    bgcolor = "#ffc300";
                } else if (obj.data.ComponentType == 4) {
                    bgcolor = "#239adc";
                } else if (obj.data.ComponentType == 8) {
                    bgcolor = "#108e2f";
                } else if (obj.data.ComponentType == 9) {
                    bgcolor = "#fa1b17";
                } else if (obj.data.ComponentType == 7) {
                    bgcolor = "#6d6e70";
                } else if (obj.data.ComponentType == 2) {
                    bgcolor = "#000000";
                    pdisplay = "none";
                }
                obj.shape = {
                    type: 'HTML', content: "<div id=" + obj.data.Id + " style='background-color:"+ bgcolor+
                        "' class='row node-style'><div class='col'><img src='https://www.w3schools.com/w3images/avatar2.png' alt='Avatar' class='avatar'>"
                        + obj.data.Name + "</div><div  class='col menu' >" +
                        "<div class='dropdown'><button class='dropbtn fas fa-ellipsis-v btn-menu'></button> " +
                        "<div class='dropdown-content'>" +
                        "<a href='#' style='display:" + pdisplay + "' id='parent_" + obj.data.Id + "' onclick='onAddParent(this)'>Choose Parent</a>" +
                        "<a href='#' id='child_" + obj.data.Id + "' onclick='s(this)'>Choose Child</a>" +
                        "<a href='#' id='step_" + obj.data.Id + "' onclick='onAddStep(this)'>Add Step</a>" +
                        "<a href='#' id='decision_" + obj.data.Id + "' onclick='onAddDecision(this)'>Add Decision</a>" +
                        "<a href='#' id='execution_" + obj.data.Id + "' onclick='onAddExecution(this)'>Add Execution</a>" +
                        "<a href='#' id='stop_" + obj.data.Id + "' onclick='onStop(this)'>Add Stop</a>" +
                        "</div>" +
                        "</div>" +
                        "</div ></div >",
                };
                obj.style = { fill: "#c15433", strokeColor: 'none', strokeWidth: 2 };
                obj.width = 100; obj.height = 70;
                return obj;
            },//Sets the default properties for and connectors
            getConnectorDefaults: (connector, diagram) => {
                connector.type = 'Orthogonal';
                connector.cornerRadius = 7;
                connector.targetDecorator.height = 7;
                connector.targetDecorator.width = 7;
                connector.style.strokeColor = '#6d6d6d';
                return connector;
            },
            snapSettings: snapSettings,
        }, '#element');
        diagram.fitToPage({ mode: 'Width' });
        //diagram.tool = ej.diagrams.DiagramTools.ZoomPan;
    },
    error: function (xhr, textStatus, errorThrown) {
        // alert(errorThrown);
    }
});

var data = [
    { "Name": "node11", "fillColor": "#e7704c", "border": "#c15433" },
    { "Name": "node21", "fillColor": "#e7704c", "border": "#c15433" },
    { "Name": "node31", "fillColor": "#e7704c", "border": "#c15433" },
    { "Name": "node114", "ReportingPerson": ["node11", "node21", "node31"], "fillColor": "#f3904a", "border": "#d3722e" }
];
var items = new ej.data.DataManager(data);


function onAddParent(e) {
    var id = e.id.split("_")[1];
    var parents = this.diagram.dataSourceSettings.dataManager.dataSource.json.find(x => x.Id == id).Parents;
    
    var nodes = this.diagram.dataSourceSettings.dataManager.dataSource.json.splice(1);
    window.parent.iframeOpenUrl = "/cms/ProcessDesign/ChooseParentNode?parents=" + JSON.stringify(parents) + "&nodes=" + JSON.stringify(nodes);
    window.parent.OpenIframePopup(650, 650, 'Choose Parent');
}

function onAddChild() {}

function onAddStep(e) {
    var id = e.id.split("_")[1];
    var newId = GenerateGuid();
    var level = this.diagram.dataSourceSettings.dataManager.dataSource.json.find(x => x.Id == id).Level;
    if (e.id.split("_")[0].includes("parent_")) {
        this.diagram.dataSourceSettings.dataManager.dataSource.json.push({ "Id": newId, "Name": "Step", "Parents": [id], "ComponentType": 4, "Level": level});
    } else {
        this.diagram.dataSourceSettings.dataManager.dataSource.json.push({ "Id": newId, "Name": "Step", "Parents": [id], "ComponentType": 4, "Level": level + 1 });
    }
    
    this.diagram.clear();
    this.diagram.refresh();

}

function onAddDecision(e) {
    var id = e.id.split("_")[1];
    var newId = GenerateGuid();
    var level = this.diagram.dataSourceSettings.dataManager.dataSource.json.find(x => x.Id == id).Level;
    this.diagram.dataSourceSettings.dataManager.dataSource.json.push({ "Id": newId, "Name": "True", "Parents": [id], "ComponentType": 8, "Level": level + 1 });
    newId = GenerateGuid();
    this.diagram.dataSourceSettings.dataManager.dataSource.json.push({ "Id": newId, "Name": "False", "Parents": [id], "ComponentType": 9, "Level": level + 1 });
    this.diagram.clear();
    this.diagram.refresh();

}

function onAddExecution(e) {
    var id = e.id.split("_")[1];
    var newId = GenerateGuid();
    var level = this.diagram.dataSourceSettings.dataManager.dataSource.json.find(x => x.Id == id).Level;
    this.diagram.dataSourceSettings.dataManager.dataSource.json.push({ "Id": newId, "Name": "Execution", "Parents": [id], "ComponentType": 7, "Level": level + 1 });
    this.diagram.clear();
    this.diagram.refresh();
}

function onStop(e) {
    var id = e.id.split("_")[1];
    var newId = GenerateGuid();
    var level = this.diagram.dataSourceSettings.dataManager.dataSource.json.find(x => x.Id == id).Level;
    this.diagram.dataSourceSettings.dataManager.dataSource.json.push({ "Id": newId, "Name": "Stop", "Parents": [id], "ComponentType": 2, "Level": level + 1 });
    this.diagram.clear();
    this.diagram.refresh();
}