using System;
using UnityEngine;

public class ButtonInteraction : TapInteraction
{

    public bool isOn = false;

    void Start()
    {
        string saved = GameManager.Instance.GetInteractionState("SampleScene_" + gameObject.name, "Off");
        isOn = saved == "On";
    }

    public override void Interact()
    {

        if (!isOn) isOn = true;

        GetComponent<InteractableState>().SetState("On");

        base.Interact();
    }

}
