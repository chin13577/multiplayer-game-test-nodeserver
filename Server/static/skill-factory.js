var Skill = require('./skill');

module.exports = class SkillFactory{

    getNormalSkill(name,position,direction){
        return new Skill(name,position,direction);
    }

}