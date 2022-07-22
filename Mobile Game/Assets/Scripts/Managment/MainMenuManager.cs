using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    void Start() {
        GameObject.FindGameObjectWithTag("StartButton").GetComponent<Button>().onClick.AddListener(delegate {StartGame();});
        GameObject.FindGameObjectWithTag("SettingsButton").GetComponent<Button>().onClick.AddListener(delegate {ShowSettings();});
        GameObject.FindGameObjectWithTag("MenuButton").GetComponent<Button>().onClick.AddListener(delegate {GetComponent<UIManager>().SetUIState("MAIN_MENU");});
        GetComponent<UIManager>().SetUIState("MAIN_MENU");

        FindObjectOfType<AudioManager>().PlaySound("music", true);
    }

    public void StartGame() {
        SceneManager.LoadScene(1);
    }

    public void ShowSettings() {
        GetComponent<UIManager>().SetUIState("SETTINGS_MENU");
    }
}
