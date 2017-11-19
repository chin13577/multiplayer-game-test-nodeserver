module.exports = (app)=>{
    var userController = require('../controllers/user.controller');
    app.post('/login',userController.login);
}