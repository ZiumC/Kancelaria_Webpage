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
    $('#news').after('<div id="paging-nav"></div>');
    var rowsShown = 10;
    var rowsTotal = $('#news tbody tr').length;
    var numPages = rowsTotal / rowsShown;
    for (i = 0; i < numPages; i++) {
        var pageNum = i + 1;
        $('#paging-nav').append('<a class="btn btn-secondary" href="#" rel="' + i + '">' + pageNum + '</a> ');
    }
    $('#data tbody tr').hide();
    $('#data tbody tr').slice(0, rowsShown).show();
    $('#paging-nav a:first').addClass('active btn-primary');
    $('#paging-nav a:first').removeClass('btn-secondary');
    $('#paging-nav a').bind('click', function () {
        $('#paging-nav a').addClass('btn-secondary');
        $('#paging-nav a').removeClass('active btn-primary');
        $(this).addClass('active btn-primary');
        $(this).removeClass('btn-secondary');

        var currPage = $(this).attr('rel');
        var startItem = currPage * rowsShown;
        var endItem = startItem + rowsShown;
        $('#news tbody tr').css('opacity', '0.0').hide().slice(startItem, endItem).
            css('display', 'table-row').animate({ opacity: 1 }, 300);
    });
});
