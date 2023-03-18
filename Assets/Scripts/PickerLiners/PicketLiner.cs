using System;
using UnityEngine;

[RequireComponent(typeof(DragController))]
public class PicketLiner : MonoBehaviour
{
    private DragController DragController;
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
        DragController = GetComponent<DragController>();
        ModelSelector = GetComponentInChildren<ModelSelector>();
        SphereOfInfluenceSelector = GetComponentInChildren<SphereOfInfluenceSelector>();

        Rank = PicketLinerRank.Basic;
        SphereOfInfluenceSelector.Initialize(this);
        DragController.Initialize(this);
    }

    public void OnIsDraggedChanged()
    {
        ModelSelector.SetCarriedSprite(IsCarried);
        SphereOfInfluenceSelector.SetCarriedSprite(IsCarried);
    }

    public bool IsCarried => DragController.IsDragging;
}
