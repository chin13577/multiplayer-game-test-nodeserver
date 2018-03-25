var Fireball = require('./skill/fireball');
var Lightning = require('./skill/lightning');
var Frost = require('./skill/frost');

module.exports = class SkillFactory{

    getSkill(data){
        if(data.skillName == 'Fireball'){
            return new Fireball(data.id,data.owner,data.skillName,data.position,data.direction);
        }else if (data.skillName == 'Lightning'){
            return new Lightning(data.id,data.owner,data.skillName,data.position,data.direction);
        }else if(data.skillName == 'Frost'){
            return new Frost(data.id,data.owner,data.skillName,data.position,data.direction);
        }
    }
    getSkillDamage(skillName){
        if(skillName == 'Fireball'){
            return 10;
        }else if (skillName == 'Lightning'){
            return 15;
        }else if(skillName == 'Frost'){
            return 15;
        }
    }

}