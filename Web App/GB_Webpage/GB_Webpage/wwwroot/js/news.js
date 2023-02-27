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

}



