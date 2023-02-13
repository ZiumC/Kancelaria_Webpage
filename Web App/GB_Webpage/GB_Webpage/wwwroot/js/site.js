// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.getElementById('hide').onclick = function () {
    document.getElementById('cookie-panel').style.display = "none";
};

document.getElementById('close-about-cookie').onclick = function () {
    document.getElementById('about-cookies').style.display = "none";
};

document.getElementById('moreInfo').onclick = function () {
    document.getElementById('about-cookies').style.display = "flex";
};