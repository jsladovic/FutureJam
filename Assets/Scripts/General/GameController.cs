using UnityEngine;
using System.Collections;
using System.Linq;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] private LevelDefinition[] Levels;
    [SerializeField] private Scab ScabPrefab;

    private int CurrentLevelIndex;
    private int ScabsRemainingInLevel;
    private MovementCurve[] CurrentLevelCurves;

    private const int MaxNumberOfScabsEntered = 5;
    private const int LevelDurationSeconds = 60;
    public int NumberOfScabsEntered { get; private set; }

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
        PrepareLevel();
    }

    private void PrepareLevel()
    {
        if (CurrentLevelIndex >= Levels.Length)
            throw new UnityException($"Oh no, level {CurrentLevelIndex + 1} is not yet implemented");
        LevelDefinition level = Levels[CurrentLevelIndex];
        ScabsRemainingInLevel = level.NumberOfDefaultScabs + level.NumberOfDesperateScabs + level.NumberOfCriticalScabs;
        CurrentLevelCurves = MovementCurvesController.Instance.GetCurvesForLevelIndex(level.Index);
        BasicScabsToSpawnRemaining = level.NumberOfDefaultScabs;
        DesperateScabsToSpawnRemaining = level.NumberOfDesperateScabs;
        CriticalScabsToSpawnRemaining = level.NumberOfCriticalScabs;
        SecondsBetweenScabsForLevel = Mathf.CeilToInt(LevelDurationSeconds / (float)ScabsRemainingInLevel);
        // Display UI options
    }

    public void StartLevel()
    {

    }

    public void OnScabEntered()
    {
        NumberOfScabsEntered++;
        if (NumberOfScabsEntered >= MaxNumberOfScabsEntered)
        {
            print("Game over");
        }
    }

    public void OnScabLeft()
    {

    }
}

