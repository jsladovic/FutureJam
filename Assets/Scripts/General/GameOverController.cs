using Assets.Scripts.Enums;
using Assets.Scripts.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.General
{
	[RequireComponent(typeof(CanvasGroup))]
	public class GameOverController : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI EndGameText;

		private CanvasGroup Canvas;

		private void Awake()
		{
			Canvas = GetComponent<CanvasGroup>();
			Canvas.Disable();
		}

		public void OnGameOver(int numberOfDays)
		{
			EndGameText.text = $"The strike lasted {numberOfDays} day{(numberOfDays == 1 ? string.Empty : "s")}, but we work the same as before." +
				$"\r\n" +
				$"\r\n" +
				$"We cannot give up!";
			Canvas.Enable();
		}

		public void OnRestartClicked()
		{
			SceneManager.LoadScene((int)SceneBuildIndex.Game);
		}
	}
}
