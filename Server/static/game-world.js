const gameloop = require('node-gameloop');

module.exports = class GameWorld {
    constructor(io, room) {
        this.room = room;
        this.io = io;
        this.gameObjects = [];
        this.loopId = 0;

        this.sendGameObjectsInterval = null;
        this.tick =0.1;
    }
    addGameObject(obj) {
        this.gameObjects.push(obj);
        let data = obj;
        this.io.in(this.room).emit('OnSkillCreated', { data });
    }
    destroyGameObject(id) {
        let targetIndex = -1;
        for (let i = 0; i < this.gameObjects.length; i++) {
            if (this.gameObjects[i].id == id) {
                targetIndex = i;
                break;
            }
        }
        if (targetIndex == -1) {
            return;
        }
        this.gameObjects.splice(targetIndex, 1);
        this.io.in(this.room).emit('OnDestroySkill', {id});

    }
    sendUpdateGameObject(data){
        this.io.in(this.room).emit('OnSkillUpdated', {data});
    }
    startGameLoop() {
        this.loopId = gameloop.setGameLoop((delta) => {
            for (let i = 0; i < this.gameObjects.length; i++) {
                this.gameObjects[i].update(delta, this);
            }
        }, 1000 / 30);

        // setTimeout(()=> {
        //     console.log('2000ms passed, stopping the game loop');
        //     gameloop.clearGameLoop(this.loopId);
        // }, 5000);
    }
    stopGameLoop() {
        gameloop.clearGameLoop(this.loopId);
    }
};

