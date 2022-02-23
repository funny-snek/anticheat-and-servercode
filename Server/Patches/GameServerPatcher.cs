using StardewValley;

namespace FunnySnek.AntiCheat.Server.Patches
{
    /// <summary>Harmony patch for kick.</summary>
    internal class GameServerPatcher
    {
        /*********
        ** Public methods
        *********/
        public static bool Prefix(long peerId)
        {
            if (Game1.IsServer && (!Game1.otherFarmers.ContainsKey(peerId)))
            {
                //They have been kicked off the server
                return false;
            }

            return true;
        }
    }
}
