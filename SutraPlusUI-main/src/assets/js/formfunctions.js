function GetnewCurrDate(id) {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);

    var dt = new Date();
    var time = dt.getHours() + ":" + dt.getMinutes();

    $('#' + id).val(today);
}