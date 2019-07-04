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

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace KerbalModEnabler
{
    public static class PackageManager
    {
        private static readonly string installedPackagesJsonPath = $"{Globals.DataDirectory}/Installed.json";
        private static List<Package> installedPackages;

        public static bool AddPackage(Package package)
        {
            var source = package.ArchiveFilePath;
            var target = $"{Config.Current.PackagesDirectory}/{package.ArchiveFileName}";

            if (File.Exists(source) && !File.Exists(target))
            {
                File.Copy(source, target, true);
                package.ArchiveFilePath = target;

                return true;
            }

            return false;
        }

        public static List<Package> GetAvailablePackages()
        {
            var packages = new List<Package>();

            foreach (var archive in Directory.GetFiles(Config.Current.PackagesDirectory, "*.zip"))
            {
                if (installedPackages.Find(p => p.ArchiveFilePath == archive) == null)
                {
                    packages.Add(new Package(archive));
                }
            }

            return packages;
        }

        public static List<Package> GetInstalledPackages()
        {
            if (installedPackages == null)
            {
                if (File.Exists(installedPackagesJsonPath))
                {
                    using (var stream = File.Open(installedPackagesJsonPath, FileMode.OpenOrCreate))
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        var json = reader.ReadToEnd();
                        installedPackages = JsonConvert.DeserializeObject<List<Package>>(json) ?? new List<Package>();
                    }
                }
                else
                {
                    installedPackages = new List<Package>();
                }
            }

            return installedPackages;
        }

        public static void InstallPackages(IList packages)
        {
            foreach (Package package in packages)
            {
                installedPackages.Add(package);
                PackageUtils.Install(package);
            }

            SaveInstalledPackages();
        }

        public static void RemovePackage(Package package)
        {
            if (File.Exists(package.ArchiveFilePath))
            {
                File.Delete(package.ArchiveFilePath);
            }
        }

        public static void SaveInstalledPackages()
        {
            using (var stream = File.Open(installedPackagesJsonPath, FileMode.Create))
            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                var json = JsonConvert.SerializeObject(GetInstalledPackages(), Formatting.Indented);
                writer.Write(json);
            }
        }

        public static void UninstallPackages(IList packages)
        {
            foreach (Package package in packages)
            {
                PackageUtils.Uninstall(package);
                installedPackages.Remove(package);
            }

            SaveInstalledPackages();
        }
    }
}