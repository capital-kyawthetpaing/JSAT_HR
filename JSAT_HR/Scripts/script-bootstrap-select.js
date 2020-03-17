$(document).ready(function () 
{
    // Enable Live Search.
    $('#StaffList').attr('data-live-search', true);

    //// Enable multiple select.
    $('#StaffList').attr('multiple', true);
    $('#StaffList').attr('data-selected-text-format', 'count');

    $('.selectStaff').selectpicker(
    {
        width: '100%',
        title: '- [Choose Multiple Staff] -',
        style: 'btn-info',
        size: 6,
        iconBase: 'fa',
        tickIcon: 'fa-check'
    });
});  

