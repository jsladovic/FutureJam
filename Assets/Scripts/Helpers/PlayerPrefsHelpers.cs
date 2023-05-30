using UnityEngine;

namespace Assets.Scripts.Helpers
{
	public static class PlayerPrefsHelpers
	{
		private const string TutorialDisplayedKey = "TutorialDisplayed";

		public static bool WasTutorialDisplayed()
		{
			return PlayerPrefs.GetInt(TutorialDisplayedKey, 0) == 1;
		}

		public static void SetTutorialDisplayed()
		{
			PlayerPrefs.SetInt(TutorialDisplayedKey, 1);
		}
	}
}
