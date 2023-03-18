using System;
using UnityEngine;

[RequireComponent(typeof(ScabMovement))]
public class Scab : MonoBehaviour
{
    public bool HasEnteredBuilding { get; private set; }
    public bool IsLeaving { get; private set; }

    private void Awake()
    {
        GetComponent<ScabMovement>().Initialize(this);
        HasEnteredBuilding = false;
        IsLeaving = false;
    }

    public void EnterBuilding()
    {
        print("scab entered building");
        HasEnteredBuilding = true;
    }

    public void Leave()
    {
        IsLeaving = true;
    }
}
