using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    public AudioMixer audioMixer;
    Resolution[] resolutions;
    public TMP_Dropdown dropdown;
    public TMP_Dropdown quality;
    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        dropdown.ClearOptions();

        List<string> options = new List<string>();

        int currResIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currResIndex = i;
            }
        }
        dropdown.AddOptions(options);
        dropdown.value = currResIndex;
        dropdown.RefreshShownValue();
    }

    public void res(int ress)
    {
        Resolution resolution = resolutions[ress];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void Vol (float volume)
    {
        audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }
    public void qual (int index)
    {
        index = quality.value;
        QualitySettings.SetQualityLevel(index);
    }
    public void full(bool isFull)
    {
        Screen.fullScreen = isFull;
    }
    public void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
