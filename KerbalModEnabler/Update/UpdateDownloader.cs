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
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KerbalModEnabler.Update
{
    public class UpdateDownloader
    {
        public event Action<float> DownloadProgressChanged;

        public async Task<bool> DownloadUpdateAsync(UpdateData updateInfo)
        {
            var client = new WebClient();
            client.DownloadProgressChanged += (s, e) =>
            {
                DownloadProgressChanged?.Invoke(e.BytesReceived / e.TotalBytesToReceive);
            };

            try
            {
                var fileName = $"{Globals.AssemblyFileName}.update";
                await client.DownloadFileTaskAsync(updateInfo.Download, $"{Globals.AssemblyDirectory}/{fileName}");
                updateInfo.FileName = fileName;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UpdateData> GetUpdateDataAsync(string address)
        {
            try
            {
                var json = await DownloadStringAsync(address);

                if (json != null)
                {
                    return JsonConvert.DeserializeObject<UpdateData>(json);
                }
            }
            catch { }

            return null;
        }

        private async Task<string> DownloadStringAsync(string address)
        {
            return await new WebClient().DownloadStringTaskAsync(address);
        }
    }
}