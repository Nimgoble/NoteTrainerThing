using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace NoteTrainerThing.Models 
{
	public class TimedSoundPlayer
	{
		private Dictionary<Timer, SoundPlayer> openSounds = new Dictionary<Timer, SoundPlayer>();
		private IDeviceSource _deviceSource;
		public TimedSoundPlayer(IDeviceSource deviceSource)
		{
			_deviceSource = deviceSource;
		}

		public void PlaySound(string fileName, double time = 1)
		{
			Timer timer = new Timer();
			timer.Elapsed += TimerOnElapsed;
			SoundPlayer soundPlayer = new SoundPlayer(_deviceSource);
			timer.Interval = time * 1000;
			openSounds.Add(timer, soundPlayer);
			soundPlayer.ChangeSound(fileName, true);
			timer.Start();
		}

		private object mutex = new object();
		private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
		{
			Timer timer = (Timer) sender;
			lock (mutex)
			{
				try
				{
					SoundPlayer soundPlayer = null;
					if (openSounds.TryGetValue(timer, out soundPlayer))
					{
						soundPlayer.Stop();
						soundPlayer.CleanupPlayback();
					}
					openSounds.Remove(timer);
				}
				catch (Exception)
				{
				}
			}
		}
	}
}
