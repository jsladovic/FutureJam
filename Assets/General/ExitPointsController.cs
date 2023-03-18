using UnityEngine;
using System.Collections;
using System.Linq;

public class ExitPointsController : MonoBehaviour
{
    private ExitPoint[] ExitPoints;

    private void Awake()
    {
        ExitPoints = GetComponentsInChildren<ExitPoint>();
        if (ExitPoints == null || ExitPoints.Any() == false)
            throw new UnityException("No exit points found");
    }

    public void GetNearestExitPoint(Vector3 currentPosition)
    {

    }
}

