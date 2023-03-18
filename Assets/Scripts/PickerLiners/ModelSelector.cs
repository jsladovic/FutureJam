using UnityEngine;

public class ModelSelector : MonoBehaviour
{
    [SerializeField] private GameObject BasicModel;
    [SerializeField] private GameObject AdvancedModel;
    [SerializeField] private GameObject EliteModel;

    public void SetRank(PicketLinerRank rank)
    {
        BasicModel.SetActive(rank == PicketLinerRank.Basic);
        AdvancedModel.SetActive(rank == PicketLinerRank.Advanced);
        EliteModel.SetActive(rank == PicketLinerRank.Elite);
    }
}
