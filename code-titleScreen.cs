using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
    public static class TitleScene
    {
        public static JrpgRoslynChip parentRef;
        //Probably need to set up a title screen meta-sprite
        //a cool widescreen image to drop in as a meta-sprite.
        //and some copyright info, plus a high score display of all classes' highest levels added up.
        static int xScreenCoords = 0; // * 43 for X screens over
        static int yScreenCoords = 0; // * 31 for Y screens down.
        static string version = "v 0.00";
        static string pushStart  ="Press Start";
        static string credits = "2021 Drake Williams and Marcos Sastre";
        static string license1 = "Eclipse Phase Content by Posthuman Studios";
        static string license2 = "Licensed via CC BY-NC-SA 4.0";
        static bool hasDrawn = false;
        static bool clear = false;

        public static void Init()
        {
            //nothing to init
        }

        public static void Update(int timeDelta)
        {
            Input();
        }

        public static void Draw()
        {
            //Plan:
            //Screen will be 344*248, approx. "widescreen SNES" size.
            //Sprites should be 16*32, SNES colors per sprite? that's 128 of 244 vert pixels, leaving 96 for menus/chrome/etc in a single vertical line.
            //TODO: multiple title screens, selected at random. possibly as unlocks.
            parentRef.BackgroundColor(0);
            parentRef.ScrollPosition(0);
            if (!hasDrawn)
                parentRef.DrawMetaSprite("EPExpendablesTitle1", 1, 1, false, false, DrawMode.Tile, 0);

            parentRef.CenterText(pushStart, 27 *8);
            parentRef.RightAlignText(version, 27 *8);
            parentRef.CenterText(credits, 28 *8);
            parentRef.CenterText(license1, 29 *8);
            parentRef.CenterText(license2, 30 *8);
            if (clear)
                parentRef.Clear();

        }

        public static void Input()
        {
            if (parentRef.Button(Buttons.Start, InputState.Released) || parentRef.Button(Buttons.B, InputState.Released))
            {
                clear = true;
                gameState.mode = gameState.NewGameSceneID; 
                //gameState.mode = gameState.FightSceneID //fight for test purposes
                //TODO: go to newGame screen to pick a party if no saved game to continue,
                //or the fight screen with an existing party if there's an active save game.
            }
        }

        

        

    }

}