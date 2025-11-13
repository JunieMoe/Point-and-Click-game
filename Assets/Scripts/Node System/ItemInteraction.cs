using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class ItemInteraction : MonoBehaviour
{
    public static List<ItemInteraction> AllNodes = new List<ItemInteraction>();
    public UnityEvent onInteract;
    public MoveNode interactionNode;
    public ItemData reqItem;

    private void OnEnable()
    {
        AllNodes.Add(this);
    }

    private void OnDisable()
    {
        AllNodes.Remove(this);
    }
    public virtual void Interact()
    {
        onInteract?.Invoke();
    }
}
