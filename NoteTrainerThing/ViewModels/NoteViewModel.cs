using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteTrainerThing.Models;

namespace NoteTrainerThing.ViewModels
{
	public class NoteViewModel
	{
		private Note note;

		public NoteViewModel(Note _note)
		{
			note = _note;
		}

		public String DisplayName
		{
			get
			{
				return note.DisplayName;
			}
		}

		public String FileName
		{
			get
			{
				return note.FileName;
			}
		}

		public float BaseHertz
		{
			get
			{
				return note.Hertz;
			}
		}

		private float currentHertz;

		public float CurrentHertz
		{
			get
			{
				return currentHertz;
			}
		}

		public override bool Equals(object obj)
		{
			if ((obj is NoteViewModel) == false)
				return false;

			NoteViewModel noteObj = (NoteViewModel)obj;
			return noteObj.DisplayName == DisplayName &&
			       noteObj.FileName == FileName &&
			       noteObj.BaseHertz == BaseHertz &&
			       noteObj.CurrentHertz == CurrentHertz;
		}
	}
}