using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
    public static class Extensions //for helpers and common functions.
    {
        public static void CenterText(this GameChip gc, string text, int y)
        {
            var startPoint = (gc.Display().X - (text.Length * 8)) / 2;
            gc.DrawText(text, startPoint, y, DrawMode.Sprite, "large", 15);
        }

        public static void RightAlignText(this GameChip gc, string text, int y)
        {
            var startPoint = (gc.Display().X - (text.Length * 8));
            gc.DrawText(text, startPoint, y, DrawMode.Sprite, "large", 15);
        }

        public static void WrapText(this GameChip gc, string text, int width, int x, int startY)
        {
            var wrapped = gc.WordWrap(text, width);
            var lines = gc.SplitLines(wrapped);
            for (int i = 0; i < lines.Count(); i++)
                gc.DrawText(lines[i], x, (startY + i) * 8, DrawMode.Sprite, "large", 15);
        }
         public static void SaveGameData(this GameChip gc)
        {
            //Call this function at the beginning of each turn, so the player always has a saved game to fall back to.

            //save unlocks and best levels
            foreach(var r in ContentLists.allRoles)
            {
                if (gameState.unlockedRoles.Contains(r.name))
                    gc.WriteSaveData("unlocked" + r.name, "1");
                else
                    gc.WriteSaveData("unlocked" + r.name, "0");

                //WriteSaveData("best" + r.name + "Level", gameState.bestLevels[r.name].ToString());
            }
            //Save high scores
            gc.WriteSaveData("fightsWon", gameState.fightsWon.ToString());
            //WriteSaveData("totalBestLevels", ) //unnecessary
            gc.WriteSaveData("timePlayed", gameState.timePlayed.TotalSeconds.ToString());
            //WriteSaveData("key", "value";)

            //save current run data
            gc.WriteSaveData("gameActive", gameState.mode.ToString()); //Check for 1, since that's the gameplay mode.

            gc.WriteSaveData("Char1Name", gameState.Char1Name);
            gc.WriteSaveData("Char2Name", gameState.Char2Name);
            gc.WriteSaveData("Char3Name", gameState.Char3Name);
            gc.WriteSaveData("Char4Name", gameState.Char4Name);
        }

        public static void LoadGameData(this GameChip gc)
        {
            //The best thing to do here would be to loop over gameState and just save each property by name and value if reflection is available.
            //load high scores
            //var x = ReadSaveData("key", "default");

            //load current run data
            
           if (gc.ReadSaveData("gameActive") == gameState.FightSceneID.ToString()) //It was saved in a fight screen.
            {
                //Load up existing game data, set title screen to Continue.
            }

            //load role-specific data
            foreach(var role in ContentLists.allRoles)
            {
                if (gc.ReadSaveData("unlocked" + role.name, "0") == "1")
                    gameState.unlockedRoles.Add(role.name);
                
                //This appears to be having issues.
                //gameState.bestLevels.Add(role.name, Int32.Parse(ReadSaveData("best" + role.name + "Level", "0")));
            }

            //debug check. Looks like this reports all roles, but the correct list shows up on screen?
            //Console.WriteLine("unlocked " + gameState.unlockedRoles.Count() + " roles");

            gameState.fightsWon = Int32.Parse(gc.ReadSaveData("fightsWon", "0"));
            gameState.totalBestLevels = gameState.bestLevels.Sum(l => l.Value);

            gameState.timePlayed = TimeSpan.FromSeconds(Double.Parse(gc.ReadSaveData("timePlayed", "0")));

            gameState.Char1Name = gc.ReadSaveData("Char1Name", "Larry");
            gameState.Char2Name = gc.ReadSaveData("Char2Name", "Gary");
            gameState.Char3Name = gc.ReadSaveData("Char3Name", "Cherri");
            gameState.Char4Name = gc.ReadSaveData("Char4Name", "Clyde");
        }

        public static void InitializeSaveData(this GameChip gc)
        {
            //Set default flags
            gc.WriteSaveData("FirstRun", "0");

            //Character names.
            gc.WriteSaveData("Char1Name", "Larry");
            gc.WriteSaveData("Char2Name", "Gary");
            gc.WriteSaveData("Char3Name", "Cherri");
            gc.WriteSaveData("Char4Name", "Clyde");

            //Unlock initial roles.
            gc.WriteSaveData("unlockedInfantry", "1");
            gc.WriteSaveData("unlockedMedic", "1");
            gc.WriteSaveData("unlockedTechie", "1");
            gc.WriteSaveData("unlockedCovertOp", "1");

            //Set default scores
            foreach(var role in ContentLists.allRoles)
                gc.WriteSaveData("best" + role.name + "Level", "0");

            gc.WriteSaveData("fightsWon", "0");
            gc.WriteSaveData("totalBestLevels", "0");
            gc.WriteSaveData("timePlayed", "0"); //in seconds.

            gc.WriteSaveData("gameActive", "0"); //is there a suspended game to load, or are we making a new game?
        }

    }
}
