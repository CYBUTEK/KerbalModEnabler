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

using System.Collections.Generic;
using System.IO;

namespace KerbalModEnabler
{
    public class Package
    {
        public Package(string archive)
        {
            ArchiveFilePath = archive;
        }

        public string ArchiveFileName
        {
            get => Path.GetFileName(ArchiveFilePath);
        }

        public string ArchiveFileNameWithoutExtension
        {
            get => Path.GetFileNameWithoutExtension(ArchiveFilePath);
        }

        public string ArchiveFilePath { get; set; }

        public List<string> ExtractedFiles { get; set; }

        public List<string> VersionFiles { get; set; }

        public override string ToString()
        {
            return ArchiveFileNameWithoutExtension;
        }
    }
}