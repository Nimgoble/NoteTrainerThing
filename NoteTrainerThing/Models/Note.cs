using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteTrainerThing.Models {
	public class Note
	{
		public String DisplayName { get; set; }
		public String FileName { get; set; }
		public float Hertz { get; set; }

		public override bool Equals(object obj)
		{
			if ((obj is Note) == false)
				return false;

			Note noteObj = (Note)obj;
			return noteObj.DisplayName == DisplayName &&
			       noteObj.FileName == FileName &&
			       noteObj.Hertz == Hertz;
		}
	}
}
