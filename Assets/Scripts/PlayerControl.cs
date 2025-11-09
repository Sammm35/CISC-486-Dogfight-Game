using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject cam;
    public GameObject motor;
    public livesController lives;
    [SerializeField] private AudioClip crashSound;
    public AudioSource audioSource; // audio source component is on child motor because Plane 1 already has an AudioSource
    Rigidbody rb;
    TrailRenderer trail;
    int motorSpeed = 16207; // degrees of rotation per second
    float speed = 25;
    float turnXSpeed = 100; // degrees or rotarion per seconds
    float turnZSpeed = 240;
    public int crashed = 0; // public so the shoot script can disable firing while crashed
    Vector3 startPos;
    Quaternion startRot;
    float respawnDelay = -2;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
        startRot = rb.transform.localRotation;
        startPos = rb.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // motor spinning animation
        if (crashed == 0)
        {
            motor.transform.Rotate(0f, 0f, motorSpeed * Time.deltaTime);
        }

        // Toggle camera view:
            // pressing 'S' changes to second person
            if (Input.GetKeyDown(KeyCode.S))
            {
                cam.transform.localPosition = new Vector3(0, 2.2f, 7.5f);
                cam.transform.localRotation = Quaternion.Euler(10, 180, 0);
            }
            // Releasing 'S' returns back to 3rd person
            if (Input.GetKeyUp(KeyCode.S))
            {
                cam.transform.localPosition = new Vector3(0, 2.2f, -7.5f);
                cam.transform.localRotation = Quaternion.Euler(10, 0, 0);
            }

        // MOVEMENT CONTROLS:
        // Plane constantly moves fowards (can tell direction based on x,y rotation)
        // While "SHIFT" is being held down: Micro aim adjustments moving x,y rotation at 1/6 turn speed
        // Without "SHIFT": Player can turn much faster but can only change x,z rotation
        if (crashed == 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
                {
                    transform.Rotate(-turnXSpeed / 6 * Time.deltaTime, 0, 0);
                }
                else if (!Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow))
                {
                    transform.Rotate(turnXSpeed / 6 * Time.deltaTime, 0, 0);
                }

                if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
                {
                    transform.Rotate(0, turnZSpeed / 6 * Time.deltaTime, 0);
                }
                else if (!Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
                {
                    transform.Rotate(0, -turnZSpeed / 6 * Time.deltaTime, 0);
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
                {
                    transform.Rotate(-turnXSpeed * Time.deltaTime, 0, 0);
                }
                else if (!Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow))
                {
                    transform.Rotate(turnXSpeed * Time.deltaTime, 0, 0);
                }

                if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
                {
                    transform.Rotate(0, 0, -turnZSpeed * Time.deltaTime);
                }
                else if (!Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
                {
                    transform.Rotate(0, 0, turnZSpeed * Time.deltaTime);
                }
            }

            if (!Input.GetKey(KeyCode.C))   // always move foward unless C is press down (for testing purposes)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }

        // Bit of logic to ensure respawning works properly
        rb.isKinematic = false;
        respawnDelay -= Time.deltaTime;
        if (respawnDelay < 0 && respawnDelay > -1)
        {
            respawnDelay = -2;
            Respawn();
        }
    }
    private void OnCollisionEnter(Collision collision)
        // Crash on collision with terrain or enemy:
        // Movement is turned off and gravity will be enabled
        // Respawn at starting pos after 3 seconds
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain") || collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            crashed++;
            if (crashed == 1)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                trail.emitting = false; // disables the trail after a collision
                lives.crash(0);
                respawnDelay = 3;
                audioSource.Pause(); // pauses the motor sound effect
            }
            rb.AddExplosionForce(1000f, transform.position, 5f);    // minor ragdoll effect
            AudioSource.PlayClipAtPoint(crashSound, transform.position, 0.5f);
        }
    }

    void Respawn()
    {
        rb.isKinematic = true;  // turns on isKinematic for 1 frame to fix a bug with movement
        rb.useGravity = false;
        trail.emitting = true;
        transform.position = startPos;
        transform.rotation = startRot;
        crashed = 0;
        audioSource.UnPause(); // unpauses the motor sound effect
    }
}
