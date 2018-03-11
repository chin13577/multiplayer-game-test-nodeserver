module.exports = class Skill {
    constructor(id, owner, skillName, position, direction) {
        this.id = id;
        this.owner = owner;
        this.skillName = skillName;
        this.position = position;
        this.direction = direction;

    }
    update(delta, world) {
        
    }
}