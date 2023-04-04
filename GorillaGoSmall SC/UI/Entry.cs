using ComputerInterface.Interfaces;
using System;
using Zenject;

namespace GorillaGoSmallGorillaGoBig.UI
{
    internal class Entry : IComputerModEntry
    {
        public string EntryName => PluginInfo.Name;
        public Type EntryViewType => typeof(Views.MainView);
    }

    // The installer class binds the entry to the computer interface mod.
    internal class MainInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<IComputerModEntry>().To<Entry>().AsSingle();
        }
    }
}