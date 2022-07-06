using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelGen : MonoBehaviour
{
    [Header("Parameters")]
    public float GEN_DISTANCE = 10;
    public bool VARIATE;

    [Header("Prefabs")]
    public GameObject wallsPrefab;
    public GameObject[] obstaclePrefabs;

    public Transform levelEnd;
    Vector2 levelEndStartPos;
    public Transform obstaclesEnd;
    Vector2 obstaclesEndStartPos;
    Transform player;
    

    List<GameObject> spawnedObstacles = new List<GameObject>();

    void Start() {
        levelEnd = GameObject.FindGameObjectWithTag("LevelEnd").transform;
        levelEndStartPos = levelEnd.position;
        player = FindObjectOfType<PlayerScript>().transform;
        obstaclesEnd = GameObject.FindGameObjectWithTag("ObstaclesEnd").transform;
        obstaclesEndStartPos = obstaclesEnd.position;
        
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
        //GENERATE SIDEWALLS
        Instantiate(wallsPrefab, levelEnd.position, Quaternion.identity);
        

        //GENERATE OBSTACLES
        while (obstaclesEnd.position.y < levelEnd.position.y+GEN_DISTANCE) {
            GameObject obstacle = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            float size = GameManager.Instance.FindChildWithTag(obstacle, "ObstacleEnd").transform.position.y;
            GameObject generatedObstacle = Instantiate(obstacle, obstaclesEnd.position, Quaternion.identity);
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
    }
}
