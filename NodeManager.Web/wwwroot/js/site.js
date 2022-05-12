/* Поиск по тегу ---------------------------------------------- */

$(document).ready(function () {
    $('.chosen-select').chosen({ width: "250px" });
});

//$(function () {
//    'use strict';

//    $('[data-toggle="offcanvas"]').on('click', function () {
//        $('.offcanvas-collapse').addClass('open');
//        $('body').addClass('offcanvas-open');
//    })
//    $('[data-toggle="offcanvas-close"]').on('click', function () {
//        $('.offcanvas-collapse').removeClass('open');
//        $('body').removeClass('offcanvas-open');

//    })
//})


/* Поиск по названию ---------------------------------------------- */

let cards = document.querySelectorAll('.node')

function liveSearch() {
    let search_query = document.getElementById("searchbox").value;

    //Use innerText if all contents are visible
    //Use textContent for including hidden elements
    for (var i = 0; i < cards.length; i++) {
        if (cards[i].innerText.toLowerCase()
            .includes(search_query.toLowerCase())) {
            cards[i].classList.remove("is-hidden");
        } else {
            cards[i].classList.add("is-hidden");
        }
    }
}

//A little delay
let typingTimer;
let typeInterval = 500;
let searchInput = document.getElementById('searchbox');

searchInput.addEventListener('keyup', () => {
    clearTimeout(typingTimer);
    typingTimer = setTimeout(liveSearch, typeInterval);
});


/* Поиск по названию ---------------------------------------------- */

// external js: isotope.pkgd.js

// store filter for each group
var buttonFilters = {};
var buttonFilter;
// quick search regex
var qsRegex;

// init Isotope
var $grid = $('.nodes').isotope({
    itemSelector: '.color-shape',
    filter: function () {
        var $this = $(this);
        var searchResult = qsRegex ? $this.text().match(qsRegex) : true;
        var buttonResult = buttonFilter ? $this.is(buttonFilter) : true;
        return searchResult && buttonResult;
    },
});

$('.filters').on('click', '.button', function () {
    var $this = $(this);
    // get group key
    var $buttonGroup = $this.parents('.button-group');
    var filterGroup = $buttonGroup.attr('data-filter-group');
    // set filter for group
    buttonFilters[filterGroup] = $this.attr('data-filter');
    // combine filters
    buttonFilter = concatValues(buttonFilters);
    // Isotope arrange
    $grid.isotope();
});

// use value of search field to filter
var $quicksearch = $('.quicksearch').keyup(debounce(function () {
    qsRegex = new RegExp($quicksearch.val(), 'gi');
    $grid.isotope();
}));

// change is-checked class on buttons
$('.button-group').each(function (i, buttonGroup) {
    var $buttonGroup = $(buttonGroup);
    $buttonGroup.on('click', 'button', function () {
        $buttonGroup.find('.is-checked').removeClass('is-checked');
        $(this).addClass('is-checked');
    });
});

// flatten object by concatting values
function concatValues(obj) {
    var value = '';
    for (var prop in obj) {
        value += obj[prop];
    }
    return value;
}

// debounce so filtering doesn't happen every millisecond
function debounce(fn, threshold) {
    var timeout;
    threshold = threshold || 100;
    return function debounced() {
        clearTimeout(timeout);
        var args = arguments;
        var _this = this;
        function delayed() {
            fn.apply(_this, args);
        }
        timeout = setTimeout(delayed, threshold);
    };
}


/* Загрузка файлов ---------------------------------------------- */

document.addEventListener('DOMContentLoaded', () => {

    input.addEventListener('change', function (e) {
        var fileName = '';
        if (this.files && this.files.length > 1)
            fileName = (this.getAttribute('data-multiple-caption') || '').replace('{count}', this.files.length);
        else
            fileName = e.target.value.split('\\').pop();

        if (fileName.length > 18)
            fileNameCut = fileName.substr(0, 11) + '…' + fileName.substr(name.length - 6);
        else fileNameCut = fileName;

        if (fileNameCut)
            label.querySelector('span').innerHTML = fileNameCut;
        else
            label.innerHTML = labelVal;
        
        document.getElementsByClassName("upload-file__text")[0].title = fileName;
    });
});