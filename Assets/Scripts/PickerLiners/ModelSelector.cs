using UnityEngine;

public class ModelSelector : MonoBehaviour
{
    [SerializeField] private PicketLinerModel BasicModel;
    [SerializeField] private PicketLinerModel AdvancedModel;
    [SerializeField] private PicketLinerModel EliteModel;

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
    }
}
