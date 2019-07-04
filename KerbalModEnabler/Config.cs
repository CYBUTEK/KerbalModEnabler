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
using Newtonsoft.Json;

namespace KerbalModEnabler
{
    public class Config
    {
        private static readonly string settingsFilePath = $"{Globals.DataDirectory}/Config.json";
        private static ConfigData current;

        static Config()
        {
            Load();
        }

        public static ConfigData Current
        {
            get
            {
                if (current == null)
                {
                    current = Load() ?? new ConfigData();
                }
                return current;
            }
        }

        public static ConfigData Load()
        {
            if (File.Exists(settingsFilePath))
            {
                using (var stream = File.OpenRead(settingsFilePath))
                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<ConfigData>(json);
                }
            }

            return null;
        }

        public static void Save()
        {
            using (var stream = File.Create(settingsFilePath))
            using (var writer = new StreamWriter(stream))
            {
                var json = JsonConvert.SerializeObject(Current, Formatting.Indented);
                writer.Write(json);
            }
        }
    }
}