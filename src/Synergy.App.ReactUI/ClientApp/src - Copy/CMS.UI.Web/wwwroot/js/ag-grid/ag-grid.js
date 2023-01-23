
function gridConfig(selector,
    url,
    columns,
    editableC,
    sortableC,
    resizableC,
    filterC,
    flexC,
    paginationC,
    pageSizeC,
) {
    ShowLoader($('#' + selector));
    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json",
        dataType: "JSON",
        success: function (response) {
            HideLoader($('#' + selector));
            document.getElementById(selector).innerHTML = "";
            if (response.length >= 0) {
                debugger;
                var tabledata = response;

                // specify the columns
                const columnDefs = columns;

                // specify the data
                const rowData = tabledata

                // let the grid know which columns and what data to use
                const gridOptions = {
                    defaultColDef: {
                        editable: editableC,
                        sortable: sortableC,
                        resizable: resizableC,
                        filter: filterC,
                        flex: flexC,
                    },
                    columnDefs: columnDefs,
                    rowData: rowData,
                    pagination: paginationC,
                    paginationPageSize: pageSizeC,

                };

                // lookup the container we want the Grid to use
                const eGridDiv = document.querySelector('#' + selector);

                // create the grid passing in the div to use together with the columns & data we want to use
                new agGrid.Grid(eGridDiv, gridOptions);4
            } else {
               // document.getElementById("global-overlay").style.display = 'none';
               // alert("No Data Found");
            }
            //document.getElementById("global-overlay").style.display = 'none';
        },
        error: function (ert) {
            HideLoader($('#' + selector));
                }
    });
}



function gridConfigByData(selector,
    data,
    columns,
    editableC,
    sortableC,
    resizableC,
    filterC,
    flexC,
    paginationC,
    pageSizeC,
) {

    // specify the columns
    const columnDefs = columns;

    // specify the data
    const rowData = data

    // let the grid know which columns and what data to use
    const gridOptions = {
        defaultColDef: {
            editable: editableC,
            sortable: sortableC,
            resizable: resizableC,
            filter: filterC,
            flex: flexC,
        },
        columnDefs: columnDefs,
        rowData: rowData,
        pagination: paginationC,
        paginationPageSize: pageSizeC,

    };

    // lookup the container we want the Grid to use
    const eGridDiv = document.querySelector('#' + selector);

    // create the grid passing in the div to use together with the columns & data we want to use
    new agGrid.Grid(eGridDiv, gridOptions);
}
