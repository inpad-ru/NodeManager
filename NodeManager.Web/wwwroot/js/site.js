$(document).ready(function () {
    $('.chosen-select').chosen({ width: "250px" });
});

function redirectPage(numPage) {
    var urlPage = window.location.href;
    var decodedURLPage = decodeURI(urlPage);
    //console.log(urlPage);
    //console.log(decodedURLPage);
    var arrURLPage = decodedURLPage.split('/');
    //console.log(arrURLPage.length);
    //console.log(arrURLPage);
    //console.log(decodedURLPage);
    if (arrURLPage.length == 4)
    {
        var newArrURLPage = new Array(6);
        newArrURLPage[0] = arrURLPage[0];
        newArrURLPage[1] = arrURLPage[1];
        newArrURLPage[2] = arrURLPage[2];
        newArrURLPage[3] = "Node";
        newArrURLPage[4] = "List";
        newArrURLPage[5] = numPage;
        var newMinURLPage = newArrURLPage.join('/');
        /*return encodeURI(newMinURLPage);*/
        //console.log(newArrURLPage);
        //console.log(encodeURI(newMinURLPage));
    }
    arrURLPage[5] = numPage;
    var newURLPage = arrURLPage.join('/');
    var encodedURLPage = encodeURI(newURLPage);
    console.log(encodedURLPage);
    //return encodedURLPage;
}