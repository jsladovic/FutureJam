using Assets.Scripts.General;
using UnityEngine;

namespace Assets.Scripts.GameEvents.Events
{
	[CreateAssetMenu(fileName = "New Level Definition Event", menuName = "Game Events/LevelDefinition")]
	public class LevelDefinitionEvent : BaseGameEvent<LevelDefinition> { }
}
