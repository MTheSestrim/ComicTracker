$(document).ready(function () {
    $('#Sorting').on('change', function () {

        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }

        let href = '/?';
        if (vars['searchTerm']) {
            href += `searchTerm=${vars['searchTerm']}&`
        }
        href += `sorting=${$(this).val()}`
        window.location.href = href;
    })
})