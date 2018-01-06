process.env.Node_ENV = process.env.Node_ENV || 'development';
var mongoose = require('./config/mongoose');
var express = require('./config/express');
var socketIO = require('./config/socketIO');

var db = mongoose();
var app = express();
var io = socketIO(app);
io.attach(4567);
app.listen(3000);
module.exports = app;

console.log('Server running at http://localhost:3000');

