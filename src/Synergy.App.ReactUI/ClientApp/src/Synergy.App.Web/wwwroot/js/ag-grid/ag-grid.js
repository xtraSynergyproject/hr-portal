function pagedGridConfig(selector,
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

    const gridOptions = {
        defaultColDef: {
            editable: editableC,
            sortable: sortableC,
            resizable: resizableC,
            filter: filterC,
            flex: flexC,
        },
        columnDefs: columnDefs,
        pagination: true,
        rowModelType: 'infinite',
        cacheBlockSize: 20, // you can have your custom page size
        paginationPageSize: 20, //pagesize
        cacheOverflowSize: 2,
        maxConcurrentDatasourceRequests: 2,
        infiniteInitialRowCount: 1,
        maxBlocksInCache: 2,
        getRowNodeId: function (item) {
            //alert(item);
            return item.id;
        },
        onGridReady(params) {
            alert();
            this.gridApi = params.api;
            this.gridApi.setDatasource(this.dataSource) // replace dataSource with your datasource
        },
        dataSource: IDatasource = {
            getRows: (params) => {

                // Use startRow and endRow for sending pagination to Backend
                // params.startRow : Start Page
                // params.endRow : End Page

                //replace this.apiService with your Backend Call that returns an Observable
                this.apiService().subscribe(response => {

                    params.successCallback(
                        response.data, response.totalRecords
                    );

                })
            }
        }
    };

    LoadPagedData(selector, url, gridOptions);

}

function LoadPagedData(selector, url, gridOptions) {
    alert(url)
    const gridDiv = document.querySelector('#' + selector);
    new agGrid.Grid(gridDiv, gridOptions);

    fetch(url)
        .then((response) => response.json())
        .then(function (data) {
            // give each row an id
            data.forEach(function (d, index) {
                d.id = 'R' + (index + 1);
            });

            const dataSource = {
                rowCount: undefined, // behave as infinite scroll

                getRows: function (params) {
                    console.log('asking for ' + params.startRow + ' to ' + params.endRow);

                    // At this point in your code, you would call the server.
                    // To make the demo look real, wait for 500ms before returning
                    setTimeout(function () {
                        // take a slice of the total rows
                        const dataAfterSortingAndFiltering = sortAndFilter(
                            data,
                            params.sortModel,
                            params.filterModel
                        );
                        const rowsThisPage = dataAfterSortingAndFiltering.slice(
                            params.startRow,
                            params.endRow
                        );
                        // if on or after the last page, work out the last row.
                        let lastRow = -1;
                        if (dataAfterSortingAndFiltering.length <= params.endRow) {
                            lastRow = dataAfterSortingAndFiltering.length;
                        }
                        // call the success callback
                        params.successCallback(rowsThisPage, lastRow);
                    }, 500);
                },
            };

            gridOptions.api.setDatasource(dataSource);
        });
}
function sortAndFilter(allOfTheData, sortModel, filterModel) {
    return sortData(sortModel, filterData(filterModel, allOfTheData));
}

function sortData(sortModel, data) {
    const sortPresent = sortModel && sortModel.length > 0;
    if (!sortPresent) {
        return data;
    }
    // do an in memory sort of the data, across all the fields
    const resultOfSort = data.slice();
    resultOfSort.sort(function (a, b) {
        for (let k = 0; k < sortModel.length; k++) {
            const sortColModel = sortModel[k];
            const valueA = a[sortColModel.colId];
            const valueB = b[sortColModel.colId];
            // this filter didn't find a difference, move onto the next one
            if (valueA == valueB) {
                continue;
            }
            const sortDirection = sortColModel.sort === 'asc' ? 1 : -1;
            if (valueA > valueB) {
                return sortDirection;
            } else {
                return sortDirection * -1;
            }
        }
        // no filters found a difference
        return 0;
    });
    return resultOfSort;
}

