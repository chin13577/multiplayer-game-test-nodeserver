
var User = require('mongoose').model('User');

exports.Create = function (req, res, next) {
    var user = new User(req.body);
    user.save(function (err) {
        if (err) {
            return next(err);
        }
        else {
            res.json(user);
        }
    })
}

exports.ShowList = function (req, res, next) {
    User.find({}, (err, result) => {
        if (err) {
            return next(err);
        } else {
            res.json(result);
        }
    });
}
exports.UserByUsername = function (req, res, next, username) {
    User.findOne({ username: username }, (err, result) => {
        if (err) {
            return next(err);
        } else {
            req.user = result;
            next();
        }
    })
}
exports.Read = function (req, res) {
    res.json(req.user);
}
exports.Update = function (req, res, next) {
    User.findOneAndUpdate({ username: req.user.username }, req.body, (err, result) => {
        if (err) {
            return next(err);
        } else {
            res.json(result);
        }
    })
}
exports.Delete = function (req, res, next) {
    User.findOneAndRemove({ username: req.user.username }, (err, result) => {
        if (err)
            return next(err);
        else
            res.json(result);
    });
}
exports.Login = function (req, res) {
    console.log(req.body);
    console.log(req.body.id);
    console.log(req.body.password);
    req.checkBody('password', 'Invalid id').notEmpty().isInt();
    res.send(req.body);
}
