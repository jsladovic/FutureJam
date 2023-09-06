using UnityEngine;

namespace Assets.Scripts.Extensions
{
	public static class GameObjectExtensions
	{
		public static void HideForMobile(this GameObject gameObject)
		{
			bool shouldDisplay = true;
#if UNITY_ANDROID
			shouldDisplay = false;
#elif UNITY_STANDALONE_OSX
			shouldDisplay = false;
#endif
			gameObject.SetActive(shouldDisplay);
		}
	}
}
