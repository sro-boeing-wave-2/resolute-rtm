"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/ChatHub").build();
var userInput = new String();
var query = new String();
//var message = new string();


connection.start()
    .catch(function (err) {
        return console.error(err.toString());
    });

document.getElementById("configButton").addEventListener("click", function (event) {
    console.log(event);
    userInput = document.getElementById("userInput").value;
    //designation = document.getElementById("designation").value;
    query = document.getElementById("query").value;

    connection.invoke("AllocateMeAnAgent", userInput, query).catch(function (err) {
        return console.error(err.toString());
    });

    connection.on("AllocateMeABot", function (groupId, query) {

    });
});

document.getElementById("sendButton").addEventListener("click", function (event) {

    var message = document.getElementById("messageInput").value;
    var messageobj = { Name : userInput.split("@")[0] , Emailid : userInput , MessageText : message }

    connection.invoke("SendMessage", messageobj).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});