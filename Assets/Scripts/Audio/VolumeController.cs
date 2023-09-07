using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class VolumeController : MonoBehaviour
	{
        public static VolumeController Instance;

        private bool DisableAudio = false;
        private EventInstance MusicMute;
        private EventInstance AudioMute;

        [SerializeField] private EventReference MusicMuteSnapshot;
        [SerializeField] private EventReference AudioMuteSnapshot;

        private void Awake()
		{
            Instance = this;
            if (RuntimeManager.HasBankLoaded("Master") == false)
            {
                RuntimeUtils.DebugLogWarning("Master audio bank not loaded");
                DisableAudio = true;
            }
            else
            {
                MusicMute = RuntimeManager.CreateInstance(MusicMuteSnapshot);
                AudioMute = RuntimeManager.CreateInstance(AudioMuteSnapshot);
            }
        }

        public void OnMusicMuteChanged(bool mute)
        {
            if (DisableAudio == true /*| MusicMute.isValid() == false*/)
                return;
            if (mute == true && PlaybackState(MusicMute) != PLAYBACK_STATE.PLAYING)
                MusicMute.start();
            else if (mute == false && PlaybackState(MusicMute) == PLAYBACK_STATE.PLAYING)
                MusicMute.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        public void OnAudioMuteChanged(bool mute)
        {
            if (DisableAudio == true /*|| AudioMute.isValid() == false*/)
                return;
            if (mute == true && PlaybackState(AudioMute) != PLAYBACK_STATE.PLAYING)
                AudioMute.start();
            else if (mute == false && PlaybackState(AudioMute) == PLAYBACK_STATE.PLAYING)
                AudioMute.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        public static PLAYBACK_STATE PlaybackState(EventInstance Event)
       {
           PLAYBACK_STATE pState;
           Event.getPlaybackState(out pState);
           return pState;
       }
    }
}