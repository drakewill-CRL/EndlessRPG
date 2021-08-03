using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
    public static class ImproveScene
    {
        public static JrpgRoslynChip parentRef;

        //3 options on improvement:
        //1: level up
        //gain points in all stats, refresh HP and AP, get better equipment rolls in the future, but enemies get slightly stronger.
        //2: stats boost
        //get 5 points added to a stat of your choice, but you don't get the HP/AP refill. Enemies won't scale up though.
        //3: item roll
        //Get a random but significant boosts to stats, (5 points per level?), that also doesn't make enemies scale. Picks 1 of 2 slots, replaces any existing entry there.

        //should also list off stats a level grants.
        //will need to get which character from gameState.

        static int xScreenCoords = 2; //In case I draw stuff in the background via tiles.
        static int yScreenCoords = 0;
        static int arrowAdjustment = 0; //add this to Y values to fix arrow positions.

        static List<Tuple<int, int>> ArrowPoints = new List<Tuple<int, int>>()
        {
            new Tuple<int, int>(5, 7),
            new Tuple<int, int>(5, 14),
            new Tuple<int, int>(5, 20),
        };

        static List<Tuple<int, int>> ArrowPointsStatBoosts = new List<Tuple<int, int>>()
        {
            new Tuple<int, int>(7, 17),
            new Tuple<int, int>(14, 17),
            new Tuple<int, int>(20, 17),
            new Tuple<int, int>(27, 17),
            new Tuple<int, int>(7, 18),
            new Tuple<int, int>(14, 18),
            new Tuple<int, int>(20, 18),
            new Tuple<int, int>(27, 18),
        };
        static int arrowPosIndex = 0;
        static int phase = 0; //0 = main list, 1 = stat boost sub-list.

        public static void Init()
        {
            //nothing to currently init
        }

        public static void Update(int timeDelta)
        {
            Input();
        }

        public static void Draw()
        {
            //Plan:
            //Screen will be 344*248, approx. "widescreen SNES" size.
            //this needs a different background color.
            parentRef.BackgroundColor(2);
            parentRef.ScrollPosition(344 * xScreenCoords); //2 screens over.
            DrawDescriptions();
            DrawArrow();
        }

        public static void Input()
        {
            if (parentRef.Button(Buttons.Down, InputState.Released))
            {
                if (phase == 0) //main list
                {
                    arrowPosIndex++;
                    if (arrowPosIndex >= ArrowPoints.Count())
                        arrowPosIndex = 0;
                }
                else if (phase == 1) //stat boost
                {
                    arrowPosIndex+= 4;
                    if (arrowPosIndex >= ArrowPointsStatBoosts.Count())
                        arrowPosIndex = arrowPosIndex - ArrowPointsStatBoosts.Count();
                }
            }

            if (parentRef.Button(Buttons.Up, InputState.Released))
            {
                if (phase == 0) //main list
                {
                    arrowPosIndex--;
                    if (arrowPosIndex < 0)
                        arrowPosIndex = ArrowPoints.Count() - 1;
                }
                else if (phase == 1) //stat boost
                {
                    arrowPosIndex -= 4;
                    if (arrowPosIndex < 0)
                        arrowPosIndex =arrowPosIndex + ArrowPointsStatBoosts.Count();

                }
            }

            if (parentRef.Button(Buttons.Left, InputState.Released))
            {
                if (phase == 0) //main list
                {
                    arrowPosIndex--;
                    if (arrowPosIndex < 0)
                        arrowPosIndex = ArrowPoints.Count() - 1;
                }
                else if (phase == 1) //stat boost
                {
                    arrowPosIndex--;
                    if (arrowPosIndex < 0)
                        arrowPosIndex = ArrowPointsStatBoosts.Count() -1;

                }
            }

            if (parentRef.Button(Buttons.Right, InputState.Released))
            {
                if (phase == 0) //main list
                {
                    arrowPosIndex++;
                    if (arrowPosIndex >= ArrowPoints.Count())
                        arrowPosIndex = 0;
                }
                else if (phase == 1) //stat boost
                {
                    arrowPosIndex++;
                    if (arrowPosIndex >= ArrowPointsStatBoosts.Count())
                        arrowPosIndex = 0;
                }
            }



            if (parentRef.Button(Buttons.B, InputState.Released)) //B is the right button, ACCEPT.
            {
                if (phase == 0) //main list
                {
                    if (arrowPosIndex == 0)
                    {
                        //Level up!
                        gameState.levelingUpChar.level++;
                        gameState.levelingUpChar.currentStats = gameState.levelingUpChar.getTotalStats(true);
                        gameState.levelingUpChar.displayStats.Set(gameState.levelingUpChar.currentStats);
                    }
                    else if (arrowPosIndex == 1)
                    {
                        arrowPosIndex = 0;
                        phase = 1;
                        return; //Don't bounce back yet.
                    }
                    else if (arrowPosIndex == 2)
                    {
                        //Item Roll!
                    }
                    //return to the fight scene.
                    gameState.mode = gameState.FightSceneID;
                }
                else if (phase == 1)
                {
                    //Boost the selected stat
                    switch(arrowPosIndex)
                    {
                        case 0:
                            gameState.levelingUpChar.statBoosts.maxHP+=5;
                            break;
                        case 1:
                            gameState.levelingUpChar.statBoosts.maxAP+=5;
                            break;
                        case 2:
                            gameState.levelingUpChar.statBoosts.STR+=5;
                            break;
                        case 3:
                            gameState.levelingUpChar.statBoosts.DEF+=5;
                            break;
                        case 4:
                            gameState.levelingUpChar.statBoosts.INS+=5;
                            break;
                        case 5:
                            gameState.levelingUpChar.statBoosts.MOX+=5;
                            break;
                        case 6:
                            gameState.levelingUpChar.statBoosts.SPD+=5;
                            break;
                        case 7:
                            gameState.levelingUpChar.statBoosts.LUK+=5;
                            break;
                    }
                    gameState.levelingUpChar.currentStats = gameState.levelingUpChar.getTotalStats(false);
                    gameState.levelingUpChar.displayStats.Set(gameState.levelingUpChar.currentStats);
                    gameState.mode = gameState.FightSceneID;
                }

                //reset for next time.
                arrowPosIndex = 0;
                phase = 0;
            }
        }

        public static void DrawDescriptions()
        {
            int lineCounter = 1;

            parentRef.DrawText("Improvement Choices", 6 * 8, lineCounter * 8, DrawMode.Sprite, "large", 15);
            lineCounter++;
            parentRef.DrawText(gameState.levelingUpChar.name + ":  Level " + gameState.levelingUpChar.level + " " + gameState.levelingUpChar.role.name, 6 * 8, lineCounter * 8, DrawMode.Sprite, "large", 15);
            lineCounter++;

            string currentCharStats = "";
            currentCharStats += "HP:" + gameState.levelingUpChar.currentStats.maxHP + ", ";
            currentCharStats += "AP:" + gameState.levelingUpChar.currentStats.maxAP + ", ";
            currentCharStats += "STR:" + gameState.levelingUpChar.currentStats.STR + ", ";
            currentCharStats += "DEF:" + gameState.levelingUpChar.currentStats.DEF + ", ";
            currentCharStats += "INS:" + gameState.levelingUpChar.currentStats.INS + ", ";
            currentCharStats += "MOX:" + gameState.levelingUpChar.currentStats.MOX + ", ";
            currentCharStats += "SPD:" + gameState.levelingUpChar.currentStats.SPD + ", ";
            currentCharStats += "LUK:" + gameState.levelingUpChar.currentStats.LUK + ", ";
            var wrapped = parentRef.WordWrap(currentCharStats, 30);
            var lines = parentRef.SplitLines(wrapped);
            for (int i = 0; i < lines.Count(); i++)
            {
                parentRef.DrawText(lines[i], 6 * 8, lineCounter * 8, DrawMode.Sprite, "large", 15);
                lineCounter++;
            }
            lineCounter++;

            arrowAdjustment = lineCounter - 7;

            parentRef.DrawText("Level Up", 6 * 8, lineCounter * 8, DrawMode.Sprite, "large", 15);
            lineCounter++;

            string levelUpDesc = "Gain your role's stats and refresh HP and AP\n";
            levelUpDesc += "HP:" + gameState.levelingUpChar.role.statsPerLevel.maxHP + ", ";
            levelUpDesc += "AP:" + gameState.levelingUpChar.role.statsPerLevel.maxAP + ", ";
            levelUpDesc += "STR:" + gameState.levelingUpChar.role.statsPerLevel.STR + ", ";
            levelUpDesc += "DEF:" + gameState.levelingUpChar.role.statsPerLevel.DEF + ", ";
            levelUpDesc += "INS:" + gameState.levelingUpChar.role.statsPerLevel.INS + ", ";
            levelUpDesc += "MOX:" + gameState.levelingUpChar.role.statsPerLevel.MOX + ", ";
            levelUpDesc += "SPD:" + gameState.levelingUpChar.role.statsPerLevel.SPD + ", ";
            levelUpDesc += "LUK:" + gameState.levelingUpChar.role.statsPerLevel.LUK + ", ";
            wrapped = parentRef.WordWrap(levelUpDesc, 30);
            lines = parentRef.SplitLines(wrapped);
            for (int i = 0; i < lines.Count(); i++)
            {
                parentRef.DrawText(lines[i], 6 * 8, lineCounter * 8, DrawMode.Sprite, "large", 15);
                lineCounter++;
            }

            lineCounter++;

            parentRef.DrawText("Stat Boost", 6 * 8, lineCounter * 8, DrawMode.Sprite, "large", 15);
            //lineCounter++;
            string StatBoostDesc = "Increases 1 stat by 5. Does not refresh HP or AP.";
            wrapped = parentRef.WordWrap(StatBoostDesc, 30);
            lines = parentRef.SplitLines(wrapped);
            for (int i = 0; i < lines.Count(); i++)
            {
                lineCounter++;
                parentRef.DrawText(lines[i], 6 * 8, lineCounter * 8, DrawMode.Sprite, "large", 15);
            }

            parentRef.DrawText("  HP     AP    STR    DEF", 6 * 8, lineCounter * 8, DrawMode.Sprite, "large", 15);
            lineCounter++;
            parentRef.DrawText("  INS    MOX   SPD    LUK", 6 * 8, lineCounter * 8, DrawMode.Sprite, "large", 15);
            lineCounter++; lineCounter++;

            parentRef.DrawText("Scavenge for an Item", 6 * 8, lineCounter * 8, DrawMode.Sprite, "large", 15);
            lineCounter++;
            string itemDesc = "Search the area and debris for an item upgrade. Items give 5 stat points per character level, randomly rolled. Will replace existing items in the same slot.";
            wrapped = parentRef.WordWrap(itemDesc, 30);
            lines = parentRef.SplitLines(wrapped);
            for (int i = 0; i < lines.Count(); i++)
            {
                parentRef.DrawText(lines[i], 6 * 8, lineCounter * 8, DrawMode.Sprite, "large", 15);
                lineCounter++;
            }
        }

        public static void DrawArrow()
        {
            if (phase == 0)
                parentRef.DrawMetaSprite("arrow", ArrowPoints[arrowPosIndex].Item1 * 8, ((ArrowPoints[arrowPosIndex].Item2) + arrowAdjustment) * 8);
            else
                parentRef.DrawMetaSprite("arrow", ArrowPointsStatBoosts[arrowPosIndex].Item1 * 8, ((ArrowPointsStatBoosts[arrowPosIndex].Item2)  + arrowAdjustment) * 8);

        }

    }

}
