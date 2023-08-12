using Assets.Scripts.Enums;
using Assets.Scripts.Extensions;
using Assets.Scripts.GameEvents.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.MainMenu
{
	[RequireComponent(typeof(CanvasGroup))]
	public class MainMenuController : MonoBehaviour
	{
		private CanvasGroup CanvasGroup;
		[SerializeField] private CanvasGroup MainMenu;

		[SerializeField] private OptionsMenuController OptionsMenu;
		[SerializeField] private VoidEvent OnMainMenuLoaded;

		private void Awake()
		{
			CanvasGroup = GetComponent<CanvasGroup>();
			CanvasGroup.Disable();
			MainMenu.Enable();
			CanvasGroup.FadeIn(1.0f, immediatelyInteractible: true, setOnComplete: () => OnMainMenuLoaded.Raise());
		}

		public void StartClicked()
		{
			CanvasGroup.FadeOut(1.0f, setOnComplete: () => SceneManager.LoadScene((int)SceneBuildIndex.Game));
		}

		public void OptionsClicked()
		{
			MainMenu.Disable();
			OptionsMenu.Enable();
		}

		public void CreditsCLicked()
		{
			MainMenu.Disable();
		}

		public void BackClicked()
		{
			MainMenu.Enable();
			OptionsMenu.Disable();
		}

		public void ExitClicked()
		{
			Application.Quit();
		}
	}
}
