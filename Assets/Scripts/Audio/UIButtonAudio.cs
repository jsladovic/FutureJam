using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Audio
{
	public class UIButtonAudio : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
	{
		[SerializeField] private EventReference HoverSound;
		[SerializeField] private EventReference PressSound;
		private bool DisableAudio = false;

		private void Start()
		{
            if (RuntimeManager.HasBankLoaded("Master") == false)
			{
                RuntimeUtils.DebugLogWarning("Master audio bank not loaded");
                DisableAudio = true;
			}
        }

		public void OnPointerEnter(PointerEventData eventData)
        {
			if (DisableAudio == false && HoverSound.IsNull == false)
				RuntimeManager.PlayOneShot(HoverSound);
		}
        public void OnPointerDown(PointerEventData eventData)
        {
            if (DisableAudio == false && PressSound.IsNull == false)
                RuntimeManager.PlayOneShot(PressSound);
        }

    }
}