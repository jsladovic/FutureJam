using UnityEngine;
using System.Collections;
using System.Linq;

public class ScabMovement : MonoBehaviour
{
    private Scab Parent;
    private MovementCurve Curve;

    [SerializeField]
    [Range(1, 5)]
    private float Speed;

    public MovementState State { get; set; }
    public object OnBuildingEndtered { get; internal set; }

    private Vector3? TargetPosition;
    private const float DistanceToPointThreshold = 0.1f;

    public void Initialize(Scab parent, MovementCurve curve)
    {
        Parent = parent;
        State = MovementState.Entering;
        Curve = curve;
        Speed = 2.0f;
        transform.position = curve.Points[0].transform.position;
    }

    void Update()
    {
        if (Curve == null)
            return;
        switch (State)
        {
            case MovementState.Entering:
                HandleEntering();
                return;
            case MovementState.Entered:
            case MovementState.Leaving:
                HandleMovementToTargetPosition();
                return;
        }
    }

    private void HandleEntering()
    {
        MoveTowardsPoint(Curve.Points[1].transform.position);
    }

    private void HandleMovementToTargetPosition()
    {
        if (TargetPosition.HasValue == false)
            TargetPosition = ExitPointsController.Instance.GetNearestExitPoint(transform.position);
        MoveTowardsPoint(TargetPosition.Value);
        if (Vector3.Distance(transform.position, TargetPosition.Value) <= DistanceToPointThreshold)
        {
            Destroy(Parent.gameObject);
            GameController.Instance.OnScabDestroyed();
        }
    }

    public void OnBuildingEntered(Vector3 doorPosition)
    {
        State = MovementState.Entered;
        TargetPosition = doorPosition;
    }

    private void MoveTowardsPoint(Vector3 point)
    {
        transform.position = Vector3.MoveTowards(transform.position, point, Time.deltaTime * Speed);
    }
}
