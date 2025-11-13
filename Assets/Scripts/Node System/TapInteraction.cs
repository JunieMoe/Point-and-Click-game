using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TapInteraction : MonoBehaviour
{
    public UnityEvent onInteract;
    public MoveNode interactionNode;
    public virtual void Interact()
    {
        onInteract?.Invoke();
    }
}
