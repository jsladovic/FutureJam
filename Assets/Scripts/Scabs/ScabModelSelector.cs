using UnityEngine;

namespace Assets.Scripts.Scabs
{
	public class ScabModelSelector : MonoBehaviour
    {
        [SerializeField] private ScabModel BasicModelEntering;
        [SerializeField] private ScabModel DesperateModelEntering;
        [SerializeField] private ScabModel EliteModelEntering;
        [SerializeField] private ScabModel BasicModelLeaving;
        [SerializeField] private ScabModel DesperateModelLeaving;
        [SerializeField] private ScabModel EliteModelLeaving;

        public void SetSprite(ScabRank rank, bool isLeaving)
        {
            BasicModelEntering.gameObject.SetActive(rank == ScabRank.Basic && isLeaving == false);
            BasicModelLeaving.gameObject.SetActive(rank == ScabRank.Basic && isLeaving == true);
            DesperateModelEntering.gameObject.SetActive(rank == ScabRank.Desperate && isLeaving == false);
            DesperateModelLeaving.gameObject.SetActive(rank == ScabRank.Desperate && isLeaving == true);
            EliteModelEntering.gameObject.SetActive(rank == ScabRank.Elite && isLeaving == false);
            EliteModelLeaving.gameObject.SetActive(rank == ScabRank.Elite && isLeaving == true);
        }
    }
}
