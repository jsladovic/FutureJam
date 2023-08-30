using Assets.Scripts.GameEvents.Events;
using Assets.Scripts.Helpers;
using Assets.Scripts.PicketLiners;
using Assets.Scripts.Scabs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.General
{
	public class GameController : MonoBehaviour
	{
		public static GameController Instance;

		[SerializeField] private LevelDefinition[] Levels;
		[SerializeField] private Scab ScabPrefab;
		[SerializeField] private ScabLeaving ScabLeavingPrefab;
		[SerializeField] private PicketLiner PicketLinerPrefab;
		[SerializeField] private Transform ScabsParent;
		[SerializeField] private Transform PicketLinersParent;
		[SerializeField] private ClockController Clock;
		[SerializeField] private HealthBarController HealthBarController;
		[SerializeField] private Transform PicketLinerSpawningLocation;
		[SerializeField] private Transform ScabLeavingSpawningLocation;
		[SerializeField] public Transform TopLeftDraggablePosition;
		[SerializeField] public Transform BottomRightDraggablePosition;
		[SerializeField] private AnimationCurve ScabSpeedCurve;
		[SerializeField] private GameData GameData;

		[SerializeField] private BoolEvent OnPausedChanged;
		[SerializeField] private BoolEvent CanUseMouseChanged;
		[SerializeField] private VoidEvent OnLevelComplete;
		[SerializeField] private VoidEvent OnGameCompleted;
		[SerializeField] private LevelDefinitionEvent OnLevelStarted;
		[SerializeField] private IntEvent OnGameOver;

		private int NumberOfScabsEntering;

		private int CurrentLevelIndex;
		private int ScabsRemainingInLevel;
		private MovementCurve[] CurrentLevelCurves;
		private int LastUsedCurveIndex;
		private int SecondLastUsedCurveIndex;
		private bool IsTimeExpired;
		private LevelDefinition CurrentLevel;
		private List<PicketLiner> AllPicketLiners;
		private bool IsGameOver;

		public const int TotalLevelDuration = LevelDurationSeconds + SecondsInLevelAfterLastSpawn;
		public const int LevelDurationSeconds = 20;
		public const int SecondsInLevelAfterLastSpawn = 2;
		private const float AfterLevelWaitSeconds = 1.0f;
		private const int MaxNumberOfPicketLiners = 8;

		public int NumberOfScabsEntered { get; private set; }

		private int TotalScabsToSpawnRemaining;
		private int BasicScabsToSpawnRemaining;
		private int DesperateScabsToSpawnRemaining;
		private int EliteScabsToSpawnRemaining;
		private float SecondsBetweenScabsForLevel;

		private bool IsGameInProgress;
		private bool LevelOverRaised;

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
			PrepareLevel();
		}

		private void Update()
		{
			if (IsGameInProgress == false)
				return;

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				StartCoroutine(OnPauseCoroutine());
			}
		}

		private IEnumerator OnPauseCoroutine()
		{
			yield return new WaitForEndOfFrame();
			OnPausedChanged.Raise(true);
			CanUseMouseChanged.Raise(false);
		}

		private void PrepareLevel()
		{
			IsGameInProgress = false;
			bool isAfterFinalLevel = CurrentLevelIndex >= Levels.Length;
			CurrentLevel = isAfterFinalLevel ? Levels[Levels.Length - 1] : Levels[CurrentLevelIndex];
			ScabsRemainingInLevel = CurrentLevel.NumberOfDefaultScabs + CurrentLevel.NumberOfDesperateScabs + CurrentLevel.NumberOfEliteScabs;
			TotalScabsToSpawnRemaining = ScabsRemainingInLevel;
			CurrentLevelCurves = MovementCurvesController.Instance.GetCurvesForLevelIndex(CurrentLevel.Index);
			if (isAfterFinalLevel == true)
			{
				int difference = CurrentLevelIndex - Levels.Length + 1;
				BasicScabsToSpawnRemaining = 1;
				DesperateScabsToSpawnRemaining = 1;
				EliteScabsToSpawnRemaining = CurrentLevel.NumberOfEliteScabs + difference;
			}
			else
			{
				BasicScabsToSpawnRemaining = CurrentLevel.NumberOfDefaultScabs;
				DesperateScabsToSpawnRemaining = CurrentLevel.NumberOfDesperateScabs;
				EliteScabsToSpawnRemaining = CurrentLevel.NumberOfEliteScabs;
			}
			SecondsBetweenScabsForLevel = LevelDurationSeconds / (float)ScabsRemainingInLevel;
			print($"starting level {CurrentLevelIndex + 1}, total scabs {ScabsRemainingInLevel}, time between {SecondsBetweenScabsForLevel}, number of curves {CurrentLevelCurves.Length}");

			int numberOfSpawnedPicketLiners = AllPicketLiners.Sum(pl => (int)pl.Rank + 1);
			CanvasController.Instance.DisplayLevel(CurrentLevelIndex, CurrentLevelIndex % 3 == 0, NumberOfScabsEntered > 0, numberOfSpawnedPicketLiners < MaxNumberOfPicketLiners);
		}

		public void KickOutScab()
		{
			if (NumberOfScabsEntered == 0)
				throw new UnityException("Can't kick out scab if there are no scabs entered");
			StartCoroutine(HealthBarController.DisplayLifeGainedCoroutine());
			NumberOfScabsEntered = 0;

			Instantiate(ScabLeavingPrefab, ScabLeavingSpawningLocation.position, Quaternion.identity);
		}

		public PicketLiner SpawnPicketLiner(Vector3? position = null, PicketLinerRank? rank = null)
		{
			PicketLiner picketLiner = Instantiate(PicketLinerPrefab, position ?? PicketLinerSpawningLocation.position, Quaternion.identity, PicketLinersParent);
			picketLiner.Initialize(rank: rank);
			AllPicketLiners.Add(picketLiner);
			return picketLiner;
		}

		public void StartLevel()
		{
			LevelOverRaised = false;
			StartCoroutine(SpawnScabCoroutine(true));
			Clock.StartLevel();
			CursorController.Instance.SetCursorSprite();
			CanUseMouseChanged.Raise(true);
			OnLevelStarted.Raise(CurrentLevel);
			IsGameInProgress = true;
		}

		private IEnumerator SpawnScabCoroutine(bool firstScab)
		{
			yield return new WaitForSeconds(SecondsBetweenScabsForLevel);
			if (IsGameOver == true)
				yield break;
			MovementCurve curve = GetNextCurve();
			Scab scab = Instantiate(ScabPrefab, curve.Points.First().transform.position, Quaternion.identity, ScabsParent);
			ScabRank rank = BasicScabsToSpawnRemaining > 0
				? ScabRank.Basic
				: (DesperateScabsToSpawnRemaining > 0 ? ScabRank.Desperate : ScabRank.Elite);
			if (rank == ScabRank.Basic)
				BasicScabsToSpawnRemaining--;
			else if (rank == ScabRank.Desperate)
				DesperateScabsToSpawnRemaining--;
			else
				EliteScabsToSpawnRemaining--;
			float scabSpeed = ScabSpeedCurve.Evaluate(CurrentLevelIndex);
			scab.Initialize(rank, curve, scabSpeed);
			TotalScabsToSpawnRemaining--;
			if (TotalScabsToSpawnRemaining > 0)
				StartCoroutine(SpawnScabCoroutine(false));
		}

		private MovementCurve GetNextCurve()
		{
			if (CurrentLevelCurves == null || CurrentLevelCurves.Length == 0)
				throw new UnityException("No curves set for level");
			if (CurrentLevelCurves.Length == 1)
			{
				LastUsedCurveIndex = 0;
				return CurrentLevelCurves[0];
			}

			if (CurrentLevelCurves.Length == 2)
			{
				LastUsedCurveIndex = 1 - LastUsedCurveIndex;
				return CurrentLevelCurves[LastUsedCurveIndex];
			}
			int curveIndex;
			do
			{
				curveIndex = Random.Range(0, CurrentLevelCurves.Length);
			} while (curveIndex == LastUsedCurveIndex || curveIndex == SecondLastUsedCurveIndex);
			SecondLastUsedCurveIndex = LastUsedCurveIndex;
			LastUsedCurveIndex = curveIndex;
			MovementCurve curve = CurrentLevelCurves[curveIndex];
			return curve;
		}

		public void OnScabEntered()
		{
			NumberOfScabsEntered++;
		}

		public void OnScabDestroyed()
		{
			bool allLivesLost = HealthBarController.AreAllLivesLost();
			if (allLivesLost == true && IsGameInProgress == true)
			{
				EndGame();
				return;
			}
			ScabsRemainingInLevel--;
			CheckForLevelOver();
		}

		private void EndGame()
		{
			IsGameOver = true;
			IsGameInProgress = false;
			OnGameOver.Raise(CurrentLevel.Index);
		}

		public void OnTimeExpired()
		{
			IsTimeExpired = true;
			CheckForLevelOver();
		}

		private void CheckForLevelOver()
		{
			if (IsTimeExpired == false || ScabsRemainingInLevel > 0 || IsGameOver == true || LevelOverRaised == true)
				return;
			StartCoroutine(RaiseLevelCompleteCoroutine());
			CanUseMouseChanged.Raise(false);
			CheckForMaxEndlessLevel();
			CurrentLevelIndex++;
			StartCoroutine(StartNewLevelCoroutine());
			LevelOverRaised = true;
		}

		private void CheckForMaxEndlessLevel()
		{
			int previousMaxLevel = PlayerPrefsHelpers.GetMaxLevelCompleted();
			if (CurrentLevelIndex + 1 > previousMaxLevel)
				PlayerPrefsHelpers.SetMaxLevelCompleted(CurrentLevelIndex + 1);
		}

		private IEnumerator RaiseLevelCompleteCoroutine()
		{
			yield return new WaitForEndOfFrame();
			OnLevelComplete.Raise();
		}

		private IEnumerator StartNewLevelCoroutine()
		{
			if (CurrentLevelIndex >= Levels.Length && GameData.GameType == Enums.GameType.Regular)
			{
				OnGameCompleted.Raise();
				yield break;
			}
			yield return new WaitForSeconds(AfterLevelWaitSeconds);
			PrepareLevel();
		}

		public void SpawnDemotedPicketLiner(PicketLiner picketLiner)
		{
			PicketLinerRank demotedRank = picketLiner.Rank == PicketLinerRank.Advanced ? PicketLinerRank.Basic : PicketLinerRank.Advanced;
			Vector3 spawnPosition = picketLiner.GetClosestClickingPoint();
			PicketLiner spawnedPicketLiner = SpawnPicketLiner(spawnPosition, rank: demotedRank);
			spawnedPicketLiner.DragController.OnCanUseMouseChanged(true);
		}

		public void OnPicketLinerDestroyed(PicketLiner picketLiner)
		{
			AllPicketLiners.Remove(picketLiner);
		}
	}
}
