using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using LibVLCSharp.Shared;
using Microsoft.Win32;

namespace TutorialApp.ViewModel
{
    public partial class VideoViewModel : ObservableObject
    {
        private LibVLC _libVLC;

        [ObservableProperty]
        private ObservableCollection<MediaPlayer> _mediaPlayers = [];
        private bool _isFileDialogOpened;

        public VideoViewModel()
        {
            Core.Initialize();
            _libVLC = new LibVLC();

            for (int i = 0; i < 4; ++i)
            {
                var player = new MediaPlayer(_libVLC);

                MediaPlayers.Add(player);
            }
            _isFileDialogOpened = false;
        }

        [RelayCommand]
        private async Task Open(MediaPlayer player)
        {
            if (_isFileDialogOpened)
            {
                return;
            }

            _isFileDialogOpened = true;

            try
            {
                OpenFileDialog openFileDialog = new();

                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;

                    await Task.Run(() =>
                    {
                        var media = new Media(_libVLC, new Uri(filePath));

                        media.AddOption(":input-repeat=1");

                        player.Play(media);
                    });
                }
            }
            finally
            {
                _isFileDialogOpened = false;
            }
        }

        [RelayCommand]
        private void Play(MediaPlayer player)
        {

        }

        [RelayCommand]
        private void Stop(MediaPlayer player)
        {

        }
    }
}
