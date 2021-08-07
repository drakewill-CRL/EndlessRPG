using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PixelVision8;
using System;
using System.Linq;

namespace PixelVision8.Player
{
    public class Ability
    {
        public string name; //Name has a 13 character limit to fit in the allocated box.
        public string description; //help text that appears in combat. Might need to be a property to update with levels.
        public int apCost;
        public int level; //Used to power up the effects of the ability.
        public int targetType; //0: no target selection(self, random, all enemies, or all allies.). 2: target  1 enemy 3: target 1 ally.
        public string damagetype; //Should have a key/dictionary for this
        public string sourceStat; //throw which stat to use for math here by name.
        public double powerMod = 1; //Applied after stat Vs stat calculations, so attacks can be stronger or weaker than normal.

        //public void UseAbility() { return; } //must be overridden? Might not be what I think
        public int abilityKey;

        public int specialSpeedLevel = 3; //3 is 'average' attacks.

        public Ability Clone()
        {
            Ability clone = (Ability)this.MemberwiseClone();
            return clone;
        }
        //TODO: allow this to be called from the ability itself instead of being static?
        public static AttackResults UseAbility(Fightable attacker, List<Fightable> targets, Ability ability)
        {
            //All abilities' effects will just get handled in this one giant switch function. 
            AttackResults results = new AttackResults();
            results.attacker = attacker;

            //NOTE: special things need targets pulled in here.
            if (ability.targetType == 0)
            {
                //Self target. Other auto-target abilities might need flagged somewhere else or processed in the attacker phase.
                targets.Add(attacker);
            }

            foreach (var t in targets) //So abilities can't have 0 targets. fake that out if needed by targeting self.
            {
                //TODO: change this to name to avoid forgetting to set abilityKey?
                //TODO: put the description in the ability itself.
                switch (ability.abilityKey)
                {
                    case 0: //Do a kickflip!
                        results.printDesc.Add(attacker.name + " does a sweet kickflip! Nothing else happens.");
                        break;
                    case 1: //Fight
                        results = BasicAttack(attacker, t, ability, "attacks");
                        break;
                    case 2: //defend
                        results.printDesc.Add(attacker.name + " defends, defend not implemented");
                        break;
                    case 3: //Run
                        results.printDesc.Add(attacker.name + " runs, run not implemented");
                        break;
                    case 4: //Snapshot
                        results = BasicAttack(attacker, t, ability, "hip-fires at");
                        break;
                    case 5: //Aimed Shot
                        results = BasicAttack(attacker, t, ability, "aims and fires at");
                        break;
                    case 6: //Covering Fire
                        results = BasicAreaAttack(attacker, t, ability, "sprays");
                        break;
                    case 7: //Tracer Rounds
                        results = BasicAttack(attacker, t, ability, "fires burning shots at");
                        break;
                    case 8: //First Aid
                        results = BasicHeal(attacker, t, ability, "patches up");
                        break;
                    case 9: //Adrenal Mist
                        results = BasicAreaHeal(attacker, t, ability, "doses");
                        break;
                    case 10: //Defib //TODO: should this become a biomorph only res?
                        results = BasicRes(attacker, t, ability, "'revives", new Stats() { HP = 1 });
                        break;
                    case 11: //Neurotoxin
                        results = BasicAttack(attacker, t, ability, "poisons");
                        break;
                    case 12: //Spot Weld
                        results = BasicHeal(attacker, t, ability, "fixes up");
                        break;
                    case 13: //Corrosive Juice
                        results = BasicAttack(attacker, t, ability, "corrodes");
                        break;
                    case 14: // Tank Tackle
                        results = BasicAttack(attacker, t, ability, "tackles");
                        break;
                    case 15: //Power Cycle
                        results = BasicBuff(attacker, t, ability, "restarts with their limiters disabled.", new Stats() { maxHP = 5, STR = 3, INS = 3, DEF = 3, MOX = 1, SPD = 2, LUK = -1 });
                        break;
                    case 16: //Ambush
                        results = BasicAttack(attacker, t, ability, "quickly stabs");
                        break;
                    case 17: //C4 Charge
                        results = BasicAreaAttack(attacker, t, ability, "explodes");
                        break;
                    case 18: //Quickhack
                        if (t.morphType == "synth")
                            results = BasicAttack(attacker, t, ability, "hacks");
                        else
                            results.printDesc.Add(attacker.name + " can't hack the target.");
                        break;
                    case 19: //combat stims
                        results = BasicBuff(attacker, t, ability, "juices up", new Stats() {SPD = 4, LUK = 1 });
                        break;
                    default:
                        results.printDesc.Add(attacker.name + " used an unimplemented ability (" + ability.abilityKey + ": " + ability.name + ")");
                        break;
                }
            }
            
            //Now that we have results, remove AP from the attacker.
            results.target.Add(attacker);
            results.targetChanges.Add(new Stats() { AP = -(ability.apCost) });
            results.statSetToApply.Add("current");

            return results;
        }

