using System.Collections.Generic;
using System.Linq;
using FunnySnek.AntiCheat.Server.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Network;

namespace FunnySnek.AntiCheat.Server
{
    /// <summary>The entry class called by SMAPI.</summary>
    internal class ModEntry : Mod
    {
        /*********
        ** Properties
        *********/
        /// <summary>The number of seconds to wait until kicking a player (to make sure they receive the chat the message).</summary>
        private readonly int SecondsUntilKick = 5;

        /// <summary>The connected players.</summary>
        private readonly List<PlayerSlot> PlayersToKick = new List<PlayerSlot>();

        /// <summary>The mod names to prohibit indexed by mod ID.</summary>
        private readonly IDictionary<string, string> ProhibitedMods = new Dictionary<string, string>
        {
            ["A Tapper's Dream"] = "ddde5195-8f85-4061-90cc-0d4fd5459358",
            ["Adjustable Price Hikes"] = "Pokeytax.AdjustablePriceHikes",
            ["All Crops All Seasons"] = "cantorsdust.AllCropsAllSeasons",
            ["All Professions"] = "cantorsdust.AllProfessions",
            ["Almighty Farming Tool"] = "439",
            ["Auto Animal Doors"] = "AaronTaggart.AutoAnimalDoors",
            ["Auto Grabber"] = "Jotser.AutoGrabberMod",
            ["AutoEat"] = "Permamiss.AutoEat",
            ["AutoFish"] = "WhiteMind.AF",
            ["Automate"] = "Pathoschild.Automate",
            ["AutoSpeed"] = "Omegasis.AutoSpeed",
            ["AutoWater"] = "Whisk.AutoWater",
            ["Backpack Resizer"] = "DefenTheNation.BackpackResizer",
            ["Basic Sprinkler Improved"] = "lrsk_sdvm_bsi.0117171308",
            ["Better Hay"] = "cat.betterhay",
            ["Better Junimos"] = "hawkfalcon.BetterJunimos",
            ["Better Sprinklers"] = "Speeder.BetterSprinklers",
            ["Better Transmutation"] = "f4iTh.BetterTransmutation",
            ["Big Silos"] = "lperkins2.BigSilo",
            ["BJS No Clip"] = "BunnyJumps.BJSNoClip",
            ["BJS Stop Grass"] = "BunnyJumps.BJSStopGrass",
            ["BJS Time Skipper"] = "BunnyJumps.BJSTimeSkipper",
            ["Bregs Fish"] = "Bregodon.BregsFish",
            ["Build Endurance"] = "Omegasis.BuildEndurance",
            ["Build Health"] = "Omegasis.BuildHealth",
            ["Casks Anywhere"] = "CasksAnywhere",
            ["Chefs Closet"] = "Duder.ChefsCloset",
            ["Chests Anywhere"] = "Pathoschild.ChestsAnywhere",
            ["CJB Cheats Menu"] = "CJBok.CheatsMenu",
            ["CJB Item Spawner"] = "CJBok.ItemSpawner",
            ["Configurable Machines"] = "21da6619-dc03-4660-9794-8e5b498f5b97",
            ["Console Commands"] = "SMAPI.ConsoleCommands",
            ["Crop Transplant"] = "DIGUS.CropTransplantMod",
            ["Custom Quest Expiration"] = "hawkfalcon.CustomQuestExpiration",
            ["Custom Warp Locations"] = "cat.customwarplocations",
            ["Customizable Cart Redux"] = "KoihimeNakamura.CCR",
            ["Customizable Death Penalty"] = "cat.customizabledeathpenalty",
            ["Daily Quest Anywhere"] = "Omegasis.DailyQuestAnywhere",
            ["Debug Mode"] = "Pathoschild.DebugMode",
            ["Dynamic Machines"] = "DynamicMachines",
            ["Equivalent Exchange"] = "MercuriusXeno.EquivalentExchange",
            ["Everlasting Baits and Unbreakable Tackles"] = "DIGUS.EverlastingBaitsAndUnbreakableTacklesMod",
            ["Expanded Fridge"] = "Uwazouri.ExpandedFridge",
            ["Extended Reach"] = "spacechase0.ExtendedReach",
            ["Extreme Fishing Overhaul"] = "DevinLematty.ExtremeFishingOverhaul",
            ["EZ Legendary Fish"] = "misscoriel.EZLegendaryFish",
            ["Fast Travel"] = "DeathGameDev.FastTravel",
            ["Faster Horse"] = "kibbe.faster_horse",
            ["Faster Run"] = "KathrynHazuka.FasterRun",
            ["Fishing Adjust"] = "shuaiz.FishingAdjustMod",
            ["Fishing Automaton"] = "Drynwynn.FishingAutomaton",
            ["Foxyfficiency"] = "Fokson.Foxyfficiency",
            ["God Mode"] = "treyh0.GodMode",
            ["Harvest With Scythe by bcmpinc"] = "bcmpinc.HarvestWithScythe",
            ["Harvest With Scythe by ThatNorthernMonkey"] = "965169fd-e1ed-47d0-9f12-b104535fb4bc",
            ["Horse Whistle"] = "icepuente.HorseWhistle",
            ["Idle Timer"] = "LordAndreios.IdleTimer",
            ["Improved Quality of Life"] = "Demiacle.ImprovedQualityOfLife",
            ["Infinite Inventory"] = "DevinLematty.InfiniteInventory",
            ["Infinite Junimo Cart Lives"] = "renny.infinitejunimocartlives",
            ["Instant Buildings"] = "BitwiseJonMods.InstantBuildings",
            ["Instant Grow Trees"] = "cantorsdust.InstantGrowTrees",
            ["Kisekae"] = "Kabigon.kisekae",
            ["Line Sprinklers (Json Assets)"] = "hootless.JASprinklers",
            ["Line Sprinklers (SMAPI)"] = "hootless.LineSprinklers",
            ["Longer Lasting Lures"] = "caraxian.LongerLastingLures",
            ["Longevity"] = "UnlockedRecipes.pseudohub.de",
            ["Luck Skill"] = "spacechase0.LuckSkill",
            ["Magic"] = "spacechase0.Magic",
            ["Mining With Explosives"] = "MiningWithExplosives",
            ["Mood Guard"] = "YonKuma.MoodGuard",
            ["More Artifact Spots"] = "451",
            ["More Map Warps"] = "rc.maps",
            ["More Mine Ladders"] = "JadeTheavas.MoreMineLadders",
            ["More Rain"] = "Omegasis.MoreRain",
            ["More Silo Storage"] = "OrneryWalrus.MoreSiloStorage",
            ["Move Faster"] = "shuaiz.MoveFasterMod",
            ["Move Though Object"] = "ylsama.MoveThoughObject",
            ["Movement Speed"] = "bcmpinc.MovementSpeed",
            ["MultiTool"] = "miome.MultiToolMod",
            ["No Crows"] = "cat.nocrows",
            ["No More Random Mine Flyers"] = "Drynwynn.NoAddedFlyingMineMonsters",
            ["No Soil Decay Redux"] = "Platonymous.NoSoilDecayRedux",
            ["One Click Shed Reloader"] = "BitwiseJonMods.OneClickShedReloader",
            ["Parsnips Absolutely Everywhere"] = "SolomonsWorkshop.ParsnipsAbsolutelyEverywhere",
            ["Part of the Community"] = "SB_PotC",
            ["Passable Objects"] = "punyo.PassableObjects",
            ["Pelican Postal Service"] = "Vylus.PelicanPostalService",
            ["Phone Villagers"] = "DewMods.StardewValleyMods.PhoneVillagers",
            ["Plantable Mushroom Trees"] = "f4iTh.PMT",
            ["Point And Plant"] = "jwdred.PointAndPlant",
            ["Prairie King Made Easy"] = "Mucchan.PrairieKingMadeEasy",
            ["Price Drops"] = "skuldomg.priceDrops",
            ["Quest Delay"] = "BadNetCode.QuestDelay",
            ["Quick Start"] = "WuestMan.QuickStart",
            ["Realistic Fishing"] = "KevinConnors.RealisticFishing",
            ["Recatch Legendary Fish"] = "cantorsdust.RecatchLegendaryFish",
            ["Rename"] = "Remmie.Rename",
            ["Rented Tools"] = "JarvieK.RentedTools",
            ["Replanter"] = "jwdred.Replanter",
            ["Rocs Reseed"] = "Roc.Reseed",
            ["Safe Lightning"] = "cat.safelightning",
            ["Scythe Harvesting"] = "mmanlapat.ScytheHarvesting",
            ["Seasonal Items"] = "midoriarmstrong.seasonalitems",
            ["Seed Catalogue"] = "spacechase0.SeedCatalogue",
            ["Self Service Shops"] = "GuiNoya.SelfServiceShop",
            ["Self Service"] = "JarvieK.SelfService",
            ["Skip Fishing Minigame"] = "DewMods.StardewValleyMods.SkipFishingMinigame",
            ["Skull Cavern Elevator"] = "SkullCavernElevator",
            ["Sprint and Dash Redux"] = "littleraskol.SprintAndDashRedux",
            ["Sprint and Dash"] = "SPDSprintAndDash",
            ["Starting Money"] = "mmanlapat.StartingMoney",
            ["Tehs Fishing Overhaul"] = "TehPers.FishingOverhaul",
            ["Tillable Ground"] = "hawkfalcon.TillableGround",
            ["Time Freeze"] = "Omegasis.TimeFreeze",
            ["Time Multiplier"] = "DefenTheNation.TimeMultiplier",
            ["TimeSpeed"] = "cantorsdust.TimeSpeed",
            ["Tool Power Select"] = "crazywig.toolpowerselect",
            ["Tree Transplant"] = "TreeTransplant",
            ["Ultimate Gold"] = "Shadowfoxss.UltimateGoldStardew",
            ["Unlocked Cooking Recipes"] = "RTGOAT.Longevity",
            ["Warp To Friends"] = "Shalankwa.WarpToFriends",
            ["wHats Up"] = "wHatsUp",
            ["Winter Grass"] = "cat.wintergrass"
        };


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            Patch.PatchAll("anticheatviachat.anticheatviachat");

