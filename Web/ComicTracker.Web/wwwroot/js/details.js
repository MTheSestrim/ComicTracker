function focusScore() {
    let currentScore = $('#userScore').text().replace('Your Score: ', '').trim();

    if (currentScore) {
        $('.score').filter((i, btn) => $(btn).text().trim() == currentScore).focus();
    }
}

function sendScore(e, t) {
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
            // Redirect to Login page
            if (res.status == 401) {
                window.location.href = '/Identity/Account/Login';
            }
        })
    }
}

function refreshEntities(res, controller , parentId, entityName) {
    // GET request to load new entities into table
    $.ajax({
        type: 'GET',
        url: `/${controller}/${parentId}`,
    }).done((res) => {
        // Take response from the GET request of updated page and replace the table's data
        let data = $('<div />').append(res).find(`#pills-${entityName.toLowerCase()}s`).html();
        $(`#pills-${entityName.toLowerCase()}s`).html(data);
    }).fail((res) => {
        window.location.href = `/Series/${parentId}`;
    })
}

function sendTemplatesCount(e, t) {
    let entityName = $(this).attr('value');

    let seriesId = $(this).attr('seriesId');

    // Gets the value of the input before the submit button, which is the target of this function
    let numberOfEntities = $(this).prev().prev().val();

    if (entityName && numberOfEntities > 0 && seriesId) {
        $.ajax({
            type: 'POST',
            url: `/api/${entityName}/CreateTemplates`,
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: JSON.stringify({ numberOfEntities: numberOfEntities, seriesId: seriesId }),
            contentType: 'application/json;charset=utf-8',
        }).done(res => refreshEntities(res, "Series", seriesId, entityName))
        .fail((res) => {
            console.log(res.text);
        })
    }
    else
    {
        $(`#${entityName.toLowerCase()}sValidation`).text('Value must be at least 1.');
    }
}

function sendEntitiesRange(e, t) {
    let entityName = $(this).attr('value');

    let seriesId = $('#seriesHref').attr('href').split('/')[2];

    let parentTypeName = $(this).attr('parentTypeName');
    let parentId = $(this).attr('parentId');

    // Gets the min and max range for given modal
    let minRange = parseInt($(this).prev().prev().find('.minRange').val());
    let maxRange = parseInt($(this).prev().prev().find('.maxRange').val());

    if (minRange > 0 && maxRange > 0) {

        if (minRange <= maxRange && entityName && parentId && seriesId) {
            $.ajax({
                type: 'POST',
                url: `/api/${entityName}/Attach`,
                headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                data: JSON.stringify({
                    seriesId: seriesId,
                    parentTypeName: parentTypeName,
                    parentId: parentId,
                    minRange: minRange,
                    maxRange: maxRange,
                }),
                contentType: 'application/json;charset=utf-8',
            }).done(res => refreshEntities(res, parentTypeName, parentId, entityName))
            .fail((res) => {
                console.log(res.text);
            })
        }
        else {
            $(`#${entityName.toLowerCase()}sValidation`)
                .text('First field should be lesser than or equal the second.')
        }
    }
    else {
        $(`#${entityName.toLowerCase()}sValidation`).text('Values must be at least 1.');
    }
}

$(document).ready(function () {
    // Focuses on the user's score when loading page
    focusScore();

    // Sends score to API and handles response upon clicking one of the .score buttons.
    $('.score').on('click', sendScore)

    // Opens modal when a .templateCreator button is pressed.
    $('.templateCreator').on('click', function () {
        let entityName = $(this).attr('value');
        // Get the modal
        $(`#${entityName.toLowerCase() + 's'}Modal`).css('display', 'block');
    })

    // Get the <span> element that closes the modal
    $(".close").on('click', function () { $(this).parent().parent().css('display', 'none') });

    // Function for entity creation specific to series
    $('.templateSubmit').on('click', sendTemplatesCount);

    $('.rangeSubmit').on('click', sendEntitiesRange);
})
