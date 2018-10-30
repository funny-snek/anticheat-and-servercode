using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Network;

namespace FunnySnek.AntiCheat.Server
{
    /// <summary>Harmony patch for kick.</summary>
    internal class Server_SendMessage_Patcher : Patch
    {
        /*********
        ** Properties
        *********/
        protected override PatchDescriptor GetPatchDescriptor() => new PatchDescriptor(typeof(GameServer), "sendMessage", new System.Type[] { typeof(long), typeof(OutgoingMessage) });


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

    /// <summary>Harmony patch for making sure no messages are received from client.</summary>
    internal class Multiplayer_ProcessIncomingMessage_Patcher : Patch
    {
        /*********
        ** Properties
        *********/
        protected override PatchDescriptor GetPatchDescriptor() => new PatchDescriptor(typeof(Multiplayer), "processIncomingMessage");


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

    internal class ModEntry : Mod
    {
        /*********
        ** Properties
        *********/
        private int connectedFarmersCount = Game1.otherFarmers.Count;
        private long[] newPlayerIDs;
        private HashSet<long> knownPlayerIDs = new HashSet<long>();
        private HashSet<long> playerIDs = new HashSet<long>();
        private HashSet<long> playerIDsFromMod = new HashSet<long>();
        private bool antiCheatMessageSent = false;
        private int kickTimer = 60; // how long in seconds before checking if a player sent id.
        //private string lastFragment = "this is the last sentence in chat";
        //private string chatMessageCheck = "this is to gate the check on the variable lastFragment";
        private string line;

        //connection slots
        private bool slot1Used = false;
        private bool slot2Used = false;
        private bool slot3Used = false;
        private bool slot4Used = false;
        private bool slot5Used = false;
        private bool slot6Used = false;
        private bool slot7Used = false;
        private bool slot8Used = false;
        private bool slot9Used = false;
        private bool slot10Used = false;
        private bool slot11Used = false;
        private bool slot12Used = false;
        private bool slot13Used = false;
        private bool slot14Used = false;
        private bool slot15Used = false;
        private bool slot16Used = false;
        private bool slot17Used = false;
        private bool slot18Used = false;
        private bool slot19Used = false;
        private bool slot20Used = false;
        private bool slot21Used = false;
        private bool slot22Used = false;
        private bool slot23Used = false;
        private bool slot24Used = false;
        private bool slot25Used = false;
        private bool slot26Used = false;
        private bool slot27Used = false;
        private bool slot28Used = false;
        private bool slot29Used = false;
        private bool slot30Used = false;

        //multiplayerID Variables
        private long multiplayerID1;
        private long multiplayerID2;
        private long multiplayerID3;
        private long multiplayerID4;
        private long multiplayerID5;
        private long multiplayerID6;
        private long multiplayerID7;
        private long multiplayerID8;
        private long multiplayerID9;
        private long multiplayerID10;
        private long multiplayerID11;
        private long multiplayerID12;
        private long multiplayerID13;
        private long multiplayerID14;
        private long multiplayerID15;
        private long multiplayerID16;
        private long multiplayerID17;
        private long multiplayerID18;
        private long multiplayerID19;
        private long multiplayerID20;
        private long multiplayerID21;
        private long multiplayerID22;
        private long multiplayerID23;
        private long multiplayerID24;
        private long multiplayerID25;
        private long multiplayerID26;
        private long multiplayerID27;
        private long multiplayerID28;
        private long multiplayerID29;
        private long multiplayerID30;

        //GalaxyID Variables
        /*private GalaxyID galaxyID1;
        private GalaxyID galaxyID2;
        private GalaxyID galaxyID3;
        private GalaxyID galaxyID4;
        private GalaxyID galaxyID5;
        private GalaxyID galaxyID6;
        private GalaxyID galaxyID7;
        private GalaxyID galaxyID8;
        private GalaxyID galaxyID9;
        private GalaxyID galaxyID10;
        private GalaxyID galaxyID11;
        private GalaxyID galaxyID12;
        private GalaxyID galaxyID13;
        private GalaxyID galaxyID14;
        private GalaxyID galaxyID15;
        private GalaxyID galaxyID16;
        private GalaxyID galaxyID17;
        private GalaxyID galaxyID18;
        private GalaxyID galaxyID19;
        private GalaxyID galaxyID20;
        private GalaxyID galaxyID21;
        private GalaxyID galaxyID22;
        private GalaxyID galaxyID23;
        private GalaxyID galaxyID24;
        private GalaxyID galaxyID25;
        private GalaxyID galaxyID26;
        private GalaxyID galaxyID27;
        private GalaxyID galaxyID28;
        private GalaxyID galaxyID29;
        private GalaxyID galaxyID30;*/

        //Private counter for each slot to give time between new player joined and check if received ID
        private bool slot1CountDown = false;
        private bool slot2CountDown = false;
        private bool slot3CountDown = false;
        private bool slot4CountDown = false;
        private bool slot5CountDown = false;
        private bool slot6CountDown = false;
        private bool slot7CountDown = false;
        private bool slot8CountDown = false;
        private bool slot9CountDown = false;
        private bool slot10CountDown = false;
        private bool slot11CountDown = false;
        private bool slot12CountDown = false;
        private bool slot13CountDown = false;
        private bool slot14CountDown = false;
        private bool slot15CountDown = false;
        private bool slot16CountDown = false;
        private bool slot17CountDown = false;
        private bool slot18CountDown = false;
        private bool slot19CountDown = false;
        private bool slot20CountDown = false;
        private bool slot21CountDown = false;
        private bool slot22CountDown = false;
        private bool slot23CountDown = false;
        private bool slot24CountDown = false;
        private bool slot25CountDown = false;
        private bool slot26CountDown = false;
        private bool slot27CountDown = false;
        private bool slot28CountDown = false;
        private bool slot29CountDown = false;
        private bool slot30CountDown = false;

        //ticks for each countdown counter
        private int oneSecondTicks1;
        private int oneSecondTicks2;
        private int oneSecondTicks3;
        private int oneSecondTicks4;
        private int oneSecondTicks5;
        private int oneSecondTicks6;
        private int oneSecondTicks7;
        private int oneSecondTicks8;
        private int oneSecondTicks9;
        private int oneSecondTicks10;
        private int oneSecondTicks11;
        private int oneSecondTicks12;
        private int oneSecondTicks13;
        private int oneSecondTicks14;
        private int oneSecondTicks15;
        private int oneSecondTicks16;
        private int oneSecondTicks17;
        private int oneSecondTicks18;
        private int oneSecondTicks19;
        private int oneSecondTicks20;
        private int oneSecondTicks21;
        private int oneSecondTicks22;
        private int oneSecondTicks23;
        private int oneSecondTicks24;
        private int oneSecondTicks25;
        private int oneSecondTicks26;
        private int oneSecondTicks27;
        private int oneSecondTicks28;
        private int oneSecondTicks29;
        private int oneSecondTicks30;

