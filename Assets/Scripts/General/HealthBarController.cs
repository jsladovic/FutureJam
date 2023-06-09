﻿using System.Linq;
using UnityEngine;

namespace Assets.Scripts.General
{
	public class HealthBarController : MonoBehaviour
    {
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

        public void DisplayLifeGained()
        {
            if (Items.Any(i => i.IsWorking == true) == false)
                throw new UnityException("No available items found for gaining a life");
            HealthBarItem healthBarItem;
            do
            {
                healthBarItem = Items[Random.Range(0, Items.Length)];
            } while (healthBarItem.IsWorking == false);
            healthBarItem.DisplayWindowWorking(false);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/light_off");
        }
    }
}
