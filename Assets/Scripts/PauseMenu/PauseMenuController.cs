using Assets.Scripts.Enums;
using Assets.Scripts.Extensions;
using Assets.Scripts.GameEvents.Events;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.PauseMenu
{
	[RequireComponent(typeof(CanvasGroup))]
	public class PauseMenuController : MonoBehaviour
	{
		private CanvasGroup Canvas;

		[SerializeField] private BoolEvent OnPauseChanged;
		[SerializeField] private BoolEvent OnCanUseMouseChanged;
		[SerializeField] private BoolEvent OnTutorialStarted;

		private bool isPaused;
		private bool IsPaused
		{
			get { return isPaused; }
			set
			{
				Time.timeScale = value ? 0.0f : 1.0f;
				if (value == true)
				{
					IsDisplayed = true;
					Canvas.Enable();
				}
				else
				{
					Canvas.Disable();
				}
				isPaused = value;
			}
		}

		private bool IsDisplayed;

		private void Awake()
		{
			Canvas = GetComponent<CanvasGroup>();
			IsPaused = false;
			IsDisplayed = false;
		}

		private void Update()
		{
			if (IsDisplayed == false)
				return;

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				StartCoroutine(OnResumeCoroutine());
			}
		}

		public void OnPausedChanged(bool paused)
		{
			IsPaused = paused;
		}

		private IEnumerator OnResumeCoroutine()
		{
			yield return new WaitForEndOfFrame();
			OnResumeClicked();
		}

		public void OnResumeClicked()
		{
			IsPaused = false;
			OnPauseChanged.Raise(false);
			OnCanUseMouseChanged.Raise(true);
			IsDisplayed = false;

		}

		public void OnMainMenuClicked()
		{
			SceneManager.LoadScene((int)SceneBuildIndex.MainMenu);
			IsDisplayed = false;
		}

		public void OnTutorialClicked()
		{
			Canvas.Disable();
			OnTutorialStarted.Raise(true);
			IsDisplayed = false;
		}
	}
}
