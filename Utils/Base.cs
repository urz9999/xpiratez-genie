using XPirateZ_Genie2._0.Utils;
using YamlDotNet.RepresentationModel;

namespace X_PirateZ_Genie
{
    internal class Base
    {
        internal string Name { get; set; }
        internal List<Facility> Facilities { get; set; }

        internal List<Item> Items { get; set; }

        internal List<Research> Researches { get; set; }

        internal List<CraftData> CraftData { get; set; }

        internal List<ProductionItem> Manufactures { get; set; }

        private static FacilitySprite[] facilitiesIds = new FacilitySprite[] 
        {
            new FacilitySprite("STR_ACCESS_LIFT", 1, XPirateZ_Genie2._0.Properties.Resources.STR_ACCESS_LIFT),
            new FacilitySprite("STR_CORRIDOR", 1, XPirateZ_Genie2._0.Properties.Resources.STR_CORRIDOR),
            new FacilitySprite("STR_BURROW", 1, XPirateZ_Genie2._0.Properties.Resources.STR_BURROW),
            new FacilitySprite("STR_BEAST_DEN", 1, XPirateZ_Genie2._0.Properties.Resources.STR_BEAST_DEN),
            new FacilitySprite("STR_LIVING_QUARTERS_SMALL", 1, XPirateZ_Genie2._0.Properties.Resources.STR_LIVING_QUARTERS_SMALL),
            new FacilitySprite("STR_LIVING_QUARTERS", 1, XPirateZ_Genie2._0.Properties.Resources.STR_LIVING_QUARTERS),
            new FacilitySprite("STR_LIVING_QUARTERS_LARGE", 2, XPirateZ_Genie2._0.Properties.Resources.STR_LIVING_QUARTERS_BIG),
            new FacilitySprite("STR_LIVING_QUARTERS_ADVANCED", 1, XPirateZ_Genie2._0.Properties.Resources.STR_LIVING_QUARTERS_ADVANCED),
            new FacilitySprite("STR_GENERAL_STORES", 1, XPirateZ_Genie2._0.Properties.Resources.STR_VAULT),
            new FacilitySprite("STR_GENERAL_STORES_LARGE", 2, XPirateZ_Genie2._0.Properties.Resources.STR_VAULT_BIG),
            new FacilitySprite("STR_GENERAL_STORES_ARMORED", 1, XPirateZ_Genie2._0.Properties.Resources.STR_VAULT_ARMORED),
            new FacilitySprite("STR_JAIL", 1, XPirateZ_Genie2._0.Properties.Resources.STR_JAIL),
            new FacilitySprite("STR_PRISON", 2, XPirateZ_Genie2._0.Properties.Resources.STR_JAIL_BIG),
            new FacilitySprite("STR_CRYO_PRISON", 1, XPirateZ_Genie2._0.Properties.Resources.STR_JAIL_CRYO),
            new FacilitySprite("STR_SMALL_RADAR_SYSTEM", 1, XPirateZ_Genie2._0.Properties.Resources.STR_RADAR_SMALL),
            new FacilitySprite("STR_LARGE_RADAR_SYSTEM", 1, XPirateZ_Genie2._0.Properties.Resources.STR_RADAR_BIG),
            new FacilitySprite("STR_ALENIUM_CHAMBER", 1, XPirateZ_Genie2._0.Properties.Resources.STR_ALENIUM_CHAMBER),
            new FacilitySprite("STR_HYPER_WAVE_DECODER", 1, XPirateZ_Genie2._0.Properties.Resources.STR_HYPERWAVE_DECODER),
            new FacilitySprite("STR_HANGAR", 2, XPirateZ_Genie2._0.Properties.Resources.STR_HANGARS),
            new FacilitySprite("STR_EYE_OF_HORUS", 1, XPirateZ_Genie2._0.Properties.Resources.STR_EYE_OF_HORUS),
            new FacilitySprite("STR_FLAK_DEFENSES", 1, XPirateZ_Genie2._0.Properties.Resources.STR_FLACK_DEFENCE),
            new FacilitySprite("STR_FLAK_TOWER", 1,XPirateZ_Genie2._0. Properties.Resources.STR_FLACK_TOWER),
            new FacilitySprite("STR_SAM_DEFENSES", 1, XPirateZ_Genie2._0.Properties.Resources.STR_SAM_DEFENCE),
            new FacilitySprite("STR_GAUSS_DEFENSES", 1, XPirateZ_Genie2._0.Properties.Resources.STR_GAUSS_DEFENCE),         
            new FacilitySprite("STR_MISSILE_DEFENSES", 1, XPirateZ_Genie2._0.Properties.Resources.STR_MISSILE_DEFENCE),
            new FacilitySprite("STR_LASER_DEFENSES", 1, XPirateZ_Genie2._0.Properties.Resources.STR_LASER_DEFENCE),
            new FacilitySprite("STR_PLASMA_DEFENSES", 1, XPirateZ_Genie2._0.Properties.Resources.STR_PLASMA_DEFENCE),
            new FacilitySprite("STR_FUSION_BALL_DEFENSES", 1, XPirateZ_Genie2._0.Properties.Resources.STR_FUSION_DEFENCE),
            new FacilitySprite("STR_ARMORY", 1, XPirateZ_Genie2._0.Properties.Resources.STR_ARMORY),
            new FacilitySprite("STR_STILL", 1, XPirateZ_Genie2._0.Properties.Resources.STR_STILL),
            new FacilitySprite("STR_GRAV_SHIELD", 1, XPirateZ_Genie2._0.Properties.Resources.STR_GRAV_SHIELD),
            new FacilitySprite("STR_GYM_ROOM", 1, XPirateZ_Genie2._0.Properties.Resources.STR_GYM),
            new FacilitySprite("STR_ONSEN", 1, XPirateZ_Genie2._0.Properties.Resources.STR_ONSEN),
            new FacilitySprite("STR_LUXURY_SPA", 2, XPirateZ_Genie2._0.Properties.Resources.STR_LARGE_SPA),
            new FacilitySprite("STR_WELL", 1, XPirateZ_Genie2._0.Properties.Resources.STR_WELL),
            new FacilitySprite("STR_MESS_HALL", 1, XPirateZ_Genie2._0.Properties.Resources.STR_MESS_HALL),
            new FacilitySprite("STR_MED_BAY", 1, XPirateZ_Genie2._0.Properties.Resources.STR_MED_BAY),
            new FacilitySprite("STR_LOVE_CLINIC", 1, XPirateZ_Genie2._0.Properties.Resources.STR_LOVE_CLINIC),
            new FacilitySprite("STR_METAGARDEN", 1, XPirateZ_Genie2._0.Properties.Resources.STR_METAGARDEN),
            new FacilitySprite("STR_CLONING_CENTRE", 1, XPirateZ_Genie2._0.Properties.Resources.STR_CLONING_CENTRE),
            new FacilitySprite("STR_PSIONIC_LABORATORY", 1, XPirateZ_Genie2._0.Properties.Resources.STR_PSYONIC_LAB),
            new FacilitySprite("STR_MIND_SHIELD", 1, XPirateZ_Genie2._0.Properties.Resources.STR_MIND_SHIELD),
            new FacilitySprite("STR_DISRUPTOR_SHROUD", 1, XPirateZ_Genie2._0.Properties.Resources.STR_DISRUPTOR_SHROUD),
            new FacilitySprite("STR_FIRE_PIT", 1, XPirateZ_Genie2._0.Properties.Resources.STR_FIRE_PIT),
            new FacilitySprite("STR_GAS_CHAMBER", 1, XPirateZ_Genie2._0.Properties.Resources.STR_GAS_CHAMBER),
            new FacilitySprite("STR_LABORATORY", 1, XPirateZ_Genie2._0.Properties.Resources.STR_LABORATORY),
            new FacilitySprite("STR_PERSONAL_LABS", 1, XPirateZ_Genie2._0.Properties.Resources.STR_PERSONAL_LAB),
            new FacilitySprite("STR_HYDROPONICS", 2, XPirateZ_Genie2._0.Properties.Resources.STR_PLANTATION),
            new FacilitySprite("STR_HYDROPONICS_BOOM", 2, XPirateZ_Genie2._0.Properties.Resources.STR_BOOM_PLANTATION),
            new FacilitySprite("STR_HYDROPONICS_BOOM_HARVEST", 2, XPirateZ_Genie2._0.Properties.Resources.STR_BOOM_PLANTATION),
            new FacilitySprite("STR_HYDROPONICS_WEED", 2, XPirateZ_Genie2._0.Properties.Resources.STR_WEED_PLANTATION),
            new FacilitySprite("STR_HYDROPONICS_WEED_HARVEST", 2, XPirateZ_Genie2._0.Properties.Resources.STR_WEED_PLANTATION),
            new FacilitySprite("STR_BEE_HIVE", 1, XPirateZ_Genie2._0.Properties.Resources.STR_BEE_HIVE),
            new FacilitySprite("STR_CASINO", 2, XPirateZ_Genie2._0.Properties.Resources.STR_CASINO),
            new FacilitySprite("STR_MINT", 1, XPirateZ_Genie2._0.Properties.Resources.STR_MINT),
            new FacilitySprite("STR_REFINERY", 1, XPirateZ_Genie2._0.Properties.Resources.STR_REFINERY),
            new FacilitySprite("STR_SUMMONING_CIRCLE", 1, XPirateZ_Genie2._0.Properties.Resources.STR_SUMMONING_CIRCLE),
            new FacilitySprite("STR_WORKSHOP", 1, XPirateZ_Genie2._0.Properties.Resources.STR_WORKSHOP),
            new FacilitySprite("STR_POWER_PLANT", 1, XPirateZ_Genie2._0.Properties.Resources.STR_POWER_PLANT),
            new FacilitySprite("STR_FUSION_REACTOR", 1, XPirateZ_Genie2._0.Properties.Resources.STR_FUSION_LAB),
            new FacilitySprite("STR_LIBRARY", 1, XPirateZ_Genie2._0.Properties.Resources.STR_LIBRARY),
            new FacilitySprite("STR_GREAT_LIBRARY", 2, XPirateZ_Genie2._0.Properties.Resources.STR_BIG_LIBRARY),
            new FacilitySprite("STR_SURGERY_ROOM", 1, XPirateZ_Genie2._0.Properties.Resources.STR_SURGERY_ROOM),
            new FacilitySprite("STR_STUDY_ROOM", 1, XPirateZ_Genie2._0.Properties.Resources.STR_STUDY_ROOM),
            new FacilitySprite("STR_DATA_CENTER", 1, XPirateZ_Genie2._0.Properties.Resources.STR_DATA_CENTER),
            new FacilitySprite("STR_COMPUTER_CORE", 1, XPirateZ_Genie2._0.Properties.Resources.STR_COMPUTER_CORE),
            new FacilitySprite("STR_ASTROSENSORIUM", 1, XPirateZ_Genie2._0.Properties.Resources.STR_ASTROSENSORIUM),
            new FacilitySprite("STR_GDX_LAB", 1, XPirateZ_Genie2._0.Properties.Resources.STR_GDX_LAB),
            new FacilitySprite("STR_RED_TOWER", 1, XPirateZ_Genie2._0.Properties.Resources.STR_RED_TOWER),
            new FacilitySprite("STR_INDUSTRIAL_PRINTER", 2, XPirateZ_Genie2._0.Properties.Resources.STR_INDUSTRIAL_PRINTER),
            new FacilitySprite("STR_FACTORY", 3, XPirateZ_Genie2._0.Properties.Resources.STR_FACTORY),
            // N1 Update
            new FacilitySprite("STR_SPACE_RADIO", 1, XPirateZ_Genie2._0.Properties.Resources.STR_SPACE_RADIO),
            new FacilitySprite("STR_VULCAN_DEFENCES", 1, XPirateZ_Genie2._0.Properties.Resources.STR_VULCAN_DEFENCES),
            new FacilitySprite("STR_WAR_BARRACKS", 3, XPirateZ_Genie2._0.Properties.Resources.STR_WAR_BARRACKS),
            new FacilitySprite("STR_HOTEL", 2, XPirateZ_Genie2._0.Properties.Resources.STR_HOTEL),
            // N2 Update
            new FacilitySprite("STR_DUNGEON", 1, XPirateZ_Genie2._0.Properties.Resources.STR_DUNGEON),
            new FacilitySprite("STR_IRRADIATOR_TOWER", 1, XPirateZ_Genie2._0.Properties.Resources.STR_IRRADIATOR_TOWER),
            // N5 Update
             new FacilitySprite("STR_RUBBLE", 1, XPirateZ_Genie2._0.Properties.Resources.STR_RUBBLE),
        };

