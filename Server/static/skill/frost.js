var Skill = require('./skill');

module.exports = class Frost extends Skill{
    constructor(id, owner, skillName, position, direction) {
        super(id, owner, skillName, position, direction);

        this.duration = 3.5;
    }
    update(delta, world) {
        if (this.duration <= 0) {
            world.destroyGameObject(this.id);
        }
        this.duration -= delta;
        console.log(`${this.id}`,`is update  ${this.position}!`);
    }
}