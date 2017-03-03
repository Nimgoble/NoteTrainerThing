using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;

namespace NoteTrainerThing.Models 
{
	public interface IDeviceSource
	{
		MMDevice GetDevice();
	}

	public class SoundPlayer : IDisposable
	{
		#region Private Members
		private ISoundOut _soundOut;
		private IWaveSource _waveSource;
		private IDeviceSource _deviceSource;
		#endregion

		public SoundPlayer(IDeviceSource deviceSource)
		{
			_deviceSource = deviceSource;
		}

		~SoundPlayer()
		{
			CleanupPlayback();
		}

		#region Methods
		public void Play()
		{
			if (_soundOut != null)
				_soundOut.Play();
		}

		public void Restart()
		{
			CleanupPlayback();
			InitializePlayback(FileName);
			Play();
		}

		public void Pause()
		{
			if (_soundOut != null)
				_soundOut.Pause();
		}

		public void Stop()
		{
			if (_soundOut != null)
				_soundOut.Stop();
		}

		public void ChangeSound(string fileName, bool autoPlay = false)
		{
			Stop();
			CleanupPlayback();
			InitializePlayback(fileName);
			if (autoPlay)
				Play();
		}

		public void InitializePlayback(string fileName)
		{
			_fileName = fileName;
			_waveSource =
				CodecFactory.Instance.GetCodec(_fileName)
					.ToSampleSource()
					.ToMono()
					.ToWaveSource();
			_soundOut = new WasapiOut() 
			{
				Latency = 100,
				Device = _deviceSource.GetDevice()
			};
			_soundOut.Initialize(_waveSource);
			_soundOut.Stopped += SoundOutOnStopped;
		}

		public void CleanupPlayback() 
		{
			if (_soundOut != null) 
			{
				//_soundOut.Stopped -= SoundOutOnStopped;
				_soundOut.Dispose();
				_soundOut = null;
			}
			if (_waveSource != null) 
			{
				_waveSource.Dispose();
				_waveSource = null;
			}
		}

		//Forward the event
		public event EventHandler<PlaybackStoppedEventArgs> Stopped;
		private void SoundOutOnStopped(object sender, PlaybackStoppedEventArgs playbackStoppedEventArgs)
		{
			if (Stopped != null)
			{
				Stopped(sender, playbackStoppedEventArgs);
			}
		}
		#endregion

		#region Properties
		private String _fileName;
		public String FileName { get { return _fileName; } }

		public PlaybackState PlaybackState
		{
			get
			{
				if (_soundOut != null)
					return _soundOut.PlaybackState;
				return PlaybackState.Stopped;
			}
		}

		public TimeSpan Position
		{
			get
			{
				if (_waveSource != null)
					return _waveSource.GetPosition();
				return TimeSpan.Zero;
			}
			set
			{
				if (_waveSource != null)
					_waveSource.SetPosition(value);
			}
		}

		public TimeSpan Length
		{
			get
			{
				if (_waveSource != null)
					return _waveSource.GetLength();
				return TimeSpan.Zero;
			}
		}

		public int Volume
		{
			get
			{
				if (_soundOut != null)
					return Math.Min(100, Math.Max((int)(_soundOut.Volume * 100), 0));
				return 100;
			}
			set
			{
				if (_soundOut != null)
					_soundOut.Volume = Math.Min(1.0f, Math.Max(value / 100f, 0f));
			}
		}
		#endregion

		#region IDisposable
		void IDisposable.Dispose()
		{
			CleanupPlayback();
		}
		#endregion
	}
}
