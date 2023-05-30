using UnityEngine;

namespace Assets.Scripts.PicketLiners
{
	public class DragColliderParent : MonoBehaviour
	{
		[SerializeField] private CapsuleCollider2D BasicCollider;
		[SerializeField] private CapsuleCollider2D AdvancedCollider;
		[SerializeField] private CapsuleCollider2D EliteCollider;

		public CapsuleCollider2D GetCollider(PicketLinerRank rank)
		{
			switch (rank)
			{
				case PicketLinerRank.Basic:
					return BasicCollider;
				case PicketLinerRank.Advanced:
					return AdvancedCollider;
				case PicketLinerRank.Elite:
					return EliteCollider;
				default:
					throw new UnityException($"Unknown picket liner rank {rank}");
			}
		}
	}
}
