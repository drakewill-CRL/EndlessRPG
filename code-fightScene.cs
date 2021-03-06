using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
    public static class FightScene
    {
        //MOST OBVIOUS TODOS:
        //correctly loop gameplay after death 
        //baseline sample content (char abilities in place)
        //--PC SPRITES for 3 of 4 classes pending
        //--5 enemies pending (abilities and sprites)
        //BOSSES are missing, clearly required. 
        //--1 for test.
        //balance, get to 20 fights and the boss there while being interesting and requiring some thought
        //Update title screen image, ponder alternative names. Gatehold?

        //fixes: 
        //make dead characters stay dead, dont change their display state.
        //Larry could fight at 0 HP (skip dead chars on turn start when setting menu data.)

        //non-MVP
        //make some sound effects and get the list going on which one is what. Expand on the 3 present.
        //Assign abilities from abilitiesByName dictionary so I don't have to worry about id/order.
        //clean up some code to make things easier/faster/cleaner later?
        //Make SELECT toggle auto-fight. Auto-fight rules will be simple AI for now, will expand on later with additional AI logic.       
        //I wanted abilities to leveup up at some point. What happened to that?
        //make functions to return a list of DisplayResults to add to the existing list to allow for more customizable/reusable animations sets


        //V 0.01 requirements: - the MVP POC
        //4 starter classes
        //-- abilities and sprites in place
        //NewGame screen to pick characters and roles for each slot. --DONE
        //Game ends after 20 fights (1 boss, PC level ~5) - could use this as a progress checkpoint to resume from next time. Would be level 5 starting this way.
        //5 enemies present and sprited in. somewhat varied in stats and abilities.
        //10 encounters made out of X enemy combos.
        //1 boss encounter (will occur once, 20 fights in)
        //test stuff absent from normal loop - DONE
        //better title screen image.
        

        public static JrpgRoslynChip parentRef;
        public static List<Character> characters = new List<Character>();
        public static List<Enemy> enemies = new List<Enemy>();

        public static bool hasDrawn = false;
        static int xScreennCoords = 1;
        static int yScreenCoords = 0;

        public static int fightsWon = 0;

        public static Stats applyToAll = new Stats(); //Possibly where fight-specific gimmicks or spells go.

        public static List<Tuple<int, int>> ArrowPoints = new List<Tuple<int, int>>()
        {
            new Tuple<int, int>(1 * 8, 20 * 8), 
            new Tuple<int, int>(1 * 8, 22 * 8), 
            new Tuple<int, int>(1 * 8, 24 * 8), 
            new Tuple<int, int>(1 * 8, 26 * 8), 
            new Tuple<int, int>(1 * 8, 28 * 8), 
        };

        public static List<Tuple<int, int>> charPositions = new List<Tuple<int, int>>()
        {
            new Tuple<int, int>(204, 16),
            new Tuple<int, int>(204, 48),
            new Tuple<int, int>(204, 80),
            new Tuple<int, int>(204, 112)
        };

        static List<Attack> pendingAttacks = new List<Attack>();
        static List<DisplayResults> resultsToParse = new List<DisplayResults>();

        public static int arrowPosIndex = 0; //default to fight
        public static int phase = 0; // 0 = action selection, 1 = watch round play out, 2 = game over.
        public static int subMenuLevel = 0; //0 is main menu, 1 is ability menu, 2 is enemy selection, 3 is PC selection.
        public static Attack currentAction = new Attack();
        public static int activeCharSelecting = 0; //Which character we're on for action selection.

        public static int currentAnimationID = 0; //future setup
        public static int currentAnimationFramesLeft = 0; //future setup

        public static bool autoFighting = false; //set to true from Auto button on menu.

        //static int displayTime = 120; //# of frames to display each message for.
        static int displayFrameCounter = 0; //the one we tick down
        static string displayResultData = "Checking...";

        public static string helpText = "";

        public static void Init()
        {
            //This gets called manually before entering the fight scene from new game.
            //Test values for layout purposes.
            helpText = GetHelpText(); // menuHelpTexts[0];

            //set up draw positions for characters
            characters[0].posX = charPositions[0].Item1;
            characters[0].posY = charPositions[0].Item2;
            characters[1].posX = charPositions[1].Item1;
            characters[1].posY = charPositions[1].Item2;
            characters[2].posX = charPositions[2].Item1;
            characters[2].posY = charPositions[2].Item2;
            characters[3].posX = charPositions[3].Item1;
            characters[3].posY = charPositions[3].Item2;

            for (int i = 0; i < enemies.Count(); i++)
            {
                enemies[i].posX = 8 + ((i / 2) * 64);
                enemies[i].posY = 16 + ((i % 2) * 64);
                if (enemies[i].currentStats.HP <= 0)
                    enemies[i].spriteSet = "Dead";
            }

            //old test code.
            // //once its finalized set up a construction for Character that handles all this.
            // Character char1 = new Character(ContentLists.allRoles[1]);
            // char1.posX = charPositions[0].Item1;
            // char1.posY = charPositions[0].Item2;
            // //char1.drawState = "";
            // char1.name = "Larry";
            // char1.role = ContentLists.allRoles[1];
            // char1.abilities = char1.role.abilities; 
            // char1.startingStats = char1.role.startStats;
            // char1.StatsPerLevel = char1.role.statsPerLevel;
            // char1.currentStats = char1.getTotalStats(true);
            // char1.displayStats = char1.currentStats.Clone();
            // characters.Add(char1);
            // var char2 = char1.Clone();
            // char2.name = "Gary";
            // char2.posX = charPositions[1].Item1;
            // char2.posY = charPositions[1].Item2;
            // characters.Add(char2);
            if (activeCharSelecting < characters.Count())  characters[activeCharSelecting].drawState = "Ready";

            //GetNewEncounter();

        }

        public static void Update(int timeDelta)
        {
            //check phase, advance animations or input requests.
            //do any math required.

            Input();

            //might want this to be 2 separate functions, 1 for phase0Update and 1 for Phase1Update?
            //Check if all enemies are dead, if so award 1 xp to all living PCs and roll a new encounter.

            if (phase == 0) //User input to select their attacks
                //TODO: maybe listen for CANCEL button to disable auto-fight
                return; //let other functions do their thing
            if (phase == 1) //Report out the resutls of the fight and animate things.
            {
                displayFrameCounter--;
                //We have to figure out where we are in the display process
                if (displayFrameCounter == 0) //TODO: if I make this a while loop, i can process all results with a frameCounter of 0 before displaying the next one.
                {
                    if (resultsToParse.Count() == 0)
                    {
                        if (characters.All(c => !c.CanAct()))
                        {
                            //Game Over logic.
                            displayResultData = "Game Over";
                            displayFrameCounter = 600;
                            //play sad song.
                            //update save game data
                            parentRef.SaveGameData();
                            parentRef.WriteSaveData("gameActive", "0"); //explicitly set the save file not to use these characters
                            phase = 2;
                            return;
                        }
                        //get a new encounter if needed
                        if (enemies.All(e => e.currentStats.HP <= 0))
                        {
                            GetNewEncounter();
                        }
                        phase = 0;
                        subMenuLevel = 0;
                        activeCharSelecting = 0;
                        foreach (var c in characters) //Update progress data
                        {
                            c.displayStats.Set(c.currentStats);
                            if (gameState.bestLevels[c.role.name] < c.level)
                            {
                                gameState.bestLevels.Remove(c.role.name);
                                gameState.bestLevels.Add(c.role.name, c.level);
                            }
                        }
                        if (activeCharSelecting < characters.Count())  characters[activeCharSelecting].drawState = "Ready";
                        arrowPosIndex = 0;
                        pendingAttacks = new List<Attack>();
                        displayResultData = "Checking...";
                        helpText = GetHelpText();
                        parentRef.SaveGameData();
                    }
                    else
                    {
                        //Get the next thing to display and process.
                        var process = resultsToParse[0];
                        displayFrameCounter = process.frameCounter;
                        if (process.target != null)
                            process.target.displayStats.Add(process.changeStats);
                        UpdateSpriteDisplay(process);
                        resultsToParse.Remove(process);
                    }
                }
            }
            if (phase == 2) //game over displayed, return to title screen.
            {
                DrawCombatLogText();
                displayFrameCounter--;
                if (displayFrameCounter == 0)
                {
                    gameState.mode = gameState.TitleSceneID;
                }
            }
        }

        public static void Draw()
        {
            //Plan:
            //Screen will be 344*248, approx. "widescreen SNES" size.
            //Sprites should be 16*32, SNES colors per sprite? that's 128 of 244 vert pixels, leaving 96 for menus/chrome/etc in a single vertical line.
            //Might make these 32x32, with some empty space to allow for dead/ready sprites to be wider than then standing ones.
            //enemies are 64x64
            //debug draws here:
            //parentRef.DrawText("debug", 12 * 8, 12, DrawMode.Sprite, "large", 12);
            // parentRef.DrawText("enemies:" + enemies.Count(), 12 * 8, 12, DrawMode.Sprite, "large", 12);
            // parentRef.DrawText("actable enemies:" + enemies.Count(e => e.CanAct()), 12 * 8, 20, DrawMode.Sprite, "large", 12);

            parentRef.BackgroundColor(0);
            parentRef.ScrollPosition(344 * xScreennCoords);
            if (!hasDrawn)
                DrawMenuChrome();

            //test sprites
            foreach (var c in characters)
            {
                parentRef.DrawMetaSprite(c.role.spriteSet + c.drawState, c.posX, c.posY, false, false, DrawMode.Sprite, 0);
            }

            foreach (var e in enemies)
            {
                parentRef.DrawMetaSprite(e.spriteSet + e.drawState, e.posX, e.posY, false, false, DrawMode.Sprite, e.colorShift);
            }
            DrawStatusDisplays();

            switch (phase)
            {
                case 0: //prep/selection phase
                    if (subMenuLevel == 0)
                        DrawAbilityList(characters[activeCharSelecting].abilities);
                    //else if (subMenuLevel == 1)
                    //DrawAbilityList(characters[activeCharSelecting].abilities);
                    else if (subMenuLevel == 2)
                        DrawEnemyList();
                    else if (subMenuLevel == 3)
                        DrawPCList();

                    DrawHelpText();

                    parentRef.DrawMetaSprite("arrow", ArrowPoints[arrowPosIndex].Item1, ArrowPoints[arrowPosIndex].Item2);
                    break;
                case 1: //display results/actions
                    DrawCombatLogText();
                    break;
            }
        }

        public static void Input()
        {
            //TODO: left and right can toggle between enemies and allies on the submenu 2/3 (which makes them the same level)?
            //up and down move arrow, A advances to next command, B rolls back to previous
            //tricky part, the arrow can be on the menu, a submenu, players, or enemies
            if (parentRef.Button(Buttons.Up, InputState.Released))
            {
                if (phase == 0) //selection phase
                {
                    switch (subMenuLevel)
                    {
                        case 0: //main menu
                            arrowPosIndex--;
                            if (arrowPosIndex < 0)
                                arrowPosIndex = ArrowPoints.Count() - 1;
                            helpText = GetHelpText();
                            break;
                        // case 1://ability menu
                        //     arrowPosIndex--;
                        //     if (arrowPosIndex < 0)
                        //         arrowPosIndex = characters[activeCharSelecting].abilities.Count() - 1;
                        //     helpText = characters[activeCharSelecting].abilities[arrowPosIndex].mpCost + "MP: " + characters[activeCharSelecting].abilities[arrowPosIndex].description;
                        //     break;
                        case 2: //target enemy. Skip dead enemies.
                            var currentIndex = arrowPosIndex;
                            List<int> validOptions = GetValidEnemyTargets();
                            arrowPosIndex = validOptions.FirstOrDefault(o => o < arrowPosIndex); //Should be 0 automatically if on the last entry.
                            if (arrowPosIndex == 0 && (currentIndex == 0 || enemies[0].currentStats.HP == 0)) //We were already on the first entry, and got 0 as the OrDefault value. OR ITS DEAD
                                arrowPosIndex = GetValidEnemyTargets().LastOrDefault();
                            helpText = enemies[arrowPosIndex].desc;
                            break;
                        case 3://target PC
                            arrowPosIndex--;
                            if (arrowPosIndex < 0)
                                arrowPosIndex = characters.Count() - 1;
                            helpText = characters[arrowPosIndex].role.name;
                            break;
                    }
                    parentRef.PlaySound(0);
                }
            }
            if (parentRef.Button(Buttons.Down, InputState.Released))
            {
                if (phase == 0) //selection phase
                {
                    switch (subMenuLevel)
                    {
                        case 0: //main menu
                            arrowPosIndex++;
                            if (arrowPosIndex >= ArrowPoints.Count())
                                arrowPosIndex = 0;
                            helpText = GetHelpText();
                            break;
                        // case 1://ability menu
                        //     arrowPosIndex++;
                        //     if (arrowPosIndex >= characters[activeCharSelecting].abilities.Count())
                        //         arrowPosIndex = 0;
                        //     helpText = characters[activeCharSelecting].abilities[arrowPosIndex].mpCost + "MP: " + characters[activeCharSelecting].abilities[arrowPosIndex].description;
                        //     break;
                        case 2: //target enemy
                            List<int> validOptions = GetValidEnemyTargets();
                            arrowPosIndex = validOptions.FirstOrDefault(o => o > arrowPosIndex); //Should be 0 automatically if on the last entry.
                            if (arrowPosIndex == 0)
                                arrowPosIndex = validOptions.First();
                            helpText = enemies[arrowPosIndex].desc;
                            break;
                        case 3://target PC
                        arrowPosIndex++;
                            if (arrowPosIndex >= characters.Count())
                                arrowPosIndex = 0;
                            helpText = characters[arrowPosIndex].role.name;
                            break;
                    }
                    parentRef.PlaySound(0);
                }
            }
            if (parentRef.Button(Buttons.B, InputState.Released)) //This is the button on the RIGHT, so it's ACCEPT (or NES button A)
            {
                if (phase == 0) //selection phase
                {
                    //Check which menu we're in, then which index we're at.
                    switch (subMenuLevel)
                    {
                        case 0: //main menu
                            if (characters[activeCharSelecting].currentStats.AP >= characters[activeCharSelecting].abilities[arrowPosIndex].apCost)
                            {
                                var tLevel = characters[activeCharSelecting].abilities[arrowPosIndex].targetType;
                                if (tLevel == 0)//self or auto-target
                                {
                                    //TODO: functionalize this, cause it's gonna get called a lot?
                                    currentAction.attacker = characters[activeCharSelecting];
                                    currentAction.targets.Add(characters[activeCharSelecting]);
                                    currentAction.thingToDo = characters[activeCharSelecting].abilities[arrowPosIndex];
                                    pendingAttacks.Add(currentAction);
                                    currentAction = new Attack();
                                    characters[activeCharSelecting].drawState = "";
                                    activeCharSelecting = SetNextActivePC();
                                    if (activeCharSelecting < characters.Count())  characters[activeCharSelecting].drawState = "Ready";
                                    arrowPosIndex = 0;
                                    subMenuLevel = 0;
                                    if (activeCharSelecting < characters.Count())
                                        helpText = characters[activeCharSelecting].name + "'s action";
                                }
                                else if (tLevel == 2) //target enemy
                                {
                                    currentAction.attacker = characters[activeCharSelecting];
                                    currentAction.thingToDo = characters[activeCharSelecting].abilities[arrowPosIndex];
                                    arrowPosIndex = GetValidEnemyTargets().FirstOrDefault();
                                    helpText = enemies[arrowPosIndex].desc;
                                    subMenuLevel = 2;
                                }
                                else if (tLevel == 3) // target ally.
                                {
                                    currentAction.attacker = characters[activeCharSelecting];
                                    currentAction.thingToDo = characters[activeCharSelecting].abilities[arrowPosIndex];
                                    arrowPosIndex = 0;
                                    helpText = characters[arrowPosIndex].role.name;
                                    subMenuLevel = 3;
                                }
                                else if (tLevel == 4) // All enemies
                                {
                                    currentAction.attacker = characters[activeCharSelecting];
                                    currentAction.targets.AddRange(enemies);
                                    currentAction.thingToDo = characters[activeCharSelecting].abilities[arrowPosIndex];
                                    pendingAttacks.Add(currentAction);
                                    currentAction = new Attack();
                                    characters[activeCharSelecting].drawState = "";
                                    activeCharSelecting = SetNextActivePC();
                                    if (activeCharSelecting < characters.Count())  characters[activeCharSelecting].drawState = "Ready";
                                    arrowPosIndex = 0;
                                    subMenuLevel = 0;
                                }
                                else if (tLevel == 5) // All allies
                                {
                                    currentAction.attacker = characters[activeCharSelecting];
                                    currentAction.targets.AddRange(characters);
                                    currentAction.thingToDo = characters[activeCharSelecting].abilities[arrowPosIndex];
                                    pendingAttacks.Add(currentAction);
                                    currentAction = new Attack();
                                    characters[activeCharSelecting].drawState = "";
                                    activeCharSelecting = SetNextActivePC();
                                    if (activeCharSelecting < characters.Count())  characters[activeCharSelecting].drawState = "Ready";
                                    arrowPosIndex = 0;
                                    subMenuLevel = 0;
                                }
                            }
                            else
                            {
                                //TODO: play negative sound, don't advance.
                                parentRef.PlaySound(2);
                            }
                            break;
                        case 2: //select target enemy and advance to next one.
                            currentAction.targets.Add(enemies[arrowPosIndex]);//selected thing.
                            pendingAttacks.Add(currentAction);
                            currentAction = new Attack();
                            characters[activeCharSelecting].drawState = "";
                            activeCharSelecting = SetNextActivePC();
                            if (activeCharSelecting < characters.Count())  characters[activeCharSelecting].drawState = "Ready";
                            arrowPosIndex = 0;
                            subMenuLevel = 0;
                            if (activeCharSelecting < characters.Count())
                                helpText = characters[activeCharSelecting].name + "'s action";
                            break;
                        case 3: //select target ally
                            currentAction.targets.Add(characters[arrowPosIndex]);//selected thing.
                            pendingAttacks.Add(currentAction);
                            currentAction = new Attack();
                            characters[activeCharSelecting].drawState = "";
                            activeCharSelecting = SetNextActivePC();
                            if (activeCharSelecting < characters.Count())  characters[activeCharSelecting].drawState = "Ready";
                            arrowPosIndex = 0;
                            subMenuLevel = 0;
                            if (activeCharSelecting < characters.Count())
                                helpText = characters[activeCharSelecting].name + "'s action";
                            break;
                    }
                    if (pendingAttacks.Count() >= characters.Count(c => c.CanAct()))
                    {
                        phase = 1; //Switch to results mode.
                        //Set display stats to the currentStats values now, then tick those down during the phase.
                        foreach (var c in characters)
                            c.displayStats.Set(c.currentStats);
                        //foreach(var e in enemies) //Not currently relevant but could be in the future.
                        //e.displayStats = e.currentStats;

                        //and run enemy attacks, which TODO should go through some AI in the future.
                        foreach (var e in enemies.Where(e => e.CanAct()))
                            pendingAttacks.Add(AI.PickRandom(e));
                        //pass attacks to combat engine to determine results
                        displayFrameCounter = 1;
                        resultsToParse = CombatEngine.ProcessRound(pendingAttacks);
                    }
                    parentRef.PlaySound(0);
                }
            }
            if (parentRef.Button(Buttons.A, InputState.Released)) //the button on the LEFT, CANCEL (NES button B)
            {
                if (phase == 0) //selection phase
                {
                    //Check which menu we're in, then which index we're at.
                    switch (subMenuLevel)
                    {
                        case 0: //main menu
                                //rollback to the previous character.
                                if(activeCharSelecting > 0)
                                {
                                    pendingAttacks.Remove(pendingAttacks.Last());
                                    characters[activeCharSelecting].drawState = "";
                                    activeCharSelecting = SetPrevActivePC();
                                    if (activeCharSelecting < characters.Count())  characters[activeCharSelecting].drawState = "Ready";
                                    arrowPosIndex = 0;
                                    helpText = "Rewinding to " + characters[activeCharSelecting].name + "'s action";
                                }
                                //play BZZT/backwards sound.

                            break;
                        //case 1: //abilities menu
                        //subMenuLevel = 0;
                        //arrowPosIndex = 0;
                        //helpText = menuHelpTexts[0];
                        //break;
                        case 2: //select target enemy
                            subMenuLevel = 0;
                            break;
                        case 3: //select target ally
                            subMenuLevel = 0;
                            break;
                    }
                    parentRef.PlaySound(0);
                }
            }
        }

        public static void DrawAbilityList(List<Ability> abilities)
        {
            //We should have 4 in this list.
            for (int i = 0; i < abilities.Count(); i++)
            {
                if (characters[activeCharSelecting].abilities[i].apCost <= characters[activeCharSelecting].currentStats.AP)
                    parentRef.DrawText(abilities[i].name, 2 * 8, (20 + (i * 2)) * 8, DrawMode.Sprite, "large", 15); //spacing in tiles, * 8 for pixels.
                else
                    parentRef.DrawText(abilities[i].name, 2 * 8, (20 + (i * 2)) * 8, DrawMode.Sprite, "large", 12);
            }
        }

        public static void DrawEnemyList()
        {
            //skip dead enemies, leave their slot blank.
            for (int i = 0; i < enemies.Count(); i++)
                if (enemies[i].currentStats.HP > 0) //skip empty spaces for dead enemies.
                    parentRef.DrawText(enemies[i].name, 2 * 8, (20 + (i * 2)) * 8, DrawMode.Sprite, "large", 15); //spacing in tiles, * 8 for pixels.
        }

        public static void DrawPCList()
        {
            //draw all PCs even dead ones, in case we're trying to res one or buff one after res.
            for (int i = 0; i < characters.Count(); i++)
                parentRef.DrawText(characters[i].name, 4 * 8, (20 + (i * 2)) * 8, DrawMode.Sprite, "large", 15); //spacing in tiles, * 8 for pixels.
        }

        public static void DrawHelpText()
        {
            parentRef.WrapText(helpText, 26, 16 * 8, 20);
            //The text that shows up on the right side of the bottom panel to show you what the thing does.
            //var wrapped = parentRef.WordWrap(helpText, 26);
            //var lines = parentRef.SplitLines(wrapped);
            //for (int i = 0; i < lines.Count(); i++)
                //parentRef.DrawText(lines[i], 16 * 8, (20 + i) * 8, DrawMode.Sprite, "large", 15);
        }

        public static void DrawCombatLogText()
        {
            parentRef.WrapText(displayResultData, 26,  16 * 8, 20);
            // var wrapped = parentRef.WordWrap(displayResultData, 26);
            // var lines = parentRef.SplitLines(wrapped);
            // for (int i = 0; i < lines.Count(); i++)
            //     parentRef.DrawText(lines[i], 16 * 8, (20 + i) * 8, DrawMode.Sprite, "large", 15);
        }

        public static List<int> GetValidEnemyTargets()
        {
            List<int> validOptions = new List<int>();
            for (int i = 0; i < enemies.Count(); i++)
            {
                if (enemies[i].currentStats.HP > 0)
                    validOptions.Add(i);
            }
            return validOptions;
        }

        public static List<int> GetLivingPCs()
        {
            List<int> validOptions = new List<int>();
            for (int i = 0; i < characters.Count(); i++)
            {
                if (characters[i].currentStats.HP > 0)
                    validOptions.Add(i);
            }
            return validOptions;
        }

        //TODO: this needs a better name since it does more than just sets sprites.
        public static void UpdateSpriteDisplay(DisplayResults dr)
        {
            displayResultData = dr.desc;
            //displayFrameCounter = displayTime;

            switch (dr.changedItem)
            {
                case "spriteState":
                    dr.target.drawState = dr.changedTo;
                    break;
                case "colorShift":
                    dr.target.colorShift = Int32.Parse(dr.changedTo);
                    break;
                case "fightWon":
                    fightsWon++;
                    if (fightsWon > gameState.bestFightsWon)
                        gameState.bestFightsWon = fightsWon;
                    break;
                default:
                    break;
            }
            if (dr.isLevelUp)
            {
                gameState.levelingUpChar = (Character)dr.target;
                gameState.mode = gameState.ImproveSceneID;
            }
        }

        public static void DrawStatusDisplays()
        {
            for (int i = 0; i < characters.Count(); i++)
            {
                parentRef.DrawText(characters[i].name, charPositions[i].Item1 + 48, charPositions[i].Item2, DrawMode.Sprite, "large", 15);
                parentRef.DrawText("HP " + characters[i].displayStats.HP + "/" + characters[i].displayStats.maxHP, charPositions[i].Item1 + 48, charPositions[i].Item2 + 8, DrawMode.Sprite, "large", 15);
                parentRef.DrawText("AP " + characters[i].displayStats.AP + "/" + characters[i].displayStats.maxAP, charPositions[i].Item1 + 48, charPositions[i].Item2 + 16, DrawMode.Sprite, "large", 15);
                //might use the 4th line for status indicators (poisoned, blind, etc)
                if (characters[i].drawState == "Dead")
                    parentRef.DrawText("Dead", charPositions[i].Item1 + 48, charPositions[i].Item2 + 24, DrawMode.Sprite, "large", 15);
            }
            parentRef.DrawText("Wins: " + fightsWon, 200, 0, DrawMode.Sprite, "large", 15);
        }

        public static void GetNewEncounter()
        {
            bool isBossFight = (fightsWon > 0 && fightsWon % 19 == 0) ; //Fight 20 is a boss fight

            List<Enemy> encounter;
            if (!isBossFight) 
            {
                encounter = ContentLists.PossibleEncounters.OrderBy(e => gameState.random.Next()).First();
                helpText = "Another wave of enemies approached!";
            }
            else
            {
                encounter = ContentLists.PossibleBossEncounters.OrderBy(e => gameState.random.Next()).First();
                helpText = "A very dangerous enemy appears!";
                //TODO: play danger sound effect/boss music?
            }
            
            var partyLevel = characters.Select(c => c.level).Average();
            enemies = new List<Enemy>();
            foreach (var e in encounter)
                enemies.Add(e.Clone());

            for (int i = 0; i < enemies.Count(); i++)
            {
                enemies[i].posX = 8 + ((i / 2) * 64);
                enemies[i].posY = 16 + ((i % 2) * 64);
                //scale to party level
                enemies[i].currentStats.Set(enemies[i].startingStats);
                for (int j = 1; j < partyLevel; j++)
                    enemies[i].currentStats.Add(enemies[i].StatsPerLevel);

                enemies[i].currentStats.HP = enemies[i].currentStats.maxHP;
            }
            
            displayFrameCounter = 60;
        }

        public static string GetHelpText()
        {
            if (characters.Count() > 0 && characters[activeCharSelecting] != null)
                return characters[activeCharSelecting].abilities[arrowPosIndex].apCost + " AP: " + characters[activeCharSelecting].abilities[arrowPosIndex].description; ; //The typical answer.

            return "";
        }

        //This could be replaced with drawing sprites directly in the tilemap, though that requires moving some sprite data to the Sprites.png file
        //and then working with the tilemap editor.
        public static void DrawMenuChrome()
        {
            //Draw in some sprites to outline areas and boxes and such.
            var tileOffsetX = 43 * xScreennCoords;
            var tileOffsetY = 31 * yScreenCoords;

            //Draw vertical separators

            for(int i =2; i < 18; i++)
                parentRef.DrawMetaSprite("pipeVert", 0 + tileOffsetX, i + tileOffsetY, false, false, DrawMode.Tile, 0);
            
            for(int i =20; i < 30; i++)
                parentRef.DrawMetaSprite("pipeVert", 0 + tileOffsetX, i + tileOffsetY, false, false, DrawMode.Tile, 0);

            parentRef.DrawMetaSprite("pipeNWcorner", 0 + tileOffsetX, 1 + tileOffsetY, false, false, DrawMode.Tile, 0);
            parentRef.DrawMetaSprite("pipeSWcorner", 0 + tileOffsetX, 18 + tileOffsetY, false, false, DrawMode.Tile, 0);
            parentRef.DrawMetaSprite("pipeNWcorner", 0 + tileOffsetX, 19 + tileOffsetY, false, false, DrawMode.Tile, 0);
            parentRef.DrawMetaSprite("pipeSWcorner", 0 + tileOffsetX, 30 + tileOffsetY, false, false, DrawMode.Tile, 0);

            parentRef.DrawMetaSprite("pipeNorthT", 15 + tileOffsetX, 19 + tileOffsetY, false, false, DrawMode.Tile, 0);
            parentRef.DrawMetaSprite("pipeSouthT", 15 + tileOffsetX, 30 + tileOffsetY, false, false, DrawMode.Tile, 0);

            for(int i =20; i < 30; i++)
                parentRef.DrawMetaSprite("pipeVert", 15 + tileOffsetX, i + tileOffsetY, false, false, DrawMode.Tile, 0);

            for(int i =20; i < 30; i++)
                parentRef.DrawMetaSprite("pipeVert", 42 + tileOffsetX, i + tileOffsetY, false, false, DrawMode.Tile, 0);

            parentRef.DrawMetaSprite("pipeNEcorner", 42 + tileOffsetX, 19 + tileOffsetY, false, false, DrawMode.Tile, 0);
            parentRef.DrawMetaSprite("pipeSEcorner", 42 + tileOffsetX, 30 + tileOffsetY, false, false, DrawMode.Tile, 0);
            parentRef.DrawMetaSprite("pipeNEcorner", 42 + tileOffsetX, 1 + tileOffsetY, false, false, DrawMode.Tile, 0);
            parentRef.DrawMetaSprite("pipeSEcorner", 42 + tileOffsetX, 18 + tileOffsetY, false, false, DrawMode.Tile, 0);

            for(int i =2; i < 18; i++)
                parentRef.DrawMetaSprite("pipeVert", 42 + tileOffsetX, i + tileOffsetY, false, false, DrawMode.Tile, 0);

            for(int i =1; i < 42; i++)
                parentRef.DrawMetaSprite("pipeHors", i + tileOffsetX, 1 + tileOffsetY, false, false, DrawMode.Tile, 0);

            for(int i =1; i < 15; i++)
                parentRef.DrawMetaSprite("pipeHors", i + tileOffsetX, 30 + tileOffsetY, false, false, DrawMode.Tile, 0);

            for(int i =16; i < 42; i++)
                parentRef.DrawMetaSprite("pipeHors", i + tileOffsetX, 30 + tileOffsetY, false, false, DrawMode.Tile, 0);

            for(int i =1; i < 42; i++)
                parentRef.DrawMetaSprite("pipeHors", i + tileOffsetX, 18 + tileOffsetY, false, false, DrawMode.Tile, 0);

            for(int i =1; i < 15; i++)
                parentRef.DrawMetaSprite("pipeHors", i + tileOffsetX, 19 + tileOffsetY, false, false, DrawMode.Tile, 0);

            for(int i =16; i < 42; i++)
                parentRef.DrawMetaSprite("pipeHors", i + tileOffsetX, 19 + tileOffsetY, false, false, DrawMode.Tile, 0);
        }

        //To be called when restarting the scene.
        public static void ClearScene()
        {
            characters = new List<Character>();
            enemies= new List<Enemy>();
            fightsWon = 0;
            arrowPosIndex = 0;
            phase = 0;
            
        }

        public static int SetNextActivePC()
        {
            var choices = GetLivingPCs();
            var next = choices.FirstOrDefault(c => c > activeCharSelecting);
            if (next == 0) //nothing was larger than current entry
                next = choices.FirstOrDefault(); //next == 0 here means the first PC is the correct next entry

            return next;
        }
        public static int SetPrevActivePC()
        {
            int startingPoint = activeCharSelecting;
            var choices = GetLivingPCs();
            var next = choices.LastOrDefault(c => c < activeCharSelecting);
            if (next == 0 && startingPoint == choices.FirstOrDefault()) 
                next = choices.LastOrDefault(); //next == 0 here means the first PC is the correct next entry

            return next;
        }
    }
}