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
        public string spriteSet; //char1, the prefix for characters. Also the file for enemy sprites.
        public string drawState = ""; //holds the state to display on screen.   blank is 'idle'
        public int colorShift = 0; //for cycling palettes in DisplayResults. TODO: use this in draw commands.
        public List<Ability> abilities;
        public string desc = "";

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
            return copy;
        }
    }


    public class Character : Fightable
    {
        public Role role;
        public int XP; //1 XP at the end of a fight if they're alive, 4XP is a level-up        
        string weaponName = ""; //Fluff, used for item rolls to name weapons.
        string armorName = ""; //fluff, used for item rolls to name defensive items.

        public Character(Role role)
        {
            //TODO: fill in all the default info from the supplied role.
            return;
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
            foreach(var a in abilities)
                copy.abilities.Add(a.Clone());

            return copy;
        }

        public void LevelUp() //TODO: move to Fightable instead of Character?
        {
            currentStats.Add(StatsPerLevel);
            currentStats.HP = currentStats.maxHP;
            currentStats.AP = currentStats.maxAP;
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
            foreach(var a in abilities)
                copy.abilities.Add(a.Clone());

            return copy;
        }

    }
}
