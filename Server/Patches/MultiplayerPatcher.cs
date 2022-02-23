using StardewValley;
using StardewValley.Network;

namespace FunnySnek.AntiCheat.Server.Patches
{
    /// <summary>Harmony patch for making sure no messages are received from client.</summary>
    internal static class MultiplayerPatcher
    {
        /*********
        ** Public methods
        *********/
        public static bool Prefix(IncomingMessage msg)
        {
            if (Game1.IsServer && (msg == null || !Game1.otherFarmers.ContainsKey(msg.FarmerID)))
            {
                //They have been kicked off the server
                return false;
            }
            return true;
        }
    }
}
