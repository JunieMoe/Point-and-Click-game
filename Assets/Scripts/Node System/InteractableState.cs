using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class InteractionEvent
{
    public string stateName;
    public UnityEvent onEnterState;
}

public class InteractableState : MonoBehaviour
{
    public string uniqueID;
    public string defaultState = "default";

    [Header("State Events")]
    public List<InteractionEvent> stateEvents = new List<InteractionEvent>();

    private void Awake()
    {
        if (string.IsNullOrEmpty(uniqueID))
            uniqueID = $"{SceneManager.GetActiveScene().name}_{gameObject.name}";

        string savedState = GameManager.Instance.GetInteractionState(uniqueID, defaultState);
        ApplyState(savedState);
    }

    public void SetState(string newState)
    {
        GameManager.Instance.SetInteractionState(uniqueID, newState);
        ApplyState(newState);
    }

    public string GetCurrentState()
    {
        return GameManager.Instance.GetInteractionState(uniqueID, defaultState);
    }

    private void ApplyState(string state)
    {
        foreach (var evt in stateEvents)
        {
            if (evt.stateName == state)
            {
                evt.onEnterState?.Invoke();
                break;
            }
        }
    }
}
