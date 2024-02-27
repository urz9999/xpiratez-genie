using System;

namespace X_PirateZ_Genie
{
    internal class SaveEditor
    {
        Facility[] clickedSlots = new Facility[2];

        internal SaveGame Load(string filePath) 
        {
            SaveGame saveGame = new SaveGame();
            saveGame.Load(filePath);
            return saveGame;
        }

        internal void Save(SaveGame saveGame) 
        {
            saveGame.Save();
        }

        internal void ChangeMoney(SaveGame saveGame, int newMoney)
        {
            saveGame.ChangeMoney(newMoney);
        }

        internal void StunAllEnemies(SaveGame saveGame)
        {
            saveGame.StunAllEnemies();
        }

        internal void KillAllEnemies(SaveGame saveGame)
        {
            saveGame.KillAllEnemies();
        }

        internal void MaxExperience(SaveGame saveGame)
        {
            saveGame.MaxExperience();
        }

        internal void MaxMovement(SaveGame saveGame)
        {
            saveGame.MaxMovement();
        }

        internal void HealAll(SaveGame saveGame)
        {
            saveGame.HealAll();
        }

        internal void SaveSoldierStats(
            SaveGame saveGame, string name, decimal tu, decimal health, 
            decimal energy, decimal bravery, decimal accuracy, decimal strength, 
            decimal throwing, decimal melee, decimal freshness, decimal reactions, 
            decimal voodooAbility, decimal voodooPower)
        {
            saveGame.SaveSoldierStats(name, tu, health, energy, bravery, accuracy, strength, throwing, melee, freshness, reactions, voodooAbility, voodooPower);
        }

        internal void ChangeBaseName(SaveGame saveGame, int currentSelectedBase, string baseName)
        {
            saveGame.ChangeBaseName(currentSelectedBase, baseName);
        }

        internal void AddClickedBaseSlot(Base baseData, string coordsEncoded, int index)
        {
            string[] coords = coordsEncoded.Split(",");
            int X = int.Parse(coords[1]);
            int Y = int.Parse(coords[2]);

            clickedSlots[index] = baseData.GetFacilityByCoords(X, Y);
        }

        internal void SwapSlotIfPossible(SaveGame saveGame, Base baseData)
        {
            if (baseData.IsLocationFree(clickedSlots[0], clickedSlots[1].X, clickedSlots[1].Y)) 
            {
                baseData.SwapFacilities(clickedSlots[0], clickedSlots[1]);
                saveGame.updateBase(baseData);
            }
        }

        internal void CleanClickedBaseSlots()
        {
            clickedSlots = new Facility[2];
        }

        internal void BuildNow(SaveGame saveGame, int currentSelectedBase, string rightClickTag)
        {
            var coords = rightClickTag.Split(",");
            var baseData = saveGame.Bases[currentSelectedBase - 1];
            var facility = baseData.GetFacilityByCoords(int.Parse(coords[1]), int.Parse(coords[2]));
            var index = baseData.GetFacilityIndexByCoord(facility);

            facility.BuildTime = 0;
            baseData.Facilities[index] = facility;

            saveGame.updateBase(baseData);
        }

        internal void RemoveAlienPact(SaveGame saveGame)
        {
            saveGame.RemoveAlienPact();
        }

        internal bool isBattleSave(SaveGame saveGame) 
        {
            return saveGame.isBattleSave();
        }

        internal int GetItemQuantity(SaveGame saveGame, int currentSelectedBase, string itemName)
        {
            return saveGame.getCurrentItemQty(currentSelectedBase, itemName);
        }

        internal void SetItemQuantity(SaveGame saveGame, int currentSelectedBase, string itemName, decimal qty)
        {
            saveGame.saveItemQuantity(currentSelectedBase,  itemName, qty);
        }

        internal List<Research> GetResearches(SaveGame saveGame, int currentSelectedBase) 
        {
            return saveGame.Bases[currentSelectedBase - 1].Researches;
        }

        internal Research GetResearch(SaveGame saveGame, int currentSelectedBase, string researchName) 
        {
            return saveGame.getResearch(currentSelectedBase, researchName);
        }

        internal void SetResearchAsCompleted(SaveGame saveGame, int currentSelectedBase, string researchName) 
        {
            saveGame.completeResearch(currentSelectedBase, researchName);
        }

        internal List<CraftData> GetCraftData(SaveGame saveGame, int currentSelectedBase)
        {
            return saveGame.Bases[currentSelectedBase - 1].CraftData;
        }

        internal void fullyRechargeAmmo(SaveGame saveGame, int currentSelectedBase, string vehicleID) 
        {
           saveGame.fullyRechargeAmmo(currentSelectedBase, vehicleID);
        }

        internal void fullyRechargeProperty(SaveGame saveGame, int currentSelectedBase, string vehicleID, int type, CraftData craftData) 
        {
            saveGame.fullyRechargeProperty(currentSelectedBase, vehicleID, type, craftData);
        }

        internal List<ProductionItem> GetProductions(SaveGame saveGame, int baseNumber)
        {
            return saveGame.GetProductions(baseNumber);
        }

        internal void SetProductionAsCompleted(SaveGame saveGame, int baseNumber, ProductionItem item)
        {
            saveGame.SetProductionAsCompleted(baseNumber, item);
        }
    }
}