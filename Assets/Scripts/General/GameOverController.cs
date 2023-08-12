using Assets.Scripts.Enums;
using Assets.Scripts.Extensions;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.General
{
	public class GameOverController : MonoBehaviour
	{
		private const float GameOverWaitSeconds = 2.0f;
		private const float FadeDuration = 1.0f;

		[SerializeField] private TextMeshProUGUI EndGameText;

		[SerializeField] private CanvasGroup Background;
		[SerializeField] private CanvasGroup MenuItems;

		private void Awake()
		{
			Background.Disable();
			MenuItems.Disable();
		}

		public void OnGameOver(int numberOfDays)
		{
			EndGameText.text = $"The strike lasted {numberOfDays} day{(numberOfDays == 1 ? string.Empty : "s")}, but we work the same as before." +
				$"\r\n" +
				$"\r\n" +
				$"We cannot give up!";
			StartCoroutine(GameOverCoroutine());
		}

		private IEnumerator GameOverCoroutine()
		{
			yield return new WaitForSeconds(GameOverWaitSeconds);
			Background.FadeIn(FadeDuration, setOnComplete: () => MenuItems.FadeIn(FadeDuration, immediatelyInteractible: true));
		}

		public void OnRestartClicked()
		{
			LoadScene(SceneBuildIndex.Game);
		}

		public void OnMainMenuClicked()
		{
			LoadScene(SceneBuildIndex.MainMenu);
		}

		private void LoadScene(SceneBuildIndex sceneBuildIndex)
		{
			MenuItems.FadeOut(FadeDuration, setOnComplete: () => SceneManager.LoadScene((int)sceneBuildIndex));
		}
	}
}
