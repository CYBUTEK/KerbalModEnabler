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
using System.Windows;
using System.Windows.Controls;
using KerbalModEnabler.Update;

namespace KerbalModEnabler
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RefreshAllPackages();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            PackageManager.InstallPackages(listAvailable.SelectedItems);
            RefreshAllPackages();
        }

        private void ListAvailable_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (var file in files)
                {
                    if (Path.GetExtension(file).ToLower() == ".zip")
                    {
                        if (PackageManager.AddPackage(new Package(file)))
                        {
                            RefreshAvailablePackages();
                        }
                    }
                }
            }
        }

        private void ListAvailable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItemNotNull = listAvailable.SelectedItem != null;

            InstallButton.IsEnabled = selectedItemNotNull;
            RemovePackageButton.IsEnabled = selectedItemNotNull;

            if (selectedItemNotNull)
            {
                listInstalled.SelectedItems.Clear();
            }
        }

        private void ListInstalled_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItemNotNull = listInstalled.SelectedItem != null;

            UninstallButton.IsEnabled = selectedItemNotNull;

            if (selectedItemNotNull)
            {
                listAvailable.SelectedItems.Clear();
            }
        }

        private void RefreshAllPackages()
        {
            RefreshInstalledPackages();
            RefreshAvailablePackages();
        }

        private void RefreshAvailablePackages()
        {
            listAvailable.Items.Clear();

            foreach (var package in PackageManager.GetAvailablePackages())
            {
                listAvailable.Items.Add(package);
            }
        }

        private void RefreshInstalledPackages()
        {
            listInstalled.Items.Clear();

            foreach (var package in PackageManager.GetInstalledPackages())
            {
                listInstalled.Items.Add(package);
            }
        }

        private void RemovePackageButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Package package in listAvailable.SelectedItems)
            {
                if (MessageBox.Show($"Delete \"{package.ArchiveFileName}\"?", "Remove Package", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    PackageManager.RemovePackage(package);
                }
            }

            RefreshAvailablePackages();
        }

        private void UninstallButton_Click(object sender, RoutedEventArgs e)
        {
            PackageManager.UninstallPackages(listInstalled.SelectedItems);
            RefreshAllPackages();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            UpdateManager.CheckForUpdatesAsync();
        }
    }
}