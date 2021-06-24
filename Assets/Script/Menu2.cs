using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu2 : MonoBehaviour
{
    public void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }
    public void Quit()
    {
        Application.Quit();
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
    public void Menu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
