var io = require('socket.io')({ transports: ['websocket'], });

var GameWorld  = require('./game-world');
module.exports = function (app) {
    var roomList = {};
    var server = require('http').Server(app);

    io.on('connection', function (socket) {
        socket.leaveAll();
        var user = null;
        var currentPlayer = null;
        
        socket.on('JoinRoom', function (data) {
            // if first join. initial user.
            if (!user) {
                user = {
                    name: data.name,
                    room: data.room
                };
            }
            if (socket.room == data.room) {
                return;
            }
            if (socket.room) {
                socket.leave(socket.room, () => {
                    removeUserInRoom(socket, user);
                    socket.broadcast.to(socket.room).emit("OnLeaveRoom", user);
                });
            }
            socket.join(data.room, () => {
                user.room = data.room;
                socket.room = data.room;
                addUserInRoom(socket, user);
            });
        });
        // on user start game.
        socket.on('Start', (data) => {
            if(!roomList[socket.room].world){
                roomList[socket.room].world = new GameWorld(io,socket.room);
                roomList[socket.room].world.startGameLoop;
            }
            socket.broadcast.to(socket.room).emit('OnStart', { data });
        });

        socket.on('Play', (data) => {
            console.log(JSON.stringify(data));
            if (!roomList[socket.room].spawnPoints) {
                roomList[socket.room].spawnPoints = [];
                data.spawnPoints.forEach(element => {
                    roomList[socket.room].spawnPoints.push(element);
                });
            }
            if (!roomList[socket.room].players) {
                roomList[socket.room].players = [];
            }
            currentPlayer = {
                name: data.name,
                hp: 100,
            }
            currentPlayer.position = roomList[socket.room].spawnPoints[roomList[socket.room].players.length];
            roomList[socket.room].players.push(currentPlayer);
            let players = roomList[socket.room].players;
            io.to(socket.room).emit('OnPlay', { players });
        });

        socket.on('Move', (data) => {
            currentPlayer.position = data.position;
            let dataObj = {
                name: currentPlayer.name,
                position: currentPlayer.position
            };
            socket.broadcast.to(socket.room).emit('OnMove', dataObj);
        });

        socket.on('Rotate', (data) => {
            currentPlayer.rotation = data.rotation;
            let dataObj = {
                name: currentPlayer.name,
                rotation: currentPlayer.rotation
            };
            socket.broadcast.to(socket.room).emit('OnRotate', dataObj);
        });

        socket.on('Animate', (data) => {
            let anim = {
                name: data.name,
                args: data.args
            };
            currentPlayer.animation = anim;
            let dataObj = {
                name: currentPlayer.name,
                animation: currentPlayer.animation
            }
            socket.broadcast.to(socket.room).emit('OnAnimChange', dataObj);
        });

        socket.on('SpawnSkill', (data) => {
            if (!roomList[socket.room].skillDict) {
                roomList[socket.room].skillDict = {};
            }
            let skillDict = roomList[socket.room].skillDict;
            let obj = {
                id: data.id,
                owner: data.owner,
                skillName: data.skillName,
                position: data.position,
                direction: data.direction
            };
            skillDict[obj.id] = obj;
            console.log(obj);
            io.to(socket.room).emit('OnSpawnSkill', obj);
        });
        
        socket.on('DestroySkill', (data) => {
            let obj = {
                id: data.id,
            };
            let skillDict = roomList[socket.room].skillDict;
            if (!skillDict[obj.id]) {
                return;
            }
            delete skillDict[obj.id];
            socket.broadcast.to(socket.room).emit('OnDestroySkill', obj);
        });


        socket.on('Test', () => {
            console.log(socket.id + ' ' + socket.room);
            console.log(io.sockets.adapter.rooms);
        });

        socket.on('disconnect', (reason) => {
            if (socket.room) {
                socket.leave(socket.room, () => {
                    socket.broadcast.to(socket.room).emit('OnLeaveRoom', user);
                    removePlayerInRoom(socket, user);
                    removeUserInRoom(socket, user);
                    socket.room = null;
                    user = null;
                });
            }
        });
    });
    return io;
}

function addUserInRoom(socket, user) {
    if (!roomList[socket.room]) {
        // check room is undefine so create room.
        roomList[socket.room] = {};
        roomList[socket.room].users = [];
        roomList[socket.room].users.push(user);
        let roomData = getRoomData();
        io.to("lobby").emit("OnRoomCreated", { roomData });
    } else {
        // else check if user isn't exist.
        let index = roomList[socket.room].users.indexOf(user);
        if (index === -1) {
            roomList[socket.room].users.push(user);
            let userInRoom = roomList[socket.room].users;
            socket.broadcast.to(socket.room).emit("OnJoinRoom", { userInRoom });
        }
    }
}
function removePlayerInRoom(socket, user) {
    if (roomList[socket.room].players) {
        let index = roomList[socket.room].players.findIndex((element) => {
            return element.name === user.name;
        });
        if (index !== -1) {
            roomList[socket.room].players.splice(index, 1);
        }
    }
}
function removeUserInRoom(socket, user) {
    let index = roomList[socket.room].users.indexOf(user);
    if (index !== -1) {
        roomList[socket.room].users.splice(index, 1);
        if (roomList[socket.room].users.length === 0) {
            delete roomList[socket.room];
        }
    }
}
function getRoomData() {
    let data = [];
    let roomNames = Object.getOwnPropertyNames(roomList);
    for (i = 0; i < roomNames.length; i++) {
        data.push({
            name: roomNames[i],
            length: roomList[roomNames[i]].users.length
        });
    }
    return data;
}