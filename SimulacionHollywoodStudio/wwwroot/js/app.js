$(document).ready(function () {
    $(".datepicker").datepicker({
        dateFormat: "MM yy",
        changeMonth: true,
        changeYear: false,
        showButtonPanel: true,
        onClose: function (dateText, inst) {
            $(this).datepicker("setDate", new Date(inst.selectedYear, inst.selectedMonth, 1));
        }
    });
});
