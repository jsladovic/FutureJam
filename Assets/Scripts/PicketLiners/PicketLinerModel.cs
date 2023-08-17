using Assets.Scripts.Extensions;
using Assets.Scripts.General;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.PicketLiners
{
	[RequireComponent(typeof(SpriteRenderer))]
	[RequireComponent(typeof(Animator))]
	public class PicketLinerModel : MonoBehaviour
	{
		private Animator Animator;
		private ModelSelector Parent;

		[SerializeField] private SpriteRenderer MergeSpriteRenderer;
		[SerializeField] private Transform ClickingPointsParent;

		public Transform[] ClickingPoints { get; set; }

		private void OnEnable()
		{
			if (Parent != null && string.IsNullOrEmpty(Parent.CurrentAnimationName) == false)
			{
				Animator.PlayAnimationWithName(Parent.CurrentAnimationName);
			}
		}

		public void Initialize(ModelSelector parent)
		{
			Parent = parent;
			Animator = GetComponent<Animator>();
			DisplayMergeSprite(false);
			ClickingPoints = ClickingPointsParent.GetComponentsInChildren<Transform>().Where(t => t != transform).ToArray();
		}

		public void DisplayMergeSprite(bool isVisible)
		{
			MergeSpriteRenderer.gameObject.SetActive(isVisible);
		}

		public Vector3 GetClosestClickingPoint()
		{
			if (ClickingPoints.Length == 0)
				throw new UnityException("No clicking points found");
			if (ClickingPoints.Length == 1)
				return ClickingPoints[0].position;

			Vector2 currentMousePosition = Camera.main.MouseWorldPosition();
			Vector3 closestClickingPoint = ClickingPoints[0].position;
			float minDistance = Vector3.Distance(currentMousePosition, closestClickingPoint);

			for (int i = 1; i < ClickingPoints.Length; i++)
			{
				Vector3 position = ClickingPoints[i].position;
				float distance = Vector3.Distance(currentMousePosition, position);
				if (distance < minDistance)
				{
					minDistance = distance;
					closestClickingPoint = position;
				}
			}

			return closestClickingPoint;
		}

		public void OnLevelStarted(LevelDefinition _)
		{
			PlayIdleAnimation();
		}

		public void PlayIdleAnimation()
		{
			PlayAnimation(Animator.PlayIdleAnimation);
		}

		public void OnLevelComplete()
		{
			PlayAnimation(Animator.PlayHappyAnimation);
		}

		public void OnGameOver(int _)
		{
			PlayAnimation(Animator.PlaySadAnimation);
		}

		public void PlayConnectBackAnimation()
		{
			PlayAnimation(Animator.PlayConnectBackAnimation);
		}

		public void PlayConnectFrontAnimation()
		{
			PlayAnimation(Animator.PlayConnectFrontAnimation);
		}

		public void PlayHoverAnimation()
		{
			PlayAnimation(Animator.PlayHoverAnimation);
		}

		private void PlayAnimation(Func<string> action)
		{
			string animationName = action();
			Parent.SetCurrentAnimationName(animationName);
		}
	}

}
