
const express = require('express');
var socketIO = require('./static/socket-io');

const PORT = process.env.PORT || 3000;

const server = express()
  .listen(PORT, () => console.log(`Listening on ${ PORT }`));

socketIO(server);
