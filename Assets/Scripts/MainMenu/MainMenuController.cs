using Assets.Scripts.Enums;
using Assets.Scripts.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.MainMenu
{
	[RequireComponent(typeof(CanvasGroup))]
	public class MainMenuController : MonoBehaviour
	{
		private CanvasGroup MainMenu;
		[SerializeField] private CanvasGroup OptionsMenu;
		[SerializeField] private CanvasGroup CreditsMenu;

		private void Awake()
		{
			MainMenu = GetComponent<CanvasGroup>();
			MainMenu.Enable();
			OptionsMenu.Disable();
			CreditsMenu.Disable();
		}

		public void StartClicked()
		{
			SceneManager.LoadScene((int)SceneBuildIndex.Game);
		}

		public void OptionsClicked()
		{
			MainMenu.Disable();
			OptionsMenu.Enable();
		}

		public void CreditsCLicked()
		{
			MainMenu.Disable();
			CreditsMenu.Enable();
		}

		public void BackClicked()
		{
			MainMenu.Enable();
			CreditsMenu.Disable();
			OptionsMenu.Disable();
		}

		public void ExitClicked()
		{
			Application.Quit();
		}
	}
}
