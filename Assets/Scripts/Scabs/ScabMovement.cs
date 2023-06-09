﻿using Assets.Scripts.GameEvents.Events;
using Assets.Scripts.General;
using UnityEngine;

namespace Assets.Scripts.Scabs
{
	public class ScabMovement : MonoBehaviour
	{
		[SerializeField] private VoidEvent OnScabDestroyed;
		[SerializeField] private VoidEvent OnScabEntered;

		private Scab Parent;
		private MovementCurve Curve;

		private bool UseLeavingSpeed;
		private float Speed;
		private float LeavingSpeed;

		public MovementState State { get; set; }
		public object OnBuildingEndtered { get; internal set; }

		private Vector3? TargetPosition;
		private const float DistanceToPointThreshold = 0.1f;
		private const float LeavingSpeedMultiplier = 1.25f;

		public void Initialize(Scab parent, MovementCurve curve, float speed)
		{
			Parent = parent;
			State = MovementState.Entering;
			Curve = curve;
			Speed = speed;
			LeavingSpeed = speed * LeavingSpeedMultiplier;
			transform.position = curve.Points[0].transform.position;
			UseLeavingSpeed = false;
		}

		void Update()
		{
			if (Curve == null)
				return;
			switch (State)
			{
				case MovementState.Entering:
					HandleEntering();
					return;
				case MovementState.Entered:
				case MovementState.Leaving:
					HandleMovementToTargetPosition(State == MovementState.Entered);
					return;
			}
		}

		private void HandleEntering()
		{
			MoveTowardsPoint(Curve.Points[1].transform.position);
		}

		private void HandleMovementToTargetPosition(bool entering)
		{
			UseLeavingSpeed = true;
			if (TargetPosition.HasValue == false)
				TargetPosition = ExitPointsController.Instance.GetNearestExitPoint(transform.position);
			MoveTowardsPoint(TargetPosition.Value);
			if (Vector3.Distance(transform.position, TargetPosition.Value) <= DistanceToPointThreshold)
			{
				if (entering == true)
					OnScabEntered.Raise();
				OnScabDestroyed.Raise();
				Destroy(Parent.gameObject);
			}
		}

		public void OnBuildingEntered(Vector3 doorPosition)
		{
			State = MovementState.Entered;
			TargetPosition = doorPosition;
		}

		private void MoveTowardsPoint(Vector3 point)
		{
			float speed = UseLeavingSpeed ? LeavingSpeed : Speed;
			transform.position = Vector3.MoveTowards(transform.position, point, Time.deltaTime * speed);
		}
	}
}
