using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;

namespace FunnySnek.AntiCheat.Client
{
    //todo
    // add warning in smapi if you have wrong mods installed
    // set up wrong mods kick
    // use "Mymod.Myaddress" to make sure peopel stay up to date

    /// <summary>The mod entry point.</summary>
    internal class ModEntry : Mod
    {
        /*********
        ** Properties
        *********/
        private int PlayerCount; //stores number of players
        private int GameClockTicks; //stores in game clock change
        private int HalfSecondTicks;
        private int CheatClockTicks;
        private readonly List<string> BlockedModNames = new List<string>();
        private bool IsCheater;
        private bool IsMessageSent;
        private bool IsPreMessageDeleteMessageSent;
        private string CurrentPassword = "SCAZ"; //current code password 4 characters only
        private string IntroMessage = "ServerCode7.3 Activated";


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            TimeEvents.TimeOfDayChanged += this.OnTimeOfDayChanged; // Time of day change handler
            GameEvents.HalfSecondTick += this.OnHalfSecondTick;
            GameEvents.FourthUpdateTick += this.OnFourthUpdateTick;
            InputEvents.ButtonPressed += this.OnButtonPressed;
            SaveEvents.BeforeSave += this.Shipping_Menu; // Shipping Menu handler
            GraphicsEvents.OnPreRenderGuiEvent += this.OnPreRenderGui; // for fixing social menu


