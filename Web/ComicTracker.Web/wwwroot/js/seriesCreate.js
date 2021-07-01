$(document).ready(function () {

    const multiSelect = new IconicMultiSelect({
        select: "#genreValues",
    });

    multiSelect.init();

    // Sets all options to false so they are not selected by default
    $('#genreValues option').prop('selected', false);

    multiSelect.subscribe(function (event) {
        // Makes option selected if the user adds it through the search bar
        if (event.action == 'ADD_OPTION') {
            $(`option[value=${event.value}]`).prop('selected', true);
            console.log($(`option[value=${event.value}]`).prop('selected'))
        }
        // Unselects option if the user removes it through the search bar
        else if (event.action == 'REMOVE_OPTION') {
            $(`option[value=${event.value}]`).prop('selected', false);
            console.log($(`option[value=${event.value}]`).prop('selected'))
        }
    });

    // Set genre input to max-width
    $('.iconic-multiselect__container').css('width', '100%');

    // For tab selection in image upload options
    $(".custom-file-input").on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
    });
})