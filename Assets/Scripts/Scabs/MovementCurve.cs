using UnityEngine;
using System.Collections;

public class MovementCurve : MonoBehaviour
{
    public MovementCurvePoint[] Points { get; private set; }

    private void Awake()
    {
        Points = GetComponentsInChildren<MovementCurvePoint>();
        if (Points.Length != 2)
            throw new UnityException($"Expected 2 points, got {Points.Length}");
    }
}

