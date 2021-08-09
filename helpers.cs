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
            //10: enemy1
            //11: enemy2
            //12: enemy3
            //13: enemy4
            //14: current fights won
            //2 more

            string unlockedRoles = "";
            string bestLevels = "";
            string charNames = "";

            foreach (var r in ContentLists.allRoles)
            {
                if (gameState.unlockedRoles.Contains(r.name))
                    unlockedRoles += r.name + "|";

                bestLevels += r.name + ":" + gameState.bestLevels[r.name] + "|";
            }

            //Save array data
            gc.WriteSaveData("unlockedRoles", unlockedRoles);
            gc.WriteSaveData("bestLevels", bestLevels);
            gc.WriteSaveData("charNames", gameState.Char1Name + "|" + gameState.Char2Name + "|" + gameState.Char3Name + "|" + gameState.Char4Name);

            //Save high scores
            gc.WriteSaveData("totalBestLevels", gameState.bestLevels.Sum(l => l.Value).ToString());
            gc.WriteSaveData("bestFightsWon", gameState.bestFightsWon.ToString());
            //gc.WriteSaveData("timePlayed", gameState.timePlayed.TotalSeconds.ToString());

            //save current run characters
            gc.WriteSaveData("gameActive", gameState.mode.ToString()); //Check for 1, since that's the gameplay mode.
            gc.WriteSaveData("currentWins", FightScene.fightsWon.ToString());
            gc.WriteSaveData("char1", FightScene.characters[0].GetSaveData());
            gc.WriteSaveData("char2", FightScene.characters[1].GetSaveData());
            gc.WriteSaveData("char3", FightScene.characters[2].GetSaveData());
            gc.WriteSaveData("char4", FightScene.characters[3].GetSaveData());

            //save enemies
            for(int i = 1; i < 5; i++)
            {
                if (FightScene.enemies.Count() >= i)
                {
                    var saveData = FightScene.enemies[i-1].GetSaveData();
                    gc.WriteSaveData("enemy" + i.ToString(), saveData);
                }
                else
                {
                    gc.WriteSaveData("enemy" + i.ToString(), "");
                }
            }

        }

        public static void LoadGameData(this GameChip gc)
        {
            string rolesUnlocked = gc.ReadSaveData("unlockedRoles", "Soldier|Medic|Techie|CovertOp");
            var splitRoles = rolesUnlocked.Split("|");
            gameState.unlockedRoles = splitRoles.ToList();

            string bestLevels = gc.ReadSaveData("bestLevels", "0");
            string[] splitLevels = bestLevels.Split("|");
            if (bestLevels == "0")
                splitLevels = gameState.bestLevels.Select(l => l.Key + ":" + l.Value.ToString()).ToArray(); //They should all be 0 here

            foreach (var l in splitLevels)
            {
                if (String.IsNullOrWhiteSpace(l))
                    continue;

                string[] kvp = l.Split(":");
                Console.WriteLine(kvp[0] + ":" + kvp[1]);
                gameState.bestLevels.Remove(kvp[0]);
                gameState.bestLevels.Add(kvp[0], Int32.Parse(kvp[1]));
            }

            gameState.bestFightsWon = Int32.Parse(gc.ReadSaveData("bestFightsWon", "0"));

            var charNames = gc.ReadSaveData("charNames", "Larry|Gary|Cherri|Clyde").Split("|");
            gameState.Char1Name = charNames[0];
            gameState.Char2Name = charNames[1];
            gameState.Char3Name = charNames[2];
            gameState.Char4Name = charNames[3];

            //character data 
            if (gc.ReadSaveData("gameActive", "0") == gameState.FightSceneID.ToString())
            {
                FightScene.fightsWon = Int32.Parse(gc.ReadSaveData("currentWins", "0"));
                //Load characters from file.
                FightScene.characters = new List<Character>();
                FightScene.characters.Add(new Character(gc.ReadSaveData("char1", "")));
                FightScene.characters.Add(new Character(gc.ReadSaveData("char2", "")));
                FightScene.characters.Add(new Character(gc.ReadSaveData("char3", "")));
                FightScene.characters.Add(new Character(gc.ReadSaveData("char4", "")));

                //load enemies.
                FightScene.enemies = new List<Enemy>();
                for (int i = 1; i < 5; i++)
                {
                    var enemyData = gc.ReadSaveData("enemy" + i.ToString(), "");
                    if (enemyData != "")
                    {
                        var enemy = ContentLists.enemiesByName[enemyData.Split("-")[0]].Clone();
                        enemy.FromSaveData(enemyData);
                        FightScene.enemies.Add(enemy);
                    }
                }
            }
        }

        public static string GetRealError(Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;

            Console.WriteLine(ex.Message + ": " + ex.StackTrace);

            return ex.Message + " " + ex.StackTrace;

        }
    }
}
