using UnityEngine;

namespace Assets.Scripts.PicketLiners
{
	public class ModelSelector : MonoBehaviour
    {
        [SerializeField] private PicketLinerModel BasicModel;
        [SerializeField] private PicketLinerModel AdvancedModel;
        [SerializeField] private PicketLinerModel EliteModel;

        public void Initialize()
        {
            BasicModel.Initialize();
            AdvancedModel.Initialize();
            EliteModel.Initialize();
        }

        public void SetCarriedSprite(bool isCarried)
        {
            BasicModel.SetCarriedSprite(isCarried);
            AdvancedModel.SetCarriedSprite(isCarried);
            EliteModel.SetCarriedSprite(isCarried);
        }

        public void SetRank(PicketLinerRank rank)
        {
            PicketLinerModel currentModel = GetCurrentModel(rank);
            BasicModel.gameObject.SetActive(currentModel == BasicModel);
            AdvancedModel.gameObject.SetActive(currentModel == AdvancedModel);
            EliteModel.gameObject.SetActive(currentModel == EliteModel);
            DisplayMergeSprite(false);
        }

		internal void DisplayMergeSprite(bool isVisible)
        {
            BasicModel.DisplayMergeSprite(isVisible);
            AdvancedModel.DisplayMergeSprite(isVisible);
            EliteModel.DisplayMergeSprite(isVisible);
        }

        public Vector3 GetClosestClickingPoint(PicketLinerRank rank)
		{
            PicketLinerModel currentModel = GetCurrentModel(rank);
            return currentModel.GetClosestClickingPoint();
		}

        private PicketLinerModel GetCurrentModel(PicketLinerRank rank)
		{
            switch (rank)
			{
                case PicketLinerRank.Basic:
                    return BasicModel;
                case PicketLinerRank.Advanced:
                    return AdvancedModel;
                case PicketLinerRank.Elite:
                    return EliteModel;
                default:
                    throw new UnityException($"Unknown model for rank {rank}");
			}
		}
	}
}
