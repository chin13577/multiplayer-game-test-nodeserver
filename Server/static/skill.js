module.exports = class Skill {
    constructor(id, owner, skillName, position, direction) {
        this.id = id;
        this.owner = owner;
        this.skillName = skillName;
        this.position = position;
        this.direction = direction;

        this.duration = 2;
    }
    update(delta, world) {
        let speed = 20;
        this.position[0] += (this.direction[0] * delta * speed);
        this.position[1] = 0.5;
        this.position[2] += (this.direction[2] * delta * speed);
        if (this.duration <= 0) {
            world.destroyGameObject(this.id);
        }
        this.duration -= delta;
        console.log(`${this.id}`,`is update  ${this.position}!`);
        //console.log(this.direction + ' ' + delta);
    }
}