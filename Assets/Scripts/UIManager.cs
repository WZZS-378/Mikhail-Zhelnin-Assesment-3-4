using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public void LoadFirstLevel()
    {
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
