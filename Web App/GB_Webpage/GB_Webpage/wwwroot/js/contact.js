
document.getElementById('send-button').onclick = function () {

    let isNameCorrect = validateName();
    let isEmailCorrect = validateEmail();
    let isMessageCorrect = validateMessage();


    if (isNameCorrect && isEmailCorrect && isMessageCorrect) {
        $('#contact-form').attr('onsubmit', 'return true;');
    } else {
        $('#contact-form').attr('onsubmit', 'return false;');
    }

}



setTimeout(() => {
    const box = document.getElementById('form-status');
    $(box).slideUp(function () {
        box.parentNode.removeChild(box);
    })
}, 3500);



const errorColor = 'red';
const correctColor = 'lightgray';
const cookieLangName = '.AspNetCore.Culture';
const nameRegex = "^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$";
const emailRegex = "^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$";
const spaceRegex = /\s/g;



function translateRequired() {

    var languageCookie = getCookie(cookieLangName);

    const pl = "Pole jest wymagane";
    const en = "Field is required";
    const de = "tłumaczenie: Pole jest wymagane"

    if (languageCookie.includes("pl")) {
        return pl;
    } else if (languageCookie.includes("en")) {
        return en;
    } else if (languageCookie.includes("de")) {
        return de;
    } else {
        return pl;
    }

}



function translateRange(min, max) {

    var languageCookie = getCookie(cookieLangName);

    const pl = "Pole powinno zawierać " + min + "-" + max + " znaków";
    const en = "Field should contain " + min + "-" + max + " characters";
    const de = "tłumaczenie: Pole powinno zawierać " + min + "-" + max + " znaków."

    if (languageCookie.includes("pl")) {
        return pl;
    } else if (languageCookie.includes("en")) {
        return en;
    } else if (languageCookie.includes("de")) {
        return de;
    } else {
        return pl;
    }
}



function translateFieldProperty() {

    var languageCookie = getCookie(cookieLangName);

    const pl = "Pole jest nieprawidłowo uzupełnione";
    const en = "Field isn't properly filled";
    const de = "tłumaczenie: Pole jest nieprawidłowo uzupełnione";

    if (languageCookie.includes("pl")) {
        return pl;
    } else if (languageCookie.includes("en")) {
        return en;
    } else if (languageCookie.includes("de")) {
        return de;
    } else {
        return pl;
    }

}



function validateName() {

    const min = 2;
    const max = 50;

    const element = document.getElementById('input-name');
    const name = element.value;
    const isNameCorrect = new RegExp(nameRegex).test(name);

    setErrorBorderTo(element);

    if (name.replace(spaceRegex, "") == "" || name == null) {

        $("#error-name").text(translateRequired());
        return false;

    } else if (name.length < min || name.length > max) {

        $("#error-name").text(translateRange(min, max));
        return false;

    } else if (isNameCorrect == false) {

        $("#error-name").text(translateFieldProperty());
        return false;

    } else {

        setCorrectBorderTo(element);
        $("#error-name").text("");
        return true;

    }
}



function validateEmail() {

    const min = 2;
    const max = 50;

    const element = document.getElementById('input-email');
    const email = element.value;
    const isEmailCorrect = new RegExp(emailRegex).test(email);

    setErrorBorderTo(element);

    if (email.replace(spaceRegex, "") == "" || email == null) {

        $("#error-email").text(translateRequired());
        return false;

    } else if (email.length < min || email.length > max) {

        $("#error-email").text(translateRange(min, max));
        return false;

    } else if (isEmailCorrect == false) {

        $("#error-email").text(translateFieldProperty());
        return false;

    } else {

        setCorrectBorderTo(element);
        $("#error-email").text("");
        return true;

    }

}



function validateMessage() {

    const min = 10;
    const max = 5000;

    const element = document.getElementById('input-message');
    const message = element.value;

    setErrorBorderTo(element);

    if (message.replace(spaceRegex, "") == "" || message == null) {

        $("#error-message").text(translateRequired());
        return false;

    } else if (message.length < min || message.length > max) {

        $("#error-message").text(translateRange(min, max));
        return false;

    } else {

        setCorrectBorderTo(element);
        $("#error-message").text("");
        return true;

    }

}



function setErrorBorderTo(element) {

    setBorderColorTo(element, errorColor);

    element.addEventListener('mouseleave', () => {
        setBorderColorTo(element, errorColor);
    });

    element.addEventListener('mouseenter', () => {
        setBorderColorTo(element, correctColor);
    });
}



function setCorrectBorderTo(element) {

    setBorderColorTo(element, correctColor);

    element.addEventListener('mouseleave', () => {
        setBorderColorTo(element, correctColor);
    });

}



function setBorderColorTo(element, color) {
    element.style.borderColor = color;
}