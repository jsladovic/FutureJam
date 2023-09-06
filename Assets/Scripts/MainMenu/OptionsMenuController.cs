using Assets.Scripts.Audio;
using Assets.Scripts.Extensions;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
	[RequireComponent(typeof(CanvasGroup))]
	public class OptionsMenuController : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI AudioToggleText;
		[SerializeField] private TextMeshProUGUI MusicToggleText;
		[SerializeField] private TextMeshProUGUI FullScreenToggleText;
		[SerializeField] private VolumeController AudioVolumeController;
		[SerializeField] private VolumeController MusicVolumeController;

		private bool IsAudioMuted = false;
		private bool IsMusicMuted = false;

		private CanvasGroup Canvas;

		private void Awake()
		{
			Canvas = GetComponent<CanvasGroup>();
			DisplayIsFullScreen();
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
			AudioToggleText.text = $"Sound Effects: {(IsAudioMuted ? "Off" : "On")}";
		}

		public void MusicToggled()
		{
			if (MusicVolumeController == null)
				return;
			IsMusicMuted = !IsMusicMuted;
			MusicVolumeController.OnMuteChanged(IsMusicMuted);

			if (MusicToggleText == null)
				return;
			MusicToggleText.text = $"Music: {(IsMusicMuted ? "Off" : "On")}";
		}

		public void FullScreenToggle()
		{
			bool isFullScreen = PlayerPrefsHelpers.IsFullScreen();
			isFullScreen = !isFullScreen;
			PlayerPrefsHelpers.SetIsFullScreen(isFullScreen);
			Screen.fullScreen = isFullScreen;
			DisplayIsFullScreen();
		}

		private void DisplayIsFullScreen()
		{
			bool isFullScreen = PlayerPrefsHelpers.IsFullScreen();
			FullScreenToggleText.text = $"Full Screen: {(isFullScreen ? "On" : "Off")}";
		}
	}
}
