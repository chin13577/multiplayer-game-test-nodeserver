const gameloop = require('node-gameloop');

module.exports = class GameWorld {
    constructor(io, room) {
        this.room = room;
        this.io = io;
        this.gameObjects = [];
        this.loopId = 0;
    }
    addGameObject(obj) {
        this.gameObjects.push(obj);
        // sending to all clients in 'game' room(channel), include sender
        let data = this.gameObjects;
        this.io.in(this.room).emit('OnSkillCreated', {data});
    }
    destroyGameObject(id) {
        for (let i = 0; i < this.gameObjects.Length; i++) {
            if (this.gameObjects[i].id == id) {
                targetIndex = i;
                this.gameObjects.splice(i, 1);
                break;
            }
        }

    }
    startGameLoop() {
        this.loopId = gameloop.setGameLoop((delta) => {
            this.gameObjects.forEach((gameObject) => {
                //console.log(gameObject.name);
                gameObject.update(delta);
            });
        }, 1000 / 30);

        setTimeout(()=> {
            console.log('2000ms passed, stopping the game loop');
            gameloop.clearGameLoop(this.loopId);
        }, 5000);
    }
    stopGameLoop() {
        gameloop.clearGameLoop(this.loopId);
    }
};

