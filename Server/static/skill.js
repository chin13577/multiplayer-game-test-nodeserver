module.exports = class Skill{
    constructor (name,position,direction)
    {
        this.name = name;
        this.position =position;
        this.direction = direction;
    }
    update(delta){
        this.position[0]+=this.direction[0]*delta;
        this.position[1]+=this.direction[1]*delta;
        this.position[2]+=this.direction[2]*delta;
        console.log(`${this.name}`,`is update  ${this.position}!`);
    }
}