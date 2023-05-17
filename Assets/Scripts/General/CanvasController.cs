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

		[SerializeField] private GameObject ButtonsParent;
		[SerializeField] private TextMeshProUGUI LevelText;
		[SerializeField] private Button AddPicketLinerButton;
		[SerializeField] private Button LevelUpPicketLinerButton;
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
			ButtonsParent.gameObject.SetActive(false);
			HideTutorialText();
		}

		public void DisplayLevel(int levelIndex, bool canAddPicketLiner, bool canKickOutScab, bool canLevelUpPicketLiner)
		{
			LevelText.text = $"Factory strike, day {levelIndex}";
			if (levelIndex == 1)
			{
				OnTutorialStarted.Raise();
				return;
			}
			if (canAddPicketLiner || canKickOutScab || canLevelUpPicketLiner)
			{
				ButtonsParent.gameObject.SetActive(true);
				LevelUpPicketLinerButton.interactable = canLevelUpPicketLiner;
				AddPicketLinerButton.interactable = canAddPicketLiner;
				KickOutScabButton.interactable = canKickOutScab;
			}
			else
			{
				GameController.Instance.StartLevel();
			}
		}

		public void DisplayLevelText(string text)
		{
			StartCoroutine(DisplayTutorialTextCoroutine(text));
		}

		public void KickOutScabClicked()
		{
			GameController.Instance.KickOutScab();
			GameController.Instance.StartLevel();
			ButtonsParent.gameObject.SetActive(false);
		}

		public void LevelUpPicketLinerClicked()
		{
			GameController.Instance.LevelUpPicketLiner();
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

		public void DisplayLevelUpText()
		{
			StartCoroutine(DisplayTutorialTextCoroutine("Click on a basic picket liner to level him up.", hideAfterWait: false));
		}

		private IEnumerator DisplayTutorialTextCoroutine(string text, bool hideAfterWait = true)
		{
			TutorialPanel.SetActive(true);
			TutorialText.text = text;
			if (hideAfterWait == true)
			{
				yield return new WaitForSeconds(TutorialTextVisibleSeconds);
				HideTutorialText();
			}
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
