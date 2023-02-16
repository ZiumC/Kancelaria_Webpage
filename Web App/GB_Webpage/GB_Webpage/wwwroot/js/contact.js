

document.getElementById('send-button').onclick = function () {

    let isNameCorrect = validateName();


    if (isNameCorrect) {
        $('#contact-form').attr('onsubmit', 'return true;');
    } else {
       $('#contact-form').attr('onsubmit', 'return false;');
    }

};

function validateName() {

    const name = document.getElementById('input-name').value;
    const isNameCorrect = new RegExp("^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$").test(name);

    if (name.replace(/\s/g, "") == "" || name == null) {

        $("#error-name").text("To pole jest wymagane.");
        return false;

    } else if (isNameCorrect == false) {

        $("#error-name").text("To pole jest niepoprawnie uzupełnione.");
        return false;

    } else if (name.length < 2 || name.length > 50) {

        $("#error-name").text("Pole powinno zawierać od 2 do 50 znaków.");
        return false;

    } else {

        $("#error-name").text("");
        return true;

    }
}