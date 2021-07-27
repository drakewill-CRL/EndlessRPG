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
            maxMP += incoming.maxMP;
            MP += incoming.MP; //attacker can burn MP after the attack.
            STR += incoming.STR;
            DEF += incoming.DEF;
            MAGIC += incoming.MAGIC;
            MDEF += incoming.MDEF;
            SPD += incoming.SPD;
            LUK += incoming.LUK;
        }
    }

    public class Ability
    {
        public string name;
        public string description; //help text that appears in combat. Might need to be a property to update with levels.
        public int mpCost;
        public int level; //Used to power up the effects of the ability.
        public int targetType; //0: no target selection(self, random, all enemies, or all allies.). 2: target  1 enemy 3: target 1 ally.

        //public void UseAbility() { return; } //must be overridden? Might not be what I think
        public int abilityKey;

        public static AttackResults UseAbility(Fightable attacker, List<Fightable> targets, int key)
        {
            //All abilities' effects will just get handled in this one giant switch function for now.
            //until i cna figure out how to in-line a function declaration in an initializer.
            AttackResults results = new AttackResults();
            results.attacker = attacker;
            foreach (var t in targets)
            {
                Stats ts = new Stats();
                results.targetChanges.Add(ts);
                //results.printDesc.Add("attack results");
                switch (key)
                {
                    case 0: //Do a kickflip!
                        results.printDesc.Add(attacker.name + " does a sweet kickflip! Nothing else happens.");
                        break;
                    case 1: //Fight
                        results.printDesc.Add(attacker.name + " bonks " + targets[0].name + " for 1 / REAL DAMAGE NOT IMPLEMENTED");
                        results.target.Add(t);
                        ts.HP = -1;
                        // results.targetChanges.Add(new Stats(){ HP = -1});
                        // Console.WriteLine("removing " + results.targetChanges[0].HP + " HP");
                        // Console.WriteLine("targetchanges is " + results.targetChanges.Count() + "  long");

                        break;
                    case 2: //defend
                        results.printDesc.Add(attacker.name + " defends, defend not implemented");
                        break;
                    case 3: //Run
                        results.printDesc.Add(attacker.name + " runs, run not implemented");
                        break;
                }
            }

            return results;
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
    }

    public class Item
    {
        public Stats statBoost = new Stats();
        public string name;
    }

    public class AttackResults
    {
        //What happens after an attack.
        public Fightable attacker;
        public List<Fightable> target = new List<Fightable>();
        //public Stats attackerChanges; //Just put the attack in the target and targetChanges list.
        public List<Stats> targetChanges = new List<Stats>(); //Must line up index-wise with the targets.
        public List<string> printDesc = new List<string>(); //Multiple targets COULD mean multiple results to display. Might not.

    }

    public class AI
    {
        //mostly a placeholder to remind me i need to work on enemy AI rules.
        //baseilne: just attack. will work until i get abilities and such in place.
    }
}
