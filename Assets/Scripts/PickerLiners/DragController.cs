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
			print($"on mouse down {name}");
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
			print($"on mouse up {name}");
			if (IsDragging == false)
				return;
			IsDragging = false;

			PicketLiner targetPicketLiner = CollidingPicketLiners.FirstOrDefault(pl => pl.CanBeUpgraded == true);
			if (targetPicketLiner != null)
			{
				targetPicketLiner.UpgradeRank();
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/liner_grow");
                DestroyPicketLiner();
			}
			FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/liner_drop");
		}

		private void OnMouseDrag()
		{
			print($"on mouse drag {name}");
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
			if (collidingPicketLiner != null)
			{
				if (CollidingPicketLiners.Contains(collidingPicketLiner) == false)
				{
					print($"{name} entering collision with {collision.name}");
					CollidingPicketLiners.Add(collidingPicketLiner);
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
					print($"{name} exiting collision with {collision.name}");
					CollidingPicketLiners.Remove(collidingPicketLiner);
				}
			}
			else
			{
				if (InvalidCollisionObjects.Contains(collision.transform) == true)
					InvalidCollisionObjects.Remove(collision.transform);
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
			Destroy(gameObject);
		}
	}
}
