using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
    public static class ContentLists
    {
        public static Stats baseStats = new Stats() { HP = 1, maxHP =1, MP= 1, maxMP = 1, STR = 1, DEF = 1, MAGIC = 1, MDEF = 1};

        public static List<Enemy> enemies = new List<Enemy>() {
        new Enemy() {name = "Test" , level = 1, startingStats = baseStats, StatsPerLevel = baseStats }
        };
    }
}