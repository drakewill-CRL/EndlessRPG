using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player{
    public static class FightScene
    {
        public static JrpgRoslynChip parentRef;
        public static List<Character> characters = new List<Character>();
        public static List<Enemy> enemies = new List<Enemy>();
        //TODO: work out a setup for transition animations (EX: characters walking forward or back to their spot)
        

        public static void Init()
        {
            //This gets called manually before entering the fight scene from new game.
            //Test values for layout purposes.
            Character char1 = new Character();
            char1.spriteSet = "char1";
            char1.posX = 300;
            char1.posY = 16;
            char1.drawState = "Idle";
            characters.Add(char1);
            Character char2 = new Character();
            char2.spriteSet = "char1";
            char2.posX = 300;
            char2.posY = 48;
            char2.drawState = "Idle";
            characters.Add(char2);
            Character char3 = new Character();
            char3.spriteSet = "char1";
            char3.posX = 300;
            char3.posY = 80;
            char3.drawState = "Idle";
            characters.Add(char3);
            Character char4 = new Character();
            char4.spriteSet = "char1";
            char4.posX = 300;
            char4.posY = 112;
            char4.drawState = "Idle";
            characters.Add(char4);

            Enemy enemy1 = new Enemy();
            enemy1.spriteSet = "enemy1";
            enemy1.posX = 8;
            enemy1.posY = 16;
            enemies.Add(enemy1);
            Enemy enemy2 = new Enemy();
            enemy2.spriteSet = "enemy1";
            enemy2.posX = 8;
            enemy2.posY = 80;
            enemies.Add(enemy2);
            Enemy enemy3 = new Enemy();
            enemy3.spriteSet = "enemy1";
            enemy3.posX = 72;
            enemy3.posY = 16;
            enemies.Add(enemy3);
            Enemy enemy4 = new Enemy();
            enemy4.spriteSet = "enemy1";
            enemy4.posX = 72;
            enemy4.posY = 80;
            enemies.Add(enemy4);
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
            //parentRef.DrawRect(300,16, 16, 32 * 4, 2, DrawMode.Sprite); //Baseline PC sprite locations
            
            parentRef.DrawRect(0, 150, 340, 8*11, 3, DrawMode.Sprite); //Possible text/command area.

            //parentRef.DrawRect(8,16, 250, 32 * 4, 4, DrawMode.Sprite); //Possible enemy area

            //test sprites
            foreach(var c in characters)
            {
                parentRef.DrawMetaSprite(c.spriteSet +c.drawState, c.posX, c.posY);
            }

            foreach(var e in enemies)
            {
                parentRef.DrawMetaSprite(e.spriteSet, e.posX, e.posY);
            }

        }

        public static void Input()
        {
        }

        public static void DrawMenuList()
        {
            //This is the default menu list of options and possibly some help text.
            //Fight, Ability, Defend, Run (% chance to re-roll the current fight, but you don't get XP for surviving.)
        }

    }


}