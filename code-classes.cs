using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
    public class Stats
    {
        public int HP;
        public int maxHP;
        public int MP;
        public int maxMP;
        public int STR;
        public int DEF;
        public int MAGIC;
        public int MDEF;
        public int SPD; //for initiative
        public int LUK; //For a few various things.
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
            maxMP += incoming.maxMP;
            MP += incoming.MP; //attacker can burn MP after the attack.
            if (MP <0)
                MP = 0;
            STR += incoming.STR;
            DEF += incoming.DEF;
            MAGIC += incoming.MAGIC;
            MDEF += incoming.MDEF;
            SPD += incoming.SPD;
            LUK += incoming.LUK;
        }
        public void Set(Stats incoming)
        {
            maxHP = incoming.maxHP;
            HP = incoming.HP; //allows damage to be relayed via a stats object
            maxMP = incoming.maxMP;
            MP = incoming.MP; //attacker can burn MP after the attack.
            STR = incoming.STR;
            DEF = incoming.DEF;
            MAGIC = incoming.MAGIC;
            MDEF = incoming.MDEF;
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
        public Stats startStats;
        public Stats statsPerLevel;
        public List<Ability> abilities;
        //Maybe creature type goes on Role to be assigned to the charactrer?
    }



    public class Attack //these are queued up in the CombatEngine.
    {
        public Fightable attacker;
        public List<Fightable> targets = new List<Fightable>();
        public string thingToRoll;
        public Ability thingToDo; //Ability here would also mean that I need entries for the baseline commands.
        //Future possible needs:
        //list of special-priority flags so some moves could always go first/last, used for ordering attacks.
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
        public Stats changeStats = new Stats(); //For displaying HP/MP value changes during display phase.
        public string changedItem = ""; //Which property changed. could be sprite.
        public string changedTo = ""; //what it changed to.
        public int frameCounter = 120; //How long to display this particular result, if they're not all on the same timer.

    }

    public class AI
    {
        //mostly a placeholder to remind me i need to work on enemy AI rules.
        //baseilne: just attack. will work until i get abilities and such in place.
        //public 
    }
}
