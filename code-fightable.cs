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
        public int level;
        public string creatureType; //Human, undead, others tbd. for weapon and spell banes?
        public Stats startingStats = new Stats();
        public Stats StatsPerLevel = new Stats();
        public Stats currentStats = new Stats(); //should be equal to getTotalStats most of the time.
        public Stats statBoosts = new Stats(); //boosted stats from XP instead of a weapon or level.
        public Stats tempChanges = new Stats(); //Effects from debuffs or situations etc.
        public int posX;
        public int posY;
        public string spriteSet; //char1, the prefix for characters. Also the file for enemy sprites.
        public List<Ability> abilities;
        public string desc = "";

        Item weapon = new Item();
        Item armor = new Item();

        public Stats getTotalStats() //TODO: do i want this to handle HP or keep that separate? I have to be care on where I set HP/MP if I do it here.
        {
            Stats final = new Stats();
            final.Add(startingStats);
            for (int i = 1; i < level; i++) //Level 1 doesn't grant stats, it gives startingStats instead.
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

        public Fightable Clone()
        {
            Fightable copy = (Fightable)this.MemberwiseClone(); 
            copy.currentStats = copy.getTotalStats(); //make sure currentStats is a separate object
            return copy;
            //TODO: do any of the lists need copied or changed?
            //currentStats probably does.
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
        //enemies don't have any unique properties?
        //public string sprite; 

    }
}
