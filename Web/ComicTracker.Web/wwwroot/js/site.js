$(document).ready(function () {
    $('#searchBar').keypress(function (e) {
        if (e.keyCode == 13) {
            let searchTerm = $('#searchBar').val()
            if (searchTerm) {
                window.location.href = `/?searchTerm=${searchTerm}`
            }
            else {
                window.location.href = '/';
            }
        }
    });
})