using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Audio
{
	public class VolumeController : MonoBehaviour
	{

        private bool DisableAudio = false;
        [SerializeField] private EventReference MuteSnapshot;
        private EventInstance SnapshotEvent;

        private void Start()
		{
            if (RuntimeManager.HasBankLoaded("Master") == false)
            {
                RuntimeUtils.DebugLogWarning("Master audio bank not loaded");
                DisableAudio = true;
            }
            else
                SnapshotEvent = RuntimeManager.CreateInstance(MuteSnapshot);
        }

		public void OnMuteChanged(bool mute)
        {
            if (DisableAudio == true || MuteSnapshot.IsNull == true)
                return;

            if (mute == true)
                SnapshotEvent.start();
            else
                SnapshotEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}