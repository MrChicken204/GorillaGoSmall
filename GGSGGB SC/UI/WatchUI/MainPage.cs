using UnityEngine;
using GorillaGoSmallGorillaGoBig.main;
using MonkeStatistics.API;

namespace GorillaGoSmallGorillaGoBig.Scripts.mainmenu
{
    [DisplayInMainMenu("GorillaGoSmall")]
    internal class MainPage : Page
    {
        //public Plugin Instance { get; private set; }

        public override void OnPageOpen()
        {
            //Instance = new Plugin();
            base.OnPageOpen();
            SetTitle("GorillaGoSmall");
            SetAuthor("By MrBanana");
            SetBackButtonOverride(typeof(MonkeStatistics.Core.Pages.MainPage));
            if (!Plugin.Instance.inRoom)
            {
                AddLine(2);
                AddLine("MOD UNABLE");
                AddLine("TO WORK");
                AddLine("YOU MUST BE IN");
                AddLine("A MODDED LOBBY");
                AddLine("TO USE");
                AddLine("THIS PLUGIN!");
                SetLines();
                return;
            }
            // In Modded room 
            Build();
            SetLines();
        }

        private void Build()
        {
            AddLine("SIZE[+]", new ButtonInfo(OnSizeHChange, 1, ButtonInfo.ButtonType.Press));
            AddLine("SIZE[-]", new ButtonInfo(OnSizeLChange, -1, ButtonInfo.ButtonType.Press));
            AddLine(1);
            AddLine("[ENABLED]", new ButtonInfo(OnEnablePress, 0, ButtonInfo.ButtonType.Toggle, Plugin.Instance.Enabled));
            AddLine(1);
            AddLine("Output:");
            AddLine("SIZE:" + Plugin.Instance.Size);
        }
        public void OnEnablePress(object Sender, object[] Args)
        {
            Plugin.Instance.Enabled = !Plugin.Instance.Enabled;
        }
        public void OnSizeHChange(object Sender, object[] Args)
        {
            Plugin.Instance.Size += (int)Args[0];
            Plugin.Instance.UpdateHSize();

            TextLines = new Line[0];
            Build();
            UpdateLines();
        }
        public void OnSizeLChange(object Sender, object[] Args)
        {
            Plugin.Instance.Size += (int)Args[0];
            Plugin.Instance.UpdateLSize();

            TextLines = new Line[0];
            Build();
            UpdateLines();
        }
    }
}
