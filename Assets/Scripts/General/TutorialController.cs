using Assets.Scripts.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.General
{
	[RequireComponent(typeof(CanvasGroup))]
	public class TutorialController : MonoBehaviour
	{
		[SerializeField] private Transform ImagesParent;

		private CanvasGroup Canvas;
		private Image[] Images;

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

		public void OnTutorialStarted()
		{
			CurrentImageIndex = 0;
			Canvas.Enable();
		}

		public void OnEndClicked()
		{
			Canvas.Disable();
			GameController.Instance.StartLevel();
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
