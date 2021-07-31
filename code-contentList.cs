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

        public static List<string> damageTypes = new List<string>() {"blunt", "pierce", "slash", "fire", "bioHeal", "synthHeal"};

        
        public static List<Ability> allAbilities = new List<Ability>() {
            new Ability() {
                name = "Kickflip",
                description = "You do a sweet kickflip and hurt the enemy or something. Maybe not.",
                mpCost = 1,
                level = 1,
                targetType = 0, //automatic targeting
                abilityKey = 0,
                damagetype = "blunt",
                sourceStat = "STR"
            },
            new Ability() {
                name = "Fight",
                description = "Basic attack on the selected target.",
                mpCost = 0,
                level = 1,
                targetType = 2, //single enemy
                abilityKey = 1,
                damagetype = "blunt",
                sourceStat = "STR"
            },
            new Ability() {
                name = "Defend",
                description = "Take half damage for the next round",
                mpCost = 0,
                level = 1,
                targetType = 0, //auto, self
                abilityKey = 2,
                damagetype = "blunt",
                sourceStat = "STR"

            },
            new Ability() {
                name = "Run", //Maybe 'Reposition'? 
                description = "Attempt to escape this fight and start a new encounter",
                mpCost = 0,
                level = 1,
                targetType = 0, //auto, special case.
                abilityKey = 3,
                damagetype = "blunt",
                sourceStat = "STR"
            },
            new Ability() {
                name = "Snapshot", 
                description = "Attack faster than usual, going before most targets.",
                mpCost = 2,
                level = 1,
                targetType = 2, //one enemy
                specialSpeedLevel = 5, //order by descending.
                abilityKey = 4,
                damagetype = "blunt",
                sourceStat = "STR",
                powerMod = .8F
            },
            new Ability() {
                name = "Aimed Shot", 
                description = "Take your time focusing your attack on a vital point for additional damage.",
                mpCost = 2,
                level = 1,
                targetType = 2, //one enemy
                specialSpeedLevel = 1,
                abilityKey = 5,
                damagetype = "blunt",
                sourceStat = "STR",
                powerMod = 2F
            },
            new Ability() {
                name = "Covering Fire", 
                description = "Spray shots wildly, doing some damage to all enemies.",
                mpCost = 2,
                level = 1,
                targetType = 4, //all enemies.
                abilityKey = 6,
                damagetype = "blunt",
                sourceStat = "STR",
                powerMod = .5F
            },
            new Ability() {
                name = "Tracer Rounds", 
                description = "Switch to incendiary shots, doing fire damage to a target.",
                mpCost = 3,
                level = 1,
                targetType = 2, //one enemy
                abilityKey = 7,
                damagetype = "fire",
                sourceStat = "STR"
            },
        };

        public static Dictionary<string, Ability> abilitiesByName = allAbilities.ToDictionary(k => k.name, v => v);

        public static List<Role> allRoles = new List<Role>() {
            new Role() {
                name = "test role", 
                startStats = new Stats() {HP = 5, maxHP = 5, MP = 5, maxMP = 5, STR = 5, DEF = 5, MAGIC = 5, MDEF = 5, SPD = 5, LUK = 5}, 
                statsPerLevel = new Stats() {HP = 1, maxHP = 1, MP = 1, maxMP = 1, STR = 1, DEF = 1, MAGIC = 1, MDEF = 1, SPD = 1, LUK = 1}, 
                abilities = new List<Ability>() //todo: insert abilities here.
            },
            new Role() {
                name = "Infantry",  //more EP name later.
                startStats = new Stats() {HP = 5, maxHP = 5, MP = 5, maxMP = 5, STR = 5, DEF = 5, MAGIC = 5, MDEF = 5, SPD = 5, LUK = 5}, 
                statsPerLevel = new Stats() {HP = 1, maxHP = 1, MP = 1, maxMP = 1, STR = 1, DEF = 1, MAGIC = 1, MDEF = 1, SPD = 1, LUK = 1}, 
                abilities = new List<Ability>() {allAbilities[1].Clone(), allAbilities[4].Clone(), allAbilities[5].Clone(), allAbilities[6].Clone(), allAbilities[7].Clone()}
            }
        };

        public static Dictionary<string, Role> rolesByName = allRoles.ToDictionary(k => k.name, v => v);

        public static List<Enemy> enemies = new List<Enemy>() {
        new Enemy() {
            name = "Test Target" ,
            desc = "a thing to beat on while making menus work",
            level = 1,
            startingStats = baseStats.Clone(),
            StatsPerLevel = new Stats(),
            currentStats = baseStats.Clone(),
            statBoosts = new Stats(),
            spriteSet="enemy1",
            abilities = new List<Ability>() {allAbilities[0].Clone(), allAbilities[1].Clone()}
            }
        };

        public static Dictionary<string, Enemy> enemiesByName = enemies.ToDictionary(k => k.name, v => v);


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