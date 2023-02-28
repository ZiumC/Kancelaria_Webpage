document.getElementById('sort-button').onclick = function () {

    let sortArrow = document.getElementById('sort-arrow');

    const currentClass = String(sortArrow.classList).split('-');
    const currentThemeClass = currentClass[0] + "-" + currentClass[1];
    const currentSort = currentClass[2];


    if (currentSort == "asc") {

        sortArrow.classList = currentThemeClass + '-desc';

    } else {

        sortArrow.classList = currentThemeClass + '-asc';

    }

    sortElementsBy(currentSort);

}



function sortElementsBy(type) {

    let newsTable = document.querySelector('#news tbody');

    let rows = [].slice.call(newsTable.querySelectorAll("tr"));

    rows.sort(function (a, b) {

        let dateFirst = a.cells[0].innerHTML.replace(/\s/g, "");
        let dateSecond = b.cells[0].innerHTML.replace(/\s/g, "");

        if (type == "asc") {

            return convertDate(dateFirst) - convertDate(dateSecond);

        } else {

            return convertDate(dateSecond) - convertDate(dateFirst);

        }


    });

    rows.forEach(function (element) {
        newsTable.appendChild(element);
    });

}


function convertDate(d) {
    let p = d.split(".");
    return +(p[2] + p[1] + p[0]);
}



