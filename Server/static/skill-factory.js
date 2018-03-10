var Skill = require('./skill');

module.exports = class SkillFactory{

    getSkill(data){
        //Mockup
        if(data.skillName == 'FireBall'){
            return new Skill(data.id,data.owner,data.skillName,data.position,data.direction);
        }else{
            return new Skill(data.id,data.owner,data.skillName,data.position,data.direction);
        }
    }

}