
//theme is changing on window load
window.onload = function () {

    isCookiePanelHidded();

    changeThemeIcon();

    setThemeToPage();
    setThemeToNavBar();
    setThemeToNote();
    setThemeToCard();
    setThemeToCounter();

    countCharacters();
}



//theme is changing on button click
document.getElementById('theme-button').onclick = function () {

    const themeIcon = document.getElementById('theme-icon');

    if (themeIcon.classList == 'dark-theme') {

        themeIcon.classList = 'light-theme';
        setCookie("theme", "dark");

    } else {

        themeIcon.classList = 'dark-theme';
        setCookie("theme", "light");

    }

    setThemeToPage();
    setThemeToNavBar();
    setThemeToNote();
    setThemeToCard();
    setThemeToCounter();
}



const maxMessageLength = 5000;
const animationTime = "0.3s";



function setThemeTo(element, disableAnimation) {

    const theme = getCookie("theme");

    if (theme == "dark") {

        element.style.background = "black";
        element.style.color = "white";

    } else if (theme == "light") {

        element.style.background = "white";
        element.style.color = "black";

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

    if (disableAnimation) {

        element.style.transition = '0s';

    } else {

        element.style.transition = animationTime;

    }

}



function changeThemeIcon() {

    const theme = getCookie("theme");
    const themeIcon = document.getElementById('theme-icon');

    if (theme == "dark") {

        themeIcon.classList = 'light-theme';

    } else if (theme == "light") {

        themeIcon.classList = 'dark-theme';

    } else {

        themeIcon.classList = 'dark-theme';

    }
}



function setThemeToPage() {

    const body = document.querySelector('body');
    const nav = document.querySelector('nav');
    const cookie = document.getElementById('cookie-modal-content');
    const mailElement = document.getElementById('mail-note');
    const footer = document.getElementById('footer');

    setThemeTo(body, false);
    setThemeTo(nav, false);
    setThemeTo(cookie, false);
    setThemeTo(footer, false);

    if (isExist(mailElement)) {
        setThemeTo(mailElement, false);
    }

    changeMenuIcon();
}



function setThemeToNavBar() {

    var targetNav = document.querySelectorAll("ul#menu-bar li a");

    for (let i = 0; i < targetNav.length; i++) {
        setThemeTo(targetNav[i], false);
    }

}



function setThemeToNote() {

    const cookie = getCookie('theme');
    const note = document.getElementById('note-dark-text');
    const arrow = document.getElementById('arrow');

    if (!isExist(note) || !isExist(arrow)) {
        return;
    }

    setThemeTo(note, false);

    if (cookie == 'dark') {

        arrow.classList = 'light-arrow';

    } else if (cookie == 'light') {

        arrow.classList = 'dark-arrow';

    } else {

        arrow.classList = 'dark-arrow';

    }
}



function setThemeToCard() {

    var cards = document.querySelectorAll("div.row div.col-md-6 div.card");
    var cardsHeader = document.querySelectorAll("div.row div.col-md-6 div.card div.card-header");

    if (!isExist(cards)) {
        return;
    }

    for (let i = 0; i < cards.length; i++) {
        setThemeTo(cards[i], true);
        setThemeTo(cardsHeader[i], true);
    }

}



function countCharacters() {

    const textAreaElement = document.getElementById('input-message');
    const counterSpanElement = document.getElementById('counter');

    if (!isExist(textAreaElement) || !isExist(counterSpanElement)) {
        return;
    }


    textAreaElement.addEventListener('input', () => {

        const charactersLeft = maxMessageLength - textAreaElement.value.length;
        counterSpanElement.textContent = charactersLeft + "/" + maxMessageLength;

        setColorToCounterText(counterSpanElement, textAreaElement);

    });

}



function setThemeToCounter() {

    const textAreaElement = document.getElementById('input-message');
    const counterSpanElement = document.getElementById('counter');

    if (!isExist(textAreaElement) || !isExist(counterSpanElement)) {
        return;
    }

    setColorToCounterText(counterSpanElement, textAreaElement);

}



function setColorToCounterText(counterElement, messageElement) {

    if (!isExist(counterElement) || !isExist(messageElement)) {
        return;
    }

    const charactersLeft = maxMessageLength - messageElement.value.length;
    const theme = getCookie('theme');

    //need to detect what color of bg add to counter
    if (theme == 'dark') {

        counterElement.style.background = 'black';

    } else if (theme == 'light') {

        counterElement.style.background = 'white';

    } else {

        counterElement.style.background = 'white';

    }

    //coloring counter text
    if (charactersLeft <= 20) {

        counterElement.style.color = 'red';

    } else if (charactersLeft > 20 && charactersLeft <= 1500) {

        counterElement.style.color = 'orange';

    } else {

        setThemeTo(counterElement, false);

    }

}



function changeMenuIcon() {

    const menu = document.getElementById('menu-icon');
    const menuButton = document.getElementById('hamburger-button');
    const theme = getCookie('theme');

    if (theme == 'dark') {

        menuButton.classList = 'navbar-toggler light-border';
        menu.classList = 'light-menu-icon';

    } else if (theme == 'light') {

        menuButton.classList = 'navbar-toggler dark-border';
        menu.classList = 'dark-menu-icon';

    } else {

        menuButton.classList = 'navbar-toggler dark-border';
        menu.classList = 'dark-menu-icon';

    }
}



function isExist(element) {

    if (element != null || element != undefined) {

        return true;

    }

    return false;

}