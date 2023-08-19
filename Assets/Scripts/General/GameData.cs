using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.General
{
	[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/Game Data")]
	public class GameData : ScriptableObject
	{
		public GameType GameType;
	}
}
