using UnityEngine;

public class Entrance : MonoBehaviour
{
    [SerializeField] private Door Door;

    public Vector3 DoorPosition => Door.transform.position;
}
