using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    public static CanvasController Instance;

    [SerializeField] private GameObject ButtonsParent;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private Button AddPicketLinerButton;
    [SerializeField] private Button KickOutScabButton;
    [SerializeField] private GameObject EndGameScreen;
    [SerializeField] private TextMeshProUGUI EndGameText;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        ButtonsParent.gameObject.SetActive(false);
        EndGameScreen.SetActive(false);
    }

    public void DisplayLevel(int levelIndex, bool canKickOutScab)
    {
        LevelText.text = $"Day {levelIndex}";
        if (levelIndex == 1)
        {
            GameController.Instance.StartLevel();
            return;
        }
        ButtonsParent.gameObject.SetActive(true);
        AddPicketLinerButton.gameObject.SetActive(levelIndex % 3 == 1);
        KickOutScabButton.gameObject.SetActive(canKickOutScab);
    }

    public void KickOutScabClicked()
    {
        GameController.Instance.KickOutScab();
        GameController.Instance.StartLevel();
        ButtonsParent.gameObject.SetActive(false);
    }

    public void LevelUpPicketLinerClicked()
    {
        GameController.Instance.LevelUpPicketLiner();
        ButtonsParent.gameObject.SetActive(false);
    }

    public void AddPicketLinerClicked()
    {
        GameController.Instance.SpawnPicketLiner();
        GameController.Instance.StartLevel();
        ButtonsParent.gameObject.SetActive(false);
    }

    public void DisplayEndGameScreen(int numberOfDays)
    {
        EndGameText.text = $"The strike lasted {numberOfDays} days";
        EndGameScreen.SetActive(true);
    }

    public void RestartClicked()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}
