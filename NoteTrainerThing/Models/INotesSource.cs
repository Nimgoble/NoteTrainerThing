using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore.CoreAudioAPI;
using NoteTrainerThing.ViewModels;

namespace NoteTrainerThing.Models
{
	public interface INotesSource
	{
		NoteViewModel GetNextNote(NoteViewModel currentNote);
	}
}