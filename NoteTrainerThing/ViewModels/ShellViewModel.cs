using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using Newtonsoft.Json;
using NoteTrainerThing.Models;

namespace NoteTrainerThing.ViewModels
{
	public class ShellViewModel : Screen, IDeviceSource, INotesSource
	{
		public ShellViewModel()
		{
			LoadNotes();
			LoadMMDevices();
			soundPlayer = new SoundPlayerViewModel(this, this);
			soundPlayer.NoteChanged += SoundPlayerOnNoteChanged;
			AnswerBoard = new AnswerBoardViewModel(notes, soundPlayer.CurrentNote, this);
		}

		private void SoundPlayerOnNoteChanged(NoteViewModel oldNote, NoteViewModel newNote)
		{
			AnswerBoard = new AnswerBoardViewModel(notes, newNote, this);
		}

		#region INotesSource

		public NoteViewModel GetNextNote(NoteViewModel currentNote)
		{
			var availableNotes = notes.Where(x => x.Equals(currentNote) == false).ToList();
			if (availableNotes.Any())
			{
				Random rand = new Random();
				int nextIndex = rand.Next(0, availableNotes.Count - 1);
				return availableNotes[nextIndex];
			}
			return null;
		}

		public MMDevice GetDevice()
		{
			return selectedDevice;
		}

		#endregion

		#region Methods

		protected override void OnDeactivate(bool close)
		{
			if (soundPlayer != null)
			{
				soundPlayer.Dispose();
				soundPlayer = null;
			}

			base.OnDeactivate(close);
		}

		private void LoadNotes()
		{
			try
			{
				string json = string.Empty;
				using (StreamReader r = new StreamReader("Notes.json"))
				{
					json = r.ReadToEnd();
				}
				List<Note> bareNotes = JsonConvert.DeserializeObject<List<Note>>(json);
				foreach (Note note in bareNotes.OrderBy(x => x.Hertz))
					notes.Add(new NoteViewModel(note));

				//answerBoard = new AnswerBoardViewModel(notes, null);
			}
			catch (Exception ex)
			{
			}
		}

		private void LoadMMDevices()
		{
			using (var mmdeviceEnumerator = new MMDeviceEnumerator())
			{
				using (var mmdeviceCollection = mmdeviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
				{
					foreach (var device in mmdeviceCollection)
						devices.Add(device);
				}
			}
			if (devices.Count > 0)
				selectedDevice = devices[0];
		}

		#endregion

		#region Properties

		private SoundPlayerViewModel soundPlayer = null;
		public SoundPlayerViewModel SoundPlayer
		{
			get
			{
				return soundPlayer;
			}
		}

		private AnswerBoardViewModel answerBoard = null;
		public AnswerBoardViewModel AnswerBoard
		{
			get
			{
				return answerBoard;
			}
			set
			{
				if (answerBoard == value)
					return;
				answerBoard = value;
				NotifyOfPropertyChange(() => AnswerBoard);
			}
		}

		private ObservableCollection<NoteViewModel> notes = new ObservableCollection<NoteViewModel>();
		private ObservableCollection<NoteViewModel> Notes
		{
			get
			{
				return notes;
			}
		}

		private ObservableCollection<MMDevice> devices = new ObservableCollection<MMDevice>();
		public ObservableCollection<MMDevice> Devices
		{
			get
			{
				return devices;
			}
		}

		private MMDevice selectedDevice = null;
		public MMDevice SelectedDevice
		{
			get
			{
				return selectedDevice;
			}
			set
			{
				if (selectedDevice == value)
					return;

				selectedDevice = value;
				NotifyOfPropertyChange(() => SelectedDevice);
			}
		}

		#endregion
	}
}