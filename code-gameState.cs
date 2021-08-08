using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
    public static class gameState
    {
        public static Point display =new Point(0,0);
        //public static int screenWidth = 344;

        public static Random random = new Random();
        //Some of this might get moved to individual game files.
        public static  TimeSpan timePlayed = new TimeSpan(0,0,0);
        public static int fightsWon = 0;
        public static int totalBestLevels = 0;
        public static Dictionary<string, int> bestLevels = new Dictionary<string, int>();
        public static int mode = TitleSceneID; // which scene we're in right now. Numbered by order they were developed in.
        //0 = title screen
        //1 = fight screen
        //2 = improve screen
        //3 = newGame screen

        /* 
        sound IDs
        0: arrow-move and menu-advance
        1: quick gun attack
        2: menu-negative (action not allowed)
        3: PC is hit (1 is the PC attacking.)

        */
        
        public const int TitleSceneID = 0;
        public const int NewGameSceneID = 1;
        public const int FightSceneID = 2;
        public const int ImproveSceneID = 3;
        public const int TestSceneID = 4;

        public static bool gameActive = false; //Is there a game in progress to continue from?
        public static int bestFightsWon = 0; //high score for a whole party.

        public static List<String> unlockedRoles = new List<string>() {"Soldier", "Medic", "Techie", "CovertOp"};
        public static List<int> unlockedTitleScreens = new List<int>(); //pending implementation

        public static Character levelingUpChar = FightScene.characters[0]; //gets set before switching to improvement scene.
        
        public static string Char1Name = "Larry";
        public static string Char2Name = "Gary";
        public static string Char3Name = "Cherri";
        public static string Char4Name = "Clyde";


        //Current game values. Copy this section once per charcter.
        public static Character char1; //Or just do this instead of tracking it multiple times?
        public static Character char2; //Or just do this instead of tracking it multiple times?
        public static Character char3; //Or just do this instead of tracking it multiple times?
        public static Character char4; //Or just do this instead of tracking it multiple times?

        public static int Char1Level = 0;
        public static int Char1Role =0; //index of the role in the role list.
        //Points spent leveling abilities
        public static int Char1Ability1Level = 0;
        public static int Char1Ability2Level = 0;
        public static int Char1Ability3Level = 0;
        public static int Char1Ability4Level = 0;
        //Points spent boosting stats. (these get added to the base stat value)
        public static int Char1BonusMaxHp = 0;
        public static int Char1BonusMaxAp = 0;
        public static int Char1BonusSTR = 0;
        public static int Char1BonusDEF = 0;
        public static int Char1BonusINS = 0;
        public static int Char1BonusMOX = 0;

    }
}