using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DragController : MonoBehaviour
{
    private static string[] InvalidLayerNames = new string[] { "Factory", "Entrance", "Scab" };

    public bool IsDragging { get; private set; }

    private Vector3 MousePositionOffset;
    private Vector3 DragStartPosition;

    private List<Transform> InvalidCollisionObjects;

    private void Awake()
    {
        IsDragging = false;
    }

    private void OnMouseDown()
    {
        DragStartPosition = transform.position;
        MousePositionOffset = transform.position - MouseWorldPosition;
        IsDragging = true;
        InvalidCollisionObjects = new List<Transform>();
    }

    private void OnMouseUp()
    {
        if (IsDragging == false)
            return;
        if (CanBeDropped == false)
            transform.position = DragStartPosition;
        IsDragging = false;
    }

    private void OnMouseDrag()
    {
        transform.position = MouseWorldPosition + MousePositionOffset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsDragging == false)
            return;
        if (IsInvalidCollision(collision.gameObject) == false)
            return;
        if (InvalidCollisionObjects.Contains(collision.transform) == true)
            return;
        InvalidCollisionObjects.Add(collision.transform);
        print($"entering collision with {collision.name}");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsDragging == false)
            return;
        if (IsInvalidCollision(collision.gameObject) == false)
            return;
        if (InvalidCollisionObjects.Contains(collision.transform) == false)
            return;
        InvalidCollisionObjects.Remove(collision.transform);
        print($"exiting collision with {collision.name}");
    }

    private bool CanBeDropped => InvalidCollisionObjects.Any() == false;

    private bool IsInvalidCollision(GameObject otherObject) => InvalidLayerNames.Contains(LayerMask.LayerToName(otherObject.layer));

    private Vector3 MouseWorldPosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);
}
