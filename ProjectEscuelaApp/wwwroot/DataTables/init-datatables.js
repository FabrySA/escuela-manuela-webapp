$(document).ready(function () {

    moment.locale('es');

    if ($('#InabilityDataTable').length > 0) {

        $('#InabilityDataTable').DataTable({

            language: {
                url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-MX.json',
            },
            "order": [1, 'desc'],
            "responsive": true,

            "stripeClasses": ['strip1', 'strip2'],

            "columnDefs": [
                {
                    "render": function (data, type, row, meta) {
                        return moment(data).format('lll');
                    }
                },
                { "responsivePriority": 1, "targets": [0] },
                { "responsivePriority": 2, "targets": [1] },
                { "responsivePriority": 3, "targets": [2] },
                { "responsivePriority": 4, "targets": [3] },
                { "responsivePriority": 5, "targets": [4] },
                { "orderable": false, "targets": [3, 4] },
                { "searchable": false, "targets": [3, 4] }
            ]
        });
    }

    if ($('#permissionDataTable').length > 0) {

        $('#permissionDataTable').DataTable({

            language: {
                url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-MX.json',
            },
            "order": [2, 'desc'],
            "responsive": true,

            "stripeClasses": ['strip1', 'strip2'],

            "columnDefs": [
                {
                    "render": function (data, type, row, meta) {
                        return moment(data).format('lll');
                    }
                },

                { "responsivePriority": 1, "targets": [0] },
                { "responsivePriority": 2, "targets": [1] },
                { "responsivePriority": 3, "targets": [2] },
                { "responsivePriority": 4, "targets": [3] },
                { "responsivePriority": 5, "targets": [4] },
                { "responsivePriority": 6, "targets": [5] },
                { "orderable": false, "targets": [4, 5] },
                { "searchable": false, "targets": [4, 5] }
            ]
        });
    }

    if ($('#userDataTable').length > 0) {

        $('#userDataTable').DataTable({

            language: {
                url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-MX.json',
            },
            "order": [1, 'desc'],
            "responsive": true,

            "columnDefs": [
                { "width": "20%", "targets": 1 }
            ],
            "columnDefs": [
                { "orderable": false, "targets": [0] },
                { "searchable": false, "targets": [0] },
                { "visible": false, "targets": [0] }
            ]
        });
    }

    if ($('#planDataTable').length > 0) {

        $('#planDataTable').DataTable({

            language: {
                url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-MX.json',
            },
            "order": [2, 'desc'],
            "responsive": true,

            "stripeClasses": ['strip1', 'strip2'],

            "columnDefs": [
                { "responsivePriority": 1, "targets": [0] },
                { "responsivePriority": 2, "targets": [1] },
                { "responsivePriority": 3, "targets": [2] },
                { "responsivePriority": 4, "targets": [3] },
                { "responsivePriority": 5, "targets": [4] },
                { "responsivePriority": 6, "targets": [5] },
                { "orderable": false, "targets": [4, 5] },
                { "searchable": false, "targets": [4, 5] }
            ]
        });
    }

    if ($('#miscDataTable').length > 0) {

        $('#miscDataTable').DataTable({

            language: {
                url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-MX.json',
            },
            "order": [1, 'desc'],
            "responsive": true,

            "stripeClasses": ['strip1', 'strip2'],

            "columnDefs": [
                { "responsivePriority": 1, "targets": [0] },
                { "responsivePriority": 2, "targets": [1] },
                { "responsivePriority": 3, "targets": [2] },
                { "responsivePriority": 4, "targets": [3] },
                { "responsivePriority": 5, "targets": [4] },
                { "orderable": false, "targets": [3, 4] },
                { "searchable": false, "targets": [3, 4] }
            ]
        });
    }

    if ($('#publicFilesDataTable').length > 0) {

        $('#publicFilesDataTable').DataTable({

            language: {
                url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-MX.json',
            },
            "order": [1, 'desc'],
            "responsive": true,

            "stripeClasses": ['strip1', 'strip2'],

            "columnDefs": [
                { "responsivePriority": 1, "targets": [0] },
                { "responsivePriority": 2, "targets": [1] },
                { "responsivePriority": 3, "targets": [2] },
                { "responsivePriority": 4, "targets": [3] },
                { "orderable": false, "targets": [3] },
                { "searchable": false, "targets": [3] }
            ]
        });
    }

    if ($('#adminPublicfilesDataTable').length > 0) {

        $('#adminPublicfilesDataTable').DataTable({

            language: {
                url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-MX.json',
            },
            "order": [1, 'desc'],
            "responsive": true,

            "stripeClasses": ['strip1', 'strip2'],

            "columnDefs": [
                { "responsivePriority": 1, "targets": [0] },
                { "responsivePriority": 2, "targets": [1] },
                { "responsivePriority": 3, "targets": [2] },
                { "responsivePriority": 4, "targets": [3] },
                { "responsivePriority": 5, "targets": [4] },
                { "responsivePriority": 6, "targets": [5] },
                { "orderable": false, "targets": [4, 5] },
                { "searchable": false, "targets": [4, 5] }
            ]
        });
    }

    if ($('#userListDataTable').length > 0) {

        $('#userListDataTable').DataTable({

            language: {
                url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-MX.json',
            },
            "order": [1, 'desc'],
            "responsive": true,

            "stripeClasses": ['strip1', 'strip2'],

            "columnDefs": [
                { "responsivePriority": 1, "targets": [0] },
                { "responsivePriority": 2, "targets": [1] },
                { "responsivePriority": 3, "targets": [2] },
                { "responsivePriority": 4, "targets": [3] },
                { "responsivePriority": 5, "targets": [4] },
                { "responsivePriority": 6, "targets": [5] },
                { "responsivePriority": 7, "targets": [6] },
                { "responsivePriority": 8, "targets": [7] },
                { "orderable": false, "targets": [5, 6, 7] },
                { "searchable": false, "targets": [5, 6, 7] }
            ]
        });
    }
});