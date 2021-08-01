//
// Pixel Vision 8 - New Template Script
// Copyright (C) 2017, Pixel Vision 8 (@pixelvision8)
// Created by Jesse Freeman (@jessefreeman)
// Converted from the Lua file by Drake Williams [drakewill+pv8@gmail.com]
//
// This project was designed to display some basic instructions when you create
// a new game.  Simply delete the following code and implement your own Init(),
// Update() and Draw() logic.
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

            LoadGameData();
            //Any post-startup resets of values would go here.
			gameState.mode = gameState.FightSceneID; //jump to fight screen
            //gameState.mode = gameState.TitleSceneID; //testing title screen.
            gameState.mode = gameState.ImproveSceneID; //Drawing level up screen.
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

            //load unlockable flags

        }
    }
}