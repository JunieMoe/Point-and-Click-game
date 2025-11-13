using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneExitNode : MonoBehaviour
{
    public string entranceNodeName;
    [SerializeField] private string sceneToLoad;
    public GameManager gameManager;


    [SerializeField] private Movement player;
    // Start is called before the first frame update



    void Start()
    {
        gameManager = GameManager.Instance;
        player = GameObject.Find("Player").GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.currentNode == gameObject)
        {
            gameManager.SaveGame();
            gameManager.entranceNodeName = entranceNodeName;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
