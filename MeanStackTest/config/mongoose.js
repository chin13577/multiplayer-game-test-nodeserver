var mongoose = require('mongoose');
var config = require("./config.js");

module.exports=function(){
    mongoose.set('debug',config.debug);
    var db = mongoose.connect(config.mongoUri);
    require('../app/models/user.model');
    return db;
}