function filterData(filterModel, data) {
    const filterPresent = filterModel && Object.keys(filterModel).length > 0;
    if (!filterPresent) {
        return data;
    }

    const resultOfFilter = [];
    for (let i = 0; i < data.length; i++) {
        const item = data[i];

        if (filterModel.age) {
            const age = item.age;
            const allowedAge = parseInt(filterModel.age.filter);
            // EQUALS = 1;
            // LESS_THAN = 2;
            // GREATER_THAN = 3;
            if (filterModel.age.type == 'equals') {
                if (age !== allowedAge) {
                    continue;
                }
            } else if (filterModel.age.type == 'lessThan') {
                if (age >= allowedAge) {
                    continue;
                }
            } else {
                if (age <= allowedAge) {
                    continue;
                }
            }
        }

        if (filterModel.year) {
            if (filterModel.year.values.indexOf(item.year.toString()) < 0) {
                // year didn't match, so skip this record
                continue;
            }
        }

        if (filterModel.country) {
            if (filterModel.country.values.indexOf(item.country) < 0) {
                continue;
            }
        }

        resultOfFilter.push(item);
    }

    return resultOfFilter;
}

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
    gridOptionsDetails = null;
    debugger;
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

                var tabledata = response;

                // specify the columns
                const columnDefs = columns;

                // specify the data
                const rowData = tabledata;
                const eGridDiv = document.querySelector('#' + selector);
                // let the grid know which columns and what data to use
                const gridOptions = {
                    defaultColDef: {
                        editable: editableC,
                        sortable: sortableC,
                        resizable: resizableC,
                        filter: filterC,
                        flex: flexC,
                        initialWidth: 200,
                        wrapHeaderText: true,
                        autoHeaderHeight: true,
                    },
                    columnDefs: columnDefs,
                    excelStyles: [
                        {
                            id: 'boldFont',
                            interior: {
                                color: '#b5e6b5',
                                pattern: 'Solid',
                            },
                        },
                    ],
                    suppressColumnVirtualisation: true,
                    rowData: rowData,
                    pagination: paginationC,
                    paginationPageSize: pageSizeC,
                    onGridReady: (event) => {
                        var allColumnIds = [];
                        event.columnApi.getAllGridColumns().forEach((column) => {
                            allColumnIds.push(column.getId());
                        });
                        event.columnApi.autoSizeColumns(allColumnIds, false);
                        var colWidth = 0;
                        event.columnApi.getAllGridColumns().forEach((column) => {
                            colWidth += column.actualWidth;
                        });
                        var gridWidth = $('#' + selector + ' .ag-center-cols-viewport')[0].scrollWidth;
                        if (gridWidth > colWidth) {
                            event.api.sizeColumnsToFit();
                        }
                    }
                };
                // gridOptions.columnApi.autoSizeAllColumns();
                // alert(123);
                // lookup the container we want the Grid to use

                gridOptionsDetails = gridOptions;
                // create the grid passing in the div to use together with the columns & data we want to use
                new agGrid.Grid(eGridDiv, gridOptions);
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

function gridConfigGroup(selector,
    url,
    columns,
    editableC,
    sortableC,
    resizableC,
    filterC,
    flexC,
    paginationC,
    pageSizeC,
    groupIncludeFooterC,
    groupIncludeTotalFooterC
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

                var tabledata = response;

                // specify the columns
                const columnDefs = columns;

                // specify the data
                const rowData = tabledata

                // let the grid know which columns and what data to use
                const gridOptions = {
                    defaultColDef: {
                        sortable: sortableC,
                        resizable: resizableC,
                        flex: flexC,
                        minWidth: 150,
                    },
                    autoGroupColumnDef: {
                        minWidth: 300,
                    },
                    columnDefs: columnDefs,
                    rowData: rowData,
                    groupIncludeFooter: groupIncludeFooterC,
                    groupIncludeTotalFooter: groupIncludeTotalFooterC,
                    animateRows: true,

                };

                // lookup the container we want the Grid to use
                const eGridDiv = document.querySelector('#' + selector);

                // create the grid passing in the div to use together with the columns & data we want to use
                new agGrid.Grid(eGridDiv, gridOptions);
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


