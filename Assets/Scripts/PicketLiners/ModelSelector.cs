using UnityEngine;

namespace Assets.Scripts.PicketLiners
{
	public class ModelSelector : MonoBehaviour
    {
        [SerializeField] private PicketLinerModel BasicModel;
        [SerializeField] private PicketLinerModel AdvancedModel;
        [SerializeField] private PicketLinerModel EliteModel;

		public string CurrentAnimationName { get; private set; }

		public void Initialize()
        {
            BasicModel.Initialize(this);
            AdvancedModel.Initialize(this);
            EliteModel.Initialize(this);
        }

        public void SetCarriedSprite(bool isCarried)
        {
            BasicModel.SetCarriedSprite(isCarried);
            AdvancedModel.SetCarriedSprite(isCarried);
            EliteModel.SetCarriedSprite(isCarried);
        }

        public void PlayHoverAnimation()
        {
            BasicModel.PlayHoverAnimation();
            AdvancedModel.PlayHoverAnimation();
            EliteModel.PlayHoverAnimation();
        }

        public void PlayIdleAnimation()
        {
            BasicModel.PlayIdleAnimation();
            AdvancedModel.PlayIdleAnimation();
            EliteModel.PlayIdleAnimation();
        }

        public void PlayConnectBackAnimation()
        {
            BasicModel.PlayConnectBackAnimation();
            AdvancedModel.PlayConnectBackAnimation();
            EliteModel.PlayConnectBackAnimation();
        }

        public void PlayConnectFrontAnimation()
        {
            BasicModel.PlayConnectFrontAnimation();
            AdvancedModel.PlayConnectFrontAnimation();
            EliteModel.PlayConnectFrontAnimation();
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

        public void SetCurrentAnimationName(string animationName)
		{
            CurrentAnimationName = animationName;
		}
	}
}
