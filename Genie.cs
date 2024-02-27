using System.Drawing;
using System.Resources;
using X_PirateZ_Genie;
using XPirateZ_Genie2._0.Utils;
using Button = System.Windows.Forms.Button;

namespace XPirateZ_Genie2._0
{
    public partial class Genie : Form
    {
        private enum LoadType
        {
            LOAD, PREVIOUS, TURBOWIN, TURBOWIN_STUN
        }
        private SaveGame save;
        private SaveEditor saveEditor;
        private int currentSelectedBase;
        private int slotClicked;
        private string rightClickTag;
        public Genie()
        {
            InitializeComponent();
            this.toolStrip1.Renderer = new BorderlessToolstrip();
            InitializeState();
        }

        private void InitializeState()
        {
            geniePanel.Visible = false;
            basesPanel.Visible = false;
            soldierPanel.Visible = false;
            researchPanel.Visible = false;
            vehiclesPanel.Visible = false;
            productionPanel.Visible = false;

            saveEditor = new SaveEditor();

            soldierTU.ValueChanged += new EventHandler(soldierVal_ValueChanged);
            soldierHEALTH.ValueChanged += new EventHandler(soldierVal_ValueChanged);
            soldierEN.ValueChanged += new EventHandler(soldierVal_ValueChanged);
            soldierBRAV.ValueChanged += new EventHandler(soldierVal_ValueChanged);
            soldierACC.ValueChanged += new EventHandler(soldierVal_ValueChanged);
            soldierSTR.ValueChanged += new EventHandler(soldierVal_ValueChanged);
            soldierTHR.ValueChanged += new EventHandler(soldierVal_ValueChanged);
            soldierMELEE.ValueChanged += new EventHandler(soldierVal_ValueChanged);
            soldierFRESH.ValueChanged += new EventHandler(soldierVal_ValueChanged);
            soldierREACTIONS.ValueChanged += new EventHandler(soldierVal_ValueChanged);
            soldierVDA.ValueChanged += new EventHandler(soldierVal_ValueChanged);
            soldierVDP.ValueChanged += new EventHandler(soldierVal_ValueChanged);

            for (int i = 1; i <= 8; i++)
            {
                basesPanel.Controls["base" + i + "Button"].BackColor = Color.Brown;
                basesPanel.Controls["base" + i + "Button"].Enabled = false;
                basesPanel.Controls["base" + i + "Button"].Click += new EventHandler(baseNumber_Click);
            }

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    basesPanel.Controls["b" + i + "" + j].Click += new EventHandler(baseSlotClick);
                    basesPanel.Controls["b" + i + "" + j].MouseDown += new MouseEventHandler(baseSlotRightClick);
                }
            }
        }

        private void baseSlotRightClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && (sender as Button).Tag.ToString().Contains("STR_"))
            {
                rightClickTag = (sender as Button).Tag.ToString();
            }
        }

        private void baseNumber_Click(object sender, System.EventArgs e)
        {
            StatusModified(0);
            GenerateBaseUI(sender);
            StatusModified(1);
        }

        private void baseSlotClick(object sender, EventArgs e)
        {
            saveEditor.AddClickedBaseSlot(save.Bases[currentSelectedBase - 1], (sender as Button).Tag as string, slotClicked);

            slotClicked++;
            if (slotClicked.Equals(2))
            {
                StatusModified(0);
                saveEditor.SwapSlotIfPossible(save, save.Bases[currentSelectedBase - 1]);
                saveEditor.CleanClickedBaseSlots();
                DrawBase();
                SaveGame();
                StatusModified(1);
                slotClicked = 0;
            }
        }

        private void soldierVal_ValueChanged(object sender, EventArgs e)
        {
            var name = comboBox1.SelectedItem.ToString();
            if (!name.Equals(string.Empty))
            {
                StatusModified(0);
                saveEditor.SaveSoldierStats(
                    save,
                    name,
                    soldierTU.Value,
                    soldierHEALTH.Value,
                    soldierEN.Value,
                    soldierBRAV.Value,
                    soldierACC.Value,
                    soldierSTR.Value,
                    soldierTHR.Value,
                    soldierMELEE.Value,
                    soldierFRESH.Value,
                    soldierREACTIONS.Value,
                    soldierVDA.Value,
                    soldierVDP.Value
                );
                SaveGame();
                StatusModified(1);
            }
        }

        private void SaveGame()
        {
            saveEditor.Save(save);
        }

        private void StatusModified(int type)
        {
            // toolStripStatusLabel1.Image = new Bitmap(type.Equals(0) ? Properties.Resources.bullet_red : Properties.Resources.bullet_green);
            // toolStripStatusLabel1.Text = type.Equals(0) ? "Status: not saved" : "Status: saved";
        }

        private void GenerateBaseUI(object sender)
        {
            items.Items.Clear();
            currentSelectedBase = int.Parse((sender as Button).Text);
            Base baseData = save.Bases[currentSelectedBase - 1];
            baseName.Text = baseData.Name;
            foreach (Item item in baseData.Items)
            {
                items.Items.Add(item);
            }
            DrawBase();
        }

        private void DrawBase()
        {
            ResetBaseSlots();

            Base baseData = save.Bases[currentSelectedBase - 1];
            var facilities = baseData.Facilities;

            foreach (Facility facility in facilities)
            {
                basesPanel.Controls["b" + facility.Y + "" + facility.X].Tag = facility.Type + "," + facility.X + "," + facility.Y;
                basesPanel.Controls["b" + facility.Y + "" + facility.X].Text = facility.BuildTime > 0 ? facility.BuildTime.ToString() : "";
                basesPanel.Controls["b" + facility.Y + "" + facility.X].BackColor = Color.DarkSalmon;
                basesPanel.Controls["b" + facility.Y + "" + facility.X].BackgroundImage = Base.GetFacilitySprite(facility);
                basesPanel.Controls["b" + facility.Y + "" + facility.X].Width = 110 * facility.Dim + facility.Dim - 1;
                basesPanel.Controls["b" + facility.Y + "" + facility.X].Height = 110 * facility.Dim + facility.Dim - 1;
                toolTip1.SetToolTip(basesPanel.Controls["b" + facility.Y + "" + facility.X], facility.Type.Replace("STR_", "").Replace("_", " "));

                if (facility.BuildTime > 0)
                {
                    basesPanel.Controls["b" + facility.Y + "" + facility.X].ContextMenuStrip = contextMenuStrip1;
                }

                if (facility.Dim > 1)
                {
                    for (int i = 0; i < facility.Dim; i++)
                    {
                        for (int j = 0; j < facility.Dim; j++)
                        {
                            if (i != 0 || j != 0)
                            {
                                basesPanel.Controls["b" + (facility.Y + i) + "" + (facility.X + j)].Visible = false;
                                basesPanel.Controls["b" + (facility.Y + i) + "" + (facility.X + j)].Enabled = false;
                            }
                        }
                    }
                }
            }
        }

        private void LoadAction(LoadType type)
        {
            if (type.Equals(LoadType.LOAD))
            {
                LoadFromFile();
                FillData();
                MessageBox.Show("Save Loaded");
            }
            else if (type.Equals(LoadType.PREVIOUS))
            {
                Properties.Settings.Default.Reload();
                Thread.Sleep(1000);
                if (Properties.Settings.Default["LastFile"].ToString() == "")
                {
                    LoadFromFile();
                    FillData();
                    MessageBox.Show("Save Loaded");
                }
                else
                {
                    save = saveEditor.Load(Properties.Settings.Default["LastFile"].ToString());
                    FillData();
                    MessageBox.Show("Save Loaded");
                }
            }
            else if (type.Equals(LoadType.TURBOWIN))
            {
                LoadAction(LoadType.PREVIOUS);
                saveEditor.KillAllEnemies(save);
                saveEditor.MaxExperience(save);
                SaveGame();
                MessageBox.Show("Save Updated");
            }
            else if (type.Equals(LoadType.TURBOWIN_STUN))
            {
                LoadAction(LoadType.PREVIOUS);
                saveEditor.StunAllEnemies(save);
                saveEditor.MaxExperience(save);
                SaveGame();
                MessageBox.Show("Save Updated");
            }
        }

        private void FillData()
        {
            SetMoney();
            SetSoldiers();
            DisableActionIfNotBattleSave();
            geniePanel.Visible = true;

            ResetBaseButtons();
            ResetBaseSlots();
            basesPanel.Visible = true;

            soldierPanel.Visible = true;

            ResetResearchPanel();
            researchPanel.Visible = true;

            ResetVehiclesPanel();
            vehiclesPanel.Visible = true;

            ResetProductionPanel();
            productionPanel.Visible = true;
        }

        private void ResetProductionPanel()
        {
            for (int i = 1; i <= 8; i++)
            {
                var baseButton = (productionPanel.Controls["baseProduction" + i] as Button);
                baseButton.BackColor = Color.Maroon;
                baseButton.Enabled = false;
                baseButton.Tag = i;
                baseButton.Click += BaseProductionButton_Click;
            }
            for (int i = 1; i <= save.Bases.Count; i++)
            {
                var baseButton = (productionPanel.Controls["baseProduction" + i] as Button);
                baseButton.BackColor = Color.AliceBlue;
                baseButton.Enabled = true;
            }

            researchesListBox.Items.Clear();
        }

        private void BaseProductionButton_Click(object? sender, EventArgs e)
        {
            FillProductionListBox(int.Parse((sender as Button).Tag.ToString()));
        }

        private void FillProductionListBox(int baseNumber)
        {
            productionListBox.Items.Clear();
            productionListBox.Tag = baseNumber;
            foreach (ProductionItem item in saveEditor.GetProductions(save, baseNumber))
            {
                if (item.Assigned > 0)
                {
                    productionListBox.Items.Add(item);
                }
            }
        }

        private void ResetVehiclesPanel()
        {
            for (int i = 1; i <= 8; i++)
            {
                var baseButton = (vehiclesPanel.Controls["base" + i + "VehiclesBtn"] as Button);
                baseButton.BackColor = Color.Maroon;
                baseButton.Enabled = false;
                baseButton.Tag = i;
                baseButton.Click += BaseVehiclesButton_Click;
            }
            for (int i = 1; i <= save.Bases.Count; i++)
            {
                var baseButton = (vehiclesPanel.Controls["base" + i + "VehiclesBtn"] as Button);
                baseButton.BackColor = Color.AliceBlue;
                baseButton.Enabled = true;
            }

            vehicleListCBox.Items.Clear();
        }

        private void ResetResearchPanel()
        {
            for (int i = 1; i <= 8; i++)
            {
                var baseButton = (researchPanel.Controls["base" + i + "Research"] as Button);
                baseButton.BackColor = Color.Maroon;
                baseButton.Enabled = false;
                baseButton.Tag = i;
                baseButton.Click += BaseResearchButton_Click;
            }
            for (int i = 1; i <= save.Bases.Count; i++)
            {
                var baseButton = (researchPanel.Controls["base" + i + "Research"] as Button);
                baseButton.BackColor = Color.AliceBlue;
                baseButton.Enabled = true;
            }

            researchesListBox.Items.Clear();
        }

        private void BaseResearchButton_Click(object? sender, EventArgs e)
        {
            FillResearchListBox(int.Parse((sender as Button).Tag.ToString()));
        }

        private void BaseVehiclesButton_Click(object? sender, EventArgs e)
        {
            FillVehiclesListBox(int.Parse((sender as Button).Tag.ToString()));
        }

        private void FillVehiclesListBox(int baseNumber)
        {
            vehicleListCBox.Items.Clear();
            vehicleListCBox.Tag = baseNumber;
            foreach (CraftData craftData in saveEditor.GetCraftData(save, baseNumber))
            {
                vehicleListCBox.Items.Add(craftData);
            }
        }

        private void FillResearchListBox(int baseNumber)
        {
            researchesListBox.Items.Clear();
            researchesListBox.Tag = baseNumber;
            foreach (Research research in saveEditor.GetResearches(save, baseNumber))
            {
                if (research.Assigned > 0)
                {
                    researchesListBox.Items.Add(research);
                }
            }
        }

        private void SetMoney()
        {
            moneyValueUpDown.Value = save.Money;
        }

        private void SetSoldiers()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(save.GetSoldierNames());
        }

        private void DisableActionIfNotBattleSave()
        {
            var enable = saveEditor.isBattleSave(save);

            maxEperienceButton.Enabled = enable;
            maxMovementButton.Enabled = enable;
            healAllButton.Enabled = enable;
            stunAllButton.Enabled = enable;
            killAllButton.Enabled = enable;

        }

        private void ResetBaseButtons()
        {
            for (int i = 1; i <= 8; i++)
            {
                var baseButton = (basesPanel.Controls["base" + i + "Button"] as Button);
                baseButton.BackColor = Color.Maroon;
                baseButton.Enabled = false;
            }
            for (int i = 1; i <= save.Bases.Count; i++)
            {
                var baseButton = (basesPanel.Controls["base" + i + "Button"] as Button);
                baseButton.BackColor = Color.AliceBlue;
                baseButton.Enabled = true;
            }
        }

        private void ResetBaseSlots()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    basesPanel.Controls["b" + i + "" + j].Text = "";
                    toolTip1.SetToolTip(basesPanel.Controls["b" + i + "" + j], "BEDROCK");
                    basesPanel.Controls["b" + i + "" + j].BackColor = Color.Brown;
                    basesPanel.Controls["b" + i + "" + j].Tag = "STR_EMPTY," + j + "," + i;
                    basesPanel.Controls["b" + i + "" + j].Visible = true;
                    basesPanel.Controls["b" + i + "" + j].Enabled = true;
                    basesPanel.Controls["b" + i + "" + j].Width = 110;
                    basesPanel.Controls["b" + i + "" + j].Height = 110;
                    basesPanel.Controls["b" + i + "" + j].BackgroundImage = Base.GetEmptySprite();
                    basesPanel.Controls["b" + i + "" + j].ContextMenuStrip = null;

                }
            }
        }

        private void LoadFromFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                save = saveEditor.Load(ofd.FileName);
                Properties.Settings.Default["LastFile"] = ofd.FileName;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }
            ofd.Dispose();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            LoadAction(LoadType.LOAD);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            LoadAction(LoadType.PREVIOUS);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            LoadAction(LoadType.TURBOWIN_STUN);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            LoadAction(LoadType.TURBOWIN);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("X-PirateZ Genie v1.0\nAlessandro Gaggia - 2021\nMIT Licence");
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var name = comboBox1.SelectedItem.ToString();
            if (!name.Equals(string.Empty))
            {
                System.Diagnostics.Debug.WriteLine(name);
                Soldier soldier = save.GetSoldier(name);
                System.Diagnostics.Debug.WriteLine(soldier);
                soldierTU.Value = soldier.TU;
                soldierHEALTH.Value = soldier.HE;
                soldierEN.Value = soldier.EN;
                soldierBRAV.Value = soldier.BR;
                soldierACC.Value = soldier.AC;
                soldierSTR.Value = soldier.ST;
                soldierTHR.Value = soldier.TH;
                soldierMELEE.Value = soldier.ME;
                soldierFRESH.Value = soldier.FR;
                soldierREACTIONS.Value = soldier.RE;
                soldierVDA.Value = soldier.VA;
                soldierVDP.Value = soldier.VP;
            }
        }

        private void maxOutButton_Click(object sender, System.EventArgs e)
        {
            StatusModified(0);
            soldierTU.Value = 255;
            soldierHEALTH.Value = 255;
            soldierEN.Value = 255;
            soldierBRAV.Value = 255;
            soldierACC.Value = 255;
            soldierSTR.Value = 255;
            soldierTHR.Value = 255;
            soldierMELEE.Value = 255;
            soldierFRESH.Value = 255;
            soldierREACTIONS.Value = 255;
            soldierVDA.Value = 255;
            soldierVDP.Value = 255;
            SaveGame();
            StatusModified(1);
        }

        private void moneyValueUpDown_ValueChanged(object sender, EventArgs e)
        {
            StatusModified(0);
            saveEditor.ChangeMoney(save, (int)moneyValueUpDown.Value);
            SaveGame();
            StatusModified(1);
        }

        private void removeAlienPact_Click(object sender, System.EventArgs e)
        {
            StatusModified(0);
            saveEditor.RemoveAlienPact(save);
            SaveGame();
            StatusModified(1);
        }

        private void stunAllButton_Click(object sender, EventArgs e)
        {
            StatusModified(0);
            saveEditor.StunAllEnemies(save);
            SaveGame();
            StatusModified(1);
        }

        private void killAllButton_Click(object sender, EventArgs e)
        {
            StatusModified(0);
            saveEditor.KillAllEnemies(save);
            SaveGame();
            StatusModified(1);
        }

        private void maxEperienceButton_Click(object sender, EventArgs e)
        {
            StatusModified(0);
            saveEditor.MaxExperience(save);
            SaveGame();
            StatusModified(1);
        }

        private void maxMovementButton_Click(object sender, EventArgs e)
        {
            StatusModified(0);
            saveEditor.MaxMovement(save);
            SaveGame();
            StatusModified(1);
        }

        private void maxMovAndExpBTN_Click(object sender, EventArgs e)
        {
            StatusModified(0);
            saveEditor.MaxExperience(save);
            saveEditor.MaxMovement(save);
            SaveGame();
            StatusModified(1);
        }

        private void healAllButton_Click(object sender, EventArgs e)
        {
            StatusModified(0);
            saveEditor.HealAll(save);
            SaveGame();
            StatusModified(1);
        }

        private void buildNowMenuItem_Click(object sender, System.EventArgs e)
        {
            StatusModified(0);
            saveEditor.BuildNow(save, currentSelectedBase, rightClickTag);
            DrawBase();
            SaveGame();
            StatusModified(1);
        }

        private void items_SelectedIndexChanged(object sender, EventArgs e)
        {
            StatusModified(0);
            int qty = saveEditor.GetItemQuantity(save, currentSelectedBase, ((Item)items.SelectedItem).ID);
            itemQty.Value = qty;
            SaveGame();
            StatusModified(1);
        }

        private void itemQty_ValueChanged(object sender, EventArgs e)
        {
            if (items.SelectedItem != null && items.SelectedItem.ToString() != String.Empty)
            {
                StatusModified(0);
                saveEditor.SetItemQuantity(save, currentSelectedBase, ((Item)items.SelectedItem).ID, itemQty.Value);
                SaveGame();
                StatusModified(1);
            }

        }

        private void baseName_TextChanged(object sender, EventArgs e)
        {
            StatusModified(0);
            saveEditor.ChangeBaseName(save, currentSelectedBase, baseName.Text);
            SaveGame();
            StatusModified(1);
        }

        private void makeResearchExcelentButton_Click(object sender, EventArgs e)
        {
            StatusModified(0);
            Research research = (Research)researchesListBox.SelectedItem;
            if (research != null)
            {
                saveEditor.SetResearchAsCompleted(save, int.Parse(researchesListBox.Tag.ToString()), research.Project);
                ((Research)researchesListBox.SelectedItem).Spent = ((Research)researchesListBox.SelectedItem).Cost;
                SaveGame();
                FillResearchListBox(int.Parse(researchesListBox.Tag.ToString()));
            }
            StatusModified(1);
        }

        private void vehicleListCBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ammoSlot1.Value = 0;
            ammoSlot2.Value = 0;
            ammoSlot3.Value = 0;
            ammoslot4.Value = 0;
            ammoSlot1.Visible = false;
            ammoSlot2.Visible = false;
            ammoSlot3.Visible = false;
            ammoslot4.Visible = false;
            vehicleImage.BackgroundImage = null;
            vehicleLife.Value = 0;
            fuelValue.Value = 0;
            shieldValue.Value = 0;
            vehicleLife.Maximum = 9999;
            shieldValue.Maximum = 9999;
            fuelValue.Maximum = 9999;
            slot1Name.Text = "";
            slot2Name.Text = "";
            slot3Name.Text = "";
            slot4Name.Text = "";
            List<NumericUpDown> slots = new List<NumericUpDown>();
            slots.Add(ammoSlot1);
            slots.Add(ammoSlot2);
            slots.Add(ammoSlot3);
            slots.Add(ammoslot4);
            List<Label> names = new List<Label>();
            names.Add(slot1Name);
            names.Add(slot2Name);
            names.Add(slot3Name);
            names.Add(slot4Name);

            CraftData craftData = vehicleListCBox.SelectedItem as CraftData;

            if (craftData != null)
            {
                // Retrieve the bitmap from the resources
                Bitmap bitmap = Properties.Resources.ResourceManager.GetObject(craftData.Sprite) as Bitmap;
                vehicleImage.BackgroundImage = bitmap;

                vehicleLife.Maximum = craftData.craftSpecs.DamageMax;
                vehicleLife.Minimum = 0;
                vehicleLife.Value = vehicleLife.Maximum - craftData.Damage;

                fuelValue.Minimum = 0;
                fuelValue.Maximum = craftData.craftSpecs.FuelMax;
                fuelValue.Value = craftData.Fuel;

                shieldValue.Minimum = 0;
                shieldValue.Maximum = craftData.craftSpecs.ShieldCapacity;
                shieldValue.Value = craftData.Shield;

                for (var i = 0; i < craftData.wSlots.Count; i++)
                {
                    slots[i].Visible = true;
                    slots[i].Value = craftData.wSlots[i].ammo;
                    names[i].Text = craftData.wSlots[i].name;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StatusModified(0);
            CraftData craftData = vehicleListCBox.SelectedItem as CraftData;
            saveEditor.fullyRechargeAmmo(save, int.Parse(vehicleListCBox.Tag.ToString()), craftData.ID);
            ammoSlot1.Value = 999;
            ammoSlot2.Value = 999;
            ammoSlot3.Value = 999;
            ammoslot4.Value = 999;
            for (var j = 0; j < vehicleListCBox.Items.Count; j++)
            {
                if ((vehicleListCBox.Items[j] as CraftData).ID == craftData.ID)
                {
                    for (var i = 0; i < (vehicleListCBox.Items[j] as CraftData).wSlots.Count; i++)
                    {
                        WSlot w = (vehicleListCBox.Items[j] as CraftData).wSlots[i];
                        w.ammo = 999;
                        (vehicleListCBox.Items[j] as CraftData).wSlots[i] = w;
                    }
                }
            }
            SaveGame();
            StatusModified(1);
        }

        private void repairButton_Click(object sender, EventArgs e)
        {
            StatusModified(0);
            CraftData craftData = vehicleListCBox.SelectedItem as CraftData;
            saveEditor.fullyRechargeProperty(save, int.Parse(vehicleListCBox.Tag.ToString()), craftData.ID, 0, craftData);
            vehicleLife.Value = vehicleLife.Maximum;
            for (var j = 0; j < vehicleListCBox.Items.Count; j++)
            {
                if ((vehicleListCBox.Items[j] as CraftData).ID == craftData.ID)
                {
                    (vehicleListCBox.Items[j] as CraftData).Damage = 0;
                }
            }
            SaveGame();
            StatusModified(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StatusModified(0);
            CraftData craftData = vehicleListCBox.SelectedItem as CraftData;
            saveEditor.fullyRechargeProperty(save, int.Parse(vehicleListCBox.Tag.ToString()), craftData.ID, 2, craftData);
            fuelValue.Value = fuelValue.Maximum;
            for (var j = 0; j < vehicleListCBox.Items.Count; j++)
            {
                if ((vehicleListCBox.Items[j] as CraftData).ID == craftData.ID)
                {
                    (vehicleListCBox.Items[j] as CraftData).Fuel = (vehicleListCBox.Items[j] as CraftData).craftSpecs.FuelMax;
                }
            }
            SaveGame();
            StatusModified(1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StatusModified(0);
            CraftData craftData = vehicleListCBox.SelectedItem as CraftData;
            saveEditor.fullyRechargeProperty(save, int.Parse(vehicleListCBox.Tag.ToString()), craftData.ID, 1, craftData);
            shieldValue.Value = shieldValue.Maximum;
            for (var j = 0; j < vehicleListCBox.Items.Count; j++)
            {
                if ((vehicleListCBox.Items[j] as CraftData).ID == craftData.ID)
                {
                    (vehicleListCBox.Items[j] as CraftData).Shield = (vehicleListCBox.Items[j] as CraftData).craftSpecs.ShieldCapacity;
                }
            }
            SaveGame();
            StatusModified(1);
        }

        private void makeAllExcellentNowBtn_Click(object sender, EventArgs e)
        {
            StatusModified(0);
            for (var i = 0; i < researchesListBox.Items.Count; i++)
            {
                Research research = (Research)researchesListBox.Items[i];
                if (research != null)
                {
                    saveEditor.SetResearchAsCompleted(save, int.Parse(researchesListBox.Tag.ToString()), research.Project);
                    ((Research)research).Spent = ((Research)research).Cost;
                    SaveGame();
                    FillResearchListBox(int.Parse(researchesListBox.Tag.ToString()));
                }
            }
            StatusModified(1);
        }

        private void productionCompleteAllBtn_Click(object sender, EventArgs e)
        {
            StatusModified(0);
            for (var i = 0; i < productionListBox.Items.Count; i++)
            {
                ProductionItem item = (ProductionItem)productionListBox.Items[i];
                if (item != null)
                {
                    saveEditor.SetProductionAsCompleted(save, int.Parse(productionListBox.Tag.ToString()), item);
                    ((ProductionItem)item).Spent = ((ProductionItem)item).Cost * ((ProductionItem)item).Amount;
                    SaveGame();
                    FillProductionListBox(int.Parse(productionListBox.Tag.ToString()));
                }
            }
            StatusModified(1);
        }

        private void productionCompleteSelectedBTN_Click(object sender, EventArgs e)
        {
            StatusModified(0);
            ProductionItem item = (ProductionItem)productionListBox.SelectedItem;
            if (item != null)
            {
                saveEditor.SetProductionAsCompleted(save, int.Parse(productionListBox.Tag.ToString()), item);
                ((ProductionItem)productionListBox.SelectedItem).Spent = ((ProductionItem)productionListBox.SelectedItem).Cost * ((ProductionItem)productionListBox.SelectedItem).Amount;
                SaveGame();
                FillProductionListBox(int.Parse(productionListBox.Tag.ToString()));
            }
            StatusModified(1);
        }
    }
}
