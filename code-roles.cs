using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{

    public class Role
    {
        public string name;
        public int[] startStats;
        public int[] statsPerLevel;
    }

    List<Role> roles = new List<Role>() {
        new Role() {name = "Generic"}
    };
}