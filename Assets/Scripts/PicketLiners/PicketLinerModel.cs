using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class PicketLinerModel : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;
    private Sprite DefaultSprite;
    [SerializeField] private Sprite CarriedSprite;
    [SerializeField] private SpriteRenderer MergeSpriteRenderer;

    public void Initialize()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        DefaultSprite = SpriteRenderer.sprite;
        DisplayMergeSprite(false);
    }

    public void SetCarriedSprite(bool isCarried)
    {
        SpriteRenderer.sprite = isCarried ? CarriedSprite : DefaultSprite;
    }

    public void DisplayMergeSprite(bool isVisible)
	{
        MergeSpriteRenderer.gameObject.SetActive(isVisible);
	}
}

