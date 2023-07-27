using Assets.Scripts.Audio;
using Assets.Scripts.Extensions;
using System.Threading;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
	[RequireComponent(typeof(CanvasGroup))]
	public class OptionsMenuController : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI AudioToggleText;
		[SerializeField] private TextMeshProUGUI MusicToggleText;
		[SerializeField] private VolumeController AudioVolumeController;
        [SerializeField] private VolumeController MusicVolumeController;

		private bool IsAudioMuted = false;
		private bool IsMusicMuted = false;

        private CanvasGroup Canvas;

		private void Awake()
		{
			Canvas = GetComponent<CanvasGroup>();
			Disable();
		}

		public void Disable()
		{
			Canvas.Disable();
		}

		public void Enable()
		{
			Canvas.Enable();
		}

		public void AudioToggled()
		{
			if (AudioVolumeController == null)
				return;
			IsAudioMuted = !IsAudioMuted;
			AudioVolumeController.OnMuteChanged(IsAudioMuted);

			if (AudioToggleText == null)
				return;
			if (IsAudioMuted == true)			
				AudioToggleText.text = "Sound Effects: Off";
			else
                AudioToggleText.text = "Sound Effects: On";
        }

		public void MusicToggled()
		{
            if (MusicVolumeController == null)
                return;
            IsMusicMuted = !IsMusicMuted;
            MusicVolumeController.OnMuteChanged(IsMusicMuted);

			if (MusicToggleText == null)
				return;
			if (IsMusicMuted == true)
				MusicToggleText.text = "Music: Off";			
			else
                MusicToggleText.text = "Music: On";
        }
	}
}
