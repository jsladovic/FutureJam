using Assets.Scripts.General;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PicketLiners
{
	public class DragController : MonoBehaviour
    {
        private static string[] ValidLayerNames = new string[] { "PicketLinerDraggable" };

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

        private void OnMouseDown()
        {
            if (GameController.Instance.IsWaitingForUpgrade == true || IsDragging == true)
                return;
            DragStartPosition = transform.position;
            MousePositionOffset = transform.position - MouseWorldPosition;
            IsDragging = true;
            InvalidCollisionObjects = new List<Transform>();
        }

        private void OnMouseUp()
        {
            if (IsDragging == false)
                return;
            IsDragging = false;
        }

        private void OnMouseDrag()
        {
            transform.position = MouseWorldPosition + MousePositionOffset;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, MinX, MaxX),
                Mathf.Clamp(transform.position.y, MinY, MaxY), transform.position.z);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsDragging == false)
                return;
            InvalidCollisionObjects.Add(collision.transform);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (IsDragging == false)
                return;

            InvalidCollisionObjects.Remove(collision.transform);
        }

        private Vector3 MouseWorldPosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
