using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using NoteTrainerThing.Models;

namespace NoteTrainerThing.ViewModels
{
	public class AnswerBoardViewModel : Screen
	{
		private NoteViewModel correctNote = null;
		private IDeviceSource _deviceSource;
		private TimedSoundPlayer _timedSoundPlayer;
		public AnswerBoardViewModel( IEnumerable<NoteViewModel> notes, NoteViewModel correctNote, IDeviceSource deviceSource )
		{
			this.correctNote = correctNote;
			this._deviceSource = deviceSource;
			_timedSoundPlayer = new TimedSoundPlayer(_deviceSource);
			foreach (NoteViewModel note in notes)
			{
				AnswerViewModel answer = new AnswerViewModel(note)
				{
					IsCorrectAnswer = (note.Equals(correctNote))
				};
				answer.Chosen += AnswerOnChosen;
				answers.Add(answer);
			}
		}

		private void AnswerOnChosen(object sender, AnswerViewModel answerViewModel)
		{
			_timedSoundPlayer.PlaySound(answerViewModel.FileName);
		}

		#region Methods
		//public void NoteChanged()
		#endregion

		#region Properties
		private ObservableCollection<AnswerViewModel> answers = new ObservableCollection<AnswerViewModel>();
		public ObservableCollection<AnswerViewModel> Answers { get { return answers; } }
		#endregion
	}
}