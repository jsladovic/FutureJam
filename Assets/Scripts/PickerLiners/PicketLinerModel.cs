using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class PicketLinerModel : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;
    private Sprite DefaultSprite;
    [SerializeField] private Sprite CarriedSprite;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        DefaultSprite = SpriteRenderer.sprite;
    }

    public void SetCarriedSprite(bool isCarried)
    {
        SpriteRenderer.sprite = isCarried ? CarriedSprite : DefaultSprite;
    }
}

