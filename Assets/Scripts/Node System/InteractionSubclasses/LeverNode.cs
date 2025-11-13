using System;
using UnityEngine;

public class LeverNode : TapInteraction
{

    public bool isOn = false;

    void Start()
    {
        string saved = GameManager.Instance.GetInteractionState("SampleScene_" + gameObject.name, "Off");
        isOn = saved == "On";
    }

    public override void Interact()
    {

        isOn = !isOn;
        
        if (!isOn)
        {
            GetComponent<InteractableState>().SetState("Off");
        }
        else
        {
            GetComponent<InteractableState>().SetState("On");
        }

        base.Interact();
    }

}
