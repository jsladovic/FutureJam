using System.Collections;
using UnityEngine;

namespace Assets.Scripts.General
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class HealthBarItem : MonoBehaviour
	{
		private SpriteRenderer SpriteRenderer;
		[SerializeField] private Sprite WindowWorkingSprite;
		private Sprite WindowNotWorkingSprite;

		private Coroutine CurrentLightsCoroutine;

		public bool IsWorking { get; private set; }

		private void Awake()
		{
			SpriteRenderer = GetComponent<SpriteRenderer>();
			WindowNotWorkingSprite = SpriteRenderer.sprite;
			DisplayWindowWorking(false);
		}

		public void DisplayWindowWorking(bool isWorking)
		{
			IsWorking = isWorking;
			CurrentLightsCoroutine = StartCoroutine(DisplayWindowWorkingCoroutine(isWorking));
		}

		private IEnumerator DisplayWindowWorkingCoroutine(bool isWorking)
		{
			if (isWorking == true)
				yield return new WaitForSeconds(isWorking ? 5.0f : 0.0f);
			SpriteRenderer.sprite = isWorking ? WindowWorkingSprite : WindowNotWorkingSprite;
			if (CurrentLightsCoroutine != null)
				StopCoroutine(CurrentLightsCoroutine);
			CurrentLightsCoroutine = null;
		}
	}
}
