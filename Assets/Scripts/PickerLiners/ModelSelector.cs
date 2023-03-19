using UnityEngine;

public class ModelSelector : MonoBehaviour
{
    [SerializeField] private PicketLinerModel BasicModel;
    [SerializeField] private PicketLinerModel AdvancedModel;

    public void Initialize()
    {
        BasicModel.Initialize();
        AdvancedModel.Initialize();
    }

    public void SetCarriedSprite(bool isCarried)
    {
        BasicModel.SetCarriedSprite(isCarried);
        AdvancedModel.SetCarriedSprite(isCarried);
    }

    public void SetRank(PicketLinerRank rank)
    {
        BasicModel.gameObject.SetActive(rank == PicketLinerRank.Basic);
        AdvancedModel.gameObject.SetActive(rank == PicketLinerRank.Advanced);
    }
}
