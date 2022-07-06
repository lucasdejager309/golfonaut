using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    public bool GENERATE;

    public int score;
    public int startWithShots = 3;
    public int shots;

    Transform player;
    Vector2 playerSpawnPos;

    Text shotsLeft;
    Text scoreUI;

    float startY;
    float currentY;

    LevelGen levelGen;

    void Awake() {
        Instance = this;
    }

    void Start() {
        player = GameObject.FindObjectOfType<PlayerScript>().transform;
        playerSpawnPos = player.position;
        startY = player.transform.position.y;
        shotsLeft = GameObject.FindGameObjectWithTag("UI_ShotsLeft").GetComponent<Text>();
        scoreUI = GameObject.FindGameObjectWithTag("UI_Score").GetComponent<Text>();
        levelGen = FindObjectOfType<LevelGen>();
        shots = startWithShots;
        UpdateShots();
    }

    void Update() {
        CheckScore();

        if (GENERATE) {
            if ((levelGen.levelEnd.position.y - player.position.y) < levelGen.GEN_DISTANCE) {
                levelGen.GenerateNewSection();
                levelGen.levelEnd.position = new Vector2(levelGen.levelEnd.position.x, levelGen.levelEnd.position.y+levelGen.GEN_DISTANCE); 
            }
        }
    }

    void Restart() {
        levelGen.Restart();

        player.position = playerSpawnPos;
        score = 0;
        shots = startWithShots;
    }

    public void AddShot(int amount) {
        shots += amount;
        UpdateShots();
    }
    public void SetShots(int amount) {
        shots = amount;
        UpdateShots();
    }

    public void UpdateShots() {
        shotsLeft.text = shots.ToString();
    }

    void CheckScore() {
        currentY = player.transform.position.y;
        if (currentY - startY > score) {
            score = (int)(currentY - startY);
        }
        scoreUI.text = score.ToString();
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
