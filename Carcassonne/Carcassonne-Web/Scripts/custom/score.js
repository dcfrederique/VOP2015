var uri = '/api/Scores';

$(document).ready(function () {
    getScores();
    getTotalScores();
});

function getScores() {
    $("#scores").html('<table id="scoretable" class="table table-striped">' +
    '<tr><th>Datum</th><th>Score</th><th>Winst/Verlies</th</tr></table>');
    $("#pageHeader").text("Scores");
    $.getJSON(uri + '/' + globalUserObj.userId)
        .done(function (data) {
            if (data) {
                $.each(data, function(key, item) {
                    $('<tr>', { html: formatScoreRow(item) }).appendTo($('#scoretable'));
                });
            }
        });
}

function formatScoreRow(item) {
    return Templater(item, '<td>{Game:{Started}}'
        + '</td><td>' + item.AchievedScore + '</td><td>'+( item.Win ? "Winst":"Verlies")+'</td>');
}

function getTotalScores() {
    $.getJSON(uri + '/' + globalUserObj.userId + '/total')
        .done(function (data) {
            if (data) {
                $("#totalscore").html(formatTotalScoreRow(data));
            }
        });
}

function formatTotalScoreRow(item) {
    return Templater(item, '<dl><dt>Score</dt><dd>' + item.score + '</dd><dt>Aantal winst</dt><dd>' + item.wins + '</dd>'
        +'<dt>Aantal verlies</dt><dd>' + (item.total - item.wins) + '</dd></dl>');
}