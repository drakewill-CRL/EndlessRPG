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
        static int xScreenCoords = 3; // * 43 for X screens over
        static int yScreenCoords = 0; // * 31 for Y screens down.

        static int char1RoleId = 0; //ID of role in gameState.unlockedroles
        static int char2RoleId = 1; //ID of role in gameState.unlockedroles
        static int char3RoleId = 2; //ID of role in gameState.unlockedroles
        static int char4RoleId = 3; //ID of role in gameState.unlockedroles
        static int activeCharSelecting = 0;
        static int activeSpotSelection = 0; //class, name, other things?

        //POints are referred to for all chars, but they use the offsets to shift their display position.
        static Point char1Portrait = new Point(32, 32);
        static Point char1DescText = new Point(64, 32); //not sure how wide this can be yet
        static Point char1RoleText = new Point(32, 24); //Above the portrait, displays role name
        static Point char1NameText = new Point(32, 16); //Above the role name

        //offset in tiles. TODO: multiply by 8 for pixels here instead of on each draw and work out how to fix that for the other draws?
        static int[] charXoffsets = new int[] { 0, 20, 0, 20 };
        static int[] charYoffsets = new int[] { 0, 0, 16, 16 };
        
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
            //I want space for 
            //4 sprites to display selected role sprite
            //small descriptions of the selected role next to each portrait
            //possibly some text for highest level per role/all roles total.
            //Switch sprite to Ready once a character is filled in.
            parentRef.BackgroundColor(0);
            parentRef.ScrollPosition(344 * xScreenCoords);
            parentRef.CenterText("Select Your Strike Team", 0);

            //Looping test, layout looks ok.
            //for (int i = 0; i < 4; i++)
            //{
                // parentRef.DrawMetaSprite(ContentLists.rolesByName[gameState.unlockedRoles[char1RoleId]].spriteSet, char1Portrait.X + (charXoffsets[i] * 8), char1Portrait.Y + (charYoffsets[i] * 8));
                // parentRef.DrawText(gameState.Char1Name, (2 + charXoffsets[i]) * 8, (2 + charYoffsets[i]) * 8, DrawMode.Sprite, "large", 15);
                // parentRef.DrawText(gameState.unlockedRoles[char1RoleId], (2 + charXoffsets[i]) * 8, (3 + charYoffsets[i]) * 8, DrawMode.Sprite, "large", 15);
                // parentRef.WrapText(ContentLists.rolesByName[gameState.unlockedRoles[char1RoleId]].desc, 15, (2 + charXoffsets[i]) * 8, (8 + charYoffsets[i]));
            //}

            //char1
            parentRef.DrawMetaSprite(ContentLists.rolesByName[gameState.unlockedRoles[char1RoleId]].spriteSet, char1Portrait.X + (charXoffsets[0] * 8), char1Portrait.Y + (charYoffsets[0] * 8));
            parentRef.DrawText(gameState.Char1Name, (2 + charXoffsets[0]) * 8, (2 + charYoffsets[0]) * 8, DrawMode.Sprite, "large", 15);
            parentRef.DrawText(gameState.unlockedRoles[char1RoleId], (2 + charXoffsets[0]) * 8, (3 + charYoffsets[0]) * 8, DrawMode.Sprite, "large", 15);
            parentRef.WrapText(ContentLists.rolesByName[gameState.unlockedRoles[char1RoleId]].desc, 15, (2 + charXoffsets[0]) * 8, (8 + charYoffsets[0]));

            //char2
            parentRef.DrawMetaSprite(ContentLists.rolesByName[gameState.unlockedRoles[char2RoleId]].spriteSet, char1Portrait.X + (charXoffsets[1] * 8), char1Portrait.Y + (charYoffsets[1] * 8));
            parentRef.DrawText(gameState.Char2Name, (2 + charXoffsets[1]) * 8, (2 + charYoffsets[1]) * 8, DrawMode.Sprite, "large", 15);
            parentRef.DrawText(gameState.unlockedRoles[char2RoleId], (2 + charXoffsets[1]) * 8, (3 + charYoffsets[1]) * 8, DrawMode.Sprite, "large", 15);
            parentRef.WrapText(ContentLists.rolesByName[gameState.unlockedRoles[char2RoleId]].desc, 15, (2 + charXoffsets[1]) * 8, (8 + charYoffsets[1]));

            //char3
            parentRef.DrawMetaSprite(ContentLists.rolesByName[gameState.unlockedRoles[char3RoleId]].spriteSet, char1Portrait.X + (charXoffsets[2] * 8), char1Portrait.Y + (charYoffsets[2] * 8));
            parentRef.DrawText(gameState.Char3Name, (2 + charXoffsets[2]) * 8, (2 + charYoffsets[2]) * 8, DrawMode.Sprite, "large", 15);
            parentRef.DrawText(gameState.unlockedRoles[char3RoleId], (2 + charXoffsets[2]) * 8, (3 + charYoffsets[2]) * 8, DrawMode.Sprite, "large", 15);
            parentRef.WrapText(ContentLists.rolesByName[gameState.unlockedRoles[char3RoleId]].desc, 15, (2 + charXoffsets[2]) * 8, (8 + charYoffsets[2]));

            //char4
            parentRef.DrawMetaSprite(ContentLists.rolesByName[gameState.unlockedRoles[char4RoleId]].spriteSet, char1Portrait.X + (charXoffsets[3] * 8), char1Portrait.Y + (charYoffsets[3] * 8));
            parentRef.DrawText(gameState.Char4Name, (2 + charXoffsets[3]) * 8, (2 + charYoffsets[3]) * 8, DrawMode.Sprite, "large", 15);
            parentRef.DrawText(gameState.unlockedRoles[char4RoleId], (2 + charXoffsets[3]) * 8, (3 + charYoffsets[3]) * 8, DrawMode.Sprite, "large", 15);
            parentRef.WrapText(ContentLists.rolesByName[gameState.unlockedRoles[char4RoleId]].desc, 15, (2 + charXoffsets[3]) * 8, (8 + charYoffsets[3]));

            //Draw arrows around appropriate spot, also with the offset.
            parentRef.DrawMetaSprite("arrow", char1Portrait.X - 16 + (charXoffsets[activeCharSelecting] * 8), char1Portrait.Y + 16 + (charYoffsets[activeCharSelecting] * 8), true, false);
            parentRef.DrawMetaSprite("arrow", char1Portrait.X + 48 + (charXoffsets[activeCharSelecting] * 8), char1Portrait.Y + 16 + (charYoffsets[activeCharSelecting] * 8));
        }

        public static void Input()
        {
            //B: ACCEPT, advance to next character. if all characters are ready, advance to fight scene.
            //Left/Right: advance the active character's role ID in that direction.
            //FUTURE TODO: figure out how to rename characters. Not an MVP requirement.

            if (parentRef.Button(Buttons.B, InputState.Released))
            {
                activeCharSelecting++;
                if (activeCharSelecting >= 4)
                {
                    //TODO Prep the fight scene. Make characters and a new encounter and all that.
                    gameState.mode = gameState.FightSceneID;
                }
             
            }
        }

    }

}