using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using LibVLCSharp.Shared;

namespace TutorialApp.ViewModel
{
    public partial class VideoViewModel : ObservableObject
    {
        private LibVLC _libVLC;

        [ObservableProperty]
        private ObservableCollection<MediaPlayer> _mediaPlayers = [];

        public VideoViewModel()
        {
            Core.Initialize();
            _libVLC = new LibVLC();

            for (int i = 0; i < 4; ++i)
            {
                var player = new MediaPlayer(_libVLC);

                MediaPlayers.Add(player);
            }
        }
    }
}
