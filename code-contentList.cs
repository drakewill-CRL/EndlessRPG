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
            name = "Test Target" ,
            desc = "a thing to beat on while making menus work",
            level = 1,
            startingStats = baseStats,
            StatsPerLevel = baseStats,
            currentStats = baseStats,
            spriteSet="enemy1",
            abilities = new List<Ability>() {}
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

        public static List<Ability> allAbilities = new List<Ability>() {
            new Ability() {
                name = "Kickflip",
                description = "You do a sweet kickflip and hurt the enemy or something. Maybe not.",
                mpCost = 1,
                level = 1,
                targetType = 0, //automatic targeting
                abilityKey = 0
            },
            new Ability() {
                name = "Fight",
                description = "Melee attack the selected target.",
                mpCost = 0,
                level = 1,
                targetType = 2, //single enemy
                abilityKey = 1
            },
            new Ability() {
                name = "Defend",
                description = "Take half damage for the next round",
                mpCost = 0,
                level = 1,
                targetType = 0, //auto, self
                abilityKey = 2
            },
            new Ability() {
                name = "Run",
                description = "Attempt to escape this fight and start a new encounter",
                mpCost = 0,
                level = 1,
                targetType = 0, //auto, special case.
                abilityKey = 3
            },
        };

        public static List<List<Enemy>> PossibleEncounters = new List<List<Enemy>>()
        { 
            new List<Enemy>() 
            { 
                (Enemy)enemies[0].Clone(), 
                (Enemy)enemies[0].Clone()
            }
        };
    }
}