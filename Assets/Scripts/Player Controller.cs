using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody playerRb;
    private Animator playerAnim;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtparticle;
    public AudioClip jumpsSound;
    public AudioClip crashSound;
    private AudioSource playerAudio;
    public float jumpforce = 10.0f;
    public float gravityModifier;
    public bool isONGround = true;
    public bool gameOver = false;

    void Start()
    {
        playerRb = GetComponent <Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isONGround && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpforce,ForceMode.Impulse);
            isONGround = false;
            playerAnim.SetTrigger("Jump_trig");
            dirtparticle.Stop();
            playerAudio.PlayOneShot(jumpsSound, 1.0f);
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.CompareTag("Ground"))
        {
            isONGround = true;
            dirtparticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!"); 
            gameOver = true ;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtparticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);
        }
        
          
        
    }

}
