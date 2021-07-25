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

        List<Role> roles = new List<Role>() {
        	new Role() {name = "Generic"}
    	};
        public override void Init()
        {

            var message = "EMPTY C# GAME\n\n\nThis is an empty game template.\n\n\nVisit 'www.pixelvision8.com' to learn more about creating games from scratch.";
            var display = Display();
            var wrap = WordWrap(message, (display.X / 8) - 2);
            var lines = SplitLines(wrap);
            var total = lines.Length;
            var startY = ((display.Y / 8) - 1) - total;

            // We want to render the text from the bottom of the screen so we offset
            // it and loop backwards.
            for (var i = total - 1; i >= 0; i--)
                DrawText(lines[i], 1, startY + (i - 1), DrawMode.Tile, "large", 15);
        }

        public override void Draw()
        {

        }

        public override void Update(int timeDelta)
        {
            RedrawDisplay();
        }

		public void SaveGameData(){
			//The best thing to do here would be to loop over gameState and just save each property by name and value, if reflection is available.
			//Save high scores

			//save current run data

			//save unlockable flags

		}
		public void LoadGameData() {
			//The best thing to do here would be to loop over gameState and just save each property by name and value if reflection is available.
			//load high scores

			//load current run data

			//load unlockable flags

		}
    }
}