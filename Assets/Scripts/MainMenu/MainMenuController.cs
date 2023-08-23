using Assets.Scripts.Enums;
using Assets.Scripts.Extensions;
using Assets.Scripts.GameEvents.Events;
using Assets.Scripts.General;
using Assets.Scripts.Helpers;
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
		[SerializeField] private TextMeshProUGUI StartStrikeButtonText;

		[SerializeField] private OptionsMenuController OptionsMenu;
		[SerializeField] private GameData GameData;
		[SerializeField] private VoidEvent OnMainMenuLoaded;

		private void Awake()
		{
			Time.timeScale = 1.0f;
			CanvasGroup = GetComponent<CanvasGroup>();
			CanvasGroup.Disable();
			MainMenu.Enable();
			CanvasGroup.FadeIn(1.0f, immediatelyInteractible: true, setOnComplete: () => OnMainMenuLoaded.Raise());

			bool isGameCompleted = PlayerPrefsHelpers.IsGameCompleted();
			if (isGameCompleted == true)
			{
				StartEndlessButton.gameObject.SetActive(true);
				StartStrikeButtonText.text = "Start regular strike";
			}
			else
			{
				StartEndlessButton.gameObject.SetActive(false);
				StartStrikeButtonText.text = "Start strike";
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
