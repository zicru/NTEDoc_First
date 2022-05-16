$(document).ready(function() {
    
    let table = $('#partners-table').DataTable({
        "dom": "lfrtip",
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "ajax": {
            "url": "/Partner/PartnersData",
            "contentType": "application/json",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                return JSON.stringify(d);
            }
        },
        "columns": [
            { "data": "Id", "name": "Id", "autoWidth": true },
            { "data": "IDFirme", "name": "IDFirme", "autoWidth": true, },
            { "data": "Konto", "name": "Konto", "autoWidth": true },
            { "data": "naziv", "name": "naziv", "autoWidth": true },
            { "data": "Mesto", "name": "Mesto", "autoWidth": true },
            { "data": "Adresa", "name": "Adresa", "autoWidth": true, },
            { "data": "PIB", "name": "PIB", "autoWidth": true, },
            { "data": "Maticni", "name": "Maticni", "autoWidth": true, },
            { "data": "ziro", "name": "ziro", "autoWidth": true, },
            { "data": "Telefon", "name": "Telefon", "autoWidth": true, },
            { "data": "ptt", "name": "ptt", "autoWidth": true, },
            { "data": "Opstina", "name": "Opstina", "autoWidth": true, },
            {
                "render": function (data, type, row, meta) {
                    return `<a class="btn btn-success" href="/Document/Index?searchParameter=${row.naziv}"><i class="fas fa-angle-double-right"></i></a>`;
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

    let perPage = parseInt(window.localStorage.getItem("perPagePartner"));
    
    if (perPage) {
        table.page.len(perPage).draw();

        $("#per-page-select p.selected-option").html(perPage);
    }

    $('#per-page-select .option-replacement').each((index, option) => {
        option.addEventListener('click', function (e) {
            let value = this.getAttribute('data-value');
    
            e.stopPropagation();
    
            table.page.len(value).draw();
            addFilterToLocalStorage("perPagePartner", parseInt(value));
            
            $('.selected-option').html(value);
            $('.options.active').removeClass('active');
        });
    });

    $('#partnerSearch').on('keyup', function() {
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