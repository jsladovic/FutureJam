using Assets.Scripts.Extensions;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.General
{
	[RequireComponent(typeof(CanvasGroup))]
	public class LevelTextController : MonoBehaviour
	{
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
			DisplayTutorialText(levelDefinition.LevelText);
		}

		private void DisplayTutorialText(string text)
		{
			FMODUnity.RuntimeManager.PlayOneShot("event:/UI/ping");
			Canvas.Enable();
			LevelText.text = text;
		}

		public void HideTutorialText()
		{
			Canvas.Disable();
		}

		public void OnLevelAlmostOver()
		{
			Canvas.FadeOut(1.0f);
		}
	}
}
