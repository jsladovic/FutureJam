using UnityEngine;
using System.Collections;
using System.Linq;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] private LevelDefinition[] Levels;
    [SerializeField] private Scab ScabPrefab;
    [SerializeField] private PicketLiner PicketLinerPrefab;
    [SerializeField] private Transform ScabsParent;
    [SerializeField] private Transform PicketLinersParent;
    [SerializeField] private ClockController Clock;
    [SerializeField] private HealthBarController HealthBarController;
    [SerializeField] private Transform PicketLinerSpawningLocation;
    [SerializeField] public Transform TopLeftDraggablePosition;
    [SerializeField] public Transform BottomRightDraggablePosition;

    private int CurrentLevelIndex;
    private int ScabsRemainingInLevel;
    private MovementCurve[] CurrentLevelCurves;
    private int LastUsedCurveIndex;
    private bool IsTimeExpired;
    private LevelDefinition CurrentLevel;

    private bool isGameOver;
    private bool IsGameOver
    {
        get { return isGameOver; }
        set
        {
            isGameOver = value;
            if (value == true)
            {
                // TODO display restart button
            }
        }
    }

    public const int LevelDurationSeconds = 30;
    public int NumberOfScabsEntered { get; private set; }
    public bool IsWaitingForUpgrade { get; private set; }

    private const float BaseScabSpeed = 1.8f;
    private const float ScabSpeedIncreasePerLevel = 0.2f;

    private int TotalScabsToSpawnRemaining;
    private int BasicScabsToSpawnRemaining;
    private int DesperateScabsToSpawnRemaining;
    private int CriticalScabsToSpawnRemaining;
    private int SecondsBetweenScabsForLevel;

    private void Awake()
    {
        Instance = this;
        NumberOfScabsEntered = 0;
        Levels = Levels.OrderBy(ld => ld.Index).ToArray();
        CurrentLevelIndex = 0;
    }

    private void Start()
    {
        CanvasController.Instance.Initialize();
        PrepareLevel();
    }

    private void PrepareLevel()
    {
        if (CurrentLevelIndex >= Levels.Length)
            throw new UnityException($"Oh no, level {CurrentLevelIndex + 1} is not yet implemented");
        CurrentLevel = Levels[CurrentLevelIndex];
        ScabsRemainingInLevel = CurrentLevel.NumberOfDefaultScabs + CurrentLevel.NumberOfDesperateScabs + CurrentLevel.NumberOfCriticalScabs;
        TotalScabsToSpawnRemaining = ScabsRemainingInLevel;
        CurrentLevelCurves = MovementCurvesController.Instance.GetCurvesForLevelIndex(CurrentLevel.Index);
        BasicScabsToSpawnRemaining = CurrentLevel.NumberOfDefaultScabs;
        DesperateScabsToSpawnRemaining = CurrentLevel.NumberOfDesperateScabs;
        CriticalScabsToSpawnRemaining = CurrentLevel.NumberOfCriticalScabs;
        SecondsBetweenScabsForLevel = Mathf.CeilToInt(LevelDurationSeconds / (float)ScabsRemainingInLevel);
        print($"starting level {CurrentLevel.Index}, total scabs {ScabsRemainingInLevel}, time between {SecondsBetweenScabsForLevel}, number of curves {CurrentLevelCurves.Length}");
        CanvasController.Instance.DisplayLevel(CurrentLevel.Index, NumberOfScabsEntered > 0);
    }

    public void KickOutScab()
    {
        HealthBarController.DisplayLifeGained();
    }

    public void SpawnPicketLiner()
    {
        Instantiate(PicketLinerPrefab, PicketLinerSpawningLocation.position, Quaternion.identity, PicketLinersParent);
    }

    public void LevelUpPicketLiner()
    {
        IsWaitingForUpgrade = true;
    }

    public void StartLevel()
    {
        IsWaitingForUpgrade = false;
        StartCoroutine(SpawnScabCoroutine(true));
        Clock.StartLevel();
        if (CurrentLevel.Index == 1)
            CanvasController.Instance.DisplayLevelOneTutorialText();
        else if (CurrentLevel.Index == 2)
            CanvasController.Instance.DisplayLevelTwoTutorialText();
    }

    private IEnumerator SpawnScabCoroutine(bool firstScab)
    {
        yield return new WaitForSeconds(firstScab ? SecondsBetweenScabsForLevel / 2 : SecondsBetweenScabsForLevel);
        if (IsGameOver == true)
            yield break;
        int curveIndex;
        do
        {
            curveIndex = Random.Range(0, CurrentLevelCurves.Length);
        } while (CurrentLevelCurves.Length != 1 && curveIndex == LastUsedCurveIndex);
        LastUsedCurveIndex = curveIndex;
        MovementCurve curve = CurrentLevelCurves[curveIndex];
        Scab scab = Instantiate(ScabPrefab, curve.Points.First().transform.position, Quaternion.identity, ScabsParent);
        scab.Initialize(ScabRank.Basic, curve, BaseScabSpeed + CurrentLevelIndex * ScabSpeedIncreasePerLevel);
        TotalScabsToSpawnRemaining--;
        if (TotalScabsToSpawnRemaining > 0)
            StartCoroutine(SpawnScabCoroutine(false));
    }

    public void OnScabEntered()
    {
        NumberOfScabsEntered++;
        bool allLivesLost = HealthBarController.DisplayLifeLost();
        if (allLivesLost == true)
        {
            print("Game over");
            IsGameOver = true;
            CanvasController.Instance.DisplayEndGameScreen(CurrentLevel.Index);
        }
    }

    public void OnScabDestroyed()
    {
        ScabsRemainingInLevel--;
        CheckForLevelOver();
    }

    public void OnTimeExpired()
    {
        IsTimeExpired = true;
        CheckForLevelOver();
    }

    private void CheckForLevelOver()
    {
        if (IsTimeExpired == false || ScabsRemainingInLevel > 0 || IsGameOver == true)
            return;
        CurrentLevelIndex++;
        PrepareLevel();
    }
}

