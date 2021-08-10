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
        public static int mode = TitleSceneID; // which scene we're in right now.        
        public const int TitleSceneID = 0;
        public const int NewGameSceneID = 1;
        public const int FightSceneID = 2;
        public const int ImproveSceneID = 3;
        public const int TestSceneID = 4;

        /* 
        sound IDs
        0: arrow-move and menu-advance
        1: quick gun attack
        2: menu-negative (action not allowed)
        3: PC is hit (1 is the PC attacking.)

        */

        public static bool gameActive = false; //Is there a game in progress to continue from?
        public static int bestFightsWon = 0; //high score for a whole party.

        public static List<string> unlockedRoles = new List<string>(); //"Soldier", "Medic", "Techie", "CovertOp"
        public static List<int> unlockedTitleScreens = new List<int>(); //pending implementation

        public static Character levelingUpChar = new Character(ContentLists.allRoles[0], 1); //FightScene.characters[0]; //gets set before switching to improvement scene.
        
        public static string Char1Name = "Larry";
        public static string Char2Name = "Gary";
        public static string Char3Name = "Cherri";
        public static string Char4Name = "Clyde";

        //TODO: make an Options screen that allows you to change this speed. Possibly also animation display speed.
        public static int displayDefaultTimer = 60; //120?
    }
}