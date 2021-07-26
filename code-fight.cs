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

        public static List<Tuple<int,int>> ArrowPoints = new List<Tuple<int, int>>()
        {
            new Tuple<int, int>(3 * 8, 20 * 8), //fight
            new Tuple<int, int>(3 * 8, 22 * 8), //ability
            new Tuple<int, int>(3 * 8, 24 * 8), //defend
            new Tuple<int, int>(3 * 8, 26 * 8), //run
            new Tuple<int, int>(3 * 8, 28 * 8), //auto
        };

        static List<AttackResults> resultsToParse = new List<AttackResults>();

        public static int arrowPosIndex = 0; //default to fight
        public static int phase = 0; // 0 = select attacks, 1 = watch round play out.

        public static int currentAnimationID = 0; //future setup
        public static int currentAnimationFramesLeft = 0; //future setup

        public static string HelpText = "Some helpful stuff goes here and oh look its on multiple lines now";
        //TODO: work out a setup for transition animations (EX: characters walking forward or back to their spot)
        //TODO: save list of places to put cursor for its positions (there's 4 for text selections, at least 4 for enemies.)
        //TODO: help text
        //TODO: chrome for frames and all that. Those might be big metasprites

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
            //check phase, advance animations or input requests.
            //do any math required.

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
            
           // parentRef.DrawRect(0, 150, 340, 8*11, 3, DrawMode.Sprite); //Possible text/command area.

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

            DrawMenuList();
            DrawHelpText();

            parentRef.DrawMetaSprite("arrow", ArrowPoints[arrowPosIndex].Item1, ArrowPoints[arrowPosIndex].Item2);

        }

        public static void Input()
        {
            //up and down move arrow, A advances to next command, B rolls back to previous
            //tricky part, the arrow can be on the menu, a submenu, players, or enemies
            //also, update help text when arrow moves to match whatever its on
        }

        public static void DrawMenuList()
        {
            //This is the default menu list of options and possibly some help text.
            //Fight, Ability, Defend, Run (% chance to re-roll the current fight, but you don't get XP for surviving.)

            parentRef.DrawText("Fight", 4, 20, DrawMode.Tile, "large", 15); //spacing in tiles, not pixels.
            parentRef.DrawText("Ability", 4, 22, DrawMode.Tile, "large", 15);
            parentRef.DrawText("Defend", 4, 24, DrawMode.Tile, "large", 15); 
            parentRef.DrawText("Run", 4, 26, DrawMode.Tile, "large", 15); 
            parentRef.DrawText("Auto", 4, 28, DrawMode.Tile, "large", 15); 
        }

        public static void DrawHelpText()
        {
            //The text that shows up on the right side of the bottom panel to show you what the thing does.
            //TODO: set up string to be drawn
            var wrapped = parentRef.WordWrap(HelpText, 25);
            var lines = parentRef.SplitLines(wrapped);
            for(int i =0; i < lines.Count(); i++)
                parentRef.DrawText(lines[i], 15, 20 + i, DrawMode.Tile, "large", 15); 
        }

    }


}