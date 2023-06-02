using UnityEngine;

namespace Assets.Scripts.Extensions
{
	public static class CanvasGroupExtensions
	{
		public static bool IsEnabled(this CanvasGroup canvasGroup) => canvasGroup.alpha == 1.0f;

		public static void Enable(this CanvasGroup canvasGroup)
		{
			canvasGroup.alpha = 1.0f;
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
		}

		public static void Disable(this CanvasGroup canvasGroup)
		{
			canvasGroup.alpha = 0.0f;
			canvasGroup.DisableInteractivity();
		}

		public static void ToggleVisibility(this CanvasGroup canvasGroup)
		{
			if (canvasGroup.IsEnabled())
				canvasGroup.Disable();
			else
				canvasGroup.Enable();
		}

		public static void FadeIn(this CanvasGroup canvasGroup, float fadeInDuration)
		{
			LeanTween.value(canvasGroup.gameObject, 0.0f, 1.0f, fadeInDuration).setOnUpdate((float value) =>
			{
				canvasGroup.alpha = value;
			}).setOnComplete(() =>
			{
				canvasGroup.Enable();
			});
		}

		public static void FadeOut(this CanvasGroup canvasGroup, float fadeOutDuration)
		{
			canvasGroup.DisableInteractivity();
			LeanTween.value(canvasGroup.gameObject, 1.0f, 0.0f, fadeOutDuration).setOnUpdate((float value) =>
			{
				canvasGroup.alpha = value;
			});
		}

		private static void DisableInteractivity(this CanvasGroup canvasGroup)
		{
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
		}
	}
}
