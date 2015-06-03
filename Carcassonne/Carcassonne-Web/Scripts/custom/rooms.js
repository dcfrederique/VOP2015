///reference="Carcassonne_Web/Views/Profile/Profile.cshtml"
var uri = '/api/Rooms';
var gameUri = '/api/Game';
var timer = 0;

$(document).ready(function () {
    getRooms();
});

function getRooms() {
    stopPollingForGameStart();
    if (globalUserObj.hasJoined) {
        stopRoomChat(globalUserObj.roomId);
    }

    $("#rooms").html('<table id="roomtable" class="table table-striped">' +
    '<tr><th>Naam</th><th>Bezetting</th><th>Gesloten?</th</tr>'
    + '</table>');
    $("#rooms").append(Templater({ createbutton: "Create" }, "{button:createbutton:btn btn-primary}"));
    $("#createbutton").click(function () {
        setCreateRoom();
    });
    $("#pageHeader").text("Rooms");
    $.getJSON(uri)
        .done(function (data) {
            $.each(data, function (key, item) {
                $('<tr>', { html: formatRoomRow(item) }).appendTo($('#roomtable'));
            });
            $(".room").click(function (evt) {
                getRoom($(evt.currentTarget).data("id"));
            });
        });
}

function setCreateRoom() {
    $("#rooms").html(Templater({ name: "Naam", submit: "Create" },
        '<form><div class="form-group">{label:name}{input:name:form-control}</div>' +
        '<div class="checkbox"><label><input id="isPrivate" type="checkbox">Gesloten</label></div>' +
        '{button:submit:btn btn-default}</form>'));

    $("#submit").click(function (event) {
        event.preventDefault();
        $.ajax({
            url: uri,
            type: 'PUT',
            data: {
                'RoomName': $(".form-control#name").val(),
                'isPrivate': $("#isPrivate").val()
            },
            success: function () {
                getRooms();
            }
        });
    });
}

function getRoom(id) {
    stopPollingForGameStart();

    $("#pageHeader").text("Room");
    $("#rooms").html('<p><a class="returnToList" style="cursor:pointer;" >Back to List</a></p>');
    $(".returnToList").click(function () {
        getRooms();
    });
    $.getJSON(uri + '/' + id)
         .done(function (data) {
             $("#pageHeader").text("Room - " + data.RoomName);
             $("#rooms").prepend(formatRoomText(data));
             globalUserObj.hasStarted = data.hasStarted;
             if (!globalUserObj.hasJoined) {
                 $("#rooms").prepend(Templater({ joinbutton: "Join" }, "{button:joinbutton:btn btn-primary}"));
                 $("#joinbutton").click(function () { joinRoom(id); });
                 stopRoomChat(globalUserObj.roomId);
             } else {
                 if (parseInt(globalUserObj.roomId) === id) {
                     startRoomChat(id);
                     $("#rooms").prepend(Templater({ leavebutton: "Leave" }, "<div>{button:leavebutton:btn btn-danger}</div>"));
                     $("#leavebutton").click(function () { leaveRoom(id); });
                     if (globalUserObj.userId === data.RoomPrincipal.ID && !globalUserObj.hasStarted) {
                         $("#rooms").prepend(Templater({ gamebutton: "Start Game" }, "<div>{button:gamebutton:btn btn-primary}</div>"));
                         $("#gamebutton").click(function () { startGame(); });
                     } else {
                         pollForGameStart();
                     }
                 }
             }
         })
        .fail(function (jqXHR, textStatus, err) {
            $('#rooms').prepend('Error: ' + err);
        });
}

function formatRoomText(data) {

    return Templater(data,
        "<dl><br/><dt>Bezetting</dt>{Participants::dd:%1/4 spelers}" +
        "</br><dd><dt>Gesloten</dt><dd>" + (data.isPrivate ? "Ja" : "Nee") + "</dd>" +
        '</br><dt>Spelers</dt><dd><ul class="list-unstyled">' +
        '{Participants:<li><img src="{Avatar}" class="img-circle" height="75" />  {span:UserName}</li>}</ul></dd></dl>'
        );
}

function formatRoomRow(item) {
    return Templater(item, '<td><a class="room" style="cursor:pointer;" data-id="{RoomId}">{RoomName}</a></td>'
         + '{Participants::td:%1/4 spelers}<td>' + (item.isPrivate ? "Ja" : "Nee") + '</td>');
}

function joinRoom(id) {
    $.ajax({
        url: uri + '/' + id + '/users',
        type: 'POST',
        data: { '': globalUserObj.userId },
        success: function (response) {
            globalUserObj.hasJoined = true;
            globalUserObj.roomId = id;
            getRoom(id);
        }
    });
}

function leaveRoom(id) {
    $.ajax({
        url: uri + '/' + id + '/users',
        type: 'DELETE',
        data: { '': globalUserObj.userId },
        success: function () {
            globalUserObj.hasJoined = false;
            globalUserObj.roomId = 0;
            getRooms();
        }
    });
}

function startGame() {
    $.ajax({
        url: gameUri,
        type: 'POST',
        data: { '': globalUserObj.roomId },
        success: function (data) {
            globalUserObj.hasStarted = true;
            getRoom(globalUserObj.roomId);
            console.log(data);
            startDesktop(data);
        }
    });
}

function startDesktop(gameid) {
    $.post("/Token", "grant_type=client_credentials", function (data) {
        var i = document.createElement('iframe');
        i.style.display = 'none';
        i.onload = function () { i.parentNode.removeChild(i); };
        i.src =
            "carcassonne:?gameid=" + gameid +
            "&playerid=" + globalUserObj.userId +
            "&token=" + data["access_token"];
        document.body.appendChild(i);
    });
}

function pollForGameStart() {

    var poll = function () {
        $.getJSON(uri + '/' + globalUserObj.roomId)
            .done(function (data) {
                if (data.hasStarted) {
                    startDesktop(data.StartedGame);
                    stopPollingForGameStart();
                }
            });
    };

    timer = setInterval(poll, 5000);
    poll();
}

function stopPollingForGameStart() {
    if (timer) {
        clearInterval(timer);
    }
}