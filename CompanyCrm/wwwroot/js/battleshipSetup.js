'use strict';

let sourceContainerId;
let orientation = "";
let existingCoordinates = new Object();
document.getElementById('startButton').disabled = true;

class Ship {
    name;
    length;
    index;
    orientation = "";
    initialCoordinate = "";
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
                this.index = 2;
                break;
            case "Carrier":
                this.length = 5;
                this.index = 3;
                break;
            default:
                break;
        }
    }
}

var dragStart = function(e) {
    try {
        e.dataTransfer.setData('text/plain', e.target.id);
    } catch (ex) {
        e.dataTransfer.setData('Text', e.target.id);
    }
    sourceContainerId = this.parentElement.id;
    if (this.classList.contains('rotate')) {
        orientation = 'vertical';
    } else {
        orientation = 'horizontal';
    }
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

var dropped = function (e) {
    let shipName = "";
    if (this.id !== sourceContainerId) {
        cancel(e);
        try {
            shipName = e.dataTransfer.getData('text/plain');
        } catch (ex) {
            shipName = e.dataTransfer.getData('Text');
        }

        if (e.target.tagName === 'IMG') {
            cancel(e);
        } else if (e.target.className === 'setupCell') {
            let targetCellId = e.target.id;
            if (ensureShipIsInbounds(shipName, targetCellId, orientation) !== null) {
                targetCellId = ensureShipIsInbounds(shipName, targetCellId, orientation);
            }
            let newCoordinates = obtainShipCoordinatesArray(shipName, targetCellId, orientation);
            if (shipsOverlap(shipName, newCoordinates, existingCoordinates)) {
                alert('Ships Overlap!');
                cancel(e);
                return;
            }
            existingCoordinates[shipName] = newCoordinates;
            populateCoordinatesInForm(shipName, newCoordinates);
            let parentCell = document.getElementById(targetCellId);
            parentCell.appendChild(document.querySelector('#' + shipName));
            buttonStatus();
        } else {
            e.target.appendChild(document.querySelector('#' + shipName));
            buttonStatus();
        }
    }
}

var dropped2 = function (e) {
    let shipName = "";
    if (this.id !== sourceContainerId) {
        cancel(e);
        try {
            shipName = e.dataTransfer.getData('text/plain');
        } catch (ex) {
            shipName = e.dataTransfer.getData('Text');
        }

        const ship = new Ship(shipName);

        if (e.target.tagName === 'IMG') {
            cancel(e);
        } else if (e.target.className === 'setupCell') {
            ship.initialCoordinate = e.target.id;
            if (!shipIsInbounds(ship, orientation)) {
                ship.initialCoordinate = adjustShip(ship, orientation);
            }
            let newCoordinates = obtainShipCoordinatesArray(ship.name, ship.initialCoordinate, orientation);
            if (shipsOverlap(ship.name, newCoordinates, existingCoordinates)) {
                alert('Ships Overlap!');
                cancel(e);
                return;
            }
            existingCoordinates[ship.name] = newCoordinates;
            populateCoordinatesInForm(ship.name, newCoordinates);
            let parentCell = document.getElementById(ship.initialCoordinate);
            parentCell.appendChild(document.querySelector('#' + ship.name));
            buttonStatus();
        } else {
            e.target.appendChild(document.querySelector('#' + ship.name));
            buttonStatus();
        }
    }
}

var shipsOverlap = function (shipName, newCoordinates, existingCoordinates) {
    for (const newCoordinate of newCoordinates) {
        for (const [ship, coordinates] of Object.entries(existingCoordinates)) {
            if (ship === shipName) {
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

var rotateShip = function (e) {
    if (e.target.classList.contains('ship') && e.target.parentElement.parentElement.classList.contains('divBoard')) {
        if (!e.target.classList.contains('rotate')) {
            let shipName = e.target.id;
            let targetCellId = e.target.parentElement.id;
            let orientation = 'vertical';
            if (ensureShipIsInbounds(shipName, targetCellId, orientation) !== null) {
                targetCellId = ensureShipIsInbounds(shipName, targetCellId, orientation);
            };
            let shipCoordinates = obtainShipCoordinatesArray(shipName, targetCellId, orientation);
            if (shipsOverlap(shipName, shipCoordinates, existingCoordinates)) {
                alert('Ships Overlap!');
                cancel(e);
                return;
            }
            existingCoordinates[shipName] = shipCoordinates;
            e.target.classList.add('rotate');
            populateCoordinatesInForm(shipName, shipCoordinates);
            let parentCell = document.getElementById(targetCellId);
            parentCell.appendChild(document.querySelector('#' + shipName));
        } else {
            let shipName = e.target.id;
            let targetCellId = e.target.parentElement.id;
            let orientation = 'horizontal';
            if (ensureShipIsInbounds(shipName, targetCellId, orientation) !== null) {
                targetCellId = ensureShipIsInbounds(shipName, targetCellId, orientation);
            };
            let shipCoordinates = obtainShipCoordinatesArray(shipName, targetCellId, orientation);
            if (shipsOverlap(shipName, shipCoordinates, existingCoordinates)) {
                alert('Ships Overlap!');
                cancel(e);
                return;
            }
            existingCoordinates[shipName] = shipCoordinates;
            e.target.classList.remove('rotate');
            populateCoordinatesInForm(shipName, shipCoordinates);
            let parentCell = document.getElementById(targetCellId);
            parentCell.appendChild(document.querySelector('#' + shipName));
        }
    }
}

var obtainShipCoordinatesArray = function (shipName, targetCellId, orientation) {
    let cells = document.getElementsByClassName("setupCell");
    let coordinateArray = [];

    let shipLength = 0;
    switch (shipName) {
        case "Destroyer":
            shipLength = 2;
            break;
        case "Submarine":
        case "Cruiser":
            shipLength = 3;
            break;
        case "Battleship":
            shipLength = 4;
            break;
        case "Carrier":
            shipLength = 5;
            break;
        default:
            shipLength = 0;
            break;
    }
    for (var i = 0; i < cells.length; i++) {
        if (cells[i].id === targetCellId && shipName === 'Destroyer') {
            coordinateArray.push(cells[i].id);
            for (var j = 1; j < shipLength; j++) {
                coordinateArray.push(nextCoordinate(cells[i].id, j, orientation));
            }
            return coordinateArray;
        } else if ((cells[i].id === targetCellId && shipName === 'Submarine') ||
          (cells[i].id === targetCellId && shipName === 'Cruiser')) {
            coordinateArray.push(cells[i].id);
            for (var j = 1; j < shipLength; j++) {
                coordinateArray.push(nextCoordinate(cells[i].id, j, orientation));
            }
            return coordinateArray;
        } else if (cells[i].id === targetCellId && shipName === 'Battleship') {
            coordinateArray.push(cells[i].id);
            for (var j = 1; j < shipLength; j++) {
                coordinateArray.push(nextCoordinate(cells[i].id, j, orientation));
            }
            return coordinateArray;
        } else if (cells[i].id === targetCellId && shipName === 'Carrier') {
            coordinateArray.push(cells[i].id);
            for (var j = 1; j < shipLength; j++) {
                coordinateArray.push(nextCoordinate(cells[i].id, j, orientation));
            }
            console.log(coordinateArray);
            return coordinateArray;
        }
    }
}

function nextCoordinate(initialCoordinate, incrementer, orientation) {
    if (orientation === 'horizontal') {
        let initialRow = initialCoordinate.slice(0, 1);
        let initialColumn = parseInt(initialCoordinate.slice(1), 10);
        let newColumn = initialColumn + incrementer;
        let newColumnString = newColumn.toString();
        let newCoordinate = initialRow + newColumnString;
        return newCoordinate;
    } else if (orientation === 'vertical') {
        let initialRow = parseInt(initialCoordinate.slice(0, 1), 10);
        let initialColumn = initialCoordinate.slice(1);
        let newRow = initialRow - incrementer;
        let newRowString = newRow.toString();
        let newCoordinate = newRowString + initialColumn;
        return newCoordinate;
    }
}

var ensureShipIsInbounds = function (shipName, targetCellId, orientation) {
    let targetCellRow = parseInt(targetCellId.charAt(0));
    let targetCellColumn = parseInt(targetCellId.charAt(1));

    if (orientation === 'horizontal') {
        if (shipName === 'Destroyer' && targetCellColumn === 9) {
            let newTargetCellId = targetCellRow.toString() + '8';
            return newTargetCellId;
        } else if (shipName === 'Submarine' && targetCellColumn >= 8 ||
            shipName === 'Cruiser' && targetCellColumn >= 8) {
            let newTargetCellId = targetCellRow.toString() + '7';
            return newTargetCellId;
        } else if (shipName === 'Battleship' && targetCellColumn >= 7) {
            let newTargetCellId = targetCellRow.toString() + '6';
            return newTargetCellId;
        } else if (shipName === 'Carrier' && targetCellColumn >= 6) {
            let newTargetCellId = targetCellRow.toString() + '5';
            return newTargetCellId;
        }
    } else if (orientation === 'vertical') {
        if (shipName === 'Destroyer' && targetCellRow === 0) {
            let newTargetCellId = '1' + targetCellColumn.toString();
            return newTargetCellId;
        } else if (shipName === 'Submarine' && targetCellRow <= 1 ||
            shipName === 'Cruiser' && targetCellRow <= 1) {
            let newTargetCellId = '2' + targetCellColumn.toString();
            return newTargetCellId;
        } else if (shipName === 'Battleship' && targetCellRow <= 2) {
            let newTargetCellId = '3' + targetCellColumn.toString();
            return newTargetCellId;
        } else if (shipName === 'Carrier' && targetCellRow <= 3) {
            let newTargetCellId = '4' + targetCellColumn.toString();
            return newTargetCellId;
        }
    }
    return null;
}

//let shipIsInbounds = function (shipLength, targetCellId, orientation) {
let shipIsInbounds = function (ship, orientation) {
    //let targetCellRow = parseInt(targetCellId.charAt(0));
    let targetCellRow = parseInt(ship.initialCoordinate.charAt(0));
    let targetCellColumn = parseInt(ship.initialCoordinate.charAt(1));

    if (orientation === 'horizontal' && (targetCellColumn + ship.length >= 10) ||
        orientation === 'vertical' && (targetCellRow + ship.length <= 0)) {
        return false;
    } else {
        return true;
    }
}

let adjustShip = function (ship, orientation) {
    let targetCellRow = parseInt(ship.initialCoordinate.charAt(0));
    let targetCellColumn = parseInt(ship.initialCoordinate.charAt(1));

    if (orientation === 'horizontal') {
        let newTargetCellId = targetCellRow.toString() + (9 - ship.length).toString();
        return newTargetCellId;
    } else if (orientation === 'vertical') {
        let newTargetCellId = ship.length.toString() + targetCellColumn.toString();
        return newTargetCellId;
    }
    return null;
};

var populateCoordinatesInForm = function (shipName, coordinateArray) {
    let shipIndex = 0;
    switch (shipName) {
        case "Destroyer":
            shipIndex = 0;
            break;
        case "Submarine":
            shipIndex = 1;
            break;
        case "Cruiser":
            shipIndex = 2;
            break;
        case "Battleship":
            shipIndex = 3;
            break;
        case "Carrier":
            shipIndex = 4;
            break;
        default:
            shipIndex = 5;
            break;
    }

    coordinateArray.forEach(function callback(value, coordinateIndex) {
        let row = value[0];
        let rowInputField = document.getElementById("CoordinateLists_" + shipIndex + "__" + coordinateIndex + "__Row");
        rowInputField.value = row;
        let column = value[1];
        let columnInputField = document.getElementById("CoordinateLists_" + shipIndex + "__" + coordinateIndex + "__Column");
        columnInputField.value = column;
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