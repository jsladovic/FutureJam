﻿using System;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
	public static class CanvasGroupExtensions
	{
		public static bool IsEnabled(this CanvasGroup canvasGroup) => canvasGroup.alpha == 1.0f;

		public static void Enable(this CanvasGroup canvasGroup)
		{
			canvasGroup.alpha = 1.0f;
			canvasGroup.EnableInteractivity();
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

		public static void FadeIn(this CanvasGroup canvasGroup, float fadeInDuration, bool immediatelyInteractible = false, Action setOnComplete = null)
		{
			if (immediatelyInteractible == true)
				canvasGroup.EnableInteractivity();
			LeanTween
				.value(canvasGroup.gameObject, 0.0f, 1.0f, fadeInDuration)
				.setIgnoreTimeScale(true)
				.setOnUpdate((float value) =>
				{
					canvasGroup.alpha = value;
				})
				.setOnComplete(() =>
				{
					canvasGroup.Enable();
					setOnComplete?.Invoke();
				});
		}

		public static void FadeOut(this CanvasGroup canvasGroup, float fadeOutDuration, Action setOnComplete = null)
		{
			canvasGroup.DisableInteractivity();
			LeanTween.value(canvasGroup.gameObject, 1.0f, 0.0f, fadeOutDuration).setOnUpdate((float value) =>
			{
				canvasGroup.alpha = value;
			}).setOnComplete(() =>
			{
				setOnComplete?.Invoke();
			});
		}

		private static void EnableInteractivity(this CanvasGroup canvasGroup)
		{
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
		}

		private static void DisableInteractivity(this CanvasGroup canvasGroup)
		{
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
		}
	}
}