            helper.Events.Multiplayer.PeerContextReceived += this.OnPeerContextReceived;
            helper.Events.Multiplayer.PeerDisconnected += this.OnPeerDisconnected;
            helper.Events.GameLoop.SaveLoaded += this.SaveLoaded;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>An event handler called when a save file is loaded.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            this.PlayersToKick.Clear();
            if (Context.IsMainPlayer)
            {
                this.SendPublicChat("Anti-Cheat activated");
                this.Monitor.Log("Starting multiplayer with anti-cheat mode enabled.");
            }
        }

        /// <summary>An event handler called when metadata about an incoming player connection is received.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnPeerContextReceived(object sender, PeerContextReceivedEventArgs e)
        {
            if (!Context.IsMainPlayer)
                return;

            // log join
            this.Monitor.Log($"Player joined: {e.Peer.PlayerID}");

            // kick: can't validate mods if they don't have a recent version of SMAPI
            if (!e.Peer.HasSmapi)
            {
                this.PlayersToKick.Add(new PlayerSlot
                {
                    Peer = e.Peer,
                    Reason = "SMAPI must be installed to join this server",
                    CountDownSeconds = this.SecondsUntilKick
                });
            }

            // detect blocked mods
            IList<string> blockedModNames = new List<string>();
            foreach (var pair in this.ProhibitedMods)
            {
                if (e.Peer.GetMod(pair.Value) != null)
                    blockedModNames.Add(pair.Key);
            }

            // kick: blocked mods found
            if (blockedModNames.Any())
            {
                int count = blockedModNames.Count;
                this.PlayersToKick.Add(new PlayerSlot
                {
                    Peer = e.Peer,
                    Reason = $"Found {(count == 1 ? "a blocked mod" : $"{count} blocked mods")}",
                    CountDownSeconds = this.SecondsUntilKick,
                    BlockedModNames = blockedModNames.ToArray()
                });
            }
        }

