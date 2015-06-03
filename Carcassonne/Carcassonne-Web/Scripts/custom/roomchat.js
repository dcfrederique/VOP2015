$(function () {

    var connected = false;

    var delay = (function () {
        var timer = 0;
        return function (callback, ms) {
            clearTimeout(timer);
            timer = setTimeout(callback, ms);
        };
    })();

    // Declare a proxy to reference the hub. 
    var chat = $.connection.chatHub;

    startRoomChat = function(id) {
        $(".row").append('<div id="roomchat" class="col-md-6"></div>');
        $("#roomchat").html('<form class="form-inline"><div class="form-group">'
            +'<label class="sr-only" for="message">Message</label>'
            +'<input type="text" class="form-control" id="message" placeholder="Message">'
            + '</div></div><input type="button" id="sendmessage" class="btn btn-default" value="Send" />'
            + '</form><br/><ul class="list-group" id="discussion"><li id="isTyping" class="list-group-item"></li></ul></div>');

        // Create a function that the hub can call to broadcast messages.
        chat.client.broadcastMessage = function(name, message) {
            // Html encode display name and message. 
            var encodedName = $('<div />').text(name).html();
            var encodedMsg = $('<div />').text(message).html();
            // Add the message to the page. 
            $('#discussion').append('<li class="list-group-item"><strong>' + encodedName
                + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
        };
        chat.client.broadcastTyping = function (name) {
            // Html encode display name. 
            var encodedName = $('<div />').text(name).html();
            // Add the message to the page. 
            if ($("." + encodedName + 'isTyping').length === 0) {
                $('#isTyping').prepend('<span class="' + encodedName + 'isTyping" ><strong>' + encodedName
                    + ' is typing...</strong></span>');
                setTimeout(function() {
                    $("." + encodedName + 'isTyping').remove();
                }, 1000);
            }
        };
        // Set initial focus to message input box.  
        $('#message').focus();
        // Start the connection.
        $.connection.hub.start().done(function() {

            chat.server.connect(id);
            connected = true;

            $('#sendmessage').click(function() {
                // Call the Send method on the hub. 
                var d = new Date().toTimeString();
                chat.server.send(id,'[' + d.substring(0, 8) + '] ' + globalUserObj.userName, $('#message').val());
                // Clear text box and reset focus for next comment. 
                $('#message').val('').focus();
            });

            $('#message').keyup(function () {
                // Call the Send method on the hub. 
                delay(function () {
                    chat.server.typing(id, globalUserObj.userName);
                }, 250);
            });
        });
    };

    stopRoomChat = function(id) {
        if (connected) {
            $("#roomchat").remove();
            chat.server.disconnect(id);
            $.connection.hub.stop();
            connected = false;
        }

    };

});