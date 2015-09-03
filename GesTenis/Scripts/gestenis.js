function initControls() {
    $('.datetimepicker').datetimepicker({
        format: 'd/m/Y H:i',
        closeOnDateSelect: true,
        step: 5,
        todayButton: false
    });
    $('.datepicker').datetimepicker({
        format: 'd/m/Y',
        minDate: 0,
        closeOnDateSelect: true,
        timepicker: false
    });
    $('.timepicker').datetimepicker({
        format: 'H:i',
        formattime: 'H:i',
        step: 60,
        closeOnDateSelect: true,
        datepicker: false
    });
    $('.focus').focus();
}

function getLoader() {
    return "<p>Cargando  ... por favor, espere </p>";
}