using System;
using System.IO;
using StardewValley;

namespace FunnySnek.AntiCheat.Server.Framework.Patches
{
    internal class Multiplayer_ParseServerToClientsMessage_Patcher : Patch  //this totally works!
    {
        /*********
        ** Properties
        *********/
        protected override PatchDescriptor GetPatchDescriptor() => new PatchDescriptor(typeof(Multiplayer), "parseServerToClientsMessage");


        /*********
        ** Public methods
        *********/
        public static void Prefix(string message) //not sure how to do this line?
        {
            if (message.Substring(0, 4) == "SCAZ")//currentpass
            {
                //save message to txt file
                try
                {

                    //Pass the filepath and filename to the StreamWriter Constructor
                    StreamWriter sw = new StreamWriter("Mods/anticheatviachat/ReceivedMultiplayerID.txt");

                    //Write a line of text
                    sw.WriteLine(message);
                    //Close the file
                    sw.Close();
                }
                catch (Exception b)
                {
                    Console.WriteLine("Exception: " + b.Message);
                }
                finally
                {
                    Console.WriteLine("Executing finally block.");
                }
                //use this to add message to my list in ModEntry
                ModEntry.Messages.Add(message);
                //////////////////////////////////////////
                // to send messages
                //Game1.client.sendMessage((byte) 18, myStringMessage)

            }

        }
    }
}
