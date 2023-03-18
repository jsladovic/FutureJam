using UnityEngine;

public class SphereOfInfluenceSelector : MonoBehaviour
{
    [SerializeField] private CapsuleCollider2D BasicCollider;
    [SerializeField] private CapsuleCollider2D AdvancedCollider;
    [SerializeField] private CapsuleCollider2D EliteCollider;

    public void SetRank(PicketLinerRank rank)
    {
        BasicCollider.gameObject.SetActive(rank == PicketLinerRank.Basic);
        AdvancedCollider.gameObject.SetActive(rank == PicketLinerRank.Advanced);
        EliteCollider.gameObject.SetActive(rank == PicketLinerRank.Elite);
    }
}
