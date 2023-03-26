using UnityEngine;

namespace Assets.Scripts.PicketLiners
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class SphereOfInfluence : MonoBehaviour
	{
		private PicketLiner Parent;
		private SpriteRenderer SpriteRenderer;

		public void Initialize(PicketLiner picketLiner)
		{
			Parent = picketLiner;
			SpriteRenderer = GetComponent<SpriteRenderer>();
		}

		public void SetCarriedSprite(bool isCarried)
		{
			SpriteRenderer.enabled = !isCarried;
		}

		public bool IsCarried => Parent.IsCarried;
	}
}
