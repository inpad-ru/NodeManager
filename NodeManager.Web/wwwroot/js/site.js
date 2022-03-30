// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//$(".chosen-select").chosen({
//    disable_search_threshold: 10,
//    no_results_text: "Oops, nothing found!",
//    width: "95%"
//});

//$('#city').chosen().change(function () {

//    var id = $(this).val();
//    console.log(id); //индекс выбранного элемента

//});

//$("#city").trigger("chosen:updated"); //обновляем select     

////$.validator.setDefaults({ ignore: ":hidden:not(select)" }) //для всех тегов select

//$.validator.setDefaults({ ignore: ":hidden:not(.chosen-select)" }) //только для select имеющий class .chosen-select

/*$(selector).selected({ [options] });*/

$(document).ready(function () {
    $('.chosen-select').chosen({ width: "200px" });
});



//$('select').chosen({ width: "200px" });

//$('select').chosen({ disable_search_threshold: 10 });

//$('select').chosen({ no_results_text: "Not found" });

//$('select').chosen({ max_selected_options: 2 });

//$('select').chosen({ allow_single_deselect: true });

