﻿'use strict';

let sourceContainerId;
let existingCoordinates = new Object();
document.getElementById('startButton').disabled = true;

class Ship {
    name;
    length;
    index;
    orientation = "";
    coordinates = new Array(this.length);
    constructor(name) {
        this.name = name;
        switch (name) {
            case "Destroyer":
                this.length = 2;
                this.index = 0;
                break;
            case "Submarine":
                this.length = 3;
                this.index = 1;
                break;
            case "Cruiser":
                this.length = 3;
                this.index = 2;
                break;
            case "Battleship":
                this.length = 4;
                this.index = 3;
                break;
            case "Carrier":
                this.length = 5;
                this.index = 4;
                break;
            default:
                break;
        }
    }
}

var dragStart = function (e) {
    try {
        e.dataTransfer.setData('text/plain', e.target.id);
    } catch (ex) {
        e.dataTransfer.setData('Text', e.target.id);
    }
    sourceContainerId = this.parentElement.id;
    //if (this.classList.contains('rotate')) {
    //    orientation = 'vertical';
    //} else {
    //    orientation = 'horizontal';
    //}
    //orientation = (this.classList.contains('rotate')) ? "vertical" : "horizontal";
};

var dragEnter = function (e) {
    cancel(e);
    let targetElement = e.target;
}

var dragLeave = function (e) {
    let targetElement = e.target;
}

var dragOver = function (e) {
    cancel(e);
}

var cancel = function (e) {
    if (e.preventDefault) e.preventDefault();
    if (e.stopPropogation) e.stopPropogation();
    return false;
}

//var dropped = function (e) {
//    let shipName = "";
//    if (this.id !== sourceContainerId) {
//        cancel(e);
//        try {
//            shipName = e.dataTransfer.getData('text/plain');
//        } catch (ex) {
//            shipName = e.dataTransfer.getData('Text');
//        }

//        if (e.target.tagName === 'IMG') {
//            cancel(e);
//        } else if (e.target.className === 'setupCell') {
//            let targetCellId = e.target.id;
//            if (ensureShipIsInbounds(shipName, targetCellId, orientation) !== null) {
//                targetCellId = ensureShipIsInbounds(shipName, targetCellId, orientation);
//            }
//            let newCoordinates = obtainShipCoordinatesArray(shipName, targetCellId, orientation);
//            if (shipsOverlap(shipName, newCoordinates, existingCoordinates)) {
//                alert('Ships Overlap!');
//                cancel(e);
//                return;
//            }
//            existingCoordinates[shipName] = newCoordinates;
//            populateCoordinatesInForm(shipName, newCoordinates);
//            let parentCell = document.getElementById(targetCellId);
//            parentCell.appendChild(document.querySelector('#' + shipName));
//            buttonStatus();
//        } else {
//            e.target.appendChild(document.querySelector('#' + shipName));
//            buttonStatus();
//        }
//    }
//}

var dropped2 = function (e) {
    if (this.id !== sourceContainerId) {
        cancel(e);
        const ship = new Ship(e.dataTransfer.getData('text/plain'));
        ship.orientation = document.getElementById(ship.name).classList.contains('rotate') ? "vertical" : "horizontal";
        if (e.target.tagName === 'IMG') {
            cancel(e);
        } else if (e.target.className === 'setupCell') {
            let cellId = e.target.id;
            ship.coordinates[0] = [Number(cellId.slice(0, 1)), Number(cellId.slice(1))];
            if (!shipIsInbounds(ship)) {
                ship.coordinates[0] = adjustShip(ship)
            }
            ship.coordinates = obtainCoordinates(ship);
            if (shipsOverlap(ship, existingCoordinates)) {
                alert('Ships Overlap!');
                cancel(e);
                return;
            }
            existingCoordinates[ship.name] = ship.coordinates;
            populateCoordinatesInForm(ship);
            let parentCell = document.getElementById(ship.coordinates[0][0].toString() + ship.coordinates[0][1].toString());
            parentCell.appendChild(document.querySelector('#' + ship.name));
            buttonStatus();
        } else {
            e.target.appendChild(document.querySelector('#' + ship.name));
            buttonStatus();
        }
    }
}

