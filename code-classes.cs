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

        public static void UseAbility(Fightable attacker, List<Fightable> targets, int key)
        {
            //All abilities' effects will just get handled in this one giant switch function for now.
            //until i cna figure out how to in-line a function declaration in an initializer.
            switch(key)
            {
                case 0: //Do a kickflip!
                break;
            }
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

    public class Fightable
    {
        public string name;
        public int level;
        public string creatureType; //Human, undead, others tbd. for weapon and spell banes?
        public Stats startingStats;
        public Stats StatsPerLevel;
        public Stats currentStats; //should be equal to getTotalStats most of the time.
        public Stats statBoosts; //boosted stats from XP instead of a weapon or level.
        public Stats tempChanges; //Effects from debuffs or situations etc.
        public int posX;
        public int posY;
        public string spriteSet; //char1, the prefix for characters. Also the file for enemy sprites.
        public List<Ability> abilities;


        Item weapon;
        Item armor;

        public Stats getTotalStats() //TODO: do i want this to handle HP or keep that separate? I have to be care on where I set HP/MP if I do it here.
        {
            Stats final = new Stats();
            final.Add(startingStats);
            for (int i = 0; i < level; i++)
                final.Add(StatsPerLevel);
            final.Add(statBoosts);
            final.Add(weapon.statBoost);
            final.Add(armor.statBoost);
            final.Add(tempChanges);

            return final;
        }

        public bool CanAct()
        {
            if (currentStats.HP > 0) //could have status effects here that also block acting later
                return true;

            return false;
        }
    }


    public class Character : Fightable
    {
        public Role role;
        public int XP; //1 XP at the end of a fight if they're alive, 4XP is a level-up        
        public string drawState = "Idle"; //holds the state to display on screen.   
        string spriteIdle = "Idle";
        string spriteAbilty = "Ability";
        int abilityFrames = 2; //files end in 1 and 2.
        string spriteAttack = "Attack";
        int attackFrames = 2; //files end in 1 and 2.
        string spriteHit = "Hit";
        string spriteDead = "Dead";

        string weaponName = ""; //Fluff, used for item rolls to name weapons.
        string armorName = ""; //fluff, used for item rolls to name defensive items.



    }

    public class Enemy : Fightable
    {
        public string sprite;
        
    }

    public class Attack //these are queued up in the CombatEngine.
    {
        public Fightable attacker;
        public List<Fightable> targets;
        public string thingToRoll;
        public Ability thingToDo; //Ability here would also mean that I need entries for the baseline commands.
    }

    public class Item
    {
        public Stats statBoost;
        public string name;
    }

    public class AttackResults
    {
        //What happens after an attack.
        public Fightable attacker;
        public List<Fightable> target;
        public Stats attackerChanges;
        public List<Stats> targetChanges; //Must line up index-wise with the targets.
        public List<string> printDesc; //Multiple targets COULD mean multiple results to display. Might not.

    }

    public class AI
    {
        //mostly a placeholder to remind me i need to work on enemy AI rules.
        //baseilne: just attack. will work until i get abilities and such in place.
    }
}
