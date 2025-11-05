using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class enemyMovement : MonoBehaviour
{
    public GameObject motor;
    public Transform playerPos;
    Rigidbody rb;
    TrailRenderer trail;
    int motorSpeed = 16207; // degrees of rotation per second
    float speed = 25;
    float rotationSpeed = 60; // degrees per second
    int state = 1;  // starts on state 1
    float stateTimer = 0;
    float pitchVar;
    float random;

    float respawnDelay = -2;
    int crashed = 0;
    Vector3 startPos;
    Quaternion startRot;

    public float bulletSpeed;
    float fireRate = 0.4f;
    float fireTimer = 0;
    public Transform bulletSpawnTransform;
    public GameObject bulletPrefab;

    Vector3 middle = new Vector3 (0, 50, 0);

    float strafeTimer = 0;
    Vector3 strafeDestination;

    GameObject item;

    float playerAngle;
    Vector3 playerToEnemyR;

    float randX = 0;    // used for adding some randomness to enemy aim
    float randY = 0;
    float randTimer = 1.5f; // how often randomness is scrambled (seconds)
    float maxRand = 3; // maximum degrees the aim can be scewed by

    private void Start()
    {
        random = Random.value;
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
        startRot = rb.transform.localRotation;
        startPos = rb.transform.localPosition;
    }

    void Update()
    {
        if (crashed == 0)
        {
            motor.transform.Rotate(0f, 0f, motorSpeed * Time.deltaTime);    // spin motor
            transform.Translate(Vector3.forward * speed * Time.deltaTime);  // move fowards
        }

        // Bit of logic to ensure respawning works properly
        rb.isKinematic = false;
        respawnDelay -= Time.deltaTime;
        if (respawnDelay < 0 && respawnDelay > -1)
        {
            respawnDelay = -2;
            Respawn();
        }

        // STATE CONTROLLER:
        stateTimer += Time.deltaTime;
        if (state == 1) // attack state
        {
            Attack();
            if (Mathf.Abs(transform.position.x) > 275 || Mathf.Abs(transform.position.z) > 275 || transform.position.y > 200)
            {
                State(4);  // transitions to state 4 if too far away from the middle
            }
            else
            {
                pitchVar = transform.eulerAngles.x;
                if (pitchVar > 180) { pitchVar = 0; }
                pitchVar = Mathf.Abs(pitchVar - 90) / 5;
                if ((transform.position.y - pitchVar) < 5)
                {
                    State(4);  // transitions to state 4 off a balance of height and angle facing downwards (avoid hitting the ground)
                }
                else if (Vector3.Distance(playerPos.position, transform.position) < 25)
                {
                    State(2);  // transitions to state 2 if too close to the player
                }
                else
                {
                    item = ItemSearch();
                    if (item)
                    {
                        if (Vector3.Distance(transform.position, item.transform.position) < 300 && stateTimer > (4 + 5 * random))
                        {
                            State(3);  // transitions to state 3 if an item is close and hasn't changed states in a while
                        }
                    }
                    else if (stateTimer > 6 + 10 * random)
                    {
                        State(2);  // transitions to state 2 if attack phase has lasted too long
                    }
                }
            }

        }
        else if (state == 2) // strafe state
        {
            Strafe();
            item = ItemSearch();
            if (item)
            {
                if (Vector3.Distance(transform.position, item.transform.position) < 300 && stateTimer > (5 * random))
                {
                    State(3);  // transitions to state 3 if an item is close and hasn't changed states in a little while
                }
            }
            else if (stateTimer > (2 + 8*random))
            {
                State(1); // transitions to state 1 if it hasn't changed states in a while
            }
        }
        else if (state == 3) // item collect phase
        {
            if (!item)
            {
                State(1);  // transitions to state 1 if the item is gone
            }
            else if (stateTimer > 6)
            {
                playerToEnemyR = (transform.position - playerPos.position).normalized;
                playerAngle = Vector3.Angle(playerPos.forward, playerToEnemyR);
                if (playerAngle < 3)
                {
                    State(2);   // transition to state 2 if the player is aiming at this plane + 6 seconds of no state change
                }
            }
            else
            {
                GoToItem(item);
            }
        }
        else if (state == 4) // avoid border phase
        {
            AvoidBorder();
            if (stateTimer > 2 + 3*random)
            {
                State(1);   // Transition to state 1 once moved away from the border
            }
        }
    }

    void AimAt(Vector3 target)
        // Turns to plane to aim at target coordinates
    {
        if (crashed == 0)
        {
            // Setting random aim variables:
            randTimer -= Time.deltaTime;
            if (randTimer < 0)
            {
                randX = Random.value * maxRand * 2 - maxRand;
                randY = Random.value * maxRand * 2 - maxRand;
                randTimer = 1.5f;
            }

            Vector3 direction = (target - transform.position); // gets direction to target
            direction.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(direction); // gets needed rotation
            Quaternion randOffset = Quaternion.Euler(randX, randY, 0);
            Quaternion finalRotation = targetRotation * randOffset; // applies randomness to target rotation

            transform.rotation = Quaternion.RotateTowards(transform.rotation, finalRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void Shoot()    // spawns in a bullet and gives it momentum
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnTransform.position, bulletSpawnTransform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnTransform.forward * bulletSpeed, ForceMode.Impulse);
    }

    void Attack()
            // State 1: Aims at the player and fires at them
    {
        float playerDist = Vector3.Distance(transform.position, playerPos.position);    // gets distance to player

        // Aims at the player + a bit in front of them accounting for distance, bullet velocity and player speed:
        Vector3 aimTarget = playerPos.position + playerPos.forward * (playerDist * 25 / bulletSpeed);
        AimAt(aimTarget);

        float angle = Vector3.Angle(transform.forward, (aimTarget - transform.position).normalized);
        if (angle < 7)  // shoots at the player when within 7 degrees
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer < 0)
            {
                Shoot();
                fireTimer = fireRate;
            }
        }
    }

    void Strafe()
        //  State 2: Strafes away by flying a random direction
        //  New destination is generated every 5 seconds of the state.
    {
        strafeTimer -= Time.deltaTime;
        if (strafeTimer < 0)
        {
            strafeDestination = new Vector3((Random.value * 500) - 250, (Random.value * 90) + 10, (Random.value * 500) - 250);
            strafeTimer = 5;
        }
        AimAt(strafeDestination);
    }

    void GoToItem(GameObject itemObj)
        //  State 3: Moves towards an item
    {
        AimAt(itemObj.transform.position);
    }

    void AvoidBorder()
        // State 4: Flies towards the middle to get away from the map border
    {
        AimAt(middle);
    }

    GameObject ItemSearch()    // Standard function to find the closest object tagged "Item"
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Item");
        GameObject nearest = null;
        float minDistance = Mathf.Infinity;
        foreach (GameObject obj in objects)
        {
            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = obj;
            }
        }

        return nearest;
    }

    void State(int num) // Used to set certain values when transitioning between enemy states
    {
        state = num;
        stateTimer = 0;
        random = Random.value;
        if (num == 1 || num == 3)
        {
            speed = 25;
            rotationSpeed = 60;
        }
        else if (num == 2)
        {
            speed = 30;
            rotationSpeed = 240;
        }
        else
        {
            speed = 25;
            rotationSpeed = 90;
            num = 4; // Just in case an out of range number is passed
        }
        Debug.Log(num);
    }
    private void OnCollisionEnter(Collision collision)
    // Crash on collision with terrain or enemy:
    // Movement is turned off and gravity will be enabled
    // Respawn at starting pos after 3 seconds
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain") || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            crashed++;
            if (crashed == 1)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                trail.emitting = false; // disables the trail after a collision
                respawnDelay = 3;
            }
            rb.AddExplosionForce(1000f, transform.position, 5f);    // minor ragdoll effect
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
    }
}
