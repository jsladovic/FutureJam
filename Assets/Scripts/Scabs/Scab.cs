using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScabMovement))]
public class Scab : MonoBehaviour
{
    public bool HasEnteredBuilding { get; private set; }
    public bool IsLeaving { get; private set; }

    private ScabMovement ScabMovement;
    private ScabRank Rank;

    private List<SphereOfInfluence> CollidedSpheresOfInfluence;

    private void Start()
    {
        CollidedSpheresOfInfluence = new List<SphereOfInfluence>();
        ScabMovement = GetComponent<ScabMovement>();
        ScabMovement.Initialize(this);
        HasEnteredBuilding = false;
        IsLeaving = false;
        Rank = ScabRank.Basic;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CheckForTriggers == false)
            return;

        PicketLiner picketLine = collision.transform.GetComponent<PicketLiner>();
        if (picketLine != null)
        {
            CollidedWithPicketLiner();
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print($"scab trigger entered {collision.name}");
        if (CheckForTriggers == false)
            return;

        Entrance entrance = collision.GetComponent<Entrance>();
        if (entrance != null)
        {
            EnterBuilding();
            return;
        }

        SphereOfInfluence sphereOfInfluence = collision.GetComponent<SphereOfInfluence>();
        if (sphereOfInfluence != null)
        {
            HandleSphereOfInfulenceCollision(sphereOfInfluence);
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

    private void HandleSphereOfInfulenceCollision(SphereOfInfluence sphereOfInfluence)
    {
        print($"scab sphere of infuelnce entered");
        if (CollidedSpheresOfInfluence.Contains(sphereOfInfluence) == true)
            return;
        CollidedSpheresOfInfluence.Add(sphereOfInfluence);
        if (TotalSpheresOfInfluence >= Rank.SpheresOfInfulenceNeededToLeave())
            Leave();
    }

    private void HandleSphereOfInfulenceCollisionExited(SphereOfInfluence sphereOfInfluence)
    {
        print($"scab sphere of infuelnce exited");
        if (CollidedSpheresOfInfluence.Contains(sphereOfInfluence) == false)
            return;
        CollidedSpheresOfInfluence.Remove(sphereOfInfluence);
    }

    private void EnterBuilding()
    {
        print("scab entered building");
        HasEnteredBuilding = true;
        ScabMovement.State = MovementState.Entered;
    }

    private void CollidedWithPicketLiner()
    {
        Leave();
    }

    public void Leave()
    {
        print("scab is leaving");
        ScabMovement.State = MovementState.Leaving;
        IsLeaving = true;
    }

    private int TotalSpheresOfInfluence => CollidedSpheresOfInfluence.Count;

    private bool CheckForTriggers => HasEnteredBuilding == false && IsLeaving == false;
}
