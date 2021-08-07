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

        //Kinetic: physical contact attack. Sword, bullets, etc.
        //energy: lasers, fire, etc.
        //shock: electrical (energy) attack plus a stun. Biomorphs have a chance to resist the stun. Synthmorphs are usually immune to the damage and the stun.
        //bioHeal: heals bio for full, pod for half, synth for none
        //synthHeal: heals synth for full, pod for half, bio for none.
        //bioTox: biomorph-only damage type.

        public static List<string> damageTypes = new List<string>() { "kinetic", "energy",  "shock", "bioHeal", "synthHeal", "bioTox", };

        public static List<Ability> allAbilities = new List<Ability>() {
            //test/sample/planning abilities
            new Ability() {
                name = "Kickflip",
                description = "You do a sweet kickflip and hurt the enemy or something. Maybe not.",
                apCost = 1,
                level = 1,
                targetType = 0, //automatic targeting
                abilityKey = 0,
                damagetype = "kinetic",
                sourceStat = "STR"
            },
            new Ability() {
                name = "Fight",
                description = "Basic attack on the selected target.",
                apCost = 0,
                level = 1,
                targetType = 2, //single enemy
                abilityKey = 1,
                damagetype = "kinetic",
                sourceStat = "STR"
            },
            new Ability() {
                name = "Defend",
                description = "Take half damage for the next round",
                apCost = 0,
                level = 1,
                targetType = 0, //auto, self
                abilityKey = 2,
                damagetype = "kinetic",
                sourceStat = "STR"
            },
            new Ability() {
                name = "Run", //Maybe 'Reposition'? 
                description = "Attempt to escape this fight and start a new encounter",
                apCost = 0,
                level = 1,
                targetType = 0, //auto, special case.
                abilityKey = 3,
                damagetype = "kinetic",
                sourceStat = "STR"
            },
            //Infantry abilities
            new Ability() {
                name = "Snapshot",
                description = "Attack faster than usual, going before most targets.",
                apCost = 2,
                level = 1,
                targetType = 2, //one enemy
                specialSpeedLevel = 5, //order by descending.
                abilityKey = 4,
                damagetype = "kinetic",
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
                damagetype = "kinetic",
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
                damagetype = "kinetic",
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
                damagetype = "energy",
                sourceStat = "STR"
            },
            //Medic abilities
            new Ability() {
                name = "First Aid",
                description = "Quickly apply some painkilers and coagulents to a biomorph, restoring HP.",
                apCost = 3,
                level = 1,
                targetType = 3, //one ally
                abilityKey = 8,
                damagetype = "bioHeal",
                sourceStat = "INS"
            },
            new Ability() {
                name = "Adrenal Mist",
                description = "Cover the team in a mist of drugs, restoring a little HP to all.",
                apCost = 4,
                powerMod = .3,
                level = 1,
                targetType = 5, //all allies
                abilityKey = 9,
                damagetype = "bioHeal",
                sourceStat = "INS"
            },
            new Ability() {
                name = "Defib",
                description = "Restats a stopped heart, bringing a dead biomorph back up with 1 HP.",
                apCost = 6,
                level = 1,
                targetType = 3, //one ally
                abilityKey = 10,
                damagetype = "bioHeal",
                sourceStat = "INS"
            },
            new Ability() {
                name = "Neurotoxin",
                description = "Injects a target with neurotoxin, damaging biomorphs severely but ineffective on synthmorphs",
                apCost = 3,
                level = 1,
                targetType = 2, //one enemy
                abilityKey = 11,
                damagetype = "bioTox",
                sourceStat = "INS"
            },
            //Techie abilities
            new Ability() {
                name = "Spot Weld",
                description = "Fast repairs on a synthmorph, restoring some HP.",
                apCost = 3,
                level = 1,
                targetType = 3, //one ally
                abilityKey = 12,
                damagetype = "synthHeal",
                sourceStat = "INS"
            },
            new Ability() { 
                name = "Corrosive Juice",
                description = "Metal-eating chemical attack, does heavy damage to synthmorphs. Less effective on biomorphs.",
                apCost = 3,
                level = 1,
                targetType = 2, //one enemy
                abilityKey = 13,
                damagetype = "energy", //close enough for now
                sourceStat = "INS"
            },
            new Ability() {
                name = "Tank Tackle",
                description = "Puts all the synthmorph mass into an extra large charge attack.",
                apCost = 3,
                level = 1,
                targetType = 2, //one enemy
                abilityKey = 14,
                damagetype = "kinetic",
                sourceStat = "STR"
            },
            new Ability() {
                name = "Power Cycle",
                description = "Take the time to restart your morph in a combat-focused mode for the rest of this encounter.",
                apCost = 3,
                level = 1,
                targetType = 0, //self buff. Requires setting up temporary buffs somehow.
                abilityKey = 15,
                specialSpeedLevel = 1, //goes last
                damagetype = "synthHeal",
                sourceStat = "INS"
            },
            //CovertOp abilities
            new Ability() {
                name = "Ambush",
                description = "Stab an enemy before they can react.",
                apCost = 1,
                level = 1,
                specialSpeedLevel = 5, //goes first
                targetType = 2, //one enemy
                abilityKey = 16,
                damagetype = "kinetic",
                sourceStat = "STR"
            },
            new Ability() {
                name = "C4 Charge", //TODO: is this a legit explosive type in EP?
                description = "Detonate a pocketed explosive in the middle of the enemy group",
                apCost = 5,
                level = 1,
                targetType = 4, //all enemies
                abilityKey = 17,
                damagetype = "energy",
                sourceStat = "INS"
            },
            new Ability() {
                name = "Quickhack",
                description = "Run several standardized exploits against a synthmorph target, disabling and damaging it",
                apCost = 2,
                level = 1,
                specialSpeedLevel = 4, //faster than average
                targetType = 2, //one enemy
                abilityKey = 18,
                damagetype = "energy", //TODO: determine correct synth-only damage type. was EMP but thats wrong.
                sourceStat = "INS"
            },
            new Ability() {
                name = "Combat Stims",
                description = "Injects some speed-enhancing drugs into the targeted ally.",
                apCost = 2,
                level = 1,
                targetType = 3, //all allies
                abilityKey = 19,
                damagetype = "",
                sourceStat = "INS"
            },
            //Enemy abilities
            //TheCheat abilities
        };

        public static Dictionary<string, Ability> abilitiesByName = allAbilities.ToDictionary(k => k.name, v => v);

        //early lazy plan for stats per level:
        //1: bad stat 2: average  3: good stat. HP probably needs to scale higher than most stats.
        public static List<Role> allRoles = new List<Role>() {
            new Role() {
                name = "test role",
                startStats = new Stats() {HP = 5, maxHP = 5, AP = 5, maxAP = 5, STR = 5, DEF = 5, INS = 5, MOX = 5, SPD = 5, LUK = 5},
                statsPerLevel = new Stats() {HP = 1, maxHP = 1, AP = 1, maxAP = 1, STR = 3, DEF = 2, INS = 1, MOX = 1, SPD = 1, LUK = 1},
                abilities = new List<Ability>() //todo: insert abilities here.
            },
            new Role() {
                name = "Soldier", //soldiers are all-offense on abilities, rounded stats.
                spriteSet = "infantry",
                morphType = "bio",
                desc = "An offensive generalist without a standout skill.",
                startStats = new Stats() {HP = 5, maxHP = 5, AP = 5, maxAP = 5, STR = 5, DEF = 4, INS = 5, MOX = 4, SPD = 5, LUK = 5},
                statsPerLevel = new Stats() {HP = 3, maxHP = 3, AP = 1, maxAP = 1, STR = 2, DEF = 2, INS = 1, MOX = 1, SPD = 2, LUK = 1},
                abilities = new List<Ability>() {allAbilities[1].Clone(), allAbilities[4].Clone(), allAbilities[5].Clone(), allAbilities[6].Clone(), allAbilities[7].Clone()}
            },
            new Role() { //Medics heal biomorphs and debuff bio enemies
                name = "Medic",
                spriteSet = "medic",
                morphType ="bio",
                desc = "Biomorph healer and pharaceutical expert",
                startStats = new Stats() {HP = 5, maxHP = 5, AP = 5, maxAP = 5, STR = 5, DEF = 4, INS = 5, MOX = 4, SPD = 4, LUK = 6},
                statsPerLevel = new Stats() {HP = 2, maxHP = 2, AP = 3, maxAP = 3, STR = 1, DEF = 2, INS = 3, MOX = 3, SPD = 2, LUK = 2},
                abilities = new List<Ability>() {abilitiesByName["Fight"].Clone(), abilitiesByName["First Aid"].Clone(), abilitiesByName["Adrenal Mist"].Clone(), abilitiesByName["Defib"].Clone(), abilitiesByName["Neurotoxin"].Clone()}
            },
            new Role() {  //techies do extar damage to synthmorphs and heal synth allies
                name = "Techie",
                spriteSet = "techie",
                morphType = "synth",
                desc = "Synthmorph repairs and robot combat specialist",
                startStats = new Stats() {HP = 9, maxHP = 9, AP = 5, maxAP = 5, STR = 5, DEF = 7, INS = 5, MOX = 4, SPD = 5, LUK = 4},
                statsPerLevel = new Stats() {HP = 3, maxHP = 3, AP = 1, maxAP = 1, STR = 2, DEF = 3, INS = 1, MOX = 2, SPD = 2, LUK = 1},
                abilities = new List<Ability>() {abilitiesByName["Fight"].Clone(), abilitiesByName["Spot Weld"].Clone(), abilitiesByName["Corrosive Juice"].Clone(), abilitiesByName["Tank Tackle"].Clone(), abilitiesByName["Power Cycle"].Clone()}
            },
            new Role() { //Covert Ops run on speed and luck and maybe lots of AP. Disposable pod body inside some armor
                name = "CovertOp",
                spriteSet = "covertop",
                morphType = "pod",
                desc = "Fast and precise, a disposable pod morph for deniabilty",
                startStats = new Stats() {HP = 4, maxHP = 4, AP = 7, maxAP = 7, STR = 4, DEF = 4, INS = 4, MOX = 3, SPD = 8, LUK = 7},
                statsPerLevel = new Stats() {HP = 2, maxHP = 2, AP = 2, maxAP = 2, STR = 2, DEF = 1, INS = 2, MOX = 1, SPD = 3, LUK = 3},
                abilities = new List<Ability>() {abilitiesByName["Fight"].Clone(), abilitiesByName["Ambush"].Clone(), abilitiesByName["C4 Charge"].Clone(), abilitiesByName["Quickhack"].Clone(), abilitiesByName["Combat Stims"].Clone()}
            },
            new Role() { //for those that want to edit their save file, I salute you. Eventually, at least. This is farther-future work.
                name = "TheCheat",
                spriteSet = "cheat",
                morphType = "bio",
                startStats = new Stats() {HP = 5, maxHP = 5, AP = 5, maxAP = 5, STR = 5, DEF = 4, INS = 5, MOX = 4, SPD = 5, LUK = 5},
                statsPerLevel = new Stats() {HP = 1, maxHP = 1, AP = 1, maxAP = 1, STR = 1, DEF = 1, INS = 1, MOX = 1, SPD = 1, LUK = 1},
                abilities = new List<Ability>() {allAbilities[1].Clone(), allAbilities[4].Clone(), allAbilities[5].Clone(), allAbilities[6].Clone(), allAbilities[7].Clone()}
            }
        };

        public static Dictionary<string, Role> rolesByName = allRoles.ToDictionary(k => k.name, v => v);

        //Rough lazy balance guide:
        //StartingStats are lower, but statsPerLevel are higher, than for PC roles.
        //If PC roles scales up from 1-3 in each stat, enemies probably get 0-2 per level.
        public static List<Enemy> enemies = new List<Enemy>() {
        new Enemy() {
            name = "TestCrab" ,
            desc = "Sample enemy for testing stuff.",
            level = 1,
            startingStats = new Stats() { HP = 5, maxHP = 5, AP = 2, maxAP = 2, STR = 2, DEF = 3, INS = 1, MOX = 1, SPD = 4, LUK = 1},
            StatsPerLevel = new Stats() { HP = 4, maxHP = 4, AP = 1, maxAP = 1, STR = 2, DEF = 2, INS = 1, MOX = 2, SPD = 1, LUK = 2},
            statBoosts = new Stats(),
            spriteSet="crab1",
            morphType = "bio",
            abilities = new List<Ability>() {allAbilities[0].Clone(), allAbilities[1].Clone()}
            },
            new Enemy() {
            name = "TITANovacrab" ,
            desc = "A novacrab overrun by TITAN nanites and weakened from its constant exposure to space.",
            level = 1,
            morphType= "pod",
            startingStats = new Stats() { HP = 5, maxHP = 5, AP = 2, maxAP = 2, STR = 3, DEF = 3, INS = 1, MOX = 1, SPD = 4, LUK = 1},
            StatsPerLevel = new Stats() { HP = 4, maxHP = 4, AP = 1, maxAP = 1, STR = 3, DEF = 3, INS = 1, MOX = 2, SPD = 2, LUK = 2},
            statBoosts = new Stats(),
            spriteSet="crab1",
            abilities = new List<Ability>() {abilitiesByName["Fight"].Clone()}
            },
            new Enemy() {
            name = "Lost Soldier" ,
            desc = "Combat trained personnel lost to TITAN influence and corruption.",
            level = 1,
            startingStats = new Stats() { HP = 5, maxHP = 5, AP = 2, maxAP = 2, STR = 3, DEF = 3, INS = 1, MOX = 1, SPD = 4, LUK = 1},
            StatsPerLevel = new Stats() { HP = 4, maxHP = 4, AP = 1, maxAP = 1, STR = 3, DEF = 3, INS = 1, MOX = 2, SPD = 2, LUK = 2},
            statBoosts = new Stats(),
            morphType="bio",
            spriteSet="crab1",
            abilities = new List<Ability>() {abilitiesByName["Fight"].Clone()}
            },
            new Enemy() {
            name = "Swarmoid" ,
            desc = "A large collection of tiny drones operating in unison.",
            level = 1,
            startingStats = new Stats() { HP = 5, maxHP = 5, AP = 2, maxAP = 2, STR = 3, DEF = 3, INS = 1, MOX = 1, SPD = 4, LUK = 1},
            StatsPerLevel = new Stats() { HP = 4, maxHP = 4, AP = 1, maxAP = 1, STR = 3, DEF = 3, INS = 1, MOX = 2, SPD = 2, LUK = 2},
            statBoosts = new Stats(),
            spriteSet="crab1",
            morphType="synth",
            abilities = new List<Ability>() {abilitiesByName["Fight"].Clone()}
            },
            new Enemy() {
            name = "Slitheroid" ,
            desc = "A synthmorph built for all-terrain traversal and durability.",
            level = 1,
            startingStats = new Stats() { HP = 5, maxHP = 5, AP = 2, maxAP = 2, STR = 3, DEF = 3, INS = 1, MOX = 1, SPD = 4, LUK = 1},
            StatsPerLevel = new Stats() { HP = 4, maxHP = 4, AP = 1, maxAP = 1, STR = 3, DEF = 3, INS = 1, MOX = 2, SPD = 2, LUK = 2},
            statBoosts = new Stats(),
            spriteSet="crab1",
            morphType="synth",
            abilities = new List<Ability>() {abilitiesByName["Fight"].Clone()}
            },
            new Enemy() {
            name = "Biomonster" ,
            desc = "Something derived from one or more biomorphs. Not entirely sure what it is now.",
            level = 1,
            startingStats = new Stats() { HP = 5, maxHP = 5, AP = 2, maxAP = 2, STR = 3, DEF = 3, INS = 1, MOX = 1, SPD = 4, LUK = 1},
            StatsPerLevel = new Stats() { HP = 4, maxHP = 4, AP = 1, maxAP = 1, STR = 3, DEF = 3, INS = 1, MOX = 2, SPD = 2, LUK = 2},
            statBoosts = new Stats(),
            spriteSet="crab1",
            morphType="bio",
            abilities = new List<Ability>() {abilitiesByName["Fight"].Clone()}
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

        public static List<List<Enemy>> PossibleBossEncounters = new List<List<Enemy>>()
        {
            new List<Enemy>()
            {
                //TODO: pick out a boss fight, insert here.
                (Enemy)enemies[1].Clone(),
                (Enemy)enemies[1].Clone()
            }
        };
    }
}