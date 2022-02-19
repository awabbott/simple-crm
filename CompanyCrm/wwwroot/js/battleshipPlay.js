'use strict';

const URL = "https://localhost:44360/api/battleship"

function shotFired(gameId, isUser, row, column) {
    let xhttp = new XMLHttpRequest();
    xhttp.onload = function () {
        if (isUser === true) {
            if (this.response === "true") {
                document.getElementById("user: " + row + ", " + column).innerHTML = "Hit";
            }
            else if (this.response === "false") {
                document.getElementById("user: " + row + ", " + column).innerHTML = "Miss";
            }
            else {
                document.getElementById("user: " + row + ", " + column).innerHTML = "Error";
            }
        }
        else if (isUser === false) {
            if (this.response === "true") {
                document.getElementById("opponent: " + row + ", " + column).innerHTML = "Hit";
            }
            else if (this.response === "false") {
                document.getElementById("opponent: " + row + ", " + column).innerHTML = "Miss";
            }
            else {
                document.getElementById("opponent: " + row + ", " + column).innerHTML = "Error";
            }
        }
    }

    let fullUrl = URL + "/?gameId=" + gameId + "&isUser=" + isUser + "&row=" + row + "&column=" + column;
    xhttp.open("Get", fullUrl, true);
    xhttp.send();
}