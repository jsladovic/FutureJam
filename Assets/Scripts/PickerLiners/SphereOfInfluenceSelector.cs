using UnityEngine;

namespace Assets.Scripts.PicketLiners
{
	public class SphereOfInfluenceSelector : MonoBehaviour
    {
        [SerializeField] private SphereOfInfluence BasicCollider;
        [SerializeField] private SphereOfInfluence AdvancedCollider;
        [SerializeField] private SphereOfInfluence EliteCollider;

        public void Initialize(PicketLiner picketLiner)
        {
            BasicCollider.Initialize(picketLiner);
            AdvancedCollider.Initialize(picketLiner);
            EliteCollider.Initialize(picketLiner);
        }

        public void SetRank(PicketLinerRank rank)
        {
            BasicCollider.gameObject.SetActive(rank == PicketLinerRank.Basic);
            AdvancedCollider.gameObject.SetActive(rank == PicketLinerRank.Advanced);
            EliteCollider.gameObject.SetActive(rank == PicketLinerRank.Elite);
        }

        public void SetCarriedSprite(bool isCarried)
        {
            BasicCollider.SetCarriedSprite(isCarried);
            AdvancedCollider.SetCarriedSprite(isCarried);
            EliteCollider.SetCarriedSprite(isCarried);
        }
    }
}
