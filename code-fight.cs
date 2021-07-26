using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player{
    public static class FightScene
    {
        public static JrpgRoslynChip parentRef;

        public static void Init()
        {
            
        }

        public static void Update(int timeDelta)
        {
            Input();
        }

        public static void Draw()
        {
            //Plan:
            //Screen will be 340*244, approx. "widescreen SNES" size.
            //Sprites should be 16*32, SNES colors per sprite? that's 128 of 244 vert pixels, leaving 96 for menus/chrome/etc in a single vertical line.
            
            //Test layout areas.
            //DrawRect ( x, y, width, height, color, drawMode )
            parentRef.DrawRect(300,16, 16, 32 * 4, 2, DrawMode.Sprite); //Baseline PC sprite locations
            
            parentRef.DrawRect(0, 150, 340, 8*11, 3, DrawMode.Sprite); //Possible text/command area.

            parentRef.DrawRect(8,16, 250, 32 * 4, 4, DrawMode.Sprite); //Possible enemy area

        }

        public static void Input()
        {
        }

    }


}