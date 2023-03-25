using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.General
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class CursorController : MonoBehaviour
	{
		public static CursorController Instance;

		private SpriteRenderer SpriteRenderer;
		private Sprite DefaultCursorSprite;
		[SerializeField] private Sprite HoveredCursorSprite;
		[SerializeField] private Sprite UpgradeCursorSprite;

		private List<PicketLiner> HoveredPicketLiners;

		private void Awake()
		{
			Instance = this;
			SpriteRenderer = GetComponent<SpriteRenderer>();
			DefaultCursorSprite = SpriteRenderer.sprite;
			HoveredPicketLiners = new List<PicketLiner>();
			Cursor.visible = false;
		}

		private void Update()
		{
			Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.position = cursorPosition;
		}

		public void RegisterPicketLinerHovered(PicketLiner picketLiner)
		{
			if (HoveredPicketLiners.Contains(picketLiner) == true)
				return;
			HoveredPicketLiners.Add(picketLiner);
			SetCursorSprite();
		}

		public void UnregisterPicketLinerHovered(PicketLiner picketLiner)
		{
			if (HoveredPicketLiners.Contains(picketLiner) == false)
				return;
			HoveredPicketLiners.Remove(picketLiner);
			SetCursorSprite();
		}

		public void SetCursorSprite()
		{
			if (HoveredPicketLiners.Any() == false)
				SpriteRenderer.sprite = DefaultCursorSprite;
			else if (GameController.Instance.IsWaitingForUpgrade == true)
				SpriteRenderer.sprite = UpgradeCursorSprite;
			else
				SpriteRenderer.sprite = HoveredCursorSprite;
		}
	}
}
