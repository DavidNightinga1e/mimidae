using System;
using System.Timers;
using System.Windows;
using Microsoft.Win32;
using Mimidae.Models;

namespace Mimidae
{
    public partial class MainWindow
    {
        private readonly Client _client;

        public MainWindow()
        {
            InitializeComponent();
            _client = new Client();
            _client.Start();

            var timer = new Timer(1000);
            timer.Elapsed += OnTimerElapsed;
            timer.Start();

            TorrentsListBox.ItemsSource = _client.TorrentModelsBindingList;

            Closed += OnClosed;
        }

        private async void OnClosed(object sender, EventArgs e)
        {
            await _client.Stop();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _client.UpdateProgress();
        }

        private async void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                CheckFileExists = true,
                Title = "Open Torrent File",
                DefaultExt = "torrent"
            };
            var showDialog = dialog.ShowDialog(this);
            if (showDialog is true)
            {
                await _client.AddFile(dialog.FileName);
            }
        }

        private async void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItems = TorrentsListBox.SelectedItems;
            if (selectedItems.Count is 1)
                await _client.StartTorrent((TorrentModel) selectedItems[0]);
        }

        private async void PauseButton_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItems = TorrentsListBox.SelectedItems;
            if (selectedItems.Count is 1)
                await _client.PauseTorrent((TorrentModel) selectedItems[0]);
        }

        private async void StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItems = TorrentsListBox.SelectedItems;
            if (selectedItems.Count is 1)
                await _client.StopTorrent((TorrentModel) selectedItems[0]);
        }

        private async void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItems = TorrentsListBox.SelectedItems;
            if (selectedItems.Count is 1)
            {
                var item = (TorrentModel) selectedItems[0];
                await _client.Delete(item);
            }
        }
    }
}