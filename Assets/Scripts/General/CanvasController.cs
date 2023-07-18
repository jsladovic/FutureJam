using Assets.Scripts.Extensions;
using Assets.Scripts.GameEvents.Events;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.General
{
	[RequireComponent(typeof(CanvasGroup))]
	public class CanvasController : MonoBehaviour
	{
		public static CanvasController Instance;

		[SerializeField] private CanvasGroup ContinueButtonsParent;
		[SerializeField] private CanvasGroup ButtonsParent;
		[SerializeField] private TextMeshProUGUI LevelText;
		[SerializeField] private Button KickOutScabButton;
		[SerializeField] private BoolEvent OnTutorialStarted;

		private CanvasGroup CanvasGroup;

		private void Awake()
		{
			Instance = this;
			CanvasGroup = GetComponent<CanvasGroup>();
		}

		public void DisplayLevel(int levelIndex, bool displayOptions, bool canKickOutScab)
		{
			LevelText.text = $"Day {levelIndex - 1} of the strike is over.";
			if (levelIndex == 1)
			{
				if (PlayerPrefsHelpers.WasTutorialDisplayed() == false)
				{
					OnTutorialStarted.Raise(false);
					CanvasGroup.Disable();
				}
				else
				{
					StartLevel(fadeOut: false);
				}
				return;
			}
			if (displayOptions)
			{
				DisplayEndOfLevelCanvas(true);
				KickOutScabButton.interactable = canKickOutScab;
			}
			else
			{
				DisplayEndOfLevelCanvas(false);
			}
		}

		private void DisplayEndOfLevelCanvas(bool allowOptions)
		{
			DisplayButtons(allowOptions ? ButtonsParent : ContinueButtonsParent);
		}

		public void ContinueClicked()
		{
			StartLevel();
		}

		public void KickOutScabClicked()
		{
			GameController.Instance.KickOutScab();
			StartLevel();
		}

		public void AddPicketLinerClicked()
		{
			GameController.Instance.SpawnPicketLiner();
			StartLevel();
		}

		public void RestartClicked()
		{
			Scene scene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(scene.buildIndex);
		}

		private void StartLevel(bool fadeOut = true)
		{
			GameController.Instance.StartLevel();
			if (fadeOut == true)
				DisplayButtons(null);
			else
				CanvasGroup.Disable();
		}

		private void DisplayButtons(CanvasGroup canvasGroup)
		{
			if (canvasGroup == null)
			{
				CanvasGroup.FadeOut(1.0f);
			}
			else
			{
				CanvasGroup.FadeIn(1.0f, immediatelyInteractible: true);
				ButtonsParent.gameObject.SetActive(canvasGroup == ButtonsParent);
				ContinueButtonsParent.gameObject.SetActive(canvasGroup == ContinueButtonsParent);
			}
		}
	}
}
