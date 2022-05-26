// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let toggleNav = document.querySelector('#toggle-nav'),
    wrapper = document.querySelector('.wrapper'),
    documentFilters = document.querySelectorAll('.document-filter'),
    customSelectWrapper = document.querySelector(`.custom-select-wrapper`),
    customSelectElement = document.querySelector(`.custom-select-wrapper select`),
    selectReplacement = document.querySelector(`.select-replacement`),
    selectOptionsReplacement = document.querySelectorAll(`.option-replacement`),
    selectedReplacement = document.querySelector(`.selected-option`),
    selectOptionsWrapper = document.querySelector('.options')
    returnToContractorModal = $('#return-to-contractor-modal'),
    editDocumentModal = $('#edit-document-modal'),
    confirmReceivingDocumentModal = $('#executor-received-modal'),
    forwardToSectorModal = $('#forward-to-sector-modal'),
    sectorProcessingModal = $('#sector-processing-modal'),
    referentProcessingModal = $('#referent-processing-modal'),
    backToRecorderModal = $('#return-to-recorder-modal'),
    sectorApprovalInput = $('input[name="sectorApproval"]'),
    approveDocumentModal = $('#approve-document-modal'),
    addFileModal = $('#add-file-modal'),
    editFileModal = $('#edit-file-modal'),
    deleteFileModal = $('#delete-file-modal'),
    viewFileModal = $('#view-file-modal'),
    sendToControllerModal = $('#sendToControllerModal'),
    addCommentModal = $('#addCommentModal'),
    readCommentModal = $('#readCommentModal'),
    returnToControllerModal = $('#returnToControllerModal');

$(document).on('click', '.custom-modal .modal-close, .custom-modal .cancel', function (e) {
    $this = $(this);

    $this.parents('.custom-modal').removeClass('active');
});


$('#addCommentToggle').on('click', function () {
    addCommentModal.addClass('active');
});

$('#readComment').on('click', function () {
    readCommentModal.addClass('active');
});



$('#btn-edit').on('click', function () {
    editDocumentModal.addClass('active');
});

$('#reject').on('click', function () {
    returnToContractorModal.addClass('active');
});




$('#forwardToSector').on('click', function () {
    forwardToSectorModal.addClass('active');
});


$('#controllerEdit').on('click', function () {
    editDocumentModal.addClass('active');
});




$('#returnToControllor').on('click', function () {
    returnToControllerModal.addClass('active');
});

$('#approveDocument').on('click', function () {
    approveDocumentModal.addClass('active');
});




$('#signAndApprove').on('click', function () {
    sectorProcessingModal.addClass('active');
});




$('#add-new-file').on('click', function () {
    addFileModal.addClass('active');
});


$('.btn-view').on('click', function () {
    let $this = $(this);

    let fileName = $this.parents('.file').attr('data-filename');

    viewFileModal.find('iframe').attr('src', `${window.location.protocol}//${window.location.host}/Document/GetPdf?path=${fileName}`);
    viewFileModal.addClass('active');
});


$('.btn-edit').on('click', function () {
    let $this = $(this);

    let documentFileId = $this.parents('.file').attr('data-id');

    $(`.edit-file-modal[data-file-id=${documentFileId}]`).addClass('active');
});

$('.btn-delete').on('click', function () {
    let $this = $(this);

    let documentFileId = $this.parents('.file').attr('data-id');

    deleteFileModal.find('input[name="fileId"]').val(documentFileId);
    deleteFileModal.addClass('active');
});

$('.nav-link.active').on('click', function() {
    let $this = $(this);

    $this.parents('.wrapper').removeClass('nav-closed');
    toggleNav.classList.add('active');
});

// Ovaj je za ispravnost dokumenta kod sektorskog
// $('#validStateSelect .option-replacement').each((index, option) => {
//     option.addEventListener('click', function (e) {
//         let value = this.getAttribute('data-value');

//         e.stopPropagation();

//         sectorApprovalInput.val(value);
//         console.log(sectorApprovalInput);
        
//         $('#validStateSelect .selected-option')[0].innerHTML = this.innerHTML;
//         $('#validStateSelect .selected-option')[0].style.color = value === '1' ? '#15AF1B' : '#F44E40';

//         $(this).parents('.options')[0].classList.remove('active');
//     });
// });

// Ovaj je za datatable i len
$('#per-page-select .option-replacement').each((index, option) => {
    option.addEventListener('click', function (e) {
        let value = this.getAttribute('data-value');
        let table = $('#example').DataTable();

        e.stopPropagation();

        sectorApprovalInput.val(value);

        table.page.len(value).draw();
        addFilterToLocalStorage("perPage", parseInt(value));
        
        selectedReplacement.innerHTML = this.innerHTML;
        selectedReplacement.style.color = value === '1' ? '#15AF1B' : '#F44E40';

        selectOptionsWrapper.classList.remove('active');
    });
});

$('.select-replacement').on('click', function(e) {
    let $this = $(this);
    e.stopPropagation();

    $('.select-replacement').children('.options').removeClass('active');

    $this.children('.options').addClass('active');
});

document.addEventListener('click', function (e) {
    let clickedElement = e.target;

    if (clickedElement !== selectReplacement) {
        selectOptionsWrapper.classList.remove('active');
    }
});

$('.filter-title').on('click', function() {
    let $this = $(this),
        $thisContainer = $this.parents('.document-filter');

    documentFilters.forEach(filter => {
        if (filter !== $thisContainer[0]) {
            filter.classList.remove('opened');
        }
    });

    $thisContainer.toggleClass('opened');
});

toggleNav.addEventListener('click', function () {
    wrapper.classList.toggle('nav-closed');
    this.classList.toggle('active');
});


function previewDocument(docLocation) {
    var iframe = document.getElementById('modalframe');
    
    iframe.src = `${window.location.protocol}//${window.location.host}/Document/GetPdf?path=` + docLocation;
    
    $("#fileModal").modal('show');
};

function roleInputHandler() {
    var selected = document.getElementById("roleSelect").selectedIndex;

    if (selected === 3) {
        document.getElementById("sectorSelect").style.display = "block";
        document.getElementById("sectorInput").focus();
    }
    else {
        document.getElementById("sectorSelect").style.display = "none";
    }
}

function addFilterToLocalStorage(filterName, filterValue) {
    // I need to do some kind of preparation. Make the object this function friendly.

    let filterToStore = JSON.stringify(filterValue);

    if (window.localStorage.getItem(filterName)){
        window.localStorage.removeItem(filterName);
    }

    window.localStorage.setItem(filterName, filterToStore);
}
