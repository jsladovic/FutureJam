using Assets.Scripts.Extensions;
using Assets.Scripts.GameEvents.Events;
using Assets.Scripts.Helpers;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace Assets.Scripts.General
{
	[RequireComponent(typeof(CanvasGroup))]
	public class TutorialController : MonoBehaviour
	{
		[SerializeField] private BoolEvent OnPauseChanged;
		[SerializeField] private VideoPlayer VideoPlayer;

		private CanvasGroup Canvas;
		private bool ResumeOnEnd;
		private bool IsDisplayed;

		public int CurrentImageIndex { get; set; }

		private void Awake()
		{
			IsDisplayed = false;
			Canvas = GetComponent<CanvasGroup>();
			Canvas.Disable();
		}

		private void Update()
		{
			if (IsDisplayed == false)
				return;

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				StartCoroutine(OnEndCoroutine());
			}
		}

		public void OnTutorialStarted(bool resumeOnEnd)
		{
			PlayerPrefsHelpers.SetTutorialDisplayed();
			ResumeOnEnd = resumeOnEnd;
			CurrentImageIndex = 0;
			Canvas.Enable();
			IsDisplayed = true;
			VideoPlayer.time = 0.0;
			VideoPlayer.Play();
		}

		private IEnumerator OnEndCoroutine()
		{
			yield return new WaitForEndOfFrame();
			OnEndClicked();
		}

		public void OnEndClicked()
		{
			IsDisplayed = false;
			Canvas.Disable();
			if (ResumeOnEnd == true)
			{
				OnPauseChanged.Raise(true);
			}
			else
			{
				Canvas.FadeOut(1.0f, setOnComplete: () => GameController.Instance.StartLevel());
			}
		}
	}
}
