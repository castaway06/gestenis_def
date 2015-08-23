function initControls() {
    $('.datetimepicker').datetimepicker({ format: 'd/m/Y H:i', closeOnDateSelect: true, step: 5, todayButton: false });
    $('.focus').focus();
}

function getLoader() {
    return "<p>Cargando  ... por favor, espere </p>";
}