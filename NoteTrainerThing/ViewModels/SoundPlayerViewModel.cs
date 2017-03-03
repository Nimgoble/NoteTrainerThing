using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using CSCore;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using NoteTrainerThing.Models;

namespace NoteTrainerThing.ViewModels
{
	public class SoundPlayerViewModel : Screen, IDisposable
	{
		private INotesSource _notesSource;
		private IDeviceSource _deviceSource;
		private SoundPlayer _soundPlayer;

		public SoundPlayerViewModel(INotesSource notesSource, IDeviceSource deviceSource)
		{
			_notesSource = notesSource;
			CurrentNote = _notesSource.GetNextNote(currentNote);
			_soundPlayer = new SoundPlayer(deviceSource);
			_soundPlayer.Stopped += SoundOutOnStopped;
		}

		~SoundPlayerViewModel()
		{
			_soundPlayer.CleanupPlayback();
		}

		#region IDisposable

		public void Dispose()
		{
			_soundPlayer.CleanupPlayback();
		}

		#endregion

		#region Methods

		public void LoadNote(NoteViewModel noteViewModel, MMDevice device)
		{
			_soundPlayer.ChangeSound(noteViewModel.FileName);
			currentNote = noteViewModel;
			NotifyOfPropertyChange(() => CanTogglePlay);
		}

		public void TogglePlay()
		{
			if (PlaybackState == PlaybackState.Stopped && currentNote != null && currentDevice != null)
				Restart();
			else if (PlaybackState == PlaybackState.Paused)
				Play();
			else
				Pause();

			NotifyOfPropertyChange(() => PlayText);
		}

		public delegate void NoteChangedHandler(NoteViewModel oldNote, NoteViewModel newNote);
		public event NoteChangedHandler NoteChanged;
		protected virtual void OnNoteChanged(NoteViewModel oldNote, NoteViewModel newNote)
		{
			if (NoteChanged != null)
			{
				NoteChanged(oldNote, newNote);
			}
		}

		public void PlayNext()
		{
			Stop();
			NoteViewModel oldNote = CurrentNote;
			CurrentNote = _notesSource.GetNextNote(currentNote);
			_soundPlayer.ChangeSound(currentNote.FileName);
			OnNoteChanged(oldNote, CurrentNote);
			Restart();
		}

		public void Play()
		{
			//if (_soundOut != null)
			//	_soundOut.Play();

			_soundPlayer.Play();
		}

		private void Restart()
		{
			_soundPlayer.Restart();
		}

		public void Pause()
		{
			//if (_soundOut != null)
			//	_soundOut.Pause();
			_soundPlayer.Pause();
		}

		public void Stop()
		{
			//if (_soundOut != null)
			//	_soundOut.Stop();
			_soundPlayer.Stop();
		}

		//private void InitializePlayback(NoteViewModel noteViewModel, MMDevice device)
		//{
		//	_waveSource =
		//		CodecFactory.Instance.GetCodec(noteViewModel.FileName)
		//			.ToSampleSource()
		//			.ToMono()
		//			.ToWaveSource();
		//	_soundOut = new WasapiOut()
		//	{
		//		Latency = 100,
		//		Device = device
		//	};
		//	_soundOut.Initialize(_waveSource);
		//	_soundOut.Stopped += SoundOutOnStopped;
		//}

		//private void CleanupPlayback()
		//{
		//	if (_soundOut != null)
		//	{
		//		//_soundOut.Stopped -= SoundOutOnStopped;
		//		_soundOut.Dispose();
		//		_soundOut = null;
		//	}
		//	if (_waveSource != null)
		//	{
		//		_waveSource.Dispose();
		//		_waveSource = null;
		//	}
		//}

		private void SoundOutOnStopped(object sender, PlaybackStoppedEventArgs playbackStoppedEventArgs)
		{
			NotifyOfPropertyChange(() => PlayText);
		}

		protected override void OnDeactivate(bool close)
		{
			base.OnDeactivate(close);
			_soundPlayer.CleanupPlayback();
			//CleanupPlayback();
		}

		#endregion

		#region Properties

		public String PlayText
		{
			get
			{
				switch (PlaybackState)
				{
					case PlaybackState.Stopped:
					case PlaybackState.Paused:
						return "Play";
					case PlaybackState.Playing:
						return "Pause";
				}

				return "Invalid Play State";
			}
		}

		public Boolean CanTogglePlay
		{
			get
			{
				return (currentNote != null);
			}
		}

		public event EventHandler<PlaybackStoppedEventArgs> PlaybackStopped
		{
			add { _soundPlayer.Stopped += value; }
			remove { _soundPlayer.Stopped -= value; }
		}

		public PlaybackState PlaybackState { get { return _soundPlayer.PlaybackState; } }

		public TimeSpan Position
		{
			get { return _soundPlayer.Position; }
			set { _soundPlayer.Position = value; }
		}

		public TimeSpan Length { get { return _soundPlayer.Length; } }

		public int Volume
		{
			get { return _soundPlayer.Volume; }
			set { _soundPlayer.Volume = value; }
		}

		private MMDevice currentDevice = null;

		private NoteViewModel currentNote;
		public NoteViewModel CurrentNote
		{
			get
			{
				return currentNote;
			}
			set
			{
				currentNote = value;
				NotifyOfPropertyChange(() => CurrentNote);
				NotifyOfPropertyChange(() => CanTogglePlay);
			}
		}

		#endregion
	}
}