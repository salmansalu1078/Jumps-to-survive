using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ObstaclePrebs; 
    private Vector3 spawnpos = new Vector3(25, 0 , 0 );
    private float StartDelay = 2; 
    private float repeatRate = 2;
    private PlayerController playerControllerScript;

    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();

        InvokeRepeating("SpawnObstacle" , StartDelay , repeatRate);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnObstacle()
    {
        if (playerControllerScript.gameOver == false)
            Instantiate(ObstaclePrebs, spawnpos, ObstaclePrebs.transform.rotation);

    }
}
