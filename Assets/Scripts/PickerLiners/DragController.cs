using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DragController : MonoBehaviour
{
    private static string[] InvalidLayerNames = new string[] { "Factory", "Entrance", "Scab" };

    private Vector3 MousePositionOffset;
    private Vector3 DragStartPosition;
    private bool IsDragging;
    private bool CanBeDropped;

    private void Awake()
    {
        IsDragging = false;
        CanBeDropped = false;
    }

    public void OnMouseDown()
    {
        print("on mouse down");
        DragStartPosition = transform.position;
        MousePositionOffset = transform.position - MouseWorldPosition;
        IsDragging = true;
        CanBeDropped = true;
    }

    public void OnMouseUp()
    {
        if (IsDragging == false)
            return;
        if (CanBeDropped == false)
            transform.position = DragStartPosition;
        print("on mouse up");
        IsDragging = false;
        CanBeDropped = false;
    }

    public void OnMouseDrag()
    {
        transform.position = MouseWorldPosition + MousePositionOffset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsInvalidCollision(collision.gameObject) == false)
            return;
        print($"entering collision with {collision.name}");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsInvalidCollision(collision.gameObject) == false)
            return;
        print($"exiting collision with {collision.name}");
    }

    private bool IsInvalidCollision(GameObject otherObject) => InvalidLayerNames.Contains(LayerMask.LayerToName(otherObject.layer));

    private Vector3 MouseWorldPosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);
}
