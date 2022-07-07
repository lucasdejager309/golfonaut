using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Shots {
    public int startWithShots;
    public int lastShotAmount;
    [SerializeField]int amount;

    public delegate void UpdateShots();
    public UpdateShots updateShots;

    public int GetAmount() {
        return amount;
    }
    public void SetAmount(int amount) {
        this.amount = amount;
        updateShots();
    }

    public void AddAmount(int amount) {
        SetAmount(this.amount+amount);
    }
}

[System.Serializable]
public class Retries {
    public int maxAmount = 3;
    [SerializeField]int amount;
    public int shotsPerCharge = 4;
    int charge;

    public delegate void UpdateShots();
    public UpdateShots updateShots;
    
    public void RechargeAmount(int amount) {
        charge += amount;
        if (charge >= shotsPerCharge && this.amount < maxAmount) {
            this.amount++;
            charge -= shotsPerCharge;
        }
    }

    public int GetAmount() {
        return amount;
    }

    public int GetCharge() {
        return charge;
    }

    public void SetAmount(int amount) {
        this.amount = amount;
        updateShots();
    }

    public void SetCharge(int amount) {
        this.charge = amount;
        updateShots();
    }

    public void AddAmount(int amount) {
        SetAmount(this.amount+amount);
    }
}

public class GameManager : MonoBehaviour
{
    public Vector2 standardScreen = new Vector2(1080, 2400);
    public bool GENERATE;

    public int score;
    public int highScore = 123;

    public Shots shots;
    public Retries retries;

    public bool takeInput = true;

    PlayerScript player;
    Vector2 playerSpawnPos;

    Text shotsLeftText;
    GameObject[] scoreTextObjects;
    GameObject[] highScoreTextObjects;
    Text retriesText;
    float currentY;

    LevelGen levelGen;
    UIManager uiManager;

    void Start() {
        GameObject.FindGameObjectWithTag("RestartButton").GetComponent<Button>().onClick.AddListener(delegate {Restart();});
        GameObject.FindGameObjectWithTag("MenuButton").GetComponent<Button>().onClick.AddListener(delegate {Exit();});
        GameObject.FindGameObjectWithTag("RetryButton").GetComponent<Button>().onClick.AddListener(delegate {Retry();});

        player = GameObject.FindObjectOfType<PlayerScript>();
        playerSpawnPos = player.transform.position;

        player.playerMove += RechargeRetry;
        shots.updateShots += UpdateShots;
        retries.updateShots += UpdateShots;


        uiManager = GetComponent<UIManager>();
        levelGen = FindObjectOfType<LevelGen>();

        shotsLeftText = GameObject.FindGameObjectWithTag("UI_ShotsLeft").GetComponent<Text>();
        scoreTextObjects = GameObject.FindGameObjectsWithTag("UI_Score");
        highScoreTextObjects = GameObject.FindGameObjectsWithTag("UI_HighScore");

        retriesText = GameObject.FindGameObjectWithTag("UI_RetriesLeft").GetComponent<Text>();

        Invoke("StartGame", 0.01f);

        //Camera.main.rect = new Rect(0, 0, Screen.width/standardScreen.x, Screen.height/standardScreen.y);
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
        shots.SetAmount(shots.startWithShots);
        retries.SetCharge(0);
        retries.SetAmount(0);
        score = 0;
        PauseInput(0.1f);
    }

    public void RechargeRetry() {
        retries.RechargeAmount(1);
    }

    public void Retry() { 
        if (retries.GetAmount() <= 0) {
            //TEMPORARY
        } else {
            retries.AddAmount(-1);
            player.Retry();
            shots.SetAmount(shots.lastShotAmount);
        }
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

    public void UpdateShots() {
        if (shotsLeftText == null) shotsLeftText = GameObject.FindGameObjectWithTag("UI_ShotsLeft").GetComponent<Text>();
        if (retriesText == null ) retriesText = GameObject.FindGameObjectWithTag("UI_RetriesLeft").GetComponent<Text>();
        shotsLeftText.text = shots.GetAmount().ToString();
        retriesText.text = retries.GetAmount().ToString();
    }

    void CheckScore() {
        currentY = player.transform.position.y;
        if (currentY - playerSpawnPos.y > score) {
            score = (int)(currentY - playerSpawnPos.y);
        }

        if (score > highScore) {
            highScore = score;
        }

        foreach(GameObject obj in scoreTextObjects) {
            obj.GetComponent<Text>().text = score.ToString();
        }
        foreach (GameObject obj in highScoreTextObjects) {
            obj.GetComponent<Text>().text = highScore.ToString();
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
