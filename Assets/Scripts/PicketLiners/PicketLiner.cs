using Assets.Scripts.General;
using UnityEngine;

namespace Assets.Scripts.PicketLiners
{
	[RequireComponent(typeof(DragController))]
	[RequireComponent(typeof(CapsuleCollider2D))]
	public class PicketLiner : MonoBehaviour
	{
		public DragController DragController { get; private set; }
		private ModelSelector ModelSelector;
		private SphereOfInfluenceSelector SphereOfInfluenceSelector;
		private DragColliderParent DragColliderParent;
		private CapsuleCollider2D Collider;

		private PicketLinerRank rank;
		public PicketLinerRank Rank
		{
			get { return rank; }
			set
			{
				rank = value;
				ModelSelector.SetRank(value);
				SphereOfInfluenceSelector.SetRank(value);
				CapsuleCollider2D collider = DragColliderParent.GetCollider(value);
				Collider.size = collider.size;
				Collider.offset = collider.offset;
			}
		}

		public void Initialize(PicketLinerRank? rank = null)
		{
			rank ??= PicketLinerRank.Basic;
			DragController = GetComponent<DragController>();
			ModelSelector = GetComponentInChildren<ModelSelector>();
			SphereOfInfluenceSelector = GetComponentInChildren<SphereOfInfluenceSelector>();
			DragColliderParent = GetComponentInChildren<DragColliderParent>();
			Collider = GetComponent<CapsuleCollider2D>();

			SphereOfInfluenceSelector.Initialize(this);
			ModelSelector.Initialize();
			Rank = rank.Value;
			print("initializing picket liner");
			DragController.Initialize(this);
		}

		public void OnIsDraggedChanged()
		{
			ModelSelector.SetCarriedSprite(IsCarried);
			SphereOfInfluenceSelector.SetCarriedSprite(IsCarried);
		}

		private void OnMouseEnter()
		{
			CursorController.Instance.RegisterPicketLinerHovered(this);
		}

		private void OnMouseExit()
		{
			CursorController.Instance.UnregisterPicketLinerHovered(this);
		}

		public void UpgradeRank()
		{
			switch (Rank)
			{
				case PicketLinerRank.Basic:
					Rank = PicketLinerRank.Advanced;
					return;
				case PicketLinerRank.Advanced:
					Rank = PicketLinerRank.Elite;
					return;
				default:
					throw new UnityException($"Unable to promote from rank {Rank}");
			}
		}

		public Vector3 GetClosestClickingPoint()
		{
			return ModelSelector.GetClosestClickingPoint(Rank);
		}

		public bool CanBeUpgraded => Rank == PicketLinerRank.Basic || Rank == PicketLinerRank.Advanced;

		public bool CanBeDemoted => Rank > PicketLinerRank.Basic;

		public bool IsCarried => DragController.IsDragging;

		internal void DisplayMergeSprite(bool isVisible)
		{
			ModelSelector.DisplayMergeSprite(isVisible);
		}
	}
}
