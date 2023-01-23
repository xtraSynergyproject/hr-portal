'use strict';
var dataTable = {
    init: function init(objectId, settings) {
        this.bindUIActions(objectId, settings);
    },
    bindUIActions: function bindUIActions(objectId, settings) {

        // event handlers
        this.table = this.handleDataTables(objectId, settings);
        this.handleSearchRecords(objectId, settings);
        this.handleSelecter(objectId, settings);
        this.handleClearSelected(objectId, settings);

        // add buttons
        this.table.buttons().container().appendTo('#dt-buttons').unwrap();
    },
    handleDataTables: function handleDataTables(objectId, settings) {
        var object = $('#' + objectId);
        var config = {
            //dom: '<\'text-muted\'Bi>\n        <\'table-responsive\'tr>\n        <\'mt-4\'p>',
            //buttons: ['copyHtml5', { extend: 'print', autoPrint: false }],
            serverSide: true,
            //searching: false,
            //lengthMenu: [[10, 20, 50, 100, -1], [10, 20, 50, 100, "All"]],
            pageLength: 10,
            //table.page.len( 25 ).draw();.
            responsive: true,
            //language: {
            //    paginate: {
            //        previous: '<i class="fa fa-lg fa-angle-left"></i>',
            //        next: '<i class="fa fa-lg fa-angle-right"></i>'
            //    }
            //},
            autoWidth: false,
            ajax: {
                url: settings.url,
                type: 'POST',
                cache: false
            },
            deferRender: true,
            order: settings.order,// [1, 'asc'],
            columns: settings.columns,
            columnDefs: settings.columnDefs
        }
        var obj = object.DataTable(config);
        $('#' + objectId + '_filter').parent().parent().hide();
        return obj;
    },
    handleSearchRecords: function handleSearchRecords(objectId, settings) {
        var self = this;
        $('#' + objectId + '_filterBy, #' + objectId + '_search').on('change', function (e) {
            var filterBy = $('#' + objectId + '_filterBy').val();
            var hasFilter = filterBy !== '';
            var value = $('#' + objectId + '_search').val();
            if (value.length && (e.type === 'keyup' || e.type === 'change')) {
                self.clearSelectedRows(objectId, settings);
            }
            //self.table.search('').columns().search('').draw();
            if (hasFilter) {
                self.table.columns(filterBy).search(value).draw();
            } else {
                self.table.search(value).draw();
            }
        });
        //$('#table-search, #filterBy').on('keyup change focus', function (e) {
        //    var filterBy = $('#filterBy').val();
        //    var hasFilter = filterBy !== '';
        //    var value = $('#table-search').val();

        //    // clear selected rows
        //    if (value.length && (e.type === 'keyup' || e.type === 'change')) {
        //        self.clearSelectedRows();
        //    }

        //    // reset search term
        //    self.table.search('').columns().search('').draw();

        //    if (hasFilter) {
        //        self.table.columns(filterBy).search(value).draw();
        //    } else {
        //        self.table.search(value).draw();
        //    }
        //});
    },
    handleSelecter: function handleSelecter(objectId, settings) {
        var self = this;

        $(document).on('change', '#' + objectId + '_check-handle', function () {
            var isChecked = $(this).prop('checked');
            $('input[name="' + objectId + '_selectedRow[]"]').prop('checked', isChecked);

            // get info
            self.getSelectedInfo(objectId, settings);
        }).on('change', 'input[name="' + objectId + '_selectedRow[]"]', function () {
            var $selectors = $('input[name="' + objectId + '_selectedRow[]"]');
            var $selectedRow = $('input[name="' + objectId + '_selectedRow[]"]:checked').length;
            var prop = $selectedRow === $selectors.length ? 'checked' : 'indeterminate';

            // reset props
            $('#' + objectId + '_check-handle').prop('indeterminate', false).prop('checked', false);

            if ($selectedRow) {
                $('#' + objectId + '_check-handle').prop(prop, true);
            }

            // get info
            self.getSelectedInfo(objectId, settings);
        });
    },
    handleClearSelected: function handleClearSelected(objectId, settings) {
        var self = this;
        // clear selected rows
        $('#' + objectId).on('page.dt', function () {
            self.clearSelectedRows(objectId, settings);
        });
        //$('#clear-search').on('click', function () {
        //    self.clearSelectedRows();
        //});
    },
    getSelectedInfo: function getSelectedInfo(objectId, settings) {
        var $selectedRow = $('input[name="' + objectId + '_selectedRow[]"]:checked').length;
        var $info = $('.thead-btn');
        var $badge = $('<span/>').addClass('selected-row-info text-muted pl-1').text($selectedRow + ' selected');
        // remove existing info
        $('.selected-row-info').remove();
        // add current info
        if ($selectedRow) {
            $info.prepend($badge);
        }
    },
    clearSelectedRows: function clearSelectedRows(objectId, setting) {
        $('#' + objectId + '_check-handle').prop('indeterminate', false).prop('checked', false).trigger('change');
    }
};

