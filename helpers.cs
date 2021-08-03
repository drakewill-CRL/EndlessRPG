using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
    public static class Extensions //for helpers and common functions.
    {
        public static void CenterText(this GameChip gc, string text, int y)
        {
            var startPoint = (gc.Display().X - (text.Length * 8)) / 2;
            gc.DrawText(text, startPoint, y, DrawMode.Sprite, "large", 15);
        }

        public static void RightAlignText(this GameChip gc, string text, int y)
        {
            var startPoint = (gc.Display().X - (text.Length * 8));
            gc.DrawText(text, startPoint, y, DrawMode.Sprite, "large", 15);
        }

        public static void WrapText(this GameChip gc, string text, int width, int x, int startY)
        {
            var wrapped = gc.WordWrap(text, width);
            var lines = gc.SplitLines(wrapped);
            for (int i = 0; i < lines.Count(); i++)
                gc.DrawText(lines[i], x, (startY + i) * 8, DrawMode.Sprite, "large", 15);
        }

    }
}
