using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image Image;
    private Button Button;
    private Vector3 OriginalSize;

    private const float HoverScaleFactor = 1.1f;
    private const float ClickScaleFactor = 1.25f;
    private const float ScaleDuration = 0.5f;

    protected void Awake()
    {
        Image = GetComponent<Image>();
        Button = GetComponent<Button>();
        OriginalSize = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Button.interactable == false)
            return;
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, HoverScaleFactor * OriginalSize, ScaleDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Button.interactable == false)
            return;
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, OriginalSize, ScaleDuration);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Button.interactable == false)
            return;
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, ClickScaleFactor * OriginalSize, ScaleDuration);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Button.interactable == false)
            return;
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, OriginalSize, ScaleDuration);
    }
}