        internal static int CheckBuildTime(YamlMappingNode facility)
        {
            try
            {
                return int.Parse(facility[new YamlScalarNode("buildTime")].ToString());
            }
            catch (Exception) { return 0; }
        }

        internal Facility GetFacilityByCoords(int x, int y)
        {
            foreach (Facility f in Facilities)
            {
                if (f.X.Equals(x) && f.Y.Equals(y))
                {
                    return f;
                }
            }
            return new Facility() { X = x, Y = y, Dim = 1, BuildTime = 0, Type = "STR_EMPTY" };
        }

        internal static Image GetEmptySprite()
        {
            return (Bitmap) XPirateZ_Genie2._0.Properties.Resources.STR_EMPTY;
        }

        public static int GetFacilityDimension(Facility facility) 
        {
            var foundFacility = facilitiesIds.FirstOrDefault(f => f.Id.Equals(facility.Type));
            return !foundFacility.Equals(null) ? foundFacility.Dim : -1;
        }

        public static int GetFacilityDimension(string type)
        {
            var foundFacility = facilitiesIds.FirstOrDefault(f => f.Id.Equals(type));
            return !foundFacility.Equals(null) ? foundFacility.Dim : -1;
        }

        public bool IsLocationFree(Facility facility, int newX, int newY) 
        {
            var dim = facility.Dim;

            foreach (Facility f in Facilities)
            {
                if (f.X.Equals(newX) && f.Y.Equals(newY)) 
                {
                    var destDim = f.Dim;
                    return destDim.Equals(dim);
                }
            }

            // Not found so clicked on dirty: check if dirty is sufficient
            var check = true;
            for (int i = 0; i < dim; i++) 
            {
                for (int j = 0; j < dim; j++)
                {
                    check &= (GetFacilityByCoords(newX + i, newY + j).Type.Equals("STR_EMPTY") && (newX + i <= 5) && (newY + j <= 5));
                }
            }
            return check;
        }

