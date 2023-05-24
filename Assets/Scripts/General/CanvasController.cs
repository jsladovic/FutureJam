using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.Extensions;
using Assets.Scripts.GameEvents.Events;

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
		[SerializeField] private VoidEvent OnTutorialStarted;

		private CanvasGroup CanvasGroup;

		private const int TutorialTextVisibleSeconds = 10;

		private void Awake()
		{
			Instance = this;
			CanvasGroup = GetComponent<CanvasGroup>();
		}

		public void Initialize()
		{
			ButtonsParent.SetActive(false);
			ContinueButtonsParent.SetActive(false);
			HideTutorialText();
		}

		public void DisplayLevel(int levelIndex, bool displayOptions, bool canKickOutScab)
		{
			LevelText.text = $"Factory strike, day {levelIndex}";
			if (levelIndex == 1)
			{
				OnTutorialStarted.Raise();
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
			ButtonsParent.SetActive(allowOptions);
			ContinueButtonsParent.SetActive(!allowOptions);
		}

		public void ContinueClicked()
		{
			ContinueButtonsParent.SetActive(false);
			GameController.Instance.StartLevel();
		}

		public void DisplayLevelText(string text)
		{
			DisplayTutorialTextCoroutine(text);
            FMODUnity.RuntimeManager.PlayOneShot("event:/UI/ping");
        }

		public void KickOutScabClicked()
		{
			GameController.Instance.KickOutScab();
			GameController.Instance.StartLevel();
			ButtonsParent.gameObject.SetActive(false);
		}

		public void AddPicketLinerClicked()
		{
			GameController.Instance.SpawnPicketLiner();
			GameController.Instance.StartLevel();
			ButtonsParent.gameObject.SetActive(false);
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
	}
}
