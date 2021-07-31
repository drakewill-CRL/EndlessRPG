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
        public int mpCost;
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
        public static AttackResults UseAbility(Fightable attacker, List<Fightable> targets, Ability ability)
        {
            //All abilities' effects will just get handled in this one giant switch function for now.
            //until i cna figure out how to in-line a function declaration in an initializer.
            AttackResults results = new AttackResults();
            results.attacker = attacker;
            foreach (var t in targets) //So abilities can't have 0 targets. fake that out if needed by targeting self.
            {
                //TODO: might just make common checks flags here to make code easier to read below.
                //EX: targetIsAlive, etc.
                switch (ability.abilityKey)
                {
                    case 0: //Do a kickflip!
                        results.printDesc.Add(attacker.name + " does a sweet kickflip! Nothing else happens.");
                        results.target.Add(attacker);
                        results.targetChanges.Add(new Stats() { MP = -1 });
                        break;
                    case 1: //Fight
                        if (t.currentStats.HP > 0)
                        {
                            var damage = CalcDamage(ability, attacker, t);
                            results.printDesc.Add(attacker.name + " bonks " + targets[0].name + " for " + damage);
                            results.target.Add(t);
                            results.targetChanges.Add(new Stats() { HP = -damage });
                        }
                        else
                        {
                            results.printDesc.Add(attacker.name + " attacked a dead target.");
                        }
                        break;
                    case 2: //defend
                        results.printDesc.Add(attacker.name + " defends, defend not implemented");
                        break;
                    case 3: //Run
                        results.printDesc.Add(attacker.name + " runs, run not implemented");
                        break;
                    case 4: //Snapshot
                        if (t.currentStats.HP > 0)
                        {
                            var damage = CalcDamage(ability, attacker, t);
                            results.printDesc.Add(attacker.name + " hip-fires at " + targets[0].name + " for " + damage);
                            results.target.Add(t);
                            results.targetChanges.Add(new Stats() { HP = -damage });
                        }
                        else
                        {
                            results.printDesc.Add(attacker.name + " attacked a dead target.");
                        }
                        break;
                    case 5: //Aimed Shot
                        if (t.currentStats.HP > 0)
                        {
                            var damage = CalcDamage(ability, attacker, t);
                            results.printDesc.Add(attacker.name + " aims and fires at " + targets[0].name + " for " + damage);
                            results.target.Add(t);
                            results.targetChanges.Add(new Stats() { HP = -damage });
                        }
                        else
                        {
                            results.printDesc.Add(attacker.name + " attacked a dead target.");
                        }
                        break;
                    case 6: //Covering Fire
                        if (t.currentStats.HP > 0)
                        {
                            var damage = CalcDamage(ability, attacker, t);
                            results.printDesc.Add(attacker.name + " sprays " + targets[0].name + " for " + damage);
                            results.target.Add(t);
                            results.targetChanges.Add(new Stats() { HP = -damage });
                        }
                        //Area attacks quietly ignore dead enemies.
                        break;
                    case 7: //Tracer Rounds
                        if (t.currentStats.HP > 0)
                        {
                            var damage = CalcDamage(ability, attacker, t);
                            results.printDesc.Add(attacker.name + " fires at " + targets[0].name + " for " + damage);
                            results.target.Add(t);
                            results.targetChanges.Add(new Stats() { HP = -damage });
                        }
                        else
                        {
                            results.printDesc.Add(attacker.name + " attacked a dead target.");
                        }
                        break;
                    default:
                        results.printDesc.Add(attacker.name + " used an unimplemented ability (" + ability.abilityKey + ": " + ability.name + ")");
                        break;
                }
            }

            return results;
        }

        public static int CalcHeals(Attack a, Fightable target) //TODO: there;s no resist on heals, i think thats the main difference in logic.
        {
            float attackPowerBase = 0;
            switch (a.thingToDo.sourceStat)
            {
                case "STR":
                    attackPowerBase = a.attacker.currentStats.STR * 2;
                    break;
                case "MAGIC":
                    attackPowerBase = a.attacker.currentStats.MAGIC * 2;
                    break;
            }

            double results = 0;
            double multiplier = 1.0;
            target.damageMultipliers.TryGetValue(a.damageType, out multiplier);
            if (multiplier == 0)
                multiplier = 1;

            switch (a.thingToDo.sourceStat)
            {
                case "STR":
                    results = (attackPowerBase * multiplier) - target.currentStats.DEF;
                    break;
                case "MAGIC":
                    results = (attackPowerBase * multiplier) - target.currentStats.MDEF;
                    break;
            }

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
                case "MAGIC":
                    attackPowerBase = attacker.currentStats.MAGIC * 2;
                    break;
            }

            double results = 0;
            double multiplier = 1.0;
            target.damageMultipliers.TryGetValue(ab.damagetype, out multiplier);
            if (multiplier == 0)
                multiplier= 1;

            switch (ab.sourceStat)
            {
                case "STR":
                    results = (attackPowerBase * multiplier) - target.currentStats.DEF;
                    break;
                case "MAGIC":
                    results = (attackPowerBase * multiplier) - target.currentStats.MDEF;
                    break;
            }
            results = results * ab.powerMod;

            if (results < 0)
                results = 0;

            return (int)results;
        }
    }
}