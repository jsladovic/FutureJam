using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.Extensions;
using Assets.Scripts.GameEvents.Events;
using Assets.Scripts.Helpers;

namespace Assets.Scripts.General
{
	[RequireComponent(typeof(CanvasGroup))]
	public class CanvasController : MonoBehaviour
	{
		public static CanvasController Instance;

		[SerializeField] private GameObject ContinueButtonsParent;
		[SerializeField] private GameObject ButtonsParent;
		[SerializeField] private TextMeshProUGUI LevelText;
		[SerializeField] private Button KickOutScabButton;
		[SerializeField] private GameObject TutorialPanel;
		[SerializeField] private TextMeshProUGUI TutorialText;
		[SerializeField] private BoolEvent OnTutorialStarted;

		private CanvasGroup CanvasGroup;

		private const int TutorialTextVisibleSeconds = 10;

		private void Awake()
		{
			Instance = this;
			CanvasGroup = GetComponent<CanvasGroup>();
		}

		public void Initialize()
		{
			DisplayButtons(null);
			HideTutorialText();
		}

		public void DisplayLevel(int levelIndex, bool displayOptions, bool canKickOutScab)
		{
			LevelText.text = $"Factory strike, day {levelIndex}";
			if (levelIndex == 1)
			{
				if (PlayerPrefsHelpers.WasTutorialDisplayed() == false)
				{
					OnTutorialStarted.Raise(false);
				}
				else
				{
					StartLevel();
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

		public void DisplayLevelText(string text)
		{
			DisplayTutorialTextCoroutine(text);
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/ping");
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

		private void DisplayTutorialTextCoroutine(string text)
		{
			TutorialPanel.SetActive(true);
			TutorialText.text = text;
		}

		public void HideTutorialText()
		{
			TutorialPanel.SetActive(false);
		}

		public void OnPausedChanged(bool isPaused)
		{
			if (isPaused == true)
				CanvasGroup.Disable();
			else
				CanvasGroup.Enable();
		}

		private void StartLevel()
		{
			GameController.Instance.StartLevel();
			DisplayButtons(null);
		}

		private void DisplayButtons(GameObject buttonsObject)
		{
			ButtonsParent.gameObject.SetActive(buttonsObject == ButtonsParent);
			ContinueButtonsParent.gameObject.SetActive(buttonsObject == ContinueButtonsParent);
		}
	}
}
