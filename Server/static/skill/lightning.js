var Skill = require('./skill');

module.exports = class Lightning extends Skill{
    constructor(id, owner, skillName, position, direction) {
        super(id, owner, skillName, position, direction);

        this.duration = 1;
    }
    update(delta, world) {
        if (this.duration <= 0) {
            world.destroyGameObject(this.id);
        }
        this.duration -= delta;
    }
}