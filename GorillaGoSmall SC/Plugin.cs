using System;
using System.ComponentModel;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using GorillaLocomotion;
using Utilla;

namespace GorillaGoSmallGorillaGoBig
{
    // Token: 0x02000003 RID: 3
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.6.7")]
    [Description("HauntedModMenu")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private void Start()
        {
            Events.GameInitialized += this.OnGameInitialized;
            SetScale(1f, false);
        }

        public void SetScale(float scale, bool isSmall)
        {
            Player.Instance.TryGetComponent<SizeManager>(out SizeManager sizeManager);
            if (isSmall)
            {
                sizeManager.enabled = false;
                Player.Instance.scale = scale;
            }
            else
            {
                sizeManager.enabled = true;
            }
        }

        private void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            IsEnabled = true;
            if (inRoom == true)
            {
                SetScale(Size.Value, true);
            }
            else
            {
                SetScale(1f, false);
            }
        }

        private void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
            IsEnabled = false;
            SetScale(1f, false);
        }

        private void OnGameInitialized(object sender, EventArgs e)
        {
            SetScale(1f, false);
            var SizeChangeFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "GorillaGoSmall.cfg"), true);
            Size = SizeChangeFile.Bind("Configuration", "Size", 0.1f, "What size do you want to be?, 0.1 is the normal small scale");
        }

        private void Update()
        {
        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            inRoom = true;
            SetScale(Size.Value, true);
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            inRoom = false;
            SetScale(1f, false);
        }

        public bool IsEnabled;

        public bool inRoom = false;

        public static ConfigEntry<float> Size;
    }
}
