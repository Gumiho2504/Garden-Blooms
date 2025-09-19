using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Item draggable = eventData.pointerDrag.GetComponent<Item>();
        if (draggable != null)
        {
            // Mark that the item was dropped on a valid zone
            draggable.isDroppedOnTarget = true;

            // Optionally: Set this object as new parent
            draggable.transform.SetParent(transform);
            draggable.transform.localPosition = Vector3.zero; // Center it in the drop zone
        }
    }

   
}
