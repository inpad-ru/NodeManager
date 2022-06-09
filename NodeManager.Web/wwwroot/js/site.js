$(document).ready(function () {
    $('.chosen-select').chosen({ width: "250px" });
});

//function redirectPage(numPage) {
//    var urlPage = window.location.href;
//    var decodedURLPage = decodeURI(urlPage);
//    //console.log(urlPage);
//    //console.log(decodedURLPage);
//    var arrURLPage = decodedURLPage.split('/');
//    //console.log(arrURLPage.length);
//    //console.log(arrURLPage);
//    //console.log(decodedURLPage);
//    if (arrURLPage.length == 4)
//    {
//        var newArrURLPage = new Array(6);
//        newArrURLPage[0] = arrURLPage[0];
//        newArrURLPage[1] = arrURLPage[1];
//        newArrURLPage[2] = arrURLPage[2];
//        newArrURLPage[3] = "Node";
//        newArrURLPage[4] = "List";
//        newArrURLPage[5] = numPage;
//        var newMinURLPage = newArrURLPage.join('/');
//        /*return encodeURI(newMinURLPage);*/
//        //console.log(newArrURLPage);
//        //console.log(encodeURI(newMinURLPage));
//    }
//    else if (decodedURLPage == "")
//    {

//    }
//    arrURLPage[5] = numPage;
//    var newURLPage = arrURLPage.join('/');
//    var encodedURLPage = encodeURI(newURLPage);
//    console.log(encodedURLPage);
//    //return encodedURLPage;
//}

function redirectPage(numPage) {
    var urlPage = window.location.href;
    var decodedURLPage = decodeURI(urlPage);
    var arrURLPage = decodedURLPage.split('/');
    if (arrURLPage.length == 4) { //для главной страницы, лист 1
        arrURLPage[3] = "Node";
        arrURLPage[4] = "List";
        arrURLPage[5] = numPage;
    }
    else if (arrURLPage.length == 6 && arrURLPage[4] == "List") { //для остальных листов главной страницы
        arrURLPage[5] = numPage;
    }
    else if ((arrURLPage.length == 7 || arrURLPage.length == 8)  && arrURLPage[4] == "List") { //для страниц разделов и категорий
        arrURLPage[5] = numPage;
    }
    else if (arrURLPage.length == 6 && (arrURLPage[5] == "SearchName" || arrURLPage[5] == "Search")) { //для страницы поиска по названию или по тегу
        arrURLPage[4] = numPage;
    }
    else if (arrURLPage.length == 7 &&  arrURLPage[5] == "ProjectSection") { //для страницы проекта
        arrURLPage[4] = numPage;
    }
    var newURLPage = arrURLPage.join('/');
    var encodedURLPage = encodeURI(newURLPage);
    return encodedURLPage;
}