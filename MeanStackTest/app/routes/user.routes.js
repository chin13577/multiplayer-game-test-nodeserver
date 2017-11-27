module.exports = (app)=>{
    var user = require('../controllers/user.controller');
    app.post('/login',user.Login);
    app.route('/user')
        .post(user.Create)
        .get(user.ShowList);
    app.route('/user/:username')
        .get(user.Read)
        .put(user.Update)
        .delete(user.Delete);
    app.param('username',user.UserByUsername);
}