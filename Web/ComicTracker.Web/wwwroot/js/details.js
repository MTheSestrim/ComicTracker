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
            }).done((res) => {
                $('#userScore').text(`Your Score: ${res}`);
            })
                .fail((res) => {
                    if (res.status == 401) {
                        window.location.href = '/Identity/Account/Login';
                    }
                })
        }
    })

    $('.templateCreator').on('click', function () {
        let entityName = $(this).attr('value');
        // Get the modal
        $(`#${entityName.toLowerCase() + 's'}Modal`).css('display', 'block');
    })

    // Get the <span> element that closes the modal
    $(".close").on('click', function () {
        $(this).parent().parent().css('display', 'none');
    });

    // Function for entity creation specific to series
    $('.templateSubmit').on('click', function () {
        let entityName = $(this).attr('value');

        let seriesId = $(this).attr('seriesId');

        // Gets the value of the input before the submit button, which is the target of this function
        let numberOfEntities = $(this).prev().val();


        if (entityName && numberOfEntities && seriesId) {
            $.ajax({
                type: 'POST',
                url: `/api/${entityName}`,
                headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                data: JSON.stringify({ numberOfEntities: numberOfEntities, seriesId: seriesId }),
                contentType: 'application/json;charset=utf-8',
            }).done((res) => {
                // GET request to load new entities into table
                $.ajax({
                    type: 'GET',
                    url: `/Series/${seriesId}`,
                }).done((res) => {
                    let data = $('<div />').append(res).find(`#pills-${entityName.toLowerCase()}s`).html();
                    $(`#pills-${entityName.toLowerCase()}s`).html(data);
                }).fail((res) => {
                    window.location.href = `/Series/${seriesId}`;
                })
            }).fail((res) => {
                console.log(res.text);
            })
        }
    })
})
