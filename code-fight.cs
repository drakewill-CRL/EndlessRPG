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
        //ensure rewards occur.
        //make some sound effects and get the list going on which one is what.
        //enemy AI need to exist (even if its randomly pull from list of abilities, including Fight as an ability.)
        //Set role to be a character property, and set stat lists from it as appropriate
        //figure out why MP is burned twice : once at end of turn (probably from combat round), and again during display for the action (and it stays applied?)
        //--i think that means i shouldn't re-process display stats until the end of the display phase?
        //ensure CANCEL button returns you to correct previous menu (dont go to Abilities menu from enemy selection on Fight)
        //rename MP to AP (Ability points)
        //auto-res people after fight with 1 HP if not a total party kill
        //check for party killed too. Haven't yet checked for that.

        public static JrpgRoslynChip parentRef;
        public static List<Character> characters = new List<Character>();
        public static List<Enemy> enemies = new List<Enemy>();

        public static Random random = new Random();

        public static Stats applyToAll = new Stats(); //Possibly where fight-specific gimmicks or spells go.

        // public static string[] menuHelpTexts = new string[5] { //The 5 main menu options
        //     "Melee attack a targeted enemy",
        //     "Use one of your abillities",
        //     "Skip an action to take half damage",
        //     "Flee from this fight and roll another encounter, but gain no XP.",
        //     "Start auto-fight until you hit B to cancel it."
        // };

        //NOTE: might just have to set this up to allow enemy and PC selection in these entries instead. Shoudl skip dead enemies, not dead PCs.
        public static List<Tuple<int, int>> ArrowPoints = new List<Tuple<int, int>>()
        {
            new Tuple<int, int>(1 * 8, 20 * 8), //fight
            new Tuple<int, int>(1 * 8, 22 * 8), //ability
            new Tuple<int, int>(1 * 8, 24 * 8), //defend
            new Tuple<int, int>(1 * 8, 26 * 8), //run
            new Tuple<int, int>(1 * 8, 28 * 8), //auto
        };

        public static List<Tuple<int, int>> charPositions = new List<Tuple<int, int>>()
        {
            new Tuple<int, int>(220, 16),
            new Tuple<int, int>(220, 48),
            new Tuple<int, int>(220, 64),
            new Tuple<int, int>(220, 80)
        };

        static List<Attack> pendingAttacks = new List<Attack>();
        static List<DisplayResults> resultsToParse = new List<DisplayResults>();

        public static int arrowPosIndex = 0; //default to fight
        public static int phase = 0; // 0 = action selection, 1 = watch round play out.
        public static int subMenuLevel = 0; //0 is main menu, 1 is ability menu, 2 is enemy selection, 3 is PC selection.
        public static Attack currentAction = new Attack();
        public static int activeCharSelecting = 0; //Which character we're on for action selection.

        public static int currentAnimationID = 0; //future setup
        public static int currentAnimationFramesLeft = 0; //future setup

        public static bool autoFighting = false; //set to true from Auto button on menu.

        static int displayTime = 120; //# of frames to display each message for.
        static int displayFrameCounter = 0; //the one we tick down
        static string displayResultData = "Checking...";

        public static string helpText = "";
        //TODO: work out a setup for transition animations (EX: characters walking forward or back to their spot)
        //TODO: chrome for frames and all that. Those might be big metasprites

        public static void Init()
        {
            //This gets called manually before entering the fight scene from new game.
            //Test values for layout purposes.
            helpText = GetHelpText(); // menuHelpTexts[0];

            //once its finalized set up a construction for Character that handles all this.
            Character char1 = new Character();
            char1.spriteSet = "char1";
            char1.posX = charPositions[0].Item1;
            char1.posY = charPositions[0].Item2;
            char1.drawState = "Idle";
            char1.name = "Larry";
            char1.role = ContentLists.allRoles[1];
            char1.abilities = char1.role.abilities; // new List<Ability>();
            char1.startingStats = char1.role.startStats;
            char1.StatsPerLevel = char1.role.statsPerLevel;
            char1.currentStats = char1.getTotalStats();
            char1.displayStats = char1.currentStats.Clone();
            characters.Add(char1);
            var char2 = char1.Clone();
            char2.posX = charPositions[1].Item1;
            char2.posY = charPositions[1].Item2;
            characters.Add(char2); //need to re-clone its abilities?

            GetNewEncounter();

            // Character char2 = new Character();
            // char2.spriteSet = "char1";
            // char2.posX = 300;
            // char2.posY = 48;
            // char2.drawState = "Idle";
            // characters.Add(char2);
            // Character char3 = new Character();
            // char3.spriteSet = "char1";
            // char3.posX = 300;
            // char3.posY = 80;
            // char3.drawState = "Idle";
            // characters.Add(char3);
            // Character char4 = new Character();
            // char4.spriteSet = "char1";
            // char4.posX = 300;
            // char4.posY = 112;
            // char4.drawState = "Idle";
            // characters.Add(char4);

            //enemies = ContentLists.PossibleEncounters[0]; //doesnt work?
            //for (int i = 0; i < enemies.Count(); i++)
            //{
            //  enemies[i].posX = 8 + ((i / 2) * 64);
            // enemies[i].posY = 16 + ((i % 2) * 64);
            //}
            // Enemy enemy1 = new Enemy();
            // enemy1.spriteSet = "enemy1";
            // enemy1.name = "Test Target";
            // enemy1.posX = 8;
            // enemy1.posY = 16;
            // enemy1.currentStats = ContentLists.baseStats;
            // enemies.Add(enemy1);
            // Enemy enemy2 = new Enemy();
            // enemy2.spriteSet = "enemy1";
            // enemy2.posX = 8;
            // enemy2.posY = 80;
            // enemies.Add(enemy2);
            // Enemy enemy3 = new Enemy();
            // enemy3.spriteSet = "enemy1";
            // enemy3.posX = 72;
            // enemy3.posY = 16;
            // enemies.Add(enemy3);
            // Enemy enemy4 = new Enemy();
            // enemy4.spriteSet = "enemy1";
            // enemy4.posX = 72;
            // enemy4.posY = 80;
            // enemies.Add(enemy4);
        }

        public static void Update(int timeDelta)
        {
            //check phase, advance animations or input requests.
            //do any math required.

            Input();

            //might want this to be 2 separate functions, 1 for phase0Update and 1 for Phase1Update?
            //Check if all living PCs have an attack registered, and if so change phases
            //if (pendingAttacks.Count() == characters.Count(c => c.CanAct())) //this might be a check on Input, not separate
            //phase = 1;
            //Also check if we're run through all our results, and if so change phases


            //Check if all enemies are dead, if so award 1 xp to all living PCs and roll a new encounter.

            if (phase == 0)
                //TODO: maybe listen for CANCEL button to disable auto-fight
                return; //let other functions do their thing
            if (phase == 1)
            {
                displayFrameCounter--;
                //We have to figure out where we are in the display process
                if (displayFrameCounter == 0)
                {
                    if (resultsToParse.Count() == 0)
                    {
                        //get a new encounter if needed
                        if (enemies.All(e => e.currentStats.HP <= 0))
                            GetNewEncounter();

                        phase = 0;
                        subMenuLevel = 0;
                        activeCharSelecting = 0;
                        arrowPosIndex = 0;
                        pendingAttacks = new List<Attack>();
                        displayResultData = "Checking...";
                        helpText = GetHelpText(); //menuHelpTexts[0];
                        //if (!enemies.Any(e => e.CanAct()))
                        //helpText = "everybody dead, spawn new encounter.";
                    }
                    else
                    {
                        //Get the next thing to display and process.
                        var process = resultsToParse[0];
                        //TODO: swap sprite states as ordered here.
                        displayFrameCounter = process.frameCounter;
                        if (process.target != null)
                            process.target.displayStats.Add(process.changeStats);
                        UpdateSpriteDisplay(process);
                        resultsToParse.Remove(process);
                    }
                }
            }
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

            //debug draws here:
            //parentRef.DrawText("debug", 12 * 8, 12, DrawMode.Sprite, "large", 12);
            // parentRef.DrawText("enemies:" + enemies.Count(), 12 * 8, 12, DrawMode.Sprite, "large", 12);
            // parentRef.DrawText("actable enemies:" + enemies.Count(e => e.CanAct()), 12 * 8, 20, DrawMode.Sprite, "large", 12);

            //test sprites
            foreach (var c in characters)
            {
                parentRef.DrawMetaSprite(c.spriteSet + c.drawState, c.posX, c.posY);
            }

            foreach (var e in enemies)
            {
                parentRef.DrawMetaSprite(e.spriteSet, e.posX, e.posY);
            }
            DrawStatusDisplays(); //Might need to increase the separation between display-data and combat-data further.

            switch (phase)
            {
                case 0: //prep/selection phase
                    if (subMenuLevel == 0)
                        //DrawMenuList();
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
                    //parentRef.DrawText("combat started", 4 * 8, 15 * 8, DrawMode.Sprite, "large", 15);
                    //parentRef.DrawText(displayResultData, 4 * 8, 15 * 8, DrawMode.Sprite, "large", 15);
                    DrawCombatLogText();
                    break;
            }
        }

        public static void Input()
        {
            //TODO: left and right can toggle between enemies and allies on the submenu 2/3 (which makes them the same level)?
            //up and down move arrow, A advances to next command, B rolls back to previous
            //tricky part, the arrow can be on the menu, a submenu, players, or enemies
            //also, update help text when arrow moves to match whatever its on
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
                            //helpText = menuHelpTexts[arrowPosIndex];=
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
                            //if (arrowPosIndex < 4)
                            helpText = enemies[arrowPosIndex].desc;
                            //else
                            //helpText = "All Enemies"; //removed this option, abilities will determine their target range.
                            break;
                        case 3://target PC
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
                            //if (arrowPosIndex < 3)
                            helpText = enemies[arrowPosIndex].desc;
                            //else
                            //helpText = "All Enemies";
                            break;
                        case 3://target PC
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
                                //switch (arrowPosIndex)
                                //{
                                //case 0:
                            if (characters[activeCharSelecting].currentStats.MP >= characters[activeCharSelecting].abilities[arrowPosIndex].mpCost)
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
                                    activeCharSelecting++;
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
                                //TODO: what about abilities that need to target someone? 
                                //Move to the next menu and set current info on the current attack.
                            }
                            else
                            {
                                //TODO: play negative sound, don't advance.
                            }
                            break;

                        //ORIGINAL 
                        //FIGHT, select a target.
                        //currentAction.attacker = characters[activeCharSelecting];
                        //currentAction.thingToDo = ContentLists.allAbilities[1]; //Fight
                        //subMenuLevel = 2;
                        //arrowPosIndex = GetValidEnemyTargets().First();
                        //helpText = enemies[0].desc;
                        //break;
                        //case 1:
                        //Abilities, show list
                        //subMenuLevel = 1;
                        //arrowPosIndex = 0;
                        //helpText = characters[activeCharSelecting].abilities[0].mpCost + "MP: " + characters[activeCharSelecting].abilities[0].description;
                        //break;
                        //case 2:
                        //DEFEND, declare it and move on
                        //break;
                        //case 3:
                        //RUN, declare it and move on
                        //break;
                        //case 4:
                        //AUTO, set auto-fight on for everybody.
                        //break;
                        //}

                        //break;
                        //case 1: //abilities menu
                        //Moved to 0;
                        //break;
                        case 2: //select target enemy and advance to next one.
                                //if (arrowPosIndex < 3)
                            currentAction.targets.Add(enemies[arrowPosIndex]);//selected thing.
                                                                              //else
                                                                              //currentAction.targets.AddRange(enemies); //all things. Should disable this option if the thing is single target? or not let things be choosable between 1 and all?

                            pendingAttacks.Add(currentAction);
                            currentAction = new Attack();
                            activeCharSelecting++;
                            arrowPosIndex = 0;
                            subMenuLevel = 0;
                            if (activeCharSelecting < characters.Count())
                                helpText = characters[activeCharSelecting].name + "'s action";
                            break;
                        case 3: //select target ally
                            currentAction.targets.Add(characters[arrowPosIndex]);//selected thing.
                            pendingAttacks.Add(currentAction);
                            currentAction = new Attack();
                            activeCharSelecting++;
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
                            pendingAttacks.Add(new Attack() { attacker = e, targets = new List<Fightable>() { characters[0] }, thingToDo = ContentLists.allAbilities[0] });
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
                                    activeCharSelecting--;
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

        // public static void DrawMenuList() //Tile draw commands only need done once until they change.
        // {
        //     //TODO: swap this to use the character's role's abilities here.
        //     //This is the default menu list of options and possibly some help text.
        //     //Fight, Ability, Defend, Run (% chance to re-roll the current fight, but you don't get XP for surviving.)

        //     parentRef.DrawText("Fight", 4 * 8, 20 * 8, DrawMode.Sprite, "large", 15); //spacing in tiles, * 8 for pixels.
        //     parentRef.DrawText("Ability", 4 * 8, 22 * 8, DrawMode.Sprite, "large", 15);
        //     parentRef.DrawText("Defend", 4 * 8, 24 * 8, DrawMode.Sprite, "large", 15);
        //     parentRef.DrawText("Run", 4 * 8, 26 * 8, DrawMode.Sprite, "large", 15);
        //     parentRef.DrawText("Auto", 4 * 8, 28 * 8, DrawMode.Sprite, "large", 15);
        // }

        public static void DrawAbilityList(List<Ability> abilities)
        {
            //We should have 4 in this list.
            for (int i = 0; i < abilities.Count(); i++)
            {
                if (characters[activeCharSelecting].abilities[i].mpCost <= characters[activeCharSelecting].currentStats.MP)
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

            //parentRef.DrawText("All", 4 * 8, 28 * 8, DrawMode.Sprite, "large", 15); //spacing in tiles, * 8 for pixels.
        }

        public static void DrawPCList()
        {
            //draw all PCs even dead ones, in case we're trying to res one or buff one after res.
            for (int i = 0; i < characters.Count(); i++)
                parentRef.DrawText(characters[i].name, 4 * 8, (20 + (i * 2)) * 8, DrawMode.Sprite, "large", 15); //spacing in tiles, * 8 for pixels.
        }

        public static void DrawHelpText()
        {
            //The text that shows up on the right side of the bottom panel to show you what the thing does.
            var wrapped = parentRef.WordWrap(helpText, 25);
            var lines = parentRef.SplitLines(wrapped);
            for (int i = 0; i < lines.Count(); i++)
                parentRef.DrawText(lines[i], 16 * 8, (20 + i) * 8, DrawMode.Sprite, "large", 15);
        }

        public static void DrawCombatLogText()
        {
            var wrapped = parentRef.WordWrap(displayResultData, 25);
            var lines = parentRef.SplitLines(wrapped);
            for (int i = 0; i < lines.Count(); i++)
                parentRef.DrawText(lines[i], 6 * 8, (20 + i) * 8, DrawMode.Sprite, "large", 15);
        }

        public static List<int> GetValidEnemyTargets()
        {
            List<int> validOptions = new List<int>();
            for (int i = 0; i < enemies.Count(); i++)
            {
                if (enemies[i].currentStats.HP > 0)
                    validOptions.Add(i);
            }
            //validOptions.Add(4); //All Enemies/Allies.
            return validOptions;
        }

        public static void UpdateSpriteDisplay(DisplayResults dr)
        {
            displayResultData = dr.desc;
            //displayFrameCounter = displayTime;

            switch (dr.changedItem)
            {
                case "spriteState":
                    dr.target.spriteSet = dr.changedTo;
                    break;
                default:
                    break;
            }
        }

        public static void DrawStatusDisplays()
        {
            for (int i = 0; i < characters.Count(); i++)
            {
                parentRef.DrawText(characters[i].name, charPositions[i].Item1 + 32, charPositions[i].Item2, DrawMode.Sprite, "large", 15);
                parentRef.DrawText("HP " + characters[i].displayStats.HP + "/" + characters[i].displayStats.maxHP, charPositions[i].Item1 + 32, charPositions[i].Item2 + 8, DrawMode.Sprite, "large", 15);
                parentRef.DrawText("MP " + characters[i].displayStats.MP + "/" + characters[i].displayStats.maxMP, charPositions[i].Item1 + 32, charPositions[i].Item2 + 16, DrawMode.Sprite, "large", 15);
                //might use the 4th line for status indicators (poisoned, blind, etc)
            }
        }

        public static void GetNewEncounter()
        {
            Console.WriteLine("getting new encounter");
            //TODO: figure out rules for which encounters are valid.
            //TODO: scale encounter to party level. (enemy levels = character average levels)
            var encounter = ContentLists.PossibleEncounters.OrderBy(e => random.Next()).First();
            var partyLevel = characters.Select(c => c.level).Average();
            enemies = new List<Enemy>();
            foreach (var e in encounter)
                enemies.Add(e.Clone());

            for (int i = 0; i < enemies.Count(); i++)
            {
                enemies[i].posX = 8 + ((i / 2) * 64);
                enemies[i].posY = 16 + ((i % 2) * 64);
                //scale to party level
                for (int j = 1; j < partyLevel; j++)
                    enemies[i].currentStats.Add(enemies[i].StatsPerLevel);

                enemies[i].currentStats.HP = enemies[i].currentStats.maxHP;

            }
            helpText = "Another wave of enemies approached!";
            displayFrameCounter = 60;
        }

        public static string GetHelpText()
        {
            if (characters.Count() > 0 && characters[activeCharSelecting] != null)
                return characters[activeCharSelecting].abilities[arrowPosIndex].mpCost + "MP: " + characters[activeCharSelecting].abilities[arrowPosIndex].description; ; //The typical answer.

            return "";
        }
    }


}