var rotateShip = function (e) {
    if (e.target.classList.contains('ship') && e.target.parentElement.parentElement.classList.contains('divBoard')) {
        if (!e.target.classList.contains('rotate')) {
            const ship = new Ship(e.target.id);
            ship.orientation = 'vertical';
            let cellId = e.target.parentElement.id;
            ship.coordinates[0] = [Number(cellId.slice(0, 1)), Number(cellId.slice(1))];
            if (!shipIsInbounds(ship)) {
                ship.coordinates[0] = adjustShip(ship)
            }
            ship.coordinates = obtainCoordinates(ship);
            if (shipsOverlap(ship, existingCoordinates)) {
                alert('Ships Overlap!');
                cancel(e);
                return;
            }
            existingCoordinates[ship.name] = ship.coordinates;
            populateCoordinatesInForm(ship);
            e.target.classList.add('rotate');
            let parentCell = document.getElementById(ship.coordinates[0][0].toString() + ship.coordinates[0][1].toString());
            parentCell.appendChild(document.querySelector('#' + shipName));
        } else {
            const ship = new Ship(e.target.id);
            ship.orientation = 'horizontal';
            let cellId = e.target.parentElement.id;
            ship.coordinates[0] = [Number(cellId.slice(0, 1)), Number(cellId.slice(1))];
            if (!shipIsInbounds(ship)) {
                ship.coordinates[0] = adjustShip(ship)
            }
            ship.coordinates = obtainCoordinates(ship);
            if (shipsOverlap(ship, existingCoordinates)) {
                alert('Ships Overlap!');
                cancel(e);
                return;
            }
            existingCoordinates[ship.name] = ship.coordinates;
            populateCoordinatesInForm(ship);
            e.target.classList.remove('rotate');
            let parentCell = document.getElementById(ship.coordinates[0][0].toString() + ship.coordinates[0][1].toString());
            parentCell.appendChild(document.querySelector('#' + shipName));
        }
    }
}

var shipsOverlap = function (ship, existingCoordinates) {
    for (const newCoordinate of ship.coordinates) {
        for (const [ship, coordinates] of Object.entries(existingCoordinates)) {
            if (ship === ship.name) {
                continue;
            }
            for (const coordinate of coordinates) {
                if (newCoordinate === coordinate) {
                    return true;
                }
            }
        }
    }
    return false;
}

var obtainCoordinates = function (ship) {
    let coordinates = [ship.coordinates[0]];
    if (ship.orientation === 'horizontal') {
        for (var i = 1; i < ship.length; i++) {
            coordinates.push([ship.coordinates[0][0], (ship.coordinates[0][1] + i)]);
        }
    } else if (ship.orientation === 'vertical') {
        for (var i = 1; i < ship.length; i++) {
            coordinates.push([(ship.coordinates[0][0] - i), ship.coordinates[0][1]]);
        }
    }
    return coordinates;
}

let shipIsInbounds = function (ship) {
    if (ship.orientation === 'horizontal' && (ship.coordinates[0][1] + ship.length >= 10) ||
        ship.orientation === 'vertical' && (ship.coordinates[0][0] - ship.length <= 0)) {
        return false;
    } else {
        return true;
    }
}

let adjustShip = function (ship) {
    let initialCoordinate = ship.coordinates[0];
    if (ship.orientation === 'horizontal') {
        initialCoordinate = [ship.coordinates[0][0], (10 - ship.length)];
    } else if (ship.orientation === 'vertical') {
        initialCoordinate = [ship.length, ship.coordinates[0][1]];
    }
    return initialCoordinate;
};

var populateCoordinatesInForm = function (ship) {
    ship.coordinates.forEach(function callback(value, coordinateIndex) {
        let rowInputField = document.getElementById("CoordinateLists_" + ship.index + "__" + coordinateIndex + "__Row");
        rowInputField.value = value[0];
        let columnInputField = document.getElementById("CoordinateLists_" + ship.index + "__" + coordinateIndex + "__Column");
        columnInputField.value = value[1];
    });
}

let buttonStatus = function() {
    var startButton = document.getElementById('startButton');
    let gameBoard = document.getElementById('target-container');
    const ships = gameBoard.querySelectorAll("img");
    if (ships.length === 5) {
        startButton.disabled = false;
    } else {
        startButton.disabled = true;
    }
}

let targets = document.querySelectorAll('[data-role="drag-drop-target"]');
[].forEach.call(targets, function(target) {
    target.addEventListener('drop', dropped2, false);
    target.addEventListener('dragenter', dragEnter, false);
    target.addEventListener('dragover', dragOver, false);
    target.addEventListener('dragleave', dragLeave, false);
});

let rotateTargets = document.querySelectorAll('[class="ship"]');
[].forEach.call(rotateTargets, function(rotateTarget) {
    rotateTarget.addEventListener('click', rotateShip, false);
});

let sources = document.querySelectorAll('[draggable="true"]');
[].forEach.call(sources, function(source) {
    source.addEventListener('dragstart', dragStart, false);
});