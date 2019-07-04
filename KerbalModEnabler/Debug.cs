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
using System.IO;

namespace KerbalModEnabler
{
    public static class Debug
    {
        private static readonly string logFilePath = $"{Globals.DataDirectory}/Log.txt";
        private static StreamWriter writer;

        static Debug()
        {
            writer = new StreamWriter(File.Open(logFilePath, FileMode.Create, FileAccess.Write, FileShare.Read));
        }

        public static void Log(string message, string type = "log")
        {
            lock (writer)
            {
                writer.WriteLine($"[{type.ToUpper()} {DateTime.Now.ToLongTimeString()}]: {message}");
                writer.Flush();
            }
        }

        public static void LogException(Exception ex)
        {
            Log(ex.ToString(), "EXC");
        }
    }
}