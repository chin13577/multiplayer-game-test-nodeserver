
var io = require('socket.io')({ transports: ['websocket'], });
var shortId = require('shortId');
var Enum = require('enum');
var userStatus = new Enum(['Lobby', 'Waiting', 'Played']);

var roomList = {
    rooms: {}
};
var player={
    name:'',
    hp:100,
    position:[0,0,0],
    rotation:[0,0,0]
}
module.exports = function (app) {
    var server = require('http').Server(app);

    io.on('connection', function (socket) {
        socket.leaveAll();
        var user = null;

        socket.on('OnJoinRoom', function (data) {
            // if first join. initial user.
            if (user === null) {
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
                    RemoveUserInRoom(socket.room, user);
                    socket.broadcast.to(socket.room).emit("OnLeaveRoom", user);
                });
            }
            socket.join(data.room, () => {
                user.room = data.room;
                socket.room = data.room;
                AddUserInRoom(socket, user.room, user);
            });
        });

        socket.on('OnPlay',()=>{

        });

        socket.on('Test', () => {
            console.log(socket.id + ' ' + socket.room);
            console.log(io.sockets.adapter.rooms);
        });

        socket.on('disconnect', (reason) => {
            if (socket.room) {
                socket.leave(socket.room, () => {
                    socket.broadcast.to(socket.room).emit('OnLeaveRoom', user);
                    RemoveUserInRoom(socket.room,user);
                    socket.room = null;
                    user = null;
                });
            }
        });
    });

    return io;
}

function AddUserInRoom(socket, roomName, user) {
    if (!roomList.rooms[roomName]) {
        // check room is undefine so create room.
        roomList.rooms[roomName] = [];
        roomList.rooms[roomName].push(user);
        let roomData = GetRoomData();
        io.to("lobby").emit("OnRoomCreated", { roomData });
    } else {
        // else check if user isn't exist.
        let index = roomList.rooms[roomName].indexOf(user);
        if (index === -1) {
            roomList.rooms[roomName].push(user);
            let userInRoom = roomList.rooms[roomName];
            socket.broadcast.to(roomName).emit("OnJoinRoom", {userInRoom});
        }
    }
}
function RemoveUserInRoom(roomName, user) {
    let index = roomList.rooms[roomName].indexOf(user);
    if (index !== -1) {
        roomList.rooms[roomName].splice(index, 1);
        if(roomList.rooms[roomName].length===0){
            delete roomList.rooms[roomName];
        }
    }
}
function GetRoomData() {
    let data = [];
    let roomNames = Object.getOwnPropertyNames(roomList.rooms);
    for (i = 0; i < roomNames.length; i++) {
        data.push({
            name: roomNames[i],
            length: roomList.rooms[roomNames[i]].length
        });
    }
    return data;
}