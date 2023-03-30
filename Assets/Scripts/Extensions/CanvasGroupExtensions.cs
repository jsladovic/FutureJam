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
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
		}

		public static void ToggleVisibility(this CanvasGroup canvasGroup)
		{
			if (canvasGroup.IsEnabled())
				canvasGroup.Disable();
			else
				canvasGroup.Enable();
		}
	}
}
