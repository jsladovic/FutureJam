using UnityEngine;

namespace Assets.Scripts.General
{
	[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level Definition")]
    public class LevelDefinition : ScriptableObject
    {
        public int Index;
        public string LevelText;
        public int NumberOfDefaultScabs;
        public int NumberOfDesperateScabs;
        public int NumberOfEliteScabs;
        public int AudioIndex;
    }
}
