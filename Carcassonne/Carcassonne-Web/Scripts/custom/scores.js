var uri = '/api/Scores';

var monthNames = [
        "January", "February", "March",
        "April", "May", "June", "July",
        "August", "September", "October",
        "November", "December"
];

$(document).ready(function () {
    $("#scores").html('<div id="monthHeader"></div><table id="scoretable" class="table table-striped">' +
    '<tr><th>Naam</th><th>Score</th><th>Winst/Verlies</th</tr></table>');
    $("#pageHeader").text("Scores");
    getScores(new Date, false);
});

function getScores(date,isTotal) {
    date = (typeof date === 'undefined') ? new Date() : new Date(Date.parse(date));
    isTotal = (typeof isTotal === 'undefined') ? false : isTotal;
    createMonthHeader(date);
    $("#scoretable > * > * > td").parent().remove();

    var newUrl = uri;
    if (!isTotal) {
        newUrl += '/month/' + date.getTime();
        $("#pageHeader").text("Scores - " + monthNames[date.getMonth()] + ' ' + date.getFullYear());
    } else {
        $("#pageHeader").text("Scores - Highscore");
    }

    $.getJSON(newUrl)
        .done(function (data) {
            $.each(data, function (key, item) {
                $("<tr>", { html: formatScoreRow(item) }).appendTo($('#scoretable'));
            });
        });
}


function formatScoreRow(item) {
    return Templater(item, '<td style="text-align: center"><img src="{Player:{Avatar}}" class="img-circle" height="75" /></td><td><a class="score" href="/Profile/Details/{Player:{ID}}">{Player:<div>{UserName}</div>}</a>'
        + '</td><td>{TotalScore}</td><td>{Wins}/' + (item.Games - item.Wins) + '</td>');
}

function createMonthHeader(date) {
    date = (typeof date === 'undefined') ? new Date() : new Date(Date.parse(date));

    $("#monthHeader").html('<nav><ul class="pager">' +
        '<li><a href="#" id="prevMonth">Previous</a></li>' +
        '<li><a id="thisMonth">This Month</a></li>' +
        '<li><a id="total" href="#">Total</a></li>' +
        '<li><a id="nextMonth" href="#">Next</a></li></ul></nav>');


    var x = date;

    $("#total").attr("data-date", x.toISOString()).click(function (e) {
        getScores(e.target.dataset["date"],true);
    });

    x.setMonth(x.getMonth() - 1);
    $("#prevMonth").html(monthNames[x.getMonth()] + ' ' + x.getFullYear()).attr("data-date", x.toISOString());
    $("#prevMonth").click(function (e) {
        getScores(e.target.dataset["date"]);
    });

    var today = new Date();
    $("#thisMonth").html(monthNames[today.getMonth()] + ' ' + today.getFullYear()).attr("data-date", today.toISOString());
    $("#thisMonth").click(function (e) {
        getScores(e.target.dataset["date"]);
    });

    x.setMonth(x.getMonth() + 2);
    $("#nextMonth").html(monthNames[x.getMonth()] + ' ' + x.getFullYear()).attr("data-date", x.toISOString());
    $("#nextMonth").click(function (e) {
        getScores(e.target.dataset["date"]);
    });
}