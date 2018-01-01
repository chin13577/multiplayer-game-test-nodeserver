var io = require('socket.io')({
	transports: ['websocket'],
});

module.exports=function(app){
    var server = require('http').Server(app);

    io.on('connection', function(socket){
        socket.on('OnJoinRoom', function(data){
            currentUser = {
                name: data.name,
            }
            console.log(currentUser.name);
            socket.emit('OnJoinRoom',currentUser);
        });

    })
    return io;
}

