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
            outerResults.Add(new DisplayResults() { frameCounter = 60, desc = "Combat Started" }); //empty for testing
            events = events.OrderByDescending(e => e.thingToDo.specialSpeedLevel).ThenByDescending(e => e.attacker.currentStats.SPD).ThenByDescending(e => gameState.random.Next()).ToList();
            foreach (var e in events)
            {
                if (e.attacker.CanAct()) //This could get flipped by earlier actions in the list.
                {
                    if (e.attacker.currentStats.MP >= e.thingToDo.mpCost)
                    {
                        //TODO: check that target of ability is still valid. Display 'ineffective;-style message if not. Might be part of UseAbility()
                        var abilOutcome = Ability.UseAbility(e.attacker, e.targets, e.thingToDo);
                        results.Add(abilOutcome);
                        for (int i = 0; i < abilOutcome.target.Count(); i++)
                        {
                            outerResults.Add(new DisplayResults() { target = abilOutcome.target[i], desc = "", changeStats = abilOutcome.targetChanges[i], frameCounter = 1 });
                        }

                        foreach (var adesc in abilOutcome.printDesc)
                            outerResults.Add(new DisplayResults() { desc = adesc });
                        //process target stat changes now, so dead enemies don't attack.
                        for (int i = 0; i < abilOutcome.target.Count(); i++)
                        {
                            abilOutcome.target[i].currentStats.Add(abilOutcome.targetChanges[i]);
                            if (abilOutcome.target[i].currentStats.HP <= 0)
                                outerResults.Add(new DisplayResults() { target = abilOutcome.target[i], desc = abilOutcome.target[i].name + " died.", changedItem = "spriteState", changedTo = "dead" });

                        }
                    }
                    else
                    {
                        //out of mana message
                        outerResults.Add(new DisplayResults() { desc = e.attacker.name + " didn't have the MP to use " + e.thingToDo.name });
                    }
                }
                //else
                //outerResults.Add(new DisplayResults() { desc = e.attacker.name + " couldn't act" });
            }

            if (FightScene.enemies.All(e => e.currentStats.HP <= 0))
            {
                DisplayResults winRewards = new DisplayResults();
                winRewards.desc = "Enemies defeated! Next wave incoming.";
                outerResults.Add(winRewards);

                foreach (var c in FightScene.characters)
                {
                    if (c.currentStats.HP > 0)
                    {
                        c.XP++;
                        if (c.XP >= 4)
                        {
                            //TODO queue level up for this character. Probably a DisplayResult entry that tells the game to switch scenes.
                            c.XP = 0;
                            c.level++;
                        }
                    }
                    else
                    {
                        c.currentStats.HP = 1;
                    }
                }
            }

            return outerResults;

        }

    }

}