using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemInteractTest : MonoBehaviour
{
    public GameObject test;
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
    }
    public void LoadOn()
    {
        test.SetActive(true);
    }

}
