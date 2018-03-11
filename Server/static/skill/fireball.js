var Skill = require('./skill');

module.exports = class Fireball extends Skill {
    constructor(id, owner, skillName, position, direction) {
        super(id, owner, skillName, position, direction);

        this.tick = 0.1;
        this.duration = 2;
    }
    update(delta, world) {
        this.position[0] += (this.direction[0] * delta * 6);
        this.position[1] = 0.5;
        this.position[2] += (this.direction[2] * delta * 6);
        if (this.tick <= 0) {
            this.tick = 0.1;
            let data ={
                id :this.id,
                position :this.position,
                direction:this.direction
            }
            world.sendUpdateGameObject(data);
            console.log('sending skill update.');
        }
        if (this.duration <= 0) {
            world.destroyGameObject(this.id);
        }

        this.tick -= delta;
        this.duration -= delta;
        console.log(`${this.id}`, `is update  ${this.position}!`);
    }
}