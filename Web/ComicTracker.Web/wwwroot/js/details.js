$(document).ready(function () {

    $('.score').on('click', function () {
        /* 
         * window.location.pathname = /{Entity}/{id}
         * .split('/')[1] -> {Entity}
         */
        let controller = window.location.pathname.split('/')[1];

        let id = $(this).val();

        let value = parseInt($(this).text());

        if (value >= 0 && value <= 10) {
            $.ajax({
                type: 'PUT',
                url: `/api/${controller}`,
                headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                data: JSON.stringify({ id: id, score: value }),
                contentType: 'application/json;charset=utf-8',
            }).done((res) =>
            {
                $('#userScore').text(`Your Score: ${res}`);
            })
            .fail((res) => {
                if (res.status == 401) {
                    window.location.href = '/Identity/Account/Login';
                }
            })
        }
    })
})
