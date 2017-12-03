var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var UserSchema = new Schema({
    username: { type: String, unique: true, required: true, trim: true },
    password: {
        type: String,
        validate: [
            function(password){
                return password && password.length>=6;
            },"Password must be at least 6 characters"
        ]
    },
    email: { type: String, index: true, match: /.+\@+\.+/ },
    created: { type: Date, default: Date.now() }
});

mongoose.model('User', UserSchema);