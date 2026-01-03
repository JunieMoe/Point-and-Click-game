using UnityEngine;

public class ConveyorButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public ConveyorSpawner widgetSpawner;
    public void LoadState()
    {
        if (gameObject.GetComponent<ButtonInteraction>().isOn)
        {

            LoadOn();
        }

    }

    public void LoadOn()
    {
        widgetSpawner.SpawnWidget();
        gameObject.SetActive(false);
    }

}

