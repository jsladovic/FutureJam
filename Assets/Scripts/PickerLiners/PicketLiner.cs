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

    private bool IsClicked;

    private void Awake()
    {
        DragController = GetComponent<DragController>();
        ModelSelector = GetComponentInChildren<ModelSelector>();
        SphereOfInfluenceSelector = GetComponentInChildren<SphereOfInfluenceSelector>();

        SphereOfInfluenceSelector.Initialize(this);
        ModelSelector.Initialize();
        Rank = PicketLinerRank.Basic;
        DragController.Initialize(this);
    }

    public void OnIsDraggedChanged()
    {
        ModelSelector.SetCarriedSprite(IsCarried);
        SphereOfInfluenceSelector.SetCarriedSprite(IsCarried);
    }

    private void OnMouseDown()
    {
        if (GameController.Instance.IsWaitingForUpgrade == false)
            return;
        IsClicked = true;
    }

    private void OnMouseUp()
    {
        if (GameController.Instance.IsWaitingForUpgrade == false)
            return;
        UpgradeRank();
        GameController.Instance.StartLevel();
        IsClicked = false;
    }

    private void OnMouseExit()
    {
        if (IsClicked == true)
            return;
        IsClicked = false;
    }

    private void UpgradeRank()
    {
        if (Rank == PicketLinerRank.Basic)
            Rank = PicketLinerRank.Advanced;
        else if (Rank == PicketLinerRank.Advanced)
            Rank = PicketLinerRank.Elite;
    }

    public bool IsCarried => DragController.IsDragging;
}
