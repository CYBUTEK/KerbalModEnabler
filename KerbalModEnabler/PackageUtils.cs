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

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace KerbalModEnabler
{
    public static class PackageUtils
    {
        public static void Install(Package package)
        {
            if (TryOpenArchive(package, out ZipArchive archive))
            {
                package.ExtractedFiles = new List<string>();
                package.VersionFiles = new List<string>();

                if (ArchiveContainsGameData(package))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (entry.FullName.StartsWith("GameData"))
                        {
                            ExtractEntry(package, entry);
                        }
                    }
                }
                else
                {
                    foreach (var entry in archive.Entries)
                    {
                        ExtractEntry(package, entry);
                    }
                }
            }
        }

        public static void Uninstall(Package package)
        {
            foreach (var file in package.ExtractedFiles)
            {
                var directory = Path.GetDirectoryName(file);

                if (directory != string.Empty && Directory.Exists($"{Config.Current.GameDataDirectory}/{directory}"))
                {
                    try
                    {
                        Directory.Delete($"{Config.Current.GameDataDirectory}/{directory}", true);
                        Debug.Log($"Deleted Directory: '{directory}'");
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(ex);
                    }
                }
                else
                {
                    var filePath = $"{Config.Current.GameDataDirectory}/{file}";
                    if (File.Exists(filePath))
                    {
                        try
                        {
                            File.Delete(filePath);
                            Debug.Log($"Deleted File: '{file}'");
                        }
                        catch (Exception ex)
                        {
                            Debug.LogException(ex);
                        }
                    }
                }
            }
        }

        private static bool ArchiveContainsGameData(Package package)
        {
            if (TryOpenArchive(package, out ZipArchive archive))
            {
                foreach (var entry in archive.Entries)
                {
                    if (Path.GetDirectoryName(entry.FullName) == "GameData")
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static void ExtractEntry(Package package, ZipArchiveEntry entry)
        {
            var fileName = entry.FullName.Replace("GameData/", string.Empty);
            var destinationFilePath = $"{Config.Current.GameDataDirectory}/{fileName}";
            var destinationDirectoryName = Path.GetDirectoryName(destinationFilePath);

            try
            {
                Directory.CreateDirectory(destinationDirectoryName);

                if (Path.GetFileName(fileName) != string.Empty)
                {
                    entry.ExtractToFile(destinationFilePath, true);
                    package.ExtractedFiles.Add(fileName);

                    if (Path.GetExtension(fileName).ToLower() == ".version")
                    {
                        package.VersionFiles.Add(fileName);
                    }

                    Debug.Log($"Extracted File: '{fileName}'");
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private static bool TryOpenArchive(Package package, out ZipArchive archive)
        {
            archive = null;

            try
            {
                archive = ZipFile.OpenRead(package.ArchiveFilePath);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            return archive != null;
        }
    }
}