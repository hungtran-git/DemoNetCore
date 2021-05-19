"use strict";
var connection = new
    signalR.HubConnectionBuilder().withUrl("/chatHub").build();
//Disable send button until connection is established.
document.getElementById("sendButton").disabled = true;
connection.on("MessageReceiver", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g,
        "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + ": " + msg;
    var li = document.createElement("li");
    var currentUser = document.getElementById("userInput").value;
    if (currentUser === user) {
        li.className = "list-group-item list-group-item-primary";
    }
    else {
        li.className = "list-group-item list-group-item-success";
    }

    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});
connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});
document.getElementById("sendButton").addEventListener("click", function
    (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("MessageSender", user, message).catch(function
        (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});