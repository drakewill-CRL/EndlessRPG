//
// Pixel Vision 8 - Eclipse Phase Expendables
// 2021 Drake Wiliams and Marcos Sastre
// 
// Learn more about making Pixel Vision 8 games at
// https://www.pixelvision8.com/getting-started
// 
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
    public class JrpgRoslynChip : GameChip
    {

        int msCounter =0;
        public override void Init()
        {
            FightScene.parentRef = this;
            TitleScene.parentRef = this;
            NewGameScene.parentRef = this;
            ImproveScene.parentRef = this;

            FightScene.Init();

            //TODO: can save 9 values in Saves.json, more get ignored. Will ahve to reconsider that impact on future plans.
            var isFirstRun = ReadSaveData("FirstRun", "1");
            Console.WriteLine(isFirstRun);
            if (isFirstRun == "1")
                InitializeSaveData();
            else
                LoadGameData();

            //Any post-startup resets of values would go here.
            gameState.mode = gameState.TitleSceneID; //normal gameplay flow
			//gameState.mode = gameState.FightSceneID; //jump to fight screen
            //gameState.mode = gameState.ImproveSceneID; //Drawing level up screen.
            //gameState.mode = gameState.NewGameSceneID; //straight to chargen.
        }

        public override void Draw()
        {
            DrawText("FPS: " + ReadFPS(), 0, 0, DrawMode.Sprite, "large", 14); //This doesn't hit 60FPS
            switch (gameState.mode)
            {
                case gameState.TitleSceneID:
                    TitleScene.Draw();
                    break;
                case gameState.FightSceneID:
                    FightScene.Draw();
                    break;
                case gameState.ImproveSceneID:
                    ImproveScene.Draw();
                    break;
                case gameState.NewGameSceneID:
                    NewGameScene.Draw();
                    break;
            }
        }

        public override void Update(int timeDelta)
        {
            //TODO: count MS here, when i hit a full seconds add 1 to gameState.TimePlayed
            msCounter += timeDelta;
            if (msCounter >= 1000)
            {
                gameState.timePlayed.Add(TimeSpan.FromSeconds(1));
                WriteSaveData("timePlayed", gameState.timePlayed.TotalSeconds.ToString());
            }

            RedrawDisplay();
            switch (gameState.mode)
            {
                case gameState.TitleSceneID:
                    TitleScene.Update(timeDelta);
                    break;
                case gameState.FightSceneID:
                    FightScene.Update(timeDelta);
                    break;
                case gameState.ImproveSceneID:
                    ImproveScene.Update(timeDelta);
                    break;
                case gameState.NewGameSceneID:
                    NewGameScene.Update(timeDelta);
                    break;
            }
        }

        public void SaveGameData()
        {
            //Call this function at the beginning of each turn, so the player always has a saved game to fall back to.

            //save unlocks and best levels
            foreach(var r in ContentLists.allRoles)
            {
                if (gameState.unlockedRoles.Contains(r.name))
                    WriteSaveData("unlocked" + r.name, "1");
                else
                    WriteSaveData("unlocked" + r.name, "0");

                //WriteSaveData("best" + r.name + "Level", gameState.bestLevels[r.name].ToString());
            }
            //Save high scores
            WriteSaveData("fightsWon", gameState.fightsWon.ToString());
            //WriteSaveData("totalBestLevels", ) //unnecessary
            WriteSaveData("timePlayed", gameState.timePlayed.TotalSeconds.ToString());
            //WriteSaveData("key", "value";)

            //save current run data
            WriteSaveData("gameActive", gameState.mode.ToString()); //Check for 1, since that's the gameplay mode.

            WriteSaveData("Char1Name", gameState.Char1Name);
            WriteSaveData("Char2Name", gameState.Char2Name);
            WriteSaveData("Char3Name", gameState.Char3Name);
            WriteSaveData("Char4Name", gameState.Char4Name);

            for (int i =0; i < 100; i++)
                WriteSaveData("testEntry" + i, i.ToString());

        }
        public void LoadGameData()
        {
            //The best thing to do here would be to loop over gameState and just save each property by name and value if reflection is available.
            //load high scores
            //var x = ReadSaveData("key", "default");

            //load current run data
            
           // if (ReadSaveData("gameActive") == gameState.FightSceneID.ToString()) //It was saved in a fight screen.
            //{
                //Load up existing game data, set title screen to Continue.
            //}

            //load role-specific data
            // foreach(var role in ContentLists.allRoles)
            // {
            //     if (ReadSaveData("unlocked" + role.name, "0") == "1")
            //         gameState.unlockedRoles.Add(role.name);
                
            //     gameState.bestLevels.Add(role.name, Int32.Parse(ReadSaveData("best" + role.name + "Level", "0")));
            // }

            //debug check
            //Console.WriteLine("unlocked " + gameState.unlockedRoles.Count() + " roles");

            // gameState.fightsWon = Int32.Parse(ReadSaveData("fightsWon", "0"));
            // gameState.totalBestLevels = gameState.bestLevels.Sum(l => l.Value);

            // gameState.timePlayed = TimeSpan.FromSeconds(Double.Parse(ReadSaveData("timePlayed", "0")));

            Console.WriteLine(ReadSaveData("Char1Name", "Larry"));
            //gameState.Char1Name = ReadSaveData("Char1Name", "Larry");
            //gameState.Char2Name = ReadSaveData("Char2Name", "Gary");
            //gameState.Char3Name = ReadSaveData("Char3Name", "Cherri");
            //gameState.Char4Name = ReadSaveData("Char4Name", "Clyde");

        }

        public void InitializeSaveData()
        {
            //Set default flags
            WriteSaveData("FirstRun", "0");

            //Character names.
            WriteSaveData("Char1Name", "Larry");
            WriteSaveData("Char2Name", "Gary");
            WriteSaveData("Char3Name", "Cherri");
            WriteSaveData("Char4Name", "Clyde");

            //Unlock initial roles.
            WriteSaveData("unlockedInfantry", "1");
            WriteSaveData("unlockedMedic", "1");
            WriteSaveData("unlockedTechie", "1");
            WriteSaveData("unlockedCovertOp", "1");

            //Set default scores
            foreach(var role in ContentLists.allRoles)
                WriteSaveData("best" + role.name + "Level", "0");

            WriteSaveData("fightsWon", "0");
            WriteSaveData("totalBestLevels", "0");
            WriteSaveData("timePlayed", "0"); //in seconds.

            WriteSaveData("gameActive", "0"); //is there a suspended game to load, or are we making a new game?

        }
    }
}