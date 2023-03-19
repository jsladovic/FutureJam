using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level Definition")]
public class LevelDefinition : ScriptableObject
{
    public int Index;
    public int NumberOfDefaultScabs;
    public int NumberOfDesperateScabs;
}

