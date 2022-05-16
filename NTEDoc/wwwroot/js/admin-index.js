$(document).ready(function() {
    
    $(document).ready( function () {
        $('#users-table').DataTable({
            "dom": "lfrtip",
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "ajax": {
                "url": "/Admin/UsersData",
                "contentType": "application/json",
                "type": "POST",
                "datatype": "json",
                "data": function (d) {
                    console.log(d);
                    return JSON.stringify(d);
                }
            },
            "columns": [
                { "data": "UserId", "name": "UserId", "autoWidth": true },
                { "data": "FullName", "name": "FullName", "autoWidth": true, },
                { "data": "Username", "name": "Username", "autoWidth": true },
                { "data": "RoleLevel", "name": "RoleLevel", "autoWidth": true },
                {
                    "data": "RoleId",
                    "render": function (data, type, row, meta) {
                        let content = parseInt(row.RoleId);
                        let response = '';

                        switch(content) {
                            case 0: 
                                response = 'Evidentičar';
                                break;
                            case 1:
                                response = 'Likvidator';
                                break;
                            case 2:
                                response = 'Sektorski evidentičar';
                                break;
                            case 99:
                                response = 'Admin';
                                break;
                        }

                        return response;
                    }
                },
                { "data": "OrganisationUnitId", "name": "OrganisationUnitId", "autoWidth": true },
                { "data": "OwnerId", "name": "OwnerId", "autoWidth": true, },
                { "data": "Program", "name": "Program", "autoWidth": true, },
                { "data": "CreatedAt", "name": "CreatedAt", "autoWidth": true, },
                { "data": "Phone", "name": "Phone", "autoWidth": true, },
                { "data": "Email", "name": "Email", "autoWidth": true, },
                {
                    "render": function (data, type, row, meta) {
                        return `<button class="btn btn-success preview-document"><i class="fas fa-edit"></i></button>`;
                    }
                },
            ]
        });
    });

});