using Assets.Scripts.Extensions;
using Assets.Scripts.GameEvents.Events;
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
			Canvas = GetComponent<CanvasGroup>();
			Canvas.Disable();
			Images = ImagesParent.GetComponentsInChildren<Image>();
			if (Images.Length == 0)
				throw new UnityException("Missing tutorial images");
		}

		public void OnTutorialStarted(bool resumeOnEnd)
		{
			ResumeOnEnd = resumeOnEnd;
			CurrentImageIndex = 0;
			Canvas.Enable();
		}

		public void OnEndClicked()
		{
			Canvas.Disable();
			if (ResumeOnEnd == true)
			{
				OnPauseChanged.Raise(true);
			}
			else
			{
				GameController.Instance.StartLevel();
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
				Images[i].gameObject.SetActive(i == CurrentImageIndex);
			}
		}
	}
}
