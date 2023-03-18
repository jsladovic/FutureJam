using UnityEngine;
using System.Collections;
using System.Linq;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] private LevelDefinition[] Levels;
    private int CurrentLevelIndex;

    private const int MaxNumberOfScabsEntered = 5;
    public int NumberOfScabsEntered { get; private set; }

    private void Awake()
    {
        Instance = this;
        NumberOfScabsEntered = 0;
        Levels = Levels.OrderBy(ld => ld.Index).ToArray();
    }

    private void PrepareLevel()
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
}

