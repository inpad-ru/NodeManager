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

    const forms = document.querySelectorAll('form');

    for (let i = 1; i <= 4; i++) { // сюда будем помещать drug-&-drop файлы (4) 
        window['uploadDragFiles_'+i] = new Object();
    }

    document.querySelectorAll('.upload-file__wrapper').forEach(function (current_item, index) {

        const inputFile = current_item.querySelector('.upload-file__input');

        // создаём массив файлов 
        let fileList = [];

        /////////// Кнопка «Прикрепить файл» /////////// 
        let textSelector = current_item.querySelector('.upload-file__text');

        // Событие выбора файла(ов) 
        inputFile.addEventListener('change', function () {
            fileList.push(...inputFile.files);
            // console.log(inputFile.files); 
            // вызов функции для каждого файла 
            fileList.forEach(file => {
                uploadFile(file);
            });
        });

        // Проверяем размер файлов и выводим название 
        const uploadFile = (file) => {

            // размер файла <5 Мб 
            if (file.size > 5 * 1024 * 1024) {
                alert('Файл должен быть не более 5 МБ.');
                return;
            }

            // Показ загружаемых файлов 
            if (file && fileList.length > 1) {
                if (fileList.length <= 4) {
                    textSelector.textContent = `Выбрано ${fileList.length} файла`;
                    console.log(`Выбрано ${fileList.length} файла`)
                } else {
                    textSelector.textContent = `Выбрано ${fileList.length} файлов`
                    console.log(`Выбрано ${fileList.length} файлов`)
                }
            } else {
                textSelector.textContent = file.name.substring(0, 19) + "...";
                console.log(`Выбран ${fileList.length} файл`)
            }
            fileList = [];
        }


        /////////// Загрузка файлов при помощи «Drag-and-drop» /////////// 
        // const dropZones = document.querySelectorAll('.upload-file__label'); 
        const dropZone = current_item.querySelector('.upload-file__label');
        const dropZoneText = current_item.querySelector('.upload-file__text');
        const maxFileSize = 5000000; // максимальный размер файла - 5 мб. 

        // Проверка поддержки «Drag-and-drop» 
        if (typeof (window.FileReader) == 'undefined') {
            dropZone.textContent = 'Drag&Drop Не поддерживается браузером!';
            dropZone.classList.add('error');
        }
        // Событие - перетаскивания файла 
        dropZone.ondragover = function () {
            this.classList.add('hover');
            return false;
        };
        // Событие - отмена перетаскивания файла 
        dropZone.ondragleave = function () {
            this.classList.remove('hover');
            return false;
        };
        // Событие - файл перетащили 
        dropZone.addEventListener('drop', function (e) {
            e.preventDefault();
            this.classList.remove('hover');
            this.classList.add('drop');

            let uploadDragFiles = new Array()/*[e.dataTransfer.files]*/;
            //uploadDragFiles = e.dataTransfer.files[0]; // один файл 
            for (let j = 0; j <= e.length; j++) { // сюда будем помещать drug-&-drop файлы (4)
                /*window['uploadDragFiles_' + j] = new Object();*/
                uploadDragFiles[j] = e.dataTransfer.files;
                event.preventDefault()
            }
            
            // Проверка размера файла 
            if (uploadDragFiles.size > maxFileSize) {
                dropZoneText.textContent = 'Размер превышает допустимое значение!';
                this.addClass('error');
                return false;
            }

            // Показ загружаемых файлов 
            if (uploadDragFiles.length > 1) {
                if (uploadDragFiles.length <= 4) {
                    dropZoneText.textContent = `Выбрано ${uploadDragFiles.length} файла`;
                    console.log(`Выбрано ${uploadDragFiles.length} файла`)
                    console.log(`Выбрано ${dropZoneText.textContent} файла`)
                } else {
                    dropZoneText.textContent = `Выбрано ${uploadDragFiles.length} файлов`;
                    console.log(`Выбрано ${uploadDragFiles.length} файлов`)
                    console.log(`Выбрано ${dropZoneText.textContent} файлов`)
                }
            } else {
                dropZoneText.textContent = e.dataTransfer.files[0].name.substring(0, 19) + "...";
                console.log(`Выбран ${uploadDragFiles.length} файл`)
                console.log(`Выбран ${dropZoneText.textContent} файл`)
            }
            console.log(`Длина ${typeof uploadDragFiles} файл`)
            // добавляем файл в объект "uploadDragFiles_i" 
            window['uploadDragFiles_'+index] = uploadDragFiles;
        });

    });


    //let dropArea = document.getElementById('.upload - file__label')
    //    ;['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
    //        dropArea.addEventListener(eventName, preventDefaults, false)
    //    })
    //function preventDefaults(e) {
    //    e.preventDefault()
    //    e.stopPropagation()
    //}
    //;['dragenter', 'dragover'].forEach(eventName => {
    //    dropArea.addEventListener(eventName, highlight, false)
    //})
    //    ;['dragleave', 'drop'].forEach(eventName => {
    //        dropArea.addEventListener(eventName, unhighlight, false)
    //    })
    //function highlight(e) {
    //    dropArea.classList.add('highlight')
    //}
    //function unhighlight(e) {
    //    dropArea.classList.remove('highlight')
    //}

    //dropArea.addEventListener('drop', handleDrop, false)
    //function handleDrop(e) {
    //    let dt = e.dataTransfer
    //    let files = dt.files
    //    handleFiles(files)
    //}
    //function handleFiles(files) {
    //    ([...files]).forEach(uploadFile)
    //}

    //function uploadFile(file) {
    //    let url = 'ВАШ URL ДЛЯ ЗАГРУЗКИ ФАЙЛОВ'
    //    let formData = new FormData()
    //    formData.append('file', file)
    //    fetch(url, {
    //        method: 'POST',
    //        body: formData
    //    })
    //        .then(() => { /* Готово. Информируем пользователя */ })
    //        .catch(() => { /* Ошибка. Информируем пользователя */ })
    //}

    //function uploadFile(file) {
    //    var url = 'ВАШ URL ДЛЯ ЗАГРУЗКИ ФАЙЛОВ'
    //    var xhr = new XMLHttpRequest()
    //    var formData = new FormData()
    //    xhr.open('POST', url, true)
    //    xhr.addEventListener('readystatechange', function (e) {
    //        if (xhr.readyState == 4 && xhr.status == 200) {
    //            // Готово. Информируем пользователя
    //        }
    //        else if (xhr.readyState == 4 && xhr.status != 200) {
    //            // Ошибка. Информируем пользователя
    //        }
    //    })
    //    formData.append('file', file)
    //    xhr.send(formData)
    //}



    // Отправка формы на сервер 
    const postData = async (url, fData) => { // имеет асинхронные операции 

        // начало отправки 
        // здесь можно сообщить пользователю о начале отправки 

        // ждём ответ, только тогда наш код пойдёт дальше 
        let fetchResponse = await fetch(url, {
            method: 'POST',
            body: fData
        });

        // ждём окончания операции 
        return await fetchResponse.text();
    };

    if (forms) {
        forms.forEach(el => {
            el.addEventListener('.subFile', function (e) {
                e.preventDefault();

                // создание объекта FormData 
                let fData = new FormData();

                // Добавление всех input, кроме type="file" 
                el.querySelectorAll('input:not([type="file"])').forEach(input => {
                    fData.append(input.name, input.value);
                });

                // Добавление файлов input type file 
                el.querySelectorAll('.upload-file__input').forEach((one_file, index) => {
                    for (let i = 0; i < (one_file.files.length); i++) {
                        fData.append('files[]', one_file.files[i]); // добавляем файлы, добавленные кнопкой 
                    }
                    fData.append('files[]', window['uploadDragFiles_'+index]); // добавляем drug-&-drop файлы 
                });

                // Отправка на сервер 
                postData('./mail-files.php', fData)
                    .then(fetchResponse => {
                        swal({
                            title: 'Спасибо!',
                            text: 'Данные отправлены.',
                            icon: 'success',
                            button: 'Ok'
                        });
                        // console.table('Данные успешно отправлены!', fetchResponse); 
                        el.reset(); // Очистка полей формы 
                        document.querySelectorAll('.upload-file__text').forEach(this_text => {
                            this_text.textContent = 'Выберите файл или перетащите в поле';
                        });
                    })
                    .catch(function (error) {
                        swal({
                            title: error,
                            icon: 'error',
                            button: 'Ok'
                        });
                        // console.table('Ошибка!', error); 
                    });
            });
        });
    };

});

