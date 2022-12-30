using BepInEx;
using System;
using UnityEngine;
using Utilla;

namespace GorillaGoSmallGorillaGoBig
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.7")]
    [BepInDependency("tonimacaroni.computerinterface", "1.5.4")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public bool IsEnabled;
        public bool inRoom = true;
        public GameObject SizeChanger;
        public Transform Parent;

        void Start()
        {
            /* A lot of Gorilla Tag systems will not be set up when start is called /*
			/* Put code in OnGameInitialized to avoid null references */

            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
            IsEnabled = true;
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

            HarmonyPatches.RemoveHarmonyPatches();
            SizeChanger.transform.localScale = new Vector3(0.3422f, 0.3422f, 0.3422f);
            IsEnabled = false;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            /* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */

            GameObject.CreatePrimitive(PrimitiveType.Cube).name = "SizeChangerModdedLobby";
            Parent = transform.Find("SizeChangerModdedLobby");
            SizeChanger = GameObject.Find("SnowGlobeTriggerVolume");

            SizeChanger.transform.parent = Parent;
            SizeChanger.transform.localScale = new Vector3(0.3422f, 0.3422f, 0.3422f);
        }

        void Update()
        {
            /* Code here runs every frame when the mod is enabled */

            if(IsEnabled == false)
            {
                SizeChanger.transform.localScale = new Vector3(0.3422f, 0.3422f, 0.3422f);
            }

            if(inRoom == true)
            {
                SizeChanger.transform.localScale = new Vector3(999, 999, 999);
            }

            if (inRoom == false)
            {
                SizeChanger.transform.localScale = new Vector3(0.3422f, 0.3422f, 0.3422f);
            }
        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = true;
            SizeChanger.GetComponent<SizeChanger>().minScale = 0.1f;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            SizeChanger.transform.localScale = new Vector3(0.3422f, 0.3422f, 0.3422f);
            inRoom = false;
            SizeChanger.GetComponent<SizeChanger>().minScale = 0.03f;
        }
    }
}
