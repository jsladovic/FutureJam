using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

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
    private List<PicketLiner> AllPicketLiners;
    private bool IsGameOver;

    public const int LevelDurationSeconds = 30;
    public int NumberOfScabsEntered { get; private set; }
    public bool IsWaitingForUpgrade { get; private set; }

    private const float BaseScabSpeed = 1.8f;
    private const float ScabSpeedIncreasePerLevel = 0.1f;

    private int TotalScabsToSpawnRemaining;
    private int BasicScabsToSpawnRemaining;
    private int DesperateScabsToSpawnRemaining;
    private int SecondsBetweenScabsForLevel;

    private void Awake()
    {
        Instance = this;
        NumberOfScabsEntered = 0;
        Levels = Levels.OrderBy(ld => ld.Index).ToArray();
        CurrentLevelIndex = 0;
        AllPicketLiners = PicketLinersParent.GetComponentsInChildren<PicketLiner>().ToList();
        foreach (PicketLiner picketLiner in AllPicketLiners)
        {
            picketLiner.Initialize();
        }
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
        ScabsRemainingInLevel = CurrentLevel.NumberOfDefaultScabs + CurrentLevel.NumberOfDesperateScabs;
        TotalScabsToSpawnRemaining = ScabsRemainingInLevel;
        CurrentLevelCurves = MovementCurvesController.Instance.GetCurvesForLevelIndex(CurrentLevel.Index);
        BasicScabsToSpawnRemaining = CurrentLevel.NumberOfDefaultScabs;
        DesperateScabsToSpawnRemaining = CurrentLevel.NumberOfDesperateScabs;
        SecondsBetweenScabsForLevel = Mathf.CeilToInt(LevelDurationSeconds / (float)ScabsRemainingInLevel);
        print($"starting level {CurrentLevel.Index}, total scabs {ScabsRemainingInLevel}, time between {SecondsBetweenScabsForLevel}, number of curves {CurrentLevelCurves.Length}");
        CanvasController.Instance.DisplayLevel(CurrentLevel.Index, CurrentLevel.Index % 3 == 1, NumberOfScabsEntered > 0, AllPicketLiners.Any(pl => pl.Rank == PicketLinerRank.Basic));
    }

    public void KickOutScab()
    {
        HealthBarController.DisplayLifeGained();
        if (NumberOfScabsEntered > 0)
            NumberOfScabsEntered--;
    }

    public void SpawnPicketLiner()
    {
        PicketLiner picketLiner = Instantiate(PicketLinerPrefab, PicketLinerSpawningLocation.position, Quaternion.identity, PicketLinersParent);
        picketLiner.Initialize();
        AllPicketLiners.Add(picketLiner);
    }

    public void LevelUpPicketLiner()
    {
        IsWaitingForUpgrade = true;
        CanvasController.Instance.DisplayLevelUpText();
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
        else if (CurrentLevel.Index == 3)
            CanvasController.Instance.DisplayLevelThreeTutorialText();
    }

    private IEnumerator SpawnScabCoroutine(bool firstScab)
    {
        yield return new WaitForSeconds(SecondsBetweenScabsForLevel);
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
        ScabRank rank = BasicScabsToSpawnRemaining > 0 ? ScabRank.Basic : ScabRank.Desperate;
        if (rank == ScabRank.Basic)
            BasicScabsToSpawnRemaining--;
        else
            DesperateScabsToSpawnRemaining--;
        scab.Initialize(rank, curve, BaseScabSpeed + CurrentLevelIndex * ScabSpeedIncreasePerLevel);
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

