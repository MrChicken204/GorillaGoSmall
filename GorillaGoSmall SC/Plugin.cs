using BepInEx;
using BepInEx.Configuration;
using Bepinject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using Utilla;

namespace GorillaGoSmallGorillaGoBig
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.7")]
    [BepInDependency("tonimacaroni.computerinterface", "1.5.4")]
    [Description("HauntedModMenu")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private GameObject SizeChanger;
        public static Plugin Instance { get; private set; }

        //Button
        private bool BButton;
        private bool AButton;
        private GameObject Player;
        private bool SizeDOWN;
        private float timer;
        private bool GameOpened;
        private float GameOpenedTimer;
        private bool GameLoaded;



        private bool IsTestingVersion = false;

        internal void Start()
        {
            Events.GameInitialized += OnGameInitialized;
            //ScaleChange = new ScaleChange();
            //ScaleChange.SetScale(1f, false);
            Instance = this;
            Events.GameInitialized += OnGameInitialized;

            // Inject the installer
            Zenjector.Install<UI.MainInstaller>().OnProject();
        }

        private void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            IsEnabled = true;
            if (inRoom == true)
            {
                SizeChanger.transform.localScale = new Vector3(999999, 99999, 999999);
                SizeChanger.GetComponent<SizeChanger>().minScale = Size;
                //ScaleChange.SetScale(Size.Value, true);
            }
        }

        private void OnDisable()
        {
            enabled = true;
            SizeChanger.GetComponent<SizeChanger>().minScale = 0.03f;
            timer = 0.5f;
            SizeDOWN = true;
            HarmonyPatches.RemoveHarmonyPatches();
            IsEnabled = false;
        }

        private void OnGameInitialized(object sender, EventArgs e)
        {
            Size = 0.1f;
            SizeDOWN = false;
            timer = 0.1f;
            SizeChanger = GameObject.Find("OuterTinySizer (1)");
            SizeChanger.GetComponent<Transform>().parent = null;
            Player = GameObject.Find("GorillaPlayer");
            inRoom = false;
            SizeChanger.GetComponent<SizeChanger>().affectLayerA = true;
            SizeChanger.GetComponent<SizeChanger>().minScale = 0.03f;
            GameOpened = true;
            GameOpenedTimer = 5;
        }

        private void Update()
        {
            List<InputDevice> list = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller, list);
            list[0].TryGetFeatureValue(CommonUsages.secondaryButton, out BButton);
            list[0].TryGetFeatureValue(CommonUsages.primaryButton, out AButton);
            SizeChanger.GetComponent<SizeChanger>().affectLayerA = true;

            if (Size < 0.1)
            {
                Size += 0.1f;
            }
            if (Size > 25)
            {
                Size -= 0.1f;
            }

            if (SizeDOWN == true)
            {
                timer -= Time.deltaTime;

                if (timer < 0)
                {
                    SizeChanger.transform.localScale = new Vector3(0, 0, 0);
                    SizeChanger.GetComponent<SizeChanger>().minScale = 0.03f;
                    timer = 0.1f;
                    SizeDOWN = false;
                }
            }

            if (AButton == true && BButton == false && inRoom == true)
            {
                SizeChanger.GetComponent<SizeChanger>().minScale = 0.03f;
                SizeDOWN = true;
                timer = 0.5f;
            }

            if (AButton == false && BButton == true && inRoom == true)
            {
                SizeChanger.GetComponent<SizeChanger>().affectLayerA = true;
                SizeDOWN = false;
                timer = 0.1f;
                SizeChanger.transform.localScale = new Vector3(99999, 999999, 999999);
                SizeChanger.GetComponent<SizeChanger>().minScale = Size;
            }

            if (AButton == true && BButton == false && IsTestingVersion == true)
            {
                SizeChanger.GetComponent<SizeChanger>().minScale = 0.03f;
                SizeDOWN = true;
                timer = 0.5f;
            }

            if (AButton == false && BButton == true && IsTestingVersion == true)
            {
                SizeChanger.GetComponent<SizeChanger>().affectLayerA = true;
                SizeDOWN = false;
                timer = 0.1f;
                SizeChanger.transform.localScale = new Vector3(99999, 999999, 999999);
                SizeChanger.GetComponent<SizeChanger>().minScale = Size;
            }

            if (GameOpened == true)
            {
                GameOpenedTimer -= Time.deltaTime;

                if (GameOpenedTimer < 0)
                {
                    SizeChanger.SetActive(true);
                    SizeChanger.GetComponent<SizeChanger>().minScale = 0.03f;
                    timer = 0.1f;
                    SizeDOWN = true;
                    GameLoaded = true;
                    GameOpenedTimer = 1;
                    GameOpened = false;
                }
            }
        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            if (IsEnabled == true)
            {
                //ScaleChange.SetScale(Size.Value, true);
                SizeChanger.GetComponent<SizeChanger>().minScale = 0.03f;
                SizeDOWN = true;
                timer = 0.5f;
                SizeChanger.SetActive(true);
                inRoom = true;
            }
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            inRoom = false;
            SizeChanger.GetComponent<SizeChanger>().minScale = 0.03f;
            SizeDOWN = true;
            SizeChanger.GetComponent<SizeChanger>().affectLayerA = false;
            timer = 0.1f;
            //timer = 0.1f;
            //SizeDOWN = true;
        }

        public bool IsEnabled;

        public bool inRoom = false;

        public static float Size = 0.1f;
    }
}