        /// <summary>An event handler called when the connection to a player is dropped.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnPeerDisconnected(object sender, PeerDisconnectedEventArgs e)
        {
            if (!Context.IsMainPlayer)
                return;

            this.Monitor.Log($"Player quit: {e.Peer.PlayerID}");
        }

        /// <summary>An event handler called once per second.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsMainPlayer || !Context.IsWorldReady || !e.IsOneSecond)
                return;

            // kick players whose countdowns expired
            foreach (PlayerSlot slot in this.PlayersToKick)
            {
                slot.CountDownSeconds--;
                if (slot.CountDownSeconds < 0)
                {
                    // get player info
                    long playerID = slot.Peer.PlayerID;
                    string name = Game1.getOnlineFarmers().FirstOrDefault(p => p.UniqueMultiplayerID == slot.Peer.PlayerID)?.Name ?? slot.Peer.PlayerID.ToString();

                    // log message
                    this.Monitor.Log($"Kicking player {playerID} ({name}): {slot.Reason}.");

                    // send chat messages
                    this.SendPublicChat("/color red");
                    this.SendPublicChat($"{name}: you're being kicked by Anti-Cheat. Please review the server rules.");

                    this.SendPrivateMessage(playerID, $"{slot.Reason}.");
                    if (slot.BlockedModNames != null)
                        this.SendPrivateMessage(playerID, $"Please remove these mods: {string.Join(", ", slot.BlockedModNames.OrderBy(p => p))}.");

                    // kick player
                    this.KickPlayer(playerID);
                }
            }
            this.PlayersToKick.RemoveAll(p => p.CountDownSeconds < 0);
        }

        /// <summary>Send a chat message to all players.</summary>
        /// <param name="text">The chat text to send.</param>
        private void SendPublicChat(string text)
        {
            Game1.chatBox.activate();
            Game1.chatBox.setText(text);
            Game1.chatBox.chatBox.RecieveCommandInput('\r');
        }

        /// <summary>Send a private message to a specified player.</summary>
        /// <param name="playerID">The player ID.</param>
        /// <param name="text">The text to send.</param>
        private void SendPrivateMessage(long playerID, string text)
        {
            Game1.server.sendMessage(playerID, Multiplayer.chatMessage, Game1.player, this.Helper.Content.CurrentLocaleConstant, text);
        }

        /// <summary>Kick a player from the server.</summary>
        /// <param name="playerID">The unique player ID.</param>
        private void KickPlayer(long playerID)
        {
            // kick player
            try
            {
                Game1.server.sendMessage(playerID, new OutgoingMessage(Multiplayer.disconnecting, playerID));
            }
            catch { /* ignore error if we can't connect to the player */ }
            Game1.server.playerDisconnected(playerID);
            Game1.otherFarmers.Remove(playerID);
        }
    }
}
