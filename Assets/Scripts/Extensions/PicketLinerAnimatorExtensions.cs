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

		public static string PlayAnimationWithName(this Animator animator, string animationName)
		{
			animator.Play(animationName);
			return animationName;
		}

		public static string PlayIdleAnimation(this Animator animator)
		{
			return animator.PlayAnimationWithName(IdleAnimationName);
		}

		public static string PlayHappyAnimation(this Animator animator)
		{
			return animator.PlayAnimationWithName(HappyAnimationName);
		}

		public static string PlaySadAnimation(this Animator animator)
		{
			return animator.PlayAnimationWithName(SadAnimationName);
		}

		public static string PlayConnectBackAnimation(this Animator animator)
		{
			return animator.PlayAnimationWithName(ConnectBackAnimationName);
		}

		public static string PlayConnectFrontAnimation(this Animator animator)
		{
			return animator.PlayAnimationWithName(ConnectFrontAnimationName);
		}
	}
}
