using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class MusicPlayer : MonoBehaviour
	{
		public EventInstance Music;
        public EventReference Drek;
		private bool DisableAudio;
		private void Start()
		{
            try
            {
                Music = RuntimeManager.CreateInstance("event:/Music/music");
                Music.start();
            }
            catch (EventNotFoundException)
            {
                RuntimeUtils.DebugLogWarning($"[FMOD] Event not found:/Music/music");
				DisableAudio = true;
                return;
            }
		}

		public void OnLevelChanged() //add level numbers
        {
			if (DisableAudio == false)
				Music.setParameterByName("Level", 0.0f);
		}

        private void OnDisable()
        {
            Music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            Music.release();
        }
	}
}
