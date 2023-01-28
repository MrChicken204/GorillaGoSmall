using BepInEx;
using System;
using System.ComponentModel;
using UnityEngine;
using GorillaLocomotion;
using Utilla;

namespace GorillaGoSmallGorillaGoBig
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.7")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    [Description("HauntedModMenu")]
    public class Plugin : BaseUnityPlugin
    {
        public bool IsEnabled;
        public bool inRoom = false;

        void Start()
        {
            /* A lot of Gorilla Tag systems will not be set up when start is called /*
			/* Put code in OnGameInitialized to avoid null references */

            Utilla.Events.GameInitialized += OnGameInitialized;
            SetScale(1, false);
        }

        public void SetScale(float scale, bool isSmall) //Code made by dev and is from my other mod "Gorilla's Doom" Thanks dev for helping!
        {
            Player.Instance.TryGetComponent(out SizeManager sizeManager);
            if (isSmall)
            {
                sizeManager.enabled = false;
                Player.Instance.scale = scale;
                return;
            }

            sizeManager.enabled = true;
        }

        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
            IsEnabled = true;

            if(inRoom == true)
            {
                SetScale(0.1f, true);
            }
            else
            {
                SetScale(1, false);
            }
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

            HarmonyPatches.RemoveHarmonyPatches();
            IsEnabled = false;
            SetScale(1, false);
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            SetScale(1, false);
            /* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */
        }

        void Update()
        {
            /* Code here runs every frame when the mod is enabled */
        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = true;
            SetScale(0.1f, true);
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/
            inRoom = false;
            SetScale(1, false);
        }
    }
}
