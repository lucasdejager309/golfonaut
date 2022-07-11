using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Vector2 standardScreen = new Vector2(1080, 2400);
    public bool GENERATE;

    public int score;
    public int highScore = 123;

    public int startWithShots;
    public int startWithRetries;
    public int maxRetries;
    public int chargePerRetry;

    public bool takeInput = true;

    PlayerScript player;
    Vector2 playerSpawnPos;
    
    float currentY;

    LevelGen levelGen;
    UIManager uiManager;

    void Start() {
        GameObject[] restartObjects = GameObject.FindGameObjectsWithTag("RestartButton");
        foreach (GameObject obj in restartObjects) obj.GetComponent<Button>().onClick.AddListener(delegate {Restart();});
        GameObject[] gotoMenuObjects = GameObject.FindGameObjectsWithTag("MenuButton");
        foreach (GameObject obj in gotoMenuObjects) obj.GetComponent<Button>().onClick.AddListener(delegate {Exit();});
        GameObject[] retryObjects = GameObject.FindGameObjectsWithTag("RetryButton");
        foreach (GameObject obj in retryObjects) obj.GetComponent<Button>().onClick.AddListener(delegate {RetryShot();});

        GameObject.FindGameObjectWithTag("PauseButton").GetComponent<Button>().onClick.AddListener(delegate {Pause();});
        GameObject.FindGameObjectWithTag("ResumeButton").GetComponent<Button>().onClick.AddListener(delegate {Resume();});

        player = GameObject.FindObjectOfType<PlayerScript>();
        player.gameManager = this;
        player.shots = new Shots(player, startWithShots, startWithRetries, maxRetries, chargePerRetry);

        player.shots.update += delegate {UpdateShotsUI(); };
        playerSpawnPos = player.transform.position;
        player.lastPos = playerSpawnPos;

        player.playerMove += RechargeRetry;

        uiManager = GetComponent<UIManager>();
        levelGen = FindObjectOfType<LevelGen>();
        Invoke("StartGame", 0.01f);

        Saving saving = GetComponent<Saving>();
        Save save = saving.GetSave();

        if (save != null) {
            highScore = save.highScore;
        }
        

        uiManager.UpdateUIElement("UI_HighScore", highScore.ToString());

        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 scale = new Vector2(Screen.width/standardScreen.x, Screen.height/standardScreen.y);
        Camera.main.rect = new Rect(0, 0, Screen.width*scale.x, Screen.height*scale.y);
    }

    void Update() {
        GetComponent<InputManager>().GetInput();
        CheckScore();
        UpdateShotsUI();

        if (GENERATE) {
            if ((levelGen.levelEnd.position.y - player.transform.position.y) < levelGen.GEN_DISTANCE) {
                levelGen.GenerateNewSection();
                levelGen.levelEnd.position = new Vector2(levelGen.levelEnd.position.x, levelGen.levelEnd.position.y+levelGen.GEN_DISTANCE); 
            }
        }
    }

    void StartGame() {
        takeInput = true;
        uiManager.SetUIState("IN_GAME");
        player.transform.position = playerSpawnPos;
        score = 0;
        
        PauseInput(0.1f);
    }

    public void Move(Vector2 input) {
        if (player.CanShoot()) {
            player.Move(input);
        }
    }

    public void RetryShot() {
        if (player.shots.currentRetries > 0 && (Vector2)transform.position != player.lastPos) {
            player.StopBall();
            player.GoToLastPos();
            player.shots.ResetToLast(true, true);
        }
    }

    public void EndOfShot() {
        player.StopBall();
        if (player.shots.currentShots > 0) {
            //nothing;
        } else {
            if (player.shots.currentRetries > 0) {
                player.shots.ResetToLast(true, false);
                player.shots.AddShots(1);
                player.GoToLastPos();
            } else if (player.shots.currentRetries <= 0 && player.shots.currentShots <= 0) {
                GameOver();
            }
        }
    }

    public void Die() {
        player.StopBall();
        player.GoToLastPos();

        if (player.shots.currentShots > 0) {
            player.shots.AddShots(-1);
        } else if (player.shots.currentRetries > 0) {
            player.shots.AddRetry(-1);
            player.shots.AddShots(1);
        } else {
            GameOver();
        }
    }

    public void GameOver() {
        FindObjectOfType<UIManager>().SetUIState("DEATH_MENU");
        takeInput = false;
    }

    public void Restart() {
        GetComponent<Saving>().Save(highScore);
        levelGen.Restart();
        player.shots.Restart();
        StartGame();
    }    

    public void RechargeRetry() {
        player.shots.Recharge(1);
    }

    public void Pause() {
        uiManager.SetUIState("PAUSE_MENU");
        takeInput = false;

        //pause stuff;
    }

    public void Resume() {
        uiManager.SetUIState("IN_GAME");
        PauseInput(0.1f);

        //resume stuff
    }
    
    void PauseInput(float time) {
        takeInput = false;
        Invoke("CanShootAgain", time);
    }

    void CanShootAgain() {
        takeInput = true;
    }

    public void Exit() {
        GetComponent<Saving>().Save(highScore);
        SceneManager.LoadScene(0);
    }

    public void UpdateShotsUI() {
        uiManager.UpdateUIElement("UI_Shots", player.shots.currentShots.ToString());
        uiManager.UpdateUIElement("UI_Retries", player.shots.currentRetries.ToString());
    }

    void CheckScore() {
        currentY = player.transform.position.y;
        if (currentY - playerSpawnPos.y > score) {
            score = (int)(currentY - playerSpawnPos.y);
            uiManager.UpdateUIElement("UI_Score", score.ToString());
        }

        if (score > highScore) {
            highScore = score;
            uiManager.UpdateUIElement("UI_HighScore", highScore.ToString());
        }
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
