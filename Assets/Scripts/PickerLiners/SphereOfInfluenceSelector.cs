using UnityEngine;

public class SphereOfInfluenceSelector : MonoBehaviour
{
    [SerializeField] private SphereOfInfluence BasicCollider;
    [SerializeField] private SphereOfInfluence AdvancedCollider;
    [SerializeField] private SphereOfInfluence EliteCollider;

    public void SetRank(PicketLinerRank rank)
    {
        BasicCollider.gameObject.SetActive(rank == PicketLinerRank.Basic);
        AdvancedCollider.gameObject.SetActive(rank == PicketLinerRank.Advanced);
        EliteCollider.gameObject.SetActive(rank == PicketLinerRank.Elite);
    }
}
