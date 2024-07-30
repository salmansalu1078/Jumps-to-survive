using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;
    public float floatForce = 50.0f;
    private float gravityModifier = 1.5f;
    public bool groundcheck = false;
    public float force = 5.0f;

    private Rigidbody playerRb;
    public AudioClip jumpsSound;
    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.UpArrow) && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }

        // Check if the player's y position is greater than or equal to 15.6f
        if (transform.position.y >= 15.6f)
        {
            // Transform the player's position to 15.0f on the y-axis
            transform.position = new Vector3(transform.position.x, 13.3f, transform.position.z);
        }

        // Check if the player's y position is less than or equal to 0
        if (transform.position.y <= 0 && !groundcheck)
        {
            // Apply a small upward force to make the player jump
            playerRb.AddForce(Vector3.up * force, ForceMode.Impulse);
            playerAudio.PlayOneShot(jumpsSound, 1.0f);

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            groundcheck = true;
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);


        }
        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
        }
    }
}
