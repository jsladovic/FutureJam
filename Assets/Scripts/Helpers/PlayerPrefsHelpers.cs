using UnityEngine;

namespace Assets.Scripts.Helpers
{
	public static class PlayerPrefsHelpers
	{
		private const string TutorialDisplayedKey = "TutorialDisplayed";
		private const string GameCompltedKey = "GameCompleted";
		private const string MaxLevelCompletedKey = "MaxLevelCompleted";

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

		public static int GetMaxLevelCompleted()
		{
			return PlayerPrefs.GetInt(MaxLevelCompletedKey, 0);
		}

		public static void SetMaxLevelCompleted(int level)
		{
			PlayerPrefs.SetInt(MaxLevelCompletedKey, level);
		}
	}
}
