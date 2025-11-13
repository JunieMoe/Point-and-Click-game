using UnityEngine.EventSystems;
using UnityEngine;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    public Movement player;
    private CanvasGroup canvasGroup;
    private Vector3 startPos;
    public Canvas canvas;

    public float interactionRadius;

    public float snapRadius;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        player = FindObjectOfType<Movement>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = rectTransform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.4f;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Vector2 screenPos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0));
        worldPos.z = 0;

        foreach (var node in ItemInteraction.AllNodes)
        {
            float dist = Vector3.Distance(worldPos, node.transform.position);
            if (dist <= interactionRadius && node.GetComponent<ItemInteraction>().reqItem == gameObject.GetComponent<InventoryItemUI>().itemData && player.currentNode.GetComponent<MoveNode>() == node.interactionNode)
            {
                node.Interact();
            }
        }

        rectTransform.position = startPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position += new Vector3(eventData.delta.x, eventData.delta.y, 0);

        InventoryItemUI itemUI = GetComponent<InventoryItemUI>();
        if (itemUI == null || itemUI.itemData == null) return;

        ItemData draggedItem = itemUI.itemData;

        ItemInteraction bestMatch = null;
        float closestDist = float.MaxValue;

        foreach (var node in FindObjectsOfType<ItemInteraction>())
        {
            Vector3 nodeScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, node.transform.position);
            float dist = Vector2.Distance(Input.mousePosition, nodeScreenPos);

            if (dist < snapRadius && dist < closestDist && player.currentNode.GetComponent<MoveNode>() == node.interactionNode)
            {
                bestMatch = node;
                closestDist = dist;
            }
        }

        // Snap and glow logic
        if (bestMatch != null)
        {
            Vector3 targetScreenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, bestMatch.transform.position);
            rectTransform.position = Vector3.Lerp(rectTransform.position, targetScreenPos, 0.75f); // smooth snap
            if (bestMatch.reqItem == draggedItem) canvasGroup.alpha = 0.9f;
        }
        else
        {
            rectTransform.position = Input.mousePosition;
            canvasGroup.alpha = 0.4f;
        }
    }

}
