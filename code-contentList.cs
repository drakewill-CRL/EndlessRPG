using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
    public static class ContentLists
    {
        public static Stats baseStats = new Stats() { HP = 1, maxHP = 1, AP = 1, maxAP = 1, STR = 1, DEF = 1, INS = 1, MOX = 1, SPD = 1, LUK = 1 };

        public static List<string> damageTypes = new List<string>() {"blunt", "pierce", "slash", "fire", "bioHeal", "synthHeal"};

        
        public static List<Ability> allAbilities = new List<Ability>() {
            new Ability() {
                name = "Kickflip",
                description = "You do a sweet kickflip and hurt the enemy or something. Maybe not.",
                apCost = 1,
                level = 1,
                targetType = 0, //automatic targeting
                abilityKey = 0,
                damagetype = "blunt",
                sourceStat = "STR"
            },
            new Ability() {
                name = "Fight",
                description = "Basic attack on the selected target.",
                apCost = 0,
                level = 1,
                targetType = 2, //single enemy
                abilityKey = 1,
                damagetype = "blunt",
                sourceStat = "STR"
            },
            new Ability() {
                name = "Defend",
                description = "Take half damage for the next round",
                apCost = 0,
                level = 1,
                targetType = 0, //auto, self
                abilityKey = 2,
                damagetype = "blunt",
                sourceStat = "STR"

            },
            new Ability() {
                name = "Run", //Maybe 'Reposition'? 
                description = "Attempt to escape this fight and start a new encounter",
                apCost = 0,
                level = 1,
                targetType = 0, //auto, special case.
                abilityKey = 3,
                damagetype = "blunt",
                sourceStat = "STR"
            },
            new Ability() {
                name = "Snapshot", 
                description = "Attack faster than usual, going before most targets.",
                apCost = 2,
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
                apCost = 2,
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
                apCost = 2,
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
                apCost = 3,
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
                startStats = new Stats() {HP = 5, maxHP = 5, AP = 5, maxAP = 5, STR = 5, DEF = 5, INS = 5, MOX = 5, SPD = 5, LUK = 5}, 
                statsPerLevel = new Stats() {HP = 1, maxHP = 1, AP = 1, maxAP = 1, STR = 1, DEF = 1, INS = 1, MOX = 1, SPD = 1, LUK = 1}, 
                abilities = new List<Ability>() //todo: insert abilities here.
            },
            new Role() {
                name = "Soldier",
                spriteSet = "infantry",
                startStats = new Stats() {HP = 5, maxHP = 5, AP = 5, maxAP = 5, STR = 5, DEF = 4, INS = 5, MOX = 4, SPD = 5, LUK = 5}, 
                statsPerLevel = new Stats() {HP = 1, maxHP = 1, AP = 1, maxAP = 1, STR = 1, DEF = 1, INS = 1, MOX = 1, SPD = 1, LUK = 1}, 
                abilities = new List<Ability>() {allAbilities[1].Clone(), allAbilities[4].Clone(), allAbilities[5].Clone(), allAbilities[6].Clone(), allAbilities[7].Clone()}
            }
        };

        public static Dictionary<string, Role> rolesByName = allRoles.ToDictionary(k => k.name, v => v);

        //Rough lazy balance guide:
        //StartingStats are lower, but statsPerLevel are higher, than for PC roles.
        public static List<Enemy> enemies = new List<Enemy>() {
        new Enemy() {
            name = "TestCrab" ,
            desc = "Sample enemy for testing stuff.",
            level = 1,
            startingStats = new Stats() { HP = 5, maxHP = 5, AP = 2, maxAP = 2, STR = 2, DEF = 3, INS = 1, MOX = 1, SPD = 4, LUK = 1},
            StatsPerLevel = new Stats() { HP = 4, maxHP = 4, AP = 1, maxAP = 1, STR = 2, DEF = 2, INS = 1, MOX = 2, SPD = 1, LUK = 2},
            statBoosts = new Stats(),
            spriteSet="crab1",
            abilities = new List<Ability>() {allAbilities[0].Clone(), allAbilities[1].Clone()}
            },
            new Enemy() {
            name = "TITANovacrab" ,
            desc = "A novacrab overrun by TITAN nanites and weakened from its constant exposure to space.",
            level = 1,
            startingStats = new Stats() { HP = 5, maxHP = 5, AP = 2, maxAP = 2, STR = 3, DEF = 3, INS = 1, MOX = 1, SPD = 4, LUK = 1},
            StatsPerLevel = new Stats() { HP = 4, maxHP = 4, AP = 1, maxAP = 1, STR = 3, DEF = 3, INS = 1, MOX = 2, SPD = 2, LUK = 2},
            statBoosts = new Stats(),
            spriteSet="crab1",
            abilities = new List<Ability>() {allAbilities[1].Clone()}
            }
        };

        public static Dictionary<string, Enemy> enemiesByName = enemies.ToDictionary(k => k.name, v => v);


        public static List<List<Enemy>> PossibleEncounters = new List<List<Enemy>>()
        { 
            new List<Enemy>() 
            { 
                (Enemy)enemies[1].Clone(), 
                (Enemy)enemies[1].Clone()
            }
        };
    }
}