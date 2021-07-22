$(document).ready(function () {

    $('.score').on('click', function () {
        let id = $(this).val();

        let value = parseInt($(this).text());

        if (value >= 0 && value <= 10) {
            $.ajax({
                type: 'PUT',
                url: `/api/Series`,
                headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                data: JSON.stringify({ seriesId: id, score: value }),
                contentType: 'application/json;charset=utf-8',
            }).done((res) =>
            {
                $('#userScore').text(`Your Score: ${res}`);
            })
        }
    })
})
