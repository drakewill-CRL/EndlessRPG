using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
    public static class NewGameScene
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
            //need room for 4 PC sprites, small descriptions of each. Possibly a naming option that kicks you to another screen

        }

        public static void Input()
        {
        }

    }

}