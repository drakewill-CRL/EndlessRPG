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

        }

        public static void Input()
        {
        }

    }

}