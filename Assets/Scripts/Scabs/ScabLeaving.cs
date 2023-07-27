using Assets.Scripts.General;
using UnityEngine;

namespace Assets.Scripts.Scabs
{
	public class ScabLeaving : MonoBehaviour
	{
		private Vector3? TargetPosition;
		private const float Speed = 2.0f;
		private const float DistanceToPointThreshold = 0.1f;

		private void Awake()
		{
			TargetPosition = ExitPointsController.Instance.GetThrownOutExitPoint();
		}

		private void Update()
		{
			if (TargetPosition.HasValue == false)
				return;

			MoveTowardsPoint(TargetPosition.Value);
			if (Vector3.Distance(transform.position, TargetPosition.Value) <= DistanceToPointThreshold)
			{
				GameController.Instance.StartLevel();
				Destroy(gameObject);
			}
		}

		private void MoveTowardsPoint(Vector3 point)
		{
			transform.position = Vector3.MoveTowards(transform.position, point, Time.deltaTime * Speed);
		}
	}
}
