using Assets.Scripts.GameEvents.Events;
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
		[SerializeField] private PicketLiner PicketLinerPrefab;
		[SerializeField] private Transform ScabsParent;
		[SerializeField] private Transform PicketLinersParent;
		[SerializeField] private ClockController Clock;
		[SerializeField] private HealthBarController HealthBarController;
		[SerializeField] private Transform PicketLinerSpawningLocation;
		[SerializeField] public Transform TopLeftDraggablePosition;
		[SerializeField] public Transform BottomRightDraggablePosition;

		[SerializeField] private BoolEvent OnPausedChanged;
		[SerializeField] private BoolEvent CanUseMouseChanged;
		[SerializeField] private VoidEvent OnLevelComplete;
		[SerializeField] private LevelDefinitionEvent OnLevelStarted;
		[SerializeField] private IntEvent OnGameOver;

		private int NumberOfScabsEntering;

		private int CurrentLevelIndex;
		private int ScabsRemainingInLevel;
		private MovementCurve[] CurrentLevelCurves;
		private int LastUsedCurveIndex;
		private bool IsTimeExpired;
		private LevelDefinition CurrentLevel;
		private List<PicketLiner> AllPicketLiners;
		private bool IsGameOver;

		public const int TotalLevelDuration = LevelDurationSeconds + SecondsInLevelAfterLastSpawn;
		public const int LevelDurationSeconds = 20;
		public const int SecondsInLevelAfterLastSpawn = 2;
		private const float AfterLevelWaitSeconds = 1.0f;
		public int NumberOfScabsEntered { get; private set; }

		private const float BaseScabSpeed = 1.5f;
		private const float ScabSpeedIncreasePerLevel = 0.075f;

		private int TotalScabsToSpawnRemaining;
		private int BasicScabsToSpawnRemaining;
		private int DesperateScabsToSpawnRemaining;
		private int EliteScabsToSpawnRemaining;
		private float SecondsBetweenScabsForLevel;

		private bool IsGameInProgress;

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
			if (CurrentLevelIndex >= Levels.Length)
				throw new UnityException($"Oh no, level {CurrentLevelIndex + 1} is not yet implemented");
			IsGameInProgress = false;
			CurrentLevel = Levels[CurrentLevelIndex];
			ScabsRemainingInLevel = CurrentLevel.NumberOfDefaultScabs + CurrentLevel.NumberOfDesperateScabs + CurrentLevel.NumberOfEliteScabs;
			TotalScabsToSpawnRemaining = ScabsRemainingInLevel;
			CurrentLevelCurves = MovementCurvesController.Instance.GetCurvesForLevelIndex(CurrentLevel.Index);
			BasicScabsToSpawnRemaining = CurrentLevel.NumberOfDefaultScabs;
			DesperateScabsToSpawnRemaining = CurrentLevel.NumberOfDesperateScabs;
			EliteScabsToSpawnRemaining = CurrentLevel.NumberOfEliteScabs;
			SecondsBetweenScabsForLevel = LevelDurationSeconds / (float)ScabsRemainingInLevel;
			print($"starting level {CurrentLevel.Index}, total scabs {ScabsRemainingInLevel}, time between {SecondsBetweenScabsForLevel}, number of curves {CurrentLevelCurves.Length}");
			CanvasController.Instance.DisplayLevel(CurrentLevel.Index, CurrentLevel.Index % 3 == 1, NumberOfScabsEntered > 0);
		}

		public void KickOutScab()
		{
			HealthBarController.DisplayLifeGained();
			if (NumberOfScabsEntered > 0)
				NumberOfScabsEntered--;
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
			int curveIndex;
			do
			{
				curveIndex = Random.Range(0, CurrentLevelCurves.Length);
			} while (CurrentLevelCurves.Length != 1 && curveIndex == LastUsedCurveIndex);
			LastUsedCurveIndex = curveIndex;
			MovementCurve curve = CurrentLevelCurves[curveIndex];
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
			scab.Initialize(rank, curve, BaseScabSpeed + CurrentLevelIndex * ScabSpeedIncreasePerLevel);
			TotalScabsToSpawnRemaining--;
			if (TotalScabsToSpawnRemaining > 0)
				StartCoroutine(SpawnScabCoroutine(false));
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
			if (IsTimeExpired == false || ScabsRemainingInLevel > 0 || IsGameOver == true)
				return;
			OnLevelComplete.Raise();
			CanUseMouseChanged.Raise(false);
			CurrentLevelIndex++;
			StartCoroutine(StartNewLevelCoroutine());
		}

		private IEnumerator StartNewLevelCoroutine()
		{
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
