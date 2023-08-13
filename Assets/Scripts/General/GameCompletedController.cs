using Assets.Scripts.Enums;
using Assets.Scripts.Extensions;
using Assets.Scripts.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.General
{
	public class GameCompletedController : MonoBehaviour
	{
		private const float FadeDuration = 1.0f;

		[SerializeField] private CanvasGroup Background;
		[SerializeField] private CanvasGroup MenuItems;

		private void Awake()
		{
			Background.Disable();
			MenuItems.Disable();
		}

		public void OnGameCompleted()
		{
			Background.FadeIn(FadeDuration, setOnComplete: () => MenuItems.FadeIn(FadeDuration, immediatelyInteractible: true));
			PlayerPrefsHelpers.SetGameCompleted();
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
