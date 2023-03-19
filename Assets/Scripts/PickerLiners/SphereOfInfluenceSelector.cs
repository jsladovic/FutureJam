using UnityEngine;

public class SphereOfInfluenceSelector : MonoBehaviour
{
    [SerializeField] private SphereOfInfluence BasicCollider;
    [SerializeField] private SphereOfInfluence AdvancedCollider;

    public void Initialize(PicketLiner picketLiner)
    {
        BasicCollider.Initialize(picketLiner);
        AdvancedCollider.Initialize(picketLiner);
    }

    public void SetRank(PicketLinerRank rank)
    {
        BasicCollider.gameObject.SetActive(rank == PicketLinerRank.Basic);
        AdvancedCollider.gameObject.SetActive(rank == PicketLinerRank.Advanced);
    }

    public void SetCarriedSprite(bool isCarried)
    {
        BasicCollider.SetCarriedSprite(isCarried);
        AdvancedCollider.SetCarriedSprite(isCarried);
    }
}
