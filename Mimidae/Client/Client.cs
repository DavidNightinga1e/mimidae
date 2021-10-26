using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Mimidae.Models;
using MonoTorrent.Client;

namespace Mimidae
{
    public class Client
    {
        private string PathToDownloads
        {
            get
            {
                var userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                var pathToDownloads = Path.Combine(userProfilePath, "Downloads");
                if (!Directory.Exists(pathToDownloads))
                    Directory.CreateDirectory(pathToDownloads);
                return pathToDownloads;
            }
        }

        private string PathToApplicationFolder
        {
            get
            {
                var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var pathToApp = Path.Combine(localAppData, "Mimidae");
                if (!Directory.Exists(pathToApp))
                    Directory.CreateDirectory(pathToApp);
                return pathToApp;
            }
        }

        private string PathToStateFile => Path.Combine(PathToApplicationFolder, "state");

        private readonly BindingList<TorrentModel> _torrentListItems = new BindingList<TorrentModel>();
        private readonly List<TorrentManager> _torrentManagers = new List<TorrentManager>();

        public BindingList<TorrentModel> TorrentModelsBindingList => _torrentListItems;

        private ClientEngine _clientEngine;

        private TorrentManager GetManagerByItem(TorrentModel item)
        {
            var id = _torrentListItems.IndexOf(item);
            return _torrentManagers[id];
        }

        public async void Start()
        {
            // if (File.Exists(PathToStateFile))
            // {
            //     _clientEngine = await ClientEngine.RestoreStateAsync(PathToStateFile);
            //     var torrentManagers = _clientEngine.Torrents;
            //     foreach (var manager in torrentManagers)
            //         await AddManager(manager);
            // }

            if (_clientEngine is null)
                _clientEngine = new ClientEngine();
        }

        public void UpdateProgress()
        {
            for (int i = 0; i < _torrentManagers.Count; i++)
            {
                var torrentManager = _torrentManagers[i];
                var torrentListItem = _torrentListItems[i];
                torrentListItem.Progress = (float) torrentManager.Progress;
                torrentListItem.TorrentState = torrentManager.State;
            }
        }

        public async Task AddFile(string path)
        {
            var torrentManager = await _clientEngine.AddStreamingAsync(path, PathToDownloads);
            await torrentManager.StartAsync();
            _torrentManagers.Add(torrentManager);
            var torrentListItem = new TorrentModel(torrentManager.ToString());
            _torrentListItems.Add(torrentListItem);
        }

        public async Task AddManager(TorrentManager torrentManager)
        {
            await torrentManager.StartAsync();
            _torrentManagers.Add(torrentManager);
            var torrentListItem = new TorrentModel(torrentManager.ToString());
            _torrentListItems.Add(torrentListItem);
        }

        public async Task StartTorrent(TorrentModel item)
        {
            var manager = GetManagerByItem(item);
            await manager.StartAsync();
        }

        public async Task PauseTorrent(TorrentModel item)
        {
            var manager = GetManagerByItem(item);
            await manager.PauseAsync();
        }

        public async Task StopTorrent(TorrentModel item)
        {
            var manager = GetManagerByItem(item);
            await manager.StopAsync();
        }

        public async Task Delete(TorrentModel item)
        {
            if (_torrentListItems.Contains(item))
            {
                var id = _torrentListItems.IndexOf(item);
                var torrentManager = _torrentManagers[id];
                await torrentManager.StopAsync();
                var isRemoved = await _clientEngine.RemoveAsync(torrentManager);
                if (isRemoved)
                {
                    _torrentListItems.RemoveAt(id);
                    _torrentManagers.RemoveAt(id);
                }
            }
        }

        public async Task Stop()
        {
            await _clientEngine.SaveStateAsync(PathToStateFile);
            _clientEngine.Dispose();
        }
    }
}