            bool consoleCommandsIsLoaded = this.Helper.ModRegistry.IsLoaded("SMAPI.ConsoleCommands");
            if (consoleCommandsIsLoaded)
            { this.BlockedModNames.Add("Console Commands"); this.IsCheater = true; }
            bool cjbCheatsMenuIsLoaded = this.Helper.ModRegistry.IsLoaded("CJBok.CheatsMenu");
            if (cjbCheatsMenuIsLoaded)
            { this.BlockedModNames.Add("CJB CHEATS"); this.IsCheater = true; }
            bool bcmpincMovementSpeedIsLoaded = this.Helper.ModRegistry.IsLoaded("bcmpinc.MovementSpeed");
            if (bcmpincMovementSpeedIsLoaded)
            { this.BlockedModNames.Add("Movement Speed"); this.IsCheater = true; }
            bool bcmpincHarvestWithScytheIsLoaded = this.Helper.ModRegistry.IsLoaded("bcmpinc.HarvestWithScythe");
            if (bcmpincHarvestWithScytheIsLoaded)
            { this.BlockedModNames.Add("HarvestWithScythe"); this.IsCheater = true; }
            bool dewModsStardewValleyModsPhoneVillagersIsLoaded = this.Helper.ModRegistry.IsLoaded("DewMods.StardewValleyMods.PhoneVillagers");
            if (dewModsStardewValleyModsPhoneVillagersIsLoaded)
            { this.BlockedModNames.Add("PhoneVillager"); this.IsCheater = true; }
            bool vylusPelicanPostalServiceIsLoaded = this.Helper.ModRegistry.IsLoaded("Vylus.PelicanPostalService");
            if (vylusPelicanPostalServiceIsLoaded)
            { this.BlockedModNames.Add("PelicanPostalService"); this.IsCheater = true; }
            bool crazywigtoolpowerselectIsLoaded = this.Helper.ModRegistry.IsLoaded("crazywig.toolpowerselect");
            if (crazywigtoolpowerselectIsLoaded)
            { this.BlockedModNames.Add("ToolPowerSelect"); this.IsCheater = true; }
            bool defenTheNationTimeMultiplierIsLoaded = this.Helper.ModRegistry.IsLoaded("DefenTheNation.TimeMultiplier");
            if (defenTheNationTimeMultiplierIsLoaded)
            { this.BlockedModNames.Add("TimeMultiplier"); this.IsCheater = true; }
            bool scythHarvestIsLoaded = this.Helper.ModRegistry.IsLoaded("965169fd-e1ed-47d0-9f12-b104535fb4bc");
            if (scythHarvestIsLoaded)
            { this.BlockedModNames.Add("ScythHarvest"); this.IsCheater = true; }
            bool omegasisAutoSpeedIsLoaded = this.Helper.ModRegistry.IsLoaded("Omegasis.AutoSpeed");
            if (omegasisAutoSpeedIsLoaded)
            { this.BlockedModNames.Add("AutoSpeed"); this.IsCheater = true; }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool cantorsdustTimeSpeedIsLoaded = this.Helper.ModRegistry.IsLoaded("cantorsdust.TimeSpeed");
            if (cantorsdustTimeSpeedIsLoaded)
            { this.BlockedModNames.Add("TimeSpeed"); this.IsCheater = true; }
            bool cJBokItemSpawnerIsLoaded = this.Helper.ModRegistry.IsLoaded("CJBok.ItemSpawner");
            if (cJBokItemSpawnerIsLoaded)
            { this.BlockedModNames.Add("CJB ItemSpawner"); this.IsCheater = true; }
            bool pathoschildChestsAnywhereIsLoaded = this.Helper.ModRegistry.IsLoaded("Pathoschild.ChestsAnywhere");
            if (pathoschildChestsAnywhereIsLoaded)
            { this.BlockedModNames.Add("ChestsAnywhere"); this.IsCheater = true; }
            bool dewModsStardewValleyModsSkipFishingMinigameIsLoaded = this.Helper.ModRegistry.IsLoaded("DewMods.StardewValleyMods.SkipFishingMinigame");
            if (dewModsStardewValleyModsSkipFishingMinigameIsLoaded)
            { this.BlockedModNames.Add("SkipFishingMinigame"); this.IsCheater = true; }
            bool shalankwaWarpToFriendsIsLoaded = this.Helper.ModRegistry.IsLoaded("Shalankwa.WarpToFriends");
            if (shalankwaWarpToFriendsIsLoaded)
            { this.BlockedModNames.Add("WarpToFriends"); this.IsCheater = true; }
            bool punyoPassableObjectsIsLoaded = this.Helper.ModRegistry.IsLoaded("punyo.PassableObjects");
            if (punyoPassableObjectsIsLoaded)
            { this.BlockedModNames.Add("PassableObjects"); this.IsCheater = true; }
            bool defenTheNationBackpackResizerdIsLoaded = this.Helper.ModRegistry.IsLoaded("DefenTheNation.BackpackResizer");
            if (defenTheNationBackpackResizerdIsLoaded)
            { this.BlockedModNames.Add("BackpackResizer"); this.IsCheater = true; }
            bool whiteMindAutoFishIsLoaded = this.Helper.ModRegistry.IsLoaded("WhiteMind.AF");
            if (whiteMindAutoFishIsLoaded)
            { this.BlockedModNames.Add("AutoFish"); this.IsCheater = true; }
            bool mucchanPrairieKingMadeEasyIsLoaded = this.Helper.ModRegistry.IsLoaded("Mucchan.PrairieKingMadeEasy");
            if (mucchanPrairieKingMadeEasyIsLoaded)
            { this.BlockedModNames.Add("PrairieKingMadeEasy"); this.IsCheater = true; }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool cantorsdustAllProfessionsIsLoaded = this.Helper.ModRegistry.IsLoaded("cantorsdust.AllProfessions");
            if (cantorsdustAllProfessionsIsLoaded)
            { this.BlockedModNames.Add("AllProfessions"); this.IsCheater = true; }
            bool moreArtifactSpotsIsLoaded = this.Helper.ModRegistry.IsLoaded("451");
            if (moreArtifactSpotsIsLoaded)
            { this.BlockedModNames.Add("More Artifact Spots"); this.IsCheater = true; }
            bool allCropsAllSeasons = this.Helper.ModRegistry.IsLoaded("cantorsdust.AllCropsAllSeasons");
            if (allCropsAllSeasons)
            { this.BlockedModNames.Add("AllCropsAllSeasons"); this.IsCheater = true; }
            bool drynwynnFishingAutomatonIsLoaded = this.Helper.ModRegistry.IsLoaded("Drynwynn.FishingAutomaton");
            if (drynwynnFishingAutomatonIsLoaded)
            { this.BlockedModNames.Add("FishingAutomaton"); this.IsCheater = true; }
            bool cantorsdustRecatchLegendaryFishIsLoaded = this.Helper.ModRegistry.IsLoaded("cantorsdust.RecatchLegendaryFish");
            if (cantorsdustRecatchLegendaryFishIsLoaded)
            { this.BlockedModNames.Add("RecatchLegendaryFish"); this.IsCheater = true; }
            bool kathrynHazukaFasterRunIsLoaded = this.Helper.ModRegistry.IsLoaded("KathrynHazuka.FasterRun");
            if (kathrynHazukaFasterRunIsLoaded)
            { this.BlockedModNames.Add("FasterRun"); this.IsCheater = true; }
            bool bitwiseJonModsInstantBuildingsIsLoaded = this.Helper.ModRegistry.IsLoaded("BitwiseJonMods.InstantBuildings");
            if (bitwiseJonModsInstantBuildingsIsLoaded)
            { this.BlockedModNames.Add("InstantBuildings"); this.IsCheater = true; }
            bool jotserAutoGrabberModIsLoaded = this.Helper.ModRegistry.IsLoaded("Jotser.AutoGrabberMod");
            if (jotserAutoGrabberModIsLoaded)
            { this.BlockedModNames.Add("AutoGrabber"); this.IsCheater = true; }
            bool dcantorsdustInstantGrowTreesIsLoaded = this.Helper.ModRegistry.IsLoaded("cantorsdust.InstantGrowTrees");
            if (dcantorsdustInstantGrowTreesIsLoaded)
            { this.BlockedModNames.Add("InstantGrowTrees"); this.IsCheater = true; }
            bool jwdredPointAndPlantIsLoaded = this.Helper.ModRegistry.IsLoaded("jwdred.PointAndPlant");
            if (jwdredPointAndPlantIsLoaded)
            { this.BlockedModNames.Add("PointAndPlant"); this.IsCheater = true; }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool magicIsLoaded = this.Helper.ModRegistry.IsLoaded("spacechase0.Magic");
            if (magicIsLoaded)
            { this.BlockedModNames.Add("Magic"); this.IsCheater = true; }
            bool fastTravelIsLoaded = this.Helper.ModRegistry.IsLoaded("DeathGameDev.FastTravel");
            if (fastTravelIsLoaded)
            { this.BlockedModNames.Add("FastTravel"); this.IsCheater = true; }
            bool scytheHarvesting2IsLoaded = this.Helper.ModRegistry.IsLoaded("mmanlapat.ScytheHarvesting");
            if (scytheHarvesting2IsLoaded)
            { this.BlockedModNames.Add("ScytheHarvesting2"); this.IsCheater = true; }
            bool skullCavernElevatorIsLoaded = this.Helper.ModRegistry.IsLoaded("SkullCavernElevator");
            if (skullCavernElevatorIsLoaded)
            { this.BlockedModNames.Add("SkullCavernElevator"); this.IsCheater = true; }
            bool horseWhistleIsLoaded = this.Helper.ModRegistry.IsLoaded("icepuente.HorseWhistle");
            if (horseWhistleIsLoaded)
            { this.BlockedModNames.Add("HorseWhistle"); this.IsCheater = true; }
            bool fasterHorseIsLoaded = this.Helper.ModRegistry.IsLoaded("kibbe.faster_horse");
            if (fasterHorseIsLoaded)
            { this.BlockedModNames.Add("FasterHorse"); this.IsCheater = true; }
            bool cropTransplantIsLoaded = this.Helper.ModRegistry.IsLoaded("DIGUS.CropTransplantMod");
            if (cropTransplantIsLoaded)
            { this.BlockedModNames.Add("CropTransplant"); this.IsCheater = true; }
            bool automateIsLoaded = this.Helper.ModRegistry.IsLoaded("Pathoschild.Automate");
            if (automateIsLoaded)
            { this.BlockedModNames.Add("Automate"); this.IsCheater = true; }
            bool kisekaeIsLoaded = this.Helper.ModRegistry.IsLoaded("Kabigon.kisekae");
            if (kisekaeIsLoaded)
            { this.BlockedModNames.Add("kisekae"); this.IsCheater = true; }
            bool bjsTimeSkipperIsLoaded = this.Helper.ModRegistry.IsLoaded("BunnyJumps.BJSTimeSkipper");
            if (bjsTimeSkipperIsLoaded)
            { this.BlockedModNames.Add("BJSTimeSkipper"); this.IsCheater = true; }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool timeFreezeIsLoaded = this.Helper.ModRegistry.IsLoaded("Omegasis.TimeFreeze");
            if (timeFreezeIsLoaded)
            { this.BlockedModNames.Add("TimeFreeze"); this.IsCheater = true; }
            bool ultimateGoldIsLoaded = this.Helper.ModRegistry.IsLoaded("Shadowfoxss.UltimateGoldStardew");
            if (ultimateGoldIsLoaded)
            { this.BlockedModNames.Add("UltimateGold"); this.IsCheater = true; }
            bool moreMapWarpsIsLoaded = this.Helper.ModRegistry.IsLoaded("rc.maps");
            if (moreMapWarpsIsLoaded)
            { this.BlockedModNames.Add("MmoreMapWarps"); this.IsCheater = true; }
            bool debugIsLoaded = this.Helper.ModRegistry.IsLoaded("Pathoschild.DebugMode");
            if (debugIsLoaded)
            { this.BlockedModNames.Add("DebugMode"); this.IsCheater = true; }
            bool idleTimerIsLoaded = this.Helper.ModRegistry.IsLoaded("LordAndreios.IdleTimer");
            if (idleTimerIsLoaded)
            { this.BlockedModNames.Add("IdleTimer"); this.IsCheater = true; }
            bool extremeFishingOverhaulIsLoaded = this.Helper.ModRegistry.IsLoaded("DevinLematty.ExtremeFishingOverhaul");
            if (extremeFishingOverhaulIsLoaded)
            { this.BlockedModNames.Add("ExtremeFishingOverhaul"); this.IsCheater = true; }
            bool tehsFishingOverhaulIsLoaded = this.Helper.ModRegistry.IsLoaded("TehPers.FishingOverhaul");
            if (tehsFishingOverhaulIsLoaded)
            { this.BlockedModNames.Add("TehsFishingOverhaul"); this.IsCheater = true; }
            bool selfServiceShopsIsLoaded = this.Helper.ModRegistry.IsLoaded("GuiNoya.SelfServiceShop");
            if (selfServiceShopsIsLoaded)
            { this.BlockedModNames.Add("SelfServiceShops"); this.IsCheater = true; }
            bool dailyQuestAnywhereIsLoaded = this.Helper.ModRegistry.IsLoaded("Omegasis.DailyQuestAnywhere");
            if (dailyQuestAnywhereIsLoaded)
            { this.BlockedModNames.Add("DailyQuestAnywhere"); this.IsCheater = true; }
            bool bjsNoClipIsLoaded = this.Helper.ModRegistry.IsLoaded("BunnyJumps.BJSNoClip");
            if (bjsNoClipIsLoaded)
            { this.BlockedModNames.Add("BJSNoClip"); this.IsCheater = true; }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool longevityIsLoaded = this.Helper.ModRegistry.IsLoaded("UnlockedRecipes.pseudohub.de");
            if (longevityIsLoaded)
            { this.BlockedModNames.Add("Longevity"); this.IsCheater = true; }
            bool unlockedCookingRecipesIsLoaded = this.Helper.ModRegistry.IsLoaded("RTGOAT.Longevity");
            if (unlockedCookingRecipesIsLoaded)
            { this.BlockedModNames.Add("UnlockedCookingRecipes"); this.IsCheater = true; }
            bool questDelayIsLoaded = this.Helper.ModRegistry.IsLoaded("BadNetCode.QuestDelay");
            if (questDelayIsLoaded)
            { this.BlockedModNames.Add("QuestDelay"); this.IsCheater = true; }
            bool moreRainIsLoaded = this.Helper.ModRegistry.IsLoaded("Omegasis.MoreRain");
            if (moreRainIsLoaded)
            { this.BlockedModNames.Add("MoreRain"); this.IsCheater = true; }
            bool fishingAdjustIsLoaded = this.Helper.ModRegistry.IsLoaded("shuaiz.FishingAdjustMod");
            if (fishingAdjustIsLoaded)
            { this.BlockedModNames.Add("FishingAdjust"); this.IsCheater = true; }
            bool oneClickShedReloaderIsLoaded = this.Helper.ModRegistry.IsLoaded("BitwiseJonMods.OneClickShedReloader");
            if (fishingAdjustIsLoaded)
            { this.BlockedModNames.Add("FishingAdjust"); this.IsCheater = true; }
            bool parsnipsIsLoaded = this.Helper.ModRegistry.IsLoaded("SolomonsWorkshop.ParsnipsAbsolutelyEverywhere");
            if (parsnipsIsLoaded)
            { this.BlockedModNames.Add("ParsnipsAbsolutelyEverywhere"); this.IsCheater = true; }
            bool godModeIsLoaded = this.Helper.ModRegistry.IsLoaded("treyh0.GodMode");
            if (godModeIsLoaded)
            { this.BlockedModNames.Add("GodMode"); this.IsCheater = true; }
            bool moveThroughObjectIsLoaded = this.Helper.ModRegistry.IsLoaded("ylsama.MoveThoughObject");
            if (moveThroughObjectIsLoaded)
            { this.BlockedModNames.Add("MoveThoughObject"); this.IsCheater = true; }
            bool easyLegendaryFishIsLoaded = this.Helper.ModRegistry.IsLoaded("misscoriel.EZLegendaryFish");
            if (easyLegendaryFishIsLoaded)
            { this.BlockedModNames.Add("EZLegendaryFish"); this.IsCheater = true; }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool infinitejunimocartlivesIsLoaded = this.Helper.ModRegistry.IsLoaded("renny.infinitejunimocartlives");
            if (infinitejunimocartlivesIsLoaded)
            { this.BlockedModNames.Add("infinitejunimocartlives"); this.IsCheater = true; }
            bool quickStartIsLoaded = this.Helper.ModRegistry.IsLoaded("WuestMan.QuickStart");
            if (quickStartIsLoaded)
            { this.BlockedModNames.Add("QuickStart"); this.IsCheater = true; }
            bool renameIsLoaded = this.Helper.ModRegistry.IsLoaded("Remmie.Rename");
            if (renameIsLoaded)
            { this.BlockedModNames.Add("Rename"); this.IsCheater = true; }
            bool everlastingBaitsIsLoaded = this.Helper.ModRegistry.IsLoaded("DIGUS.EverlastingBaitsAndUnbreakableTacklesMod");
            if (everlastingBaitsIsLoaded)
            { this.BlockedModNames.Add("EverlastingBaitsAndUnbreakableTackles"); this.IsCheater = true; }
            bool realisticFishingIsLoaded = this.Helper.ModRegistry.IsLoaded("KevinConnors.RealisticFishing");
            if (realisticFishingIsLoaded)
            { this.BlockedModNames.Add("RealisticFishing"); this.IsCheater = true; }
            bool adjustablePriceHikesIsLoaded = this.Helper.ModRegistry.IsLoaded("Pokeytax.AdjustablePriceHikes");
            if (adjustablePriceHikesIsLoaded)
            { this.BlockedModNames.Add("AdjustablePriceHikes"); this.IsCheater = true; }
            bool moreSiloStorageIsLoaded = this.Helper.ModRegistry.IsLoaded("OrneryWalrus.MoreSiloStorage");
            if (moreSiloStorageIsLoaded)
            { this.BlockedModNames.Add("MoreSiloStorage"); this.IsCheater = true; }
            bool multiToolModIsLoaded = this.Helper.ModRegistry.IsLoaded("miome.MultiToolMod");
            if (multiToolModIsLoaded)
            { this.BlockedModNames.Add("MultiTool"); this.IsCheater = true; }
            bool foxyfficiencyIsLoaded = this.Helper.ModRegistry.IsLoaded("Fokson.Foxyfficiency");
            if (foxyfficiencyIsLoaded)
            { this.BlockedModNames.Add("Foxyfficiency"); this.IsCheater = true; }
            bool noAddedFlyingMineMonstersIsLoaded = this.Helper.ModRegistry.IsLoaded("Drynwynn.NoAddedFlyingMineMonsters");
            if (noAddedFlyingMineMonstersIsLoaded)
            { this.BlockedModNames.Add("No More Random Mine Flyers"); this.IsCheater = true; }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool longerLastingLuresIsLoaded = this.Helper.ModRegistry.IsLoaded("caraxian.LongerLastingLures");
            if (longerLastingLuresIsLoaded)
            { this.BlockedModNames.Add("LongerLastingLures"); this.IsCheater = true; }
            bool bjsStopGrassIsLoaded = this.Helper.ModRegistry.IsLoaded("BunnyJumps.BJSStopGrass");
            if (bjsStopGrassIsLoaded)
            { this.BlockedModNames.Add("BJSStopGrass"); this.IsCheater = true; }
            bool bigSiloIsLoaded = this.Helper.ModRegistry.IsLoaded("lperkins2.BigSilo");
            if (bigSiloIsLoaded)
            { this.BlockedModNames.Add("BigSilos"); this.IsCheater = true; }
            bool plantableMushroomTreesIsLoaded = this.Helper.ModRegistry.IsLoaded("f4iTh.PMT");
            if (plantableMushroomTreesIsLoaded)
            { this.BlockedModNames.Add("PlantableMushroomTrees"); this.IsCheater = true; }
            bool infiniteInventoryIsLoaded = this.Helper.ModRegistry.IsLoaded("DevinLematty.InfiniteInventory");
            if (infiniteInventoryIsLoaded)
            { this.BlockedModNames.Add("InfiniteInventory"); this.IsCheater = true; }
            bool betterJunimosIsLoaded = this.Helper.ModRegistry.IsLoaded("hawkfalcon.BetterJunimos");
            if (betterJunimosIsLoaded)
            { this.BlockedModNames.Add("BetterJunimos"); this.IsCheater = true; }
            bool tillableGroundIsLoaded = this.Helper.ModRegistry.IsLoaded("hawkfalcon.TillableGround");
            if (tillableGroundIsLoaded)
            { this.BlockedModNames.Add("TillableGround"); this.IsCheater = true; }
            bool customQuestExpirationIsLoaded = this.Helper.ModRegistry.IsLoaded("hawkfalcon.CustomQuestExpiration");
            if (tillableGroundIsLoaded)
            { this.BlockedModNames.Add("TillableGround"); this.IsCheater = true; }
            bool betterTransmutationIsLoaded = this.Helper.ModRegistry.IsLoaded("f4iTh.BetterTransmutation");
            if (betterTransmutationIsLoaded)
            { this.BlockedModNames.Add("BetterTransmutation"); this.IsCheater = true; }
            bool moreMineLaddersIsLoaded = this.Helper.ModRegistry.IsLoaded("JadeTheavas.MoreMineLadders");
            if (moreMineLaddersIsLoaded)
            { this.BlockedModNames.Add("MoreMineLadders"); this.IsCheater = true; }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool priceDropsIsLoaded = this.Helper.ModRegistry.IsLoaded("skuldomg.priceDrops");
            if (priceDropsIsLoaded)
            { this.BlockedModNames.Add("PriceDrops"); this.IsCheater = true; }
            bool safelightningIsLoaded = this.Helper.ModRegistry.IsLoaded("cat.safelightning");
            if (safelightningIsLoaded)
            { this.BlockedModNames.Add("Safelightnings"); this.IsCheater = true; }
            bool customizabledeathpenaltyIsLoaded = this.Helper.ModRegistry.IsLoaded("cat.customizabledeathpenalty");
            if (customizabledeathpenaltyIsLoaded)
            { this.BlockedModNames.Add("CustomizableDeathPenalty"); this.IsCheater = true; }
            bool customwarplocationsIsLoaded = this.Helper.ModRegistry.IsLoaded("cat.customwarplocations");
            if (customwarplocationsIsLoaded)
            { this.BlockedModNames.Add("CustomWarpLocations"); this.IsCheater = true; }
            bool nocrowsIsLoaded = this.Helper.ModRegistry.IsLoaded("cat.nocrows");
            if (nocrowsIsLoaded)
            { this.BlockedModNames.Add("No Crows"); this.IsCheater = true; }
            bool autoWaterIsLoaded = this.Helper.ModRegistry.IsLoaded("Whisk.AutoWater");
            if (autoWaterIsLoaded)
            { this.BlockedModNames.Add("AutoWater"); this.IsCheater = true; }
            bool equivalentExchangeIsLoaded = this.Helper.ModRegistry.IsLoaded("MercuriusXeno.EquivalentExchange");
            if (equivalentExchangeIsLoaded)
            { this.BlockedModNames.Add("EquivalentExchange"); this.IsCheater = true; }
            bool seedCatalogueIsLoaded = this.Helper.ModRegistry.IsLoaded("spacechase0.SeedCatalogue");
            if (seedCatalogueIsLoaded)
            { this.BlockedModNames.Add("SeedCatalogue"); this.IsCheater = true; }
            bool wintergrassIsLoaded = this.Helper.ModRegistry.IsLoaded("cat.wintergrass");
            if (wintergrassIsLoaded)
            { this.BlockedModNames.Add("WinterGrass"); this.IsCheater = true; }
            bool moodGuardIsLoaded = this.Helper.ModRegistry.IsLoaded("YonKuma.MoodGuard");
            if (moodGuardIsLoaded)
            { this.BlockedModNames.Add("MoodGuard"); this.IsCheater = true; }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool extendedReachIsLoaded = this.Helper.ModRegistry.IsLoaded("spacechase0.ExtendedReach");
            if (extendedReachIsLoaded)
            { this.BlockedModNames.Add("ExtendedReach"); this.IsCheater = true; }
            bool seasonalitemsIsLoaded = this.Helper.ModRegistry.IsLoaded("midoriarmstrong.seasonalitems");
            if (seasonalitemsIsLoaded)
            { this.BlockedModNames.Add("SeasonalItems"); this.IsCheater = true; }
            bool betterhayIsLoaded = this.Helper.ModRegistry.IsLoaded("cat.betterhay");
            if (betterhayIsLoaded)
            { this.BlockedModNames.Add("BetterHay"); this.IsCheater = true; }
            bool customizableCartReduxIsLoaded = this.Helper.ModRegistry.IsLoaded("KoihimeNakamura.CCR");
            if (customizableCartReduxIsLoaded)
            { this.BlockedModNames.Add("CustomizableCartRedux"); this.IsCheater = true; }
            bool moveFasterIsLoaded = this.Helper.ModRegistry.IsLoaded("shuaiz.MoveFasterMod");
            if (moveFasterIsLoaded)
            { this.BlockedModNames.Add("MoveFaster"); this.IsCheater = true; }
            bool treeTransplantIsLoaded = this.Helper.ModRegistry.IsLoaded("TreeTransplant");
            if (treeTransplantIsLoaded)
            { this.BlockedModNames.Add("TreeTransplant"); this.IsCheater = true; }
            bool rentedToolsIsLoaded = this.Helper.ModRegistry.IsLoaded("JarvieK.RentedTools");
            if (rentedToolsIsLoaded)
            { this.BlockedModNames.Add("RentedTools"); this.IsCheater = true; }
            bool selfServiceIsLoaded = this.Helper.ModRegistry.IsLoaded("JarvieK.SelfService");
            if (selfServiceIsLoaded)
            { this.BlockedModNames.Add("SelfService"); this.IsCheater = true; }
            bool bregsFishIsLoaded = this.Helper.ModRegistry.IsLoaded("Bregodon.BregsFish");
            if (bregsFishIsLoaded)
            { this.BlockedModNames.Add("BregsFish"); this.IsCheater = true; }
            bool expandedFridgeIsLoaded = this.Helper.ModRegistry.IsLoaded("Uwazouri.ExpandedFridge");
            if (expandedFridgeIsLoaded)
            { this.BlockedModNames.Add("ExpandedFridge"); this.IsCheater = true; }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool startingMoneyIsLoaded = this.Helper.ModRegistry.IsLoaded("mmanlapat.StartingMoney");
            if (startingMoneyIsLoaded)
            { this.BlockedModNames.Add("StartingMoney"); this.IsCheater = true; }
            bool wHatsUpIsLoaded = this.Helper.ModRegistry.IsLoaded("wHatsUp");
            if (wHatsUpIsLoaded)
            { this.BlockedModNames.Add("wHatsUp"); this.IsCheater = true; }
            bool chefsClosetIsLoaded = this.Helper.ModRegistry.IsLoaded("Duder.ChefsCloset");
            if (chefsClosetIsLoaded)
            { this.BlockedModNames.Add("ChefsCloset"); this.IsCheater = true; }
            bool improvedQualityOfLifeIsLoaded = this.Helper.ModRegistry.IsLoaded("Demiacle.ImprovedQualityOfLife");
            if (improvedQualityOfLifeIsLoaded)
            { this.BlockedModNames.Add("ImprovedQualityOfLife"); this.IsCheater = true; }
            bool autoAnimalDoorsIsLoaded = this.Helper.ModRegistry.IsLoaded("AaronTaggart.AutoAnimalDoors");
            if (autoAnimalDoorsIsLoaded)
            { this.BlockedModNames.Add("AutoAnimalDoors"); this.IsCheater = true; }
            bool partOftheCommunityIsLoaded = this.Helper.ModRegistry.IsLoaded("SB_PotC");
            if (partOftheCommunityIsLoaded)
            { this.BlockedModNames.Add("PartoftheCommunity"); this.IsCheater = true; }
            bool casksAnywhereIsLoaded = this.Helper.ModRegistry.IsLoaded("CasksAnywhere");
            if (casksAnywhereIsLoaded)
            { this.BlockedModNames.Add("CasksAnywhere"); this.IsCheater = true; }
            bool rocsReseedIsLoaded = this.Helper.ModRegistry.IsLoaded("Roc.Reseed");
            if (rocsReseedIsLoaded)
            { this.BlockedModNames.Add("RocsReseed"); this.IsCheater = true; }
            bool basicSprinklerImprovedIsLoaded = this.Helper.ModRegistry.IsLoaded("lrsk_sdvm_bsi.0117171308");
            if (basicSprinklerImprovedIsLoaded)
            { this.BlockedModNames.Add("BasicSprinklerImproved"); this.IsCheater = true; }
            bool miningWithExplosivesIsLoaded = this.Helper.ModRegistry.IsLoaded("MiningWithExplosives");
            if (miningWithExplosivesIsLoaded)
            { this.BlockedModNames.Add("MiningWithExplosives"); this.IsCheater = true; }
            bool autoEatIsLoaded = this.Helper.ModRegistry.IsLoaded("Permamiss.AutoEat");
            if (autoEatIsLoaded)
            { this.BlockedModNames.Add("AutoEat"); this.IsCheater = true; }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool replanterIsLoaded = this.Helper.ModRegistry.IsLoaded("jwdred.Replanter");
            if (replanterIsLoaded)
            { this.BlockedModNames.Add("Replanter"); this.IsCheater = true; }
            bool sprintAndDashReduxIsLoaded = this.Helper.ModRegistry.IsLoaded("littleraskol.SprintAndDashRedux");
            if (sprintAndDashReduxIsLoaded)
            { this.BlockedModNames.Add("SprintAndDashRedux"); this.IsCheater = true; }
            bool luckSkillIsLoaded = this.Helper.ModRegistry.IsLoaded("spacechase0.LuckSkill");
            if (luckSkillIsLoaded)
            { this.BlockedModNames.Add("LuckSkill"); this.IsCheater = true; }
            bool buildHealthIsLoaded = this.Helper.ModRegistry.IsLoaded("Omegasis.BuildHealth");
            if (buildHealthIsLoaded)
            { this.BlockedModNames.Add("BuildHealth"); this.IsCheater = true; }
            bool buildEnduranceIsLoaded = this.Helper.ModRegistry.IsLoaded("Omegasis.BuildEndurance");
            if (buildEnduranceIsLoaded)
            { this.BlockedModNames.Add("BuildEndurance"); this.IsCheater = true; }
            bool almightyFarmingToolIsLoaded = this.Helper.ModRegistry.IsLoaded("439");
            if (almightyFarmingToolIsLoaded)
            { this.BlockedModNames.Add("Almighty Farming Tool"); this.IsCheater = true; }
            bool dynamicMachinesIsLoaded = this.Helper.ModRegistry.IsLoaded("DynamicMachines");
            if (dynamicMachinesIsLoaded)
            { this.BlockedModNames.Add("DynamicMachines"); this.IsCheater = true; }
            bool configurableMachinesIsLoaded = this.Helper.ModRegistry.IsLoaded("21da6619-dc03-4660-9794-8e5b498f5b97");
            if (configurableMachinesIsLoaded)
            { this.BlockedModNames.Add("ConfigurableMachines"); this.IsCheater = true; }
            bool tappersDreamIsLoaded = this.Helper.ModRegistry.IsLoaded("ddde5195-8f85-4061-90cc-0d4fd5459358");
            if (tappersDreamIsLoaded)
            { this.BlockedModNames.Add("ATappersDream"); this.IsCheater = true; }
            bool noSoilDecayReduxIsLoaded = this.Helper.ModRegistry.IsLoaded("Platonymous.NoSoilDecayRedux");
            if (noSoilDecayReduxIsLoaded)
            { this.BlockedModNames.Add("NoSoilDecayRedux"); this.IsCheater = true; }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            bool sprintAndDashIsLoaded = this.Helper.ModRegistry.IsLoaded("SPDSprintAndDash");
            if (sprintAndDashIsLoaded)
            { this.BlockedModNames.Add("SprintAndDash"); this.IsCheater = true; }
            bool betterSprinklersIsLoaded = this.Helper.ModRegistry.IsLoaded("Speeder.BetterSprinklers");
            if (betterSprinklersIsLoaded)
            { this.BlockedModNames.Add("BetterSprinklers"); this.IsCheater = true; }
            bool lineSprinklersIsLoaded = this.Helper.ModRegistry.IsLoaded("hootless.LineSprinklers");
            if (lineSprinklersIsLoaded)
            { this.BlockedModNames.Add("LineSprinklers"); this.IsCheater = true; }
            bool jaSprinklersIsLoaded = this.Helper.ModRegistry.IsLoaded("hootless.JASprinklers");
            if (jaSprinklersIsLoaded)
            { this.BlockedModNames.Add("LineSprinklers"); this.IsCheater = true; }
        }


