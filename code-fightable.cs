using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
    public class Fightable //enemies and player characters, PCs get some extra stuff.
    {
        public string name;
        public int level = 1;
        public string morphType; //Bio, Synth, Pod. Possibly others for enemies
        public Stats startingStats = new Stats();
        public Stats StatsPerLevel = new Stats();
        public Stats currentStats = new Stats(); //should be equal to getTotalStats most of the time.
        public Stats statBoosts = new Stats(); //boosted stats from XP instead of a weapon or level.
        public Stats tempChanges = new Stats(); //Effects from debuffs or situations etc.
        public Stats displayStats = new Stats(); //The stats to use on-screen and process during fight results animation, so HP/AP bars show correct values during playback.
        public int posX;
        public int posY;
        public string spriteSet = ""; //char1, the prefix for characters. Also the file for enemy sprites.
        public string drawState = ""; //holds the state to display on screen.   blank is 'idle'
        public int colorShift = 0; //for cycling palettes in DisplayResults. TODO: use this in draw commands.
        public List<Ability> abilities;
        public string desc = "";
        public bool isPlayer = false; //For being lazy on checking if this thing is a players character or not.

        Item weapon = new Item();
        Item armor = new Item();

        public Dictionary<string, double> damageMultipliers = new Dictionary<string, double>(); //damage type is the key, damage gets multiplied by the double.

        public Stats getTotalStats(bool refill = false) //TODO: do i want this to handle HP or keep that separate? I have to be care on where I set HP/AP if I do it here.
        {
            Stats final = new Stats();
            final.Add(startingStats);
            for (int i = 1; i < level; i++) //Level 1 doesn't grant stats, it gives startingStats instead.
                final.Add(StatsPerLevel);
            final.Add(statBoosts);
            final.Add(weapon.statBoost);
            final.Add(armor.statBoost);
            final.Add(tempChanges);

            if (refill || currentStats == null)
            {
                final.HP = final.maxHP;
                final.AP = final.maxAP;
            }
            else
            {
                final.HP = currentStats.HP;
                final.AP = currentStats.AP;
            }
            return final;
        }

        public bool CanAct()
        {
            if (currentStats.HP > 0) //could have status effects here that also block acting later
                return true;

            return false;
        }

        public Fightable Clone()
        {
            Fightable copy = (Fightable)this.MemberwiseClone();
            copy.startingStats = copy.startingStats.Clone();
            copy.StatsPerLevel = copy.StatsPerLevel.Clone();
            copy.currentStats = copy.getTotalStats(); //make sure currentStats is a separate object
            copy.displayStats = copy.currentStats.Clone();
            //TODO: damage multipliers?
            return copy;
        }
    }


    public class Character : Fightable
    {
        public Role role;
        public int XP = 0; //1 XP at the end of a fight if they're alive, 4XP is a level-up        
        string weaponName = ""; //Fluff, used for item rolls to name weapons.
        string armorName = ""; //fluff, used for item rolls to name defensive items.

        public Character(Role r, int lvl)
        {
            level = lvl;
            role = r;

            switch (r.morphType)
            {
                case "bio":
                    damageMultipliers.Add("bioHeal", 1);
                    damageMultipliers.Add("synthHeal", 0.01);
                    break;
                case "pod":
                    damageMultipliers.Add("bioHeal", 0.5);
                    damageMultipliers.Add("synthHeal", 0.5);
                    break;
                case "synth":
                    damageMultipliers.Add("bioHeal", 0.01);
                    damageMultipliers.Add("synthHeal", 1);
                    break;
            }

            //If I create any roles that get specific damage multipliers, they'll get processed here.
            foreach (var dm in r.damageMultipliers)
                damageMultipliers.Add(dm.Key, dm.Value);

            //these get copied to make sure the functions from the parent class work.
            morphType = r.morphType;
            startingStats = r.startStats.Clone();
            StatsPerLevel = r.statsPerLevel.Clone();
            currentStats = getTotalStats(true);
            displayStats = currentStats.Clone();
            abilities = r.abilities; //.Clone(); Might be unnecessary until abilities can be leveled up.
            isPlayer = true;
        }

        //TODO: make all calls use the main constructor above and drop this one out of code.
        public Character(Role r) : this(r, 1)
        {
        }

        public string GetSaveData()
        {
            string results = "";
            results += name + "-" +
            role.name + "-" +
            level.ToString() + "-" +
            statBoosts.GetAsSaveData() + "-" +
            currentStats.GetAsSaveData() + "-" +
            XP;
            return results;
        }

        public Character(string savedData)
        {
            //Format:
            //name-roleName-level-statBoosts-currentStats-XP
            //Gotta save currentStats or else we don't track HP/AP used.
            var splitData = savedData.Split("-");
            name = splitData[0];
            role = ContentLists.rolesByName[splitData[1]];
            level = Int32.Parse(splitData[2]);
            //NOTE: doing all the work in the normal constructor here again to make sure I dont refill player HP every save.
            statBoosts = new Stats();
            statBoosts.LoadFromSaveData(splitData[3]);
            currentStats = new Stats();
            currentStats.LoadFromSaveData(splitData[4]);
            XP = Int32.Parse(splitData[5]);

            //The rest of this is boilerplate:
            switch (role.morphType)
            {
                case "bio":
                    damageMultipliers.Add("bioHeal", 1);
                    damageMultipliers.Add("synthHeal", 0.01);
                    break;
                case "pod":
                    damageMultipliers.Add("bioHeal", 0.5);
                    damageMultipliers.Add("synthHeal", 0.5);
                    break;
                case "synth":
                    damageMultipliers.Add("bioHeal", 0.01);
                    damageMultipliers.Add("synthHeal", 1);
                    break;
            }

            //If I create any roles that get specific damage multipliers, they'll get processed here.
            foreach (var dm in role.damageMultipliers)
                damageMultipliers.Add(dm.Key, dm.Value);

            //these get copied to make sure the functions from the parent class work.
            morphType = role.morphType;
            startingStats = role.startStats.Clone();
            StatsPerLevel = role.statsPerLevel.Clone();
            displayStats = currentStats.Clone();
            abilities = role.abilities; //.Clone(); Might be unnecessary until abilities can be leveled up.
            isPlayer = true;
        }

        public Character Clone()
        {
            Character copy = (Character)this.MemberwiseClone();
            copy.startingStats = copy.startingStats.Clone();
            copy.StatsPerLevel = copy.StatsPerLevel.Clone();
            copy.statBoosts = copy.statBoosts.Clone();
            copy.currentStats = copy.getTotalStats();
            copy.displayStats = copy.currentStats.Clone();
            copy.abilities = new List<Ability>();
            foreach (var a in abilities)
                copy.abilities.Add(a.Clone());

            return copy;
        }
    }

    public class Enemy : Fightable
    {
        //enemies don't have any unique properties?
        //public string sprite; 

        public Enemy Clone()
        {
            Enemy copy = (Enemy)this.MemberwiseClone();
            copy.startingStats = copy.startingStats.Clone();
            copy.StatsPerLevel = copy.StatsPerLevel.Clone();
            copy.statBoosts = copy.statBoosts.Clone();
            copy.currentStats = copy.getTotalStats();
            copy.displayStats = copy.currentStats.Clone();
            copy.abilities = new List<Ability>();
            foreach (var a in abilities)
                copy.abilities.Add(a.Clone());

            return copy;
        }

        public string GetSaveData()
        {
            string results = "";
            results += name + "-" +
            level.ToString() + "-" +
            statBoosts.GetAsSaveData() + "-" +
            currentStats.GetAsSaveData();

            return results;
        }


        public void FromSaveData(string savedData)
        {
            //NOTE: clone the enemy first, THEN call this function to set their stats.
            //Format:
            //name-level-statBoosts-currentStats
            //Gotta save currentStats or else we don't track HP/AP used.

            var splitData = savedData.Split("-");
            name = splitData[0];
            level = Int32.Parse(splitData[1]);

            //NOTE: doing all the work in the normal constructor here again to make sure I dont refill player HP every save.
            statBoosts = new Stats();
            statBoosts.LoadFromSaveData(splitData[2]);
            currentStats = new Stats();
            currentStats.LoadFromSaveData(splitData[3]);
        }

    }
}
