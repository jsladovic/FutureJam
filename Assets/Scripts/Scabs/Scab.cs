using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScabMovement))]
public class Scab : MonoBehaviour
{
    public bool HasEnteredBuilding { get; private set; }
    public bool IsLeaving { get; private set; }

    private ScabMovement ScabMovement;
    private Rigidbody2D Rigidbody;
    private ScabRank Rank;

    private List<SphereOfInfluence> CollidedSpheresOfInfluence;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        CollidedSpheresOfInfluence = new List<SphereOfInfluence>();
        ScabMovement = GetComponent<ScabMovement>();
    }

    public void Initialize(ScabRank rank, MovementCurve curve, float speed)
    {
        Rank = rank;
        ScabMovement.Initialize(this, curve, speed);
        HasEnteredBuilding = false;
        IsLeaving = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CheckForTriggers == false)
            return;

        PicketLiner picketLiner = collision.transform.GetComponent<PicketLiner>();
        if (picketLiner != null && picketLiner.IsCarried == false)
        {
            CollidedWithPicketLiner();
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckForTriggers == false)
            return;

        Entrance entrance = collision.GetComponent<Entrance>();
        if (entrance != null)
        {
            EnterBuilding(entrance);
            return;
        }

        SphereOfInfluence sphereOfInfluence = collision.GetComponent<SphereOfInfluence>();
        if (sphereOfInfluence != null)
        {
            HandleSphereOfInfulenceCollisionEntered(sphereOfInfluence);
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SphereOfInfluence sphereOfInfluence = collision.GetComponent<SphereOfInfluence>();
        if (sphereOfInfluence != null)
        {
            HandleSphereOfInfulenceCollisionExited(sphereOfInfluence);
            return;
        }
    }

    private void HandleSphereOfInfulenceCollisionEntered(SphereOfInfluence sphereOfInfluence)
    {
        if (CollidedSpheresOfInfluence.Contains(sphereOfInfluence) == true)
            return;
        if (sphereOfInfluence.IsCarried == true)
            return;
        CollidedSpheresOfInfluence.Add(sphereOfInfluence);
        if (TotalSpheresOfInfluence >= Rank.SpheresOfInfulenceNeededToLeave())
            Leave();
    }

    private void HandleSphereOfInfulenceCollisionExited(SphereOfInfluence sphereOfInfluence)
    {
        if (CollidedSpheresOfInfluence.Contains(sphereOfInfluence) == false)
            return;
        CollidedSpheresOfInfluence.Remove(sphereOfInfluence);
    }

    private void EnterBuilding(Entrance entrance)
    {
        Destroy(Rigidbody);
        Rigidbody = null;
        HasEnteredBuilding = true;
        ScabMovement.OnBuildingEntered(entrance.DoorPosition);
        GameController.Instance.OnScabEntered();
    }

    private void CollidedWithPicketLiner()
    {
        Leave();
    }

    public void Leave()
    {
        Destroy(Rigidbody);
        Rigidbody = null;
        ScabMovement.State = MovementState.Leaving;
        IsLeaving = true;
    }

    private int TotalSpheresOfInfluence => CollidedSpheresOfInfluence.Count;

    private bool CheckForTriggers => HasEnteredBuilding == false && IsLeaving == false;
}
