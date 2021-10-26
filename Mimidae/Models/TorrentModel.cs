using System.ComponentModel;
using MonoTorrent.Client;

namespace Mimidae.Models
{
    public class TorrentModel : INotifyPropertyChanged
    {
        private float _progress;
        private TorrentState _torrentState;

        public string Name { get; }

        public float Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                InvokePropertyChanged(nameof(FullDescription));
                InvokePropertyChanged(nameof(Progress));
            }
        }

        public TorrentState TorrentState
        {
            get => _torrentState;
            set
            {
                _torrentState = value;
                InvokePropertyChanged(nameof(FullDescription));
                InvokePropertyChanged(nameof(TorrentState));
            }
        }

        private void InvokePropertyChanged(string propertyName) =>
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

        public string FullDescription => $"{Name} | {Progress:00}% | {TorrentState}";

        public TorrentModel(string name)
        {
            Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}