        //Private counter for each Galaxy ID kick
        /*private bool galaxy1CountDown = false;
        private bool galaxy2CountDown = false;
        private bool galaxy3CountDown = false;
        private bool galaxy4CountDown = false;
        private bool galaxy5CountDown = false;
        private bool galaxy6CountDown = false;
        private bool galaxy7CountDown = false;
        private bool galaxy8CountDown = false;
        private bool galaxy9CountDown = false;
        private bool galaxy10CountDown = false;
        private bool galaxy11CountDown = false;
        private bool galaxy12CountDown = false;
        private bool galaxy13CountDown = false;
        private bool galaxy14CountDown = false;
        private bool galaxy15CountDown = false;
        private bool galaxy16CountDown = false;
        private bool galaxy17CountDown = false;
        private bool galaxy18CountDown = false;
        private bool galaxy19CountDown = false;
        private bool galaxy20CountDown = false;
        private bool galaxy21CountDown = false;
        private bool galaxy22CountDown = false;
        private bool galaxy23CountDown = false;
        private bool galaxy24CountDown = false;
        private bool galaxy25CountDown = false;
        private bool galaxy26CountDown = false;
        private bool galaxy27CountDown = false;
        private bool galaxy28CountDown = false;
        private bool galaxy29CountDown = false;
        private bool galaxy30CountDown = false;*/

        //ticks for each galaxyID countdown counter
        /*private int galaxySecondTicks1;
        private int galaxySecondTicks2;
        private int galaxySecondTicks3;
        private int galaxySecondTicks4;
        private int galaxySecondTicks5;
        private int galaxySecondTicks6;
        private int galaxySecondTicks7;
        private int galaxySecondTicks8;
        private int galaxySecondTicks9;
        private int galaxySecondTicks10;
        private int galaxySecondTicks11;
        private int galaxySecondTicks12;
        private int galaxySecondTicks13;
        private int galaxySecondTicks14;
        private int galaxySecondTicks15;
        private int galaxySecondTicks16;
        private int galaxySecondTicks17;
        private int galaxySecondTicks18;
        private int galaxySecondTicks19;
        private int galaxySecondTicks20;
        private int galaxySecondTicks21;
        private int galaxySecondTicks22;
        private int galaxySecondTicks23;
        private int galaxySecondTicks24;
        private int galaxySecondTicks25;
        private int galaxySecondTicks26;
        private int galaxySecondTicks27;
        private int galaxySecondTicks28;
        private int galaxySecondTicks29;
        private int galaxySecondTicks30;*/

        //dont run kick code if they quit manually
        private bool didQuit1 = false;
        private bool didQuit2 = false;
        private bool didQuit3 = false;
        private bool didQuit4 = false;
        private bool didQuit5 = false;
        private bool didQuit6 = false;
        private bool didQuit7 = false;
        private bool didQuit8 = false;
        private bool didQuit9 = false;
        private bool didQuit10 = false;
        private bool didQuit11 = false;
        private bool didQuit12 = false;
        private bool didQuit13 = false;
        private bool didQuit14 = false;
        private bool didQuit15 = false;
        private bool didQuit16 = false;
        private bool didQuit17 = false;
        private bool didQuit18 = false;
        private bool didQuit19 = false;
        private bool didQuit20 = false;
        private bool didQuit21 = false;
        private bool didQuit22 = false;
        private bool didQuit23 = false;
        private bool didQuit24 = false;
        private bool didQuit25 = false;
        private bool didQuit26 = false;
        private bool didQuit27 = false;
        private bool didQuit28 = false;
        private bool didQuit29 = false;
        private bool didQuit30 = false;


        /*********
        ** Accessors
        *********/
        /// <summary>use this to fix messaging when you get a chance.</summary>
        public static readonly List<string> Messages = new List<string>();//to store messages


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            Patch.PatchAll("anticheatviachat.anticheatviachat");
            GameEvents.OneSecondTick += this.OnOneSecondTick;
            GameEvents.FourthUpdateTick += this.OnFourthUpdateTick;

        }


        /*********
        ** Private methods
        *********/
        private void WhatColorToSayWhenKickBefore()
        {
            Game1.chatBox.activate();
            Game1.chatBox.setText("/color red");
            Game1.chatBox.chatBox.RecieveCommandInput('\r');
        }

        private void WhatToSayWhenKickAfter()
        {
            Game1.chatBox.activate();
            Game1.chatBox.setText("Please Install Latest ServerPack");
            Game1.chatBox.chatBox.RecieveCommandInput('\r');
        }

        private void OnFourthUpdateTick(object sender, EventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (this.antiCheatMessageSent == false)
            {
                Game1.chatBox.activate();
                Game1.chatBox.setText("Anticheat Activated");
                Game1.chatBox.chatBox.RecieveCommandInput('\r');
                this.antiCheatMessageSent = true;
            }

            //start get multiplayer ID
            this.ReadReceivedMultiplayerID();
            string cleanFragment = "a";

            if (this.line.Length >= 4)
            {
                cleanFragment = this.line.Substring(4); //starts a new string at 5th character 
            }
            if (long.TryParse(cleanFragment, out long lastID))
            {
                if (!this.playerIDsFromMod.Contains(lastID))
                {
                    this.Monitor.Log($"ID From Mod: {lastID}");
                    this.playerIDsFromMod.Add(lastID);
                }
                //clear out txt file holding latest ID
                try
                {

                    //Pass the filepath and filename to the StreamWriter Constructor
                    StreamWriter sw = new StreamWriter("Mods/anticheatviachat/ReceivedMultiplayerID.txt");

                    //Write a line of text
                    sw.WriteLine(" ");
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

            }

        }

        //read from file
        private void ReadReceivedMultiplayerID()
        {

            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("Mods/anticheatviachat/ReceivedMultiplayerID.txt"))
                {
                    // Read the stream to a string, and write the string to the console.
                    this.line = sr.ReadToEnd();

                }
            }
            catch (Exception e)
            {
                this.Monitor.Log("The file could not be read:");
                this.Monitor.Log(e.Message);
            }
        }

        private void OnOneSecondTick(object sender, EventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // if someone new connects to the game store their ID's in am unused variable slot
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (this.connectedFarmersCount < Game1.otherFarmers.Count)
            {
                //store newest joined player(s) to a list, exclude already joined players (knonwnPlayerIDs)
                this.newPlayerIDs = Game1
                    .getOnlineFarmers()
                    .Select(p => p.UniqueMultiplayerID)
                    .Except(this.knownPlayerIDs)
                    .ToArray();


                if (this.slot1Used == false)
                {
                    this.multiplayerID1 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID1);

                    this.Monitor.Log($"New Player: {this.multiplayerID1}");
                    this.connectedFarmersCount += 1;
                    this.slot1CountDown = true;
                    this.slot1Used = true;
                    return;
                }

                if (this.slot2Used == false)
                {
                    this.multiplayerID2 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID2);
                    this.Monitor.Log($"New Player: {this.multiplayerID2}");
                    this.connectedFarmersCount += 1;
                    this.slot2CountDown = true;
                    this.slot2Used = true;
                    return;
                }

