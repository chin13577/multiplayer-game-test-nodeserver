var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var PlayerSchema = new Schema({
    id: {
        type: Schema.ObjectId,
        ref:"User"
    },
    playerName: String,

});
mongoose.model('Player', UserSchema);