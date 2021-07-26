using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
    public static class ContentLists
    {
        public static Stats baseStats = new Stats() { HP = 1, maxHP = 1, MP = 1, maxMP = 1, STR = 1, DEF = 1, MAGIC = 1, MDEF = 1, SPD = 1, LUK = 1 };

        public static List<Enemy> enemies = new List<Enemy>() {
        new Enemy() {
            name = "Test" ,
            level = 1,
            startingStats = baseStats,
            StatsPerLevel = baseStats
            }
        };

        public static List<Role> allRoles = new List<Role>() {
            new Role() {
                name = "test role", 
                startStats = new Stats() {HP = 5, maxHP = 5, MP = 5, maxMP = 5, STR = 5, DEF = 5, MAGIC = 5, MDEF = 5, SPD = 5, LUK = 5}, 
                statsPerLevel = new Stats() {HP = 1, maxHP = 1, MP = 1, maxMP = 1, STR = 1, DEF = 1, MAGIC = 1, MDEF = 1, SPD = 1, LUK = 1}, 
                abilities = new List<Ability>() //todo: insert abilities here.
            }

        };
    }
}