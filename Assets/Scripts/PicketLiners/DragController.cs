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
			Parent.ModelSelector.PlayHoverAnimation();
		}

		private void OnMouseUp()
		{
			if (IsDragging == false)
				return;
			IsDragging = false;

			PicketLiner targetPicketLiner = GetClosestCollidingPicketLiner();
			if (targetPicketLiner != null)
			{
				foreach(PicketLiner picketLiner in CollidingPicketLiners)
				{
					picketLiner.ModelSelector.PlayIdleAnimation();
				}
				targetPicketLiner.UpgradeRank();
				FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/liner_grow");
                DestroyPicketLiner();
			}
			else
			{
				Parent.ModelSelector.PlayIdleAnimation();
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
					PicketLiner closestPicketLiner = GetClosestCollidingPicketLiner();
					foreach(PicketLiner picketLiner in CollidingPicketLiners)
					{
						if (picketLiner == closestPicketLiner)
						{
							DisplayPicketLinerReadyToMerge(picketLiner);
						}
						else
						{
							picketLiner.DisplayMergeSprite(false);
							picketLiner.ModelSelector.PlayIdleAnimation();
						}
					}
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
						Parent.ModelSelector.PlayHoverAnimation();
					}
					else
					{
						PicketLiner closestPicketLiner = GetClosestCollidingPicketLiner();
						if (closestPicketLiner != null)
							DisplayPicketLinerReadyToMerge(closestPicketLiner);
					}
				}
			}
			else
			{
				if (InvalidCollisionObjects.Contains(collision.transform) == true)
					InvalidCollisionObjects.Remove(collision.transform);
			}
		}

		private PicketLiner GetClosestCollidingPicketLiner()
		{
			List<PicketLiner> eligiblePicketLiners = CollidingPicketLiners.Where(pl => pl.CanBeUpgraded == true).ToList();
			if (eligiblePicketLiners.Any() == false)
				return null;
			if (eligiblePicketLiners.Count == 1)
				return eligiblePicketLiners.First();
			List<PicketLiner> sortedPicketLiners = eligiblePicketLiners.OrderBy(pl => Vector3.Distance(transform.position, pl.transform.position)).ToList();
			return sortedPicketLiners.First();
		}

		private void DisplayPicketLinerReadyToMerge(PicketLiner picketLiner)
		{
			picketLiner.DisplayMergeSprite(true);
			picketLiner.ModelSelector.PlayConnectFrontAnimation();
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
