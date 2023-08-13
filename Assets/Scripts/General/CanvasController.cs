using Assets.Scripts.Extensions;
using Assets.Scripts.GameEvents.Events;
using Assets.Scripts.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.General
{
	public class CanvasController : MonoBehaviour
	{
		public static CanvasController Instance;

		private const float FadeDuration = 1.0f;

		private CanvasGroup Canvas;
		[SerializeField] private CanvasGroup Background;
		[SerializeField] private CanvasGroup MenuItems;

		[SerializeField] private CanvasGroup ContinueButtonsParent;
		[SerializeField] private CanvasGroup ButtonsParent;
		[SerializeField] private TextMeshProUGUI LevelText;
		[SerializeField] private Button KickOutScabButton;
		[SerializeField] private BoolEvent OnTutorialStarted;

		private bool IsClickable;

		private void Awake()
		{
			Instance = this;
			Canvas = GetComponent<CanvasGroup>();
			Canvas.Enable();
		}

		public void DisplayLevel(int levelIndex, bool displayOptions, bool canKickOutScab)
		{
			LevelText.text = $"Day {levelIndex - 1} of the strike is over.";
			if (levelIndex == 1)
			{
				if (PlayerPrefsHelpers.WasTutorialDisplayed() == false)
				{
					OnTutorialStarted.Raise(false);
					Disable(false);
				}
				else
				{
					Background.Enable();
					MenuItems.Disable();
					Background.FadeOut(FadeDuration, setOnComplete: () => StartLevel(fadeOut: false));
				}
				return;
			}

			IsClickable = true;
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
			if (IsClickable == false)
				return;
			IsClickable = false;
			StartLevel();
		}

		public void KickOutScabClicked()
		{
			if (IsClickable == false)
				return;
			IsClickable = false;
			GameController.Instance.KickOutScab();
			StartLevel(startLevel: false);
		}

		public void AddPicketLinerClicked()
		{
			if (IsClickable == false)
				return;
			IsClickable = false;
			GameController.Instance.SpawnPicketLiner();
			StartLevel();
		}

		public void RestartClicked()
		{
			Scene scene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(scene.buildIndex);
		}

		private void StartLevel(bool fadeOut = true, bool startLevel = true)
		{
			if (startLevel == true)
				GameController.Instance.StartLevel();
			if (fadeOut == true)
				DisplayButtons(null);
			else
				Disable(false);
		}

		private void DisplayButtons(CanvasGroup canvasGroup)
		{
			if (canvasGroup == null)
			{
				Disable(true);
			}
			else
			{
				Enable();
				ButtonsParent.gameObject.SetActive(canvasGroup == ButtonsParent);
				ContinueButtonsParent.gameObject.SetActive(canvasGroup == ContinueButtonsParent);
			}
		}

		private void Disable(bool fadeOut)
		{
			if (fadeOut == true)
			{
				MenuItems.FadeOut(FadeDuration, setOnComplete: () => Background.FadeOut(FadeDuration));
			}
			else
			{
				Background.Disable();
				MenuItems.Disable();
			}
		}

		private void Enable()
		{
			Background.FadeIn(FadeDuration, setOnComplete: () => MenuItems.FadeIn(FadeDuration, immediatelyInteractible: true));
		}
	}
}
