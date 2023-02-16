

document.getElementById('send-button').onclick = function () {

    let isNameCorrect = validateName();
    let isEmailCorrect = validateEmail();
    let isMessageCorrect = validateMessage();


    if (isNameCorrect && isEmailCorrect && isMessageCorrect) {
        $('#contact-form').attr('onsubmit', 'return true;');
    } else {
        $('#contact-form').attr('onsubmit', 'return false;');
    }

};

function validateName() {

    const name = document.getElementById('input-name').value;
    const isNameCorrect = new RegExp("^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$").test(name);

    if (name.replace(/\s/g, "") == "" || name == null) {

        $("#error-name").text("Pole jest wymagane.");
        return false;

    } else if (name.length < 2 || name.length > 50) {

        $("#error-name").text("Pole powinno zawierać od 2 do 50 znaków.");
        return false;

    } else if (isNameCorrect == false) {

        $("#error-name").text("Pole jest niepoprawnie uzupełnione.");
        return false;

    } else {

        $("#error-name").text("");
        return true;

    }
}

function validateEmail() {

    const email = document.getElementById('input-email').value;
    const isEmailCorrect = new RegExp("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$").test(email);

    if (email.replace(/\s/g, "") == "" || email == null) {

        $("#error-email").text("Pole jest wymagane.");
        return false;

    } else if (email.length < 2 || email.length > 50) {

        $("#error-email").text("Pole powinno zawierać od 2 do 50 znaków.");
        return false;

    } else if (isEmailCorrect == false) {

        $("#error-email").text("E-mail jest niepoprawny.");
        return false;

    } else {

        $("#error-email").text("");
        return true;

    }

}

function validateMessage() {

    const message = document.getElementById('input-message').value;

    if (message.replace(/\s/g, "") == "" || message == null) {

        $("#error-message").text("Pole jest wymagane.");
        return false;

    } else if (message.length < 10 || message.length > 5000) {

        $("#error-message").text("Pole powinno zawierać od 10 do 5000 znaków.");
        return false;

    } else {

        $("#error-message").text("");
        return true;

    }

}