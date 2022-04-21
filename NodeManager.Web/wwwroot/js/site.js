$(document).ready(function () {
    $('.chosen-select').chosen({ width: "250px" });
});

var inputs = document.querySelectorAll('.upload-file__input');
Array.prototype.forEach.call(inputs, function (input) {
    var label = input.nextElementSibling,
        labelVal = label.innerHTML;

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