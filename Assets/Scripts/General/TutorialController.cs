using Assets.Scripts.Extensions;
using Assets.Scripts.GameEvents.Events;
using Assets.Scripts.Helpers;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.General
{
	[RequireComponent(typeof(CanvasGroup))]
	public class TutorialController : MonoBehaviour
	{
		[SerializeField] private Transform ImagesParent;
		[SerializeField] private BoolEvent OnPauseChanged;

		private CanvasGroup Canvas;
		private Image[] Images;
		private bool ResumeOnEnd;
		private bool IsDisplayed;

		private int currentImageIndex;
		private int CurrentImageIndex
		{
			get { return currentImageIndex; }
			set
			{
				currentImageIndex = value;
				DisplayCurrentImage();
			}
		}

		private void Awake()
		{
			IsDisplayed = false;
			Canvas = GetComponent<CanvasGroup>();
			Canvas.Disable();
			Images = ImagesParent.GetComponentsInChildren<Image>();
			if (Images.Length == 0)
				throw new UnityException("Missing tutorial images");
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

		public void OnNextClicked()
		{
			CurrentImageIndex = (CurrentImageIndex + 1) % Images.Length;
		}

		public void OnPreviousClicked()
		{
			CurrentImageIndex = (CurrentImageIndex + Images.Length - 1) % Images.Length;
		}

		private void DisplayCurrentImage()
		{
			for (int i = 0; i < Images.Length; i++)
			{
				Images[i].gameObject.SetActive(false);
				//Images[i].gameObject.SetActive(i == CurrentImageIndex);
			}
		}
	}
}
