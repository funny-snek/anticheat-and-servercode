﻿using StardewValley;
using StardewValley.Network;

namespace FunnySnek.AntiCheat.Server.Framework.Patches
{
    /// <summary>Harmony patch for making sure no messages are received from client.</summary>
    internal class Multiplayer_ProcessIncomingMessage_Patcher : Patch
    {
        /*********
        ** Properties
        *********/
        protected override PatchDescriptor GetPatchDescriptor() => new PatchDescriptor(typeof(Multiplayer), nameof(Multiplayer.processIncomingMessage));


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
