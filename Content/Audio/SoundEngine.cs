using System;
using NAudio.Wave;

namespace Comp_296_project_ARMA.Audio
{
   public class SoundEngine : IDisposable
   {
      private WaveOutEvent _waveOut;
      private AudioFileReader _audioFileReader;

      public bool IsPlaying { get; private set; }
      
      // Current position in ms
      public double CurrentTime
        {
            get
            {
                if (_audioFileReader != null)
                {
                    return _audioFileReader.CurrentTime.TotalMilliseconds;
                }
                return 0;
            }
        }

        // Total song length in ms
        public double TotalTime
        {
            get
            {
                if (_audioFileReader != null)
                {
                    return _audioFileReader.TotalTime.TotalMilliseconds;
                }
                return 0;
            }
        }

        public void LoadSong(string filePath)
        {
            Stop();
            _audioFileReader?.Dispose();
            _waveOut?.Dispose();

            _audioFileReader = new AudioFileReader(filePath);
            _waveOut = new WaveOutEvent();
            _waveOut.Init(_audioFileReader);
            _waveOut.PlaybackStopped += (s, e) => IsPlaying = false;
        }

        public void Play()
        {
            if (_waveOut != null)
            {
                _waveOut.Play();
                IsPlaying = true;
            }
            return;
        }

        public void Pause()
        {
            if (_waveOut != null)
            {
                _waveOut.Pause();
                IsPlaying = false;
            }
            return;
        }

        public void Stop()
        {
            if (_waveOut != null)
            {
                _waveOut.Stop();
                IsPlaying = false;
            }
            if (_audioFileReader != null)
            {
                _audioFileReader.Position = 0;
            }
        }

        public void SetVolume(float volume)
        {
            if (_waveOut != null)
            {
                _waveOut.Volume = Math.Clamp(volume, 0f, 1f);
            }
        }

        public void Dispose()
        {
            _audioFileReader?.Dispose();
            _waveOut?.Dispose();

        }
   }
}
