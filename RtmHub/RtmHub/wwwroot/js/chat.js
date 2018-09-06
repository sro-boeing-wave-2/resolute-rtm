"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/ChatHub").build();
var userInput, designation, query, messageInput;

//.then(setInterval(connection.invoke("JustLikeThat"), 5000))

connection.start()
    .catch(function (err) {
        return console.error(err.toString());
    });


document.getElementById("configButton").addEventListener("click", function (event) {

    userInput = document.getElementById("userInput").value;
    designation = document.getElementById("designation").value;
    query = document.getElementById("query").value;
    connection.invoke("Config", userInput, designation, query).catch(function (err) {
        return console.error(err.toString());
    });

    connection.on("ReceiveMessage", function (user, message) {
        var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        var encodedMsg = user + " says " + msg;
        var li = document.createElement("li");
        li.textContent = encodedMsg;
        document.getElementById("messagesList").appendChild(li);
    });

    connection.on("ErrorMessage", function () {
        var msg = "Please Enter the Email Again";
        document.getElementById("err").innerHTML = msg;
    });
    
});


document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});