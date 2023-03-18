using UnityEngine;
using System.Collections;

public class MovementCurve : MonoBehaviour
{
    public MovementCurvePoint[] Points { get; private set; }

    private void Awake()
    {
        Points = GetComponentsInChildren<MovementCurvePoint>();
    }
}

