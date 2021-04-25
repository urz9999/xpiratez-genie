using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace X_PirateZ_Genie
{
    public partial class Form : System.Windows.Forms.Form
    {
        private enum LoadType { LOAD, PREVIOUS }
        private SaveGame save;
        private SaveEditor saveEditor;
        private int currentSelectedBase;
        private int slotClicked;
        private string rightClickTag;

        public Form()
        {
            InitializeComponent();
            InitializeState();
        }

        private void InitializeState()
        {
            geniePanel.Visible = false;
            basesPanel.Visible = false;

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

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Tag) 
            {
                case "ABOUT": MessageBox.Show("X-PirateZ Genie v1.0\nAlessandro Gaggia - 2021\nMIT Licence");  break;
                case "EXIT": Application.Exit();  break;
                case "LOAD": LoadAction(LoadType.LOAD);  break;
                case "LOADPREVIOUS": LoadAction(LoadType.PREVIOUS); break;
            }
        }

        private void LoadAction(LoadType type) 
        {
            if (type.Equals(LoadType.LOAD))
            {
                LoadFromFile();
                FillData();
            }
            else if (type.Equals(LoadType.PREVIOUS)) 
            {
                Properties.Settings.Default.Reload();
                Thread.Sleep(1000);
                if (Properties.Settings.Default["LastFile"].ToString() == "")
                {
                    LoadFromFile();
                    FillData();
                }
                else
                {
                    save = saveEditor.Load(Properties.Settings.Default["LastFile"].ToString());
                    FillData();
                }
            }
        }

        private void FillData()
        {
            moneyValueUpDown.Value = save.Money;
            comboBox1.Items.AddRange(save.GetSoldierNames());
            geniePanel.Visible = true;
            
            ResetBaseButtons();
            ResetBaseSlots();
            basesPanel.Visible = true;
        }

        private void ResetBaseButtons()
        {
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
                    basesPanel.Controls["b" + i + "" + j].Width = 64;
                    basesPanel.Controls["b" + i + "" + j].Height = 64;
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

        private void baseNumber_Click(object sender, System.EventArgs e)
        {
            StatusModified(0);
            GenerateBaseUI(sender);
            StatusModified(1);
        }

        private void GenerateBaseUI(object sender)
        {
            currentSelectedBase = int.Parse((sender as Button).Text);
            Base baseData = save.Bases[currentSelectedBase - 1];
            baseName.Text = baseData.Name;
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
                basesPanel.Controls["b" + facility.Y + "" + facility.X].Width  = 64 * facility.Dim + facility.Dim - 1;
                basesPanel.Controls["b" + facility.Y + "" + facility.X].Height = 64 * facility.Dim + facility.Dim - 1;
                toolTip1.SetToolTip(basesPanel.Controls["b" + facility.Y + "" + facility.X], facility.Type.Replace("STR_","").Replace("_", " "));

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

        private void removeAlienPact_Click(object sender, System.EventArgs e)
        {
            StatusModified(0);
            saveEditor.RemoveAlienPact(save);
            SaveGame();
            StatusModified(1);
        }

        private void stunAll_Click(object sender, System.EventArgs e)
        {
            StatusModified(0);
            saveEditor.StunAllEnemies(save);
            SaveGame();
            StatusModified(1);
        }

        private void killAll_Click(object sender, System.EventArgs e)
        {
            StatusModified(0);
            saveEditor.KillAllEnemies(save);
            SaveGame();
            StatusModified(1);
        }

        private void maxEperienceButton_Click(object sender, System.EventArgs e)
        {
            StatusModified(0);
            saveEditor.MaxExperience(save);
            SaveGame();
            StatusModified(1);
        }

        private void MaxMovementButton_Click(object sender, System.EventArgs e)
        {
            StatusModified(0);
            saveEditor.MaxMovement(save);
            SaveGame();
            StatusModified(1);
        }

        private void maxOutButton_Click(object sender, System.EventArgs e)
        {
            StatusModified(0);
            soldierTU.Value     = 255;
            soldierHEALTH.Value = 255;
            soldierEN.Value     = 255;
            soldierBRAV.Value   = 255;
            soldierACC.Value    = 255;
            soldierSTR.Value    = 255;
            soldierTHR.Value    = 255;
            soldierMELEE.Value  = 255;
            soldierFRESH.Value  = 255;
            soldierREACTIONS.Value   = 255;
            soldierVDA.Value    = 255;
            soldierVDP.Value    = 255;
            SaveGame();
            StatusModified(1);
        }

        private void moneyValueUpDown_ValueChanged(object sender, EventArgs e)
        {
            StatusModified(0);
            saveEditor.ChangeMoney(save, (int) moneyValueUpDown.Value);
            SaveGame();
            StatusModified(1);
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

        private void baseName_KeyUp(object sender, KeyEventArgs e)
        {
            StatusModified(0);
            saveEditor.ChangeBaseName(save, currentSelectedBase, baseName.Text);
            SaveGame();
            StatusModified(1);
        }

        private void BuildNowMenuItem_Click(object sender, System.EventArgs e)
        {
            StatusModified(0);
            saveEditor.BuildNow(save, currentSelectedBase, rightClickTag);
            DrawBase();
            SaveGame();
            StatusModified(1);
        }

        private void StatusModified(int type)
        {
            toolStripStatusLabel1.Image = new Bitmap(type.Equals(0) ? Properties.Resources.bullet_red : Properties.Resources.bullet_green);
            toolStripStatusLabel1.Text = type.Equals(0) ? "Status: not saved" : "Status: saved";
        }

        private void SaveGame()
        {
            saveEditor.Save(save);
        }

        private void healAllButton_Click(object sender, EventArgs e)
        {
            StatusModified(0);
            saveEditor.HealAll(save);
            SaveGame();
            StatusModified(1);
        }
    }
}
