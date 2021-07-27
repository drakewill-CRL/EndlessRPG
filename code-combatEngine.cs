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

        public static List<DisplayResults> ProcessRound(List<Attack> events)
        {
            List<DisplayResults> outerResults = new List<DisplayResults>();
            List<AttackResults> results = new List<AttackResults>();
            //results.Add(new AttackResults(){ printDesc = new List<string>(){"Combat Started"}}); //empty for testing
            events = events.OrderByDescending(e => e.attacker.currentStats.SPD).ToList();
            foreach (var e in events)
            {
                if (e.attacker.CanAct()) //This could get flipped by earlier actions in the list.
                {
                    //TODO: check that target of ability is still valid. Display 'ineffective;-style message if not.
                    var abilOutcome = Ability.UseAbility(e.attacker, e.targets, e.thingToDo.abilityKey);
                    results.Add(abilOutcome);

                    //process target stat changes now, so dead enemies don't attack. TODO update the results returned here to be display-results, not stat-results.
                    for (int i = 0; i < abilOutcome.target.Count(); i++)
                    {
                        abilOutcome.target[i].currentStats.Add(abilOutcome.targetChanges[i]);
                        if (abilOutcome.target[i].currentStats.HP <= 0)
                            outerResults.Add(new DisplayResults() { target = abilOutcome.target[i], desc= abilOutcome.target[i].name + " died.", changedItem = "spriteState", changedTo = "dead" });
                    }

                    foreach(var adesc in abilOutcome.printDesc)
                        outerResults.Add(new DisplayResults(){ desc = adesc});
                }
                else
                    outerResults.Add(new DisplayResults(){  desc = e.attacker.name + " couldn't act"});
            }

            return outerResults;

        }

        public static void CalcDamage(Attack a)
        {

        }

    }

}