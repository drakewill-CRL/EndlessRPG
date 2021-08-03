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
        public override void Init()
        {
            FightScene.parentRef = this;
            TitleScene.parentRef = this;
            NewGameScene.parentRef = this;
            ImproveScene.parentRef = this;

            FightScene.Init();

            if (ReadSaveData("FirstRun") == "undefined")
                InitializeSaveData();
            else
                LoadGameData();

            //Any post-startup resets of values would go here.
			//gameState.mode = gameState.FightSceneID; //jump to fight screen
            gameState.mode = gameState.TitleSceneID; //testing title screen.
            //gameState.mode = gameState.ImproveSceneID; //Drawing level up screen.
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
                //case 3:
                    //Roulette.Draw();
                    //break;
            }
        }

        public override void Update(int timeDelta)
        {
            //TODO: count MS here, when i hit a full seconds add 1 to gameState.TimePlayed

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
                //case 3:
                    //Roulette.Draw();
                    //break;
            }
        }

        public void SaveGameData()
        {
            //The best thing to do here would be to loop over gameState and just save each property by name and value, if reflection is available.
            //Save high scores
            //WriteSaveData("key", "value";)

            //save current run data

            //save unlockable flags

        }
        public void LoadGameData()
        {
            //The best thing to do here would be to loop over gameState and just save each property by name and value if reflection is available.
            //load high scores
            //var x = ReadSaveData("key", "default");

            //load current run data

            //load role-specific data
            foreach(var role in ContentLists.allRoles)
            {
                if (ReadSaveData("unlocked" + role.name, "0") == "1")
                    gameState.unlockedRoles.Add(role.name);
                
                gameState.bestLevels.Add(role.name, Int32.Parse(ReadSaveData("best" + role.name + "Level", "0")));

            }

            gameState.fightsWon = Int32.Parse(ReadSaveData("fightsWon", "0"));
            gameState.totalBestLevels = gameState.bestLevels.Sum(l => l.Value);

            gameState.timePlayed = TimeSpan.FromSeconds(Double.Parse(ReadSaveData("timePlayed", "0")));

        }

        public void InitializeSaveData()
        {
            //Set default flags
            WriteSaveData("FirstRun", "0");

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