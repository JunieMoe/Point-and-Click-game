using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(LoadNextScene), 14f);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
