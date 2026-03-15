using StardewModdingAPI;

namespace FunnySnek.AntiCheat.Server.Framework
{
    /// <summary>A connected player slot.</summary>
    internal class PlayerSlot
    {
        /*********
        ** Accessors
        *********/
        /// <summary>The metadata for this player.</summary>
        public IMultiplayerPeer Peer { get; }

        /// <summary>The names of the blocked mods the player has installed.</summary>
        public string[] BlockedModNames { get; }

        /// <summary>The number of seconds until the player should be kicked.</summary>
        public int CountDownSeconds { get; set; }


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="peer"><inheritdoc cref="Peer" path="/summary"/></param>
        /// <param name="blockedModNames"><inheritdoc cref="BlockedModNames" path="/summary"/></param>
        /// <param name="countDownSeconds"><inheritdoc cref="CountDownSeconds" path="/summary"/></param>
        public PlayerSlot(IMultiplayerPeer peer, string[] blockedModNames, int countDownSeconds)
        {
            this.Peer = peer;
            this.BlockedModNames = blockedModNames;
            this.CountDownSeconds = countDownSeconds;
        }
    }
}
