using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
namespace NoteTrainerThing.ViewModels
{
	public class AnswerViewModel : Screen
	{
		private NoteViewModel note;
		public AnswerViewModel(NoteViewModel note)
		{
			this.note = note;
			DisplayName = note.DisplayName;
		}

		#region Methods

		public void Choose()
		{
			HasBeenChosen = true;
			IsCorrectAnswer = IsCorrectAnswer; //Force update...?
			if (Chosen != null)
				Chosen(this, this);
		}

		public event EventHandler<AnswerViewModel> Chosen;

		#endregion

		#region Properties

		public String FileName { get { return note.FileName; } }

		private bool hasBeenChosen = false;
		public bool HasBeenChosen
		{
			get { return hasBeenChosen; }
			set
			{
				if (value == hasBeenChosen)
					return;
				hasBeenChosen = value;
				NotifyOfPropertyChange(() => HasBeenChosen);
			}
		}

		private bool isCorrectAnswer = false;
		public bool IsCorrectAnswer 
		{
			get { return isCorrectAnswer; }
			set 
			{
				if (value == isCorrectAnswer)
					return;
				isCorrectAnswer = value;
				NotifyOfPropertyChange(() => IsCorrectAnswer);
			}
		}

		private String displayName = String.Empty;
		public String DisplayName
		{
			get { return displayName; }
			set
			{
				if (displayName == value)
					return;
				displayName = value;
				NotifyOfPropertyChange(() => DisplayName);
			}
		}

		#endregion

	}
}