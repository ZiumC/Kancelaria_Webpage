﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
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

function isCookiePanelHidded() {
    const isCookieHidded = getCookie("cookie_panel_hidded");

    if (isCookieHidded === null || isCookieHidded === undefined || isCookieHidded != "true") {
        document.getElementById('cookie-panel').style.display = "block";
    } else {
        document.getElementById('cookie-panel').style.display = "none";
    }
}

window.onload = function () {

    isCookiePanelHidded();
    changeThemeIcon();

    const body = document.querySelector('body');
    const nav = document.querySelector('nav');
    const cookie = document.getElementById('cookie-modal-content');

    setThemeFor(body, false);
    setThemeFor(nav, false);
    setThemeFor(cookie, false);

    var targetNav = document.querySelectorAll("ul#menu-bar li a");

    for (let i = 0; i < targetNav.length; i++) {
        setThemeFor(targetNav[i], true);
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

function setThemeFor(element, isTopNavElement) {

    const theme = getCookie("theme");

    if (theme == "dark") {

        element.style.background = "white";
        element.style.color = "black";

    } else if (theme == "light") {

        element.style.background = "black";
        element.style.color = "white";

    } else {

        element.style.background = "white";
        element.style.color = "black";

    }

    if (isTopNavElement) {
        element.style.background = "none";
    }
    element.style.transition = '1s';
}

function changeThemeIcon() {

    const theme = getCookie("theme");
    const themeIcon = document.getElementById('theme-icon');

    if (theme == "dark") {

        themeIcon.classList = 'dark-theme';

    } else if (theme == "light") {

        themeIcon.classList = 'light-theme';

    } else {

        themeIcon.classList = 'dark-theme';

    }
}

function setTheme() {
    const themeIcon = document.getElementById('theme-icon');

    if (themeIcon.classList == 'dark-theme') {

        themeIcon.classList = 'light-theme';
        setCookie("theme", "light");

    } else {

        themeIcon.classList = 'dark-theme';
        setCookie("theme", "dark");

    }

    const body = document.querySelector('body');
    const nav = document.querySelector('nav');
    const cookie = document.getElementById('cookie-modal-content');

    setThemeFor(body, false);
    setThemeFor(nav, false);
    setThemeFor(cookie, false);

    var targetNav = document.querySelectorAll("ul#menu-bar li a");

    for (let i = 0; i < targetNav.length; i++) {
        setThemeFor(targetNav[i], true);
    }

}