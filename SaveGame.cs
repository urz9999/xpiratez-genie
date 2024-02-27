using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.RepresentationModel;

namespace X_PirateZ_Genie
{
    internal class SaveGame
    {
        internal int Money { get; set; }
        internal List<Soldier> Soldiers { get; set; }
        internal List<Base> Bases { get; set; }

        private YamlMappingNode mapping;
        private YamlStream yaml;
        private int moneyLastIndex = -1;
        private string filePath;

        internal SaveGame() 
        {
            yaml = new YamlStream();
            Bases = new List<Base>();
            Soldiers = new List<Soldier>();
        }

        internal void Load(string filePath) 
        {
            this.filePath = filePath;
            using var input = new StreamReader(filePath);
            
            yaml.Load(input);
            mapping = (YamlMappingNode)yaml.Documents[1].RootNode;
            
            // Money
            var fundings = (YamlSequenceNode)mapping.Children[new YamlScalarNode("funds")];
            foreach (YamlScalarNode funding in fundings)
            {
                Money = int.Parse(funding.ToString());
                moneyLastIndex++;
            }

            // Bases & Soldiers
            var bases = (YamlSequenceNode)mapping.Children[new YamlScalarNode("bases")];
            foreach (YamlMappingNode baseUnit in bases)
            {
                var facilities = new List<Facility>();
                var facilityNodes = (YamlSequenceNode)baseUnit[new YamlScalarNode("facilities")];
                foreach (YamlMappingNode facility in facilityNodes)
                {
                    facilities.Add(new Facility() {
                        Type = facility[new YamlScalarNode("type")].ToString(),
                        X = int.Parse(facility[new YamlScalarNode("x")].ToString()),
                        Y = int.Parse(facility[new YamlScalarNode("y")].ToString()),
                        BuildTime = Base.CheckBuildTime(facility),
                        Dim = Base.GetFacilityDimension(facility[new YamlScalarNode("type")].ToString())
                    });
                }

                Bases.Add(new Base()
                {
                    Name = baseUnit[new YamlScalarNode("name")].ToString(),
                    Facilities = facilities
                });

                var soldiers = (YamlSequenceNode)baseUnit[new YamlScalarNode("soldiers")];
                foreach (YamlMappingNode soldier in soldiers)
                {
                    var stats = soldier.Children[new YamlScalarNode("currentStats")];
                    Soldiers.Add(new Soldier() {
                        Name = soldier[new YamlScalarNode("name")].ToString(),
                        TU = Int32.Parse(stats[new YamlScalarNode("tu")].ToString()),
                        EN = Int32.Parse(stats[new YamlScalarNode("stamina")].ToString()),
                        HE = Int32.Parse(stats[new YamlScalarNode("health")].ToString()),
                        BR = Int32.Parse(stats[new YamlScalarNode("bravery")].ToString()),
                        RE = Int32.Parse(stats[new YamlScalarNode("reactions")].ToString()),
                        AC = Int32.Parse(stats[new YamlScalarNode("firing")].ToString()),
                        TH = Int32.Parse(stats[new YamlScalarNode("throwing")].ToString()),
                        ST = Int32.Parse(stats[new YamlScalarNode("strength")].ToString()),
                        VP = Int32.Parse(stats[new YamlScalarNode("psiStrength")].ToString()),
                        VA = Int32.Parse(stats[new YamlScalarNode("psiSkill")].ToString()),
                        ME = Int32.Parse(stats[new YamlScalarNode("melee")].ToString()),
                        FR = Int32.Parse(stats[new YamlScalarNode("mana")].ToString()),
                    });
                }
            }
        }

        internal void RemoveAlienPact()
        {
            // TODO: implement this one
        }

        internal void updateBase(Base baseData)
        {
            var bases = (YamlSequenceNode)mapping.Children[new YamlScalarNode("bases")];
            foreach (YamlMappingNode baseUnit in bases)
            {
                if (baseUnit[new YamlScalarNode("name")].ToString().Equals(baseData.Name)) 
                {
                    var facilityNodes = (YamlSequenceNode)baseUnit[new YamlScalarNode("facilities")];
                    var index = 0;
                    foreach (YamlMappingNode facility in facilityNodes)
                    {
                        facility.Children[new YamlScalarNode("type")] = new YamlScalarNode(baseData.Facilities[index].Type);
                        facility.Children[new YamlScalarNode("x")] = new YamlScalarNode(baseData.Facilities[index].X.ToString());
                        facility.Children[new YamlScalarNode("y")] = new YamlScalarNode(baseData.Facilities[index].Y.ToString());
                        facility.Children[new YamlScalarNode("buildTime")] = new YamlScalarNode(baseData.Facilities[index].BuildTime.ToString());
                        index++;
                    }
                }
            }
        }

        internal void ChangeBaseName(int currentSelectedBase, string baseName)
        {
            // Bases & Soldiers
            var index = 1;
            var bases = (YamlSequenceNode)mapping.Children[new YamlScalarNode("bases")];
            foreach (YamlMappingNode baseUnit in bases)
            {
                if (currentSelectedBase.Equals(index)) 
                {
                    baseUnit.Children[new YamlScalarNode("name")] = new YamlScalarNode(baseName);
                    Bases[index - 1].Name = baseName;
                }
                index++;
            } 
        }

