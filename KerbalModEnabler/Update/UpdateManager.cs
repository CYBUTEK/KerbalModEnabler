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

using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace KerbalModEnabler.Update
{
    public class UpdateManager
    {
        public static async void CheckForUpdatesAsync(bool showWarnings = true)
        {
            var downloader = new UpdateDownloader();
            var updateData = await downloader.GetUpdateDataAsync(Config.Current.UpdateDataUrl);

            if (updateData != null)
            {
                if (updateData.IsNewer)
                {
                    if (MessageBox.Show($"Latest Version: {updateData.Version}\n\nWould you like to install this update?", "Update Available", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        if (await downloader.DownloadUpdateAsync(updateData))
                        {
                            MessageBox.Show("Kerbal Mod Enabler will now restart.", "Installing Update", MessageBoxButton.OK, MessageBoxImage.Information);
                            Config.Current.IsUpdatedRun = true;
                            ReloadApplication(updateData.FileName);
                        }
                        else
                        {
                            MessageBox.Show("There was a problem downloading the update.", "Kerbal Mod Enabler", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
                else if (showWarnings)
                {
                    MessageBox.Show("There are currently no updates available.", "Kerbal Mod Enabler", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else if (showWarnings)
            {
                MessageBox.Show("Could not retrieve update information.", "Connection Problem", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public static void ReloadApplication(string updateFileName)
        {
            var cmdResourceName = Globals.Assembly.GetManifestResourceNames().FirstOrDefault(r => r.EndsWith("UpdateReloader.cmd"));
            if (cmdResourceName != null)
            {
                var cmdFile = $"{Globals.AssemblyDirectory}/{Globals.AssemblyName}_UpdateHelper.cmd";

                try
                {
                    using (var stream = Globals.Assembly.GetManifestResourceStream(cmdResourceName))
                    using (var reader = new StreamReader(stream))
                    using (var writer = new StreamWriter(cmdFile))
                    {
                        writer.Write(reader.ReadToEnd());
                    }

                    Process.Start(cmdFile, $"\"{updateFileName}\" \"{Globals.AssemblyFileName}\"");
                    Application.Current.Shutdown();
                }
                catch
                {
                    // exception occured whilst trying to reload the application
                }
            }
        }

        public static void ShowUpdateOnStartupCheck()
        {
            var result = MessageBox.Show(
                "Check for updates on startup?",
                "Kerbal Mod Enabler",
                MessageBoxButton.YesNo,
                MessageBoxImage.Information);

            Config.Current.CheckForUpdatesOnStartup = result == MessageBoxResult.Yes;
        }
    }
}