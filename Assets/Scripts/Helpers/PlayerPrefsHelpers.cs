using UnityEngine;

namespace Assets.Scripts.Helpers
{
	public static class PlayerPrefsHelpers
	{
		private const string TutorialDisplayedKey = "TutorialDisplayed";
		private const string GameCompltedKey = "GameCompleted";

		public static bool WasTutorialDisplayed()
		{
			return PlayerPrefs.GetInt(TutorialDisplayedKey, 0) == 1;
		}

		public static void SetTutorialDisplayed()
		{
			PlayerPrefs.SetInt(TutorialDisplayedKey, 1);
		}

		public static bool IsGameCompleted()
		{
			return PlayerPrefs.GetInt(GameCompltedKey, 0) == 1;
		}

		public static void SetGameCompleted()
		{
			PlayerPrefs.SetInt(GameCompltedKey, 1);
		}
	}
}
