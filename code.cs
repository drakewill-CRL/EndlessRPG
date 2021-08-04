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
            if (isFirstRun == "1")
                this.InitializeSaveData();
            else
                this.LoadGameData();

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

       
    }
}