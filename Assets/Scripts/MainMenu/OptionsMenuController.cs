using Assets.Scripts.Extensions;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
	[RequireComponent(typeof(CanvasGroup))]
	public class OptionsMenuController : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI AudioToggleText;
		[SerializeField] private TextMeshProUGUI MusciToggleText;

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

		}

		public void MusicToggled()
		{

		}
	}
}
