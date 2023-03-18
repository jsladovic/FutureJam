using UnityEngine;
using UnityEngine.UI;

public class ClockController : MonoBehaviour
{
    [SerializeField] private Image ClockHand;

    private const int StartingHours = 7;

    private float currentTime;
    private float CurrentTime
    {
        get { return currentTime; }
        set
        {
            currentTime = value;
            DisplayTime();
        }
    }

    private float TimeRemainingInLevel;
    private bool IsRunning;

    private void Update()
    {
        if (IsRunning == false)
            return;
        TimeRemainingInLevel -= Time.deltaTime;
        CurrentTime = StartingHours + ((GameController.LevelDurationSeconds - TimeRemainingInLevel) / GameController.LevelDurationSeconds) * 12.0f;
        if (TimeRemainingInLevel <= 0)
        {
            IsRunning = false;
        }
    }

    public void StartLevel()
    {
        CurrentTime = StartingHours;
        TimeRemainingInLevel = GameController.LevelDurationSeconds;
        IsRunning = true;
    }

    private void DisplayTime()
    {
        print($"current time: {CurrentTime}");
        ClockHand.rectTransform.rotation = Quaternion.Euler(0.0f, 0.0f, -CurrentTime / 12.0f * 360.0f);
    }
}
