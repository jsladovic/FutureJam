using Assets.Scripts.General;
using System;
using UnityEngine.Events;

namespace Assets.Scripts.GameEvents.UnityEvents
{
	[Serializable]
	public class UnityLevelDefinitionEvent : UnityEvent<LevelDefinition> { }
}
