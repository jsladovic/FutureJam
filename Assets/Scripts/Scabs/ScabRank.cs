using UnityEngine;

public enum ScabRank
{
    Basic = 0,
    Desperate = 1,
    Elite = 2,
}

public static class ScabRankExtensions
{
    public static int SpheresOfInfulenceNeededToLeave(this ScabRank rank)
    {
        switch (rank)
        {
            case ScabRank.Basic:
                return 1;
            case ScabRank.Desperate:
                return 2;
            case ScabRank.Elite:
                return 2;
            default:
                throw new UnityException($"Unknown scab rank {rank}");
        }
    }
}
