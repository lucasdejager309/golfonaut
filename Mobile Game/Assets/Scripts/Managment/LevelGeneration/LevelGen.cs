using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DifficultyGroup {
    public GameObject[] prefabs;

    public GameObject GetObstacle() {
        Random.InitState((int)System.DateTime.Now.Ticks);
        return prefabs[Random.Range(0, prefabs.Length-1)];
    }
}

public class LevelGen : MonoBehaviour
{
    [Header("Parameters")]
    public float GEN_DISTANCE = 10;
    public float DIFFICULTY_SCALE = 50;
    public bool VARIATE;

    [Header("Prefabs")]
    public GameObject wallsPrefab;
    public DifficultyGroup[] difficultyGroups;

    public Transform levelEnd;
    Vector2 levelEndStartPos;
    public Transform obstaclesEnd;
    Vector2 obstaclesEndStartPos;
    Transform player;
    GameManager gameManager;
    

    List<GameObject> spawnedObstacles = new List<GameObject>();

    void Start() {
        levelEnd = GameObject.FindGameObjectWithTag("LevelEnd").transform;
        levelEndStartPos = levelEnd.position;
        player = FindObjectOfType<PlayerScript>().transform;
        obstaclesEnd = GameObject.FindGameObjectWithTag("ObstaclesEnd").transform;
        obstaclesEndStartPos = obstaclesEnd.position;

        gameManager = FindObjectOfType<GameManager>();
    }

    public void Restart() {
        foreach (GameObject obj in spawnedObstacles) {
            Destroy(obj);
        }
        spawnedObstacles.Clear();
        obstaclesEnd.position = obstaclesEndStartPos;
        levelEnd.position = levelEndStartPos;
    }

    public void GenerateNewSection() {
        //GENERATE OBSTACLES
        while (obstaclesEnd.position.y < levelEnd.position.y+GEN_DISTANCE) {
            GameObject obstacle = GetDifficulty(gameManager.score).GetObstacle();
            float size = FindObjectOfType<GameManager>().FindChildWithTag(obstacle, "ObstacleEnd").transform.position.y;
            GameObject generatedObstacle = Instantiate(obstacle, obstaclesEnd.position, Quaternion.identity);
            generatedObstacle.transform.position = new Vector3(generatedObstacle.transform.position.x, generatedObstacle.transform.position.y, generatedObstacle.transform.position.z+2);
            spawnedObstacles.Add(generatedObstacle);
            
            if (VARIATE) generatedObstacle.GetComponent<PlacementVariation>().Variate();
            
            RightSideUp[] fixRot = FindObjectsOfType<RightSideUp>();
            if (fixRot.Length > 0) {
                foreach (RightSideUp rs in fixRot) {
                    rs.Fix();
                }
            }
            
            obstaclesEnd.position += new Vector3(0, size, 0);
        }

        //GENERATE SIDEWALLS
        GameObject walls = Instantiate(wallsPrefab, levelEnd.position, Quaternion.identity);
        spawnedObstacles.Add(walls);
    }

    DifficultyGroup GetDifficulty(int currentScore) {
        int difficulty = Mathf.CeilToInt((currentScore/DIFFICULTY_SCALE)*difficultyGroups.Length);
        Debug.Log(difficulty);

        int curve = Random.Range(0, difficultyGroups.Length) + Random.Range(0,difficultyGroups.Length) - (difficultyGroups.Length)-1;
        int result = Mathf.Clamp(difficulty + curve, 0, difficultyGroups.Length-1);
        //Debug.Log(result);

        return difficultyGroups[result];
    }
}
