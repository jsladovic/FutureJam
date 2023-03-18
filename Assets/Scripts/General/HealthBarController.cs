using UnityEngine;
using System.Collections;
using System.Linq;

public class HealthBarController : MonoBehaviour
{
    private HealthBarItem[] Items;

    private void Awake()
    {
        Items = GetComponentsInChildren<HealthBarItem>();
    }

    public bool DisplayLifeLost()
    {
        if (Items.Any(i => i.IsWorking == false) == false)
            throw new UnityException("No available items found for losing a life");
        HealthBarItem healthBarItem;
        do
        {
            healthBarItem = Items[Random.Range(0, Items.Length)];
        } while (healthBarItem.IsWorking == true);
        healthBarItem.DisplayWindowWorking(true);
        var workingItems = Items.Where(i => i.IsWorking == false).ToList();
        return !Items.Any(i => i.IsWorking == false);
    }

    public void DisplayLifeGained()
    {
        if (Items.Any(i => i.IsWorking == true))
            throw new UnityException("No available items found for gaining a life");
        HealthBarItem healthBarItem;
        do
        {
            healthBarItem = Items[Random.Range(0, Items.Length)];
        } while (healthBarItem.IsWorking == false);
        healthBarItem.DisplayWindowWorking(false);
    }
}
