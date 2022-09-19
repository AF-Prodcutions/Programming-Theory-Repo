using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefab;
    public GameObject powerupPrefab;
    private Vector3 spawnPosition = new Vector3 (25,0,0);
    private Vector3 powerupSpawnPosition = new Vector3(13, 4, 0);
    private float startDelay = 2;
    private float repeatRate = 2;
    private PlayerController playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
        InvokeRepeating("SpawnPowerup", startDelay * 2, repeatRate*2);
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();   
    }
    void SpawnObstacle()
    {
        if (playerControllerScript.gameOver == false)
        {
            int obstacleIndex = Random.Range(0, obstaclePrefab.Length);
            Instantiate(obstaclePrefab[obstacleIndex], spawnPosition, obstaclePrefab[obstacleIndex].transform.rotation);
        }
    }
    void SpawnPowerup()
    {
        if (playerControllerScript.gameOver == false)
        {
            Instantiate(powerupPrefab, powerupSpawnPosition, powerupPrefab.transform.rotation);
        }
    }
    
}
