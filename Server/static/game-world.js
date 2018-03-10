const gameloop = require('node-gameloop');

module.exports = class GameWorld {
    constructor(io, room) {
        this.room = room;
        this.io = io;
        this.gameObjects = [];
        this.loopId = 0;

        this.sendGameObjectsInterval = null;
        this.tick =0.3;
    }
    addGameObject(obj) {
        this.gameObjects.push(obj);
        // sending to all clients in 'game' room(channel), include sender
        let data = obj;

        this.io.in(this.room).emit('OnSkillCreated', { data });
        // if(this.sendGameObjectsInterval){
        //     clearInterval(this.sendGameObjectsInterval);
        // }
        // this.sendGameObjectsInterval = setInterval(() => {
        //     if(this.gameObjects.length>0){
        //         let gameObjects = this.gameObjects;
        //         this.io.in(this.room).emit('OnSkillUpdated', {gameObjects});
        //     }else{
        //         clearInterval(this.sendGameObjectsInterval);
        //     }
        //   }, 120);
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
        let obj = this.gameObjects[targetIndex];
        this.gameObjects.splice(targetIndex, 1);
        this.io.in(this.room).emit('OnDestroySkill', obj);

    }
    startGameLoop() {
        this.loopId = gameloop.setGameLoop((delta) => {
            for (let i = 0; i < this.gameObjects.length; i++) {
                this.gameObjects[i].update(delta, this);
            }
            if ( this.tick <= 0 && this.gameObjects.length > 0) {
                this.tick=0.3;
                let gameObjects = this.gameObjects;
                this.io.in(this.room).emit('OnSkillUpdated', {gameObjects});
                console.log('update tick');
            } else {
                this.tick-=delta;
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

