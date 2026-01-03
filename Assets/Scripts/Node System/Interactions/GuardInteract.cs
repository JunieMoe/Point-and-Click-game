using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GuardInteract : MonoBehaviour
{
    public GameObject test;
    public GameObject workSpeech;
    public GameObject keySpeech;

    public void Test()
    {
        if (gameObject.GetComponent<ItemInteractionTestNode>().isDone)
        {
            LoadOn();
        }
        else
        {
            LoadOff();
        }
    }

    public void LoadOff()
    {
        test.SetActive(false);
        keySpeech.SetActive(false);
        workSpeech.SetActive(true);

    }
    public void LoadOn()
    {
        test.SetActive(true);
        keySpeech.SetActive(true);
        workSpeech.SetActive(false);
    }

}