        /*********
        ** Private methods
        *********/
        private void OnButtonPressed(object sender, EventArgs e)
        {
            if (this.IsCheater)
            {
                Game1.activeClickableMenu.exitThisMenu();
                this.Monitor.Log("Blocked Mods Detected:", LogLevel.Warn);
                foreach (string o in this.BlockedModNames)
                    this.Monitor.Log(o, LogLevel.Warn);
            }
            else
                InputEvents.ButtonPressed -= this.OnButtonPressed;
        }

        private void OnHalfSecondTick(object sender, EventArgs e)
        {
            if (Game1.activeClickableMenu is TitleMenu)
                this.IsMessageSent = false;

            if (!Context.IsWorldReady)
                return;

            this.HalfSecondTicks += 1;
            if (this.HalfSecondTicks >= 30 && this.IsMessageSent == false && this.IsCheater == false)
            {
                var playerID = Game1.player.UniqueMultiplayerID;

                Game1.chatBox.addInfoMessage($"{this.IntroMessage}");
                Game1.displayHUD = true;
                Game1.addHUDMessage(new HUDMessage($"{this.IntroMessage}", ""));

                string myStringMessage = $"{this.CurrentPassword}{playerID}";
                Game1.client.sendMessage(18, myStringMessage);

                this.IsMessageSent = true;
            }
        }

