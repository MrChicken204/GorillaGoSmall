using BepInEx;
using System;
using UnityEngine;
using Utilla;
using HoneyLib.Utils;
using MonkeStatistics.API;
using HarmonyLib;
using System.Reflection;
using System.Collections;
using Technie.PhysicsCreator;

namespace GorillaGoSmallGorillaGoBig.main
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.7")]
    [BepInDependency("com.buzzbzzzbzzbzzzthe18th.gorillatag.HoneyLib")]
    [BepInDependency("Crafterbot.MonkeStatistics")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public GameObject SizeChanger;
        public static Plugin Instance { get; private set; }
        private GameObject Player;

        internal void Start()
        {
            //LoadGame
            Events.GameInitialized += OnGameInitialized;
            Instance = this;
        }

        private void Awake()
        {
            Logger.LogInfo("Init : " + PluginInfo.Name);
            Registry.AddAssembly();
            new Harmony(PluginInfo.GUID).PatchAll(Assembly.GetExecutingAssembly());
        }

        private void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
        }

        private void OnDisable()
        {
            enabled = true;
            HarmonyPatches.RemoveHarmonyPatches();
        }

        private void OnGameInitialized(object sender, EventArgs e)
        {
            //Change the size to be the normal value
            Size = 0.1f;

            //Get Gameobjects
            SizeChanger = GameObject.Find("OuterTinySizer (1)");
            Player = GameObject.Find("GorillaPlayer");
            SizeChanger.GetComponent<Transform>().parent = null;
            
            //Room/Size Settings
            inRoom = false;
            SizeChanger.GetComponent<SizeChanger>().affectLayerA = true;
            SizeChanger.GetComponent<SizeChanger>().minScale = 0.03f;
        }

        public void UpdateHSize()
        {
            Instance.Size -= 0.9f;
        }
        public void UpdateLSize()
        {
            Instance.Size += 0.9f;
        }

        private void FixedUpdate()
        {
            //Utils
            SizeChanger.SetActive(true);

            EasyInput.UpdateInput();

            //If size is to low, make it higher.
            if (Size < 0.1)
            {
                Size += 0.1f;
            }
            //If size is to high make it lower.
            if (Size > 25)
            {
                Size -= 0.1f;
            }

            //Check if the player pressed A, if so then make the player size normal
            if (EasyInput.FaceButtonB && Enabled && inRoom)
            {
                Instance.SetScale(true);
            }

            //Check if the player pressed B, if so then make the player size whatever is was set to.
            if (EasyInput.FaceButtonA && Enabled && inRoom)
            {
                Instance.SetScale(false);
            }
        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            inRoom = true;
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            Instance.SetScale(true);
            Enabled = false;
            inRoom = false;
        }
        public bool inRoom;

        public float Size = 0.1f;

        public bool Enabled;

        //ScaleChange
        public void SetScale(bool Normal)
        {
            if (Normal == true)
            {
                SizeChanger.GetComponent<SizeChanger>().minScale = 0.9f;
                BecomeNormal();
            }
            if(Normal == false)
            {
                Debug.Log("ChangedSize");
                SizeChanger.transform.localScale = new Vector3(99999, 999999, 999999);
                SizeChanger.GetComponent<SizeChanger>().minScale = Size;
            }
        }

        private void BecomeNormal()
        {
            SizeChanger.transform.localScale = new Vector3(0, 0, 0);
            SizeChanger.GetComponent<SizeChanger>().minScale = 0.03f;
            Debug.Log("BecameNormalSize");
        }
    }
}