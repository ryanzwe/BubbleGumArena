using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneController : MonoBehaviour
{

    Scene currentScene;
    //GameObject myGameController;

    void Start()
    {
        //myGameController = GameObject.FindGameObjectWithTag("GameController");
        currentScene = SceneManager.GetActiveScene();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentScene.buildIndex == 0)
            {
                Application.Quit();
            }
            else
            {
                SceneManager.LoadScene((currentScene.buildIndex - 1));
            }
            //if (currentScene.name == "GameScene" || currentScene.name == "HighScoreScene")
            //{
            //    SceneManager.LoadScene("TitleScene");
            //}
            //if (currentScene.name == "TitleScene")
            //{
            //    Application.Quit();
            //}
        }
    }


    public void ChangeLevel(string LevelName)
    {
        SceneManager.LoadScene(LevelName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