        private void OnFourthUpdateTick(object sender, EventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (this.IsPreMessageDeleteMessageSent == false)
            {
                var playerID = Game1.player.UniqueMultiplayerID;
                Game1.chatBox.activate();
                Game1.chatBox.setText("/color");
                Game1.chatBox.chatBox.RecieveCommandInput('\r');
                Game1.chatBox.activate();
                Game1.chatBox.setText("ServerCode Sent");
                Game1.chatBox.chatBox.RecieveCommandInput('\r');
                string myStringMessage = $"{this.CurrentPassword}{playerID}";
                Game1.client.sendMessage(18, myStringMessage);
                this.IsPreMessageDeleteMessageSent = true;
            }
        }

        //fix social tab
        private void OnPreRenderGui(object sender, EventArgs e)
        {
            if (Game1.activeClickableMenu is GameMenu gameMenu && gameMenu.currentTab == GameMenu.socialTab)
            {
                List<IClickableMenu> tabs = this.Helper.Reflection.GetField<List<IClickableMenu>>(gameMenu, "pages").GetValue();
                SocialPage socialPage = (SocialPage)tabs[gameMenu.currentTab];
                IReflectedField<int> numFarmers = this.Helper.Reflection.GetField<int>(socialPage, "numFarmers");
                numFarmers.SetValue(1);

            }
        }
        
