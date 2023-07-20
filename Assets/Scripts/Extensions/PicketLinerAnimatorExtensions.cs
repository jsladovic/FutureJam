using UnityEngine;

namespace Assets.Scripts.Extensions
{
	public static class PicketLinerAnimatorExtensions
	{
		private const string IdleAnimationName = "Idle";
		private const string HappyAnimationName = "Happy";
		private const string SadAnimationName = "Sad";
		private const string ConnectBackAnimationName = "ConnectBack";
		private const string ConnectFrontAnimationName = "ConnectFront";

		public static void PlayIdleAnimation(this Animator animator)
		{
			animator.Play(IdleAnimationName);
		}

		public static void PlayHappyAnimation(this Animator animator)
		{
			animator.Play(HappyAnimationName);
		}

		public static void PlaySadAnimation(this Animator animator)
		{
			animator.Play(SadAnimationName);
		}

		public static void PlayConnectBackAnimation(this Animator animator)
		{
			animator.Play(ConnectBackAnimationName);
		}

		public static void PlayConnectFrontAnimation(this Animator animator)
		{
			animator.Play(ConnectFrontAnimationName);
		}
	}
}
