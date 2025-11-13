using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractionTestNode : ItemInteraction
{

    public bool isDone = false;


    void Start()
    {
        string saved = GameManager.Instance.GetInteractionState("SampleScene_" + gameObject.name, "Incomplete");
        isDone = saved == "Complete";
    }

    public override void Interact()
    {

        isDone = !isDone;

        if (!isDone)
        {
            GetComponent<InteractableState>().SetState("Incomplete");
        }
        else
        {
            GetComponent<InteractableState>().SetState("Complete");
        }

        base.Interact();
    }

}