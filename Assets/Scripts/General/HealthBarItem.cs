using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HealthBarItem : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;
    [SerializeField] private Sprite WindowWorkingSprite;
    private Sprite WindowNotWorkingSprite;

    public bool IsWorking { get; private set; }

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        WindowNotWorkingSprite = SpriteRenderer.sprite;
        DisplayWindowWorking(false);
    }

    public void DisplayWindowWorking(bool isWorking)
    {
        IsWorking = isWorking;
        StartCoroutine(DisplayWindowWorkingCoroutine(isWorking));
    }

    private IEnumerator DisplayWindowWorkingCoroutine(bool isWorking)
    {
        if (isWorking == true)
            yield return new WaitForSeconds(isWorking ? 5.0f : 0.0f);
        SpriteRenderer.sprite = isWorking ? WindowWorkingSprite : WindowNotWorkingSprite;
    }
}
