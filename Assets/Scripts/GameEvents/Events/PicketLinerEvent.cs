using Assets.Scripts.PicketLiners;
using UnityEngine;

namespace Assets.Scripts.GameEvents.Events
{
	[CreateAssetMenu(fileName = "New Picket Liner Event", menuName = "Game Events/Picket Liner")]
	public class PicketLinerEvent : BaseGameEvent<PicketLiner> { }
}
