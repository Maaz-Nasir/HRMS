var CustomWidgets = {
    LoadDataGrid: function (Element, Columns, Url, Type, Value, Key, PageSize) {
        $(Element).DataTable({
            "processing": true,
            "language": {
                processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
            },
            "serverSide": true,
            "searchable": true,
            "filter": true,
            "lengthMenu": PageSize,
            "ajax": {
                "url": Url,
                "type": Type,
                "datatype": "json"
            },
            "columnDefs": [{
                "targets": [0],
                "visible": false,
                "searchable": false,
            }],
            "columns": Columns,
        })
    }
}