using Assets.Scripts.Extensions;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.General
{
	[RequireComponent(typeof(CanvasGroup))]
	public class LevelTextController : MonoBehaviour
	{
		private const float CanvasFadeTimeSeconds = 1.0f;

		[SerializeField] private GameData GameData;

		private CanvasGroup Canvas;
		private TextMeshProUGUI LevelText;

		private void Awake()
		{
			Canvas = GetComponent<CanvasGroup>();
			LevelText = GetComponentInChildren<TextMeshProUGUI>();
			HideTutorialText();
		}

		public void OnLevelChanged(LevelDefinition levelDefinition)
		{
			DisplayLevelText(levelDefinition.LevelText);
		}

		private void DisplayLevelText(string text)
		{
			FMODUnity.RuntimeManager.PlayOneShot("event:/UI/ping");
			Canvas.FadeIn(CanvasFadeTimeSeconds);
			LevelText.text = GameData.GameType == Enums.GameType.Endless ? string.Empty : text;
		}

		public void HideTutorialText()
		{
			Canvas.Disable();
		}

		public void OnLevelAlmostOver()
		{
			Canvas.FadeOut(CanvasFadeTimeSeconds);
		}
	}
}
