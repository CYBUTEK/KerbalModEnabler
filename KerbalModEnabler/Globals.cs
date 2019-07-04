﻿/*
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
using System.IO;
using System.Reflection;

namespace KerbalModEnabler
{
    public static class Globals
    {
        public static Assembly Assembly
        {
            get => Assembly.GetExecutingAssembly();
        }

        public static string AssemblyDirectory
        {
            get => Path.GetDirectoryName(AssemblyLocation);
        }

        public static string AssemblyFileName
        {
            get => Path.GetFileName(AssemblyLocation);
        }

        public static string AssemblyLocation
        {
            get => Assembly.Location;
        }

        public static string AssemblyName
        {
            get => Assembly.GetName().Name;
        }

        public static Version AssemblyVersion
        {
            get => Assembly.GetName().Version;
        }

        public static string DataDirectory
        {
            get => $"{AssemblyDirectory}/{AssemblyName}_Data";
        }
    }
}