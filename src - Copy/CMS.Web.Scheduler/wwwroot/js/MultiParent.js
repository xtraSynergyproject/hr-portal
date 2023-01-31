ej.diagrams.Diagram.Inject(ej.diagrams.DataBinding, ej.diagrams.ComplexHierarchicalTree, ej.diagrams.LineDistribution);
var data = [
    { "Name": "node11", "fillColor": "#e7704c", "border": "#c15433" },
    { "Name": "node21", "fillColor": "#e7704c", "border": "#c15433" },
    { "Name": "node31", "fillColor": "#e7704c", "border": "#c15433" },
    { "Name": "node114", "ReportingPerson": ["node11", "node21", "node31"], "fillColor": "#f3904a", "border": "#d3722e" }
];
var items = new ej.data.DataManager(data);

var diagram = new ej.diagrams.Diagram({
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
        id: 'Name',
        parentId: 'ReportingPerson',
        dataManager: items
    },//Sets the default properties for nodes
    getNodeDefaults: (obj) => {
        //obj.width = 40; obj.height = 40;
        obj.shape = {
            type: 'HTML', content: "<div id=" + obj.data.Name +" style='background-color:" + obj.data.fillColor +
                "' class='row node-style'><div class='col'><img src='https://www.w3schools.com/w3images/avatar2.png' alt='Avatar' class='avatar'>"
                + obj.data.Name +"</div><div  class='col menu' >" + 
                "<div class='dropdown'><button class='dropbtn fas fa-ellipsis-v btn-menu'></button> "+
                "<div class='dropdown-content'>"+
                "<a href='#' onclick='onAddParent()'>Add Parent</a>" +
                "<a href='#' onclick='onAddChild()'>Add Child</a>" +
                    "</div>"+
                "</div>" +
            "</div ></div >",
            //margin: { left: 10, right: 10, top: 10, bottom: 10 }
        };
        obj.style = { fill: obj.data.fillColor, strokeColor: 'none', strokeWidth: 2 };
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
}, '#element');
diagram.fitToPage({ mode: 'Width' });

function onAddParent() {
    debugger;
    //data.push({ "Name": "nodeNew", "ReportingPerson": [], "fillColor": "#14ad85" });
    //data.find(x => x.Name == 'node114').ReportingPerson.push("nodeNew");
    //diagram.refresh();


    //add new node to datasource 
    this.diagram.dataSourceSettings.dataManager.dataSource.json.find(x => x.Name == 'node114').ReportingPerson.push("nodeNew");
    this.diagram.dataSourceSettings.dataManager.dataSource.json.push({ "Name": "nodeNew", "fillColor": "#14ad85", "border": "#c15433" });
    this.diagram.clear();
    //refresh the diagram 
    this.diagram.refresh(); 
}

function onAddChild() {
    debugger;
    //data.push({ "Name": "nodeNew", "ReportingPerson": [], "fillColor": "#14ad85" });
    //data.find(x => x.Name == 'node114').ReportingPerson.push("nodeNew");
    //diagram.refresh();


    //add new node to datasource 
    this.diagram.dataSourceSettings.dataManager.dataSource.json.push({ "Name": "nodeNewChild", "ReportingPerson": ["node114"], "fillColor": "#14ad85", "border": "#c15433" });
    this.diagram.clear();
    //refresh the diagram 
    this.diagram.refresh(); 
}