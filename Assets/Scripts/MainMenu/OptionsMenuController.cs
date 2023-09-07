using Assets.Scripts.Audio;
using Assets.Scripts.Extensions;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MainMenu
{
	[RequireComponent(typeof(CanvasGroup))]
	public class OptionsMenuController : MonoBehaviour
	{
		[SerializeField] private Button FullScreenToggleButton;
		[SerializeField] private TextMeshProUGUI AudioToggleText;
		[SerializeField] private TextMeshProUGUI MusicToggleText;
		[SerializeField] private TextMeshProUGUI FullScreenToggleText;
		[SerializeField] private VolumeController VolumeController;

		private CanvasGroup Canvas;

		private void Awake()
		{
			FullScreenToggleButton.gameObject.HideForMobile();
			Canvas = GetComponent<CanvasGroup>();
			DisplayIsFullScreen();
			Disable();
		}

        private void Start()
        {
            VolumeController = GameObject.Find("AudioObject").GetComponent<VolumeController>();
            DisplayIsMusicMuted();
            DisplayIsSoundEffectMuted();
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
			if (VolumeController == null)
				return;
			bool isAudioMuted = PlayerPrefsHelpers.IsSoundEffectsMuted();
			isAudioMuted = !isAudioMuted;
			PlayerPrefsHelpers.SetIsSoundEffectsMuted(isAudioMuted);
			DisplayIsSoundEffectMuted();
		}

		public void MusicToggled()
		{
			if (VolumeController == null)
				return;
			bool isMusicMuted = PlayerPrefsHelpers.IsMusicMuted();
			isMusicMuted = !isMusicMuted;
			PlayerPrefsHelpers.SetIsMusicMuted(isMusicMuted);
			DisplayIsMusicMuted();
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
			if (FullScreenToggleText == null)
				return;
			bool isFullScreen = PlayerPrefsHelpers.IsFullScreen();
			FullScreenToggleText.text = $"Full Screen: {(isFullScreen ? "On" : "Off")}";
		}

		private void DisplayIsMusicMuted()
		{
			if (MusicToggleText == null)
				return;
			bool isMusicMuted = PlayerPrefsHelpers.IsMusicMuted();
			VolumeController.OnMusicMuteChanged(isMusicMuted);
			MusicToggleText.text = $"Music: {(isMusicMuted ? "Off" : "On")}";

		}

		private void DisplayIsSoundEffectMuted()
		{
			if (AudioToggleText == null)
				return;
			bool isSoundEffectsMuted = PlayerPrefsHelpers.IsSoundEffectsMuted();
			VolumeController.OnAudioMuteChanged(isSoundEffectsMuted);
			AudioToggleText.text = $"Sound Effects: {(isSoundEffectsMuted ? "Off" : "On")}";
		}
	}
}
