using System.Linq;
using UnityEngine;

namespace Assets.Scripts.General
{
	public class ExitPointsController : MonoBehaviour
	{
		public static ExitPointsController Instance;
		private ExitPoint[] ExitPoints;
		[SerializeField] private ExitPoint ThrownOutExitPoint;

		private void Awake()
		{
			Instance = this;
			ExitPoints = GetComponentsInChildren<ExitPoint>();
			if (ExitPoints == null || ExitPoints.Any() == false)
				throw new UnityException("No exit points found");
		}

		public Vector3 GetNearestExitPoint(Vector3 currentPosition)
		{
			return ExitPoints.OrderBy(ep => Vector3.Distance(ep.transform.position, currentPosition)).First().transform.position;
		}

		public Vector3 GetThrownOutExitPoint() => ThrownOutExitPoint.transform.position;
	}
}
