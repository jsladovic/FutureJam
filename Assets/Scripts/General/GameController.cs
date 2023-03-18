using UnityEngine;
using System.Collections;
using System.Linq;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] private LevelDefinition[] Levels;
    [SerializeField] private Scab ScabPrefab;
    [SerializeField] private Transform ScabsParent;
    [SerializeField] private Transform PicketLinersParent;
    [SerializeField] private ClockController Clock;

    private int CurrentLevelIndex;
    private int ScabsRemainingInLevel;
    private MovementCurve[] CurrentLevelCurves;
    private int LastUsedCurveIndex;
    private bool IsTimeExpired;

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
    private const int StartingNumberOfPicketLiners = 2;
    private const int MaxNumberOfScabsEntered = 5;
    public int NumberOfScabsEntered { get; private set; }

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
        LevelDefinition level = Levels[CurrentLevelIndex];
        ScabsRemainingInLevel = level.NumberOfDefaultScabs + level.NumberOfDesperateScabs + level.NumberOfCriticalScabs;
        TotalScabsToSpawnRemaining = ScabsRemainingInLevel;
        CurrentLevelCurves = MovementCurvesController.Instance.GetCurvesForLevelIndex(level.Index);
        BasicScabsToSpawnRemaining = level.NumberOfDefaultScabs;
        DesperateScabsToSpawnRemaining = level.NumberOfDesperateScabs;
        CriticalScabsToSpawnRemaining = level.NumberOfCriticalScabs;
        SecondsBetweenScabsForLevel = Mathf.CeilToInt(LevelDurationSeconds / (float)ScabsRemainingInLevel);
        print($"starting level {level.Index}, total scabs {ScabsRemainingInLevel}, time between {SecondsBetweenScabsForLevel}, number of curves {CurrentLevelCurves.Length}");
        CanvasController.Instance.DisplayLevel(level.Index, NumberOfScabsEntered > 0);
    }

    public void StartLevel()
    {
        StartCoroutine(SpawnScabCoroutine(true));
        Clock.StartLevel();
    }

    private IEnumerator SpawnScabCoroutine(bool firstScab)
    {
        yield return new WaitForSeconds(firstScab ? SecondsBetweenScabsForLevel / 2 : SecondsBetweenScabsForLevel);
        int curveIndex;
        do
        {
            curveIndex = Random.Range(0, CurrentLevelCurves.Length - 1);
        } while (CurrentLevelCurves.Length != 1 && curveIndex == LastUsedCurveIndex);
        LastUsedCurveIndex = curveIndex;
        MovementCurve curve = CurrentLevelCurves[curveIndex];
        Scab scab = Instantiate(ScabPrefab, curve.Points.First().transform.position, Quaternion.identity, ScabsParent);
        scab.Initialize(ScabRank.Basic, curve);
        TotalScabsToSpawnRemaining--;
        if (TotalScabsToSpawnRemaining > 0)
            StartCoroutine(SpawnScabCoroutine(false));
    }

    public void OnScabEntered()
    {
        NumberOfScabsEntered++;
        if (NumberOfScabsEntered >= MaxNumberOfScabsEntered)
        {
            print("Game over");
            IsGameOver = true;
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

