using UnityEngine;

namespace Assets.Scripts.General
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class HealthBarItem : MonoBehaviour
	{
		private SpriteRenderer SpriteRenderer;
		[SerializeField] private Sprite WindowWorkingSprite;
		private Sprite WindowNotWorkingSprite;

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
			SpriteRenderer.sprite = isWorking ? WindowWorkingSprite : WindowNotWorkingSprite;
			if (isWorking == true)
				FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/light_on");
		}
	}
}
