document.getElementById('sort-button').onclick = function () {

    var sortArrow = document.getElementById('sort-arrow');

    const currentClass = String(sortArrow.classList).split('-');
    const currentThemeClass = currentClass[0] + "-" + currentClass[1];
    const currentSort = currentClass[2];

    
    if (currentSort == "asc") {

        sortArrow.classList = currentThemeClass + '-desc';

    } else {

        sortArrow.classList = currentThemeClass + '-asc';

    }

    sortBy(currentSort);

}



function sortBy(type) {

    var newsTable = document.querySelector('#news tbody');

    var arrayOfTextDates = newsTable.querySelectorAll('tr th');

    var dates = [arrayOfTextDates.length];
    for (let i = 0; i < arrayOfTextDates.length; i++) {
        dates[i] = arrayOfTextDates[i].innerHTML.replace(/\s/g, "");
    }


    if (type == "asc") {

        dates.sort(function (a, b) {
            return convertDate(a) - convertDate(b);
        });

    } else {

        dates.sort(function (a, b) {
            return convertDate(b) - convertDate(a);
        });

    }

}



function convertDate(d) {
    var p = d.split(".");
    return +(p[2] + p[1] + p[0]);
}



