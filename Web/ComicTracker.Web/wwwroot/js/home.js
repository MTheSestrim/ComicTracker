$(document).ready(function () {
    $('#Sorting').on('change', function () {

        let args = [], hash;

        let hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');

        for (let i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            args.push(hash[0]);
            args[hash[0]] = hash[1];
        }

        let href = '/?';

        if (args['searchTerm']) {
            href += `searchTerm=${args['searchTerm']}&`;
        }

        href += `sorting=${$(this).val()}`;

        window.location.href = href;
    })
})