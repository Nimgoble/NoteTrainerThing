using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using NoteTrainerThing.ViewModels;

namespace NoteTrainerThing {
	class OurBootstrapper : BootstrapperBase {
		public OurBootstrapper() {
			Initialize();
		}

		protected override void OnStartup(object sender, StartupEventArgs e) {
			DisplayRootViewFor<ShellViewModel>();
		}
	}
}