        // shipping menu"OK" click through code
        private void Shipping_Menu(object sender, EventArgs e)
        {

            if (Game1.activeClickableMenu is ShippingMenu)
            {
                this.Helper.Reflection.GetMethod(Game1.activeClickableMenu, "okClicked").Invoke();
            }

        }

        // Holiday code
        private void OnTimeOfDayChanged(object sender, EventArgs e)
        {
            this.GameClockTicks += 1;


            if (this.IsCheater)
            {
                this.CheatClockTicks += 1;
                Game1.chatBox.addInfoMessage("Blocked Mods Detected!");
                Game1.displayHUD = true;
                Game1.addHUDMessage(new HUDMessage("Blocked Mods Detected!", ""));
                if (this.CheatClockTicks == 6)
                {
                    Game1.exitToTitle = true;
                    this.CheatClockTicks = 0;
                }
            }

            this.PlayerCount = Game1.otherFarmers.Count;
            
            if (this.GameClockTicks >= 1)
            {
                var currentTime = Game1.timeOfDay;
                var currentDate = SDate.Now();
                var eggFestival = new SDate(13, "spring");
                var dayAfterEggFestival = new SDate(14, "spring");
                var flowerDance = new SDate(24, "spring");
                var luau = new SDate(11, "summer");
                var danceOfJellies = new SDate(28, "summer");
                var stardewValleyFair = new SDate(16, "fall");
                var spiritsEve = new SDate(27, "fall");
                var festivalOfIce = new SDate(8, "winter");
                var feastOfWinterStar = new SDate(25, "winter");

                
                if (currentDate == eggFestival && this.PlayerCount >= 1)   //set back to 1 after testing~!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    EggFestival();


                //flower dance
                else if (currentDate == flowerDance && this.PlayerCount >= 1)
                    FlowerDance();

                else if (currentDate == luau && this.PlayerCount >= 1)
                    Luau();

                else if (currentDate == danceOfJellies && this.PlayerCount >= 1)
                    DanceOfTheMoonlightJellies();

                else if (currentDate == stardewValleyFair && this.PlayerCount >= 1)
                    StardewValleyFair();

                else if (currentDate == spiritsEve && this.PlayerCount >= 1)
                    SpiritsEve();

                else if (currentDate == festivalOfIce && this.PlayerCount >= 1)
                    FestivalOfIce();

                else if (currentDate == feastOfWinterStar && this.PlayerCount >= 1)
                    FeastOfWinterStar();



                this.GameClockTicks = 0;   // never reaches rest of code bc gameClockTicks is reset to 0, these methods below are called higher up.




                void EggFestival()
                {
                    if (currentTime >= 900 && currentTime <= 1400)
                    {
                        //teleports to egg festival
                        Game1.player.team.SetLocalReady("festivalStart", true);
                        Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalStart", true, (ConfirmationDialog.behavior)(who =>
                        {
                            Game1.exitActiveMenu();
                            Game1.warpFarmer("Town", 1, 20, 1);
                        }), (ConfirmationDialog.behavior)null);
                    }
                }

