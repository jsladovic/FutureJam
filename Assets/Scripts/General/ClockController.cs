using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.General
{
    public class ClockController : MonoBehaviour
    {
        [SerializeField] private Image ClockHand;

        private const int StartingHours = 7;
        private const int NumberOfWorkHours = 12;

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
            CurrentTime = StartingHours + ((GameController.TotalLevelDuration - TimeRemainingInLevel) / GameController.TotalLevelDuration) * NumberOfWorkHours;
            if (TimeRemainingInLevel <= 0)
            {
                IsRunning = false;
                GameController.Instance.OnTimeExpired();
            }
        }

        public void StartLevel()
        {
            CurrentTime = StartingHours;
            TimeRemainingInLevel = GameController.TotalLevelDuration;
            IsRunning = true;
        }

        private void DisplayTime()
        {
            ClockHand.rectTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f - CurrentTime / NumberOfWorkHours * 360.0f);
        }
    }
}
