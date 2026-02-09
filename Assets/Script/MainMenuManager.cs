using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionPanel;
    public GameObject loadGamePanel;

    [Header("Option UI")]
    public Slider volumeSlider;
    public Dropdown resolutionDropdown;

    public void OnClickNewGame() => SceneManager.LoadScene("CharacterCreation");
    public void OnClickLoadGame() => loadGamePanel.SetActive(true);
    public void OnClickOptions() => optionPanel.SetActive(true);
    public void OnClickExit() => Application.Quit();
    public void BackToMain()
    {
        optionPanel.SetActive(false);
        loadGamePanel.SetActive(false);
    }

    public void SetVolume(float volume) => AudioListener.volume = volume;
    public void SetResolution(int index)
    {
        Resolution res = Screen.resolutions[index];
        Screen.SetResolution(res.width, res.height, FullScreenMode.Windowed);
    }
}