        internal void SaveSoldierStats(string name, decimal tu, decimal health, decimal energy, 
            decimal bravery, decimal accuracy, decimal strength, decimal throwing, 
            decimal melee, decimal freshness, decimal reactions, decimal voodooAbility, decimal voodooPower)
        {
            // Bases & Soldiers
            var bases = (YamlSequenceNode)mapping.Children[new YamlScalarNode("bases")];
            foreach (YamlMappingNode baseUnit in bases)
            {
                var soldiers = (YamlSequenceNode)baseUnit[new YamlScalarNode("soldiers")];
                foreach (YamlMappingNode soldier in soldiers)
                {
                    if (soldier[new YamlScalarNode("name")].ToString().Equals(name)) 
                    {
                        var stats = (YamlMappingNode)soldier[new YamlScalarNode("currentStats")];

                        stats.Children[new YamlScalarNode("tu")] = new YamlScalarNode(tu.ToString());
                        stats.Children[new YamlScalarNode("stamina")] = new YamlScalarNode(energy.ToString());
                        stats.Children[new YamlScalarNode("health")] = new YamlScalarNode(health.ToString());
                        stats.Children[new YamlScalarNode("bravery")] = new YamlScalarNode(bravery.ToString());
                        stats.Children[new YamlScalarNode("reactions")] = new YamlScalarNode(reactions.ToString());
                        stats.Children[new YamlScalarNode("firing")] = new YamlScalarNode(accuracy.ToString());
                        stats.Children[new YamlScalarNode("throwing")] = new YamlScalarNode(throwing.ToString());
                        stats.Children[new YamlScalarNode("strength")] = new YamlScalarNode(strength.ToString());
                        stats.Children[new YamlScalarNode("psiStrength")] = new YamlScalarNode(voodooPower.ToString());
                        stats.Children[new YamlScalarNode("psiSkill")] = new YamlScalarNode(voodooAbility.ToString());
                        stats.Children[new YamlScalarNode("melee")] = new YamlScalarNode(melee.ToString());
                        stats.Children[new YamlScalarNode("mana")] = new YamlScalarNode(freshness.ToString());
                    }
                }
            }
        }

        internal string[] GetSoldierNames()
        {
            return Soldiers.Select(soldier => soldier.Name).ToArray();
        }

        internal Soldier GetSoldier(string name)
        {
            return Soldiers.Where(soldier => soldier.Name.Equals(name)).ToArray()[0];
        }

        internal void MaxMovement()
        {
            MovementOrExperience(0);
        }

        internal void HealAll()
        {
            var battleGame = (YamlNode)mapping.Children[new YamlScalarNode("battleGame")];
            var units = (YamlSequenceNode)battleGame[new YamlScalarNode("units")];

            foreach (YamlMappingNode unit in units)
            {
                if (unit[new YamlScalarNode("faction")].ToString().Equals("0"))
                {
                    unit.Children[new YamlScalarNode("health")] = new YamlScalarNode("255");
                    unit.Children[new YamlScalarNode("fatalWounds")] = new YamlSequenceNode(new YamlNode[] {"0", "0", "0", "0", "0", "0"});
                }
            }
        }

        internal void MaxExperience()
        {
            MovementOrExperience(1);
        }

        internal void KillAllEnemies()
        {
            StunOrKill(1);
        }

        internal void StunAllEnemies()
        {
            StunOrKill(0);
        }

        private void StunOrKill(int type)
        {
            var battleGame = (YamlNode)mapping.Children[new YamlScalarNode("battleGame")];
            var units = (YamlSequenceNode)battleGame[new YamlScalarNode("units")];

            foreach (YamlMappingNode unit in units)
            {
                if (unit[new YamlScalarNode("faction")].ToString().Equals("1"))
                {
                    if (type.Equals(0))
                    {
                        unit.Children[new YamlScalarNode("stunlevel")] = new YamlScalarNode("300");
                    }
                    else 
                    {
                        unit.Children[new YamlScalarNode("health")] = new YamlScalarNode("-1");
                    }
                }
            }
        }

        private void MovementOrExperience(int type)
        {
            var battleGame = (YamlNode)mapping.Children[new YamlScalarNode("battleGame")];
            var units = (YamlSequenceNode)battleGame[new YamlScalarNode("units")];

            foreach (YamlMappingNode unit in units)
            {
                if (unit[new YamlScalarNode("faction")].ToString().Equals("0"))
                {
                    if (type.Equals(0))
                    {
                        unit.Children[new YamlScalarNode("tu")] = new YamlScalarNode("9999");
                        unit.Children[new YamlScalarNode("energy")] = new YamlScalarNode("9999");
                    }
                    else
                    {
                        unit.Children[new YamlScalarNode("expBravery")] = new YamlScalarNode("9999");
                        unit.Children[new YamlScalarNode("expReactions")] = new YamlScalarNode("9999");
                        unit.Children[new YamlScalarNode("expFiring")] = new YamlScalarNode("9999");
                        unit.Children[new YamlScalarNode("expThrowing")] = new YamlScalarNode("9999");
                        unit.Children[new YamlScalarNode("expPsiSkill")] = new YamlScalarNode("9999");
                        unit.Children[new YamlScalarNode("expPsiStrength")] = new YamlScalarNode("9999");
                        unit.Children[new YamlScalarNode("expMana")] = new YamlScalarNode("9999");
                        unit.Children[new YamlScalarNode("expMelee")] = new YamlScalarNode("9999");
                    }
                }
            }
        }

        internal void ChangeMoney(int newMoney)
        {
            var fundings = (YamlSequenceNode)mapping.Children[new YamlScalarNode("funds")];
            fundings.Children[moneyLastIndex] = new YamlScalarNode(newMoney.ToString());
        }

        internal void Save() 
        {
            using (TextWriter writer = File.CreateText(filePath))
            {
                yaml.Save(writer, false);
            }
        }
    }
}