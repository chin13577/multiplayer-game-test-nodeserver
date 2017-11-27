var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var UserSchema = new Schema({
    username: { type: String, unique: true ,allowNull: false},
    password: String,
    email: {type:String,index:true}
});

mongoose.model('User',UserSchema);