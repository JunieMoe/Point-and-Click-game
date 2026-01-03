using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LeverInteractTest : MonoBehaviour
{
    public GameObject test;
    public void Test()
    {
        if (gameObject.GetComponent<LeverNode>().isOn)
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
