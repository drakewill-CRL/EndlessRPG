using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
    public static class gameState
    {

        public static Random random = new Random();
        //Some of this might get moved to individual game files.
        public static int mode = 0; // which scene we're in right now.
        //0 = title screen
        //1 = newGame screen
        //2 = fight screen
        //3 = improve screen

        public static bool gameActive = false; //Is there a game in progress to continue from?
        public static int fightsWon = 0; //Current fights survived,
        public static int bestFightsWon = 0; //high score for a whole party.

        //This part would need copypasted per Role.
        public static bool BestRoleUnlocked = false;
        public static int BestRoleLevel = 0;
        

        //Current game values. Copy this section once per charcter.\
        public static Character char1; //Or just do this instead of tracking it multiple times?
        public static string Char1Name  = "";
        public static int Char1Level = 0;
        public static int Char1Role =0; //index of the role in the role list.
        //Points spent leveling abilities
        public static int Char1Ability1Level = 0;
        public static int Char1Ability2Level = 0;
        public static int Char1Ability3Level = 0;
        public static int Char1Ability4Level = 0;
        //Points spent boosting stats. (these get added to the base stat value)
        public static int Char1BonusMaxHp = 0;
        public static int Char1BonusMaxMp = 0;
        public static int Char1BonusSTR = 0;
        public static int Char1BonusDEF = 0;
        public static int Char1BonusMAGIC = 0;
        public static int Char1BonusMDEF = 0;

    }
}