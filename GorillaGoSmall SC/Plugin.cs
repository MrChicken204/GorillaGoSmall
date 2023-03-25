using System;
using System.ComponentModel;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using Utilla;
using GorillaGoSmallGorillaGoBig.Scripts;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR;

namespace GorillaGoSmallGorillaGoBig
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.7")]
    [Description("HauntedModMenu")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private GameObject SizeChanger;
        public ScaleChange ScaleChange { get; private set; }
        public static Plugin Instance { get; private set; }

        //Button
        private bool BButton;
        private bool AButton;
        private GameObject Player;
        private bool SizeDown;
        private float timer;

        internal void Start()
        {
            Events.GameInitialized += this.OnGameInitialized;
            //ScaleChange = new ScaleChange();
            //ScaleChange.SetScale(1f, false);
            Instance = this;
            Events.GameInitialized += OnGameInitialized;
            ScaleChange.SetScale(1f, false);
        }

        private void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            IsEnabled = true;
            if (inRoom == true)
            {
                SizeChanger.GetComponent<Transform>().transform.localScale = new Vector3(9999, 9999, 9999);
                SizeChanger.GetComponent<SizeChanger>().minScale = Size.Value;
                //ScaleChange.SetScale(Size.Value, true);
            }
            else
            {
                SizeChanger.GetComponent<SizeChanger>().minScale = 0.03f;
                timer = 0.1f;
                SizeDown = true;
                //ScaleChange.SetScale(1f, false);
            }
        }

        private void OnDisable()
        {
            this.enabled = true;
            SizeChanger.GetComponent<SizeChanger>().minScale = 0.03f;
            SizeChanger.GetComponent<Transform>().transform.localScale = new Vector3(17.906f, 1.6123f, 18.0854f);
            timer = 0.1f;
            SizeDown = true;
            HarmonyPatches.RemoveHarmonyPatches();
            IsEnabled = false;
        }

        private void OnGameInitialized(object sender, EventArgs e)
        {   //Hopefully Fixed
            SizeDown = false;
            timer = 0.1f;
            SizeChanger = GameObject.Find("tiny sizer (1)");
            SizeChanger.GetComponent<Transform>().parent = null;
            Player = GameObject.Find("GorillaPlayer");
            var SizeChangeFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "GorillaGoSmall.cfg"), true);
            Size = SizeChangeFile.Bind("Configuration", "Size", 0.1f, "What size do you want to be?, 0.1 is the normal small scale");
            inRoom = false;
            SizeChanger.GetComponent<Transform>().transform.localScale = new Vector3(17.906f, 1.6123f, 18.0854f);
            SizeChanger.GetComponent<SizeChanger>().minScale = 0.03f;
        }

        private void Update()
        {
            List<InputDevice> list = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller, list);
            list[0].TryGetFeatureValue(CommonUsages.secondaryButton, out BButton);
            list[0].TryGetFeatureValue(CommonUsages.primaryButton, out AButton);

            if(AButton == true && BButton == false && inRoom == true)
            {
                SizeChanger.GetComponent<SizeChanger>().minScale = 0.03f;
                timer = 0.1f;
                SizeDown = true;
            }

            if (AButton == false && BButton == true && inRoom == true)
            {
                SizeChanger.GetComponent<Transform>().transform.localScale = new Vector3(9999, 9999, 9999);
                SizeChanger.GetComponent<SizeChanger>().minScale = Size.Value;
            }

            if(SizeDown == true)
            {
                timer -= Time.deltaTime;

                if(timer < 0)
                {
                    timer = 0.1f;
                    SizeChanger.GetComponent<Transform>().transform.localScale = new Vector3(17.906f, 1.6123f, 18.0854f);
                    SizeDown = false;
                }
            }
        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            if(IsEnabled == true)
            {
                //ScaleChange.SetScale(Size.Value, true);
                SizeChanger.GetComponent<Transform>().transform.localScale = new Vector3(9999, 9999, 9999);
                SizeChanger.GetComponent<SizeChanger>().minScale = Size.Value;
                inRoom = true;
            }
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            inRoom = false;
            SizeChanger.GetComponent<SizeChanger>().minScale = 0.03f;
            timer = 0.1f;
            SizeDown = true;
            ScaleChange.SetScale(1f, false);
        }

        public bool IsEnabled;

        public bool inRoom = false;

        public static ConfigEntry<float> Size;
    }
}
