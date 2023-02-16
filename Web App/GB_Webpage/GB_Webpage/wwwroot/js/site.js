// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const maxMessageLength = 5000;

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

function countCharacters() {

    const textAreaElement = document.getElementById('input-message');
    const counterSpanElement = document.getElementById('counter');

    if (textAreaElement == undefined || textAreaElement == null) {
        return;
    }

    if (counterSpanElement == undefined || counterSpanElement == null) {
        return;
    }


    textAreaElement.addEventListener('input', () => {

        const charactersLeft = maxMessageLength - textAreaElement.value.length;
        counterSpanElement.textContent = charactersLeft + "/" + maxMessageLength;

        const theme = getCookie('theme');

        if (theme == 'dark') {
            counterSpanElement.style.background = 'white';
        } else {
            counterSpanElement.style.background = 'black';
        }

        if (charactersLeft <= 20) {

            counterSpanElement.style.color = 'red';

        } else if (charactersLeft > 20 && charactersLeft <= 1500) {

            counterSpanElement.style.color = 'orange';

        } else {

            setThemeToCounter();

        }

    });

}

window.onload = function () {

    isCookiePanelHidded();
    changeThemeIcon();

    setThemeToPage();
    setThemeToNavBar();
    setThemeToCard();
    setThemeToCounter();

    countCharacters();
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

function setThemeTo(element, disableAnimation) {

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

    /*
     * Disabling bg for nav elements. 
     * Changing bg for those elements makes visual problems.
    */
    if (element.classList == 'nav-link') {

        element.style.background = "none";

    }

    /*
     * By default animation is turned on.
     */
    if (disableAnimation) {

        element.style.transition = '0s';

    } else {

        element.style.transition = '1s';

    }

}

document.getElementById('theme-button').onclick = function () {

    const themeIcon = document.getElementById('theme-icon');

    if (themeIcon.classList == 'dark-theme') {

        themeIcon.classList = 'light-theme';
        setCookie("theme", "light");

    } else {

        themeIcon.classList = 'dark-theme';
        setCookie("theme", "dark");

    }

    setThemeToPage();
    setThemeToNavBar();
    setThemeToCard();
    setThemeToCounter();

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

function setThemeToPage() {

    const body = document.querySelector('body');
    const nav = document.querySelector('nav');
    const cookie = document.getElementById('cookie-modal-content');

    setThemeTo(body, false);
    setThemeTo(nav, false);
    setThemeTo(cookie, false);

}

function setThemeToNavBar() {

    var targetNav = document.querySelectorAll("ul#menu-bar li a");

    for (let i = 0; i < targetNav.length; i++) {
        setThemeTo(targetNav[i], false);
    }

}

function setThemeToCard() {

    var targetCard = document.querySelectorAll("div.row div.col-md-6 div.card");
    var targetCardHeader = document.querySelectorAll("div.row div.col-md-6 div.card div.card-header");

    for (let i = 0; i < targetCard.length; i++) {
        setThemeTo(targetCard[i], true);
        setThemeTo(targetCardHeader[i], true);
    }

}

function setThemeToCounter() {


    const textAreaElement = document.getElementById('input-message');
    const counterSpanElement = document.getElementById('counter');

    const charactersLeft = maxMessageLength - textAreaElement.value.length;
    const theme = getCookie('theme');

    if (theme == 'dark') {
        counterSpanElement.style.background = 'white';
    } else {
        counterSpanElement.style.background = 'black';
    }

    if (charactersLeft <= 20) {

        counterSpanElement.style.color = 'red';

    } else if (charactersLeft > 20 && charactersLeft <= 1500) {

        counterSpanElement.style.color = 'orange';

    } else {
        setThemeTo(counterSpanElement, false);
    }

}