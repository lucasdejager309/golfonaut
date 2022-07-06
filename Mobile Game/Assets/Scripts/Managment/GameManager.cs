using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public bool GENERATE;

    public int score;
    public int startWithShots = 3;
    [SerializeField] int shots;
    public bool takeInput = true;

    PlayerScript player;
    Vector2 playerSpawnPos;

    Text shotsLeftText;
    Text scoreText;
    float currentY;

    LevelGen levelGen;
    UIManager uiManager;

    void Start() {
        GameObject.FindGameObjectWithTag("RestartButton").GetComponent<Button>().onClick.AddListener(delegate {Restart();});
        GameObject.FindGameObjectWithTag("MenuButton").GetComponent<Button>().onClick.AddListener(delegate {Exit();});

        player = GameObject.FindObjectOfType<PlayerScript>();
        playerSpawnPos = player.transform.position;

        uiManager = GetComponent<UIManager>();
        levelGen = FindObjectOfType<LevelGen>();

        shotsLeftText = GameObject.FindGameObjectWithTag("UI_ShotsLeft").GetComponent<Text>();
        scoreText = GameObject.FindGameObjectWithTag("UI_Score").GetComponent<Text>();

        Invoke("StartGame", 0.01f);
    }

    void Update() {
        GetComponent<InputManager>().GetInput();
        CheckScore();

        if (GENERATE) {
            if ((levelGen.levelEnd.position.y - player.transform.position.y) < levelGen.GEN_DISTANCE) {
                levelGen.GenerateNewSection();
                levelGen.levelEnd.position = new Vector2(levelGen.levelEnd.position.x, levelGen.levelEnd.position.y+levelGen.GEN_DISTANCE); 
            }
        }
    }

    void StartGame() {
        uiManager.SetUIState("IN_GAME");
        player.transform.position = playerSpawnPos;
        SetShots(startWithShots);
        score = 0;
        PauseInput(0.1f);
    }

    public void Restart() {
        levelGen.Restart();
        StartGame();
    }
    
    void PauseInput(float time) {
        takeInput = false;
        Invoke("CanShootAgain", time);
    }

    void CanShootAgain() {
        takeInput = true;
    }

    public void Exit() {
        SceneManager.LoadScene(0);
    }

    public void AddShot(int amount) {
        SetShots(GetShots()+amount);
        UpdateShots();
    }
    public void SetShots(int amount) {
        shots = amount;
        UpdateShots();
    }

    public int GetShots() {
        return shots;
    }

    public void UpdateShots() {
        if (shotsLeftText == null) shotsLeftText = GameObject.FindGameObjectWithTag("UI_ShotsLeft").GetComponent<Text>();
        shotsLeftText.text = GetShots().ToString();
    }

    void CheckScore() {
        currentY = player.transform.position.y;
        if (currentY - playerSpawnPos.y > score) {
            score = (int)(currentY - playerSpawnPos.y);
        }
        scoreText.text = score.ToString();
    }

    public GameObject FindChildWithTag(GameObject parent, string tag) {
        foreach (Transform child in parent.transform) {
            if (child.CompareTag(tag)) {
                return child.gameObject;
            }
        }
        
        return null;
    }

    public void Null() {}
}
