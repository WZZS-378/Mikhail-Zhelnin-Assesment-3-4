using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{


    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            GameObject quitBttn = GameObject.FindWithTag("QuitButton");
            Button quitButton = quitBttn.GetComponent<Button>();

            quitButton.onClick.AddListener(LoadMainMenu);
        }
    }

    public void LoadMainMenu()
    {
        Destroy(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(0);
    }

    public void LoadFirstLevel()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(1);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject FirstBttn = GameObject.FindWithTag("Level1Button");
        Button firstButton = FirstBttn.GetComponent<Button>();
        firstButton.onClick.AddListener(() => LoadFirstLevel());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
