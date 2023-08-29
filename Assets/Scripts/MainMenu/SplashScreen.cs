using Assets.Scripts.Enums;
using Assets.Scripts.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Assets.Scripts.MainMenu
{
	public class SplashScreen : MonoBehaviour
    {
        private const float FadeDuration = 1.0f;

        [SerializeField] private VideoPlayer VideoPlayer;
        [SerializeField] private CanvasGroup Background;

		private void Awake()
		{
            Background.Disable();
            VideoPlayer.loopPointReached += OnVideoOver;
        }

        private void OnVideoOver(VideoPlayer _)
        {
            Background.FadeIn(FadeDuration, false, () => SceneManager.LoadScene((int)SceneBuildIndex.MainMenu));
        }
    }
}
