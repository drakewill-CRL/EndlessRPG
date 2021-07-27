using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
    public static class CombatEngine
    {
        //The code for actually handling fights.
        //Queues up inputs, spits out list of queued up things to display/animate.

        public static void ProcessRound(List<Attack> events)
        {
            events = events.OrderByDescending(e => e.attacker.currentStats.SPD).ToList();
            foreach(var e in events)
            {
                if (e.attacker.CanAct())
                {
                    
                }
            }

        }

        public static void CalcDamage(Attack a)
        {

        }

    }

}