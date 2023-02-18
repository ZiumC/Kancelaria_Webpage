
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