                if (this.slot3Used == false)
                {
                    this.multiplayerID3 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID3);
                    this.Monitor.Log($"New Player: {this.multiplayerID3}");
                    this.connectedFarmersCount += 1;
                    this.slot3CountDown = true;
                    this.slot3Used = true;
                    return;
                }
                if (this.slot4Used == false)
                {
                    this.multiplayerID4 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID4);

                    this.Monitor.Log($"New Player: {this.multiplayerID4}");

                    this.connectedFarmersCount += 1;
                    this.slot4CountDown = true;
                    this.slot4Used = true;
                    return;
                }
                if (this.slot5Used == false)
                {
                    this.multiplayerID5 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID5);
                    this.Monitor.Log($"New Player: {this.multiplayerID5}");
                    this.connectedFarmersCount += 1;
                    this.slot5CountDown = true;
                    this.slot5Used = true;
                    return;
                }
                if (this.slot6Used == false)
                {
                    this.multiplayerID6 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID6);
                    this.Monitor.Log($"New Player: {this.multiplayerID6}");
                    this.connectedFarmersCount += 1;
                    this.slot6CountDown = true;
                    this.slot6Used = true;
                    return;
                }
                if (this.slot7Used == false)
                {
                    this.multiplayerID7 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID7);
                    this.Monitor.Log($"New Player: {this.multiplayerID7}");
                    this.connectedFarmersCount += 1;
                    this.slot7CountDown = true;
                    this.slot7Used = true;
                    return;
                }
                if (this.slot8Used == false)
                {
                    this.multiplayerID8 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID8);
                    this.Monitor.Log($"New Player: {this.multiplayerID8}");
                    this.connectedFarmersCount += 1;
                    this.slot8CountDown = true;
                    this.slot8Used = true;
                    return;
                }

                if (this.slot9Used == false)
                {
                    this.multiplayerID9 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID9);
                    this.Monitor.Log($"New Player: {this.multiplayerID9}");
                    this.connectedFarmersCount += 1;
                    this.slot9CountDown = true;
                    this.slot9Used = true;
                    return;
                }
                if (this.slot10Used == false)
                {
                    this.multiplayerID10 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID10);
                    this.Monitor.Log($"New Player: {this.multiplayerID10}");
                    this.connectedFarmersCount += 1;
                    this.slot10CountDown = true;
                    this.slot10Used = true;
                    return;
                }
                if (this.slot11Used == false)
                {
                    this.multiplayerID11 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID11);
                    this.Monitor.Log($"New Player: {this.multiplayerID11}");
                    this.connectedFarmersCount += 1;
                    this.slot11CountDown = true;
                    this.slot11Used = true;
                    return;
                }

                if (this.slot12Used == false)
                {
                    this.multiplayerID12 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID12);
                    this.Monitor.Log($"New Player: {this.multiplayerID12}");
                    this.connectedFarmersCount += 1;
                    this.slot12CountDown = true;
                    this.slot12Used = true;
                    return;
                }

                if (this.slot13Used == false)
                {
                    this.multiplayerID13 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID13);
                    this.Monitor.Log($"New Player: {this.multiplayerID3}");
                    this.connectedFarmersCount += 1;
                    this.slot13CountDown = true;
                    this.slot13Used = true;
                    return;
                }
                if (this.slot14Used == false)
                {
                    this.multiplayerID14 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID14);
                    this.Monitor.Log($"New Player: {this.multiplayerID14}");
                    this.connectedFarmersCount += 1;
                    this.slot14CountDown = true;
                    this.slot14Used = true;
                    return;
                }
                if (this.slot15Used == false)
                {
                    this.multiplayerID15 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID15);
                    this.Monitor.Log($"New Player: {this.multiplayerID15}");
                    this.connectedFarmersCount += 1;
                    this.slot15CountDown = true;
                    this.slot15Used = true;
                    return;
                }
                if (this.slot16Used == false)
                {
                    this.multiplayerID16 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID16);
                    this.Monitor.Log($"New Player: {this.multiplayerID16}");
                    this.connectedFarmersCount += 1;
                    this.slot16CountDown = true;
                    this.slot16Used = true;
                    return;
                }
                if (this.slot17Used == false)
                {
                    this.multiplayerID17 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID17);
                    this.Monitor.Log($"New Player: {this.multiplayerID17}");
                    this.connectedFarmersCount += 1;
                    this.slot17CountDown = true;
                    this.slot17Used = true;
                    return;
                }
                if (this.slot18Used == false)
                {
                    this.multiplayerID18 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID18);
                    this.Monitor.Log($"New Player: {this.multiplayerID18}");
                    this.connectedFarmersCount += 1;
                    this.slot18CountDown = true;
                    this.slot18Used = true;
                    return;
                }
                if (this.slot19Used == false)
                {
                    this.multiplayerID19 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID19);
                    this.Monitor.Log($"New Player: {this.multiplayerID19}");
                    this.connectedFarmersCount += 1;
                    this.slot19CountDown = true;
                    this.slot19Used = true;
                    return;
                }
                if (this.slot20Used == false)
                {
                    this.multiplayerID20 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID20);
                    this.Monitor.Log($"New Player: {this.multiplayerID20}");
                    this.connectedFarmersCount += 1;
                    this.slot20CountDown = true;
                    this.slot20Used = true;
                    return;
                }
                if (this.slot21Used == false)
                {
                    this.multiplayerID21 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID21);
                    this.Monitor.Log($"New Player: {this.multiplayerID21}");
                    this.connectedFarmersCount += 1;
                    this.slot21CountDown = true;
                    this.slot21Used = true;
                    return;
                }

                if (this.slot22Used == false)
                {
                    this.multiplayerID22 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID22);
                    this.Monitor.Log($"New Player: {this.multiplayerID22}");
                    this.connectedFarmersCount += 1;
                    this.slot22CountDown = true;
                    this.slot22Used = true;
                    return;
                }

                if (this.slot23Used == false)
                {
                    this.multiplayerID23 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID23);
                    this.Monitor.Log($"New Player: {this.multiplayerID23}");
                    this.connectedFarmersCount += 1;
                    this.slot23CountDown = true;
                    this.slot23Used = true;
                    return;
                }
                if (this.slot24Used == false)
                {
                    this.multiplayerID24 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID24);
                    this.Monitor.Log($"New Player: {this.multiplayerID24}");
                    this.connectedFarmersCount += 1;
                    this.slot24CountDown = true;
                    this.slot24Used = true;
                    return;
                }
                if (this.slot25Used == false)
                {
                    this.multiplayerID25 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID25);
                    this.Monitor.Log($"New Player: {this.multiplayerID25}");
                    this.connectedFarmersCount += 1;
                    this.slot25CountDown = true;
                    this.slot25Used = true;
                    return;
                }
                if (this.slot26Used == false)
                {
                    this.multiplayerID26 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID26);
                    this.Monitor.Log($"New Player: {this.multiplayerID26}");
                    this.connectedFarmersCount += 1;
                    this.slot26CountDown = true;
                    this.slot26Used = true;
                    return;
                }
                if (this.slot27Used == false)
                {
                    this.multiplayerID27 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID27);
                    this.Monitor.Log($"New Player: {this.multiplayerID27}");
                    this.connectedFarmersCount += 1;
                    this.slot27CountDown = true;
                    this.slot27Used = true;
                    return;
                }
                if (this.slot28Used == false)
                {
                    this.multiplayerID28 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID28);
                    this.Monitor.Log($"New Player: {this.multiplayerID28}");
                    this.connectedFarmersCount += 1;
                    this.slot28CountDown = true;
                    this.slot28Used = true;
                    return;
                }

                if (this.slot29Used == false)
                {
                    this.multiplayerID29 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID29);
                    this.Monitor.Log($"New Player: {this.multiplayerID29}");
                    this.connectedFarmersCount += 1;
                    this.slot29CountDown = true;
                    this.slot29Used = true;
                    return;
                }
                if (this.slot30Used == false)
                {
                    this.multiplayerID30 = this.newPlayerIDs.LastOrDefault();
                    this.knownPlayerIDs.Add(this.multiplayerID30);
                    this.Monitor.Log($"New Player: {this.multiplayerID30}");
                    this.connectedFarmersCount += 1;
                    this.slot30CountDown = true;
                    this.slot30Used = true;
                    return;
                }
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //if someone disconnects from the game find out who it was and open up their used variable slot
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (this.connectedFarmersCount > Game1.otherFarmers.Count)
            {
                this.playerIDs.Clear();
                //store all game MultiplayerIDs to a list
                foreach (Farmer onlineFarmer in Game1.getOnlineFarmers())
                {
                    this.playerIDs.Add(onlineFarmer.UniqueMultiplayerID);
                }

                if (this.slot1Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID1))// player has left game if we have their ID stored but the game doesn't any more
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID1))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID1);
                            this.Monitor.Log($"Removing: {this.multiplayerID1}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID1);
                        this.connectedFarmersCount -= 1;
                        this.didQuit1 = true;
                        this.slot1Used = false;
                    }
                }
                if (this.slot2Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID2))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID2))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID2);
                            this.Monitor.Log($"Removing: {this.multiplayerID2}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID2);
                        this.connectedFarmersCount -= 1;
                        this.didQuit2 = true;
                        this.slot2Used = false;
                    }
                }
                if (this.slot3Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID3))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID3))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID3);
                            this.Monitor.Log($"Removing: {this.multiplayerID3}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID3);
                        this.connectedFarmersCount -= 1;
                        this.didQuit3 = true;
                        this.slot3Used = false;
                    }
                }
                if (this.slot4Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID4))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID4))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID4);
                            this.Monitor.Log($"Removing: {this.multiplayerID4}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID4);
                        this.connectedFarmersCount -= 1;
                        this.didQuit4 = true;
                        this.slot4Used = false;
                    }
                }
                if (this.slot5Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID5))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID5))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID5);
                            this.Monitor.Log($"Removing: {this.multiplayerID5}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID5);
                        this.connectedFarmersCount -= 1;
                        this.didQuit5 = true;
                        this.slot5Used = false;
                    }
                }
                if (this.slot6Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID6))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID6))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID6);
                            this.Monitor.Log($"Removing: {this.multiplayerID6}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID6);
                        this.connectedFarmersCount -= 1;
                        this.didQuit6 = true;
                        this.slot6Used = false;
                    }
                }
                if (this.slot7Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID7))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID7))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID7);
                            this.Monitor.Log($"Removing: {this.multiplayerID7}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID7);
                        this.connectedFarmersCount -= 1;
                        this.didQuit7 = true;
                        this.slot7Used = false;
                    }
                }
                if (this.slot8Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID8))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID8))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID8);
                            this.Monitor.Log($"Removing: {this.multiplayerID8}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID8);
                        this.connectedFarmersCount -= 1;
                        this.didQuit8 = true;
                        this.slot8Used = false;
                    }
                }
                if (this.slot9Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID9))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID9))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID9);
                            this.Monitor.Log($"Removing: {this.multiplayerID9}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID9);
                        this.connectedFarmersCount -= 1;
                        this.didQuit9 = true;
                        this.slot9Used = false;
                    }
                }
                if (this.slot10Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID10))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID10))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID10);
                            this.Monitor.Log($"Removing: {this.multiplayerID10}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID10);
                        this.connectedFarmersCount -= 1;
                        this.didQuit10 = true;
                        this.slot10Used = false;
                    }
                }
                if (this.slot11Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID11))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID11))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID11);
                            this.Monitor.Log($"Removing: {this.multiplayerID11}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID11);
                        this.connectedFarmersCount -= 1;
                        this.didQuit11 = true;
                        this.slot11Used = false;
                    }
                }
                if (this.slot12Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID12))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID12))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID12);
                            this.Monitor.Log($"Removing: {this.multiplayerID12}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID12);
                        this.connectedFarmersCount -= 1;
                        this.didQuit12 = true;
                        this.slot12Used = false;
                    }
                }
                if (this.slot13Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID13))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID13))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID13);
                            this.Monitor.Log($"Removing: {this.multiplayerID13}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID13);
                        this.connectedFarmersCount -= 1;
                        this.didQuit13 = true;
                        this.slot13Used = false;
                    }
                }
                if (this.slot14Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID14))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID14))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID14);
                            this.Monitor.Log($"Removing: {this.multiplayerID14}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID14);
                        this.connectedFarmersCount -= 1;
                        this.didQuit14 = true;
                        this.slot14Used = false;
                    }
                }
                if (this.slot15Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID15))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID15))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID15);
                            this.Monitor.Log($"Removing: {this.multiplayerID15}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID15);
                        this.connectedFarmersCount -= 1;
                        this.didQuit15 = true;
                        this.slot15Used = false;
                    }
                }
                if (this.slot16Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID16))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID16))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID16);
                            this.Monitor.Log($"Removing: {this.multiplayerID16}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID16);
                        this.connectedFarmersCount -= 1;
                        this.didQuit16 = true;
                        this.slot16Used = false;
                    }
                }
                if (this.slot17Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID17))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID17))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID17);
                            this.Monitor.Log($"Removing: {this.multiplayerID17}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID17);
                        this.connectedFarmersCount -= 1;
                        this.didQuit17 = true;
                        this.slot17Used = false;
                    }
                }
                if (this.slot18Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID18))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID18))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID18);
                            this.Monitor.Log($"Removing: {this.multiplayerID18}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID18);
                        this.connectedFarmersCount -= 1;
                        this.didQuit18 = true;
                        this.slot18Used = false;
                    }
                }
                if (this.slot19Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID19))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID19))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID19);
                            this.Monitor.Log($"Removing: {this.multiplayerID19}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID19);
                        this.connectedFarmersCount -= 1;
                        this.didQuit19 = true;
                        this.slot19Used = false;
                    }
                }
                if (this.slot20Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID20))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID20))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID20);
                            this.Monitor.Log($"Removing: {this.multiplayerID20}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID20);
                        this.connectedFarmersCount -= 1;
                        this.didQuit20 = true;
                        this.slot20Used = false;
                    }
                }
                if (this.slot21Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID21))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID21))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID21);
                            this.Monitor.Log($"Removing: {this.multiplayerID21}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID21);
                        this.connectedFarmersCount -= 1;
                        this.didQuit21 = true;
                        this.slot21Used = false;
                    }
                }
                if (this.slot22Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID22))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID22))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID22);
                            this.Monitor.Log($"Removing: {this.multiplayerID22}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID22);
                        this.connectedFarmersCount -= 1;
                        this.didQuit22 = true;
                        this.slot22Used = false;
                    }
                }
                if (this.slot23Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID23))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID23))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID23);
                            this.Monitor.Log($"Removing: {this.multiplayerID23}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID23);
                        this.connectedFarmersCount -= 1;
                        this.didQuit23 = true;
                        this.slot23Used = false;
                    }
                }
                if (this.slot24Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID24))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID24))
                            this.Monitor.Log($"Removing: {this.multiplayerID24}");
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID24);
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID24);
                        this.connectedFarmersCount -= 1;
                        this.didQuit24 = true;
                        this.slot24Used = false;
                    }
                }
                if (this.slot25Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID25))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID25))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID25);
                            this.Monitor.Log($"Removing: {this.multiplayerID25}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID25);
                        this.connectedFarmersCount -= 1;
                        this.didQuit25 = true;
                        this.slot25Used = false;
                    }
                }
                if (this.slot26Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID26))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID26))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID26);
                            this.Monitor.Log($"Removing: {this.multiplayerID26}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID26);
                        this.connectedFarmersCount -= 1;
                        this.didQuit26 = true;
                        this.slot26Used = false;
                    }
                }
                if (this.slot27Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID27))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID27))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID27);
                            this.Monitor.Log($"Removing: {this.multiplayerID27}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID27);
                        this.connectedFarmersCount -= 1;
                        this.didQuit27 = true;
                        this.slot27Used = false;
                    }
                }
                if (this.slot28Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID28))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID28))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID28);
                            this.Monitor.Log($"Removing: {this.multiplayerID28}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID28);
                        this.connectedFarmersCount -= 1;
                        this.didQuit28 = true;
                        this.slot28Used = false;
                    }
                }
                if (this.slot29Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID29))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID29))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID29);
                            this.Monitor.Log($"Removing: {this.multiplayerID29}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID29);
                        this.connectedFarmersCount -= 1;
                        this.didQuit29 = true;
                        this.slot29Used = false;
                    }
                }
                if (this.slot30Used)
                {
                    if (!this.playerIDs.Contains(this.multiplayerID30))
                    {
                        if (this.playerIDsFromMod.Contains(this.multiplayerID30))
                        {
                            this.playerIDsFromMod.Remove(this.multiplayerID30);
                            this.Monitor.Log($"Removing: {this.multiplayerID30}");
                        }
                        this.knownPlayerIDs.Remove(this.multiplayerID30);
                        this.connectedFarmersCount -= 1;
                        this.didQuit30 = true;
                        this.slot30Used = false;
                    }
                }
            }


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //if a slot is used countdown until check if they sent their ID through mod, if not kick
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (this.slot1CountDown)
            {

                this.oneSecondTicks1 += 1;
                if (this.oneSecondTicks1 >= this.kickTimer)
                {
                    if (this.didQuit1)
                    {
                        this.oneSecondTicks1 = 0;
                        this.slot1CountDown = false;
                        this.didQuit1 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID1))
                    {

                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID1, new OutgoingMessage(19, this.multiplayerID1));
                        Game1.server.playerDisconnected(this.multiplayerID1);
                        Game1.otherFarmers.Remove(this.multiplayerID1);
                        this.knownPlayerIDs.Remove(this.multiplayerID1);
                        this.WhatToSayWhenKickAfter();

                        this.connectedFarmersCount -= 1;
                        this.slot1Used = false;
                    }
                    this.oneSecondTicks1 = 0;
                    this.slot1CountDown = false;
                }
            }

            if (this.slot2CountDown)
            {
                this.oneSecondTicks2 += 1;
                if (this.oneSecondTicks2 >= this.kickTimer)
                {
                    if (this.didQuit2)
                    {
                        this.oneSecondTicks2 = 0;
                        this.slot2CountDown = false;
                        this.didQuit2 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID2))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID2, new OutgoingMessage(19, this.multiplayerID2));
                        Game1.server.playerDisconnected(this.multiplayerID2);
                        Game1.otherFarmers.Remove(this.multiplayerID2);
                        this.knownPlayerIDs.Remove(this.multiplayerID2);


                        this.WhatToSayWhenKickAfter();

                        this.connectedFarmersCount -= 1;
                        this.slot2Used = false;
                    }
                    this.oneSecondTicks2 = 0;
                    this.slot2CountDown = false;
                }
            }

            if (this.slot3CountDown)
            {
                this.oneSecondTicks3 += 1;
                if (this.oneSecondTicks3 >= this.kickTimer)
                {
                    if (this.didQuit3)
                    {
                        this.oneSecondTicks3 = 0;
                        this.slot3CountDown = false;
                        this.didQuit3 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID3))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID3, new OutgoingMessage(19, this.multiplayerID3));
                        Game1.server.playerDisconnected(this.multiplayerID3);
                        Game1.otherFarmers.Remove(this.multiplayerID3);
                        this.knownPlayerIDs.Remove(this.multiplayerID3);

                        this.WhatToSayWhenKickAfter();

                        this.connectedFarmersCount -= 1;
                        this.slot3Used = false;
                    }
                    this.oneSecondTicks3 = 0;
                    this.slot3CountDown = false;
                }
            }

            if (this.slot4CountDown)
            {

                this.oneSecondTicks4 += 1;
                if (this.oneSecondTicks4 >= this.kickTimer)
                {
                    if (this.didQuit4)
                    {
                        this.oneSecondTicks4 = 0;
                        this.slot4CountDown = false;
                        this.didQuit4 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID4))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID4, new OutgoingMessage(19, this.multiplayerID4));
                        Game1.server.playerDisconnected(this.multiplayerID4);
                        Game1.otherFarmers.Remove(this.multiplayerID4);
                        this.knownPlayerIDs.Remove(this.multiplayerID4);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot4Used = false;
                    }
                    this.oneSecondTicks4 = 0;
                    this.slot4CountDown = false;
                }
            }

            if (this.slot5CountDown)
            {
                this.oneSecondTicks5 += 1;
                if (this.oneSecondTicks5 >= this.kickTimer)
                {
                    if (this.didQuit5)
                    {
                        this.oneSecondTicks5 = 0;
                        this.slot5CountDown = false;
                        this.didQuit5 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID5))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID5, new OutgoingMessage(19, this.multiplayerID5));
                        Game1.server.playerDisconnected(this.multiplayerID5);
                        Game1.otherFarmers.Remove(this.multiplayerID5);
                        this.knownPlayerIDs.Remove(this.multiplayerID5);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot5Used = false;
                    }
                    this.oneSecondTicks5 = 0;
                    this.slot5CountDown = false;
                }
            }

            if (this.slot6CountDown)
            {
                this.oneSecondTicks6 += 1;
                if (this.oneSecondTicks6 >= this.kickTimer)
                {

                    if (this.didQuit6)
                    {
                        this.oneSecondTicks6 = 0;
                        this.slot6CountDown = false;
                        this.didQuit6 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID6))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID6, new OutgoingMessage(19, this.multiplayerID6));
                        Game1.server.playerDisconnected(this.multiplayerID6);
                        Game1.otherFarmers.Remove(this.multiplayerID6);
                        this.knownPlayerIDs.Remove(this.multiplayerID6);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot6Used = false;
                    }
                    this.oneSecondTicks6 = 0;
                    this.slot6CountDown = false;
                }
            }
            if (this.slot7CountDown)
            {
                this.oneSecondTicks7 += 1;
                if (this.oneSecondTicks7 >= this.kickTimer)
                {
                    if (this.didQuit7)
                    {
                        this.oneSecondTicks7 = 0;
                        this.slot7CountDown = false;
                        this.didQuit7 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID7))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID7, new OutgoingMessage(19, this.multiplayerID7));
                        Game1.server.playerDisconnected(this.multiplayerID7);
                        Game1.otherFarmers.Remove(this.multiplayerID7);
                        this.knownPlayerIDs.Remove(this.multiplayerID7);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot7Used = false;
                    }
                    this.oneSecondTicks7 = 0;
                    this.slot7CountDown = false;
                }
            }
            if (this.slot8CountDown)
            {
                this.oneSecondTicks8 += 1;
                if (this.oneSecondTicks8 >= this.kickTimer)
                {
                    if (this.didQuit8)
                    {
                        this.oneSecondTicks8 = 0;
                        this.slot8CountDown = false;
                        this.didQuit8 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID8))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID8, new OutgoingMessage(19, this.multiplayerID8));
                        Game1.server.playerDisconnected(this.multiplayerID8);
                        Game1.otherFarmers.Remove(this.multiplayerID8);
                        this.knownPlayerIDs.Remove(this.multiplayerID8);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot8Used = false;
                    }
                    this.oneSecondTicks8 = 0;
                    this.slot8CountDown = false;
                }
            }
            if (this.slot9CountDown)
            {
                this.oneSecondTicks9 += 1;
                if (this.oneSecondTicks9 >= this.kickTimer)
                {
                    if (this.didQuit9)
                    {
                        this.oneSecondTicks9 = 0;
                        this.slot9CountDown = false;
                        this.didQuit9 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID9))
                    {
                        this.WhatColorToSayWhenKickBefore();
                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID9, new OutgoingMessage(19, this.multiplayerID9));
                        Game1.server.playerDisconnected(this.multiplayerID9);
                        Game1.otherFarmers.Remove(this.multiplayerID9);
                        this.knownPlayerIDs.Remove(this.multiplayerID9);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot9Used = false;
                    }
                    this.oneSecondTicks9 = 0;
                    this.slot9CountDown = false;
                }
            }
            if (this.slot10CountDown)
            {
                this.oneSecondTicks10 += 1;
                if (this.oneSecondTicks10 >= this.kickTimer)
                {
                    if (this.didQuit10)
                    {
                        this.oneSecondTicks10 = 0;
                        this.slot10CountDown = false;
                        this.didQuit10 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID10))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID10, new OutgoingMessage(19, this.multiplayerID10));
                        Game1.server.playerDisconnected(this.multiplayerID10);
                        Game1.otherFarmers.Remove(this.multiplayerID10);
                        this.knownPlayerIDs.Remove(this.multiplayerID10);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot10Used = false;
                    }
                    this.oneSecondTicks10 = 0;
                    this.slot10CountDown = false;
                }
            }
            if (this.slot11CountDown)
            {

                this.oneSecondTicks11 += 1;
                if (this.oneSecondTicks11 >= this.kickTimer)
                {
                    if (this.didQuit11)
                    {
                        this.oneSecondTicks11 = 0;
                        this.slot11CountDown = false;
                        this.didQuit11 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID11))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID11, new OutgoingMessage(19, this.multiplayerID11));
                        Game1.server.playerDisconnected(this.multiplayerID11);
                        Game1.otherFarmers.Remove(this.multiplayerID11);
                        this.knownPlayerIDs.Remove(this.multiplayerID11);

                        this.WhatToSayWhenKickAfter();

                        this.connectedFarmersCount -= 1;
                        this.slot11Used = false;
                    }
                    this.oneSecondTicks11 = 0;
                    this.slot11CountDown = false;
                }
            }

            if (this.slot12CountDown)
            {
                this.oneSecondTicks12 += 1;
                if (this.oneSecondTicks12 >= this.kickTimer)
                {
                    if (this.didQuit12)
                    {
                        this.oneSecondTicks12 = 0;
                        this.slot12CountDown = false;
                        this.didQuit12 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID12))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID12, new OutgoingMessage(19, this.multiplayerID12));
                        Game1.server.playerDisconnected(this.multiplayerID12);
                        Game1.otherFarmers.Remove(this.multiplayerID12);
                        this.knownPlayerIDs.Remove(this.multiplayerID12);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot12Used = false;
                    }
                    this.oneSecondTicks12 = 0;
                    this.slot12CountDown = false;
                }
            }

            if (this.slot13CountDown)
            {
                this.oneSecondTicks13 += 1;
                if (this.oneSecondTicks13 >= this.kickTimer)
                {
                    if (this.didQuit13)
                    {
                        this.oneSecondTicks13 = 0;
                        this.slot13CountDown = false;
                        this.didQuit13 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID13))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID13, new OutgoingMessage(19, this.multiplayerID13));
                        Game1.server.playerDisconnected(this.multiplayerID13);
                        Game1.otherFarmers.Remove(this.multiplayerID13);
                        this.knownPlayerIDs.Remove(this.multiplayerID13);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot13Used = false;
                    }
                    this.oneSecondTicks13 = 0;
                    this.slot13CountDown = false;
                }
            }

            if (this.slot14CountDown)
            {

                this.oneSecondTicks14 += 1;
                if (this.oneSecondTicks14 >= this.kickTimer)
                {
                    if (this.didQuit14)
                    {
                        this.oneSecondTicks14 = 0;
                        this.slot14CountDown = false;
                        this.didQuit14 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID14))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID14, new OutgoingMessage(19, this.multiplayerID14));
                        Game1.server.playerDisconnected(this.multiplayerID14);
                        Game1.otherFarmers.Remove(this.multiplayerID14);
                        this.knownPlayerIDs.Remove(this.multiplayerID14);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot14Used = false;
                    }
                    this.oneSecondTicks14 = 0;
                    this.slot14CountDown = false;
                }
            }

            if (this.slot15CountDown)
            {
                this.oneSecondTicks5 += 1;
                if (this.oneSecondTicks15 >= this.kickTimer)
                {
                    if (this.didQuit15)
                    {
                        this.oneSecondTicks15 = 0;
                        this.slot15CountDown = false;
                        this.didQuit15 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID15))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID15, new OutgoingMessage(19, this.multiplayerID15));
                        Game1.server.playerDisconnected(this.multiplayerID15);
                        Game1.otherFarmers.Remove(this.multiplayerID15);
                        this.knownPlayerIDs.Remove(this.multiplayerID15);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot15Used = false;
                    }
                    this.oneSecondTicks15 = 0;
                    this.slot15CountDown = false;
                }
            }

            if (this.slot16CountDown)
            {
                this.oneSecondTicks16 += 1;
                if (this.oneSecondTicks16 >= this.kickTimer)
                {
                    if (this.didQuit16)
                    {
                        this.oneSecondTicks16 = 0;
                        this.slot16CountDown = false;
                        this.didQuit16 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID16))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID16, new OutgoingMessage(19, this.multiplayerID16));
                        Game1.server.playerDisconnected(this.multiplayerID16);
                        Game1.otherFarmers.Remove(this.multiplayerID16);
                        this.knownPlayerIDs.Remove(this.multiplayerID16);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot16Used = false;
                    }
                    this.oneSecondTicks16 = 0;
                    this.slot16CountDown = false;
                }
            }
            if (this.slot17CountDown)
            {
                this.oneSecondTicks17 += 1;
                if (this.oneSecondTicks17 >= this.kickTimer)
                {
                    if (this.didQuit17)
                    {
                        this.oneSecondTicks17 = 0;
                        this.slot17CountDown = false;
                        this.didQuit17 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID17))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID17, new OutgoingMessage(19, this.multiplayerID17));
                        Game1.server.playerDisconnected(this.multiplayerID17);
                        Game1.otherFarmers.Remove(this.multiplayerID17);
                        this.knownPlayerIDs.Remove(this.multiplayerID17);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot17Used = false;
                    }
                    this.oneSecondTicks17 = 0;
                    this.slot17CountDown = false;
                }
            }
            if (this.slot18CountDown)
            {
                this.oneSecondTicks18 += 1;
                if (this.oneSecondTicks18 >= this.kickTimer)
                {
                    if (this.didQuit18)
                    {
                        this.oneSecondTicks18 = 0;
                        this.slot18CountDown = false;
                        this.didQuit18 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID18))
                    {
                        this.WhatColorToSayWhenKickBefore();
                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID18, new OutgoingMessage(19, this.multiplayerID18));
                        Game1.server.playerDisconnected(this.multiplayerID18);
                        Game1.otherFarmers.Remove(this.multiplayerID18);
                        this.knownPlayerIDs.Remove(this.multiplayerID18);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot18Used = false;
                    }
                    this.oneSecondTicks18 = 0;
                    this.slot18CountDown = false;
                }
            }
            if (this.slot19CountDown)
            {
                this.oneSecondTicks19 += 1;
                if (this.oneSecondTicks19 >= this.kickTimer)
                {
                    if (this.didQuit19)
                    {
                        this.oneSecondTicks19 = 0;
                        this.slot19CountDown = false;
                        this.didQuit19 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID19))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID19, new OutgoingMessage(19, this.multiplayerID19));
                        Game1.server.playerDisconnected(this.multiplayerID19);
                        Game1.otherFarmers.Remove(this.multiplayerID19);
                        this.knownPlayerIDs.Remove(this.multiplayerID19);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot19Used = false;
                    }
                    this.oneSecondTicks19 = 0;
                    this.slot19CountDown = false;
                }
            }
            if (this.slot20CountDown)
            {
                this.oneSecondTicks20 += 1;
                if (this.oneSecondTicks20 >= this.kickTimer)
                {
                    if (this.didQuit20)
                    {
                        this.oneSecondTicks20 = 0;
                        this.slot20CountDown = false;
                        this.didQuit20 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID20))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID20, new OutgoingMessage(19, this.multiplayerID20));
                        Game1.server.playerDisconnected(this.multiplayerID20);
                        Game1.otherFarmers.Remove(this.multiplayerID20);
                        this.knownPlayerIDs.Remove(this.multiplayerID20);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot20Used = false;
                    }
                    this.oneSecondTicks20 = 0;
                    this.slot20CountDown = false;
                }
            }
            if (this.slot21CountDown)
            {

                this.oneSecondTicks21 += 1;
                if (this.oneSecondTicks21 >= this.kickTimer)
                {
                    if (this.didQuit21)
                    {
                        this.oneSecondTicks21 = 0;
                        this.slot21CountDown = false;
                        this.didQuit21 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID21))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID21, new OutgoingMessage(19, this.multiplayerID21));
                        Game1.server.playerDisconnected(this.multiplayerID21);
                        Game1.otherFarmers.Remove(this.multiplayerID21);
                        this.knownPlayerIDs.Remove(this.multiplayerID21);

                        this.WhatToSayWhenKickAfter();

                        this.connectedFarmersCount -= 1;
                        this.slot21Used = false;
                    }
                    this.oneSecondTicks21 = 0;
                    this.slot21CountDown = false;
                }
            }

            if (this.slot22CountDown)
            {
                this.oneSecondTicks22 += 1;
                if (this.oneSecondTicks22 >= this.kickTimer)
                {
                    if (this.didQuit22)
                    {
                        this.oneSecondTicks22 = 0;
                        this.slot22CountDown = false;
                        this.didQuit22 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID22))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID22, new OutgoingMessage(19, this.multiplayerID22));
                        Game1.server.playerDisconnected(this.multiplayerID22);
                        Game1.otherFarmers.Remove(this.multiplayerID22);
                        this.knownPlayerIDs.Remove(this.multiplayerID22);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot22Used = false;
                    }
                    this.oneSecondTicks22 = 0;
                    this.slot22CountDown = false;
                }
            }

            if (this.slot23CountDown)
            {
                this.oneSecondTicks23 += 1;
                if (this.oneSecondTicks23 >= this.kickTimer)
                {
                    if (this.didQuit23)
                    {
                        this.oneSecondTicks23 = 0;
                        this.slot23CountDown = false;
                        this.didQuit23 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID23))
                    {
                        this.WhatColorToSayWhenKickBefore();
                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID23, new OutgoingMessage(19, this.multiplayerID23));
                        Game1.server.playerDisconnected(this.multiplayerID23);
                        Game1.otherFarmers.Remove(this.multiplayerID23);
                        this.knownPlayerIDs.Remove(this.multiplayerID23);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot23Used = false;
                    }
                    this.oneSecondTicks23 = 0;
                    this.slot23CountDown = false;
                }
            }

            if (this.slot24CountDown)
            {

                this.oneSecondTicks24 += 1;
                if (this.oneSecondTicks24 >= this.kickTimer)
                {
                    if (this.didQuit24)
                    {
                        this.oneSecondTicks24 = 0;
                        this.slot24CountDown = false;
                        this.didQuit24 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID24))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID24, new OutgoingMessage(19, this.multiplayerID24));
                        Game1.server.playerDisconnected(this.multiplayerID24);
                        Game1.otherFarmers.Remove(this.multiplayerID24);
                        this.knownPlayerIDs.Remove(this.multiplayerID24);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot24Used = false;
                    }
                    this.oneSecondTicks24 = 0;
                    this.slot24CountDown = false;
                }
            }

            if (this.slot25CountDown)
            {
                this.oneSecondTicks25 += 1;
                if (this.oneSecondTicks25 >= this.kickTimer)
                {
                    if (this.didQuit25)
                    {
                        this.oneSecondTicks25 = 0;
                        this.slot25CountDown = false;
                        this.didQuit25 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID25))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID25, new OutgoingMessage(19, this.multiplayerID25));
                        Game1.server.playerDisconnected(this.multiplayerID25);
                        Game1.otherFarmers.Remove(this.multiplayerID25);
                        this.knownPlayerIDs.Remove(this.multiplayerID25);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot25Used = false;
                    }
                    this.oneSecondTicks25 = 0;
                    this.slot25CountDown = false;
                }
            }

            if (this.slot26CountDown)
            {
                this.oneSecondTicks26 += 1;
                if (this.oneSecondTicks26 >= this.kickTimer)
                {
                    if (this.didQuit26)
                    {
                        this.oneSecondTicks26 = 0;
                        this.slot26CountDown = false;
                        this.didQuit26 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID26))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID26, new OutgoingMessage(19, this.multiplayerID26));
                        Game1.server.playerDisconnected(this.multiplayerID26);
                        Game1.otherFarmers.Remove(this.multiplayerID26);
                        this.knownPlayerIDs.Remove(this.multiplayerID26);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot26Used = false;
                    }
                    this.oneSecondTicks26 = 0;
                    this.slot26CountDown = false;
                }
            }
            if (this.slot27CountDown)
            {
                this.oneSecondTicks27 += 1;
                if (this.oneSecondTicks27 >= this.kickTimer)
                {
                    if (this.didQuit27)
                    {
                        this.oneSecondTicks27 = 0;
                        this.slot27CountDown = false;
                        this.didQuit27 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID27))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID27, new OutgoingMessage(19, this.multiplayerID27));
                        Game1.server.playerDisconnected(this.multiplayerID27);
                        Game1.otherFarmers.Remove(this.multiplayerID27);
                        this.knownPlayerIDs.Remove(this.multiplayerID27);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot27Used = false;
                    }
                    this.oneSecondTicks27 = 0;
                    this.slot27CountDown = false;
                }
            }
            if (this.slot28CountDown)
            {
                this.oneSecondTicks28 += 1;
                if (this.oneSecondTicks28 >= this.kickTimer)
                {
                    if (this.didQuit28)
                    {
                        this.oneSecondTicks28 = 0;
                        //slot28CountDown = false;
                        this.didQuit28 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID28))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID28, new OutgoingMessage(19, this.multiplayerID28));
                        Game1.server.playerDisconnected(this.multiplayerID28);
                        Game1.otherFarmers.Remove(this.multiplayerID28);
                        this.knownPlayerIDs.Remove(this.multiplayerID28);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot28Used = false;
                    }
                    this.oneSecondTicks28 = 0;
                    this.slot28CountDown = false;
                }
            }
            if (this.slot29CountDown)
            {
                this.oneSecondTicks29 += 1;
                if (this.oneSecondTicks29 >= this.kickTimer)
                {
                    if (this.didQuit29)
                    {
                        this.oneSecondTicks29 = 0;
                        this.slot29CountDown = false;
                        this.didQuit29 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID29))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID29, new OutgoingMessage(19, this.multiplayerID29));
                        Game1.server.playerDisconnected(this.multiplayerID29);
                        Game1.otherFarmers.Remove(this.multiplayerID29);
                        this.knownPlayerIDs.Remove(this.multiplayerID29);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot29Used = false;
                    }
                    this.oneSecondTicks29 = 0;
                    this.slot29CountDown = false;
                }
            }
            if (this.slot30CountDown)
            {
                this.oneSecondTicks30 += 1;
                if (this.oneSecondTicks30 >= this.kickTimer)
                {
                    if (this.didQuit30)
                    {
                        this.oneSecondTicks30 = 0;
                        this.slot30CountDown = false;
                        this.didQuit30 = false;
                    }
                    else if (!this.playerIDsFromMod.Contains(this.multiplayerID30))
                    {
                        this.WhatColorToSayWhenKickBefore();

                        Game1.chatBox.activate();
                        Game1.chatBox.setText("You are being kicked by AntiCheat");
                        Game1.chatBox.chatBox.RecieveCommandInput('\r');
                        Game1.server.sendMessage(this.multiplayerID30, new OutgoingMessage(19, this.multiplayerID30));
                        Game1.server.playerDisconnected(this.multiplayerID30);
                        Game1.otherFarmers.Remove(this.multiplayerID30);
                        this.knownPlayerIDs.Remove(this.multiplayerID30);

                        this.WhatToSayWhenKickAfter();
                        this.connectedFarmersCount -= 1;
                        this.slot30Used = false;
                    }
                    this.oneSecondTicks30 = 0;
                    this.slot30CountDown = false;
                }
            }
        }
    }
}
