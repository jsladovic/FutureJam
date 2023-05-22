using Assets.Scripts.General;
using UnityEngine;

namespace Assets.Scripts.PicketLiners
{
	[RequireComponent(typeof(DragController))]
	public class PicketLiner : MonoBehaviour
	{
		public DragController DragController { get; private set; }
		private ModelSelector ModelSelector;
		private SphereOfInfluenceSelector SphereOfInfluenceSelector;

		private PicketLinerRank rank;
		public PicketLinerRank Rank
		{
			get { return rank; }
			set
			{
				rank = value;
				ModelSelector.SetRank(value);
				SphereOfInfluenceSelector.SetRank(value);
			}
		}

		public void Initialize(PicketLinerRank? rank = null)
		{
			rank ??= PicketLinerRank.Basic;
			DragController = GetComponent<DragController>();
			ModelSelector = GetComponentInChildren<ModelSelector>();
			SphereOfInfluenceSelector = GetComponentInChildren<SphereOfInfluenceSelector>();

			SphereOfInfluenceSelector.Initialize(this);
			ModelSelector.Initialize();
			Rank = rank.Value;
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

		public bool CanBeUpgraded => Rank == PicketLinerRank.Basic || Rank == PicketLinerRank.Advanced;

		public bool CanBeDemoted => Rank > PicketLinerRank.Basic;

		public bool IsCarried => DragController.IsDragging;
	}
}
