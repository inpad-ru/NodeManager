$(document).ready(function () {
    $('.chosen-select').chosen({ width: "250px" });
});

function redirectPage(numPage) {
    var urlPage = window.location.href;
    var decodedURLPage = decodeURI(urlPage);
    var arrURLPage = decodedURLPage.split('/');
    arrURLPage[5] = numPage;
    var newURLPage = arrURLPage.join('/');
    var encodedURLPage = encodeURI(newURLPage);
    return encodedURLPage;
    console.log(newURLPage);
    //return encodedURLPage;
    //console.log(decodedURLPage);
    //console.log(arrURLPage);
    //console.log(encodedURLPage);
}