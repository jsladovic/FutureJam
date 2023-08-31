using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Assets.Scripts.GameEvents.Events;
using Assets.Scripts.General;
using Unity.VisualScripting;
using Unity.Mathematics;

namespace Assets.Scripts.Audio
{
	public class MusicPlayer : MonoBehaviour
	{
        private static MusicPlayer Instance;
		public EventInstance Music;
		private bool DisableAudio;
        private int CurrentAudioIndex = 1;
        private bool IsMenuLoaded;

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

		public void OnAudioIndexChanged(LevelDefinition levelDef)
        {
            if (DisableAudio == true)
                return;

            //if (levelDef.AudioIndex != CurrentAudioIndex)
                //Music.setParameterByName("LevelEnd", 1);

            Music.setParameterByName("Level", levelDef.AudioIndex);
            //CurrentAudioIndex = levelDef.AudioIndex;
        }

        public void OnMenuLoaded()
        {
            if (DisableAudio == true)
                return;
            Music.setParameterByName("Level", 0);
            //CurrentAudioIndex = 1;
        }

        public void OnGameOver()
        {
            if (DisableAudio == true)
                return;

            Music.setParameterByName("LevelEnd", 1);
            Music.setParameterByName("Level", 0);
        }

       /* public static PLAYBACK_STATE PlaybackState(EventInstance Event)
        {
            PLAYBACK_STATE pState;
            Event.getPlaybackState(out pState);
            return pState;
        }*/

        private void OnDisable()
        {
            Music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            Music.release();
        }

	}
}
