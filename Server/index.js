var socketIO = require('./static/socket-io');

var app = require('express')();
var server = require('http').Server(app);
var io = socketIO(app);

server.listen(3000);

console.log('Server running at http://localhost:3000');