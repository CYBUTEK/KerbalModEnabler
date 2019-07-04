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

namespace KerbalModEnabler
{
    public class ConfigData
    {
        ~ConfigData()
        {
            Config.Save();
        }

        public bool CheckForUpdatesOnStartup { get; set; } = false;

        public string GameDataDirectory { get; set; } = $"{Globals.AssemblyDirectory}/GameData";

        public bool IsFirstRun { get; set; } = true;

        public bool IsUpdatedRun { get; set; } = false;

        public string PackagesDirectory { get; set; } = $"{Globals.DataDirectory}/Packages";

        public string UpdateDataUrl { get; set; } = "https://raw.githubusercontent.com/CYBUTEK/KerbalModEnabler/master/Updates/CurrentVersion.json";
    }
}