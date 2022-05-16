$(document).ready(function() {
    
    let table = $('#contracts-table').DataTable({
        "dom": "lfrtip",
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "ajax": {
            "url": "/Contract/ContractsData",
            "contentType": "application/json",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                return JSON.stringify(d);
            }
        },
        "columns": [
            { "data": "CompanyContractId", "name": "CompanyContractId", "autoWidth": true },
            { "data": "ContractId", "name": "ContractId", "autoWidth": true, },
            { "data": "ArchiveNumber", "name": "ArchiveNumber", "autoWidth": true },
            { "data": "Name", "name": "Name", "autoWidth": true },
            { "data": "CompanyId", "name": "CompanyId", "autoWidth": true },
            { "data": "ContractDate", "name": "ContractDate", "autoWidth": true, },
            { "data": "Description", "name": "Description", "autoWidth": true, },
            { "data": "OwnerId", "name": "OwnerId", "autoWidth": true, },
            { 
                "data" : "Active",
                "render": function(data, type, row, meta) {
                    return row.Active ? 'Aktivan' : 'Neaktivan'
                }
            },
            { "data": "EndingDeadline", "name": "EndingDeadline", "autoWidth": true, },
            { "data": "ContractTypeId", "name": "ContractTypeId", "autoWidth": true, },
            { "data": "AnnexId", "name": "AnnexId", "autoWidth": true, },
            { "data": "ExecutorId", "name": "ExecutorId", "autoWidth": true, },
            { "data": "TotalSum", "name": "TotalSum", "autoWidth": true, },
            {
                "render": function (data, type, row, meta) {
                    return `<a class="btn btn-success" href="/Document/Index?searchParameter=${row.ArchiveNumber}"><i class="fas fa-angle-double-right"></i></a>`;
                }
            },
        ],
        "columnDefs":
        [
            {
                "targets": [0],
                "visible": false,
                "searchable": false
            },
        ],
    });

    let perPage = parseInt(window.localStorage.getItem("perPageContract"));
    
    if (perPage) {
        table.page.len(perPage).draw();

        $("#per-page-select p.selected-option").html(perPage);
    }

    $('#per-page-select .option-replacement').each((index, option) => {
        option.addEventListener('click', function (e) {
            let value = this.getAttribute('data-value');
    
            e.stopPropagation();
    
            table.page.len(value).draw();
            addFilterToLocalStorage("perPageContract", parseInt(value));
            
            $('.selected-option').html(value);
            $('.options.active').removeClass('active');
        });
    });

    $('#contractSearch').on('keyup', function() {
        if (this.value.length < 3 && this.value.length > 0) {
            return;
        }
        
        table.search(this.value).draw();
    })

    function addFilterToLocalStorage(filterName, filterValue) {
        // I need to do some kind of preparation. Make the object this function friendly.

        let filterToStore = JSON.stringify(filterValue);

        if (window.localStorage.getItem(filterName)){
            window.localStorage.removeItem(filterName);
        }

        window.localStorage.setItem(filterName, filterToStore);
    }
});