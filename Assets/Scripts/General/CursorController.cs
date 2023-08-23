using Assets.Scripts.Extensions;
using Assets.Scripts.PicketLiners;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.General
{
	public class CursorController : MonoBehaviour
	{
		public static CursorController Instance;

		[SerializeField] private Texture2D DefaultCursorSprite;
		[SerializeField] private Texture2D HoveredCursorSprite;
		[SerializeField] private Texture2D UpgradeCursorSprite;

		private List<PicketLiner> HoveredPicketLiners;

		private void Awake()
		{
			Instance = this;
			HoveredPicketLiners = new List<PicketLiner>();
			Cursor.SetCursor(DefaultCursorSprite, Vector2.zero, CursorMode.Auto);
		}

		public void OnPauseChanged(bool paused)
		{
			Cursor.visible = paused;
		}

		private void Update()
		{
			Vector2 cursorPosition = Camera.main.MouseWorldPosition();
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
				Cursor.SetCursor(DefaultCursorSprite, Vector2.zero, CursorMode.ForceSoftware);
			else
				Cursor.SetCursor(HoveredCursorSprite, Vector2.zero, CursorMode.ForceSoftware);
		}
	}
}
