using System.Diagnostics;
using XPirateZ_Genie2._0.Utils;
using YamlDotNet.RepresentationModel;
using static System.Reflection.Metadata.BlobBuilder;

namespace X_PirateZ_Genie
{
    internal class SaveGame
    {
        internal int Money { get; set; }
        internal List<Soldier> Soldiers { get; set; }
        internal List<Base> Bases { get; set; }

        private YamlMappingNode mapping;
        private YamlStream yaml;
        private Manufactures manufacturesData;
        private int moneyLastIndex = -1;
        private string filePath;

        internal SaveGame() 
        {
            yaml = new YamlStream();
            Bases = new List<Base>();
            Soldiers = new List<Soldier>();
            manufacturesData = new Manufactures();
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
                Money = Math.Abs(int.Parse(funding.ToString()));
                moneyLastIndex++;
            }

            // Bases & Soldiers & Researches
            var bases = (YamlSequenceNode)mapping.Children[new YamlScalarNode("bases")];
            foreach (YamlMappingNode baseUnit in bases)
            {
                var facilities = new List<Facility>();
                var facilityNodes = (YamlSequenceNode)baseUnit[new YamlScalarNode("facilities")];
                foreach (YamlMappingNode facility in facilityNodes)
                {
                    facilities.Add(new Facility()
                    {
                        Type = facility[new YamlScalarNode("type")].ToString(),
                        X = int.Parse(facility[new YamlScalarNode("x")].ToString()),
                        Y = int.Parse(facility[new YamlScalarNode("y")].ToString()),
                        BuildTime = Base.CheckBuildTime(facility),
                        Dim = Base.GetFacilityDimension(facility[new YamlScalarNode("type")].ToString())
                    });
                }

                var items = new List<Item>();
                var itemsNodes = (YamlMappingNode)baseUnit[new YamlScalarNode("items")];
                Console.WriteLine(baseUnit);
                foreach (var item in itemsNodes)
                {
                    Console.WriteLine(item.Key);
                    var name = Translations.GetTranslationByKey(item.Key.ToString());
                    items.Add(new Item { name=name, ID = item.Key.ToString(), qty = Int32.Parse(item.Value.ToString()) });
                }

                var researches = new List<Research>();
                try
                {
                    var researchesNodes = (YamlSequenceNode)baseUnit[new YamlScalarNode("research")];
                    foreach (YamlMappingNode research in researchesNodes)
                    {
                        researches.Add(new Research()
                        {
                            Project = research[new YamlScalarNode("project")].ToString(),
                            Assigned = Int32.Parse(research[new YamlScalarNode("assigned")].ToString()),
                            Cost = Int32.Parse(research[new YamlScalarNode("cost")].ToString()),
                            Spent = Int32.Parse(research[new YamlScalarNode("spent")].ToString()),
                        });
                    }
                }
                catch 
                {
                
                }

                var manufactures = new List<ProductionItem>();

                try
                {
                    var manufacturesNodes = (YamlSequenceNode)baseUnit[new YamlScalarNode("productions")];
                    foreach (YamlMappingNode item in manufacturesNodes)
                    {
                        try
                        {
                            var productionItem = new ProductionItem()
                            {
                                ID = item[new YamlScalarNode("item")].ToString(),
                                Name = Translations.GetTranslationByKey(item[new YamlScalarNode("item")].ToString()),
                                Spent = Int32.Parse(item[new YamlScalarNode("spent")].ToString()),
                                Assigned = Int32.Parse(item[new YamlScalarNode("assigned")].ToString()),
                                Amount = Int32.Parse(item[new YamlScalarNode("amount")].ToString()),
                                Cost = manufacturesData.ManufactureItems[item[new YamlScalarNode("item")].ToString()].Time
                            };
                            manufactures.Add(productionItem);
                            Debug.WriteLine(productionItem);
                        }
                        catch { }

                    }
                } catch { }

                var crafts = new List<CraftData>();
                try 
                {
                    var craftsNodes = (YamlSequenceNode)baseUnit[new YamlScalarNode("crafts")];
                    foreach (YamlMappingNode craft in craftsNodes)
                    {
                        var type = craft[new YamlScalarNode("type")].ToString();
                        var craftSpecs = CraftsSpecs.CraftDictionary[type];
                        var name = Translations.GetTranslationByKey(type);
                        var slots = new List<WSlot>();
                        try
                        {
                            var wslotsNodes = (YamlSequenceNode)craft[new YamlScalarNode("weapons")];
                            foreach (YamlMappingNode wslot in wslotsNodes)
                            {
                                try
                                {
                                    slots.Add(new WSlot()
                                    {
                                        ID = wslot[new YamlScalarNode("type")].ToString(),
                                        name = Translations.GetTranslationByKey(wslot[new YamlScalarNode("type")].ToString()),
                                        ammo = Int32.Parse(wslot[new YamlScalarNode("ammo")].ToString()),
                                    });
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        catch (Exception ex) { }
                        crafts.Add(new CraftData()
                        {
                            ID = type,
                            Name = name,
                            Fuel = Int32.Parse(craft[new YamlScalarNode("fuel")].ToString()),
                            Damage = Int32.Parse(craft[new YamlScalarNode("damage")].ToString()),
                            Shield = Int32.Parse(craft[new YamlScalarNode("shield")].ToString()),
                            wSlots = slots,
                            Sprite = PlaneReferences.GetSpriteByKey($"Code_{craftSpecs.SpriteId + 33}"),
                            craftSpecs = craftSpecs
                        });
                    }
                } catch (Exception ex) { }
                    

                Bases.Add(new Base()
                {
                    Name = baseUnit[new YamlScalarNode("name")].ToString(),
                    Facilities = facilities,
                    Items = items,
                    Researches = researches,
                    CraftData = crafts,
                    Manufactures = manufactures,
                });

                try
                {
                    var soldiers = (YamlSequenceNode)baseUnit[new YamlScalarNode("soldiers")];
                    foreach (YamlMappingNode soldier in soldiers)
                    {
                        var stats = soldier.Children[new YamlScalarNode("currentStats")];
                        Soldiers.Add(new Soldier()
                        {
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
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.ToString());
                }

            }
        }

        internal void saveItemQuantity(int currentSelectedBase, string itemID, decimal qty)
        {
            var bases = (YamlSequenceNode)mapping.Children[new YamlScalarNode("bases")];
            YamlMappingNode baseUnit = (YamlMappingNode) bases[currentSelectedBase - 1];
            ((YamlMappingNode)baseUnit[new YamlScalarNode("items")]).Children[new YamlScalarNode(itemID)] = qty.ToString();
        }

        internal int getCurrentItemQty(int currentSelectedBase, string itemID)
        {
            return this.Bases[currentSelectedBase - 1].Items.Find(item => item.ID == itemID).qty;
        }

        internal Research getResearch(int currentSelectedBase, string researchName) 
        {
            return this.Bases[currentSelectedBase - 1].Researches.Find(item => item.Project == researchName);
        }

        internal void completeResearch(int currentSelectedBase, string researchName)
        {
            var baseData = this.Bases[currentSelectedBase - 1];
            var bases = (YamlSequenceNode)mapping.Children[new YamlScalarNode("bases")];
            foreach (YamlMappingNode baseUnit in bases)
            {
                if (baseUnit[new YamlScalarNode("name")].ToString().Equals(baseData.Name))
                {
                    var researchNodes = (YamlSequenceNode)baseUnit[new YamlScalarNode("research")];
                    foreach (YamlMappingNode research in researchNodes)
                    {
                        if (research.Children[new YamlScalarNode("project")].ToString() == researchName) 
                        {
                            research.Children[new YamlScalarNode("spent")] = research.Children[new YamlScalarNode("cost")];
                        }        
                    }
                }
            }
        }

        internal void fullyRechargeAmmo(int currentSelectedBase, string vehicleID)
        {
            var baseData = this.Bases[currentSelectedBase - 1];
            var bases = (YamlSequenceNode)mapping.Children[new YamlScalarNode("bases")];
            foreach (YamlMappingNode baseUnit in bases)
            {
                if (baseUnit[new YamlScalarNode("name")].ToString().Equals(baseData.Name))
                {
                    var craftsNodes = (YamlSequenceNode)baseUnit[new YamlScalarNode("crafts")];
                    foreach (YamlMappingNode craft in craftsNodes)
                    {
                        if (craft.Children[new YamlScalarNode("type")].ToString() == vehicleID)
                        {
                            var wslotsNodes = (YamlSequenceNode)craft[new YamlScalarNode("weapons")];
                            foreach (YamlMappingNode wslot in wslotsNodes)
                            {
                                if (wslot[new YamlScalarNode("type")].ToString() != "0") 
                                {
                                    wslot.Children[new YamlScalarNode("ammo")] = 999.ToString();
                                }
                            }
                        }
                    }
                }
            }
        }

        internal void fullyRechargeProperty(int currentSelectedBase, string vehicleID, int type, CraftData craftData)
        {
            var baseData = this.Bases[currentSelectedBase - 1];
            var bases = (YamlSequenceNode)mapping.Children[new YamlScalarNode("bases")];
            foreach (YamlMappingNode baseUnit in bases)
            {
                if (baseUnit[new YamlScalarNode("name")].ToString().Equals(baseData.Name))
                {
                    var craftsNodes = (YamlSequenceNode)baseUnit[new YamlScalarNode("crafts")];
                    foreach (YamlMappingNode craft in craftsNodes)
                    {
                        if (craft.Children[new YamlScalarNode("type")].ToString() == vehicleID)
                        {
                            if (type == 0)
                            {
                                craft.Children[new YamlScalarNode("damage")] = 0.ToString();
                            }
                            else if (type == 1) 
                            {
                                craft.Children[new YamlScalarNode("shield")] = craftData.craftSpecs.ShieldCapacity.ToString();
                            }
                            else if (type == 2) 
                            {
                                craft.Children[new YamlScalarNode("fuel")] = craftData.craftSpecs.FuelMax.ToString();
                            }
                        }
                    }
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
                try
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
                catch { }
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

        internal bool isBattleSave() 
        {
            try
            {
                _ = mapping.Children[new YamlScalarNode("battleGame")];
                return true;
            } 
            catch (KeyNotFoundException)
            {
                return false;
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

        internal List<ProductionItem> GetProductions(int baseNumber)
        {
            return this.Bases[baseNumber - 1].Manufactures;
        }

        internal void SetProductionAsCompleted(int baseNumber, ProductionItem item)
        {
            var baseData = this.Bases[baseNumber - 1];
            var bases = (YamlSequenceNode)mapping.Children[new YamlScalarNode("bases")];
            foreach (YamlMappingNode baseUnit in bases)
            {
                if (baseUnit[new YamlScalarNode("name")].ToString().Equals(baseData.Name))
                {
                    var productionNodes = (YamlSequenceNode)baseUnit[new YamlScalarNode("productions")];
                    foreach (YamlMappingNode itemNode in productionNodes)
                    {
                        if (itemNode.Children[new YamlScalarNode("item")].ToString() == item.ID)
                        {
                            itemNode.Children[new YamlScalarNode("spent")] = new YamlScalarNode((item.Cost*item.Amount).ToString());
                        }
                    }
                }
            }
        }
    }
}