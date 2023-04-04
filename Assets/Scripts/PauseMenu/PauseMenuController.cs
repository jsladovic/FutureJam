using Assets.Scripts.Enums;
using Assets.Scripts.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.PauseMenu
{
	[RequireComponent(typeof(CanvasGroup))]
	public class PauseMenuController : MonoBehaviour
	{
		private CanvasGroup Canvas;

		private bool isPaused;
		private bool IsPaused
		{
			get { return isPaused; }
			set
			{
				Time.timeScale = value ? 0.0f : 1.0f;
				if (value == true)
					Canvas.Enable();
				else
					Canvas.Disable();
				isPaused = value;
			}
		}

		private void Awake()
		{
			Canvas = GetComponent<CanvasGroup>();
			IsPaused = false;
		}

		public void OnPausedChanged(bool paused)
		{
			if (IsPaused == paused)
				return;
			IsPaused = paused;
		}

		public void OnResumeClicked()
		{
			IsPaused = false;
		}

		public void OnMainMenuClicked()
		{
			SceneManager.LoadScene((int)SceneBuildIndex.MainMenu);
		}
	}
}
