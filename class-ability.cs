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

        //public void UseAbility() { return; } //must be overridden? Might not be what I think
        public int abilityKey;

        public int specialSpeedLevel = 3; //3 is 'average' attacks.

        public Ability Clone()
        {
            Ability clone = (Ability)this.MemberwiseClone();
            return clone;

        }
        public static AttackResults UseAbility(Fightable attacker, List<Fightable> targets, int key)
        {
            //All abilities' effects will just get handled in this one giant switch function for now.
            //until i cna figure out how to in-line a function declaration in an initializer.
            AttackResults results = new AttackResults();
            results.attacker = attacker;
            foreach (var t in targets) //So abilities can't have 0 targets. fake that out if needed by targeting self.
            {
                switch (key)
                {
                    case 0: //Do a kickflip!
                        results.printDesc.Add(attacker.name + " does a sweet kickflip! Nothing else happens.");
                        results.target.Add(attacker);
                        results.targetChanges.Add(new Stats() { MP = -1 });
                        break;
                    case 1: //Fight
                        if (t.currentStats.HP > 0)
                        {
                            results.printDesc.Add(attacker.name + " bonks " + targets[0].name + " for 1 / REAL DAMAGE NOT IMPLEMENTED");
                            results.target.Add(t);
                            results.targetChanges.Add(new Stats() { HP = -1 });
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
                    default:
                        results.printDesc.Add(attacker.name + " used an unimplemented ability (" + key + ": " + ContentLists.allAbilities[key].name  +  ")");
                        break;
                }
            }

            return results;
        }
    }
}