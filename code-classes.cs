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
    }


    public class Character : Fightable
    {
        public Role role;
        public string spriteSet;

    }

    public class Enemy : Fightable
    {
        public string sprite;
        public List<Ability> abilities;
    }
}