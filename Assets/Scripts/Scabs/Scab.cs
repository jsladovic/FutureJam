using Assets.Scripts.GameEvents.Events;
using Assets.Scripts.PicketLiners;
using UnityEngine;

namespace Assets.Scripts.Scabs
{
	[RequireComponent(typeof(ScabMovement))]
	public class Scab : MonoBehaviour
	{
		[SerializeField] private VoidEvent OnScabEntering;

		public bool HasEnteredBuilding { get; private set; }
		public bool IsLeaving { get; private set; }

		private ScabMovement ScabMovement;
		private ScabModelSelector ModelSelector;
		private Rigidbody2D Rigidbody;

		private ScabRank rank;
		private ScabRank Rank
		{
			get { return rank; }
			set
			{
				rank = value;
			}
		}

		private void Awake()
		{
			Rigidbody = GetComponent<Rigidbody2D>();
			ScabMovement = GetComponent<ScabMovement>();
		}

		public void Initialize(ScabRank rank, MovementCurve curve, float speed)
		{
			Rank = rank;
			ModelSelector = GetComponentInChildren<ScabModelSelector>();
			ModelSelector.SetSprite(rank, false);
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
				CollidedWithPicketLiner(picketLiner);
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

			PicketLiner picketLiner = collision.transform.GetComponent<PicketLiner>();
			if (picketLiner != null)
			{
				CollidedWithPicketLiner(picketLiner);
				return;
			}
			SphereOfInfluence sphereOfInfluence = collision.GetComponent<SphereOfInfluence>();
			if (sphereOfInfluence != null)
			{
				CollidedWithPicketLiner(sphereOfInfluence.Parent);
				return;
			}
		}

		private void EnterBuilding(Entrance entrance)
		{
			Destroy(Rigidbody);
			Rigidbody = null;
			HasEnteredBuilding = true;
			ScabMovement.OnBuildingEntered(entrance.DoorPosition);
			OnScabEntering.Raise();
		}

		private void CollidedWithPicketLiner(PicketLiner picketLiner)
		{
			if (picketLiner == null || picketLiner.IsCarried == true)
				return;
			if ((int)picketLiner.Rank >= (int)Rank)
				Leave();
		}

		public void Leave()
		{
			Destroy(Rigidbody);
			Rigidbody = null;
			ScabMovement.State = MovementState.Leaving;
			IsLeaving = true;
			ModelSelector.SetSprite(Rank, true);
		}

		private bool CheckForTriggers => HasEnteredBuilding == false && IsLeaving == false;
	}
}
