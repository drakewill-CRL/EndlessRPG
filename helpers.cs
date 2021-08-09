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
            //PV8 internally clamps save slots to 16. I'll need to use 16 or fewer total vars.
            //Call this function at the beginning of each turn, so the player always has a saved game to fall back to.
            //save unlocks and best levels
            // for(int i= 0; i < 100; i++)
            //     gc.WriteSaveData("testEntry"+ i, i.ToString());

            //16 save items:
            //1: UnlockedRoles[]
            //2: BestRoleLevels[]
            //3: BestFightsWon
            //4: TotalFightsWon
            //5: char1SaveDAta
            //6: char2SaveDAta
            //7: char3SaveDAta
            //8: char4SaveDAta
            //9: default names[]
            //10: enemies
            //6 more

            string unlockedRoles = "";
            string bestLevels = "";
            string charNames = "";

            foreach(var r in ContentLists.allRoles)
            {
                if (gameState.unlockedRoles.Contains(r.name))
                    unlockedRoles += r.name + "|";
                
                bestLevels += r.name + ":" +gameState.bestLevels[r.name] + "|";
            }

            //Save array data
            gc.WriteSaveData("unlockedRoles", unlockedRoles);
            gc.WriteSaveData("bestLevels", bestLevels);
            gc.WriteSaveData("charNames", gameState.Char1Name + "|" + gameState.Char2Name + "|" + gameState.Char3Name + "|" + gameState.Char4Name);
            
            //Save high scores
            gc.WriteSaveData("totalBestLevels", gameState.bestLevels.Sum(l => l.Value).ToString());
            gc.WriteSaveData("bestFightsWon", gameState.bestFightsWon.ToString());
            gc.WriteSaveData("timePlayed", gameState.timePlayed.TotalSeconds.ToString());

            //save current run data
            gc.WriteSaveData("gameActive", gameState.mode.ToString()); //Check for 1, since that's the gameplay mode.

            //TODO: if active, save characters and enemies list.

        }

        

        // public static void InitializeSaveData(this GameChip gc) //This should be redundant
        // {
        //     //Set default flags
        //     //gc.WriteSaveData("FirstRun", "0");

        //     //Character names.
        //     gc.WriteSaveData("Char1Name", "Larry");
        //     gc.WriteSaveData("Char2Name", "Gary");
        //     gc.WriteSaveData("Char3Name", "Cherri");
        //     gc.WriteSaveData("Char4Name", "Clyde");

        //     //Unlock initial roles.
        //     gc.WriteSaveData("unlockedInfantry", "1");
        //     gc.WriteSaveData("unlockedMedic", "1");
        //     gc.WriteSaveData("unlockedTechie", "1");
        //     gc.WriteSaveData("unlockedCovertOp", "1");

        //     //Set default scores
        //     foreach(var role in ContentLists.allRoles)
        //         gc.WriteSaveData("best" + role.name + "Level", "0");

        //     gc.WriteSaveData("fightsWon", "0");
        //     gc.WriteSaveData("totalBestLevels", "0");
        //     gc.WriteSaveData("timePlayed", "0"); //in seconds.

        //     gc.WriteSaveData("gameActive", "0"); //is there a suspended game to load, or are we making a new game?
        // }

        public static string GetRealError(Exception ex)
        {
            while(ex.InnerException != null)
                ex = ex.InnerException;

            Console.WriteLine(ex.Message + ": " + ex.StackTrace);

            return ex.Message + " " + ex.StackTrace;

        }
    }
}
