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
                    if (e.attacker.currentStats.AP >= e.thingToDo.apCost)
                    {
                        //attack animations!
                        //TODO: use different cycle for 'ability' versus 'fight'?
                        outerResults.Add(new DisplayResults() { target = e.attacker, desc = "", changedItem = "spriteState", changedTo = "Attack", frameCounter = 6 });
                        outerResults.Add(new DisplayResults() { target = e.attacker, desc = "", changedItem = "spriteState", changedTo = "Ready", frameCounter = 6 });
                        outerResults.Add(new DisplayResults() { target = e.attacker, desc = "", changedItem = "spriteState", changedTo = "Attack", frameCounter = 6 });
                        outerResults.Add(new DisplayResults() { target = e.attacker, desc = "", changedItem = "spriteState", changedTo = "Ready", frameCounter = 6 });

                        //TODO: check that target of ability is still valid. Display 'ineffective;-style message if not. Might be part of UseAbility()
                        var abilOutcome = Ability.UseAbility(e.attacker, e.targets, e.thingToDo);
                        results.Add(abilOutcome);
                        for (int i = 0; i < abilOutcome.target.Count(); i++)
                        {
                            outerResults.Add(new DisplayResults() { target = abilOutcome.target[i], desc = "", changeStats = abilOutcome.targetChanges[i], frameCounter = 1 });
                        }

                        foreach (var adesc in abilOutcome.printDesc)
                        {
                            outerResults.Add(new DisplayResults() { desc = adesc, frameCounter = 120 });
                        }
                        //process target stat changes now, so dead enemies don't attack.
                        for (int i = 0; i < abilOutcome.target.Count(); i++)
                        {
                            abilOutcome.target[i].currentStats.Add(abilOutcome.targetChanges[i]);
                            if (abilOutcome.target[i].currentStats.HP <= 0)
                                outerResults.Add(new DisplayResults() { target = abilOutcome.target[i], desc = abilOutcome.target[i].name + " died.", changedItem = "spriteState", changedTo = "Dead" });
                        }

                        outerResults.Add(new DisplayResults() { target = e.attacker, desc = "", changedItem = "spriteState", changedTo = "", frameCounter = 1 });
                    }
                    else
                    {
                        //out of mana message
                        outerResults.Add(new DisplayResults() { desc = e.attacker.name + " didn't have the AP to use " + e.thingToDo.name });
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
                            c.XP = 0;
                            var levelUpEntry = new DisplayResults();
                            levelUpEntry.desc =  c.name + " Gets an improvement!";
                            outerResults.Add(levelUpEntry);
                            var levelUpSwitch = new DisplayResults();
                            levelUpSwitch.target = c;
                            levelUpSwitch.isLevelUp = true;
                            levelUpSwitch.frameCounter = 6;
                            outerResults.Add(levelUpSwitch);
                        }
                    }
                    else
                    {
                        c.currentStats.HP = 1;
                        var resTo1 = new DisplayResults();
                        resTo1.target = c;
                        resTo1.changeStats = new Stats(){ HP = 1};
                        resTo1.frameCounter = 1;
                    }
                }
            }

            //Dump combat log to the console.
            Console.WriteLine("--------------------------");
            foreach(var o in outerResults)
                Console.WriteLine(o.desc + " : " + o.frameCounter);

            return outerResults;

        }

    }

}