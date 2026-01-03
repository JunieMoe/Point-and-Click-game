using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void LoadState()
    {
        if (gameObject.GetComponent<ButtonInteraction>().isOn)
        {

            LoadOn();
        }

    }

    public void LoadOn()
    {
        SceneManager.LoadScene("Credits");
        gameObject.SetActive(false);
    }

}
