using UnityEngine;
using System.Collections;
using System.Linq;

public class ScabMovement : MonoBehaviour
{
    private Scab Parent;

    [SerializeField] private MovementCurve Curve;

    [SerializeField]
    [Range(1, 5)]
    private float Speed;

    public MovementState State { get; set; }
    private float InterpolateAmount;

    public void Initialize(Scab parent)
    {
        Parent = parent;
        State = MovementState.Entering;
        transform.position = Curve.Points[0].transform.position;
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
        transform.position = Vector3.MoveTowards(transform.position, Curve.Points[1].transform.position, Time.deltaTime * Speed);
    }

    private void HandleEntered()
    {

    }

    private void HandleLeaving()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
}
