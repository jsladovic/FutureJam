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

		private void Awake()
		{
			MainMenu = GetComponent<CanvasGroup>();
			MainMenu.Enable();
			OptionsMenu.Disable();
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
