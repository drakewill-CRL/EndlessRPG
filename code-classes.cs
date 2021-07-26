using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
    public class Stats{
        public int HP;
        public int maxHP;
        public int MP;
        public int maxMP;
        public int STR;
        public int DEF;
        public int MAGIC;
        public int MDEF;
    }

    public class Ability{
        public string name;
        public string description;
        public int mpCost;
        public int level;

        public void UseAbility() {return;} //must be overridden

    }

    
    public class Role
    {
        public string name;
        public int[] startStats;
        public int[] statsPerLevel;
        public Ability[] abilities;
    }

    public class Fightable
    {
        public string name;
        public int level;
        public string creatureType; //Human, undead, others tbd. for weapon and spell banes?
        public Stats startingStats;
        public Stats StatsPerLevel;
        public int posX;
        public int posY;
        public string spriteSet; //char1, the prefix for characters. Also the file for enemy sprites.
    }


    public class Character : Fightable
    {
        public Role role;
        public int XP; //1 XP at the end of a fight if they're alive, 4XP is a level-up
        
        public string drawState = "Idle"; //holds the state to display on screen.   
        string spriteIdle = "Idle";
        string spriteAbilty = "Ability";
        int abilityFrames =2; //files end in 1 and 2.
        string spriteAttack = "Attack";
        int attackFrames =2; //files end in 1 and 2.
        string spriteHit = "Hit";
        string spriteDead = "Dead";


    }

    public class Enemy : Fightable
    {
        public string sprite;
        public List<Ability> abilities;
    }
}