using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject cam;
    public GameObject motor;
    int motorSpeed = 16207; // degrees of rotation per second
    float speed = 25;
    float turnXSpeed = 100; // degrees or rotarion per seconds
    float turnZSpeed = 240;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // motor spinning animation
        motor.transform.Rotate(0f, 0f, motorSpeed * Time.deltaTime);

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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
            {
                transform.Rotate(-turnXSpeed/6 * Time.deltaTime, 0, 0);
            }
            else if (!Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow))
            {
                transform.Rotate(turnXSpeed/6 * Time.deltaTime, 0, 0);
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
            transform.Translate(Vector3.forward*speed*Time.deltaTime);
        }
    }
}
