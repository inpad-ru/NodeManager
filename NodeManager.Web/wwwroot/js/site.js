﻿$(document).ready(function () {
    $('.chosen-select').chosen({ width: "250px" });
});

let form = document.querySelector('.subscription');
let enteredText = document.querySelector('.search_by_name');

form.onsubmit = function (evt) {
    evt.preventDefault();
    var inputText = enteredText.value;
    var strURL = "/Node/1/SearchName/";
    window.location.assign(strURL + inputText);
};

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
    else if (arrURLPage.length == 7 && (arrURLPage[5] == "SearchName" || arrURLPage[5] == "Search")) { //для страницы поиска по названию или по тегу
        arrURLPage[4] = numPage;
    }
    else if (arrURLPage.length == 7 &&  arrURLPage[5] == "ProjectSection") { //для страницы проекта
        arrURLPage[4] = numPage;
    }
    var newURLPage = arrURLPage.join('/');
    var encodedURLPage = encodeURI(newURLPage);
    window.location.assign(encodedURLPage);
}