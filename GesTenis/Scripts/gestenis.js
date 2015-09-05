function initControls() {
    $('.datetimepicker').datetimepicker({
        format: 'd/m/Y H:i',
        closeOnDateSelect: true,
        step: 5,
        todayButton: false
    });
    $('.alldatepicker').datetimepicker({
        format: 'd/m/Y',
        closeOnDateSelect: true,
        timepicker: false
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
        datepicker: false,
        allowTimes: [ '9:00', '10:00', '11:00', '12:00', '13:00', '14:00', '15:00', '16:00' , '17:00', '18:00', '19:00', '20:00', '21:00' ]
    });

    $('.focus').focus();
}

function getLoader() {
    return "<p>Cargando  ... por favor, espere </p>";
}