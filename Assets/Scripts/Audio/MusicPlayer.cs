using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class MusicPlayer : MonoBehaviour
	{
        private static MusicPlayer Instance;
		public EventInstance Music;
		private bool DisableAudio;
		private void Awake()
		{
            if (Instance == null)
                Instance = this;
            else
            {
                Object.Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);

            if (RuntimeManager.HasBankLoaded("Master") == false)
            {
                RuntimeUtils.DebugLogWarning("Master audio bank not loaded");
                return;
            }
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

		public void OnAudioIndexChanged(int audioLevel) //add level numbers
        {
            if (DisableAudio == true)
                return;

            if (audioLevel >= 1 && audioLevel < 6)
				Music.setParameterByName("Level", 1);
            else if (audioLevel >= 6 && audioLevel < 11)
                Music.setParameterByName("Level", 2);
            else if (audioLevel >= 11)
                Music.setParameterByName("Level", 3);
        }

        private void OnDisable()
        {
            Music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            Music.release();
        }
	}
}
