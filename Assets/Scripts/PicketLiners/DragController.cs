using Assets.Scripts.Extensions;
using Assets.Scripts.GameEvents.Events;
using Assets.Scripts.General;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.PicketLiners
{
	public class DragController : MonoBehaviour
	{
		private static string[] ValidLayerNames = new string[] { "PicketLinerDraggable" };

		[SerializeField] private PicketLinerEvent SpawnDemotedPicketLiner;
		[SerializeField] private PicketLinerEvent OnPicketLinerDestroyed;

		private float MinX;
		private float MaxX;
		private float MinY;
		private float MaxY;

		private PicketLiner Parent;
		private bool isDragging;
		public bool IsDragging
		{
			get { return isDragging; }
			private set
			{
				isDragging = value;
				Parent.OnIsDraggedChanged();
			}
		}

		private Vector3 MousePositionOffset;
		private Vector3 DragStartPosition;

		private List<Transform> InvalidCollisionObjects;
		private List<PicketLiner> CollidingPicketLiners;
		private bool CanUseMouse;

		public void Initialize(PicketLiner parent)
		{
			Parent = parent;
			IsDragging = false;
			Transform topLeftDraggableLimit = GameController.Instance.TopLeftDraggablePosition;
			MinX = topLeftDraggableLimit.position.x;
			MaxY = topLeftDraggableLimit.position.y;
			Transform bottomRightDraggableLimit = GameController.Instance.BottomRightDraggablePosition;
			MaxX = bottomRightDraggableLimit.position.x;
			MinY = bottomRightDraggableLimit.position.y;
		}

		public void OnMouseDown()
		{
			if (CanUseMouse == false || IsDragging == true)
				return;
			if (Parent.CanBeDemoted == true)
			{
				SpawnDemotedPicketLiner.Raise(Parent);
				Parent.Rank = PicketLinerRank.Basic;
			}

			DragStartPosition = transform.position;
			MousePositionOffset = transform.position - Camera.main.MouseWorldPosition();
			IsDragging = true;
			InvalidCollisionObjects = new List<Transform>();
			CollidingPicketLiners = new List<PicketLiner>();
			FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/liner_pickup");
		}

		private void OnMouseUp()
		{
			if (IsDragging == false)
				return;
			IsDragging = false;

			PicketLiner targetPicketLiner = CollidingPicketLiners.FirstOrDefault(pl => pl.CanBeUpgraded == true);
			if (targetPicketLiner != null)
			{
				targetPicketLiner.UpgradeRank();
				List<PicketLiner> remainingPicketLIners = CollidingPicketLiners.Where(pl => pl != targetPicketLiner).ToList();
				foreach(PicketLiner picketLiner in remainingPicketLIners)
				{
					picketLiner.ModelSelector.PlayIdleAnimation();
				}
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/liner_grow");
                DestroyPicketLiner();
			}
			FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/liner_drop");
		}

		private void OnMouseDrag()
		{
			if (IsDragging == false || CanUseMouse == false)
				return;
			transform.position = Camera.main.MouseWorldPosition() + MousePositionOffset;
			transform.position = new Vector3(Mathf.Clamp(transform.position.x, MinX, MaxX),
				Mathf.Clamp(transform.position.y, MinY, MaxY), transform.position.z);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (IsDragging == false)
				return;
			PicketLiner collidingPicketLiner = collision.GetComponent<PicketLiner>();
			if (collidingPicketLiner != null && collidingPicketLiner.CanBeUpgraded == true)
			{
				if (CollidingPicketLiners.Contains(collidingPicketLiner) == false)
				{
					CollidingPicketLiners.Add(collidingPicketLiner);
					DisplayMergeSprite();
					collidingPicketLiner.ModelSelector.PlayConnectFrontAnimation();
					Parent.ModelSelector.PlayConnectBackAnimation();
				}
			}
			else
			{
				if (InvalidCollisionObjects.Contains(collision.transform) == false)
					InvalidCollisionObjects.Add(collision.transform);
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (IsDragging == false)
				return;

			PicketLiner collidingPicketLiner = collision.GetComponent<PicketLiner>();
			if (collidingPicketLiner != null)
			{
				if (CollidingPicketLiners.Contains(collidingPicketLiner) == true)
				{
					collidingPicketLiner.DisplayMergeSprite(false);
					CollidingPicketLiners.Remove(collidingPicketLiner);
					collidingPicketLiner.ModelSelector.PlayIdleAnimation();

					if (CollidingPicketLiners.Any() == false)
					{
						// TODO play carried animation
						Parent.ModelSelector.PlayIdleAnimation();
					}
				}
			}
			else
			{
				if (InvalidCollisionObjects.Contains(collision.transform) == true)
					InvalidCollisionObjects.Remove(collision.transform);
			}
		}

		private void DisplayMergeSprite()
		{
			if (CollidingPicketLiners.Any() == false)
				return;
			for (int i = 0; i < CollidingPicketLiners.Count; i++)
			{
				CollidingPicketLiners[i].DisplayMergeSprite(i == 0);
			}
		}

		public void OnCanUseMouseChanged(bool canUseMouse)
		{
			if (canUseMouse == false)
				OnMouseUp();
			CanUseMouse = canUseMouse;
		}

		private void DestroyPicketLiner()
		{
			OnPicketLinerDestroyed.Raise(Parent);
			Destroy(gameObject);
		}
	}
}
