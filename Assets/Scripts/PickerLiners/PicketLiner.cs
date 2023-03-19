using System;
using UnityEngine;

[RequireComponent(typeof(DragController))]
public class PicketLiner : MonoBehaviour
{
    private DragController DragController;
    private ModelSelector ModelSelector;
    private SphereOfInfluenceSelector SphereOfInfluenceSelector;

    private PicketLinerRank rank;
    public PicketLinerRank Rank
    {
        get { return rank; }
        private set
        {
            rank = value;
            ModelSelector.SetRank(value);
            SphereOfInfluenceSelector.SetRank(value);
        }
    }

    private bool IsClicked;

    public void Initialize()
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
        if (UpgradeRank() == true)
        {
            GameController.Instance.StartLevel();
            IsClicked = false;
            CanvasController.Instance.HideTutorialText();
        }
    }

    private void OnMouseExit()
    {
        if (IsClicked == true)
            return;
        IsClicked = false;
    }

    private bool UpgradeRank()
    {
        if (Rank == PicketLinerRank.Basic)
        {
            Rank = PicketLinerRank.Advanced;
            return true;
        }
        return false;
    }

    public bool CanBeUpgraded => Rank == PicketLinerRank.Basic;

    public bool IsCarried => DragController.IsDragging;
}
