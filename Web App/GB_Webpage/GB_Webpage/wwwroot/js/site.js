// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.getElementById('hide').onclick = function () {
    document.getElementById('cookie-panel').style.display = "none";
    setCookie("cookie_panel_hidded", "true");
};

document.getElementById('close-about-cookie').onclick = function () {
    document.getElementById('about-cookies').style.display = "none";
};

document.getElementById('moreInfo').onclick = function () {
    document.getElementById('about-cookies').style.display = "flex";
};

window.onload = function () {

    const isCookieHidded = getCookie("cookie_panel_hidded");

    if (isCookieHidded === null || isCookieHidded === undefined || isCookieHidded != "true") {
        document.getElementById('cookie-panel').style.display = "block";
    } else {
        document.getElementById('cookie-panel').style.display = "none";
    }

};

function getCookie(name) {

    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);

    if (parts.length === 2) {
        return parts
            .pop()
            .split(';')
            .shift();
    } 

}

function setCookie(name, value) {
    document.cookie = name + "=" + value + ";";
}

function changeTheme() {
    const themeIcon = document.getElementById('themeIcon');
    const body = document.querySelector('body');

    if (themeIcon.classList == 'dark-theme') {
        themeIcon.classList = 'light-theme';

    } else {
        themeIcon.classList = 'dark-theme';
    }
}