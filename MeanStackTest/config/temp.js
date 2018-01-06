var io = require("socket.io")({ transports: ["websocket"] });
io.attach(4567);

var users = {
  rooms: {}
};

io.on("connection", function(socket) {
  socket.leaveAll();
  var user = null;
  console.log(socket.id);

  socket.on("JoinRoom", function(data) {
    // if first join. initial user.
    if (user === null) {
      user = {
        name: data.name,
        room: data.room
      };
    }
    if (socket.room && socket.room == data.room) {
      return;
    }

    if (socket.room) {
      socket.leave(socket.room, () => {
        RemoveUserInRoom(socket.room, user);
        console.log(user.rooms[socket.room].length);
        socket.broadcast.to(socket.room).emit("OnLeaveRoom", user);
      });
    }

    socket.join(data.room, () => {
      user.room = data.room;
      socket.room = data.room;

      if (!users.rooms[data.room]) {
        // create room
        CreateRoom(data.room);
        io.to("lobby").emit("OnRoomCreated", users.rooms);
      }
      AddUserInRoom(user.room, user);
      // send message to clients.
      if (data.room === "lobby") {
        io.to(socket.room).emit("OnJoinRoom", users.rooms);
      } else {
        io.to(socket.room).emit("OnJoinRoom", users.rooms[socket.room]);
      }
    });
  });

  socket.on("Test", () => {
    console.log(socket.id + " " + socket.room);
    console.log(io.sockets.adapter.rooms);
  });

  socket.on("disconnect", reason => {
    if (socket.room) {
      socket.leave(socket.room, () => {
        socket.broadcast.to(socket.room).emit("OnLeaveRoom", user);
        socket.room = null;
        user = null;
      });
    }
  });
});

function CreateRoom(roomName) {
  // check room is undefine so create room.
  users.rooms[roomName] = [];
}

function AddUserInRoom(roomName, user) {
  // else check if user isn't exist.
  let index = users.rooms[roomName].indexOf(user);
  if (index === -1) {
    users.rooms[roomName].push(user);
  }
}
function RemoveUserInRoom(roomName, user) {
  let index = users.rooms[roomName].indexOf(user);
  if (index !== -1) {
    users.rooms.splice(index, 1);
  }
}
function RemoveRoom(roomName) {
  delete users.rooms[roomName];
  if (users.rooms[roomName]) {
    let index = users.rooms.indexOf(roomName);
    if (index !== -1) {
      users.rooms.splice(index, 1);
    }
  }
}