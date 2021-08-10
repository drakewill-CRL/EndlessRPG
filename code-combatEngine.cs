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
            //Recent Changes:
            //Initiative has a d10 added to your SPD stat so combat order isn't as guaranteed.
            List<DisplayResults> outerResults = new List<DisplayResults>();
            List<AttackResults> results = new List<AttackResults>();
            outerResults.Add(new DisplayResults() { frameCounter = 60, desc = "Combat Started" }); //empty for testing
            events = events.OrderByDescending(e => e.thingToDo.specialSpeedLevel).ThenByDescending(e => e.attacker.currentStats.SPD + gameState.random.Next(1, 10)).ThenByDescending(e => gameState.random.Next()).ToList();
            foreach (var e in events)
            {
                //TODO: attempt to intelligently short-cut stuff if all enemies are dead?
                if (e.attacker.CanAct()) //This could get flipped by earlier actions in the list.
                {
                    if (e.attacker.currentStats.AP >= e.thingToDo.apCost)
                    {
                        //attack animations!
                        //TODO: use different cycle for 'ability' versus 'fight'?
                        if (e.attacker.isPlayer)
                            outerResults.AddRange(GetPcAttackAnimation(e.attacker));
                        else
                            outerResults.AddRange(GetEnemyAttackAnimation(e.attacker));

                        var abilOutcome = Ability.UseAbility(e.attacker, e.targets, e.thingToDo);
                        results.Add(abilOutcome);
                        for (int i = 0; i < abilOutcome.target.Count(); i++)
                        {
                            outerResults.Add(new DisplayResults() { target = abilOutcome.target[i], desc = "", changeStats = abilOutcome.targetChanges[i], frameCounter = 1 });
                        }

                        foreach (var adesc in abilOutcome.printDesc)
                        {
                            outerResults.Add(new DisplayResults() { desc = adesc, frameCounter = gameState.displayDefaultTimer });
                        }
                        //process target stat changes now, so dead enemies don't attack.
                        for (int i = 0; i < abilOutcome.target.Count(); i++)
                        {
                            switch (abilOutcome.statSetToApply[i])
                            {
                                case "current":
                                    abilOutcome.target[i].currentStats.Add(abilOutcome.targetChanges[i]);
                                    break;
                                case "temp":
                                    abilOutcome.target[i].tempChanges.Add(abilOutcome.targetChanges[i]);
                                    abilOutcome.target[i].currentStats = abilOutcome.target[i].getTotalStats(false);
                                    break;
                            }

                            if (abilOutcome.target[i].currentStats.HP <= 0)
                                outerResults.Add(new DisplayResults() { target = abilOutcome.target[i], desc = abilOutcome.target[i].name + " was downed.", changedItem = "spriteState", changedTo = "Dead" });
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
                winRewards.changedItem = "fightWon";
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
                            levelUpEntry.desc = c.name + " Gets an improvement!";
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
                        resTo1.changeStats = new Stats() { HP = 1 };
                        resTo1.frameCounter = 1;
                    }
                    c.tempChanges = new Stats(); //Temp changes expire at the end of the encounter.
                    c.currentStats = c.getTotalStats(false);
                }
            }

            //Dump combat log to the console.
            Console.WriteLine("--------------------------");
            foreach (var o in outerResults)
                Console.WriteLine(o.desc + " : " + o.frameCounter);

            return outerResults;

        }

        public static List<DisplayResults> GetPcAttackAnimation(Fightable f)
        {
            var results = new List<DisplayResults>();
            results.Add(new DisplayResults() { target = f, desc = "", changedItem = "spriteState", changedTo = "Attack", frameCounter = 6 });
            results.Add(new DisplayResults() { target = f, desc = "", changedItem = "spriteState", changedTo = "Ready", frameCounter = 6 });
            results.Add(new DisplayResults() { target = f, desc = "", changedItem = "spriteState", changedTo = "Attack", frameCounter = 6 });
            results.Add(new DisplayResults() { target = f, desc = "", changedItem = "spriteState", changedTo = "Ready", frameCounter = 6 });
            return results;
        }

        public static List<DisplayResults> GetEnemyAttackAnimation(Fightable f)
        {
            var results = new List<DisplayResults>();
            results.Add(new DisplayResults() { target = f, desc = "", changedItem = "colorShift", changedTo = "1", frameCounter = 6 });
            results.Add(new DisplayResults() { target = f, desc = "", changedItem = "colorShift", changedTo = "0", frameCounter = 6 });
            results.Add(new DisplayResults() { target = f, desc = "", changedItem = "colorShift", changedTo = "1", frameCounter = 6 });
            results.Add(new DisplayResults() { target = f, desc = "", changedItem = "colorShift", changedTo = "0", frameCounter = 6 });
            return results;
        }

    }

}