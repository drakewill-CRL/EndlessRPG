using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
    public class Stats
    {
        //TODO: EP-theming. STR should be SOM, and DEF can be VIGOR? LUK might become FLEX?
        public int HP = 0;
        public int maxHP = 0;
        public int AP = 0;
        public int maxAP = 0;
        public int STR = 0;
        public int DEF = 0;
        public int INS = 0;
        public int MOX = 0;
        public int SPD = 0; //for initiative
        public int LUK = 0; //For a few various things but mostly crit.

        //You know, I had a write up for luck getting boosts from various things rolled randomly per character, like 
        //favorite color or lucky number or zodiac sign or such. Maybe I can port that in here later.

        //Stats might be where status effects get stored or set. Normal ones and special ones. Might need to figure out how to un-set them.
        //Later feature. Not needed just yet.

        public void Add(Stats incoming)
        {
            //HP += incoming.HP;
            maxHP += incoming.maxHP;
            HP += incoming.HP; //allows damage to be relayed via a stats object
            if (HP <0)
                HP = 0;
            maxAP += incoming.maxAP;
            AP += incoming.AP; //attacker can burn AP after the attack.
            if (AP <0)
                AP = 0;
            STR += incoming.STR;
            DEF += incoming.DEF;
            INS += incoming.INS;
            MOX += incoming.MOX;
            SPD += incoming.SPD;
            LUK += incoming.LUK;
        }
        public void Set(Stats incoming)
        {
            maxHP = incoming.maxHP;
            HP = incoming.HP; //allows damage to be relayed via a stats object
            maxAP = incoming.maxAP;
            AP = incoming.AP; //attacker can burn AP after the attack.
            STR = incoming.STR;
            DEF = incoming.DEF;
            INS = incoming.INS;
            MOX = incoming.MOX;
            SPD = incoming.SPD;
            LUK = incoming.LUK;
        }

        public Stats Clone()
        {
            Stats clone = (Stats)this.MemberwiseClone();
            return clone;
        }
    }

    public class Role
    {
        public string name;
        public string morphType; //Bio, Synth, or Pod. Affects healing.
        public string spriteSet;
        public Stats startStats;
        public Stats statsPerLevel;
        public List<Ability> abilities;
        
        public string desc = "";
    }



    public class Attack //these are queued up in the CombatEngine.
    {
        public Fightable attacker;
        public List<Fightable> targets = new List<Fightable>();
        public string thingToRoll;
        public Ability thingToDo; //Ability here would also mean that I need entries for the baseline commands.
        //Future possible needs:
        //list of special-priority flags so some moves could always go first/last, used for ordering attacks.
        public string damageType = "blunt"; //used for damage-mod calculations elsewhere.
    }

    public class Item
    {
        public Stats statBoost = new Stats();
        public string name;
    }

    //AttackResults is now the number-changing results for CombatEngine.
    public class AttackResults
    {
        //What happens after an attack.
        public Fightable attacker;
        public List<Fightable> target = new List<Fightable>();
        //public Stats attackerChanges; //Just put the attack in the target and targetChanges list.
        public List<Stats> targetChanges = new List<Stats>(); //Must line up index-wise with the targets.
        public List<string> printDesc = new List<string>();
        //targetchanges should really be done in the CombatEngine, and the printDesc should just show what happened/update the display
        //So I might also need a DIFFERENT list of stuff (display changes, animation effects to draw, etc) for the display-phase stuff, and let stats be done automatically in the combat engine.
    }

    //DisplayResults is the class used by FightScene to show how combat went.
    //Might have a thing set up where display texts have a frameCounter value long enough to read it
    //and stat changes have a 0 or 1 frame time to make those advance along faster.
    public class DisplayResults
    {
        public string desc; //Explains what happened this step.
        public Fightable target; //Could be null. MIght need to be a list in the future?
        public Stats changeStats = new Stats(); //For displaying HP/AP value changes during display phase.
        public string changedItem = ""; //Which property changed. could be sprite.
        public string changedTo = ""; //what it changed to.
        public int frameCounter = 120; //How long to display this particular result, if they're not all on the same timer.
        public bool isLevelUp = false;

    }

    public static class AI
    {
        //mostly a placeholder to remind me i need to work on enemy AI rules.
        //baseilne: just attack. will work until i get abilities and such in place.
        //TODO: make a setup to attach different AIs to different enemies.
        //most will use PickRandom for the most part, but some might need a script to follow for tactics.

        public static Attack PickRandom(Fightable f)
        {
            Attack results = new Attack();
            results.attacker = f;
            results.thingToDo = f.abilities.Where(a => a.apCost <= f.currentStats.AP).OrderBy(a => gameState.random.Next()).First();
            switch(results.thingToDo.targetType)
            {
                case 0: //ability determines targeting.
                break;
                //1 was removed
                case 2: //target a random opponent (characters here)
                results.targets.Add(FightScene.characters.Where(c => c.CanAct()).OrderBy(a => gameState.random.Next()).FirstOrDefault());
                break;
                case 3: //target a random ally (enemies here)
                results.targets.Add(FightScene.enemies.Where(c => c.CanAct()).OrderBy(a => gameState.random.Next()).FirstOrDefault());
                break;
            }
            return results;
        }
    }
}
