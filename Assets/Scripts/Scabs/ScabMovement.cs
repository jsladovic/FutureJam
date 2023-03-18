using UnityEngine;
using System.Collections;
using System.Linq;

public class ScabMovement : MonoBehaviour
{
    private Scab Parent;

    [SerializeField] private MovementCurve Curve;
    private MovementState State;
    private float InterpolateAmount;

    public void Initialize(Scab parent)
    {
        Parent = parent;
        State = MovementState.Entering;
    }

    void Update()
    {
        switch (State)
        {
            case MovementState.Entering:
                HandleEntering();
                return;
            case MovementState.Entered:
                HandleEntered();
                return;
            case MovementState.Leaving:
                HandleLeaving();
                return;
        }
    }

    private void HandleEntering()
    {
        InterpolateAmount = (InterpolateAmount + Time.deltaTime);
        transform.position = Vector3.Lerp(Curve.Points[0].transform.position, Curve.Points[1].transform.position, InterpolateAmount);
        if (InterpolateAmount == 1.0f)
        {
            State = MovementState.Leaving;
            throw new UnityException("Scab reached end of curve, but hasn't entered the building");
        }
    }

    private void HandleEntered()
    {

    }

    private void HandleLeaving()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print($"scab trigger entered {collision.name}");
        if (CheckForTriggers == false)
            return;

        Entrance entrance = collision.GetComponent<Entrance>();
        if (entrance != null)
        {
            Parent.EnterBuilding();
            State = MovementState.Entered;
        }
    }

    private bool CheckForTriggers => Parent.HasEnteredBuilding == false && Parent.IsLeaving == false;

    private enum MovementState
    {
        Entering = 0,
        Entered = 1,
        Leaving = 2,
    }
}
