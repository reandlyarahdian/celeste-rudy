using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pause;
    public static bool paused = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Resume();
            }
            else 
            {
                Paused();
            }
        }
    }

    void Paused()
    {
        Time.timeScale = 0f;
        pause.SetActive(true);
        paused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pause.SetActive(false);
        paused = false;
    }
    public void Menu()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene("Main Menu");
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Level1()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }
    public void Level2()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level2");
    }
    public void Level3()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level3");
    }
}