        public static int CalcHeals(Ability ab, Fightable attacker, Fightable target) // there;s no resist and no crits on heals, i think thats the main difference in logic.
        {
            float attackPowerBase = 0;
            switch (ab.sourceStat)
            {
                case "STR":
                    attackPowerBase = attacker.currentStats.STR * 2;
                    break;
                case "INS":
                    attackPowerBase = attacker.currentStats.INS * 2;
                    break;
            }

            double results = 0;
            double multiplier = 1.0;
            target.damageMultipliers.TryGetValue(ab.damagetype, out multiplier);
            if (multiplier == 0)
                multiplier = 1;

            results = attackPowerBase * multiplier * ab.powerMod;

            if (results < 0)
                results = 0;

            return (int)results;
        }

        public static int CalcDamage(Ability ab, Fightable attacker, Fightable target) //This gets called per target, so i don't need to loop here too.
        {
            double attackPowerBase = 0;
            switch (ab.sourceStat)
            {
                case "STR":
                    attackPowerBase = attacker.currentStats.STR * 2;
                    break;
                case "INS":
                    attackPowerBase = attacker.currentStats.INS * 2;
                    break;
            }

            double results = 0;
            double multiplier = 1.0;
            target.damageMultipliers.TryGetValue(ab.damagetype, out multiplier);
            if (multiplier == 0)
                multiplier = 1; //nothing is immune. but can have .01 multiplier.

            switch (ab.sourceStat)
            {
                case "STR":
                    results = (attackPowerBase * multiplier) - target.currentStats.DEF;
                    break;
                case "INS":
                    results = (attackPowerBase * multiplier) - target.currentStats.MOX;
                    break;
            }
            results = results * ab.powerMod;

            //crit chance calc
            //TODO: return an object that reports damage and if its a crit or not, maybe other info?
            var luckSpread = attacker.currentStats.LUK - target.currentStats.LUK;
            var isCrit = (gameState.random.Next(0, 100) <= Math.Abs(luckSpread)); //Roll under the spread.
            if (isCrit && luckSpread > 0)
                results *= 2;
            if (isCrit && luckSpread < 0)
                results /= 2;

            if (results < 0)
                results = 0;

            return (int)results;
        }

        public static AttackResults BasicAttack(Fightable attacker, Fightable target, Ability ability, string description)
        {
            var results = new AttackResults();
            if (target.currentStats.HP > 0)
            {
                var damage = CalcDamage(ability, attacker, target);
                results.printDesc.Add(attacker.name + " " + description + " " + target.name + " for " + damage + " damage");
                results.target.Add(target);
                results.targetChanges.Add(new Stats() { HP = -damage });
                results.statSetToApply.Add("current");
            }
            else
                results.printDesc.Add(attacker.name + " attacked a dead target.");

            return results;
        }

        public static AttackResults BasicAreaAttack(Fightable attacker, Fightable target, Ability ability, string description)
        {
            //As above, but quietly skips dead targets.
            var results = new AttackResults();
            if (target.currentStats.HP > 0)
            {
                var damage = CalcDamage(ability, attacker, target);
                results.printDesc.Add(attacker.name + " " + description + " " + target.name + " for " + damage + " damage");
                results.target.Add(target);
                results.targetChanges.Add(new Stats() { HP = -damage });
                results.statSetToApply.Add("current");
            }
            return results;
        }

        public static AttackResults BasicHeal(Fightable attacker, Fightable target, Ability ability, string description)
        {
            var results = new AttackResults();
            if (target.currentStats.HP > 0)
            {
                var heals = CalcHeals(ability, attacker, target);
                results.printDesc.Add(attacker.name + " " + description + " " + target.name + " to restore " + heals + " HP");
                results.target.Add(target);
                results.targetChanges.Add(new Stats() { HP = heals });
                results.statSetToApply.Add("current");
            }
            else
                results.printDesc.Add(attacker.name + " can't heal the dead.");

            return results;
        }
        public static AttackResults BasicAreaHeal(Fightable attacker, Fightable target, Ability ability, string description)
        {
            //As above, but quietly skip dead targets
            var results = new AttackResults();
            if (target.currentStats.HP > 0)
            {
                var heals = CalcHeals(ability, attacker, target);
                results.printDesc.Add(attacker.name + " " + description + " " + target.name + " to restore " + heals + " HP");
                results.target.Add(target);
                results.targetChanges.Add(new Stats() { HP = heals });
                results.statSetToApply.Add("current");
            }
            return results;
        }

        public static AttackResults BasicBuff(Fightable attacker, Fightable target, Ability ability, string description, Stats statChanges)
        {
            var results = new AttackResults();
            if (target.currentStats.HP > 0)
            {
                results.printDesc.Add(attacker.name + " " + description + " " + target.name); //TODO: make target name toggleable
                results.target.Add(target);
                results.targetChanges.Add(statChanges);
                results.statSetToApply.Add("temp");
            }
            else
                results.printDesc.Add(attacker.name + " can't help the dead.");

            return results;
        }

        public static AttackResults BasicRes(Fightable attacker, Fightable target, Ability ability, string description, Stats statChanges)
        {
            var results = new AttackResults();
            if (target.currentStats.HP <= 0)
            {
                results.printDesc.Add(attacker.name + " " + description + " " + target.name);
                results.target.Add(target);
                results.targetChanges.Add(statChanges);
                results.statSetToApply.Add("current");
            }
            else
                results.printDesc.Add(attacker.name + " can't res the living.");

            return results;
        }
    }
}