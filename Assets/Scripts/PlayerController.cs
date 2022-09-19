using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    [SerializeField] private int jumpForce = 5;
    [SerializeField] private float gravityModifier;
    public bool isOnGround = true;
    public bool gameOver;
    private Animator playerAnim;
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private ParticleSystem powerParticle;
    [SerializeField] private ParticleSystem dirtParticle;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip crashSound;
    private AudioSource playerAudio;
    public bool hasPowerup = false;
    [SerializeField] private GameObject powerUpIndicator;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }

        powerUpIndicator.transform.position = transform.position + new Vector3(0, 0.5f, 0);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtParticle.Play();
        } 
        else if (collision.gameObject.CompareTag("Obstacle") && hasPowerup)
        {
            powerParticle.Play();
            playerAudio.PlayOneShot(crashSound, 1.0f);
            collision.gameObject.SetActive(false);

        } else if (collision.gameObject.CompareTag("Obstacle") && !hasPowerup)
        {
            gameOver = true;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);
            powerUpIndicator.SetActive(false);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            powerUpIndicator.SetActive(true);
            StartCoroutine(PowerCountdownRoutine());
        }
    }
    IEnumerator PowerCountdownRoutine()
    {
        yield return new WaitForSeconds(3);
        hasPowerup = false;
        powerUpIndicator.SetActive(false);
    }
}
