//const columnDefs = [
//    { field: "CustomerNumber", headerName: "Customer Number", width: 150 },
//    { field: "CustomerName", headerName: "Customer Name" },
//    { field: "PC1", headerName: "PC1", width: 100 },
//    { field: "PC2", headerName: "PC2", width: 100 },
//    { field: "PC3", headerName: "PC3", width: 100 }
//];

const modelColumnDefs = [
    {
        field: '', checkboxSelection: true, headerCheckboxSelection: true, headerCheckboxSelectionFilteredOnly: true, sortable: false, filter: false, floatingFilter: false,
        width: 40, pinned: 'left'
    },
    { field: "CustomerNumber", headerName: "Customer Number", width: 150 },
    { field: "CustomerName", headerName: "Customer Name" },
    { field: "PC1", headerName: "PC1", width: 100 },
    { field: "PC2", headerName: "PC2", width: 100 },
    { field: "PC3", headerName: "PC3", width: 100 }
];

// specify the data
let rowData = [];

// specify the data
let modalRowData = [
    { CustomerNumber: "54544", CustomerName: "Flag Creek Water", PC1: 'EU', PC2: 'EU3', PC3: 'E3N' },
    { CustomerNumber: "5000048021", CustomerName: "Flowtech Ac Sales Creek", PC1: 'EP', PC2: 'EP1', PC3: 'EP1' },
    { CustomerNumber: "54613", CustomerName: "Hail Creek Coal Pty Ltd", PC1: 'EU', PC2: 'EUO', PC3: 'EUO' },
    { CustomerNumber: "5000012111", CustomerName: "HP Data Center Eastern Creek", PC1: 'EP', PC2: 'EP1', PC3: 'EP1' },
    { CustomerNumber: "5000012111", CustomerName: "HP Data Center Eastern Creek", PC1: 'EP', PC2: 'EP1', PC3: 'EP1' }
];

// let the grid know which columns and what data to use



// setup the grid after the page has finished loading
//document.addEventListener('DOMContentLoaded', () => {
//    const gridDiv = document.querySelector('#myGrid');
  
//    new agGrid.Grid(gridDiv, gridOptions);
  
//    gridOptions.api.sizeColumnsToFit();
//});

$(document).ready(function () {

    $("header .multi-level-menu").mouseenter(function () {
        $(this).find(".sub-menu").slideDown();
    });

    $("header .multi-level-menu").mouseleave(function () {
        $(this).find(".sub-menu").hide();
    });

    //$('#exampleModal').on('shown.bs.modal', function (e) {
    //    modelGridOptions.api.sizeColumnsToFit();
    //});

    //$(".discount-check").on('change', function () {
    //    if ($(this).is(":checked")) {
    //        $(this).parent().find("span").removeClass("d-none");
    //    } else {
    //        $(this).parent().find("span").addClass("d-none");
    //    }
    //});



    //$(".list-selection").click(function () {
    //    rowData = modelGridOptions.api.getSelectedRows();
    //    gridOptions.api.setRowData(rowData);
    //});

    $(".edit-icon").on('click', function () {
        $('#editModal').modal('show');
    });

    $(".delete-icon").on('click', function () {
        $('#deleteModal').modal('show');
    });

});

$(window).scroll(function () {
    if ($(window).scrollTop() > 60) {
        $(".menu-bar").addClass("fixed");
    } else {
        $(".menu-bar").removeClass("fixed");
    }
});

// Users Table

let usersColumnDefs = [
    { field: "sesaID", filter: true, floatingFilter: true, suppressMenu: true },
    { field: "FirstName", filter: true, floatingFilter: true, suppressMenu: true },
    { field: "LastNmae", filter: true, floatingFilter: true, suppressMenu: true },
    { field: "EmailAddress", filter: true, floatingFilter: true, suppressMenu: true },
    { field: "UserLevel", filter: true, floatingFilter: true, suppressMenu: true },
    {
        field: "Status", filter: true, floatingFilter: true, suppressMenu: true,
        cellRenderer: (param) => {
            return param.value === 'Active' ? `<span class="text-success fw-bold">Active</span>` : `<span class="text-danger fw-bold">Inactive</span>`
        },
    },
    {
        field: "Action", filter: true, width: 175, suppressMenu: true,
        cellRenderer: () => {
            return '<i class="edit-icon" title="Edit"></i><i class="delete-icon ms-3" title="Delete"></i><i class="refresh-icon ms-3" title="Refresh"></i><i class="new-icon ms-3" title="New"></i>'
        },
    }
];

// specify the data
const usersRowData = [
    { sesaID: "SESA452754", FirstName: "Amit", LastNmae: 'Singh', EmailAddress: 'Amit.Singh@se.com', UserLevel: 'Tech', Status: 'Active' },
    { sesaID: "SESA620585", FirstName: "Anushree", LastNmae: 'Shetty', EmailAddress: 'anushree.shetty@non.se.com', UserLevel: 'Tech', Status: 'Active' },
    { sesaID: "SESA284921", FirstName: "Ashwathi", LastNmae: 'Venugopal', EmailAddress: 'ashwathi.venugopal@se.com', UserLevel: 'Tech', Status: 'Active' },
    { sesaID: "SESA47855", FirstName: "Aurore", LastNmae: 'CHAMPAGNE', EmailAddress: 'aurore.champagne@se.com', UserLevel: 'Tech', Status: 'Active' },
    { sesaID: "SESA97466", FirstName: "Avalon", LastNmae: 'COETZER', EmailAddress: 'Avalon.Coetzer@se.com', UserLevel: 'Tech', Status: 'Active' },
    { sesaID: "SESA625278", FirstName: "Axel", LastNmae: 'GAMER', EmailAddress: 'axel.gamer@se.com', UserLevel: 'Tech', Status: 'Active' },
    { sesaID: "SESA452754", FirstName: "CINDY", LastNmae: 'DI FRANCESCO', EmailAddress: 'cindy.di-francesco@se.com', UserLevel: 'Tech', Status: 'Active' },
    { sesaID: "SESA437344", FirstName: "Fatima", LastNmae: 'Ezzahra Alorchi', EmailAddress: 'fatimaezzahra.alorchi@non.se.com', UserLevel: 'Tech', Status: 'Active' },
    { sesaID: "SESA559059", FirstName: "Helmi", LastNmae: 'ARIYANTO', EmailAddress: 'helmi.ariyanto@se.com', UserLevel: 'Tech', Status: 'Active' },
    { sesaID: "SESA19352", FirstName: "Jean", LastNmae: 'li', EmailAddress: 'jing-jean.li@non.se.com', UserLevel: 'Tech', Status: 'Active' }
];

// let the grid know which columns and what data to use
const usersGridOptions = {
    columnDefs: usersColumnDefs,
    rowData: usersRowData,
    rowHeight: 30,
    headerHeight: 30,
    pagination: true
};

//document.addEventListener('DOMContentLoaded', () => {
//    const gridDiv = document.querySelector('#usersGrid');
//    new agGrid.Grid(gridDiv, usersGridOptions);
//    usersGridOptions.api.sizeColumnsToFit();
//});