                // flower dance turned off causes game crashes
                void FlowerDance()
                {
                    if (currentTime >= 900 && currentTime <= 1400)
                    {
                        Game1.player.team.SetLocalReady("festivalStart", true);
                        Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalStart", true, (ConfirmationDialog.behavior)(who =>
                        {
                            Game1.exitActiveMenu();
                            Game1.warpFarmer("Forest", 1, 20, 1);
                        }), (ConfirmationDialog.behavior)null);
                    }
                }

                void Luau()
                {
                    if (currentTime >= 900 && currentTime <= 1400)
                    {
                        Game1.player.team.SetLocalReady("festivalStart", true);
                        Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalStart", true, (ConfirmationDialog.behavior)(who =>
                        {
                            Game1.exitActiveMenu();
                            Game1.warpFarmer("Beach", 1, 20, 1);
                        }), (ConfirmationDialog.behavior)null);
                    }
                }

                void DanceOfTheMoonlightJellies()
                {
                    if (currentTime >= 2200 && currentTime <= 2400)
                    {
                        Game1.player.team.SetLocalReady("festivalStart", true);
                        Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalStart", true, (ConfirmationDialog.behavior)(who =>
                        {
                            Game1.exitActiveMenu();
                            Game1.warpFarmer("Beach", 1, 20, 1);
                        }), (ConfirmationDialog.behavior)null);
                    }
                }

                void StardewValleyFair()
                {
                    if (currentTime >= 900 && currentTime <= 1500)
                    {
                        Game1.player.team.SetLocalReady("festivalStart", true);
                        Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalStart", true, (ConfirmationDialog.behavior)(who =>
                        {
                            Game1.exitActiveMenu();
                            Game1.warpFarmer("Town", 1, 20, 1);
                        }), (ConfirmationDialog.behavior)null);
                    }
                }

                void SpiritsEve()
                {
                    if (currentTime >= 2200 && currentTime <= 2350)
                    {
                        Game1.player.team.SetLocalReady("festivalStart", true);
                        Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalStart", true, (ConfirmationDialog.behavior)(who =>
                        {
                            Game1.exitActiveMenu();
                            Game1.warpFarmer("Town", 1, 20, 1);
                        }), (ConfirmationDialog.behavior)null);
                    }
                }

                void FestivalOfIce()
                {
                    if (currentTime >= 900 && currentTime <= 1400)
                    {
                        Game1.player.team.SetLocalReady("festivalStart", true);
                        Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalStart", true, (ConfirmationDialog.behavior)(who =>
                        {
                            Game1.exitActiveMenu();
                            Game1.warpFarmer("Forest", 1, 20, 1);
                        }), (ConfirmationDialog.behavior)null);
                    }
                }

                void FeastOfWinterStar()
                {
                    if (currentTime >= 900 && currentTime <= 1400)
                    {
                        Game1.player.team.SetLocalReady("festivalStart", true);
                        Game1.activeClickableMenu = (IClickableMenu)new ReadyCheckDialog("festivalStart", true, (ConfirmationDialog.behavior)(who =>
                        {
                            Game1.exitActiveMenu();
                            Game1.warpFarmer("Town", 1, 20, 1);
                        }), (ConfirmationDialog.behavior)null);
                    }
                }
            }
        }
    }
}
