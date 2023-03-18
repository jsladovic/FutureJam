using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public static CanvasController Instance;

    [SerializeField] private GameObject ButtonsParent;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private Button AddPicketLinerButton;
    [SerializeField] private Button KickOutScabButton;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        ButtonsParent.gameObject.SetActive(false);
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
        AddPicketLinerButton.enabled = levelIndex % 3 == 0;
        KickOutScabButton.enabled = canKickOutScab;
    }

    public void KickOutScabClicked()
    {
        GameController.Instance.StartLevel();
        ButtonsParent.gameObject.SetActive(false);
    }

    public void LevelUpPicketLinerClicked()
    {
        GameController.Instance.StartLevel();
        ButtonsParent.gameObject.SetActive(false);
    }

    public void AddPicketLinerClicked()
    {
        GameController.Instance.StartLevel();
        ButtonsParent.gameObject.SetActive(false);
    }
}
