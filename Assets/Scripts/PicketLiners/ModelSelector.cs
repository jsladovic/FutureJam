using System;
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
            BasicModel.gameObject.SetActive(rank == PicketLinerRank.Basic);
            AdvancedModel.gameObject.SetActive(rank == PicketLinerRank.Advanced);
            EliteModel.gameObject.SetActive(rank == PicketLinerRank.Elite);
            DisplayMergeSprite(false);
        }

		internal void DisplayMergeSprite(bool isVisible)
        {
            BasicModel.DisplayMergeSprite(isVisible);
            AdvancedModel.DisplayMergeSprite(isVisible);
            EliteModel.DisplayMergeSprite(isVisible);
        }
	}
}
