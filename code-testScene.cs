using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
    public static class TestScene
    {
        public static JrpgRoslynChip parentRef;
        //Probably need to set up a title screen meta-sprite
        //a cool widescreen image to drop in as a meta-sprite.
        //and some copyright info, plus a high score display of all classes' highest levels added up.
        static int xScreenCoords = 0; // * 43 for X screens over
        static int yScreenCoords = 0; // * 31 for Y screens down.

        static int frameCounter = 0;
        
        public static void Init()
        {
            //nothing to init
        }

        public static void Update(int timeDelta)
        {
            parentRef.WriteSaveData("testEntry"+ frameCounter, frameCounter.ToString());
            frameCounter++;
        }

        public static void Draw()
        {
            DrawColorshiftTest();
        }

        public static void DrawColorshiftTest()
        {
            //Color shift sprite test.
            //If i want shadows to come out as different colors, i have to make
            //variant colors that are like, 1 bit off and then put the color it should shift up to after.
            string sprite = "crab1";
            parentRef.DrawMetaSprite(sprite, 1, 1, false, false, DrawMode.Sprite, 0);
            parentRef.DrawMetaSprite(sprite, 65, 1, false, false, DrawMode.Sprite, 1);
            parentRef.DrawMetaSprite(sprite, 129, 1, false, false, DrawMode.Sprite, 2);
            parentRef.DrawMetaSprite(sprite, 194, 1, false, false, DrawMode.Sprite, 3);
            parentRef.DrawMetaSprite(sprite, 1, 65, false, false, DrawMode.Sprite, 4);
            parentRef.DrawMetaSprite(sprite, 65, 65, false, false, DrawMode.Sprite, 5);
            parentRef.DrawMetaSprite(sprite, 129, 65, false, false, DrawMode.Sprite, 6);
            parentRef.DrawMetaSprite(sprite, 194, 65, false, false, DrawMode.Sprite, 7);
            parentRef.DrawMetaSprite(sprite, 1, 129, false, false, DrawMode.Sprite, 8);
            parentRef.DrawMetaSprite(sprite, 65, 129, false, false, DrawMode.Sprite, 9);
            parentRef.DrawMetaSprite(sprite, 129, 129, false, false, DrawMode.Sprite, 10);
            parentRef.DrawMetaSprite(sprite, 194, 129, false, false, DrawMode.Sprite, 11);
            parentRef.DrawMetaSprite(sprite, 1, 194, false, false, DrawMode.Sprite, 12);
            parentRef.DrawMetaSprite(sprite, 65, 194, false, false, DrawMode.Sprite, 13);
            parentRef.DrawMetaSprite(sprite, 129, 194, false, false, DrawMode.Sprite, 14);
            parentRef.DrawMetaSprite(sprite, 194, 194, false, false, DrawMode.Sprite, 15);
        }
    }
}