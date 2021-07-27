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

        public static List<AttackResults> ProcessRound(List<Attack> events)
        {
            List<AttackResults> results = new List<AttackResults>();
            //results.Add(new AttackResults(){ printDesc = new List<string>(){"Combat Started"}}); //empty for testing
            events = events.OrderByDescending(e => e.attacker.currentStats.SPD).ToList();
            foreach(var e in events)
            {
                if (e.attacker.CanAct()) //This could get flipped by earlier actions in the list.
                {
                    var abilOutcome = Ability.UseAbility(e.attacker, e.targets, e.thingToDo.abilityKey);
                    results.Add(abilOutcome);
                }
            }

            return results;

        }

        public static void CalcDamage(Attack a)
        {

        }

    }

}