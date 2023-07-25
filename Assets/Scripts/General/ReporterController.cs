using UnityEngine;

namespace Assets.Scripts.General
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class ReporterController : MonoBehaviour
	{
		[SerializeField] private int ShowsUpLevelIndex;

		private SpriteRenderer SpriteRenderer;

		private void Awake()
		{
			SpriteRenderer = GetComponent<SpriteRenderer>();
			SpriteRenderer.enabled = false;
		}

		public void OnLevelStarted(LevelDefinition levelDefinition)
		{
			SpriteRenderer.enabled = levelDefinition.Index >= ShowsUpLevelIndex;
		}
	}
}
