﻿
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
const nameRegex = "^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$";
const emailRegex = "^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$";
const spaceRegex = /\s/g;



function validateName() {

    const element = document.getElementById('input-name');
    const name = element.value;
    const isNameCorrect = new RegExp(nameRegex).test(name);

    setErrorBorderTo(element);

    if (name.replace(spaceRegex, "") == "" || name == null) {

        $("#error-name").text("Pole jest wymagane.");
        return false;

    } else if (name.length < 2 || name.length > 50) {

        $("#error-name").text("Pole powinno zawierać od 2 do 50 znaków.");
        return false;

    } else if (isNameCorrect == false) {

        $("#error-name").text("Pole jest niepoprawnie uzupełnione.");
        return false;

    } else {

        setCorrectBorderTo(element);
        $("#error-name").text("");
        return true;

    }
}



function validateEmail() {

    const element = document.getElementById('input-email');
    const email = element.value;
    const isEmailCorrect = new RegExp(emailRegex).test(email);

    setErrorBorderTo(element);

    if (email.replace(spaceRegex, "") == "" || email == null) {

        $("#error-email").text("Pole jest wymagane.");
        return false;

    } else if (email.length < 2 || email.length > 50) {

        $("#error-email").text("Pole powinno zawierać od 2 do 50 znaków.");
        return false;

    } else if (isEmailCorrect == false) {

        $("#error-email").text("E-mail jest niepoprawny.");
        return false;

    } else {

        setCorrectBorderTo(element);
        $("#error-email").text("");
        return true;

    }

}



function validateMessage() {

    const element = document.getElementById('input-message');
    const message = element.value;

    setErrorBorderTo(element);

    if (message.replace(spaceRegex, "") == "" || message == null) {

        $("#error-message").text("Pole jest wymagane.");
        return false;

    } else if (message.length < 10 || message.length > 5000) {

        $("#error-message").text("Pole powinno zawierać od 10 do 5000 znaków.");
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