var dropDownList = {
    init: function init(objectId, config) {
        var settings = { id: 'RID', name: "Name", placeholder: 'Select', enablePagination: false, pageSize: 50, searchDelay: 250, init: true };
        $.extend(settings, config);
        this.bindUIActions(objectId, settings);

        var studentSelect = $('#' + objectId);
        $.ajax({
            type: 'GET',
            url: settings.valueUrl
        }).then(function (data) {
            // create the option and append to Select2
            var option = new Option(data[settings["name"]], data[settings["id"]], true, true);
            studentSelect.append(option).trigger('change');

            // manually trigger the `select2:select` event
            studentSelect.trigger({
                type: 'select2:select',
                params: {
                    data: data
                }
            });
        });

    },
    bindUIActions: function bindUIActions(objectId, settings) {
        // responsive setting
        $.fn.select2.defaults.set('width', '100%');


        //this.singleSelect();
        //this.multipleSelect();
        //this.arrayData();
        this.remoteData(objectId, settings);
        //this.tagging();
        //this.disableMode();
    },
    remoteData: function remoteData(objectId, settings) {
        var object = $('#' + objectId);
        var formatRepo = function formatRepo(repo) {

            //if (repo.Name) return repo.Name;

            var markup = '<div class="media">' + '<div class="user-avatar mr-2"><img src="' + repo.Name + '" /></div>' + '<div class="media-body">' + '<h6 class="my-0">' + repo.Name + '</h6>';

            if (repo.Name) {
                markup += '<div class="small text-muted">' + repo.Name + '</div>';
            }

            markup += '<ul class="list-inline small text-muted">' + '<li class="list-inline-item"><i class="fa fa-flash"></i> ' + repo.Name + ' Forks</li>' + '<li class="list-inline-item"><i class="fa fa-star"></i> ' + repo.Name + ' Stars</li>' + '<li class="list-inline-item"><i class="fa fa-eye"></i> ' + repo.Name + ' Watchers</li>' + '</ul>' + '</div></div>';

            return markup;
        };

        var formatRepoSelection = function formatRepoSelection(repo) {
            // return repo;//'<div class="user-avatar user-avatar-xs mr-2"><img src="' + repo.owner.avatar_url + '" /></div>' + repo.full_name || repo.text;
            return '<div class="user-avatar user-avatar-xs mr-2"><img src="' + repo.Name + '" /></div>13' + repo.Name || repo.Name;
            return repo.Name;
        };
        object.select2({
            placeholder: settings.placeholder,
            allowClear: true,
            ajax: {
                // url: 'https://api.github.com/search/repositories',
                url: settings.url,


                dataType: 'json',
                delay: settings.searchDelay,
                data: function data(params) {
                    return {
                        search: params.term, // search term
                        page: params.page,
                        pageSize: settings.pageSize
                    };
                },
                processResults: function processResults(data, params) {
                    params.page = params.page || 0;
                    return {
                        results: data.items.map(function (item) {
                            return {
                                id: item[settings["id"]],
                                text: item[settings['name']],
                            };
                        }
                        ),
                        pagination: {
                            more: settings.enablePagination && params.page * settings.pageSize < data.TotalCount
                        }
                    };

                },
                cache: true
            },
            escapeMarkup: function escapeMarkup(markup) {
                return markup;
            },
            minimumInputLength: -1,
            // templateResult: formatRepo,
            //templateSelection: formatRepoSelection
        });
    },
    //tagging: function tagging() {
    //    var data = ['SandyBrown', 'GhostWhite', 'LightSalmon', 'Bisque', 'LightSlateGray', 'PaleTurquoise', 'MediumVioletRed', 'LightSteelBlue', 'MidnightBlue', 'Peru', 'CornflowerBlue', 'DimGray', 'LightPink', 'Lime', 'Cornsilk', 'Cyan', 'DeepPink', 'BurlyWood', 'LightBlue', 'Fuchsia', 'LightGoldenRodYellow', 'PaleGoldenRod', 'DarkSalmon', 'Darkorange', 'Orange', 'FloralWhite', 'Ivory', 'Pink', 'Teal', 'Tan', 'LightCoral', 'ForestGreen', 'LimeGreen', 'Chocolate', 'Linen', 'RosyBrown', 'DarkTurquoise', 'DarkOrchid', 'DarkBlue', 'Magenta', 'SeaGreen', 'DarkRed', 'DarkSlateGray', 'SaddleBrown', 'DarkMagenta', 'Gray', 'Azure', 'Black', 'DarkKhaki', 'Lavender', 'Maroon', 'Orchid', 'DarkSeaGreen', 'Gainsboro', 'Brown', 'Khaki', 'MediumSeaGreen', 'LightYellow', 'Salmon', 'MediumTurquoise', 'IndianRed', 'AntiqueWhite', 'SpringGreen', 'MistyRose', 'DarkOliveGreen', 'Thistle', 'Violet', 'Olive', 'Crimson', 'BlanchedAlmond', 'PowderBlue', 'SlateGray', 'LawnGreen', 'MintCream', 'LightGreen', 'LightSkyBlue', 'Yellow', 'Indigo', 'HotPink', 'WhiteSmoke', 'Gold', 'BlueViolet', 'LavenderBlush', 'OliveDrab', 'PeachPuff', 'OldLace', 'GreenYellow', 'Navy', 'Aquamarine', 'DarkSlateBlue', 'Purple', 'PaleGreen', 'SteelBlue', 'Blue', 'Coral', 'PaleVioletRed', 'RoyalBlue', 'Turquoise', 'MediumOrchid', 'Green', 'Sienna', 'DarkGray', 'DodgerBlue', 'SlateBlue', 'LightGray', 'DarkGoldenRod', 'SkyBlue', 'LightSeaGreen', 'GoldenRod', 'Snow', 'YellowGreen', 'CadetBlue', 'PapayaWhip', 'DeepSkyBlue', 'LemonChiffon', 'DimGrey', 'MediumSpringGreen', 'HoneyDew', 'Plum', 'Silver', 'MediumBlue', 'Aqua', 'Chartreuse', 'FireBrick', 'Beige', 'SeaShell', 'Wheat', 'AliceBlue', 'MediumPurple', 'OrangeRed', 'DarkGreen', 'Moccasin', 'NavajoWhite', 'DarkCyan', 'MediumAquaMarine', 'Red', 'DarkViolet', 'LightCyan', 'MediumSlateBlue'];
    //    $('#select2-tagging').select2({
    //        tags: data,
    //        tokenSeparators: [',', ' ']
    //    });
    //},
    //disableMode: function disableMode() {
    //    $('#select2-disabled-mode1, #select2-disabled-mode2').select2({
    //        placeholder: 'Select a state'
    //    });
    //}
};
 //dropDownList.init();