using System.Collections.Generic;

public static class PlaneReferences
{
    public static Dictionary<string, string> Dictionary = new Dictionary<string, string>
    {
        {"Code_35", "DrakkarBase"},
        {"Code_36", "HKBase"},
        {"Code_55", "PredatorBase"},
        {"Code_56", "KrakenBase"},
        {"Code_57", "SabreBase"},
        {"Code_58", "HydraBase"},
        {"Code_59", "BrigBase"},
        {"Code_60", "VenturaBase"},
        {"Code_61", "MiningShipBase"},
        {"Code_155", "MercBase"},
        {"Code_156", "NightmareBase"},
        {"Code_157", "ZeppelinBase"},
        {"Code_158", "ChariotBase"},
        {"Code_159", "HawkeyeBase"},
        {"Code_160", "DropshipBase"},
        {"Code_161", "BlazerBase"},
        {"Code_162", "ConquerorBase"},
        {"Code_163", "BraveWhalerBase"},
        {"Code_164", "BaracudaBase"},
        {"Code_165", "GunfighterBase"},
        {"Code_177", "JetbikeBase"},
        {"Code_178", "MBTBase"},
        {"Code_179", "MWingBase"},
        {"Code_180", "AirbusBase"},
        {"Code_181", "AircarBase"},
        {"Code_182", "AirspeederBase"},
        {"Code_183", "XTritonBase"},
        {"Code_184", "SakurabusBase"},
        {"Code_185", "MI2400Base"},
        {"Code_186", "VSnakeBase"},
        {"Code_187", "AirvanBase"},
        {"Code_199", "ScarabBase"},
        {"Code_200", "ExpeditionBase"},
        {"Code_201", "JellyfishBase"},
        {"Code_202", "ShadowbatBase"},
        {"Code_203", "DevastatorBase"},
        {"Code_204", "FatsubBase"},
        {"Code_205", "GoldhawkBase"},
        {"Code_206", "WhiteWormBase"},
        {"Code_207", "PiranhaBase"},
        {"Code_208", "CapsuleBase"},
        {"Code_209", "GDXCarBase"},
        {"Code_233", "RailPodBase"},
        {"Code_234", "VenturaBase1"},
        {"Code_235", "VenturaBase2"},
        {"Code_236", "VenturaBase3"},
        {"Code_238", "ConvoyBase"},
        {"Code_239", "BikesBase"},
        {"Code_240", "TrucksBase"},
        {"Code_241", "CadillacBase"},
        {"Code_242", "LittleBirdBase"},
        {"Code_243", "FaustBase"},
        {"Code_255", "DeloreanBase"},
        {"Code_256", "SBusBase"},
        {"Code_257", "Mi8Base"},
        {"Code_258", "BuckarooBase"},
        {"Code_259", "BomberBase"},
        {"Code_260", "SpitfireBase"},
        {"Code_261", "HyperjetBase"},
        {"Code_262", "HarvesterBase"},
        {"Code_263", "HighstarBase"},
        {"Code_264", "Mi24ABase"},
        {"Code_265", "Thunderbird_Base"},
        {"Code_277", "QTaxiBase"},
        {"Code_278", "AirtruckBase"},
        {"Code_280", "HummerBase"},
        {"Code_281", "HueyBase"},
        {"Code_282", "TigerBase"},
    };

    public static string GetSpriteByKey(string key)
    {
        if (Dictionary.ContainsKey(key))
        {
            return Dictionary[key];
        }
        return null;
    }
}
