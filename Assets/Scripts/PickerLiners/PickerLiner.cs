using System;
using UnityEngine;

[RequireComponent(typeof(DragController))]
public class PickerLiner : MonoBehaviour
{
    private ModelSelector ModelSelector;
    private SphereOfInfluenceSelector SphereOfInfluenceSelector;

    private PicketLinerRank rank;
    private PicketLinerRank Rank
    {
        get { return rank; }
        set
        {
            rank = value;
            ModelSelector.SetRank(value);
            SphereOfInfluenceSelector.SetRank(value);
        }
    }

    private void Awake()
    {
        ModelSelector = GetComponentInChildren<ModelSelector>();
        SphereOfInfluenceSelector = GetComponentInChildren<SphereOfInfluenceSelector>();

        Rank = PicketLinerRank.Basic;
    }
}
