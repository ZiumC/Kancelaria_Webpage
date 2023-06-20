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

    if (newsTable == null) {
        return;
    }

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


$(document).ready(function () {
    const maxRowsToShow = 10;
    const startRow = 0;

    $('#news').after('<div id="paging-nav"></div>');
    const totalRows = $('#news tbody tr').length;
    const numberOfPages = totalRows / maxRowsToShow;

    for (i = startRow; i < numberOfPages; i++) {
        const pageNum = i + 1;
        $('#paging-nav').append('<a class="btn btn-secondary" href="#" rel="' + i + '">' + pageNum + '</a> ');
    }

    $('#data tbody tr').hide();
    $('#data tbody tr').slice(startRow, maxRowsToShow).show();

    styleOfNext('#paging-nav a:first');
    showTableRange(startRow, maxRowsToShow);

    $('#paging-nav a').bind('click', function () {

        styleOfPrevious('#paging-nav a');
        styleOfNext(this);

        const currPage = $(this).attr('rel');
        const startItem = currPage * maxRowsToShow;
        const endItem = startItem + maxRowsToShow;
        showTableRange(startItem, endItem);
    });
});

function styleOfNext(element) {
    $(element).addClass('active btn-primary');
    $(element).removeClass('btn-secondary');
}

function styleOfPrevious(element) {
    $(element).addClass('btn-secondary');
    $(element).removeClass('active btn-primary');
}

function showTableRange(start, end) {
    $('#news tbody tr').css('opacity', '0.0').hide().slice(start, end).
        css('display', 'table-row').animate({ opacity: 1 }, 300);
}