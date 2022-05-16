$(document).ready(function() {
    
    let partners = $('.partner-select');
    let table = $('#example').DataTable();
    let documentSearch = $('#docSearch');
    //let shouldPrintAll = $('#previouslyPrinted')[0].checked;

    documentSearch.on('keyup', function() {
        table.search(this.value).draw();
    });
    
    if (documentSearch.val()) {
        documentSearch.keyup();
    }

    $("#partnersSearch").on('input', function () {
        let searchValue = this.value;
    
        if (searchValue.length <= 2) {
            partners.hide();
            return;
        }
        
        partners.each((index, partner) => {
            
            let partnerName = partner.getAttribute('data-partner');
    
            if (partnerName.toLowerCase().includes(`${searchValue.toLowerCase()}`)) {
                partner.style.display = 'flex';
            } else {
                partner.style.display = 'none';
            }
    
        });
    });

    $('#export-excel').on('click', function() {
        //let dataFilters = JSON.parse(table.ajax.params());
        //dataFilters.PrintAll = shouldPrintAll;

        //table.ajax.param.draw();
        $('.dt-button.buttons-excel').trigger('click');
    });

    $('#export-pdf').on('click', function() {
        $('.dt-button.buttons-pdf').trigger('click');
    });

    //$('#previouslyPrinted').on('change', function () {
    //    shouldPrintAll = this.checked;
    //});

    //$('#export-columns').on('click', function() {
    //    let $this = $(this);
    //    let exportType = $this.attr('data-type');

    //    let dataFilters = table.ajax.params();

    //    //$.ajax({
    //    //    url: `/Document/GenerateReport/${exportType}`,
    //    //    method: 'POST',
    //    //    data: JSON.stringify(data),
    //    //    success: function (response) {
    //    //        console.log(response);
    //    //    }
    //    //});

    //    window.open(`/Document/GenerateReport?reportType=${exportType}&documentFilters=${dataFilters}`, '_blank');
    //});

    $('#advanced-filters-open').on('click', function() {
        $('#advanced-filters').addClass('active');
    });

    $('#submit-advanced-filters').on('click', addAppliedAdvancedFilter);

    addAppliedAdvancedFilter();

    function addAppliedAdvancedFilter() {
        let filtersOn = $('[data-is-on="true"]').length || window.localStorage.getItem("advancedFilters");
        
        if (!filtersOn) {
            return;
        }

        let output = `
            <div class="applied-filter advanced">
                
                <div class="applied-filter-title">
                    <span class="applied-title-for">
                        Napredni filteri
                    </span>

                    <span class="applied-filter-value">
                        
                    </span>
                </div>

                <div class="cancel-filter advanced">
                    <i class="fa fa-times"></i>
                </div>

            </div>
        `;

        $('.applied-filter.advanced').remove();
        $('.applied-filters').append(output);
        
        table.draw();
    }

    
});