        internal static Image GetFacilitySprite(Facility facility)
        {
            return (Bitmap) facilitiesIds.FirstOrDefault(fId => fId.Id.Equals(facility.Type)).Sprite;
        }

        public void SwapFacilities(Facility f1, Facility f2) 
        {
            var originalIndex1 = GetFacilityIndexByCoord(f1);
            var originalIndex2 = GetFacilityIndexByCoord(f2);

            var tX = f1.X;
            var tY = f1.Y;

            f1.X = f2.X;
            f1.Y = f2.Y;

            f2.X = tX;
            f2.Y = tY;

            if (originalIndex1 > -1)
                Facilities[originalIndex1] = f1;
            if (originalIndex2 > -1)
                Facilities[originalIndex2] = f2;
        }

        public int GetFacilityIndexByCoord(Facility f)
        {
            return Facilities.FindIndex(fac => fac.X.Equals(f.X) && fac.Y.Equals(f.Y));
        }
    }

    struct Item { 
        public string name { get; set; }
        public int qty { get; set; }

        public string ID { get; set; }

        override
        public string ToString()
        {
            return name;
        }
    }

    struct Facility
    {
        public string Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Dim { get; set; }
        public int BuildTime { get; set; }
    }

    struct FacilitySprite 
    {
        public FacilitySprite(string id, int dim, Object sprite)
        {
            Id = id;
            Dim = dim;
            if (sprite is string) 
            {
                Sprite = null;
            }
            else 
            {
                Sprite = (Bitmap)sprite;
            }
        }

