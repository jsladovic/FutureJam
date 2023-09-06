using Assets.Scripts.Enums;
using Assets.Scripts.Extensions;
using Assets.Scripts.GameEvents.Events;
using Assets.Scripts.General;
using Assets.Scripts.Helpers;
using Assets.Scripts.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.MainMenu
{
	[RequireComponent(typeof(CanvasGroup))]
	public class MainMenuController : MonoBehaviour
	{
		private CanvasGroup CanvasGroup;
		[SerializeField] private CanvasGroup MainMenu;
		[SerializeField] private Button StartEndlessButton;
		[SerializeField] private Button ExitButton;
		[SerializeField] private TextMeshProUGUI StartStrikeButtonText;
		[SerializeField] private TextMeshProUGUI LongestStrikeText;
		[SerializeField] private TMPTextAutoSize TMPTextAutoSize;

		[SerializeField] private OptionsMenuController OptionsMenu;
		[SerializeField] private GameData GameData;
		[SerializeField] private VoidEvent OnMainMenuLoaded;

		private void Awake()
		{
			Time.timeScale = 1.0f;
			CanvasGroup = GetComponent<CanvasGroup>();
			CanvasGroup.Disable();
			MainMenu.Enable();
			ExitButton.gameObject.HideForMobile();
			CanvasGroup.FadeIn(1.0f, immediatelyInteractible: true, setOnComplete: () => OnMainMenuLoaded.Raise());

			bool isGameCompleted = PlayerPrefsHelpers.IsGameCompleted();

			StartStrikeButtonText.text = isGameCompleted ? "Start regular strike" : "Start strike";
			StartEndlessButton.gameObject.SetActive(isGameCompleted);
			LongestStrikeText.gameObject.SetActive(isGameCompleted);
			if (isGameCompleted)
			{
				int maxLevelCompleted = PlayerPrefsHelpers.GetMaxLevelCompleted();
				LongestStrikeText.text = $"Our longest strike lasted for {maxLevelCompleted} day{(maxLevelCompleted != 1 ? "s" : string.Empty)}.";
			}
			else
			{
				TMPTextAutoSize.Refresh();
			}
		}

		public void StartRegularStrikeClicked()
		{
			StartGame(GameType.Regular);
		}

		public void StartEndlessStrikeClicked()
		{
			StartGame(GameType.Endless);
		}

		public void OptionsClicked()
		{
			MainMenu.Disable();
			OptionsMenu.Enable();
		}

		public void CreditsCLicked()
		{
			MainMenu.Disable();
		}

		public void BackClicked()
		{
			MainMenu.Enable();
			OptionsMenu.Disable();
		}

		public void ExitClicked()
		{
			Application.Quit();
		}

		private void StartGame(GameType gameType)
		{
			GameData.GameType = gameType;
			CanvasGroup.FadeOut(1.0f, setOnComplete: () => SceneManager.LoadScene((int)SceneBuildIndex.Game));
		}
	}
}
