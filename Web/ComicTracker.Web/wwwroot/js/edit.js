$(document).ready(function () {

    // Array to take advanatage of the default includes method rather than working with more complex objects.

    let selectedArr = [];

    /*
     * jQuery variants, e.g. $('#genreValues option:selected'), are not working for some reason.
     * Therefore, I use simple JS DOM manipulation to get all selected genres.
     */

    let selectedGenres = $('#genreValues option')
        .filter((i, e) => e.hasAttribute('selected'))
        .map((i, g) => selectedArr.push($(g).val()));

    let allGenres = $('div.iconic-multiselect__options li');

    allGenres = allGenres.filter(function (i, e) {
        return selectedArr.includes($(e).attr('data-value'))
    })

    allGenres.each((i, e) => {
        $(e).trigger('click');
    })
})