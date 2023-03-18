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

    private int CurrentLevelIndex;
    private int ScabsRemainingInLevel;
    private MovementCurve[] CurrentLevelCurves;
    private int LastUsedCurveIndex;

    private const int StartingNumberOfPicketLiners = 2;
    private const int MaxNumberOfScabsEntered = 5;
    private const int LevelDurationSeconds = 30;
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
        StartLevel();
        // Display UI options
    }

    public void StartLevel()
    {
        StartCoroutine(SpawnScabCoroutine());
    }

    private IEnumerator SpawnScabCoroutine()
    {
        yield return new WaitForSeconds(SecondsBetweenScabsForLevel);
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
            StartCoroutine(SpawnScabCoroutine());
    }

    public void OnScabEntered()
    {
        NumberOfScabsEntered++;
        ScabsRemainingInLevel--;
        if (NumberOfScabsEntered >= MaxNumberOfScabsEntered)
        {
            print("Game over");
        }
    }

    public void OnScabLeft()
    {
        ScabsRemainingInLevel--;
    }

    private void CheckForLevelOver()
    {

    }
}

