using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Button newGameButton;
    public Button loadGameButton;
    public Button quitButton;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainMenu")
            return;

        RebindButtons();
    }

    void RebindButtons()
    {
        GameManager gm = FindObjectOfType<GameManager>();

        newGameButton.onClick.RemoveAllListeners();
        loadGameButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();

        newGameButton.onClick.AddListener(gm.NewGame);
        loadGameButton.onClick.AddListener(gm.LoadGame);
        quitButton.onClick.AddListener(gm.QuitGame);
    }
}
