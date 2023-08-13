using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.General
{
	public class HealthBarController : MonoBehaviour
    {
        private const float TimeBetweenLightsBeingTurnedOff = 1.0f;

        private HealthBarItem[] Items;

        private void Awake()
        {
            Items = GetComponentsInChildren<HealthBarItem>();
        }

        public bool AreAllLivesLost()
        {
            return Items.Any(i => i.IsWorking == false) == false;
        }

        public void OnScabEntered()
        {
            if (Items.Any(i => i.IsWorking == false) == false)
                return;
            HealthBarItem healthBarItem;
            do
            {
                healthBarItem = Items[Random.Range(0, Items.Length)];
            } while (healthBarItem.IsWorking == true);
            healthBarItem.DisplayWindowWorking(true);
        }

        public IEnumerator DisplayLifeGainedCoroutine()
        {
            List<HealthBarItem> workingItems = Items.Where(i => i.IsWorking == true).ToList();
            if (workingItems.Any() == false)
                throw new UnityException("No available items found for gaining a life");
            foreach(HealthBarItem healthBarItem in workingItems)
            {
                healthBarItem.DisplayWindowWorking(false);
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/light_off");
                yield return new WaitForSeconds(TimeBetweenLightsBeingTurnedOff);
            }
        }
    }
}
