using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOveLeft : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed = 30.0f;
    public float leftBound = -15.0f ; 
    private PlayerController playerControllerScript;

    void Start()
    {
        playerControllerScript= GameObject.Find("Player").GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.gameOver == false)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);

        }
        if (transform.position.x <leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
