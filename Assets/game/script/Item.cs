using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform rectTransform;
    public CanvasGroup canvasGroup;
    public Canvas canvas;

    public Vector3 originalPosition;
    public Transform originalParent;


    public bool isDroppedOnTarget = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        gameObject.transform.SetParent(canvas.transform);
        originalPosition = rectTransform.localPosition;
        originalParent = transform.parent;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        isDroppedOnTarget = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPointerPosition))
        {
            rectTransform.localPosition = localPointerPosition; //+ new Vector2(0, 400);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (!isDroppedOnTarget)
        {
            // Return to original position
            transform.SetParent(originalParent);
            rectTransform.localPosition = originalPosition;
        }
    }


}