/*
    Kerbal Mod Enabler handles quick and easy installation of mods.
    Copyright (C) 2019  CYBUTEK

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System.IO;
using System.Threading;
using System.Windows;
using KerbalModEnabler.Update;

namespace KerbalModEnabler
{
    public partial class App : Application
    {
        private Mutex mutex;

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if (mutex != null)
            {
                mutex.ReleaseMutex();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            mutex = new Mutex(true, Globals.AssemblyName, out bool createdNewMutex);

            if (createdNewMutex)
            {
                Directory.CreateDirectory(Globals.DataDirectory);
                Directory.CreateDirectory(Config.Current.PackagesDirectory);

                Debug.Log($"{Globals.AssemblyName} - v{Globals.AssemblyVersion}");

                if (Config.Current.IsFirstRun)
                {
                    Config.Current.IsFirstRun = false;
                    Debug.Log("IsFirstRun: True");
                    UpdateManager.ShowUpdateOnStartupCheck();
                }

                if (Config.Current.IsUpdatedRun)
                {
                    Config.Current.IsUpdatedRun = false;
                    Debug.Log("IsUpdatedRun: True");
                }

                if (Config.Current.CheckForUpdatesOnStartup)
                {
                    UpdateManager.CheckForUpdatesAsync(false);
                }
            }
            else
            {
                MessageBox.Show("Kerbal Mod Enabler is already running.", "Kerbal Mod Enabler", MessageBoxButton.OK, MessageBoxImage.Information);
                Shutdown();
            }
        }
    }
}