        public string Id { get; }
        public int Dim { get; }
        public Image Sprite { get; }
    }

    class Research
    {
        internal required string Project { get; set; }
        internal int Assigned { get; set; }
        internal int Cost { get; set; }
        internal int Spent { get; set; }

        internal void SetToExcellent()
        {
            this.Spent = this.Cost;
        }

        override
        public string ToString() 
        {
            return Project + "  ---  [ " + Spent + " ] of [ " + Cost + " ]";
        }
    }

    struct WSlot 
    {
        internal required string ID { get; set; }
        internal int ammo {  get; set; }
        internal string name { get; set; }  
    }

    class CraftData
    {
        internal required string ID { get; set; }
        internal required string Name { get; set; }
        internal required string Sprite { get; set; }
        internal int Fuel { get; set; }
        internal int Damage { get; set; }
        internal int Shield { get; set; }
        internal required List<WSlot> wSlots { get; set; }
        internal required Craft craftSpecs { get; set; }

        override
        public string ToString()
        {
            return Name + "  ---  [ " + ID + " ]";
        }
    }
    class ProductionItem 
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Spent { get; set; }

        public int Cost { get; set; }

        public int Assigned { get; set; }

        public int Amount { get; set; }

        override
        public string ToString()
        {
            return Name + "  ---  [ " + ID + " ][" + Spent + "][" + Cost*Amount + "]";
        }
    }
}
